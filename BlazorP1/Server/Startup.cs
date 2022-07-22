using BlazorP1.Server.Data;
using BlazorP1.Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System;
using System.Linq;

namespace BlazorP1.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            // postgres://leclbiltkorfkz:e784defc1fc66f9519242073b49ff30ff93d4f9e74bada384dee357983b0dc4b@ec2-54-76-43-89.eu-west-1.compute.amazonaws.com:5432/d98gg5dk7rka1c
            var env = Environment.GetEnvironmentVariable("DATABASE_URL");
            //postgres://1user:1password@dbserver.com:4568/testdb
            var raw = env.Split("/");


            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = raw[1].Split("@")[1].Split(":")[0],
                Username = raw[1].Split(":")[0],
                Password = raw[1].Split(":")[1].Split("@")[0],
                Port = Convert.ToInt32(raw[1].Split(":")[2]),
                Database = raw[2]
            };
            

            services.AddDbContext<DataContext>(o => 
                o.UseNpgsql(builder.ToString()));

            services.Configure<EmailOptions>(Configuration);
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IUtilityService, UtilityService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("TOKEN"))),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbInitializer dbInit)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            dbInit.Initialize().GetAwaiter();
            
            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
