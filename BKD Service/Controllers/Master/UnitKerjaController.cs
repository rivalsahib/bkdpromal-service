using BKD_API.Models;
using BKD_Service.Model.Data;
using BKD_Service.Model.Master;
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

namespace BKD_Service.Controllers.Master
{
    [Route("api/master/[controller]")]
    [ApiController]
    [Authorize]
    public class UnitKerjaController : Controller
    {
        public IConfiguration Configuration { get; }
        string constrSimpeg = Startup.ConnectionStringSimpeg;

        [HttpGet(Name = "GetUnkerAll")]
        public async Task<ActionResult<Respon>> GetAll()
        {
            Respon respon = new Respon();
            List<UnitKerja> daftar_unit_kerja = new List<UnitKerja>();

            using (SqlConnection conn = new SqlConnection(constrSimpeg))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_TampilMasterUnitKerjaAll", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            daftar_unit_kerja.Add(new UnitKerja()
                            {
                                id_satker = rdr.GetString("id").ToString(),
                                satker = rdr.GetString("satker").ToString(),
                                id_unit_kerja = rdr.GetString("kelompok_unker").ToString(),
                                unit_kerja = rdr.GetString("unker").ToString()
                            });
                        }
                        respon.pesan = "Data Berhasil Ditemukan";
                        respon.data = daftar_unit_kerja;
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
