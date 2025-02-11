namespace DataContracts.Models
{
    public class VideoGame
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public int PublisherId { get; set; }

        public Publisher Publisher { get; set; }

        public VideoGameDetail Details { get; set; }

        public List<Genre> Genres { get; set; }
    }
}
