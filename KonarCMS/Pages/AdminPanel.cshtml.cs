using KonarCMS.Interfaces;
using KonarCMS.Models;
using KonarCMS.Models.Tiles;
using KonarCMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KonarCMS.Pages
{
    public class AdminPanelModel : PageModel
    {

        [BindProperty]
        public Dictionary<string, TileBase> Tiles { get; set; }
        [BindProperty]
        public OverallData OveralData { get; set; }
        private readonly IWebHostEnvironment _environment;
        private readonly IDataLoaderService _dataLoaderService;
        private readonly IImagesService _imagesService;
        private readonly string _appDataPath;
        public AdminPanelModel(IWebHostEnvironment environment, IDataLoaderService dataLoaderService, IImagesService imagesService)
        {
            _environment = environment;
            _appDataPath = Path.Combine(_environment.ContentRootPath, "App_Data");
            _dataLoaderService = dataLoaderService;
            _imagesService = imagesService;

            OveralData = _dataLoaderService.GetOverallData();
            Tiles = _dataLoaderService.GetTiles();
        }
        public ActionResult OnPostSaveTile()
        {
            var tile = Tiles.First();

            if (tile.Value != null)
                tile.Value.Serialize(_appDataPath, tile.Key);

            return RedirectToPage();
        }
        public ActionResult OnPostSaveOverall()
        {
            OveralData.OfferList = OveralData.OfferList.Where(s => s != null).ToList();
            OveralData.ProjectsList = OveralData.ProjectsList.Where(s => s != null).ToList();
            OveralData.Serialize(_appDataPath, "overall");

            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostUploadImagesAsync(List<IFormFile> uploadedFiles, string targetName)
        {
            string uploadPath = Path.Combine(_environment.WebRootPath, "photos");
            IImagesContainer target = _dataLoaderService.GetImagesContainer(targetName);            

            if (target != null && _imagesService.UploadImages(target, uploadedFiles, uploadPath))
            {
                if (target is SerializableObject serializable)
                    serializable.Serialize(_appDataPath, targetName);
            }
            else
            {
                //log
            }
            return RedirectToPage();
        }
        public List<string> GetImages(string source)
        {
            if (source == "overall")
                return _imagesService.GetImages(OveralData);

            if (_dataLoaderService.GetTile(source) is ProjectsTile tile)
                return _imagesService.GetImages(tile);

            else return null;
        }
        public IActionResult OnPostDeleteImageAsync()
        {
            var imageToDelete = Request.Form["imageToDelete"];
            var targetName = Request.Form["target"];
            IImagesContainer target = _dataLoaderService.GetImagesContainer(targetName);            

            if(target==null)
            {
                //log
                return RedirectToPage();
            }

            string path = Path.Combine(_environment.WebRootPath, imageToDelete);
            string fileName = Path.GetFileName(imageToDelete);

            if (_imagesService.DeleteImage(target, path))
            {
                if (target is SerializableObject serializable)
                    serializable.Serialize(_appDataPath, targetName);
            }

            return RedirectToPage();
        }
    }
}
