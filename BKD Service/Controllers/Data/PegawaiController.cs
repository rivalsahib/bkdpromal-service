using BKD_API.Models;
using BKD_Service.Model.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BKD_Service.Controllers.Data
{
    [Route("api/data/[controller]")]
    [ApiController]
    [Authorize]
    public class PegawaiController : Controller
    {
        public IConfiguration Configuration { get; }
        string constrSimpeg = Startup.ConnectionStringSimpeg;
        [HttpGet("{nip}", Name = "GetPegawaiByNip")]
        public async Task<ActionResult<Respon>> GetByNip(string nip)
        {
            Respon respon = new Respon();
            Pegawai pegawai = new Pegawai();

            using (SqlConnection conn = new SqlConnection(constrSimpeg))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_TampilDataPegawaiByNIP", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nip", nip);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            pegawai.id = rdr.GetInt32("id");
                            pegawai.id_bkn = rdr.GetString("id_bkn").ToString();
                            pegawai.nip = rdr.GetString("nip").ToString();
                            pegawai.gelar_depan = rdr.GetString("gd").ToString();
                            pegawai.nama = rdr.GetString("nama").ToString();
                            pegawai.gelar_belakang = rdr.GetString("gb").ToString();
                            pegawai.tgl_lahir = rdr.GetString("tgl_lahir").ToString();
                            pegawai.email = rdr.GetString("email_pribadi").ToString();
                            pegawai.nomor_telp = rdr.GetString("nomor_hp").ToString();
                            pegawai.pangkat = rdr.GetString("pangkat").ToString();
                            pegawai.jabatan = rdr.GetString("jabatan").ToString();
                            pegawai.id_satker = rdr.GetString("id_satker").ToString();
                            pegawai.satker = rdr.GetString("satker").ToString();
                            pegawai.id_unit_kerja = rdr.GetString("kelompok_unker").ToString();
                            pegawai.unit_kerja = rdr.GetString("unit_kerja").ToString();
                            pegawai.foto = "https://simpeg.malukuprov.go.id/img/foto/" + rdr.GetString("nip").ToString() + ".jpg";

                            respon.pesan = "Data Berhasil Ditemukan";
                            respon.data = pegawai;
                            return StatusCode(StatusCodes.Status200OK, respon);
                        }
                        else
                        {
                            respon.pesan = "Data Tidak Ditemukan";
                            respon.data = null;
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

        [HttpGet(Name = "GetPegawaiAll")]
        public async Task<ActionResult<Respon>> GetAll()
        {
            Respon respon = new Respon();
            List<Pegawai> daftar_pegawai = new List<Pegawai>();

            using (SqlConnection conn = new SqlConnection(constrSimpeg))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_TampilDataPegawaiAll", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            daftar_pegawai.Add(new Pegawai()
                            {
                                id = rdr.GetInt32("id"),
                                id_bkn = rdr.GetString("id_bkn").ToString(),
                                nip = rdr.GetString("nip").ToString(),
                                gelar_depan = rdr.GetString("gd").ToString(),
                                nama = rdr.GetString("nama").ToString(),
                                gelar_belakang = rdr.GetString("gb").ToString(),
                                tgl_lahir = rdr.GetString("tgl_lahir").ToString(),
                                email = rdr.GetString("email_pribadi").ToString(),
                                nomor_telp = rdr.GetString("nomor_hp").ToString(),
                                pangkat = rdr.GetString("pangkat").ToString(),
                                jabatan = rdr.GetString("jabatan").ToString(),
                                id_satker = rdr.GetString("id_satker").ToString(),
                                satker = rdr.GetString("satker").ToString(),
                                id_unit_kerja = rdr.GetString("kelompok_unker").ToString(),
                                unit_kerja = rdr.GetString("unit_kerja").ToString(),
                                foto = "https://simpeg.malukuprov.go.id/img/foto/" + rdr.GetString("nip").ToString() + ".jpg"
                            });
                        }
                        respon.pesan = "Data Berhasil Ditemukan";
                        respon.data = daftar_pegawai;
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
