using DataAccess;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Paula_Cardona_SWD6._3A
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"E:\Paula\Paulene\Level 6 Third Year\Second Semester\Assignments\Programming for the cloud\vernal-layout-340609-1d035cefb454.json");
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
         .AddAuthentication(options =>
         {
             options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
             options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
         })
         .AddCookie()
         .AddGoogle(options =>
         {
             options.ClientId = "529434771206-k5lph18h349pb8tm8ndm44nraicg6t8i.apps.googleusercontent.com";
             options.ClientSecret = "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";
         });

            services.AddRazorPages();
            services.AddControllersWithViews();

            string projectId = Configuration["Project"];

            services.AddScoped<FireStoreDataAccess>(
                x =>
                {
                    return new FireStoreDataAccess(projectId);
                }
                );

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
         
     
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
