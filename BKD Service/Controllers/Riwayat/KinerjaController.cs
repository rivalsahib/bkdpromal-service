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
    public class KinerjaController : Controller
    {
        public IConfiguration Configuration { get; }
        string constr = Startup.ConnectionStringSimpeg;

        [HttpPost(Name = "SimpanRiwayatKinerja")]
        public async Task<ActionResult<Respon>> simpan([FromBody] Kinerja riwayat)
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
                    SqlCommand cmd = new SqlCommand("sp_SimpanRiwayatKinerja", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", riwayat.id);
                    cmd.Parameters.AddWithValue("@id_bkn", riwayat.id_bkn);
                    cmd.Parameters.AddWithValue("@id_orang", riwayat.pnsDinilaiId);
                    cmd.Parameters.AddWithValue("@nipbaru", riwayat.nipbaru);
                    cmd.Parameters.AddWithValue("@niplama", riwayat.niplama);
                    cmd.Parameters.AddWithValue("@hasilKinerja", riwayat.hasilKinerja);
                    cmd.Parameters.AddWithValue("@hasilKinerjaNilai", riwayat.hasilKinerjaNilai);
                    cmd.Parameters.AddWithValue("@kuadranKinerja", riwayat.kuadranKinerja);
                    cmd.Parameters.AddWithValue("@KuadranKinerjaNilai",riwayat.KuadranKinerjaNilai);
                    cmd.Parameters.AddWithValue("@namaPenilai", riwayat.namaPenilai);
                    cmd.Parameters.AddWithValue("@nipNrpPenilai", riwayat.nipNrpPenilai);
                    cmd.Parameters.AddWithValue("@penilaiGolonganId", riwayat.penilaiGolonganId);
                    cmd.Parameters.AddWithValue("@penilaiJabatanNm", riwayat.penilaiJabatanNm);
                    cmd.Parameters.AddWithValue("@penilaiUnorNm", riwayat.penilaiUnorNm);
                    cmd.Parameters.AddWithValue("@perilakuKerja", riwayat.perilakuKerja);
                    cmd.Parameters.AddWithValue("@PerilakuKerjaNilai", riwayat.PerilakuKerjaNilai);
                    cmd.Parameters.AddWithValue("@statusPenilai", riwayat.statusPenilai);
                    cmd.Parameters.AddWithValue("@tahun", riwayat.tahun);
                    cmd.Parameters.AddWithValue("@path", riwayat.path);
                    cmd.Parameters.AddWithValue("@statusVerifikasi", riwayat.statusVerifikasi);
                    cmd.Parameters.AddWithValue("@pesanBKD", riwayat.pesanBKD);
                    cmd.Parameters.AddWithValue("@pesanOPD", riwayat.pesanOPD);

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

        [HttpPost("hapus/{id}", Name = "HapusRiwayatKinerja")]
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
                    SqlCommand cmd = new SqlCommand("sp_HapusRiwayatKinerja", conn);
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
