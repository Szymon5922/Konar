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
        public DataModel Data { get; set; } = new DataModel();
        public Dictionary<string, TileBase> Tiles { get; set; }
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
        public IndexModel(IWebHostEnvironment environment)
        {
            _environment = environment;
            _appDataPath = Path.Combine(_environment.ContentRootPath, "App_Data");

            Data.Deserialize(_appDataPath, "overall");
            Tiles = _tilesList;
        }
        public List<string> GetImages(string source)
        {
            if (source == "overall")
                return ImagesService.GetImages(Data);

            if (_tilesList[source] is ProjectsTile tile)
                return ImagesService.GetImages(tile);

            else return null;
        }
    }
}
