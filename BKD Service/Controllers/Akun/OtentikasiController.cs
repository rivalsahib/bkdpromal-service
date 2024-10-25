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

namespace BKD_Service.Controllers
{
    [Route("api/admin/akun/[controller]")]
    [ApiController]
    public class OtentikasiController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        private readonly JWTSetting setting;
        string constr = Startup.ConnectionStringAdmin;

        public OtentikasiController(IOptions<JWTSetting> options)
        {
            setting = options.Value;
        }


        [HttpPost("login")]
        public async Task<ActionResult<ResponOtentikasi>> login([FromBody] UserCredential user)
        {
            string retval;
            string tipe_pengguna;
            DateTime exp_date = DateTime.UtcNow.AddSeconds(3600);
            ResponOtentikasi respon = new ResponOtentikasi();

            using (SqlConnection conn = new SqlConnection(constr))
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
                    SqlParameter returnParm2 = cmd.Parameters.Add("@tipe_pengguna", SqlDbType.VarChar);
                    returnParm2.Size = 100;
                    returnParm2.Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();

                    retval = returnParm1.Value.ToString();
                    tipe_pengguna = returnParm2.Value.ToString();

                    if (retval == "OK")
                    {
                        RefreshToken refreshToken = GenerateRefreshToken();
                        //Simpan Refresh Token di Database
                        SqlCommand cmdx = new SqlCommand("sp_SimpanRefreshToken", conn);
                        cmdx.CommandType = CommandType.StoredProcedure;
                        cmdx.Parameters.AddWithValue("@nama_pengguna", user.nama_pengguna);
                        cmdx.Parameters.AddWithValue("@token", refreshToken.token);
                        cmdx.Parameters.AddWithValue("@expired_date", refreshToken.expired_date);
                        cmdx.ExecuteNonQuery();

                        string finalToken = GenerateAccessToken(user.nama_pengguna, tipe_pengguna, exp_date);

                        respon.pesan = retval;
                        respon.nama_pengguna = user.nama_pengguna;
                        respon.access_token = finalToken;
                        respon.refresh_token = refreshToken.token;
                        respon.expire_in = exp_date;

                        return StatusCode(StatusCodes.Status200OK, respon);
                    }
                    else
                    {
                        respon.pesan = retval;
                        respon.nama_pengguna = user.nama_pengguna;
                        respon.access_token = null;
                        respon.refresh_token = null;
                        respon.expire_in = DateTime.Now;

                        return StatusCode(StatusCodes.Status404NotFound, respon);
                    }

                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
                finally { conn.Close(); }
            }

            //, respon);
            //return respon;
        }

        [HttpPost("tokenAPIBKN")]
        [Authorize]
        public async Task<ActionResult<ResponOtentikasi>> tokenAPIBKN()
        {
            ResponOtentikasi respon = new ResponOtentikasi();

            try
            {
                var options = new RestClientOptions("https://apimws.bkn.go.id")
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest("/oauth2/token", Method.Post);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", "Basic NTVwU3JTeVdDS1N1TFNxdG5OUWtJVm1WZ1cwYTp4dUs4U19iUzRmX2Y1RlFJTFpXbjAzV2Z2YUlh");
                request.AddHeader("Cookie", "ff8d625df24f2272ecde05bd53b814bc=e01e6fbb5ec7493a7de473cc51bc31de; pdns=1091068938.58148.0000");
                request.AddParameter("grant_type", "client_credentials");
                RestResponse response = await client.ExecuteAsync(request);
                Console.WriteLine(response.Content);

                dynamic results = JsonConvert.DeserializeObject<dynamic>(response.Content);

                respon.pesan = "Berhasil";
                respon.nama_pengguna = "";
                respon.access_token = results.access_token;
                respon.refresh_token = "";
                respon.expire_in = DateTime.UtcNow.AddSeconds(3600);

                return StatusCode(StatusCodes.Status200OK, respon);
            }
            catch(Exception ex)
            {
                respon.pesan = "Gagal";
                respon.nama_pengguna = "";
                respon.access_token = "";
                respon.refresh_token = "";
                respon.expire_in = DateTime.Now;

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            } 
        }

        [HttpPost("tokenAksesBKN")]
        [Authorize]
        public async Task<ActionResult<ResponOtentikasi>> tokenAksesBKN()
        {
            ResponOtentikasi respon = new ResponOtentikasi();

            try
            {
                var options = new RestClientOptions("https://sso-siasn.bkn.go.id")
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest("/auth/realms/public-siasn/protocol/openid-connect/token", Method.Post);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Cookie", "SERVERID=keycloak-01|ZSYm4|ZSYm4");
                request.AddParameter("client_id", "malukuprov");
                request.AddParameter("grant_type", "password");
                request.AddParameter("username", "198708062010011003");
                request.AddParameter("password", "Rivalsahib7");
                RestResponse response = await client.ExecuteAsync(request);
                Console.WriteLine(response.Content);

                dynamic results = JsonConvert.DeserializeObject<dynamic>(response.Content);

                respon.pesan = "Berhasil";
                respon.nama_pengguna = "";
                respon.access_token = results.access_token;
                respon.refresh_token = results.refresh_token;
                respon.expire_in = DateTime.UtcNow.AddSeconds(43200);

                return StatusCode(StatusCodes.Status200OK, respon);
            }
            catch (Exception ex)
            {
                respon.pesan = "Gagal";
                respon.nama_pengguna = "";
                respon.access_token = "";
                respon.refresh_token = "";
                respon.expire_in = DateTime.Now;

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPost("refreshtoken")]
        public async Task<ActionResult<ResponOtentikasi>> refreshtoken([FromBody] RefreshRequest request)
        {
            DateTime exp_date = DateTime.UtcNow.AddMinutes(10);

            ResponOtentikasi respon = new ResponOtentikasi();
            UserFromAccessToken _user = new UserFromAccessToken();
            _user = GetUserFromAccessToken(request.access_token);

            //if (ValidateRefreshToken(_user, request.refresh_token))
            //{
            //    respon.access_token = "asdasd";
            //}
            //else
            //{
            //    respon.access_token = "ddddddddd";
            //}

            if (_user != null && ValidateRefreshToken(_user, request.refresh_token))
            {
                respon.pesan = "Token Berhasil Digenerate";
                respon.nama_pengguna = _user.nama_pengguna;
                respon.expire_in = exp_date;
                respon.access_token = GenerateAccessToken(_user.nama_pengguna, _user.type_pengguna, exp_date);
                respon.refresh_token = request.refresh_token;
            }
            else
            {
                respon.pesan = "Token Gagal Digenerate";
                respon.nama_pengguna = _user.nama_pengguna;
                respon.expire_in = exp_date;
                respon.access_token = null;
                respon.refresh_token = null;
            }

            return respon;
        }

        private bool ValidateRefreshToken(UserFromAccessToken user, string refreshToken)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmdx = new SqlCommand("SELECT * FROM tbl_Data_Refresh_Token WHERE nama_pengguna = @nama_pengguna AND token = @token AND expired_date > GETDATE()", conn);
                    cmdx.CommandType = CommandType.Text;
                    cmdx.Parameters.AddWithValue("@nama_pengguna", user.nama_pengguna);
                    cmdx.Parameters.AddWithValue("@token", refreshToken);
                    SqlDataReader rdr = cmdx.ExecuteReader();
                    if (rdr.Read())
                    {
                        return true;
                    }
                    rdr.Close();
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally { conn.Close(); }
            }
            return false;
        }


        //[HttpPost("test")]
        //public async Task<ActionResult<ResponOtentikasi>> coba([FromBody] Coba coba)
        //{
        //    ResponOtentikasi respon = new ResponOtentikasi();

        //    using (SqlConnection conn = new SqlConnection(constr))
        //    {
        //        try
        //        {
        //            conn.Open();
        //            SqlCommand cmdx = new SqlCommand("SELECT * FROM tbl_Data_Refresh_Token WHERE nama_pengguna = @nama_pengguna AND token = @token AND expired_date > GETDATE()", conn);
        //            cmdx.CommandType = CommandType.Text;
        //            cmdx.Parameters.AddWithValue("@nama_pengguna", coba.user);
        //            cmdx.Parameters.AddWithValue("@token", coba.refreshToken);
        //            SqlDataReader rdr = cmdx.ExecuteReader();
        //            if (rdr.Read())
        //            {
        //                respon.access_token = "aaaaaaaaaa";
        //                return respon;
        //            }
        //            rdr.Close();
        //        }
        //        catch (Exception ex)
        //        {
        //            respon.access_token = ex.Message;
        //            return respon;
        //        }
        //        finally { conn.Close(); }
        //    }
        //    respon.access_token = "ccccccccccc";
        //    return respon;
        //}

        //[HttpPost("coba")]
        //public async Task<ActionResult<UserFromAccessToken>> coba([FromBody] RefreshRequest request)
        //{
        //    UserFromAccessToken user = new UserFromAccessToken();
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var tokenKey = Encoding.UTF8.GetBytes(setting.securitykey);

        //    var TokenValidationParameters = new TokenValidationParameters()
        //    {
        //        ValidateIssuerSigningKey = true,
        //        IssuerSigningKey = new SymmetricSecurityKey(tokenKey),
        //        ValidateIssuer = false,
        //        ValidateAudience = false,
        //        ValidateLifetime = true,
        //        ClockSkew = TimeSpan.Zero
        //    };

        //    SecurityToken securityToken;
        //    var principle = tokenHandler.ValidateToken(request.access_token, TokenValidationParameters, out securityToken);
        //    JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;

        //    //if (jwtSecurityToken != null && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        //    //{
        //    //    user.nama_pengguna = principle.FindFirst(ClaimTypes.Name)?.Value;
        //    //    user.type_pengguna = principle.FindFirst(ClaimTypes.Role)?.Value;

        //    //    return user;
        //    //}
        //    //else
        //    //{
        //    //    return null;
        //    //}
        //    if (jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase))
        //    {
        //        //access_token.nama_pengguna = access_token;
        //        //user.type_pengguna = "ssssssss";
        //        user.nama_pengguna = principle.FindFirst(ClaimTypes.Name)?.Value;
        //        user.type_pengguna = principle.FindFirst(ClaimTypes.Role)?.Value;
        //        return Ok(user);
        //    }
        //    else
        //    {
        //        //user.nama_pengguna = "asdsd";
        //        user.type_pengguna = "dfdfdf";
        //        return NotFound(user);
        //    }
        //}

        private UserFromAccessToken GetUserFromAccessToken(string access_token)
        {
            UserFromAccessToken user = new UserFromAccessToken();
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(setting.securitykey);

            var TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(tokenKey),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            SecurityToken securityToken;
            var principle = tokenHandler.ValidateToken(access_token, TokenValidationParameters, out securityToken);
            JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken != null && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase))
            {
                user.nama_pengguna = principle.FindFirst(ClaimTypes.Name)?.Value;
                user.type_pengguna = principle.FindFirst(ClaimTypes.Role)?.Value;

                return user;
            }
            return null;
        }

        private RefreshToken GenerateRefreshToken()
        {
            RefreshToken refreshToken = new RefreshToken();

            var randomNumber = new Byte[32];
            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                randomNumberGenerator.GetBytes(randomNumber);

                refreshToken.token = Convert.ToBase64String(randomNumber);
            }

            refreshToken.expired_date = DateTime.UtcNow.AddDays(7);

            return refreshToken;
        }

        private string GenerateAccessToken(string nama_pengguna, string tipe_pengguna, DateTime exp_date)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(setting.securitykey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name, Convert.ToString(nama_pengguna)),
                        new Claim(ClaimTypes.Role, Convert.ToString(tipe_pengguna))
                    }
                ),
                Expires = exp_date,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha512)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string finaltoken = tokenHandler.WriteToken(token);

            return finaltoken;
        }
    }
}
