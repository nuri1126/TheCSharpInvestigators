using LetsGoSEA.WebSite.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LetsGoSEA.WebSite
{
    /// <summary>
    /// Startup configures services required by the app and defines the apps
    /// request handling pipeline and middleware. 
    /// </summary>
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
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddHttpClient();
            services.AddControllers();

            // Add Neighborhood service.
            services.AddTransient<NeighborhoodService>();

            // Add AboutUs service.
            services.AddTransient<AboutUsService>();
        }

        /// <summary>
        /// Configure sets up the HTTP request pipeline and associated middleware. It is called during runtime. 
        /// </summary>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // When env is set to development, displays dev exception page.
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // When env is in prod, shows real error page 
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Adds middleware components to the request pipeline.
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
            });
        }
    }
}
