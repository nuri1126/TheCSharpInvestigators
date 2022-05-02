using System.Text.Json;

namespace LetsGoSEA.WebSite.Models
{
    /// <summary>
    /// AboutUsModel represents an individual developer profile
    /// </summary>
    public class AboutUsModel
    {
        // Primary key Id
        public int Id { get; set; } = 0;

        // Developer name
        public string Name { get; set; } = "Default";

        // LinkedIn web address to developer profile
        public string LinkedIn { get; set; } = "Default";

        // Developer photo 
        public string Image { get; set; } = "Default";

        // Short bio 
        public string Bio { get; set; } = "Default";

        /// <summary>
        /// Converts Model to JSON
        /// </summary>
        /// <returns>A string of all the team member data in JSON format</returns>
        public override string ToString() => JsonSerializer.Serialize<AboutUsModel>(this);
    }
}