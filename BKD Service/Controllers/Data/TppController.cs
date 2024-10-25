using BKD_API.Models;
using BKD_Service.Model.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BKD_Service.Controllers.Data
{
    [Route("api/data/[controller]")]
    [ApiController]
    [Authorize]
    public class TppController : Controller
    {
        public IConfiguration Configuration { get; }
        string constr = Startup.ConnectionStringSimpeg;
        [HttpPost(Name = "ProsesTPP")]
        public async Task<ActionResult<Respon>> simpan([FromBody] RequestTPP request)
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
                    SqlCommand cmd = new SqlCommand("sp_calculate_Daftar_Bayar_TPP_Promal_2024", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@tgl", request.tgl);
                    cmd.Parameters.AddWithValue("@bln", request.bln);
                    cmd.Parameters.AddWithValue("@thn", request.thn);
                    cmd.Parameters.AddWithValue("@kelompok_unker", request.kelompok_unker);
                 
                    SqlParameter returnParm1 = cmd.Parameters.Add("@return_val", SqlDbType.VarChar);
                    returnParm1.Size = 100;
                    returnParm1.Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    retval = Convert.ToString(cmd.Parameters["@return_val"].Value);
                    conn.Close();
                }


                respon.pesan = retval;
                respon.data = request;
                return StatusCode(StatusCodes.Status200OK, respon);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
