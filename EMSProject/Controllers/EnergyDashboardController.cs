using BLL.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Controllers
{
    [ApiController]
    [Route("api/energy-dashboard")]
    public class EnergyDashboardController : ControllerBase
    {
        private readonly IEnergyDashboardService _energyDashboardService;

        public EnergyDashboardController(IEnergyDashboardService energyDashboardService)
        {
            _energyDashboardService = energyDashboardService;
        }

        [HttpGet("GetSummary")]
        public async Task<IActionResult> GetSummary(int? factoryId, int duration = 0, DateTime? From = null, DateTime? To = null)
        {
            try
            {
                var result = await _energyDashboardService.GetSummary(factoryId, duration, From, To);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpGet("GetOnlineStatus")]
        public async Task<IActionResult> GetOnlineStatus(int? factoryId, int duration = 0, DateTime? From = null, DateTime? To = null)
        {
            try
            {
                var result = await _energyDashboardService.GetOnlineStatus(factoryId, duration, From, To);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("GetVoltageGraph")]
        public async Task<IActionResult> GetVoltageGraph(
    [FromQuery] int factoryId,
    [FromQuery] int duration = 0,
    [FromQuery] DateTime? from = null,
    [FromQuery] DateTime? to = null)
        {
            var result = await _energyDashboardService.GetVoltageGraph(
                factoryId,
                duration,
                from,
                to
            );

            return Ok(result);
        }
        [HttpGet("GetCurrentGraph")]
        public async Task<IActionResult> GetCurrentGraph(
    [FromQuery] int factoryId,
    [FromQuery] int duration = 0,
    [FromQuery] DateTime? from = null,
    [FromQuery] DateTime? to = null)
        {
            var result = await _energyDashboardService.GetCurrentGraph(
                factoryId,
                duration,
                from,
                to
            );

            return Ok(result);
        }
        [HttpGet("GetHarmonicsGraph")]
        public async Task<IActionResult> GetHarmonicsGraph(
    [FromQuery] int factoryId,
    [FromQuery] int duration = 0,
    [FromQuery] DateTime? from = null,
    [FromQuery] DateTime? to = null)
        {
            var result = await _energyDashboardService.GetHarmonicsGraph(
                factoryId,
                duration,
                from,
                to
            );

            return Ok(result);
        }




        [HttpGet("GetEnergyHeatmap")]
        public async Task<IActionResult> GetEnergyHeatmap(
    [FromQuery] int factoryId,
    [FromQuery] int? transformerId = null,
    [FromQuery] int? zoneId = null,
    [FromQuery] int duration = 0,
    [FromQuery] DateTime? from = null,
    [FromQuery] DateTime? to = null)
        {
            var result = await _energyDashboardService.GetEnergyHeatmap(
                factoryId,
                transformerId,
                zoneId,
                duration,
                from,
                to
            );

            return Ok(result);
        }




        [HttpGet("GetTotalEnergy")]
        public async Task<IActionResult> GetTotalEnergy(
    [FromQuery] int factoryId,
    [FromQuery] int? transformerId,
    [FromQuery] int? zoneId,
    [FromQuery] int duration = 0,
    [FromQuery] DateTime? from = null,
    [FromQuery] DateTime? to = null)
        {
            var result = await _energyDashboardService.GetTotalEnergyPerTransformer(
                factoryId,
                transformerId,
                zoneId,
                duration,
                from,
                to
            );

            return Ok(result);
        }

        [HttpGet("GetHourlyEnergy")]
        public async Task<IActionResult> GetHourlyEnergy(
            [FromQuery] int factoryId,
            [FromQuery] int? transformerId,
            [FromQuery] int? zoneId,
            [FromQuery] int duration = 0,
            [FromQuery] DateTime? from = null,
            [FromQuery] DateTime? to = null)
        {
            var result = await _energyDashboardService.GetHourlyEnergyPerTransformer(
                factoryId,
                transformerId,
                zoneId,
                duration,
                from,
                to
            );

            return Ok(result);
        }

        [HttpGet("GetTopEnergyConsumers")]
        public async Task<IActionResult> GetTopEnergyConsumers(
            [FromQuery] int factoryId,
            [FromQuery] int? transformerId,
            [FromQuery] int? zoneId,
            [FromQuery] int duration = 0,
            [FromQuery] DateTime? from = null,
            [FromQuery] DateTime? to = null,
            [FromQuery] int top = 5)
        {
            var result = await _energyDashboardService.GetTopEnergyConsumers(
                factoryId,
                transformerId,
                zoneId,
                duration,
                from,
                to,
                top
            );

            return Ok(result);
        }
    }
}
