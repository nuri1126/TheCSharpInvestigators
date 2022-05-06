using System.ComponentModel.DataAnnotations.Schema;
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
        [NotMapped] public string[] Comments { get; set; } = {"Default"};

        // Address of neighborhood
        public string Address { get; set; } = "Default";

        // Latitude of neighborhood
        public float Latitude { get; set; } = -1;

        // Longitude of neighborhood
        public float Longitude { get; set; } = -1;

        // Walk score of the neighborhood
        public int WalkScore { get; set; } = -1;

        // Walk Score Description
        public string WalkScoreDescription { get; set; } = "Default";

        // Bike Score of the neighborhood
        public int BikeScore { get; set; } = -1;

        // Bike Score Description
        public string BikeScoreDescription { get; set; } = "Default";

        /// <summary>
        /// Converts Model to JSON
        /// </summary>
        /// <returns>A string of all the neighborhood data in JSON format</returns>
        public override string ToString() => JsonSerializer.Serialize<NeighborhoodModel>(this);
    }
}