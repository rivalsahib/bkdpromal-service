using BKD_API.Models;
using BKD_Service.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using RestSharp;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.IO;

namespace BKD_Service.Controllers.Dokumen
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DokumenController : Controller
    {
        public IConfiguration Configuration { get; }
        string constr = Startup.ConnectionStringSimpeg;

        [Microsoft.AspNetCore.Mvc.HttpPost("upload-rw/",Name = "UploadDokumenRiwayat")]
        public async Task<ActionResult<Respon>> uploadDokumenRiwayat([FromBody] Doc dokumen)
        {
            string retval = "";
            int i = 0;
            Respon respon = new Respon();

            try
            {
                string fileurl = "https://simpeg.malukuprov.go.id/upload/arsip/" + dokumen.file;
                string targetfile = "C:/inetpub/wwwroot/BKDService/dokumen/" + dokumen.file;
                using (HttpClient clients = new HttpClient())
                {
                    HttpResponseMessage responses = await clients.GetAsync(fileurl);
                    if (responses.IsSuccessStatusCode)
                    {
                        byte[] filebytes = await responses.Content.ReadAsByteArrayAsync();
                        System.IO.File.WriteAllBytes(targetfile, filebytes);
                    }
                }


                var options = new RestClientOptions("https://apimws.bkn.go.id:8243")
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest("/apisiasn/1.0/upload-dok-rw", Method.Post);
                request.AddHeader("Auth", "bearer " +  dokumen.token_akses_bkn);
                request.AddHeader("Authorization", "Bearer " + dokumen.token_api_bkn);
                request.AddHeader("Cookie", "ff8d625df24f2272ecde05bd53b814bc=3a50ad3e37443f6db1b39ffed17b52b9; pdns=1091068938.13088.0000");
                request.AlwaysMultipartFormData = true;
                request.AddParameter("id_riwayat", dokumen.id_riwayat);
                request.AddParameter("id_ref_dokumen", dokumen.id_ref_dokumen);
                request.AddFile("file", "dokumen/" + dokumen.file);
                RestResponse response = await client.ExecuteAsync(request);
                Console.WriteLine(response.Content);

                dynamic results = JsonConvert.DeserializeObject<dynamic>(response.Content);

                if (results.code == 1)
                {
                    respon.pesan = results.message;
                    respon.data = results.data.dok_uri.Value;
                }
                else
                {
                    respon.pesan = results.message;
                    respon.data = null;
                }
                return StatusCode(StatusCodes.Status200OK, respon);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



        [Microsoft.AspNetCore.Mvc.HttpGet("download-rw/", Name = "DownloadDokumenRiwayat")]
        public async Task<ActionResult<Respon>> downloadDokumenRiwayat([FromBody] Doc dokumen)
        {
            string retval = "";
            string targetfile = "https://simpeg.malukuprov.go.id/upload/" + Guid.NewGuid() + ".pdf";

            int i = 0;
            Respon respon = new Respon();

            try
            {
                var options = new RestClientOptions("https://apimws.bkn.go.id:8243")
                {
                    MaxTimeout = -1,
                };

                var client = new RestClient(options);
                var request = new RestRequest("https://apimws.bkn.go.id:8243/apisiasn/1.0/download-dok?filePath=" + dokumen.file, Method.Get);

                request.AddHeader("Auth", "bearer " + dokumen.token_akses_bkn);
                request.AddHeader("Authorization", "Bearer " + dokumen.token_api_bkn);
                request.AddHeader("Cookie", "ff8d625df24f2272ecde05bd53b814bc=9dda53a995c862fb190bbc7ea14bcb2d; pdns=1091068938.13088.0000");
                RestResponse response = await client.ExecuteAsync(request);
                Console.WriteLine(response.Content);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // Path file tujuan
                    string outputPath = @"C:/inetpub/wwwroot/BKD Service/dokumen/"+ Guid.NewGuid() + ".pdf";

                    // Menyimpan respons ke dalam file
                    System.IO.File.WriteAllText(outputPath, response.Content);

                    Console.WriteLine("Respons berhasil disimpan dalam file: " + outputPath);
                }
                else
                {
                    Console.WriteLine("Gagal menyimpan respons karena status kode tidak OK: " + response.StatusCode);
                }


                //dynamic results = JsonConvert.DeserializeObject<dynamic>(response.Content);

                //if (results.code == 1)
                //{
                //    respon.pesan = results.message;
                //    respon.data = results.data.dok_uri.Value;
                //}
                //else
                //{
                //    respon.pesan = results.message;
                //    respon.data = null;
                //}
                return StatusCode(StatusCodes.Status200OK, respon);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


    }
}
