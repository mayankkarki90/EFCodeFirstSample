namespace DataContracts.Models
{
    public class VideoGameDetail
    {
        public int Id { get; set; }

        public int VideoGameId { get; set; }

        public string Description { get; set; }

        public DateTime? ReleaseDate { get; set; }
    }
}
