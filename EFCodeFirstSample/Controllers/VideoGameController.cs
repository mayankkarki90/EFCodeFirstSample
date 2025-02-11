using AutoMapper;
using DataContracts.Models;
using DataContracts.Services;
using EFCodeFirstSample.Models.Dto;
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
        /// Gets all video games
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<VideoGameDto>>> GetAllAsync()
        {
            var games = await _videoGameService.GetAllAsync();
            var response = _mapper.Map<List<VideoGameDto>>(games);
            return Ok(response);
        }

        /// <summary>
        /// Gets the video game by code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        [HttpGet("/code/{code}", Name = "GetByCode")]
        public async Task<ActionResult<VideoGameDto>> GetByCodeAsync(string code)
        {
            var videoGame = await _videoGameService.GetByCodeAsync(code);
            if (videoGame == null)
            {
                return NotFound($"Video game with code '{code}' doesn't exist");
            }

            var response = _mapper.Map<VideoGameDto>(videoGame);
            return Ok(response);
        }

        /// <summary>
        /// Create a new video game.
        /// </summary>
        /// <param name="videoGame">The video game.</param>
        /// <returns></returns>
        /// <response code="201">A new video game created</response>
        [HttpPost]
        public async Task<ActionResult<VideoGameDto>> PostAsync(VideoGameDto videoGame)
        {
            var game = _mapper.Map<VideoGame>(videoGame);
            await _videoGameService.AddAsync(game);
            return CreatedAtAction("GetByCode", new { code = videoGame.Code }, videoGame);
        }

        /// <summary>
        /// Update a video game.
        /// </summary>
        /// <param name="videoGame">The video game.</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> PutAsync(VideoGameDto videoGame)
        {
            var existingGame = await _videoGameService.GetByCodeAsync(videoGame.Code);
            if (existingGame == null)
                return NotFound("Video game not found");

            var newGame = _mapper.Map<VideoGame>(videoGame);
            await _videoGameService.UpdateAsync(existingGame.Id, newGame);

            return NoContent();
        }

        /// <summary>
        /// Deletes a video game.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        [HttpDelete("/code/{code}")]
        public async Task<ActionResult> DeleteAsync(string code)
        {
            var existingGame = await _videoGameService.GetByCodeAsync(code);
            if (existingGame == null)
                return NotFound("Video game not found");

            await _videoGameService.DeleteAsync(existingGame.Id);
            return NoContent();
        }
    }
}
