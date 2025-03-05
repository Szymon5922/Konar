using KonarCMS.Interfaces;
using KonarCMS.Models;
using KonarCMS.Models.Tiles;

namespace KonarCMS.Services
{
    public interface IImagesService
    {
        public List<string> GetImages(IImagesContainer source);
        public bool DeleteImage(IImagesContainer target, string imageToDeletePath);
        public bool UploadImages(IImagesContainer target, List<IFormFile> images, string uploadPath);
    }
    public class ImagesService : IImagesService
    {
        public List<string> GetImages(IImagesContainer source)
        {
            List<string> imagesUrls = new();

            foreach (var image in source.Images)
                imagesUrls.Add(Path.Combine("photos", image));

            return imagesUrls;
        }
        public bool DeleteImage(IImagesContainer target, string imageToDeletePath)
        {
            string fileName = Path.GetFileName(imageToDeletePath);

            try
            {
                System.IO.File.Delete(imageToDeletePath);

                target.Images.Remove(fileName);

                return true;
            }
            catch (Exception ex)
            {
                //log
            }

            return false;
        }
        public bool UploadImages(IImagesContainer target, List<IFormFile> images, string uploadPath)
        {
            HashSet<string> targetContainer = target.Images;

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
