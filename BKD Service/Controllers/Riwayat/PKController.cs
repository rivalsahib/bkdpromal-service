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
    public class PKController : Controller
    {
        public IConfiguration Configuration { get; }
        string constr = Startup.ConnectionStringSimpeg;

        [HttpPost(Name = "SimpanRiwayatPK")]
        public async Task<ActionResult<Respon>> simpan([FromBody] PK riwayat)
        {
            string retval = "";
            int i = 0;
            Respon respon = new Respon();

            try
            {
                //Simpan Riwayat PK
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_SimpanRiwayatPK", conn);
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.AddWithValue("@id", riwayat.id);
                    cmd.Parameters.AddWithValue("@id_orang", "");
                    cmd.Parameters.AddWithValue("@nipbaru", riwayat.nipbaru);
                    cmd.Parameters.AddWithValue("@nama", "");
                    cmd.Parameters.AddWithValue("@jenis_jabatan", riwayat.jenis_jabatan);
                    cmd.Parameters.AddWithValue("@jenis_formasi", riwayat.jenis_formasi);
                    cmd.Parameters.AddWithValue("@kd_tingkat_pendidikan", riwayat.kd_tingkat_pendidikan);
                    cmd.Parameters.AddWithValue("@tingkat_pendidikan", riwayat.tingkat_pendidikan);
                    cmd.Parameters.AddWithValue("@kd_pendidikan", riwayat.kd_pendidikan);
                    cmd.Parameters.AddWithValue("@pendidikan", riwayat.pendidikan);
                    cmd.Parameters.AddWithValue("@kd_jabatan", riwayat.kd_jabatan);
                    cmd.Parameters.AddWithValue("@jabatan", riwayat.jabatan);
                    cmd.Parameters.AddWithValue("@id_satker", riwayat.id_satker);
                    cmd.Parameters.AddWithValue("@satker", riwayat.satker);
                    cmd.Parameters.AddWithValue("@unker", riwayat.unker);
                    cmd.Parameters.AddWithValue("@id_lokasi_kerja", riwayat.id_lokasi_kerja);
                    cmd.Parameters.AddWithValue("@lokasi_kerja", riwayat.lokasi_kerja);
                    cmd.Parameters.AddWithValue("@rencana_perjanjian_kontrak_kerja", Convert.ToDateTime(riwayat.rencana_perjanjian_kontrak_kerja));
                    cmd.Parameters.AddWithValue("@tanggal_awal", Convert.ToDateTime(riwayat.tanggal_awal));
                    cmd.Parameters.AddWithValue("@tanggal_akhir", Convert.ToDateTime(riwayat.tanggal_akhir));
                    cmd.Parameters.AddWithValue("@gol_ruang_awal", riwayat.gol_ruang_awal);
                    cmd.Parameters.AddWithValue("@tahun_gaji", riwayat.tahun_gaji);
                    cmd.Parameters.AddWithValue("@gaji", riwayat.gaji);
                    cmd.Parameters.AddWithValue("@path_pk", riwayat.path_pk);

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

        [HttpPost("hapus/{id}", Name = "HapusRiwayatPK")]
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
                    SqlCommand cmd = new SqlCommand("sp_HapusRiwayatPK", conn);
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
