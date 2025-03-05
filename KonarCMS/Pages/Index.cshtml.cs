using KonarCMS.Models;
using KonarCMS.Models.Tiles;
using KonarCMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace KonarCMS.Pages
{
    public class IndexModel : PageModel
    {
        public OverallData Data { get; set; } = new OverallData();
        public Dictionary<string, TileBase> Tiles { get; set; }
        private readonly IWebHostEnvironment _environment;
        private readonly IDataLoaderService _dataLoaderService;
        private readonly IImagesService _imagesService;
        private readonly string _appDataPath;
        public IndexModel(IWebHostEnvironment environment, IDataLoaderService dataLoaderService, IImagesService imagesService)
        {
            _environment = environment;
            _appDataPath = Path.Combine(_environment.ContentRootPath, "App_Data");
            _dataLoaderService = dataLoaderService;
            _imagesService = imagesService;

            Data = dataLoaderService.GetOverallData();
            Tiles = dataLoaderService.GetTiles();
        }
        public List<string> GetImages(string source)
        {
            if (source == "overall")
                return _imagesService.GetImages(Data);

            if (_dataLoaderService.GetTile(source) is ProjectsTile tile)
                return _imagesService.GetImages(tile);

            else return null;
        }
    }
}
