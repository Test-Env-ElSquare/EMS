using BLL.Dtos;
using DAL.Repositories.Interface;
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

        public async Task<List<TopEnergyConsumerDto>> GetTopEnergyConsumers(int factoryId, DateTime startTime, DateTime endTime, bool isCurrentShift, int top)
        {
            List<TransformerEnergyDto> data;

            if (isCurrentShift)
            {
                data = await _emsContext.VW_TransformerAnalysis
                    .Where(x => x.FactoryId == factoryId)
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
                data = await _emsContext.TransformerAnalysis
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

            decimal totalEnergy = data.Sum(x => x.TotalEnergyConsumption);

            return data
                .OrderByDescending(x => x.TotalEnergyConsumption)
                .Take(top)
                .Select((x, index) => new TopEnergyConsumerDto
                {
                    Rank = index + 1,
                    TransformerId = x.TransformerId,
                    TransformerName = x.TransformerName,
                    TotalEnergyConsumption = x.TotalEnergyConsumption,
                    PercentageOfTotal = totalEnergy > 0
                        ? Math.Round(x.TotalEnergyConsumption / totalEnergy * 100, 0)
                        : 0
                })
                .ToList();
        }

        public async Task<List<VoltageStabilityDto>> GetVoltageStability(int factoryId, DateTime startTime, DateTime endTime, bool isCurrentShift)
        {
            //if (isCurrentShift)
            //{
            //    return await _emsContext.VW_TransformerHourlyAnalysis
            //        .Where(x => x.HourStartTime >= startTime && x.HourStartTime <= endTime && x.FactoryId == factoryId && x.Status == "Online")
            //        .GroupBy(x => new { x.FactoryId, x.HourStartTime })
            //        .Select(g => new VoltageStabilityDto
            //        {
            //            FactoryId = g.Key.FactoryId,
            //            HourStartTime = g.Key.HourStartTime,
            //            AvgVoltage = g.Average(x => x.Voltage)
            //        })
            //        .OrderBy(x => x.HourStartTime)
            //        .ToListAsync();
            //}
            if (isCurrentShift)
            {
                return await _emsContext.VW_TransformerHourlyAnalysis
                    .Where(x =>
                              x.FactoryId == factoryId
                             && x.Status == "Online")
                    .GroupBy(x => new { x.FactoryId, x.HourStartTime })
                    .Select(g => new VoltageStabilityDto
                    {
                        FactoryId = g.Key.FactoryId,
                        HourStartTime = g.Key.HourStartTime,
                        AvgVoltage = g.Average(x => x.Voltage)
                    })
                    .OrderBy(x => x.HourStartTime)
                    .ToListAsync();
            }
            else
            {
                return await _emsContext.TransformerHourlyAnalysis
                    .Where(x => x.ShiftStartTime >= startTime && x.ShiftStartTime <= endTime && x.FactoryId == factoryId && x.Status == "Online")
                    .GroupBy(x => new { x.FactoryId, x.HourStartTime })
                    .Select(g => new VoltageStabilityDto
                    {
                        FactoryId = g.Key.FactoryId,
                        HourStartTime = g.Key.HourStartTime,
                        AvgVoltage = g.Average(x => x.Voltage)
                    })
                    .OrderBy(x => x.HourStartTime)
                    .ToListAsync();
            }
        }

        public async Task<List<CurrentFluctuationDto>> GetCurrentFluctuation(int factoryId, DateTime startTime, DateTime endTime, bool isCurrentShift)
        {
            if (isCurrentShift)
            {
                return await _emsContext.VW_TransformerHourlyAnalysis
                    .Where(x => x.FactoryId == factoryId && x.Status == "Online")
                    .GroupBy(x => new { x.FactoryId, x.HourStartTime })
                    .Select(g => new CurrentFluctuationDto
                    {
                        FactoryId = g.Key.FactoryId,
                        HourStartTime = g.Key.HourStartTime,
                        AvgCurrent = g.Average(x => x.Current)
                    })
                    .OrderBy(x => x.HourStartTime)
                    .ToListAsync();
            }
            else
            {
                return await _emsContext.TransformerHourlyAnalysis
                    .Where(x => x.ShiftStartTime >= startTime && x.ShiftStartTime <= endTime && x.FactoryId == factoryId && x.Status == "Online")
                    .GroupBy(x => new { x.FactoryId, x.HourStartTime })
                    .Select(g => new CurrentFluctuationDto
                    {
                        FactoryId = g.Key.FactoryId,
                        HourStartTime = g.Key.HourStartTime,
                        AvgCurrent = g.Average(x => x.Current)
                    })
                    .OrderBy(x => x.HourStartTime)
                    .ToListAsync();
            }
        }

        public async Task<List<HarmonicsLevelDto>> GetHarmonicsLevel(int factoryId, DateTime startTime, DateTime endTime, bool isCurrentShift)
        {
            if (isCurrentShift)
            {
                return await _emsContext.VW_TransformerHourlyAnalysis
                    .Where(x => x.HourStartTime >= startTime && x.HourStartTime <= endTime && x.FactoryId == factoryId)
                    .GroupBy(x => new { x.FactoryId, x.HourStartTime })
                    .Select(g => new HarmonicsLevelDto
                    {
                        FactoryId = g.Key.FactoryId,
                        HourStartTime = g.Key.HourStartTime,
                        AvgTHDv = g.Average(x => x.AvgTHDv),
                        AvgTHDi = g.Average(x => x.AvgTHDi)
                    })
                    .OrderBy(x => x.HourStartTime)
                    .ToListAsync();
            }
            else
            {
                return await _emsContext.TransformerHourlyAnalysis
                    .Where(x => x.ShiftStartTime >= startTime && x.ShiftStartTime <= endTime && x.FactoryId == factoryId)
                    .GroupBy(x => new { x.FactoryId, x.HourStartTime })
                    .Select(g => new HarmonicsLevelDto
                    {
                        FactoryId = g.Key.FactoryId,
                        HourStartTime = g.Key.HourStartTime,
                        AvgTHDv = g.Average(x => x.AvgTHDv),
                        AvgTHDi = g.Average(x => x.AvgTHDi)
                    })
                    .OrderBy(x => x.HourStartTime)
                    .ToListAsync();
            }
        }


        //public async Task<TransformerSummaryDto> GetTransformerSummary(int factoryId, DateTime startTime, DateTime endTime, bool isCurrentShift)
        //{
        //    int totalTransformers = await _emsContext.Transformers
        //        .Where(x => x.FactoryId == factoryId)
        //        .CountAsync();

        //    if (isCurrentShift)
        //    {
        //        var data = await _emsContext.VW_TransformerAnalysis
        //            .Where(x => x.FactoryId == factoryId)
        //            .ToListAsync();

        //        return new TransformerSummaryDto
        //        {
        //            TotalEnergy = data.Sum(x => x.TotalEnergyConsumption),
        //            TotalTransformers = totalTransformers,
        //            OnlineTransformers = data.Count(x => x.Status == "Online"),
        //            OfflineTransformers = data.Count(x => x.Status == "Offline"),
        //            AvgPowerFactor = data.Any(x => x.Status == "Online")
        //                ? data.Where(x => x.Status == "Online").Average(x => x.PowerFactor)
        //                : 0
        //        };
        //    }
        //    else
        //    {
        //        var data = await _emsContext.TransformerAnalysis
        //            .Where(x => x.ShiftStartTime >= startTime && x.ShiftStartTime <= endTime && x.FactoryId == factoryId)
        //            .ToListAsync();

        //        return new TransformerSummaryDto
        //        {
        //            TotalEnergy = data.Sum(x => x.TotalEnergyConsumption),
        //            TotalTransformers = totalTransformers,
        //            OnlineTransformers = data.Count(x => x.Status == "Online"),
        //            OfflineTransformers = data.Count(x => x.Status == "Offline"),
        //            AvgPowerFactor = data.Any(x => x.Status == "Online")
        //                ? data.Where(x => x.Status == "Online").Average(x => x.PowerFactor)
        //                : 0
        //        };
        //    }
        //}
        //public async Task<TransformerSummaryDto> GetTransformerSummary(int? factoryId, DateTime startTime, DateTime endTime, bool isCurrentShift)
        //{
        //    if (isCurrentShift)
        //    {
        //        // Transformers
        //        var transformerData = await _emsContext.VW_TransformerAnalysis
        //            .Where(x => !factoryId.HasValue || x.FactoryId == factoryId)
        //            .ToListAsync();

        //        // Zones
        //        var zoneData = await _emsContext.VW_ZoneAnalysis
        //            .Where(x => !factoryId.HasValue || x.FactoryId == factoryId)
        //            .ToListAsync();

        //        // Lines
        //        var lineData = await _emsContext.VW_LineAnalysis
        //            .Where(x => !factoryId.HasValue || x.FactoryId == factoryId)
        //            .ToListAsync();

        //        var onlineTransformers = transformerData.Where(x => x.Status == "Online").ToList();

        //        return new TransformerSummaryDto
        //        {
        //            TotalEnergy = transformerData.Sum(x => x.TotalEnergyConsumption),
        //            Transformers = new EntitySummaryDto
        //            {
        //                TotalCount = transformerData.Count,
        //                OnlineCount = transformerData.Count(x => x.Status == "Online"),
        //                OfflineCount = transformerData.Count(x => x.Status == "Offline")
        //            },
        //            Zones = new EntitySummaryDto
        //            {
        //                TotalCount = zoneData.Count,
        //                OnlineCount = zoneData.Count(x => x.Status == "Online"),
        //                OfflineCount = zoneData.Count(x => x.Status == "Offline")
        //            },
        //            Lines = new EntitySummaryDto
        //            {
        //                TotalCount = lineData.Count,
        //                OnlineCount = lineData.Count(x => x.Status == "Online"),
        //                OfflineCount = lineData.Count(x => x.Status == "Offline")
        //            },
        //            AvgPowerFactor = onlineTransformers.Any()
        //                ? onlineTransformers.Average(x => x.PowerFactor)
        //                : 0
        //        };
        //    }
        //    else
        //    {
        //        // Transformers
        //        var transformerData = await _emsContext.TransformerAnalysis
        //            .Where(x => x.ShiftStartTime >= startTime && x.ShiftStartTime <= endTime
        //                && (!factoryId.HasValue || x.FactoryId == factoryId))
        //            .ToListAsync();

        //        // Zones
        //        var zoneData = await _emsContext.ZoneAnalysis
        //            .Where(x => x.ShiftStartTime >= startTime && x.ShiftStartTime <= endTime
        //                && (!factoryId.HasValue || x.FactoryId == factoryId))
        //            .ToListAsync();

        //        // Lines
        //        var lineData = await _emsContext.LineAnalysis
        //            .Where(x => x.ShiftStartTime >= startTime && x.ShiftStartTime <= endTime
        //                && (!factoryId.HasValue || x.FactoryId == factoryId))
        //            .ToListAsync();

        //        var onlineTransformers = transformerData.Where(x => x.Status == "Online").ToList();

        //        return new TransformerSummaryDto
        //        {
        //            TotalEnergy = transformerData.Sum(x => x.TotalEnergyConsumption),
        //            Transformers = new EntitySummaryDto
        //            {
        //                TotalCount = transformerData.Count,
        //                OnlineCount = transformerData.Count(x => x.Status == "Online"),
        //                OfflineCount = transformerData.Count(x => x.Status == "Offline")
        //            },
        //            Zones = new EntitySummaryDto
        //            {
        //                TotalCount = zoneData.Count,
        //                OnlineCount = zoneData.Count(x => x.Status == "Online"),
        //                OfflineCount = zoneData.Count(x => x.Status == "Offline")
        //            },
        //            Lines = new EntitySummaryDto
        //            {
        //                TotalCount = lineData.Count,
        //                OnlineCount = lineData.Count(x => x.Status == "Online"),
        //                OfflineCount = lineData.Count(x => x.Status == "Offline")
        //            },
        //            AvgPowerFactor = onlineTransformers.Any()
        //                ? onlineTransformers.Average(x => x.PowerFactor)
        //                : 0
        //        };
        //    }
        //}

    }
}
