using DAL.Context;
using DAL.Models.RealTime;
using Microsoft.AspNetCore.Mvc;

namespace EMS
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnergyController : ControllerBase
    {
        private readonly EmsContext _context;

        public EnergyController(EmsContext context)
        {
            _context = context;
        }


        [HttpGet("Energy")]
        public async Task<ActionResult> Energy(
           decimal V1, decimal V2, decimal V3,
           decimal I1, decimal I2, decimal I3,
           decimal Pf1, decimal Pf2, decimal Pf3,
           decimal Pact1, decimal Pact2, decimal Pact3,
           decimal Preact1, decimal Preact2, decimal Preact3,
           decimal Papp1, decimal Papp2, decimal Papp3,
           decimal Energy1, decimal Energy2, decimal Energy3,
           decimal THDi1, decimal THDi2, decimal THDi3,
           decimal THDv1, decimal THDv2, decimal THDv3,
           string id,
           DateTime? timeStamp)
        {
            try
            {
                var actualTime = timeStamp ?? DateTime.Now;

                DateTime shiftStartTime;
                if (actualTime.TimeOfDay >= TimeSpan.FromHours(8) &&
                    actualTime.TimeOfDay < TimeSpan.FromHours(20))
                {
                    shiftStartTime = actualTime.Date.AddHours(8);
                }
                else if (actualTime.TimeOfDay >= TimeSpan.FromHours(20))
                {
                    shiftStartTime = actualTime.Date.AddHours(20);
                }
                else
                {
                    shiftStartTime = actualTime.Date.AddDays(-1).AddHours(20);
                }

                var reading = new Energy
                {
                    V1 = V1,
                    V2 = V2,
                    V3 = V3,
                    I1 = I1,
                    I2 = I2,
                    I3 = I3,
                    PF1 = Pf1,
                    PF2 = Pf2,
                    PF3 = Pf3,
                    Pact1 = Pact1,
                    Pact2 = Pact2,
                    Pact3 = Pact3,
                    Preact1 = Preact1,
                    Preact2 = Preact2,
                    Preact3 = Preact3,
                    Papp1 = Papp1,
                    Papp2 = Papp2,
                    Papp3 = Papp3,
                    Energy1 = Energy1,
                    Energy2 = Energy2,
                    Energy3 = Energy3,
                    THDi1 = THDi1,
                    THDi2 = THDi2,
                    THDi3 = THDi3,
                    THDv1 = THDv1,
                    THDv2 = THDv2,
                    THDv3 = THDv3,
                    SourceId = id,
                    TimeStamp = actualTime,
                    ShiftStartTime = shiftStartTime,

                    //
                    AvgVoltage = (V1 + V2 + V3) / 3,
                    AvgCurrent = (I1 + I2 + I3) / 3,
                    AvgPowerFactor = (Pf1 + Pf2 + Pf3) / 3,
                    AvgPact = (Pact1 + Pact2 + Pact3) / 3,
                    AvgPreact = (Preact1 + Preact2 + Preact3) / 3,
                    AvgPapp = (Papp1 + Papp2 + Papp3) / 3,
                    SumEnergy = Energy1 + Energy2 + Energy3,
                    AvgTHDi = (THDi1 + THDi2 + THDi3) / 3,
                    AvgTHDv = (THDv1 + THDv2 + THDv3) / 3
                };

                var lastRead = _context.Energies
                    .OrderByDescending(r => r.Id)
                    .Where(r => r.SourceId == reading.SourceId && r.TimeStamp < reading.TimeStamp)
                    .FirstOrDefault();

                if (lastRead != null)
                {
                    if (reading.Energy1 < lastRead.Energy1 ||
                        reading.Energy2 < lastRead.Energy2 ||
                        reading.Energy3 < lastRead.Energy3)
                    {
                        reading.DiffEnergy1 = reading.Energy1;
                        reading.DiffEnergy2 = reading.Energy2;
                        reading.DiffEnergy3 = reading.Energy3;
                        reading.Fault = 1;
                    }
                    else
                    {
                        reading.DiffEnergy1 = reading.Energy1 - lastRead.Energy1;
                        reading.DiffEnergy2 = reading.Energy2 - lastRead.Energy2;
                        reading.DiffEnergy3 = reading.Energy3 - lastRead.Energy3;
                        reading.Fault = 0;
                    }
                }
                else
                {
                    reading.DiffEnergy1 = reading.Energy1;
                    reading.DiffEnergy2 = reading.Energy2;
                    reading.DiffEnergy3 = reading.Energy3;
                    reading.Fault = 1;
                }
                reading.TotalEnergyConsumptionDiff = reading.DiffEnergy1 + reading.DiffEnergy2 + reading.DiffEnergy3;
                _context.Energies.Add(reading);
                await _context.SaveChangesAsync();

                return Ok($"Ok,{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}");
            }
            catch (Exception e)
            {
                return BadRequest($"message: {e.Message} | stacktrace: {e.StackTrace}");
            }
        }
    }
}