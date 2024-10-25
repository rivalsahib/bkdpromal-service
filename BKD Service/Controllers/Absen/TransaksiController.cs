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
using BKD_API.Models.Absen;
using BKD_API.Models;
using Microsoft.AspNetCore.Authorization;

namespace BKD_Service.Controllers.Absen
{
    [Route("api/absen/[controller]")]
    [ApiController]
    //[Authorize]
    public class TransaksiController : Controller
    {
        public IConfiguration Configuration { get; }
        string constr = Startup.ConnectionStringAbsen;

        [HttpPost("save")]
        
        public async Task<ActionResult<Respon>> save([FromBody] TransaksiAbsen transaksi)
        {
            string retval;
            string tipe_pengguna;
            DateTime exp_date = DateTime.UtcNow.AddMinutes(10);
            Respon respon = new Respon();

            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    conn.Open();
                    //Cari Data Login
                    SqlCommand cmd = new SqlCommand("sp_SimpanPollingTransaksiAbsenMobile", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_orang", transaksi.id_orang);
                    cmd.Parameters.AddWithValue("@nip", transaksi.nip);
                    cmd.Parameters.AddWithValue("@tanggal", transaksi.tanggal);
                    cmd.Parameters.AddWithValue("@jenis_absen", transaksi.jenis_absen);
                    cmd.Parameters.AddWithValue("@jenis_waktu_absen", transaksi.jenis_waktu_absen);
                    cmd.Parameters.AddWithValue("@waktu", transaksi.waktu);
                    cmd.Parameters.AddWithValue("@longitude", transaksi.longitude);
                    cmd.Parameters.AddWithValue("@latitude", transaksi.latitude);
                    SqlParameter returnParm1 = cmd.Parameters.Add("@retval", SqlDbType.VarChar);
                    returnParm1.Size = 100;
                    returnParm1.Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();

                    retval = returnParm1.Value.ToString();

                    if (retval == "OK")
                    {
                        respon.pesan = "Berhasil";
                        respon.data = transaksi;

                        return StatusCode(StatusCodes.Status200OK, respon);
                    }
                    else
                    {
                        respon.pesan = retval;
                        respon.data = transaksi;

                        return StatusCode(StatusCodes.Status302Found, respon);
                    }

                }
                catch (Exception ex)
                {
                    respon.pesan = ex.Message;
                    respon.data = null;
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
                finally { conn.Close(); }
            }
        }
    }
}
