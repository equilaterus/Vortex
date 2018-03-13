using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vortex.SampleMvc.Data;
using Vortex.SampleMvc.Models;
using Vortex.SampleMvc.Services;
using Equilaterus.Vortex.Services;
using Equilaterus.Vortex.Services.EFCore;
using Equilaterus.Vortex.Engine;
using Equilaterus.Vortex.Engine.Configuration;
using Equilaterus.Vortex.Managers;
using Equilaterus.Vortex;

namespace Vortex.SampleMvc
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();


            #region VortexConfiguration
            
            // Inject DbContext to VortexServices
            services.AddScoped<DbContext>(p => p.GetRequiredService<ApplicationDbContext>());
            services.AddSingleton(typeof(IVortexGraph), typeof(VortexGraph));
            services.AddScoped(typeof(IVortexExecutor<>), typeof(VortexExecutor<>));
            services.AddScoped(typeof(IDataStorage<>), typeof(EFCoreDataStorage<>));
            services.AddScoped(typeof(IRelationalDataStorage<>), typeof(EFCoreDataStorage<>));
            services.AddScoped(typeof(IPersistanceManager<>), typeof(PersistanceManager<>));
            services.AddScoped(typeof(ICrudBehavior<>), typeof(CrudBehavior<>));

            #endregion


            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            #region VortexGraphConfiguration

            // Vortex Graph
            var vortexGraph = app.ApplicationServices.GetService<IVortexGraph>();
            vortexGraph.LoadDefaults();

            #endregion

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

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
