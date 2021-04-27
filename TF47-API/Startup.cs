using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using TF47_API.Database;
using TF47_API.Middleware;
using TF47_API.Services;
using TF47_API.Services.ApiToken;
using TF47_API.Services.Authentication;
using TF47_API.Services.Authorization;
using TF47_API.Services.Mail;
using TF47_API.Services.OAuth;
using TF47_API.Services.SquadManager;
using TF47_API.SignalR;

namespace TF47_API
{
    public class Startup
    {
        readonly string CustomOrigins = "_myAllowSpecificOrigins";

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
                options.AddPolicy(name: CustomOrigins,
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:5001",
                            "https://test.taskforce47.com",
                            "https://beta.taskforce47.com",
                            "https://api.taskforce47.com",
                            "https://gadget.taskforce47.com",
                            "https://gadget.taskforce47.com:8080");
                        builder.AllowCredentials();
                        builder.AllowAnyMethod();
                        builder.AllowAnyHeader();
                    });
            });
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            });
            services.AddSignalR();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TF47 Backend", Version = "v1" });
            });

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(options =>
                {
                    options.Events.OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    };
                    options.AccessDeniedPath = Configuration["Redirections:Unauthorized"];
                    options.Cookie.HttpOnly = false;
                    options.Cookie.Domain = Configuration["CookieBaseUrl"];
                    options.Cookie.Name = "OperationDachdaggerAuthCookie";
                });

            services.AddDbContextPool<DatabaseContext>(options =>
            {
                var builder = new NpgsqlConnectionStringBuilder
                {
                    Host = Configuration["Credentials:Database:Server"],
                    Port = int.Parse(Configuration["Credentials:Database:Port"]),
                    Username = Configuration["Credentials:Database:Username"],
                    Password = Configuration["Credentials:Database:Password"],
                    Database = Configuration["Credentials:Database:Database"]
                };
                //Console.WriteLine(builder.ToString());
                options.UseNpgsql(builder.ToString());
                options.UseSnakeCaseNamingConvention();

                options.LogTo(Console.WriteLine);

                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            });
            services.AddTransient<MailService>();
            services.AddTransient<IUserProviderService, UserProviderService>();
            services.AddTransient<IAuthenticationManager, AuthenticationManager>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ISquadManagerService, SquadManagerService>();
            services.AddSingleton<ISteamAuthenticationService, SteamAuthenticationService>();
            services.AddSingleton<IDiscordAuthenticationService, DiscordAuthenticationService>();
            services.AddSingleton<IGroupPermissionCache, GroupPermissionCache>();
            services.AddSingleton<ApiTokenCache>();
            services.AddHttpClient();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger, DatabaseContext database)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = true,
            });
            app.UseDefaultFiles();
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TF47 Backend V1");
            });

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors(CustomOrigins);
            //app.UseGrpcWeb();

            app.UseApiTokenAuthentication();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<TestHub>("/hub");
                endpoints.MapHub<ShoutboxHub>("/shoutbox");
                endpoints.MapHub<NotificationHub>("/notification");
                endpoints.MapHub<EchelonHub>("/echelon");
                endpoints.MapControllers();
            });
        }
    }
}
