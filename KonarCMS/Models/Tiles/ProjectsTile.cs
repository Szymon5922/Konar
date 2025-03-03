using Newtonsoft.Json;

namespace KonarCMS.Models.Tiles
{
    public class ProjectsTile : TileBase
    {
        public HashSet<string> Images { get; set; } = new();
    }
}
