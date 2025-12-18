using MentalHealthSystem.Application.Interfaces.IServices;
using MentalHealthSystem.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MentalHealthSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TherapySessionController(ITherapySessionService therapySessionService) : ControllerBase
    {
        private readonly ITherapySessionService _therapySessionService = therapySessionService;

        [HttpPost("BookSession/{therapistId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> BookSession([FromRoute] Guid therapistId)
        {
            var response = await _therapySessionService.BookSession(therapistId);
            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpGet("Get/{id}")]
        [Authorize]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var response = await _therapySessionService.Get(id);
            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpGet("GetAllByTherapist")]
        [Authorize(Roles = "Therapist")]
        public async Task<IActionResult> GetAllByTherapist()
        {
            var response = await _therapySessionService.GetAllByTherapist();
            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpGet("GetAllByActiveUser")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetAllByActiveUser()
        {
            var response = await _therapySessionService.GetAllByActiveUser();
            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpPut("UpdateSessionStatus/{id}")]
        [Authorize(Roles = "Therapist")]
        public async Task<IActionResult> UpdateSessionStatus([FromRoute] Guid id, [FromBody] TherapySessionStatus statusDto)
        {
            var response = await _therapySessionService.UpdateSessionStatus(id, statusDto);
            return response.Status ? Ok(response) : BadRequest(response);
        }
    }
}
