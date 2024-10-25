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
    public class SKPController : Controller
    {
        public IConfiguration Configuration { get; }
        string constr = Startup.ConnectionStringSimpeg;

        [HttpPost(Name = "SimpanRiwayatSKP")]
        public async Task<ActionResult<Respon>> simpan([FromBody] SKP riwayat)
        {
            string retval = "";
            int i = 0;
            Respon respon = new Respon();

            try
            {
                //Simpan Riwayat SKP
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_SimpanRiwayatSKP", conn);
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.AddWithValue("@Id", riwayat.Id);
                    cmd.Parameters.AddWithValue("@IdBkn", riwayat.IdBkn);
                    cmd.Parameters.AddWithValue("@IdOrang", riwayat.IdOrang);
                    cmd.Parameters.AddWithValue("@NipBaru", riwayat.NipBaru);
                    cmd.Parameters.AddWithValue("@NipLama", riwayat.NipLama);
                    cmd.Parameters.AddWithValue("@Tahun", riwayat.Tahun);
                    cmd.Parameters.AddWithValue("@Jenjab", riwayat.Jenjab);
                    cmd.Parameters.AddWithValue("@JenisPeraturanKinerjaKd", riwayat.JenisPeraturanKinerjaKd);
                    cmd.Parameters.AddWithValue("@Pelayanan", riwayat.Pelayanan);
                    cmd.Parameters.AddWithValue("@Komitmen", riwayat.Komitmen);
                    cmd.Parameters.AddWithValue("@Kerjasama", riwayat.Kerjasama);
                    cmd.Parameters.AddWithValue("@InisiatifKerja", riwayat.InisiatifKerja);
                    cmd.Parameters.AddWithValue("@Integritas", riwayat.Integritas);
                    cmd.Parameters.AddWithValue("@Disiplin", riwayat.Disiplin);
                    cmd.Parameters.AddWithValue("@Kepemimpinan", riwayat.Kepemimpinan);
                    cmd.Parameters.AddWithValue("@NilaiPerilakuKerja", riwayat.NilaiPerilakuKerja);
                    cmd.Parameters.AddWithValue("@nilaiPerilakuKerjaRataRata", riwayat.NilaiPerilakuKerjaRataRata);
                    cmd.Parameters.AddWithValue("@NilaiPrestasiKerja", riwayat.NilaiPrestasiKerja);
                    cmd.Parameters.AddWithValue("@NilaiSkp", riwayat.NilaiSkp);                 
                    cmd.Parameters.AddWithValue("@KonversiNilai", riwayat.KonversiNilai);
                    cmd.Parameters.AddWithValue("@NilaiIntegrasi", riwayat.NilaiIntegrasi);
                    cmd.Parameters.AddWithValue("@Jumlah", riwayat.Jumlah);
                    cmd.Parameters.AddWithValue("@PenilaiIdOrang", riwayat.PenilaiIdOrang);
                    cmd.Parameters.AddWithValue("@PenilaiNonPns", riwayat.PenilaiNonPns);
                    cmd.Parameters.AddWithValue("@PenilaiNipNrp", riwayat.PenilaiNipNrp);
                    cmd.Parameters.AddWithValue("@PenilaiNama", riwayat.PenilaiNama);
                    cmd.Parameters.AddWithValue("@PenilaiGolongan", riwayat.PenilaiGolongan);
                    cmd.Parameters.AddWithValue("@PenilaiTmtGolongan", riwayat.PenilaiTmtGolongan);
                    cmd.Parameters.AddWithValue("@PenilaiJabatan", riwayat.PenilaiJabatan);
                    cmd.Parameters.AddWithValue("@PenilaiUnorNama", riwayat.PenilaiUnorNama);
                    cmd.Parameters.AddWithValue("@PenilaiStatus", riwayat.PenilaiStatus);
                    cmd.Parameters.AddWithValue("@AtasanPenilaiIdOrang", riwayat.AtasanPenilaiIdOrang);
                    cmd.Parameters.AddWithValue("@AtasanPenilaiNonPns", riwayat.AtasanPenilaiNonPns);
                    cmd.Parameters.AddWithValue("@AtasanPenilaiNipNrp", riwayat.AtasanPenilaiNipNrp);
                    cmd.Parameters.AddWithValue("@AtasanPenilaiNama", riwayat.AtasanPenilaiNama);
                    cmd.Parameters.AddWithValue("@AtasanPenilaiGolongan", riwayat.AtasanPenilaiGolongan);
                    cmd.Parameters.AddWithValue("@AtasanPenilaiTmtGolongan", riwayat.AtasanPenilaiTmtGolongan);
                    cmd.Parameters.AddWithValue("@AtasanPenilaiJabatan", riwayat.AtasanPenilaiJabatan);
                    cmd.Parameters.AddWithValue("@AtasanPenilaiUnorNama", riwayat.AtasanPenilaiUnorNama);
                    cmd.Parameters.AddWithValue("@AtasanPenilaiStatus", riwayat.AtasanPenilaiStatus);
                    cmd.Parameters.AddWithValue("@path", riwayat.path);
                    cmd.Parameters.AddWithValue("@StatusVerifikasi", riwayat.StatusVerifikasi);
                    cmd.Parameters.AddWithValue("@PesanBKD", riwayat.PesanBKD);
                    cmd.Parameters.AddWithValue("@PesanOPD", riwayat.PesanOPD);

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

        [HttpPost("hapus/{id}", Name = "HapusRiwayatSKP")]
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
                    SqlCommand cmd = new SqlCommand("sp_HapusRiwayatSKP", conn);
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
