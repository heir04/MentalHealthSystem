using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MentalHealthSystem.Application.DTOs;
using MentalHealthSystem.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MentalHealthSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlaggedContentController(IFlaggedContentService flaggedContentService) : ControllerBase
    {
        private readonly IFlaggedContentService _flaggedContentService = flaggedContentService;

        [HttpPost("ReportStory/{storyId}")]
        [Authorize]
        public async Task<IActionResult> ReportStory([FromRoute] Guid storyId, CreateFlaggedContentDto flaggedContentDto)
        {
            var response = await _flaggedContentService.ReportStory(flaggedContentDto, storyId);
            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpPost("ReportComment/{commentId}")]
        [Authorize]
        public async Task<IActionResult> ReportComment([FromRoute] Guid commentId, CreateFlaggedContentDto flaggedContentDto)
        {
            var response = await _flaggedContentService.ReportComment(flaggedContentDto, commentId);
            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpPut("Review/{id}")]
        [Authorize]
        public async Task<IActionResult> Review([FromRoute] Guid id, UpdateFlaggedContentDto updateContentDto)
        {
            var response = await _flaggedContentService.Review(id, updateContentDto);
            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpGet("Get/{id}")]
        [Authorize]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var response = await _flaggedContentService.Get(id);
            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpGet("GetAll")]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var response = await _flaggedContentService.GetAll();
            return response.Status ? Ok(response) : BadRequest(response);
        }
    }
}
