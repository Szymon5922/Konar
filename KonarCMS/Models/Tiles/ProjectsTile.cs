using KonarCMS.Interfaces;
using Newtonsoft.Json;

namespace KonarCMS.Models.Tiles
{
    public class ProjectsTile : TileBase, IImagesContainer
    {
        public HashSet<string> Images { get; set; } = new();
    }
}
