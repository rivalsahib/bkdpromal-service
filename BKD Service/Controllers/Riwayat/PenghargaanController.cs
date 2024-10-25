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

namespace BKD_Service.Controllers.Riwayat
{
    [Route("api/riwayat/[controller]")]
    [ApiController]
    [Authorize]
    public class PenghargaanController : Controller
    {
        public IConfiguration Configuration { get; }
        string constr = Startup.ConnectionStringSimpeg;

        [HttpPost(Name = "SimpanRiwayatPenghargaan")]
        public async Task<ActionResult<Respon>> simpan([FromBody] Penghargaan riwayat)
        {
            string retval = "";
            int i = 0;
            Respon respon = new Respon();

            try
            {
                //Simpan Riwayat Penghargaan
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_SimpanRiwayatPenghargaan", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", riwayat.id);
                    cmd.Parameters.AddWithValue("@id_bkn", riwayat.id_bkn);
                    cmd.Parameters.AddWithValue("@id_orang", riwayat.id_orang);
                    cmd.Parameters.AddWithValue("@nipbaru", riwayat.nipbaru);
                    cmd.Parameters.AddWithValue("@niplama", riwayat.niplama);
                    cmd.Parameters.AddWithValue("@kd_penghargaan", riwayat.kd_penghargaan);
                    cmd.Parameters.AddWithValue("@kd_penghargaan_bkn", riwayat.kd_penghargaan_bkn);
                    cmd.Parameters.AddWithValue("@no_SK", riwayat.no_SK);
                    cmd.Parameters.AddWithValue("@tgl_SK", Convert.ToDateTime(riwayat.tgl_SK));
                    cmd.Parameters.AddWithValue("@tahun", riwayat.tahun);
                    cmd.Parameters.AddWithValue("@asal_perolehan", riwayat.asal_perolehan);
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

        [HttpPost("hapus/{id}", Name = "HapusRiwayatPenghargaan")]
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
                    SqlCommand cmd = new SqlCommand("sp_HapusRiwayatPenghargaan", conn);
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
