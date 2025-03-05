using Newtonsoft.Json;

namespace KonarCMS.Models
{
    public class OverallData : SerializableObject
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> OfferList { get; set; } = new();
        public List<string> ProjectsList { get; set; } = new();
        public HashSet<string> Photos { get; set; } = new();
    }
}
