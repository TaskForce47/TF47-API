using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using TF47_API.Database;
using TF47_API.Database.Models;
using TF47_API.Database.Models.GameServer;
using TF47_API.Database.Models.Services;
using TF47_API.Services;
using TF47_API.Services.ApiToken;
using TF47_API.Services.Authentication;
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
                            "https://gadget.taskforce47.com:8080");
                        builder.AllowCredentials();
                        builder.AllowAnyMethod();
                    });
            });
            services.AddControllers();
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
            services.AddSingleton<ApiTokenCache>();
            services.AddHttpClient();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger, DatabaseContext database)
        {

            if (!database.Players.Any(x => x.PlayerUid == "Player1"))
            {
                var users = new List<TF47_API.Database.Models.GameServer.Player>();
                for(int i = 0; i < 1000; i++)
                {
                    var random = new Random();
                    var user = new TF47_API.Database.Models.GameServer.Player
                    {
                        PlayerUid = $"Player{i}",
                        FirstVisit = DateTime.Now - TimeSpan.FromMinutes(random.Next(0, 100000)),
                        LastVisit = DateTime.Now,
                        NumberConnections = random.Next(0, 600),
                        PlayerName = $"Player{i}"
                    };
                    users.Add(user);
                }
                database.Players.AddRange(users);

                var campaign = new Campaign
                {
                    Description = "Test Campaign",
                    Name = "Test",
                    TimeCreated = DateTime.Now
                };
                database.Campaigns.Add(campaign);

                var mission = new Mission
                {
                    Campaign = campaign,
                    Description = "Test mission",
                    MissionType = MissionType.Coop,
                    Name = "Test"
                };
                database.Missions.Add(mission);

                var session = new Session
                {
                    Mission = mission,
                    WorldName = "Altis",
                    SessionCreated = DateTime.Now,
                };
                database.Sessions.Add(session);
                
                var chats = new List<Chat>();
                var channels = new string[] {"Direct", "Side", "Group", "Global"};
                
                for (int i = 0; i < 1000; i++)
                {
                    var random = new Random();
                    var chat = new Chat
                    {
                        Channel = channels[random.Next(0, 3)],
                        PlayerId = $"Player{random.Next(0, 1000)}",
                        Session = session,
                        Text = "ABCE",
                        TimeSend = DateTime.Now - TimeSpan.FromMinutes(random.Next(0, 10000))
                    };
                    chats.Add(chat);
                }
                database.Chats.AddRange(chats);
                database.SaveChanges();
                //33e78b09-02a7-4007-abab-049ff9ad04be
            }
            
            
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<TestHub>("/hub");
                endpoints.MapControllers();
            });
        }
    }
}
