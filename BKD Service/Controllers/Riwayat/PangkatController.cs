using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using BKD_API.Models;
using BKD_API.Models.Data;
using System.Net.Http;
using RestSharp;
using Microsoft.AspNetCore.Authorization;
using BKD_Service.Model;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using BKD_Service.Model.riwayat;
using Newtonsoft.Json.Linq;
using BKD_Service.Model.Riwayat;

namespace BKD_Service.Controllers.riwayat
{
    [Route("api/riwayat/[controller]")]
    [ApiController]
    [Authorize]
    public class PangkatController : Controller
    {
        public IConfiguration Configuration { get; }
        string constr = Startup.ConnectionStringSimpeg;

        [HttpGet("{nip}", Name = "GetRiwayatPangkat")]
        public async Task<ActionResult<Respon>> GetByNip(string nip)
        {
            string retval = "";
            //string url = "/apisiasn/1.0/pns/rw-golongan/" + nip;
            Respon respon = new Respon();
            List<Pangkat> riwayat_pangkat = new List<Pangkat>();
            int i = 0;

            try
            {
                //var options = new RestClientOptions("https://apimws.bkn.go.id:8243")
                //{
                //    MaxTimeout = -1,
                //};
                //var client = new RestClient(options);
                //var request = new RestRequest(url, Method.Get);
                //request.AddHeader("accept", "application/json");
                //request.AddHeader("Auth", "bearer " + tokenaksesbkn);
                //request.AddHeader("Authorization", "Bearer " + tokenapibkn);
                //request.AddHeader("Cookie", "ff8d625df24f2272ecde05bd53b814bc=03b84dc733e49356be29cae4eafc63ae; pdns=1091068938.13088.0000");
                //RestResponse response = await client.ExecuteAsync(request);
                //Console.WriteLine(response.Content);

                //dynamic results = JsonConvert.DeserializeObject<dynamic>(response.Content);


                //foreach (var item in results.data)
                //{
                //    //Simpegn Riwayat Pangkat
                //    using (SqlConnection conn = new SqlConnection(constr))
                //    {
                //        conn.Open();
                //        SqlCommand cmd = new SqlCommand("sp_SimpanRiwayatPangkatSinkron", conn);
                //        cmd.CommandType = CommandType.StoredProcedure;
                //        cmd.Parameters.AddWithValue("@id", 0);
                //        cmd.Parameters.AddWithValue("@id_bkn", data.id.Value);
                //        cmd.Parameters.AddWithValue("@id_orang", data.idPns.Value);
                //        cmd.Parameters.AddWithValue("@nipbaru", data.nipBaru.Value);
                //        cmd.Parameters.AddWithValue("@niplama", data.nipLama.Value);
                //        cmd.Parameters.AddWithValue("@kd_pangkat", data.golonganId.Value);
                //        cmd.Parameters.AddWithValue("@tmt_pangkat", Convert.ToDateTime(data.tmtGolongan.Value));
                //        cmd.Parameters.AddWithValue("@pejabat_yang_menetapkan", "");
                //        cmd.Parameters.AddWithValue("@nomor_sk", data.skNomor.Value);

                //        if (data.skTanggal.Value != null)
                //        {
                //            cmd.Parameters.AddWithValue("@tgl_sk", Convert.ToDateTime(data.skTanggal.Value));
                //        }
                //        else
                //        {
                //            cmd.Parameters.AddWithValue("@tgl_sk", "1900/01/01");
                //        }



                //        cmd.Parameters.AddWithValue("@ms_th", data.masaKerjaGolonganTahun.Value);
                //        cmd.Parameters.AddWithValue("@ms_bl", data.masaKerjaGolonganBulan.Value);
                //        cmd.Parameters.AddWithValue("@nomor_pertek", data.noPertekBkn.Value);

                //        if (data.tglPertekBkn.Value != null)
                //        {
                //            cmd.Parameters.AddWithValue("@tgl_pertek", Convert.ToDateTime(data.tglPertekBkn.Value));
                //        }
                //        else
                //        {
                //            cmd.Parameters.AddWithValue("@tgl_pertek", "1900/01/01");
                //        }


                //        if (data.jumlahKreditUtama.Value != null)
                //        {
                //            cmd.Parameters.AddWithValue("@jumlahKreditUtama", data.jumlahKreditUtama.Value);
                //        }
                //        else
                //        {
                //            cmd.Parameters.AddWithValue("@jumlahKreditUtama", "");
                //        }
                //        if (data.jumlahKreditTambahan.Value != null)
                //        {
                //            cmd.Parameters.AddWithValue("@jumlahKreditTambahan", data.jumlahKreditTambahan.Value);
                //        }
                //        else
                //        {
                //            cmd.Parameters.AddWithValue("@jumlahKreditTambahan", "");
                //        }
                //        cmd.Parameters.AddWithValue("@id_jenis_kp_bkn", data.jenisKPId.Value);
                //        cmd.Parameters.AddWithValue("@gaji", "");
                //        if (data.path.Value != null)
                //        {
                //            cmd.Parameters.AddWithValue("@path", data.path.Value);
                //        }
                //        else
                //        {
                //            cmd.Parameters.AddWithValue("@path", "");
                //        }

                //        SqlParameter returnParm1 = cmd.Parameters.Add("@return_val", SqlDbType.VarChar);
                //        returnParm1.Size = 100;
                //        returnParm1.Direction = ParameterDirection.Output;
                //        cmd.ExecuteNonQuery();
                //        retval = Convert.ToString(cmd.Parameters["@return_val"].Value);
                //        conn.Close();
                //        i++;
                //    }
                //}




                //Get Riwayat Pangkat
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_TampilRiwayatPangkat", conn);
                    cmd.Parameters.AddWithValue("@nipbaru", nip);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Pangkat pangkat = new Pangkat();
                        pangkat.id = Convert.ToInt32(rdr["id"].ToString());
                        pangkat.id_bkn = rdr["id_bkn"].ToString();
                        pangkat.id_orang = rdr["id_orang"].ToString();
                        pangkat.nipbaru = rdr["nipbaru"].ToString();
                        pangkat.niplama = rdr["niplama"].ToString();
                        pangkat.kd_pangkat = Convert.ToInt32(rdr["kd_pangkat"].ToString());
                        pangkat.nama_pangkat_gol = rdr["nama_pangkat_gol"].ToString();
                        pangkat.nomor_sk = rdr["nomor"].ToString();
                        pangkat.tgl_sk = rdr["tgl"].ToString();
                        pangkat.tmt_pangkat = rdr["tmt_pangkat"].ToString();
                        pangkat.pejabat_yang_menetapkan = rdr["pejabat_yg_menetapkan"].ToString();
                        pangkat.nomor_pertek = rdr["notaSetujuBKN"].ToString();
                        pangkat.tgl_pertek = rdr["tglnotaSetujuBKN"].ToString();
                        pangkat.jumlahKreditUtama = rdr["jumlahKreditUtama"].ToString();
                        pangkat.jumlahKreditTambahan = rdr["jumlahKreditTambahan"].ToString();
                        pangkat.id_jenis_kp_bkn = rdr["id_jenis_kp_BKN"].ToString();
                        pangkat.kd_jenis_kp = rdr["kd_jenis_kp"].ToString();
                        pangkat.jenis_kp = rdr["jenis_kp"].ToString();
                        pangkat.gaji = rdr["gaji"].ToString();
                        pangkat.ms_th = rdr["ms_th"].ToString();
                        pangkat.ms_bl = rdr["ms_bl"].ToString();
                        pangkat.path = rdr["path"].ToString();
                        riwayat_pangkat.Add(pangkat);
                    }
                    conn.Close();
                }


                respon.pesan = "Data Berhasil Ditemukan";
                respon.data = riwayat_pangkat;
                return StatusCode(StatusCodes.Status200OK, respon);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



        [HttpPost(Name = "SimpanRiwayatPangkat")]
        public async Task<ActionResult<Respon>> simpan([FromBody] Pangkat riwayat)
        {
            string retval = "";
            int i = 0;
            Respon respon = new Respon();

            try
            {
                //Simpan Riwayat Pangkat
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_SimpanRiwayatPangkat", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", riwayat.id);
                    cmd.Parameters.AddWithValue("@id_bkn", riwayat.id_bkn);
                    cmd.Parameters.AddWithValue("@id_orang", riwayat.id_orang);
                    cmd.Parameters.AddWithValue("@nipbaru", riwayat.nipbaru);
                    cmd.Parameters.AddWithValue("@niplama", riwayat.niplama);
                    cmd.Parameters.AddWithValue("@kd_pangkat", riwayat.kd_pangkat);
                    cmd.Parameters.AddWithValue("@nomor_sk", riwayat.nomor_sk);
                    cmd.Parameters.AddWithValue("@tgl_sk", riwayat.tgl_sk);
                    cmd.Parameters.AddWithValue("@tmt_pangkat", riwayat.tmt_pangkat);
                    cmd.Parameters.AddWithValue("@pejabat_yang_menetapkan", riwayat.pejabat_yang_menetapkan);
                    cmd.Parameters.AddWithValue("@nomor_pertek", riwayat.nomor_pertek);
                    cmd.Parameters.AddWithValue("@tgl_pertek", riwayat.tgl_pertek);
                    cmd.Parameters.AddWithValue("@jumlahKreditUtama", riwayat.jumlahKreditUtama);
                    cmd.Parameters.AddWithValue("@jumlahKreditTambahan", riwayat.jumlahKreditTambahan);
                    cmd.Parameters.AddWithValue("@id_jenis_kp_bkn", riwayat.id_jenis_kp_bkn);
                    cmd.Parameters.AddWithValue("kd_jenis_kp", riwayat.kd_jenis_kp);
                    cmd.Parameters.AddWithValue("@gaji", riwayat.gaji);
                    cmd.Parameters.AddWithValue("@ms_th", riwayat.ms_th);
                    cmd.Parameters.AddWithValue("@ms_bl", riwayat.ms_bl);
                    cmd.Parameters.AddWithValue("@path", riwayat.path);

                    SqlParameter returnParm1 = cmd.Parameters.Add("@return_val", SqlDbType.VarChar);
                    returnParm1.Size = 100;
                    returnParm1.Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    retval = Convert.ToString(cmd.Parameters["@return_val"].Value);
                    conn.Close();
                }

                respon.pesan = retval;
                respon.data = riwayat;
                return StatusCode(StatusCodes.Status200OK, respon);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("hapus/{id}", Name = "HapusRiwayatPangkat")]
        public async Task<ActionResult<Respon>> hapus(string id)
        {
            string retval = "";
            Respon respon = new Respon();

            try
            {
                //Simpan Riwayat Pangkat
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_HapusRiwayatPangkat", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);


                    SqlParameter returnParm1 = cmd.Parameters.Add("@return_val", SqlDbType.VarChar);
                    returnParm1.Size = 100;
                    returnParm1.Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    retval = Convert.ToString(cmd.Parameters["@return_val"].Value);
                    conn.Close();
                }

                respon.pesan = retval;
                respon.data = id;
                return StatusCode(StatusCodes.Status200OK, respon);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }




        [HttpPost("sinkron/{nip}", Name = "GetRiwayatPangkatSinkron")]
        public async Task<ActionResult<Respon>> Sinkron([FromBody] List<PangkatBKN> riwayat, string nip)
        {
            string retval = "";
            int i = 0;
            Respon respon = new Respon();
            
            try
            {
               foreach (var data in riwayat)
                {
                    var pathObj = riwayat[i].path;

                    // Mengakses properti dok_uri dalam objek path
                    


                    //Simpan Riwayat Pangkat
                    using (SqlConnection conn = new SqlConnection(constr))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("sp_SimpanRiwayatPangkatSinkron", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", 0);
                        cmd.Parameters.AddWithValue("@id_bkn", riwayat[i].id);
                        cmd.Parameters.AddWithValue("@id_orang", riwayat[i].idPns);
                        cmd.Parameters.AddWithValue("@nipbaru", nip);
                        cmd.Parameters.AddWithValue("@niplama", riwayat[i].nipLama);
                        cmd.Parameters.AddWithValue("@kd_pangkat", riwayat[i].golonganId);
                        cmd.Parameters.AddWithValue("@tmt_pangkat", Convert.ToDateTime(riwayat[i].tmtGolongan));
                        cmd.Parameters.AddWithValue("@pejabat_yang_menetapkan", "");
                        cmd.Parameters.AddWithValue("@nomor_sk", riwayat[i].skNomor);

                        if (riwayat[i].skTanggal != null)
                        {
                            string skTanggal = riwayat[i].skTanggal;
                            string tahun = skTanggal.Substring(6, 4); 
                            string bulan = skTanggal.Substring(3, 2);
                            string hari = skTanggal.Substring(0, 2);
                            cmd.Parameters.AddWithValue("@tgl_sk", Convert.ToDateTime(tahun + "/" + bulan + "/" + hari));
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@tgl_sk", "1900/01/01");
                        }



                        cmd.Parameters.AddWithValue("@ms_th", riwayat[i].masaKerjaGolonganTahun);
                        cmd.Parameters.AddWithValue("@ms_bl", riwayat[i].masaKerjaGolonganBulan);
                        cmd.Parameters.AddWithValue("@nomor_pertek", riwayat[i].noPertekBkn);

                        if (riwayat[i].tglPertekBkn != null)
                        {
                            string tglPertekBkn = riwayat[i].tglPertekBkn;
                            string tahun = tglPertekBkn.Substring(6, 4); 
                            string bulan = tglPertekBkn.Substring(3, 2);
                            string hari = tglPertekBkn.Substring(0, 2);
                            cmd.Parameters.AddWithValue("@tgl_pertek", Convert.ToDateTime(tahun + "/" + bulan + "/" + hari));
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@tgl_pertek", "1900/01/01");
                        }


                        if (riwayat[i].jumlahKreditUtama != null)
                        {
                            cmd.Parameters.AddWithValue("@jumlahKreditUtama", riwayat[i].jumlahKreditUtama);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@jumlahKreditUtama", "");
                        }
                        if (riwayat[i].jumlahKreditTambahan != null)
                        {
                            cmd.Parameters.AddWithValue("@jumlahKreditTambahan", riwayat[i].jumlahKreditTambahan);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@jumlahKreditTambahan", "");
                        }
                        cmd.Parameters.AddWithValue("@id_jenis_kp_bkn", riwayat[i].jenisKPId);
                        cmd.Parameters.AddWithValue("@gaji", "");
                        if (riwayat[i].path != null)
                        {
                            cmd.Parameters.AddWithValue("@path", riwayat[i].path["858"].dok_uri);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@path", "");
                        }

                        SqlParameter returnParm1 = cmd.Parameters.Add("@return_val", SqlDbType.VarChar);
                        returnParm1.Size = 100;
                        returnParm1.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                        retval = Convert.ToString(cmd.Parameters["@return_val"].Value);
                        conn.Close();
                        i++;
                    }
                }

                respon.pesan = "Data Berhasil Ditemukan";
                respon.data = retval;
                return StatusCode(StatusCodes.Status200OK, respon);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
