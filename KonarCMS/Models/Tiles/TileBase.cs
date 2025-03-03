using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KonarCMS.Models.Tiles
{
    public class TileBase:SerializableObject
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
