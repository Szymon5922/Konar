using KonarCMS.Models;
using KonarCMS.Models.Tiles;

namespace KonarCMS.Services
{
    public interface IDataLoaderService
    {
        OverallData GetOverallData();
        Dictionary<string, TileBase> GetTiles();
        TileBase GetTile(string tileName);
    }
    public class DataLoaderService : IDataLoaderService
    {
        private readonly string _appDataPath;
        public DataLoaderService(IWebHostEnvironment environment)
        {
            _appDataPath = Path.Combine(environment.ContentRootPath, "App_Data");
        }
        public OverallData GetOverallData()
        {
            var data = new OverallData();
            data.Deserialize(_appDataPath, "overall");
            return data;
        }
        public TileBase GetTile(string tileName)
        {
            var tile = GetTilesList[tileName];
            tile.Deserialize(_appDataPath, tileName);
            return tile;
        }
        public Dictionary<string, TileBase> GetTiles()
        {
            var tiles = GetTilesList;

            foreach (var tile in tiles)
                tile.Value.Deserialize(_appDataPath, tile.Key);
            return tiles;
        }
        public Dictionary<string,TileBase> GetTilesList =>
            new Dictionary<string, TileBase>
                {
                    {"experience", new TextTile()},
                    {"commitment", new TextTile()},
                    { "clients", new TextTile() },
                    { "workplaces", new ProjectsTile() },
                    { "constructions", new ProjectsTile() },
                    { "expertise", new TextTile() }
                };
    }
}
