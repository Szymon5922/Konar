using Newtonsoft.Json;

namespace KonarCMS.Models
{
    public abstract class SerializableObject
    {
        public void Serialize(string appDataPath, string file)
        {
            var jsonString = JsonConvert.SerializeObject(this, Formatting.Indented);

            File.WriteAllText(Path.Combine(appDataPath, "json", file+".json"), jsonString);
        }
        public void Deserialize(string appDataPath, string file)
        {
            string jsonFilePath = Path.Combine(appDataPath, "json", file+".json");
            var jsonString = File.ReadAllText(jsonFilePath);

            JsonConvert.PopulateObject(jsonString, this);
        }
    }
}
