using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BKD_API.Models;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using Hangfire;
using Hangfire.SqlServer;
using BKD_Service.Controllers.Absen;

using Microsoft.Extensions.Logging;


namespace BKD_Service
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration config)
        {
            Configuration = config;
        }
        ////public string conn { get; set; }
        //public IConfiguration Configuration { get; }
        //public IConfigurationRoot ConfigurationRoot { get; set; }

        //public static string ConnectionStringAdmin { get; private set; }
        //public static string ConnectionStringAbsen { get; private set; }
        //public static string ConnectionStringSimpeg { get; private set; }
        //public static string ConnectionString { get; internal set; }

        //public Startup(IConfiguration config, IWebHostEnvironment env, IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //    ConfigurationRoot = new ConfigurationBuilder()
        //        .SetBasePath(env.ContentRootPath)
        //        .AddJsonFile("appsettings.json")
        //        .Build();
        //}

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Mengambil nilai koneksi dari appsettings.json
            var adminConnectionString = Configuration.GetConnectionString("conAdmin");
            var absenConnectionString = Configuration.GetConnectionString("conAbsen");
            var simpegConnectionString = Configuration.GetConnectionString("conSimpeg");

            // Inisialisasi properti koneksi dengan nilai dari appsettings.json
            Startup.ConnectionStringAdmin = adminConnectionString;
            Startup.ConnectionStringAbsen = absenConnectionString;
            Startup.ConnectionStringSimpeg = simpegConnectionString;

            // Tambahkan layanan Hangfire Server terlebih dahulu
            services.AddHangfireServer();
            services.AddHttpClient();

            // Kemudian tambahkan penyimpanan SQL Server untuk Hangfire
            services.AddHangfire(configuration => configuration.UseSqlServerStorage(Startup.ConnectionStringAbsen));

            services.AddControllers();

            // Konfigurasi autentikasi JWT
            var jwtSettingSection = Configuration.GetSection("JWTSettings");
            services.Configure<JWTSetting>(jwtSettingSection);
            var authKey = Configuration.GetValue<string>("JWTSettings:securitykey");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            // Gunakan Hangfire Dashboard
            app.UseHangfireDashboard();

            // Jadwalkan tugas untuk menjalankan FetchAndSaveData setiap 10 detik
            //RecurringJob.AddOrUpdate<BiotimeController>(x => x.FetchAndSaveData(), Cron.MinuteInterval(1));
            RecurringJob.AddOrUpdate<BiotimeController>(x => x.FetchAndSaveData(), "*/5 * * * * *");
            //RecurringJob.AddOrUpdate<BiotimeController>(x => x.FetchAndSaveData(), "*/1 * * * *");


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        //public void ConfigureLogging(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        //{
        //    loggerFactory.AddFile("Logs/myapp-{Date}.txt");

        //    ILogger logger = loggerFactory.CreateLogger<Startup>();

        //    logger.LogInformation("Aplikasi dimulai.");

        //    if (env.IsDevelopment())
        //    {
        //        app.UseDeveloperExceptionPage();
        //    }
        //    else
        //    {
        //        app.UseExceptionHandler("/Home/Error");
        //        app.UseHsts();
        //    }

        //    // Konfigurasi lainnya...
        //}

        // Properti koneksi
        public static string ConnectionStringAdmin { get; private set; }
        public static string ConnectionStringAbsen { get; private set; }
        public static string ConnectionStringSimpeg { get; private set; }
    }
}
