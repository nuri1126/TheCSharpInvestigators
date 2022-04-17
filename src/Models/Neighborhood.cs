using System.Text.Json;

namespace LetsGoSEA.WebSite.Models
{
    public class Neighborhood
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string shortDesc { get; set; }
        public string[] Comments { get; set; }

        public override string ToString() => JsonSerializer.Serialize<Neighborhood>(this);
    }
}