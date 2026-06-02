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

        [HttpGet("GetVoltageGraph")]
        public async Task<IActionResult> GetVoltageGraph(int factoryId, int duration, DateTime? From, DateTime? To)
        {
            try
            {
                var result = await _dashboardService.GetVoltageStability(factoryId, duration, From, To);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("GetCurrentGraph")]
        public async Task<IActionResult> GetCurrentGraph(int factoryId, int duration, DateTime? From, DateTime? To)
        {
            try
            {
                var result = await _dashboardService.GetCurrentFluctuation(factoryId, duration, From, To);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("GetHarmonicsGraph")]
        public async Task<IActionResult> GetHarmonicsGraph(int factoryId, int duration, DateTime? From, DateTime? To)
        {
            try
            {
                var result = await _dashboardService.GetHarmonicsLevel(factoryId, duration, From, To);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("GetTransformerSummary")]
        public async Task<IActionResult> GetTransformerSummary(int factoryId, int duration, DateTime? From, DateTime? To)
        {
            try
            {
                var result = await _dashboardService.GetTransformerSummary(factoryId, duration, From, To);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("GetTopEnergyConsumers")]
        public async Task<IActionResult> GetTopEnergyConsumers(int factoryId, int duration, DateTime? From, DateTime? To, int top = 10)
        {
            try
            {
                var result = await _dashboardService.GetTopEnergyConsumers(factoryId, duration, From, To, top);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}