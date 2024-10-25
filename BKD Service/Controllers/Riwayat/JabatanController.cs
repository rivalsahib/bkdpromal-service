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
    public class JabatanController : Controller
    {
        public IConfiguration Configuration { get; }
        string constr = Startup.ConnectionStringSimpeg;

        [HttpPost(Name = "SimpanRiwayatJabatan")]
        public async Task<ActionResult<Respon>> simpan([FromBody] Jabatan riwayat)
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
                    SqlCommand cmd = new SqlCommand("sp_SimpanRiwayatJabatan", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", riwayat.id);
                    cmd.Parameters.AddWithValue("@id_bkn", riwayat.id_bkn);
                    cmd.Parameters.AddWithValue("@id_orang", riwayat.id_orang);
                    cmd.Parameters.AddWithValue("@nipbaru", riwayat.nipbaru);
                    cmd.Parameters.AddWithValue("@niplama", riwayat.niplama);
                    cmd.Parameters.AddWithValue("@jenjab", riwayat.jenjab);
                    cmd.Parameters.AddWithValue("@kd_jabatan_bkn", riwayat.kd_jabatan_bkn);
                    cmd.Parameters.AddWithValue("@kd_jabatan", riwayat.kd_jabatan);
                    cmd.Parameters.AddWithValue("@jabatan", riwayat.jabatan);
                    cmd.Parameters.AddWithValue("@ak", riwayat.ak);
                    cmd.Parameters.AddWithValue("@kd_eselon", riwayat.kd_eselon);
                    cmd.Parameters.AddWithValue("@tmt_jabatan", Convert.ToDateTime(riwayat.tmt_jabatan));
                    cmd.Parameters.AddWithValue("@pejabat_yg_menetapkan", riwayat.pejabat_yg_menetapkan);
                    cmd.Parameters.AddWithValue("@nomor_sk_jabatan", riwayat.nomor_sk_jabatan);
                    cmd.Parameters.AddWithValue("@tgl_sk", Convert.ToDateTime(riwayat.tgl_sk));
                    cmd.Parameters.AddWithValue("@no_sk_pelantikan", riwayat.no_sk_pelantikan);
                    cmd.Parameters.AddWithValue("@tgl_sk_pelantikan", Convert.ToDateTime(riwayat.tgl_sk_pelantikan));
                    cmd.Parameters.AddWithValue("@id_satker_bkn", riwayat.id_satker_bkn);
                    cmd.Parameters.AddWithValue("@id_satker", riwayat.id_satker);
                    cmd.Parameters.AddWithValue("@satker", riwayat.satker);
                    cmd.Parameters.AddWithValue("@unker", riwayat.unker);
                    cmd.Parameters.AddWithValue("@instansi", riwayat.instansi);
                    cmd.Parameters.AddWithValue("@path_sk_jabatan", riwayat.path_sk_jabatan);
                    cmd.Parameters.AddWithValue("@path_sk_pelantikan", riwayat.path_sk_pelantikan);
                    cmd.Parameters.AddWithValue("@id_substansi", "");
                    cmd.Parameters.AddWithValue("@nomor_spmt", riwayat.nomor_spmt);
                    cmd.Parameters.AddWithValue("@tgl_spmt", Convert.ToDateTime(riwayat.tgl_spmt));
                    cmd.Parameters.AddWithValue("@path_spmt", riwayat.path_spmt);

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

        [HttpPost("hapus/{id}", Name = "HapusRiwayatJabatan")]
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
                    SqlCommand cmd = new SqlCommand("sp_HapusRiwayatJabatan", conn);
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
    
    }
}
