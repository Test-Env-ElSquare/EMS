using BLL.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Controller
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

        [HttpGet("GetTotalEnergyPerTransformer")]
        public async Task<IActionResult> GetTotalEnergyPerTransformer(int factoryId, int duration, DateTime? From, DateTime? To)
        {
            try
            {
                var result = await _dashboardService.GetTotalEnergyPerTransformer(factoryId, duration, From, To);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [HttpGet("GetHourlyEnergyPerTransformer")]
        public async Task<IActionResult> GetHourlyEnergyPerTransformer(int factoryId, int duration, DateTime? From, DateTime? To)
        {
            try
            {
                var result = await _dashboardService.GetHourlyEnergyPerTransformer(factoryId, duration, From, To);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}