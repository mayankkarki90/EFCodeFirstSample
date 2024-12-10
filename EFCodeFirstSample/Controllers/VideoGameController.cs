using AutoMapper;
using DataContracts.Services;
using EFCodeFirstSample.Dto;
using Microsoft.AspNetCore.Mvc;

namespace EFCodeFirstSample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VideoGameController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IVideoGameService _videoGameService;

        public VideoGameController(IMapper mapper, IVideoGameService videoGameService)
        {
            _mapper = mapper;
            _videoGameService = videoGameService;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<VideoGameDto>>> GetAllAsync()
        {
            var games = await _videoGameService.GetAllAsync();
            var response = _mapper.Map<List<VideoGameDto>>(games);
            return Ok(response);
        }
    }
}
