using MentalHealthSystem.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MentalHealthSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("User")]
        [Authorize]
        public async Task<IActionResult> GetUserDashboard()
        {
            var response = await _dashboardService.GetUserDashboard();
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
    }
}
