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
        public async Task<IActionResult> Signal([FromBody] Signal signal)
        {
            _context.Signals.Add(signal);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
