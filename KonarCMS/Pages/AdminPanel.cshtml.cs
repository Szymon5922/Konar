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
        public DataModel OveralData { get; set; } = new();
        private Dictionary<string, TileBase> _tilesList
        {
            get
            {
                var tiles = new Dictionary<string, TileBase>
                {
                    {"experience", new TextTile()},
                    {"commitment", new TextTile()},
                    {"clients", new TextTile() },
                    {"workplaces", new ProjectsTile() },
                    {"constructions", new ProjectsTile() },
                    {"expertise", new TextTile() }
                };
                foreach (var tile in tiles)
                    tile.Value.Deserialize(_appDataPath, tile.Key);
                return tiles;
            }
        }
        private readonly IWebHostEnvironment _environment;
        private readonly string _appDataPath;
        public AdminPanelModel(IWebHostEnvironment environment)
        {
            _environment = environment;
            _appDataPath = Path.Combine(_environment.ContentRootPath, "App_Data");

            OveralData.Deserialize(_appDataPath, "overall");
            Tiles = _tilesList;
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
            object target;

            if (targetName == "overall")
            {
                OveralData.Deserialize(_appDataPath, targetName);
                target = OveralData;
            }
            else
                target = _tilesList[targetName];

            if (ImagesService.UploadImages(target, uploadedFiles, uploadPath))

                if (target is SerializableObject serializable)
                    serializable.Serialize(_appDataPath, targetName);

            return RedirectToPage();
        }
        public List<string> GetImages(string source)
        {
            if (source == "overall")
                return ImagesService.GetImages(OveralData);

            if (_tilesList[source] is ProjectsTile tile)
                return ImagesService.GetImages(tile);

            else return null;
        }
        public IActionResult OnPostDeleteImageAsync()
        {
            var imageToDelete = Request.Form["imageToDelete"];
            var targetName = Request.Form["target"];
            object target;

            if (targetName == "overall")
            {
                OveralData.Deserialize(_appDataPath, "overall");
                target = OveralData;
            }
            else
                target = _tilesList[targetName];

            string path = Path.Combine(_environment.WebRootPath, imageToDelete);
            string fileName = Path.GetFileName(imageToDelete);

            if (ImagesService.DeleteImage(target, path))
            {
                if(target is SerializableObject serializable)
                    serializable.Serialize(_appDataPath, targetName);
            }

            return RedirectToPage();
        }
    }
}
