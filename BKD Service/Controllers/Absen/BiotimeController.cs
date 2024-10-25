using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using Hangfire;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;


namespace BKD_Service.Controllers.Absen
{
    [Route("api/[controller]")]
    [ApiController]
    public class BiotimeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _jwtApiUrl = "http://118.91.131.245:8080/jwt-api-token-auth/";
        ////private readonly string _apiUrl = "http://118.91.131.245:8080/iclock/api/transactions/";
        private readonly string _apiUrl = "http://118.91.131.245:8080/iclock/api/transactions/?page=1&page_size=100&emp_code=&start_time=02/01/2024&end_time=02/25/2024";
        private readonly string _username = "admin";
        private readonly string _password = "Siwalima01";
        private string _jwtToken = null;

        public IConfiguration Configuration { get; }
        string constrAbsen = Startup.ConnectionStringAbsen;

        public BiotimeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("trigger")]
        public IActionResult TriggerApiCall()
        {
            // Menjadwalkan tugas untuk mengeksekusi API dan menyimpan respons ke database setiap 10 detik
            RecurringJob.AddOrUpdate(() => FetchAndSaveData(), "*/5 * * * * *");
            //RecurringJob.AddOrUpdate<BiotimeController>(x => x.FetchAndSaveData(), Cron.MinuteInterval(1));

            return Ok("API call scheduled.");
        }

        [HttpGet("fetch")]
        public async Task<IActionResult> FetchAndSaveData()
        {
            try
            {
                DateTime today = DateTime.Today;
                DateTime tomorrow = DateTime.Today.AddDays(1);
                string formattedToday = today.ToString("MM/dd/yyyy");
                string formattedTomorrow = tomorrow.ToString("MM/dd/yyyy");

                // Mendapatkan atau memperbarui token JWT sebelum melakukan pemanggilan API
                await UpdateJwtToken();

                // Jika token berhasil diperbarui atau diperoleh
                if (!string.IsNullOrEmpty(_jwtToken))
                {
                    // Atur header Authorization dengan token JWT yang didapatkan
                    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("JWT", _jwtToken);

                    // Atur header Content-Type
                    _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    int pageSize = 10;
                    int page = 1; // Mulai dengan halaman pertama

                    do
                    {
                        // Membuat permintaan GET ke API dengan URL yang telah diformat
                        string apiUrl = $"http://118.91.131.245:8080/iclock/api/transactions/?page={page}&page_size={pageSize}&emp_code=&start_time={formattedToday}&end_time={formattedTomorrow}";
                        HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                        // Mengecek apakah respon berhasil (kode status 200-299)
                        response.EnsureSuccessStatusCode();

                        // Membaca konten respon sebagai string
                        string responseBody = await response.Content.ReadAsStringAsync();

                        // Konversi responseBody menjadi objek ResponseModel
                        var responseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseModel>(responseBody);

                        // Simpan respons ke dalam tabel database
                        await SaveResponseModelToDatabase(responseModel);

                        // Pemeriksaan halaman berikutnya
                        if (!string.IsNullOrEmpty(responseModel.next))
                        {
                            // Ekstrak nomor halaman berikutnya dari URL
                            page = int.Parse(responseModel.next.Substring(responseModel.next.IndexOf("page=") + 5, responseModel.next.IndexOf("&page_size=") - (responseModel.next.IndexOf("page=") + 5)));
                        }
                        else
                        {
                            // Tidak ada halaman berikutnya, keluar dari loop
                            break;
                        }
                    } while (true);

                    // Mengembalikan respons HTTP 200 OK
                    return Ok("Data fetched and saved successfully.");
                }
                else
                {
                    // Token JWT tidak berhasil diperbarui atau diperoleh
                    throw new Exception("Failed to get or update JWT token.");
                }
            }
            catch (Exception ex)
            {
                // Menangani kesalahan dan mengembalikan respons HTTP 500 Internal Server Error
                return StatusCode(500, $"Error executing API call: {ex.Message}");
            }
        }

        private async Task UpdateJwtToken()
        {
            if (string.IsNullOrEmpty(_jwtToken) || IsTokenExpired(_jwtToken))
            {
                _jwtToken = await GetJwtToken();
            }

            // Cetak token JWT di konsol
            Console.WriteLine($"JWT Token: {_jwtToken}");
        }

        [HttpGet("test-token")]
        public async Task<IActionResult> TestGetJwtToken()
        {
            try
            {
                // Panggil function GetJwtToken
                string jwtToken = await GetJwtToken();

                // Jika token berhasil diperoleh
                if (!string.IsNullOrEmpty(jwtToken))
                {
                    // Cetak token JWT di konsol
                    Console.WriteLine($"JWT Token: {jwtToken}");

                    // Mengembalikan respons HTTP 200 OK bersama dengan token JWT
                    return Ok(jwtToken);
                }
                else
                {
                    // Token JWT tidak berhasil diperoleh
                    throw new Exception("Failed to get JWT token.");
                }
            }
            catch (Exception ex)
            {
                // Menangani kesalahan dan mengembalikan respons HTTP 500 Internal Server Error
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        private async Task<string> GetJwtToken()
        {
            string token = null;

            // Membuat payload JSON untuk autentikasi
            var requestBody = new StringContent($"{{\"username\": \"{_username}\", \"password\": \"{_password}\"}}", System.Text.Encoding.UTF8, "application/json");

            // Melakukan permintaan POST untuk mendapatkan token JWT
            HttpResponseMessage response = await _httpClient.PostAsync(_jwtApiUrl, requestBody);

            // Mengecek apakah respon berhasil (kode status 200-299)
            if (response.IsSuccessStatusCode)
            {
                // Membaca konten respon sebagai string
                string responseBody = await response.Content.ReadAsStringAsync();

                // Mengurai respons JSON menjadi objek menggunakan kelas model
                var tokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenResponseModel>(responseBody);

                // Mengambil token JWT dari properti token di model respons
                token = tokenResponse.token;
            }
            else
            {
                // Gagal mendapatkan token JWT
                throw new Exception($"Failed to get JWT token. Status code: {response.StatusCode}");
            }

            return token;
        }

        private bool IsTokenExpired(string token)
        {
            // Implementasikan logika pengecekan token kadaluarsa di sini
            // Misalnya, Anda dapat memeriksa waktu kadaluarsa token dan membandingkannya dengan waktu saat ini
            // Jika waktu kadaluarsa sudah lewat, kembalikan true, jika tidak, kembalikan false
            return false; // Contoh sederhana: selalu mengembalikan false untuk sekarang
        }

        private async Task SaveResponseToDatabase(string responseBody)
        {
            try
            {
                // Anda perlu mengonversi responseBody menjadi objek ResponseModel terlebih dahulu
                var responseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseModel>(responseBody);

                // Panggil metode penyimpanan ke database dengan menggunakan objek responseModel
                await SaveResponseModelToDatabase(responseModel);
            }
            catch (Exception ex)
            {
                // Tangani kesalahan jika terjadi
                Console.WriteLine($"Error: {ex.Message}");
                throw; // Lepaskan exception untuk ditangkap di endpoint yang memanggil metode ini
            }
        }

        private async Task SaveResponseModelToDatabase(ResponseModel responseModel)
        {
            using (SqlConnection conn = new SqlConnection(_constrAbsen))
            {
                try
                {
                    conn.Open();

                    // Lakukan operasi penyimpanan data ke database menggunakan objek responseModel
                    foreach (var data in responseModel.data)
                    {
                        // Cek apakah nilai emp tidak null sebelum mengaksesnya
                        int? empValue = data.emp;

                        if (empValue.HasValue)
                        {
                            // Lakukan operasi penyimpanan data ke database
                            SqlCommand cmd = new SqlCommand("sp_SimpanPollingTransaksiAbsenMobile", conn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@id_orang", data.emp_code);
                            cmd.Parameters.AddWithValue("@nip", data.emp_code);
                            cmd.Parameters.AddWithValue("@tanggal", data.punch_time);
                            cmd.Parameters.AddWithValue("@jenis_absen", "Mesin");
                            cmd.Parameters.AddWithValue("@jenis_waktu_absen", "Mesin");
                            cmd.Parameters.AddWithValue("@waktu", data.punch_time);
                            cmd.Parameters.AddWithValue("@longitude", "-");
                            cmd.Parameters.AddWithValue("@latitude", "-");
                            SqlParameter returnParm1 = cmd.Parameters.Add("@retval", SqlDbType.VarChar);
                            returnParm1.Size = 100;
                            returnParm1.Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();

                            string retval = returnParm1.Value.ToString();

                            if (retval == "OK")
                            {
                                Console.WriteLine("Data has been saved to the database.");
                            }
                            else
                            {
                                Console.WriteLine("Data already exists.");
                            }
                        }
                        else
                        {
                            // Handle nilai emp yang null sesuai kebutuhan Anda
                            // Misalnya, lewati atau tangani dengan cara tertentu
                            Console.WriteLine("Nilai emp null, tidak dapat menyimpan data ke database.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    throw; // Lepaskan exception untuk ditangkap di endpoint yang memanggil metode ini
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        //private async Task SaveResponseToDatabase(string responseBody)
        //{
        //    using (SqlConnection conn = new SqlConnection(_constrAbsen))
        //    {
        //        try
        //        {
        //            conn.Open();

        //            foreach (var data in responseBody.data)
        //            {
        //                SqlCommand cmd = new SqlCommand("sp_SimpanPollingTransaksiAbsenMobile", conn);
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.AddWithValue("@id_orang", data.emp_code);
        //                cmd.Parameters.AddWithValue("@nip", data.emp_code);
        //                cmd.Parameters.AddWithValue("@tanggal", data.punch_time);
        //                cmd.Parameters.AddWithValue("@jenis_absen", "Mesin");
        //                cmd.Parameters.AddWithValue("@jenis_waktu_absen", "Mesin");
        //                cmd.Parameters.AddWithValue("@waktu", data.punch_time);
        //                cmd.Parameters.AddWithValue("@longitude", "-");
        //                cmd.Parameters.AddWithValue("@latitude", "-");
        //                SqlParameter returnParm1 = cmd.Parameters.Add("@retval", SqlDbType.VarChar);
        //                returnParm1.Size = 100;
        //                returnParm1.Direction = ParameterDirection.Output;
        //                cmd.ExecuteNonQuery();

        //                string retval = returnParm1.Value.ToString();

        //                if (retval == "OK")
        //                {
        //                    Console.WriteLine("Data has been saved to the database.");
        //                }
        //                else
        //                {
        //                    Console.WriteLine("Data already exists.");
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"Error: {ex.Message}");
        //            throw; // Lemparkan exception agar dapat ditangkap oleh penangan kesalahan di endpoint.
        //        }
        //    }
        //}

      
        public class ResponseModel
        {
            public int count { get; set; }
            public string next { get; set; }
            public string previous { get; set; }
            public string msg { get; set; }
            public int code { get; set; }
            public List<DataModel> data { get; set; }
        }

        public class DataModel
        {
            public int id { get; set; }
            public int? emp { get; set; }
            public string? emp_code { get; set; }
            public string? first_name { get; set; }
            public object? last_name { get; set; }
            public string? department { get; set; }
            public string? position { get; set; }
            public string? punch_time { get; set; }
            public string? punch_state { get; set; }
            public string? punch_state_display { get; set; }
            public int? verify_type { get; set; }
            public string? verify_type_display { get; set; }
            public string? work_code { get; set; }
            public object? gps_location { get; set; }
            public string? area_alias { get; set; }
            public string? terminal_sn { get; set; }
            public double? temperature { get; set; }
            public string? is_mask { get; set; }
            public string? terminal_alias { get; set; }
            //public DateTime upload_time { get; set; }
        }

        // Definisikan kelas model untuk respons token
        public class TokenResponseModel
        {
            public string token { get; set; }
        }









        private readonly string _constrAbsen = Startup.ConnectionStringAbsen;


        [HttpPost("save-response")]
        public IActionResult SaveResponsex([FromBody] ResponseModel responseBody)
        {
            try
            {
                SaveResponseToDatabasex(responseBody);
                return Ok("Data saved to database successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        private void SaveResponseToDatabasex(ResponseModel responseBody)
        {
            using (SqlConnection conn = new SqlConnection(_constrAbsen))
            {
                try
                {
                    conn.Open();

                    foreach (var data in responseBody.data)
                    {
                        SqlCommand cmd = new SqlCommand("sp_SimpanPollingTransaksiAbsenMobile", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_orang", data.emp_code);
                        cmd.Parameters.AddWithValue("@nip", data.emp_code);
                        cmd.Parameters.AddWithValue("@tanggal", data.punch_time);
                        cmd.Parameters.AddWithValue("@jenis_absen", "Mesin");
                        cmd.Parameters.AddWithValue("@jenis_waktu_absen", "Mesin");
                        cmd.Parameters.AddWithValue("@waktu", data.punch_time);
                        cmd.Parameters.AddWithValue("@longitude", "-");
                        cmd.Parameters.AddWithValue("@latitude", "-");
                        SqlParameter returnParm1 = cmd.Parameters.Add("@retval", SqlDbType.VarChar);
                        returnParm1.Size = 100;
                        returnParm1.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();

                        string retval = returnParm1.Value.ToString();

                        if (retval == "OK")
                        {
                            Console.WriteLine("Data has been saved to the database.");
                        }
                        else
                        {
                            Console.WriteLine("Data already exists.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    throw; // Lemparkan exception agar dapat ditangkap oleh penangan kesalahan di endpoint.
                }
            }
        }























    }
}
