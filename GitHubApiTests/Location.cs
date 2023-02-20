using System.Text.Json.Serialization;

namespace GitHubApiTests
{
    internal class Location
    {
        [JsonPropertyName("country")]
        public string Country { get; set; }
    }
}