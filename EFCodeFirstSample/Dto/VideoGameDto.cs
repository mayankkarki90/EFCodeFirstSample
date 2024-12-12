using DataContracts.Models;

namespace EFCodeFirstSample.Dto
{
    public class VideoGameDto
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public VideoGameDetail VideoGameDetail { get; set; }

        public PublisherDto Publisher { get; set; }
    }
}
