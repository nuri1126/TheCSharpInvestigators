using System.Text.Json;

namespace LetsGoSEA.WebSite.Models
{
    /// <summary>
    /// NeighborhoodModel represents a single neighborhoods and its characteristics
    /// </summary>
    public class NeighborhoodModel
    {
        // Primary key Id
        public int Id { get; set; } = 0;

        // Neighborhood name, eg "West Seattle" 
        public string Name { get; set; } = "Default";

        // Image of the neighborhood 
        public string Image { get; set; } = "Default";

        // City 
        public string City { get; set; } = "Default";

        // State
        public string State { get; set; } = "Default";

        // Short description of the neighborhood
        public string ShortDesc { get; set; } = "Default";

        // Array of comments from registered users 
        public string[] Comments { get; set; } =  {"Default"};

        // Converts Model to JSON
        public override string ToString() => JsonSerializer.Serialize<NeighborhoodModel>(this);
    }
}