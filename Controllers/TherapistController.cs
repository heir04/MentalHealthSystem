using MentalHealthSystem.Application.DTOs;
using MentalHealthSystem.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MentalHealthSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TherapistController(ITherapistService therapistService) : ControllerBase
    {
        private readonly ITherapistService _therapistService = therapistService;

        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateTherapistDto therapistDto)
        {
            var response = await _therapistService.Create(therapistDto);
            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpPut("Update/{id}")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateTherapistDto therapistDto)
        {
            var response = await _therapistService.Update(id, therapistDto);
            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpPost("Delete/{id}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var response = await _therapistService.Delete(id);
            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _therapistService.GetAll();
            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var response = await _therapistService.Get(id);
            return response.Status ? Ok(response) : BadRequest(response);
        }
    }
}
