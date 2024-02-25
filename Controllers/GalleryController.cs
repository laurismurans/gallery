using Microsoft.AspNetCore.Mvc;

namespace Thailand.Controllers
{
    public class GalleryController : Controller
    {
        public IActionResult Index()
        {
            // Get a list of image paths (you can replace this with your logic to retrieve paths)
            var imagePaths = GetImagePaths();

            // Create an instance of the model and set the ImagePaths property
            var model = new Models.ImageModel
            {
                ImagePaths = imagePaths
            };

            // Pass the model to the view
            return View(model);
        }
        private List<string> GetImagePaths()
        {
            string imagesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "thumbnails", "thailand");
            return Directory.GetFiles(imagesDirectory, "*.jpg")
                            .Select(Path.GetFileName)
                            .ToList();
        }

        // Add this action to your HomeController
        public IActionResult LoadMoreImages(int pageNumber)
        {
            // Use the pageNumber to fetch the next set of images from your data source
            var additionalImagePaths = GetAdditionalImagePaths(pageNumber);

            return Json(additionalImagePaths);
        }

        private List<string> GetAdditionalImagePaths(int pageNumber)
        {
            int pageSize = 20;

            // Specify the folder path
            string folderPath = "wwwroot/images/thumbnails/thailand";

            // Create a DirectoryInfo object for the folder
            DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);

            // Get all files in the folder
            FileInfo[] files = directoryInfo.GetFiles();


            int startingFileIndex = pageNumber * pageSize;

            List<string> imagePaths = [];

            if (files.Length <= startingFileIndex)
            {
                return [];
            }

            for (int i = startingFileIndex; i < startingFileIndex + pageSize; i++)
            {
                FileInfo file = files[i];

                imagePaths.Add(file.Name);
            }


            return imagePaths;
        }

        public IActionResult ImageModal(string imagePath)
        {
            ViewBag.ImagePath = imagePath;
            return PartialView("_ImageModal");
        }


    }
}