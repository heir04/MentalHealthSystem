using MentalHealthSystem.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MentalHealthSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController(IDashboardService dashboardService) : ControllerBase
    {
        private readonly IDashboardService _dashboardService = dashboardService;

        [HttpGet("User")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetUserDashboard()
        {
            var response = await _dashboardService.GetUserDashboard();
            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpGet("Therapist")]
        [Authorize(Roles = "Therapist")]
        public async Task<IActionResult> GetTherapistDashboard()
        {
            var response = await _dashboardService.GetTherapistDashboard();
            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpGet("Admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAdminDashboard()
        {
            var response = await _dashboardService.GetAdminDashboard();
            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpGet("Reports")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetReportsDashboard()
        {
            var response = await _dashboardService.GetReportsDashboard();
            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpGet("General")]
        [Authorize]
        public async Task<IActionResult> GetGeneralDashboard()
        {
            var response = await _dashboardService.GetGeneralDashboard();
            return response.Status ? Ok(response) : BadRequest(response);
        }
    }
}
