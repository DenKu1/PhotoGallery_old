using System;
using System.Net;
using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

using Newtonsoft.Json;

using PhotoGallery.BLL.Configuration;
using PhotoGallery.BLL.Configuration.Automapper;
using PhotoGallery.BLL.Configuration.DI;
using PhotoGallery.BLL.Exceptions;
using PhotoGallery.API.Configuration.Automapper;
using PhotoGallery.API.Configuration.DI;

namespace PhotoGallery.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var jwtKey = Configuration["ApplicationSettings:JwtKey"];
            var connectionString = Configuration.GetConnectionString("PhotoGalleryConnection");

            ConfigureAuthentication(services, jwtKey);

            services.Configure<IdentityOptions>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequiredLength = 4;
            });

            services.AddCors();
            services.AddControllers();
            services.AddAutoMapper(typeof(ModelProfile), typeof(DtoProfile));

            services.AddEntityFramework(connectionString);
            services.AddRepositories();
            services.AddServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(a => a.Run(async context =>
                {
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var exception = exceptionHandlerPathFeature.Error;

                    var result = JsonConvert.SerializeObject(new { message = exception.Message });

                    context.Response.StatusCode = exception is PhotoGalleryException
                    ? (int)HttpStatusCode.BadRequest
                    : (int)HttpStatusCode.InternalServerError;

                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsync(result);
                }));
            }

            app.UseCors(builder => builder.WithOrigins("http://localhost:5001")
                .AllowAnyHeader()
                .AllowAnyMethod());

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                    //spa.UseProxyToSpaDevelopmentServer("http://localhost:52818");
                }
            });
        }

        protected virtual void ConfigureAuthentication(IServiceCollection services, string jwtKey)
        {
            services.Configure<JwtSettings>(Configuration.GetSection("ApplicationSettings"));
            
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                opt.RequireHttpsMetadata = false;
                opt.SaveToken = false;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });
        }
    }
}
