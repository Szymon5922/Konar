using KonarCMS.Models;
using KonarCMS.Models.Tiles;

namespace KonarCMS.Services
{
    public static class ImagesService
    {
        public static List<string> GetImages(ProjectsTile tile)
        {
            List<string> imagesUrls = new();

            foreach (var image in tile.Images)
                imagesUrls.Add(Path.Combine("photos", image));

            return imagesUrls;
        }
        public static List<string> GetImages(OverallData overall)
        {
            List<string> imagesUrls = new();

            foreach (var photo in overall.Photos)
                imagesUrls.Add(Path.Combine("photos", photo));

            return imagesUrls;
        }
        public static bool DeleteImage(object target, string imageToDeletePath)
        {
            string fileName = Path.GetFileName(imageToDeletePath);

            try
            {
                System.IO.File.Delete(imageToDeletePath);

                if (target is ProjectsTile projectsTile)
                    projectsTile.Images.Remove(fileName);
                else if (target is OverallData overall)
                    overall.Photos.Remove(fileName);

                return true;
            }
            catch (Exception ex)
            {
                //log
            }

            return false;
        }
        public static bool UploadImages(object target, List<IFormFile> images, string uploadPath)
        {
            HashSet<string> targetContainer;
            if (target is ProjectsTile projectsTile)
                targetContainer = projectsTile.Images;
            else if (target is OverallData overall)
                targetContainer = overall.Photos;
            else
                throw new ArgumentException();

            foreach (var file in images)
            {
                if (file != null && file.Length > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    string extension = Path.GetExtension(file.FileName);
                    string uniqueName = $"{fileName}_{Guid.NewGuid()}{extension}";
                    string filePath = Path.Combine(uploadPath, uniqueName);

                    targetContainer.Add(uniqueName);

                    try
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                    }
                    catch (Exception)
                    {
                        targetContainer.Remove(uniqueName);
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
