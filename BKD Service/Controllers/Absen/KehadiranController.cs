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
using BKD_API.Models.Absen;
using BKD_API.ModelS.Absen;
using Microsoft.AspNetCore.Authorization;

namespace BKD_Service.Controllers.Absen
{
    [Route("api/absen/[controller]")]
    [ApiController]
    [Authorize]
    public class KehadiranController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        string constr = Startup.ConnectionStringSimpeg;

        [HttpPost("proses")]
        public async Task<ActionResult<Respon>> save([FromBody] FormInput form)
        {
            int retval;
            string tipe_pengguna;
            DateTime exp_date = DateTime.UtcNow.AddMinutes(10);
            Respon respon = new Respon();

            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    conn.Open();
                    //Cari Data Login
                    SqlCommand cmd = new SqlCommand("sp_proses_Laporan_Absensi", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@bulan", form.bulan);
                    cmd.Parameters.AddWithValue("@tahun", form.tahun);
                    cmd.Parameters.AddWithValue("@unker", form.unker);
                    cmd.Parameters.AddWithValue("@tipe", form.tipe);
                    SqlParameter returnParm1 = cmd.Parameters.Add("@return_val", SqlDbType.Int);


                    returnParm1.Size = 100;
                    returnParm1.Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();

                    retval = Convert.ToInt32(returnParm1.Value.ToString());

                    if (retval == 14)
                    {
                        respon.pesan = "Berhasil";
                        respon.data = form;
                    }
                    else
                    {
                        respon.pesan = "Gagal";
                        respon.data = null;
                    }

                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
                finally { conn.Close(); }
            }

            //, respon);
            return respon;
        }


        [HttpGet]
        public async Task<ActionResult<Respon>> GetAll([FromBody] FormInput form)
        {
            Respon respon = new Respon();
            List<Kehadiran> daftar_kehadiran = new List<Kehadiran>();

            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_get_Laporan_Absensi_Daftar", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@bulan", form.bulan);
                    cmd.Parameters.AddWithValue("@tahun", form.tahun);
                    cmd.Parameters.AddWithValue("@kelompok_unker", form.unker);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            daftar_kehadiran.Add(new Kehadiran()
                            {
                                id = Convert.ToInt32(rdr["id"]),
                                no = rdr["nipbaru"].ToString(),
                                nipbaru = rdr["nipbaru"].ToString(),
                                nama = rdr["nama"].ToString(),
                                tanggal = Convert.ToDateTime(rdr["tanggal"].ToString()),
                                shift = rdr["shift"].ToString(),
                                jam_masuk = rdr["jam_masuk"].ToString(),
                                jam_pulang = rdr["jam_pulang"].ToString(),
                                terlambat = rdr["terlambat"].ToString(),
                                pulang_cepat = rdr["pulang_cepat"].ToString(),

                                jam_kerja = rdr["jam_kerja"].ToString(),
                                //daftar.jam_kerja_dari_opd = rdr["jam_kerja_dari_opd"].ToString();
                                status_absensi = rdr["status_absensi"].ToString()
                            });
                        }
                        respon.pesan = "Data Berhasil Ditemukan";
                        respon.data = daftar_kehadiran;
                        return StatusCode(StatusCodes.Status200OK, respon);
                    }
                }
                catch (Exception ex)
                {
                    respon.pesan = ex.Message;
                    respon.data = null;
                    return StatusCode(StatusCodes.Status500InternalServerError, respon);
                }
                finally { conn.Close(); }
            }
        }
    }
}
