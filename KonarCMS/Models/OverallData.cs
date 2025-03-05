using KonarCMS.Interfaces;
using Newtonsoft.Json;

namespace KonarCMS.Models
{
    public class OverallData : SerializableObject, IImagesContainer
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> OfferList { get; set; } = new();
        public List<string> ProjectsList { get; set; } = new();
        public HashSet<string> Images { get; set; } = new();
    }
}
