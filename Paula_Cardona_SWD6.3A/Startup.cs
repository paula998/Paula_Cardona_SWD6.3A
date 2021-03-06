using Common;
using DataAccess;
using Google.Cloud.Diagnostics.AspNetCore3;
using Google.Cloud.SecretManager.V1;
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
        
        public Startup(IConfiguration configuration, IWebHostEnvironment host)
        {
            System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", host.ContentRootPath + @"/vernal-layout-340609-1d035cefb454.json");
            Configuration = configuration;
        }

      
        public IConfiguration Configuration { get; }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string projectId = Configuration["Project"];

            //for login
            SecretManagerServiceClient client = SecretManagerServiceClient.Create();

            SecretVersionName secretVersionName = new SecretVersionName(projectId, "secretkey_ouathlogin", "1");

            AccessSecretVersionResponse result = client.AccessSecretVersion(secretVersionName);

            // Convert the payload to a string. Payloads are bytes by default.
            String payload = result.Payload.Data.ToStringUtf8();

            services.AddGoogleDiagnosticsForAspNetCore(projectId);

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
            options.ClientSecret = payload;
        });

            services.AddRazorPages();
            services.AddControllersWithViews();

          

            services.AddScoped<FireStoreDataAccess>(
                x =>
                {
                    return new FireStoreDataAccess(projectId);
                }
                );

            services.AddScoped<CacheDataAccess>(
           x =>
           {
               return new CacheDataAccess("redis-17679.c98.us-east-1-4.ec2.cloud.redislabs.com:17679,password=EndztbRUBUVqXSUyghEe7zkcHk0JUhIQ");
           }
           );

            services.AddScoped<PubSubAccess>(
            x => {
                return new PubSubAccess(projectId);
            }
            );

   


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseHsts();


           // if (env.IsDevelopment())
            //{
              //  app.UseDeveloperExceptionPage();
           // }
            //else
            //{
               // app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
               // app.UseHsts();
           // }
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
