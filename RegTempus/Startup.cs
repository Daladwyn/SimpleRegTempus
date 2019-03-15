using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RegTempus.Models;
using RegTempus.Services;

namespace RegTempus
{
    public class Startup
    {
        public IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<RegTempusDbContext>(
                            options => options.UseSqlServer(_configuration.GetConnectionString("RegTempus")));

            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<RegTempusDbContext>();

            services.AddTransient<IRegTempus, SqlRegTempusData>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseStatusCodePages();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            //app.UseNodeModules(env.ContentRootPath);

            app.UseAuthentication();

            
            app.UseMvc(RouteOptions);

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
        }
        private void RouteOptions(IRouteBuilder routes)
        {
            routes.MapRoute("Default", "{controller=Home}/{action=Index}");
        }
    }
}
