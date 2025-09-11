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

        /// <summary>
        /// React to a story (Like, Love, Laugh, Sad, etc.)
        /// </summary>
        [HttpPost("Story/{storyId}")]
        [Authorize]
        public async Task<IActionResult> ReactToStory([FromRoute] Guid storyId, [FromBody] CreateReactionDto reactionDto)
        {
            var response = await _reactionService.ReactToStoryAsync(storyId, reactionDto);
            return response.Status ? Ok(response) : BadRequest(response);
        }

        /// <summary>
        /// React to a comment (Like, Love, Laugh, Sad, etc.)
        /// </summary>
        [HttpPost("Comment/{commentId}")]
        [Authorize]
        public async Task<IActionResult> ReactToComment([FromRoute] Guid commentId, [FromBody] CreateReactionDto reactionDto)
        {
            var response = await _reactionService.ReactToCommentAsync(commentId, reactionDto);
            return response.Status ? Ok(response) : BadRequest(response);
        }

        /// <summary>
        /// Remove reaction from a story
        /// </summary>
        [HttpDelete("Story/{storyId}")]
        [Authorize]
        public async Task<IActionResult> RemoveReactionFromStory([FromRoute] Guid storyId)
        {
            var response = await _reactionService.RemoveReactionFromStoryAsync(storyId);
            return response.Status ? Ok(response) : BadRequest(response);
        }

        /// <summary>
        /// Remove reaction from a comment
        /// </summary>
        [HttpDelete("Comment/{commentId}")]
        [Authorize]
        public async Task<IActionResult> RemoveReactionFromComment([FromRoute] Guid commentId)
        {
            var response = await _reactionService.RemoveReactionFromCommentAsync(commentId);
            return response.Status ? Ok(response) : BadRequest(response);
        }

        /// <summary>
        /// Get all reactions for a story
        /// </summary>
        [HttpGet("Story/{storyId}")]
        public async Task<IActionResult> GetStoryReactions([FromRoute] Guid storyId)
        {
            var response = await _reactionService.GetStoryReactionsAsync(storyId);
            return response.Status ? Ok(response) : BadRequest(response);
        }

        /// <summary>
        /// Get all reactions for a comment
        /// </summary>
        [HttpGet("Comment/{commentId}")]
        public async Task<IActionResult> GetCommentReactions([FromRoute] Guid commentId)
        {
            var response = await _reactionService.GetCommentReactionsAsync(commentId);
            return response.Status ? Ok(response) : BadRequest(response);
        }

        /// <summary>
        /// Get reaction summary for a story (counts by type)
        /// </summary>
        [HttpGet("Story/{storyId}/Summary")]
        public async Task<IActionResult> GetStoryReactionSummary([FromRoute] Guid storyId)
        {
            var response = await _reactionService.GetStoryReactionSummaryAsync(storyId);
            return response.Status ? Ok(response) : BadRequest(response);
        }

        /// <summary>
        /// Get reaction summary for a comment (counts by type)
        /// </summary>
        [HttpGet("Comment/{commentId}/Summary")]
        public async Task<IActionResult> GetCommentReactionSummary([FromRoute] Guid commentId)
        {
            var response = await _reactionService.GetCommentReactionSummaryAsync(commentId);
            return response.Status ? Ok(response) : BadRequest(response);
        }
    }
}
