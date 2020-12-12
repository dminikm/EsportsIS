using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebApp
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
            services.AddControllersWithViews();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => false; // consent required
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSession((options) =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = false;
                options.Cookie.IsEssential = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var router = new Router();

            router
                .AddRoute<LoginController>(HTTPMethod.GET, "/", "Index")
                .AddRoute<LoginController>(HTTPMethod.GET, "/login", "LoginGET")
                .AddRoute<LoginController>(HTTPMethod.POST, "/login", "LoginPOST")
                .AddRoute<LoginController>(HTTPMethod.GET, "/logout", "Logout")
                .AddRoute<EventController>(HTTPMethod.GET, "/overview", "Overview")
                .AddRoute<EventController>(HTTPMethod.POST, "/event/{eventID}/join", "Join")
                .AddRoute<EventController>(HTTPMethod.GET, "/event/{eventID}", "Detail");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                router.BindRoutes(endpoints);
            });
        }
    }
}
