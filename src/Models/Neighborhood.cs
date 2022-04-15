using System.Text.Json;

namespace ContosoCrafts.WebSite.Models
{
    public class Neighborhood
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string shortDesc { get; set; }
        public string[] Comments { get; set; }

        public override string ToString() => JsonSerializer.Serialize<Neighborhood>(this);
    }
}