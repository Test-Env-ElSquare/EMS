using DAL.Context;
using DAL.Models.RealTime;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignalController : ControllerBase
    {
        private readonly EmsContext _context;

        public SignalController(EmsContext context)
        {
            _context = context;
        }
        [HttpPost("Signal")]
        public async Task<IActionResult> Signal(
             string machineName,
             DateTime timeStamp,
             DateTime shiftStartTime,
             int state,
             int count,
             int countDiff,
             int qreject,
             int qrejectDiff,
             string? stateType,
             string? stateReason,
             string? sku,
             int fault,
             decimal speed)
        {
            var signal = new Signal
            {
                MachineName = machineName,
                TimeStamp = timeStamp,
                ShiftStartTime = shiftStartTime,
                State = state,
                Count = count,
                CountDiff = countDiff,
                Qreject = qreject,
                QrejectDiff = qrejectDiff,
                StateType = stateType,
                StateReason = stateReason,
                SKU = sku,
                Fault = fault,
                Speed = speed
            };

            _context.Signals.Add(signal);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
