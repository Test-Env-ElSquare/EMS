using BLL.Dtos;
using DAL.Context;
using DAL.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;

namespace DAL.Repositories.Implementation
{
    public class DashboardRepository : IDashboradRepository

    {
        private readonly EmsContext _emsContext;

        public DashboardRepository(EmsContext emsContext)
        {
            _emsContext = emsContext;
        }

        public async Task<List<TransformerEnergyDto>> GetTotalEnergyPerTransformer(int factoryId, DateTime startTime, DateTime endTime, bool isCurrentShift)
        {
            if (isCurrentShift)
            {
                return await _emsContext.VW_TransformerAnalysis
                    .Where(x => x.ShiftStartTime >= startTime && x.FactoryId == factoryId)
                    .GroupBy(x => new { x.TransformerId, x.TransformerName })
                    .Select(g => new TransformerEnergyDto
                    {
                        TransformerId = g.Key.TransformerId,
                        TransformerName = g.Key.TransformerName,
                        TotalEnergyConsumption = g.Sum(x => x.TotalEnergyConsumption)
                    })
                    .ToListAsync();
            }
            else
            {
                return await _emsContext.TransformerAnalysis
                    .Where(x => x.ShiftStartTime >= startTime && x.ShiftStartTime <= endTime && x.FactoryId == factoryId)
                    .GroupBy(x => new { x.TransformerId, x.TransformerName })
                    .Select(g => new TransformerEnergyDto
                    {
                        TransformerId = g.Key.TransformerId,
                        TransformerName = g.Key.TransformerName,
                        TotalEnergyConsumption = g.Sum(x => x.TotalEnergyConsumption)
                    })
                    .ToListAsync();
            }
        }

        public async Task<List<TransformerHourlyEnergyDto>> GetHourlyEnergyPerTransformer(int factoryId, DateTime startTime, DateTime endTime, bool isCurrentShift)
        {
            if (isCurrentShift)
            {
                return await _emsContext.VW_TransformerHourlyAnalysis
                    .Where(x => x.ShiftStartTime >= startTime && x.FactoryId == factoryId)
                    .GroupBy(x => new { x.FactoryId, x.TransformerId, x.TransformerName, x.HourStartTime })
                    .Select(g => new TransformerHourlyEnergyDto
                    {
                        FactoryId = g.Key.FactoryId,
                        TransformerId = g.Key.TransformerId,
                        TransformerName = g.Key.TransformerName,
                        HourStartTime = g.Key.HourStartTime,
                        TotalEnergyConsumption = g.Sum(x => x.TotalEnergyConsumption)
                    })
                    .ToListAsync();
            }
            else
            {
                return await _emsContext.TransformerHourlyAnalysis
                    .Where(x => x.ShiftStartTime >= startTime && x.ShiftStartTime <= endTime && x.FactoryId == factoryId)
                    .GroupBy(x => new { x.FactoryId, x.TransformerId, x.TransformerName, x.HourStartTime })
                    .Select(g => new TransformerHourlyEnergyDto
                    {
                        FactoryId = g.Key.FactoryId,
                        TransformerId = g.Key.TransformerId,
                        TransformerName = g.Key.TransformerName,
                        HourStartTime = g.Key.HourStartTime,
                        TotalEnergyConsumption = g.Sum(x => x.TotalEnergyConsumption)
                    })
                    .ToListAsync();
            }
        }
    }
}
