using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
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
    string destinationPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "thumbnails", "thailand");

    if (!Directory.Exists(destinationPath))
    {
        Directory.CreateDirectory(destinationPath);
    }

    foreach (string filePath in Directory.GetFiles(sourcePath, "*.jpg")) // Adjust the file extension as needed
    {
        try
        {
            string fileName = Path.GetFileName(filePath);
            string destinationFilePath = Path.Combine(destinationPath, fileName);

            // Check if the thumbnail already exists
            if (File.Exists(destinationFilePath))
            {
                Console.WriteLine($"Thumbnail already exists for '{fileName}'. Skipping.");
                continue;
            }

            using (var image = Image.Load(filePath))
            {
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(300, 300), // Adjust the thumbnail size as needed
                    Mode = ResizeMode.Max
                }));

                image.Save(destinationFilePath);
            }
        }
        catch (Exception ex)
        {
            // Handle any exceptions that may occur during image processing
            Console.WriteLine($"Error processing image '{filePath}': {ex.Message}");
        }
    }
}

    }
}
