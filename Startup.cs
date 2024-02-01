using SixLabors.ImageSharp.Web.DependencyInjection;

namespace Thailand
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
            services.AddImageSharp();
            services.AddControllers();
            // services.AddMvc();
            services.AddControllersWithViews();

            // Other service configurations...
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            GenerateThumbnails();

            app.UseImageSharp();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Gallery}/{action=Index}/{id?}");
            });
        }

        private void GenerateThumbnails()
        {
            string sourcePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "thailand");
            string destinationThumbnailPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "thumbnails", "thailand");
            string destinationPublishedPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "published", "thailand");


            if (!Directory.Exists(destinationThumbnailPath))
            {
                Directory.CreateDirectory(destinationThumbnailPath);
            }

            if (!Directory.Exists(destinationPublishedPath))
            {
                Directory.CreateDirectory(destinationPublishedPath);
            }


            ImageHelper.GenerateImages(sourcePath, destinationThumbnailPath);
            ImageHelper.GenerateImages(sourcePath, destinationPublishedPath);
        }

    }
}
