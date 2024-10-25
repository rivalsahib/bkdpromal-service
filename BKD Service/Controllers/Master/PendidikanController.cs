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
using BKD_Service.Model.Master;
using Newtonsoft.Json.Linq;
namespace BKD_Service.Controllers.Master
{
    [Route("api/master/[controller]")]
    [ApiController]
    [Authorize]
    public class PendidikanController : Controller
    {
        public IConfiguration Configuration { get; }
        string constr = Startup.ConnectionStringAdmin;

        [HttpGet(Name = "GetMasterPendidikan")]
        public async Task<ActionResult<Respon>> GetAll([FromBody] ReqTokenBKN reqData)
        {
            string retval = "";
            string url = "/referensi_siasn/1/pendidikan?limit=1000000&offset=0";
            Respon respon = new Respon();
            List<Pendidikan> master_pendidikan = new List<Pendidikan>();
            int i = 0;

            try
            {
                var options = new RestClientOptions("https://apimws.bkn.go.id:8243")
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest(url, Method.Get);
                request.AddHeader("accept", "application/json");
                request.AddHeader("Auth", "bearer " + reqData.tokenAksesBKN);
                request.AddHeader("Authorization", "Bearer " + reqData.tokenAPIBKN);
                request.AddHeader("Cookie", "ff8d625df24f2272ecde05bd53b814bc=03b84dc733e49356be29cae4eafc63ae; pdns=1091068938.13088.0000");
                RestResponse response = await client.ExecuteAsync(request);
                Console.WriteLine(response.Content);

                dynamic results = JsonConvert.DeserializeObject<dynamic>(response.Content);


                foreach (var item in results.data)
                {
                    //Simpegn Riwayat Pangkat
                    using (SqlConnection conn = new SqlConnection(constr))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("sp_SimpanMasterPendidikanSinkron", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", 0);
                        cmd.Parameters.AddWithValue("@id_bkn", results.id.Value);
                        cmd.Parameters.AddWithValue("@id_tingkat_pendidikan", "");
                        cmd.Parameters.AddWithValue("@id_tingkat_pendidikan_bkn", results.tingkat_pendidikan_id.Value);
                        cmd.Parameters.AddWithValue("@mgr_cepat_kode", results.mgr_cepat_kode.Value);
                        cmd.Parameters.AddWithValue("@nama_asli", results.nama_asli.Value);
                        cmd.Parameters.AddWithValue("@status", results.status.Value);
                        cmd.Parameters.AddWithValue("@subrumpun_prog_id", results.subrumpun_prog_id.Value);
                        cmd.Parameters.AddWithValue("@cepat_kode_induk", results.cepat_kode_induk.Value);
                        cmd.Parameters.AddWithValue("@subrumpun_prog_kode", results.subrumpun_prog_kode.Value);
                        cmd.Parameters.AddWithValue("@kode_cepat", results.cepat_kode.Value);
                        cmd.Parameters.AddWithValue("@nama_pendidikan", results.nama.Value);

                        SqlParameter returnParm1 = cmd.Parameters.Add("@return_val", SqlDbType.VarChar);
                        returnParm1.Size = 100;
                        returnParm1.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                        retval = Convert.ToString(cmd.Parameters["@return_val"].Value);
                        conn.Close();
                        i++;
                    }
                }




                //Get Master Pendidikan
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_TampilMasterPendidikan", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Pendidikan pendidikan = new Pendidikan();
                        pendidikan.id = Convert.ToInt32(rdr["id"].ToString());
                        pendidikan.id_bkn = rdr["id_bkn"].ToString();
                        pendidikan.id_tingkat_pendidikan  = rdr["id_tingkat_pendidikan"].ToString();
                        pendidikan.id_tingkat_pendidikan_bkn  = rdr["id_tingkat_pendidikan_bkn"].ToString();
                        pendidikan.mgr_cepat_kode  = rdr["mgr_cepat_kode"].ToString();
                        pendidikan.nama_asli  = rdr["nama_asli"].ToString();
                        pendidikan.status  = rdr["status"].ToString();
                        pendidikan.subrumpun_prog_id  = rdr["subrumpun_prog_id"].ToString();
                        pendidikan.cepat_kode_induk  = rdr["cepat_kode_induk"].ToString();
                        pendidikan.subrumpun_prog_kode  = rdr["subrumpun_prog_kode"].ToString();
                        pendidikan.kode_cepat  = rdr["kode_cepat"].ToString();
                        pendidikan.nama_pendidikan  = rdr["nama_pendidikan"].ToString();
                        master_pendidikan.Add(pendidikan);
                    }
                    conn.Close();
                }


                respon.pesan = "Data Berhasil Ditemukan";
                respon.data = master_pendidikan;
                return StatusCode(StatusCodes.Status200OK, respon);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("sinkron", Name = "GetMasterPendidikanSinkron")]
        public async Task<ActionResult<Respon>> Sinkron([FromBody] ReqTokenBKN reqData)
        {
            string retval = "";
            string url = "/referensi_siasn/1/pendidikan?limit=100000&offset=0";
            Respon respon = new Respon();
            List<Pendidikan> master_pendidikan = new List<Pendidikan>();
            int i = 0;

            try
            {
                var options = new RestClientOptions("https://apimws.bkn.go.id:8243")
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest(url, Method.Get);
                request.AddHeader("accept", "application/json");
                request.AddHeader("Auth", "bearer " + reqData.tokenAksesBKN);
                request.AddHeader("Authorization", "Bearer " + reqData.tokenAPIBKN);
                request.AddHeader("Cookie", "ff8d625df24f2272ecde05bd53b814bc=03b84dc733e49356be29cae4eafc63ae; pdns=1091068938.13088.0000");
                RestResponse response = await client.ExecuteAsync(request);
                Console.WriteLine(response.Content);

                dynamic results = JsonConvert.DeserializeObject<dynamic>(response.Content);


                foreach (var item in results)
                {
                    //Simpegn Riwayat Pangkat
                    using (SqlConnection conn = new SqlConnection(constr))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("sp_SimpanMasterPendidikanSinkron", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", 0);
                        cmd.Parameters.AddWithValue("@id_bkn", results[i].id.Value);
                        cmd.Parameters.AddWithValue("@id_tingkat_pendidikan", "");
                        cmd.Parameters.AddWithValue("@id_tingkat_pendidikan_bkn", results[i].tingkat_pendidikan_id.Value);
                        cmd.Parameters.AddWithValue("@mgr_cepat_kode", results[i].mgr_cepat_kode.Value);
                        cmd.Parameters.AddWithValue("@nama_asli", results[i].nama_asli.Value);
                        cmd.Parameters.AddWithValue("@status", results[i].status.Value);
                        cmd.Parameters.AddWithValue("@subrumpun_prog_id", results[i].subrumpun_prog_id.Value);
                        cmd.Parameters.AddWithValue("@cepat_kode_induk", results[i].cepat_kode_induk.Value);
                        cmd.Parameters.AddWithValue("@subrumpun_prog_kode", results[i].subrumpun_prog_kode.Value);
                        cmd.Parameters.AddWithValue("@kode_cepat", results[i].cepat_kode.Value);
                        cmd.Parameters.AddWithValue("@nama_pendidikan", results[i].nama.Value);

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
