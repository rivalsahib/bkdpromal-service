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
    public class DiklatController : Controller
    {
        public IConfiguration Configuration { get; }
        string constr = Startup.ConnectionStringSimpeg;

        [HttpPost(Name = "SimpanRiwayatDiklat")]
        public async Task<ActionResult<Respon>> simpan([FromBody] Diklat riwayat)
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
                    SqlCommand cmd = new SqlCommand("sp_SimpanRiwayatDiklat", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", riwayat.id);
                    cmd.Parameters.AddWithValue("@id_bkn", riwayat.id_bkn);
                    cmd.Parameters.AddWithValue("@id_orang", riwayat.id_orang);
                    cmd.Parameters.AddWithValue("@nipbaru", riwayat.nipbaru);
                    cmd.Parameters.AddWithValue("@niplama", riwayat.niplama);
                    cmd.Parameters.AddWithValue("@kd_jenis_diklat", riwayat.kd_jenis_diklat);
                    cmd.Parameters.AddWithValue("@jenis_diklat", riwayat.jenis_diklat);
                    cmd.Parameters.AddWithValue("@kd_diklat", riwayat.kd_diklat);
                    cmd.Parameters.AddWithValue("@nama_diklat", riwayat.nama_diklat);
                    cmd.Parameters.AddWithValue("@jenis_kursus_sertifikat", riwayat.jenis_kursus_sertifikat);
                    cmd.Parameters.AddWithValue("@tempat_diklat", riwayat.tempat_diklat);
                    cmd.Parameters.AddWithValue("@penyelenggara", riwayat.penyelenggara);
                    cmd.Parameters.AddWithValue("@angkatan", riwayat.angkatan);
                    cmd.Parameters.AddWithValue("@jam", Convert.ToInt32(riwayat.jam));
                    cmd.Parameters.AddWithValue("@tgl_mulai", Convert.ToDateTime(riwayat.tgl_mulai));
                    cmd.Parameters.AddWithValue("@tgl_selesai", Convert.ToDateTime(riwayat.tgl_selesai));
                    cmd.Parameters.AddWithValue("@tahun", riwayat.tahun);
                    cmd.Parameters.AddWithValue("@no_sttpp", riwayat.no_sttpp);
                    cmd.Parameters.AddWithValue("@tgl_sttpp", Convert.ToDateTime(riwayat.tgl_sttpp));
                    cmd.Parameters.AddWithValue("@path", riwayat.path_sertifikat);

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

        [HttpPost("hapus/{id}", Name = "HapusRiwayatDiklat")]
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
                    SqlCommand cmd = new SqlCommand("sp_HapusRiwayatDiklat", conn);
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

        [HttpPost("unduh/{nip}", Name = "GetRiwayatDiklatUnduh")]
        public async Task<ActionResult<Respon>> Unduh([FromBody] List<DiklatBKN> riwayat, string nip)
        {
            string retval = "";
            int i = 0;
            Respon respon = new Respon();

            try
            {
                foreach (var data in riwayat)
                {
                    var pathObj = riwayat[i].path;

                    // Mengakses properti dok_uri dalam objek path



                    //Simpan Riwayat Pangkat
                    using (SqlConnection conn = new SqlConnection(constr))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("sp_SimpanRiwayatDiklatUnduh", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", 0);
                        cmd.Parameters.AddWithValue("@id_bkn", riwayat[i].id);
                        cmd.Parameters.AddWithValue("@id_orang", riwayat[i].idPns);
                        cmd.Parameters.AddWithValue("@nipbaru", riwayat[i].nipBaru);
                        cmd.Parameters.AddWithValue("@niplama", riwayat[i].nipLama);
                        cmd.Parameters.AddWithValue("@kd_jenis_diklat", "1");
                        cmd.Parameters.AddWithValue("@jenis_diklat", "Diklat Struktural");
                        cmd.Parameters.AddWithValue("@kd_diklat", riwayat[i].latihanStrukturalId);
                        cmd.Parameters.AddWithValue("@nama_diklat", riwayat[i].latihanStrukturalNama);
                        cmd.Parameters.AddWithValue("@jenis_kursus_sertifikat", "");
                        cmd.Parameters.AddWithValue("@tempat_diklat", "");
                        cmd.Parameters.AddWithValue("@penyelenggara", riwayat[i].institusiPenyelenggara);
                        cmd.Parameters.AddWithValue("@angkatan", "");
                        cmd.Parameters.AddWithValue("@jam", "0");
                        cmd.Parameters.AddWithValue("@tahun", riwayat[i].tahun);
                        cmd.Parameters.AddWithValue("@no_sttpp", riwayat[i].nomor);

                        if (riwayat[i].tanggal != null)
                        {
                            string tanggal = riwayat[i].tanggal;
                            string tahun = tanggal.Substring(6, 4);
                            string bulan = tanggal.Substring(3, 2);
                            string hari = tanggal.Substring(0, 2);
                            cmd.Parameters.AddWithValue("@tgl_mulai", Convert.ToDateTime(tahun + "/" + bulan + "/" + hari));
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@tgl_mulai", "1900/01/01");
                        }

                        if (riwayat[i].tanggalSelesai != null)
                        {
                            string tanggalSelesai = riwayat[i].tanggalSelesai;
                            string tahun = tanggalSelesai.Substring(6, 4);
                            string bulan = tanggalSelesai.Substring(3, 2);
                            string hari = tanggalSelesai.Substring(0, 2);
                            cmd.Parameters.AddWithValue("@tgl_selesai", Convert.ToDateTime(tahun + "/" + bulan + "/" + hari));
                            cmd.Parameters.AddWithValue("@tgl_sttpp", Convert.ToDateTime(tahun + "/" + bulan + "/" + hari));
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@tgl_selesai", "1900/01/01");
                            cmd.Parameters.AddWithValue("@tgl_sttpp", "1900/01/01");
                        }



                        if (riwayat[i].path != null && riwayat[i].path.ContainsKey("874"))
                        {
                            string dokUri = riwayat[i].path["874"].dok_uri;
                            if (dokUri != null)
                            {
                                cmd.Parameters.AddWithValue("@path", dokUri);
                            }
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@path", "");
                        }


                        SqlParameter returnParm1 = cmd.Parameters.Add("@return_val", SqlDbType.VarChar);
                        returnParm1.Size = 100;
                        returnParm1.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                        retval = Convert.ToString(cmd.Parameters["@return_val"].Value);
                        conn.Close();
                        i++;
                    }
                }

                respon.pesan = "Data Berhasil Ditemukan";
                respon.data = retval;
                return StatusCode(StatusCodes.Status200OK, respon);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
