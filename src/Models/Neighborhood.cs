using System.Text.Json;

namespace ContosoCrafts.WebSite.Models
{
    public class Neighbourhood
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string[] Comments { get; set; }

        public override string ToString() => JsonSerializer.Serialize<Neighbourhood>(this);
    }
}