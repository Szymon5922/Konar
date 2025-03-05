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
        private readonly string _appDataPath;
        public IndexModel(IWebHostEnvironment environment, IDataLoaderService dataLoaderService)
        {
            _environment = environment;
            _appDataPath = Path.Combine(_environment.ContentRootPath, "App_Data");

            Data = dataLoaderService.GetOverallData();
            Tiles = dataLoaderService.GetTiles();
        }
        public List<string> GetImages(string source)
        {
            if (source == "overall")
                return ImagesService.GetImages(Data);

            if (_dataLoaderService.GetTile(source) is ProjectsTile tile)
                return ImagesService.GetImages(tile);

            else return null;
        }
    }
}
