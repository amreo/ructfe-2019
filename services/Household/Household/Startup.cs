using System.IO;
using AutoMapper;
using Household.DataBase;
using Household.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Household
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
            services.AddDbContext<HouseholdDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DataBaseContext")));

            services.AddDefaultIdentity<ApplicationUser>()
                .AddEntityFrameworkStores<HouseholdDbContext>();
            //.AddSignInManager<HouseholdSignInManager>();

            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, HouseholdDbContext>();
            //.AddCookieAuthentication();

            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.AddCors();
            services.AddControllersWithViews();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/dist"; });

            services.AddRazorPages();

            services.AddSwaggerGen(c =>
            {
                c.IncludeXmlComments(GetXmlCommentsPath());
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Apishka", Version = "v1"});
                {
                    //c.SwaggerDoc("v1", new OpenApiInfo {Title = IdentityServerConfig.ApiFriendlyName, Version = "v1"});

                    //c.OperationFilter<AuthorizeCheckOperationFilter>();
                    //c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    //{
                    //    Type = SecuritySchemeType.OAuth2,
                    //    Flows = new OpenApiOAuthFlows
                    //    {
                    //        Password = new OpenApiOAuthFlow
                    //        {
                    //            TokenUrl = new Uri("/connect/token", UriKind.Relative),
                    //            Scopes = new Dictionary<string, string>()
                    //            {
                    //                {IdentityServerConfig.ApiName, IdentityServerConfig.ApiFriendlyName}
                    //            }
                    //        }
                    //    }
                    //});
                }
            });

            services.AddAutoMapper(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts(); // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            }

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = "Swagger UI - QuickApp";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Apishka Name V1");
                {
                    //c.OAuthClientId("OAuthClientId value");
                    ////c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{IdentityServerConfig.ApiFriendlyName} V1");
                    ////c.OAuthClientId(IdentityServerConfig.SwaggerClientID);
                    //c.OAuthClientSecret("no_password"); //Leaving it blank doesn't work
                }
            });

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default", pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";// To learn more about options for serving an Angular SPA from ASP.NET Core, see https://go.microsoft.com/fwlink/?linkid=864501
                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }

        private string GetXmlCommentsPath()
        {
            var dllFilePath = typeof(Startup).Assembly.Location;
            var directoryPath = Path.GetDirectoryName(dllFilePath);
            var fileName = Path.GetFileNameWithoutExtension(dllFilePath) + ".xml";
            return Path.GetFullPath(fileName, directoryPath);
        }
    }
}
