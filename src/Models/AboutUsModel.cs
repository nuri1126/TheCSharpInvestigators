namespace LetsGoSEA.WebSite.Models
{
    /// <summary>
    /// AboutUsModel represents an individual developer profile.
    /// </summary>
    public class AboutUsModel
    {
        // Developer name.
        public string name { get; set; } = "Default";

        // LinkedIn web address to developer profile.
        public string linkedIn { get; set; } = "Default";

        // Developer photo. 
        public string image { get; set; } = "Default";

        // Short bio. 
        public string bio { get; set; } = "Default";
    }
}