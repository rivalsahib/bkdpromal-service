using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BKD_API.Models;
using BKD_API.Models.Akun;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using RestSharp;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BKD_Service.Model.Akun;

namespace BKD_Service.Controllers.Akun
{
    [Route("api/admin/akun/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        public IConfiguration Configuration { get; }
        private readonly JWTSetting setting;
        string constrAdmin = Startup.ConnectionStringAdmin;
        string constrSimpeg = Startup.ConnectionStringSimpeg;

        [HttpPost]
        public async Task<ActionResult<Respon>> login([FromBody] UserCredential user)
        {
            string retval;
            string nip;
            string nama;
            Respon respon = new Respon();
            PenggunaProfil profil = new PenggunaProfil();

            using (SqlConnection conn = new SqlConnection(constrSimpeg))
            {
                try
                {
                    conn.Open();
                    //Cari Data Login
                    SqlCommand cmd = new SqlCommand("sp_CekLogin", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nama_pengguna", user.nama_pengguna);
                    cmd.Parameters.AddWithValue("@kata_sandi", user.kata_sandi);
                    SqlParameter returnParm1 = cmd.Parameters.Add("@retval", SqlDbType.VarChar);
                    returnParm1.Size = 100;
                    returnParm1.Direction = ParameterDirection.Output;
                    SqlParameter returnParm2 = cmd.Parameters.Add("@nip", SqlDbType.VarChar);
                    returnParm2.Size = 100;
                    returnParm2.Direction = ParameterDirection.Output;
                    SqlParameter returnParm3 = cmd.Parameters.Add("@nama", SqlDbType.VarChar);
                    returnParm3.Size = 100;
                    returnParm3.Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();

                    retval = returnParm1.Value.ToString();
                    nip = returnParm2.Value.ToString();
                    nama = returnParm3.Value.ToString();

                    profil.nip = nip;
                    profil.nama = nama;
                    profil.username = user.nama_pengguna;

                    if (retval == "Login Berhasil")
                    {
                        respon.pesan = retval;
                        respon.data = profil;

                        return StatusCode(StatusCodes.Status200OK, respon);
                    }
                    else
                    {
                        respon.pesan = retval;
                        respon.data = profil;

                        return StatusCode(StatusCodes.Status404NotFound, respon);
                    }

                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
                finally { conn.Close(); }
            }
        }


    }
}
