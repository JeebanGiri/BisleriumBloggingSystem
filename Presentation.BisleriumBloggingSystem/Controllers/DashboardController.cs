using Application.BisleriumBloggingSystem.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.BisleriumBloggingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("dashboard/stats")]
        public async Task<IActionResult> GetBlogStats()
        {
            try
            {
                // Get the dashboard statistics
                var stats = await _dashboardService.GetBlogStats();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to retrieve blog statistics: {ex.Message}");
            }
        }
    }
}
