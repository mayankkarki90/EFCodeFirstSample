using DataContracts.Models;

namespace EFCodeFirstSample.Dto
{
    public class VideoGameDto
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public VideoGameDetailDto Details { get; set; }

        public PublisherDto Publisher { get; set; }

        public List<GenreDto> Genres { get; set; }
    }
}
