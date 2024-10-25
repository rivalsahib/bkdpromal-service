using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using BKD_API.Models;
using BKD_API.Models.Akun;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace BKD_Service.Controllers
{
    [Route("api/admin/akun/[controller]")]
    [ApiController]
    [Authorize]
    public class PenggunaController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        string constrAdmin = Startup.ConnectionStringAdmin;

        [HttpPost("reset")]
        public async Task<ActionResult<Respon>> reset([FromBody] ResetPengguna user)
        {
            string retval;
            Respon respon = new Respon();

            using (SqlConnection conn = new SqlConnection(constrAdmin))
            {
                try
                {
                    conn.Open();
                    //Cari Data Login
                    SqlCommand cmd = new SqlCommand("sp_save_Reset_Kata_Sandi", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nama_pengguna", user.nama_pengguna);
                    cmd.Parameters.AddWithValue("@kata_sandi", user.kata_sandi);
                    cmd.Parameters.AddWithValue("@otp", user.otp);
                    SqlParameter returnParm1 = cmd.Parameters.Add("@retval", SqlDbType.VarChar);
                    returnParm1.Size = 100;
                    returnParm1.Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();

                    retval = returnParm1.Value.ToString();

                    if (retval == "OK")
                    {
                        respon.pesan = "";
                        respon.data = "";
                    }
                    else
                    {
                        respon.pesan = retval;
                        respon.data = "";
                    }

                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
                finally { conn.Close(); }
            }

            return respon;
        }

        [HttpGet]
        public async Task<ActionResult<Respon>> GetAll()
        {

            Respon respon = new Respon();
            List<Pengguna> daftar_pengguna = new List<Pengguna>();

            using (SqlConnection conn = new SqlConnection(constrAdmin))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_GetPenggunaAll", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            daftar_pengguna.Add(new Pengguna()
                            {
                                id = rdr.GetInt32("id"),
                                id_orang = rdr.GetString("id_orang"),
                                nama = rdr.GetString("nama"),
                                id_profil_pengguna = rdr.GetString("id_profil_pengguna"),
                                nama_pengguna = rdr.GetString("nama_pengguna"),
                                tmt_pengguna = rdr.GetDateTime("tmt_pengguna"),
                                tgl_selesai_pengguna = rdr.GetDateTime("tgl_selesai_pengguna"),
                                dokumen = rdr.GetString("dokumen"),
                                status = rdr.GetString("status"),
                                date_created = rdr.GetDateTime("date_created"),
                                date_edited = rdr.GetDateTime("date_edited"),
                                date_deleted = rdr.GetDateTime("date_deleted")
                            });
                        }
                        respon.pesan = "Data Berhasil Ditemukan";
                        respon.data = daftar_pengguna;
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

        [HttpGet("{nip}", Name = "GetPenggunaByNip")]
        public async Task<ActionResult<Respon>> GetById(string nip)
        {
            Respon respon = new Respon();
            Pengguna pengguna = new Pengguna();

            using (SqlConnection conn = new SqlConnection(constrAdmin))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_GetPenggunaByNIP", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nip", nip);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            pengguna.id = rdr.GetInt32("id");
                            pengguna.id_orang = rdr.GetString("id_orang");
                            pengguna.nama = rdr.GetString("nama");
                            pengguna.id_profil_pengguna = rdr.GetString("id_profil_pengguna");
                            pengguna.nama_pengguna = rdr.GetString("nama_pengguna");
                            pengguna.tmt_pengguna = rdr.GetDateTime("tmt_pengguna");
                            pengguna.tgl_selesai_pengguna = rdr.GetDateTime("tgl_selesai_pengguna");
                            pengguna.dokumen = rdr.GetString("dokumen");
                            pengguna.status = rdr.GetString("status");
                            pengguna.date_created = rdr.GetDateTime("date_created");
                            pengguna.date_edited = rdr.GetDateTime("date_edited");
                            pengguna.date_deleted = rdr.GetDateTime("date_deleted");

                            respon.pesan = "Data Berhasil Ditemukan";
                            respon.data = pengguna;
                            return StatusCode(StatusCodes.Status200OK, respon);
                        }
                        else
                        {
                            respon.pesan = "Data Tidak Ditemukan";
                            respon.data = pengguna;
                            return StatusCode(StatusCodes.Status404NotFound, respon);
                        }
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
