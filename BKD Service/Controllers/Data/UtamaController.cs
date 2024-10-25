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

namespace BKD_Service.Controllers.Data
{
    [Route("api/data/[controller]")]
    [ApiController]
    [Authorize]
    public class UtamaController : Controller
    {
        public IConfiguration Configuration { get; }
        string constr = Startup.ConnectionStringSimpeg;

        [HttpPost(Name = "SimpanDataUtama")]
        public async Task<ActionResult<Respon>> simpan([FromBody] Utama data)
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
                    SqlCommand cmd = new SqlCommand("sp_SimpanDataPegawai", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", data.id);
                    cmd.Parameters.AddWithValue("@id_orang", data.id_orang);
                    cmd.Parameters.AddWithValue("@nipbaru", data.nipbaru);
                    cmd.Parameters.AddWithValue("@niplama", data.niplama);
                    cmd.Parameters.AddWithValue("@nama", data.nama);
                    cmd.Parameters.AddWithValue("@id_t4lahir", data.id_t4lahir);
                    cmd.Parameters.AddWithValue("@t4lahir", data.t4lahir);
                    cmd.Parameters.AddWithValue("@tgl_lahir", Convert.ToDateTime(data.tgl_lahir));
                    cmd.Parameters.AddWithValue("@jkel", data.jkel);
                    cmd.Parameters.AddWithValue("@id_agama", data.id_agama);
                    cmd.Parameters.AddWithValue("@agama", data.agama);
                    cmd.Parameters.AddWithValue("@goldarah", data.goldarah);
                    cmd.Parameters.AddWithValue("@asal_pengangkatan", data.asal_pengangkatan);
                    cmd.Parameters.AddWithValue("@kd_status_kepegawaian", data.kd_status_kepegawaian);
                    cmd.Parameters.AddWithValue("@status_kepegawaian", data.status_kepegawaian);
                    cmd.Parameters.AddWithValue("@kd_jenis_kepegawaian", data.kd_jenis_kepegawaian);
                    cmd.Parameters.AddWithValue("@jenis_kepegawaian", data.jenis_kepegawaian);
                    cmd.Parameters.AddWithValue("@kd_kedudukan_kepegawaian", data.kd_kedudukan_kepegawaian);
                    cmd.Parameters.AddWithValue("@kedudukan_kepegawaian", data.kedudukan_kepegawaian);
                    cmd.Parameters.AddWithValue("@id_status", data.id_status);
                    cmd.Parameters.AddWithValue("@status", data.status);
                    cmd.Parameters.AddWithValue("@no_ktp", data.no_ktp);
                    cmd.Parameters.AddWithValue("@no_akte_lahir", data.no_akte_lahir);
                    cmd.Parameters.AddWithValue("@no_SK_konversi_NIP", data.no_SK_konversi_NIP);
                    cmd.Parameters.AddWithValue("@alamat", data.alamat);
                    cmd.Parameters.AddWithValue("@kode_pos", data.kode_pos);
                    cmd.Parameters.AddWithValue("@kelurahan", data.kelurahan);
                    cmd.Parameters.AddWithValue("@kecamatan", data.kecamatan);
                    cmd.Parameters.AddWithValue("@kota", data.kota);
                    cmd.Parameters.AddWithValue("@provinsi", data.provinsi);
                    cmd.Parameters.AddWithValue("@nomor_telp", data.nomor_telp);
                    cmd.Parameters.AddWithValue("@nomor_hp", data.nomor_hp);
                    cmd.Parameters.AddWithValue("@email_pribadi", data.email_pribadi);
                    cmd.Parameters.AddWithValue("@email_dinas", data.email_dinas);
                    cmd.Parameters.AddWithValue("@nomor_kartu_asn", data.nomor_kartu_asn);
                    cmd.Parameters.AddWithValue("@nomor_karpeg", data.nomor_karpeg);
                    cmd.Parameters.AddWithValue("@nomor_kartu_askes", data.nomor_kartu_askes);
                    cmd.Parameters.AddWithValue("@nomor_kartu_taspen", data.nomor_kartu_taspen);
                    cmd.Parameters.AddWithValue("@npwp", data.npwp);
                    cmd.Parameters.AddWithValue("@id_lokasi_kerja", data.id_lokasi_kerja);
                    cmd.Parameters.AddWithValue("@lokasi_kerja", data.lokasi_kerja);

                    SqlParameter returnParm1 = cmd.Parameters.Add("@return_val", SqlDbType.VarChar);
                    returnParm1.Size = 100;
                    returnParm1.Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    retval = Convert.ToString(cmd.Parameters["@return_val"].Value);
                    conn.Close();
                }


                respon.pesan = retval;
                respon.data = data;
                return StatusCode(StatusCodes.Status200OK, respon);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
