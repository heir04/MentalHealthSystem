using MentalHealthSystem.Application.DTOs;
using MentalHealthSystem.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MentalHealthSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReactionController : ControllerBase
    {
        private readonly IReactionService _reactionService;

        public ReactionController(IReactionService reactionService)
        {
            _reactionService = reactionService;
        }

        [HttpPost("Story/{storyId}/React")]
        [Authorize]
        public async Task<IActionResult> ReactToStory([FromRoute] Guid storyId, [FromBody] CreateReactionDto reactionDto)
        {
            var response = await _reactionService.ReactToStoryAsync(storyId, reactionDto);
            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpPost("Comment/{commentId}/React")]
        [Authorize]
        public async Task<IActionResult> ReactToComment([FromRoute] Guid commentId, [FromBody] CreateReactionDto reactionDto)
        {
            var response = await _reactionService.ReactToCommentAsync(commentId, reactionDto);
            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpPost("RemoveReact/{storyId}/Story")]
        [Authorize]
        public async Task<IActionResult> RemoveReactionFromStory([FromRoute] Guid storyId)
        {
            var response = await _reactionService.RemoveReactionFromStoryAsync(storyId);
            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpPost("RemoveReact/{commentId}/Comment")]
        [Authorize]
        public async Task<IActionResult> RemoveReactionFromComment([FromRoute] Guid commentId)
        {
            var response = await _reactionService.RemoveReactionFromCommentAsync(commentId);
            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpGet("Story/{storyId}/Reactions")]
        [Authorize]
        public async Task<IActionResult> GetStoryReactions([FromRoute] Guid storyId)
        {
            var response = await _reactionService.GetStoryReactionsAsync(storyId);
            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpGet("Comment/{commentId}/Reactions")]
        [Authorize]
        public async Task<IActionResult> GetCommentReactions([FromRoute] Guid commentId)
        {
            var response = await _reactionService.GetCommentReactionsAsync(commentId);
            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpGet("Story/{storyId}/Reactions/Summary")]
        [Authorize]
        public async Task<IActionResult> GetStoryReactionSummary([FromRoute] Guid storyId)
        {
            var response = await _reactionService.GetStoryReactionSummaryAsync(storyId);
            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpGet("Comment/{commentId}/Reactions/Summary")]
        [Authorize]
        public async Task<IActionResult> GetCommentReactionSummary([FromRoute] Guid commentId)
        {
            var response = await _reactionService.GetCommentReactionSummaryAsync(commentId);
            return response.Status ? Ok(response) : BadRequest(response);
        }
    }
}
