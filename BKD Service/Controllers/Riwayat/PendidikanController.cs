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

namespace BKD_Service.Controllers.riwayat
{
    [Route("api/riwayat/[controller]")]
    [ApiController]
    [Authorize]
    public class PendidikanController : Controller
    {
        public IConfiguration Configuration { get; }
        string constr = Startup.ConnectionStringSimpeg;

        [HttpGet("{nip}", Name = "GetRiwayatPendidikan")]
        public async Task<ActionResult<Respon>> GetByNip(string nip)
        {
            string retval;
            string url = "/apisiasn/1.0/pns/rw-pendidikan/" + nip;
            Respon respon = new Respon();
            List<Pendidikan> riwayat_pendidikan = new List<Pendidikan>();
            int i = 0;

            try
            {
                //Get Riwayat Pendidikan
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_TampilRiwayatPendidikan", conn);
                    cmd.Parameters.AddWithValue("@nipbaru", nip);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Pendidikan pendidikan = new Pendidikan();
                        pendidikan.id = Convert.ToInt32(rdr["id"].ToString());
                        pendidikan.id_bkn = rdr["id"].ToString();
                        pendidikan.id_orang = rdr["idPns"].ToString();
                        pendidikan.nipbaru = rdr["nipbaru"].ToString();
                        pendidikan.niplama = rdr["niplama"].ToString();
                        pendidikan.id_pendidikan_bkn = rdr["id_pendidikan_bkn"].ToString();
                        pendidikan.kd_pendidikan = rdr["kd_pendidikan"].ToString();
                        pendidikan.nama_pendidikan = rdr["nama_pendidikan"].ToString();
                        pendidikan.jurusan = rdr["jurusan"].ToString();
                        pendidikan.id_tingkat_pendidikan_bkn = rdr["id_tingkat_pendidikan_bkn"].ToString();
                        pendidikan.kd_tingkat_pendidikan = rdr["kd_tingkat_pendidikan"].ToString();
                        pendidikan.nama_tingkat_pendidikan = rdr["nama_tingkat_pendidikan"].ToString();
                        pendidikan.tahun_lulus = rdr["tahun"].ToString();
                        pendidikan.tgl_lulus = rdr["tgl_sttb"].ToString();
                        pendidikan.isPendidikanPertama = rdr["pendidikan_pertama"].ToString();
                        pendidikan.nama_kepsek_rektor = rdr["nama_kepsek_rektor"].ToString();
                        pendidikan.nama_sekolah = rdr["nama_sekolah"].ToString();
                        pendidikan.nomor_ijazah = rdr["no_sttb"].ToString();
                        pendidikan.tempat_sekolah = rdr["tempat"].ToString();
                        pendidikan.gd = rdr["gd"].ToString();
                        pendidikan.gb = rdr["gb"].ToString();
                        pendidikan.path_ijazah = rdr["path_ijazah"].ToString();
                        pendidikan.path_transkrip = rdr["path_transkrip"].ToString();
                        riwayat_pendidikan.Add(pendidikan);
                    }
                    conn.Close();
                }


                respon.pesan = "Data Berhasil Ditemukan";
                respon.data = riwayat_pendidikan;
                return StatusCode(StatusCodes.Status200OK, respon);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPost(Name = "SimpanRiwayatPendidikan")]
        public async Task<ActionResult<Respon>> simpan([FromBody] Pendidikan riwayat)
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
                    SqlCommand cmd = new SqlCommand("sp_SimpanRiwayatPendidikan", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", riwayat.id);
                    cmd.Parameters.AddWithValue("@id_bkn", riwayat.id_bkn);
                    cmd.Parameters.AddWithValue("@id_orang", riwayat.id_orang);
                    cmd.Parameters.AddWithValue("@nipbaru", riwayat.nipbaru);
                    cmd.Parameters.AddWithValue("@niplama", riwayat.niplama);
                    cmd.Parameters.AddWithValue("@kd_pendidikan_bkn", riwayat.id_pendidikan_bkn);
                    cmd.Parameters.AddWithValue("@kd_pendidikan", riwayat.kd_pendidikan);
                    cmd.Parameters.AddWithValue("@nama_pendidikan", riwayat.nama_pendidikan);
                    cmd.Parameters.AddWithValue("@kd_tingkat_Pendidikan_bkn", riwayat.id_tingkat_pendidikan_bkn);
                    cmd.Parameters.AddWithValue("@kd_tingkat_Pendidikan", riwayat.kd_tingkat_pendidikan);
                    cmd.Parameters.AddWithValue("@nama_tingkat_pendidikan", riwayat.nama_tingkat_pendidikan);
                    cmd.Parameters.AddWithValue("@jurusan", riwayat.jurusan);
                    cmd.Parameters.AddWithValue("@tempat", riwayat.tempat_sekolah);
                    cmd.Parameters.AddWithValue("@tahun_lulus", riwayat.tahun_lulus);
                    cmd.Parameters.AddWithValue("@nama_kepsek_rektor", riwayat.nama_kepsek_rektor);
                    cmd.Parameters.AddWithValue("@is_pendidikan_pertama", riwayat.isPendidikanPertama);
                    cmd.Parameters.AddWithValue("@nomor_ijazah", riwayat.nomor_ijazah);
                    cmd.Parameters.AddWithValue("@nama_sekolah", riwayat.nama_sekolah);
                    cmd.Parameters.AddWithValue("@tgl_lulus", riwayat.tgl_lulus);
                    cmd.Parameters.AddWithValue("@gd", riwayat.gd);
                    cmd.Parameters.AddWithValue("@gb", riwayat.gb);
                    cmd.Parameters.AddWithValue("@path_ijazah", riwayat.path_ijazah);
                    cmd.Parameters.AddWithValue("@path_transkrip", riwayat.path_transkrip);

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

        [HttpPost("hapus/{id}", Name = "HapusRiwayatPendidikan")]
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
                    SqlCommand cmd = new SqlCommand("sp_HapusRiwayatPendidikan", conn);
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

        [HttpPost("sinkron/{nip}", Name = "GetRiwayatPendidikanUnduh")]
        public async Task<ActionResult<Respon>> unduh([FromBody] List<PendidikanBKN> riwayat, string nip)
        {
            string retval = "";
            int i = 0;
            Respon respon = new Respon();

            try
            {

                foreach (var data in riwayat)
                {
                    var pathObj = riwayat[i].path;

                //Simpan Riwayat Pendidikan
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_SimpanRiwayatPendidikanSinkron", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", 0);
                    cmd.Parameters.AddWithValue("@id_bkn", riwayat[i].id);
                    cmd.Parameters.AddWithValue("@id_orang", riwayat[i].idPns);
                    cmd.Parameters.AddWithValue("@nipbaru", riwayat[i].nipBaru);
                    cmd.Parameters.AddWithValue("@niplama", riwayat[i].nipLama);
                    cmd.Parameters.AddWithValue("@kd_pendidikan_bkn", riwayat[i].pendidikanId);
                    cmd.Parameters.AddWithValue("@kd_pendidikan", "");
                    cmd.Parameters.AddWithValue("@nama_pendidikan", riwayat[i].pendidikanNama);
                    cmd.Parameters.AddWithValue("@kd_tingkat_Pendidikan_bkn", riwayat[i].tkPendidikanId);
                    cmd.Parameters.AddWithValue("@kd_tingkat_Pendidikan", "");
                    cmd.Parameters.AddWithValue("@nama_tingkat_pendidikan", riwayat[i].tkPendidikanNama);
                    cmd.Parameters.AddWithValue("@jurusan", "");
                    cmd.Parameters.AddWithValue("@tempat", "");
                    cmd.Parameters.AddWithValue("@tahun_lulus", riwayat[i].tahunLulus);
                    cmd.Parameters.AddWithValue("@nama_kepsek_rektor", "");

                    if (riwayat[i].isPendidikanPertama != null)
                    {
                        cmd.Parameters.AddWithValue("@is_pendidikan_pertama", riwayat[i].isPendidikanPertama);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@is_pendidikan_pertama", "0");
                    }

                    if (riwayat[i].nomorIjasah != null)
                    {
                        cmd.Parameters.AddWithValue("@nomor_ijazah", riwayat[i].nomorIjasah);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@nomor_ijazah", "");
                    }

                    if (riwayat[i].namaSekolah != null)
                    {
                        cmd.Parameters.AddWithValue("@nama_sekolah", riwayat[i].namaSekolah);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@nama_sekolah", "");
                    }


                    if (riwayat[i].tglLulus != null)
                    {
                        if (riwayat[i].tglLulus == "")
                        {
                            cmd.Parameters.AddWithValue("@tgl_lulus", "1900/01/01");
                        }
                        else
                        {
                            string tglLulus = riwayat[i].tglLulus;
                            string tahun = tglLulus.Substring(6);
                            string bulan = tglLulus.Substring(3, 2);
                            string hari = tglLulus.Substring(0, 2);
                            cmd.Parameters.AddWithValue("@tgl_lulus", Convert.ToDateTime(tahun + "/" + bulan + "/" + hari));
                        }             
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@tgl_lulus", "1900/01/01");
                    }

                    if (riwayat[i].gelarDepan != null)
                    {
                        cmd.Parameters.AddWithValue("@gd", riwayat[i].gelarDepan);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@gd", "");
                    }

                    if (riwayat[i].gelarBelakang != null)
                    {
                        cmd.Parameters.AddWithValue("@gb", riwayat[i].gelarBelakang);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@gb", "");
                    }

                    if (riwayat[i].path != null)
                    {
                        if (riwayat[i].path["870"].dok_uri != null)
                        {
                            cmd.Parameters.AddWithValue("@path_ijazah", riwayat[i].path["870"].dok_uri);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@path_ijazah", "");
                        }

                        if (riwayat[i].path["871"].dok_uri != null)
                        {
                            cmd.Parameters.AddWithValue("@path_transkrip", riwayat[i].path["871"].dok_uri);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@path_transkrip", "");
                        }
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@path_ijazah", "");
                        cmd.Parameters.AddWithValue("@path_transkrip", "");
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
