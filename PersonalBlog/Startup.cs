using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PersonalBlog.Data;
using System;
using Npgsql;
using System.Threading.Tasks;

namespace PersonalBlog
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(
            //        Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            {
                var dbPassword = Environment.GetEnvironmentVariable("DATABASE_PASSWORD");
                var dbHost = Environment.GetEnvironmentVariable("DATABASE_HOST");
                var dbUsername = Environment.GetEnvironmentVariable("DATABASE_USERNAME");
                var dbName = Environment.GetEnvironmentVariable("DATABASE_NAME");
                //var gClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENTID");
                //var gClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENTSECRET");
                //var fClientId = Environment.GetEnvironmentVariable("FACEBOOK_CLIENTID");
                //var fClientSecret = Environment.GetEnvironmentVariable("FACEBOOK_CLIENTSECRET");
                int.TryParse(Configuration["PostgreSql:Port"], out int port);

                var builder = new NpgsqlConnectionStringBuilder()
                {
                    Host = dbHost,
                    Password = dbPassword,
                    Username = dbUsername,
                    Database = dbName,
                    Port = port
                };

                services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(builder.ConnectionString));
                

                //services.AddAuthentication()
                //.AddGoogle(googleOptions =>
                //{
                //    googleOptions.ClientId = gClientId;
                //    googleOptions.ClientSecret = gClientSecret;
                //    //googleOptions.CallbackPath = "/Account/ExternalLoginCallback";
                //})
                //.AddFacebook(facebookOptions =>
                //{
                //    facebookOptions.AppId = fClientId;
                //    facebookOptions.AppSecret = fClientSecret;
                //});

            }
            else
            {
                var connectionString = Configuration["PostgreSql:ConnectionString"];
                var dbPassword = Configuration["PostgreSql:DbPassword"];

                var builder = new NpgsqlConnectionStringBuilder(connectionString)
                {
                    Password = dbPassword
                };

                services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(builder.ConnectionString));

                //services.AddAuthentication()
                //.AddGoogle(googleOptions =>
                //{
                //    googleOptions.ClientId = Configuration["OAUTH:providers:0:clientId"];
                //    googleOptions.ClientSecret = Configuration["OAUTH:providers:0:clientSecret"];
                //    //googleOptions.NonceCookie.SameSite = (SameSiteMode)(-1);
                //    googleOptions.CorrelationCookie.SameSite = SameSiteMode.None;
                //    googleOptions.Events = new Microsoft.AspNetCore.Authentication.OAuth.OAuthEvents
                //    {
                //        //OnRemoteFailure = (context) =>
                //        //{
                //        //    context.Response.Redirect(context.Properties.GetString("returnUrl"));
                //        //    context.HandleResponse();
                //        //    return Task.CompletedTask;
                //        //},

                //        OnRedirectToAuthorizationEndpoint = redirectContext =>
                //        {
                //            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
                //            {
                //                //Force scheme of redirect URI (THE IMPORTANT PART)
                //                redirectContext.RedirectUri = redirectContext.RedirectUri.Replace("http://", "https://", StringComparison.OrdinalIgnoreCase);
                //            }
                //            return Task.FromResult(0);
                //        }
                //    };
                //    //googleOptions.CallbackPath = "/Account/ExternalLoginCallback";
                //})
                //.AddFacebook(facebookOptions =>
                //{
                //    facebookOptions.AppId = Configuration["OAUTH:providers:1:clientId"];
                //    facebookOptions.AppSecret = Configuration["OAUTH:providers:1:clientSecret"];
                //});
            }

            // Automatically perform database migration
            services.BuildServiceProvider().GetService<ApplicationDbContext>().Database.Migrate();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
