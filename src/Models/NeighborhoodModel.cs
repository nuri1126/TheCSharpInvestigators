using System.Text.Json;

namespace LetsGoSEA.WebSite.Models
{
    public class NeighborhoodModel
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = "Default";
        public string Image { get; set; } = "Default";
        public string City { get; set; } = "Default";
        public string State { get; set; } = "Default";
        public string ShortDesc { get; set; } = "Default";
        public string[] Comments { get; set; } =  {"Default"};

    public override string ToString() => JsonSerializer.Serialize<NeighborhoodModel>(this);
    }
}