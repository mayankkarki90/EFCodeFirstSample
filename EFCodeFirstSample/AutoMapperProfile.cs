using AutoMapper;
using DataContracts.Models;
using EFCodeFirstSample.Dto;

namespace EFCodeFirstSample
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<VideoGame, VideoGameDto>().ReverseMap();
            CreateMap<Publisher, PublisherDto>().ReverseMap();
        }
    }
}
