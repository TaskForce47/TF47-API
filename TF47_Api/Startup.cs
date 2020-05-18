using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using TF47_Api.Database;
using TF47_Api.Middleware;
using TF47_Api.Services;

namespace TF47_Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("Policy",
                    builder =>
                    {
                        /*
                        builder.WithOrigins("https://gadget.taskforce47.com:4200",
                                "http://gadget.taskforce47.com",
                                "https://gadget.taskforce47.com",
                                "http://api.taskforce47.com",
                                "https://api.taskforce47.com")*/
                        builder.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
            });
            services.AddControllers();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "CustomAuthentication";
            });
            services.AddAuthorization();
            
            services.AddDbContext<Tf47DatabaseContext>(
                options => options.UseMySql(
                    "server=server.taskforce47.com;port=3306;user=dragon;password=bambus123xx#;database=tf47_database_2"));
            services.AddScoped<ForumDataProviderService>();
            services.AddScoped<ClaimProviderService>();
            services.AddSingleton<AuthenticationProviderService>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Taskforce47 API",
                    Version = "v1"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "TF47 API V1"); });
            //app.UseCorsMiddleware();
            app.UseCors("Policy");
            //app.UseHttpsRedirection();
            app.UseCustomCookieAuthentication();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
