// ImageHelper.cs

namespace Thailand
{
    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.Processing;
    using System;

    public static class ImageHelper
    {
        public static void GenerateImages(string sourcePath, string destinationPath)
        {
            foreach (string filePath in Directory.GetFiles(sourcePath, "*.jpg")) // Adjust the file extension as needed
            {
                try
                {
                    string fileName = Path.GetFileName(filePath);
                    string destinationFilePath = Path.Combine(destinationPath, fileName);

                    // Check if the thumbnail already exists
                    if (File.Exists(destinationFilePath))
                    {
                        Console.WriteLine($"Image already exists for '{destinationFilePath}'. Skipping.");
                        continue;
                    }

                    using (var image = Image.Load(filePath))
                    {
                        ResizeImage(filePath, destinationFilePath, 1920, 0);
                    }
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that may occur during image processing
                    Console.WriteLine($"Error processing image '{filePath}': {ex.Message}");
                }
            }
        }

        private static void ResizeImage(string inputPath, string outputPath, int targetWidth, int targetHeight)
        {
            using (var image = Image.Load(inputPath))
            {
                // Resize the image while maintaining the aspect ratio
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(targetWidth, targetHeight == 0 ? 0 : targetHeight),
                    Mode = ResizeMode.Max
                }));

                // Save the resized image to the specified output path
                image.Save(outputPath);
                Console.WriteLine($"Successfully processed image '{outputPath}'");

            }
        }
    }
}
