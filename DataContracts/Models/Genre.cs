using System.Text.Json.Serialization;

namespace DataContracts.Models
{
    public class Genre
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public List<VideoGame> VideoGames { get; set; }
    }
}
