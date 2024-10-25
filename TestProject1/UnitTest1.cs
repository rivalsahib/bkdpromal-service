using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Threading.Tasks;
using TestProject1;
using BKD_Service.Controllers.Absen; // Contoh namespace tempat BiotimeController berada


namespace TestProject1.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task SaveResponseToDatabase_ValidResponse_Success()
        {
            // Arrange
            var controller = new BiotimeController(new HttpClient()); // Anda perlu menyediakan HttpClient palsu atau menggunakan mocking framework untuk menggantinya

            // Response JSON dummy yang sesuai dengan struktur yang diharapkan
            string responseBody = @"
            {
                ""count"": 1,
                ""data"": [
                    {
                        ""emp_code"": ""123456"",
                        ""punch_time"": ""2024-02-24T00:01:14"",
                        // Sisipkan properti lain sesuai dengan struktur model DataModel
                    }
                ]
            }";

            // Act
            await controller.SaveResponseToDatabase(responseBody);

            // Assert
            // Tambahkan asserstions di sini untuk memastikan bahwa data telah disimpan ke database dengan benar
        }


        [TestMethod]
        public async Task SaveResponseToDatabase_InvalidResponse_HandleError()
        {
            // Arrange
            var controller = new BiotimeController(new HttpClient()); // Anda perlu menyediakan HttpClient palsu atau menggunakan mocking framework untuk menggantinya

            // Response JSON tidak valid atau tidak sesuai dengan struktur yang diharapkan
            string responseBody = "Invalid JSON";

            // Act
            await controller.SaveResponseToDatabase(responseBody);

            // Assert
            // Tambahkan assertions di sini untuk memastikan bahwa metode dapat menangani kesalahan dengan benar
        }
    }
}
