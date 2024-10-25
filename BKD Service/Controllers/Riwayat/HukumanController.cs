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
    public class HukumanController : Controller
    {
        public IConfiguration Configuration { get; }
        string constr = Startup.ConnectionStringSimpeg;

        [HttpPost(Name = "SimpanRiwayatHukuman")]
        public async Task<ActionResult<Respon>> simpan([FromBody] Hukuman riwayat)
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
                    SqlCommand cmd = new SqlCommand("sp_SimpanRiwayatHukuman", conn);
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.AddWithValue("@id", riwayat.id);
                    cmd.Parameters.AddWithValue("@id_bkn", riwayat.id_bkn);
                    cmd.Parameters.AddWithValue("@kategori", riwayat.kategori);
                    cmd.Parameters.AddWithValue("@pns_orang_id", riwayat.pns_orang_id);
                    cmd.Parameters.AddWithValue("@pns_orang_nip", riwayat.pns_orang_nip);
                    cmd.Parameters.AddWithValue("@jenis_tingkat_hukuman_id", riwayat.jenis_tingkat_hukuman_id);
                    cmd.Parameters.AddWithValue("@kedudukan_hukum", riwayat.kedudukan_hukum);
                    cmd.Parameters.AddWithValue("@golongan_id", riwayat.golongan_id);
                    cmd.Parameters.AddWithValue("@golongan_lama_id", riwayat.golongan_lama_id);
                    cmd.Parameters.AddWithValue("@jenis_hukuman_id", riwayat.jenis_hukuman_id);
                    cmd.Parameters.AddWithValue("@jenis_hukuman_nama", riwayat.jenis_hukuman_nama);
                    cmd.Parameters.AddWithValue("@sk_nomor", riwayat.sk_nomor);
                    cmd.Parameters.AddWithValue("@sk_tanggal", Convert.ToDateTime(riwayat.sk_tanggal));
                    cmd.Parameters.AddWithValue("@hukuman_tanggal", Convert.ToDateTime(riwayat.hukuman_tanggal));
                    cmd.Parameters.AddWithValue("@masa_tahun", riwayat.masa_tahun);
                    cmd.Parameters.AddWithValue("@masa_bulan", riwayat.masa_bulan);
                    cmd.Parameters.AddWithValue("@akhir_hukum_tanggal", Convert.ToDateTime(riwayat.akhir_hukum_tanggal));
                    cmd.Parameters.AddWithValue("@nomor_pp", riwayat.nomor_pp);
                    cmd.Parameters.AddWithValue("@sk_pembatalan_nomor", riwayat.sk_pembatalan_nomor);
                    cmd.Parameters.AddWithValue("@sk_pembatalan_tanggal", Convert.ToDateTime(riwayat.sk_pembatalan_tanggal));
                    cmd.Parameters.AddWithValue("@ncsistime", Convert.ToDateTime(riwayat.ncsistime));
                    cmd.Parameters.AddWithValue("@alasan_hukuman", riwayat.alasan_hukuman);
                    cmd.Parameters.AddWithValue("@alasan_hukuman_nama", riwayat.alasan_hukuman_nama);
                    cmd.Parameters.AddWithValue("@rw_hukuman_disiplin", riwayat.rw_hukuman_disiplin);
                    cmd.Parameters.AddWithValue("@keterangan", riwayat.keterangan);
                    cmd.Parameters.AddWithValue("@path_sk_penetapan_hukuman", riwayat.path_sk_penetapan_hukuman);
                    cmd.Parameters.AddWithValue("@path_sk_pengaktifan_hukuman", riwayat.path_sk_pengaktifan_hukuman);

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

        [HttpPost("hapus/{id}", Name = "HapusRiwayatHukuman")]
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
                    SqlCommand cmd = new SqlCommand("sp_HapusRiwayatHukuman", conn);
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
