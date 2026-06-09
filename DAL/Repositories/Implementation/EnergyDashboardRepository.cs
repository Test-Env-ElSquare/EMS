//using BLL.Dtos;
//using DAL.Models.Threshold;
//using DAL.Repositories.Interface;
//using Shared.Dtos;
//using Shared.Dtos.EnergyDashboard;
//using Shared.Dtos.Heatmap;
//using Shared.Enum;

//namespace DAL.Repositories.Implementation
//{
//    public class EnergyDashboardRepository : IEnergyDashboardRepository
//    {
//        private readonly EmsContext _emsContext;

//        public EnergyDashboardRepository(EmsContext emsContext)
//        {
//            _emsContext = emsContext;
//        }

//        public async Task<DashboardSummaryDto> GetSummary(
//            int? factoryId,
//            DateTime startTime,
//            DateTime endTime,
//            bool isCurrentShift)
//        {
//            var factoryIds = await GetFactoryIds(factoryId);

//            var transformerSummary = await GetTransformersSummary(
//                factoryIds,
//                null,
//                startTime,
//                endTime,
//                isCurrentShift
//            );

//            var lineSummary = await GetLinesSummary(
//                factoryIds,
//                null
//            );

//            var zoneSummary = await GetZonesSummary(
//                factoryIds,
//                null
//            );

//            return new DashboardSummaryDto
//            {
//                Level = factoryId.HasValue && factoryId.Value > 0
//                    ? "Factory"
//                    : "Main",

//                TotalEnergy = transformerSummary.TotalEnergy,
//                AvgPowerFactor = transformerSummary.AvgPowerFactor,

//                Transformers = new CountStatusDto
//                {
//                    TotalCount = transformerSummary.TotalCount,
//                    OnlineCount = transformerSummary.OnlineCount,
//                    OfflineCount = transformerSummary.OfflineCount
//                },

//                Lines = lineSummary,

//                Zones = zoneSummary
//            };
//        }

//        public async Task<List<VoltageGraphDto>> GetVoltageGraph(
//            int? factoryId,
//            DateTime startTime,
//            DateTime endTime,
//            bool isCurrentShift)
//        {
//            var factoryIds = await GetFactoryIds(factoryId);

//            if (isCurrentShift)
//            {
//                return await _emsContext.VW_TransformerHourlyAnalysis
//                    .Where(x =>
//                        factoryIds.Contains(x.FactoryId) &&
//                        x.Status == "Online")
//                    .GroupBy(x => x.HourStartTime)
//                    .Select(g => new VoltageGraphDto
//                    {
//                        FactoryId = factoryId ?? 0,
//                        HourStartTime = g.Key,
//                        AvgVoltage = g.Average(x => x.Voltage)
//                    })
//                    .OrderBy(x => x.HourStartTime)
//                    .ToListAsync();
//            }

//            return await _emsContext.TransformerHourlyAnalysis
//                .Where(x =>
//                    factoryIds.Contains(x.FactoryId) &&
//                    x.Status == "Online" &&
//                    x.ShiftStartTime >= startTime &&
//                    x.ShiftStartTime <= endTime)
//                .GroupBy(x => x.HourStartTime)
//                .Select(g => new VoltageGraphDto
//                {
//                    FactoryId = factoryId ?? 0,
//                    HourStartTime = g.Key,
//                    AvgVoltage = g.Average(x => x.Voltage)
//                })
//                .OrderBy(x => x.HourStartTime)
//                .ToListAsync();
//        }

//        public async Task<List<CurrentGraphDto>> GetCurrentGraph(
//            int? factoryId,
//            DateTime startTime,
//            DateTime endTime,
//            bool isCurrentShift)
//        {
//            var factoryIds = await GetFactoryIds(factoryId);

//            if (isCurrentShift)
//            {
//                return await _emsContext.VW_TransformerHourlyAnalysis
//                    .Where(x =>
//                        factoryIds.Contains(x.FactoryId) &&
//                        x.Status == "Online")
//                    .GroupBy(x => x.HourStartTime)
//                    .Select(g => new CurrentGraphDto
//                    {
//                        FactoryId = factoryId ?? 0,
//                        HourStartTime = g.Key,
//                        AvgCurrent = g.Average(x => x.Current)
//                    })
//                    .OrderBy(x => x.HourStartTime)
//                    .ToListAsync();
//            }

//            return await _emsContext.TransformerHourlyAnalysis
//                .Where(x =>
//                    factoryIds.Contains(x.FactoryId) &&
//                    x.Status == "Online" &&
//                    x.ShiftStartTime >= startTime &&
//                    x.ShiftStartTime <= endTime)
//                .GroupBy(x => x.HourStartTime)
//                .Select(g => new CurrentGraphDto
//                {
//                    FactoryId = factoryId ?? 0,
//                    HourStartTime = g.Key,
//                    AvgCurrent = g.Average(x => x.Current)
//                })
//                .OrderBy(x => x.HourStartTime)
//                .ToListAsync();
//        }

//        public async Task<List<HarmonicsGraphDto>> GetHarmonicsGraph(
//            int? factoryId,
//            DateTime startTime,
//            DateTime endTime,
//            bool isCurrentShift)
//        {
//            var factoryIds = await GetFactoryIds(factoryId);

//            if (isCurrentShift)
//            {
//                return await _emsContext.VW_TransformerHourlyAnalysis
//                    .Where(x => factoryIds.Contains(x.FactoryId))
//                    .GroupBy(x => x.HourStartTime)
//                    .Select(g => new HarmonicsGraphDto
//                    {
//                        FactoryId = factoryId ?? 0,
//                        HourStartTime = g.Key,
//                        AvgHarmonics = g.Average(x => x.Hermonics)
//                    })
//                    .OrderBy(x => x.HourStartTime)
//                    .ToListAsync();
//            }

//            return await _emsContext.TransformerHourlyAnalysis
//                .Where(x =>
//                    factoryIds.Contains(x.FactoryId) &&
//                    x.ShiftStartTime >= startTime &&
//                    x.ShiftStartTime <= endTime)
//                .GroupBy(x => x.HourStartTime)
//                .Select(g => new HarmonicsGraphDto
//                {
//                    FactoryId = factoryId ?? 0,
//                    HourStartTime = g.Key,
//                    AvgHarmonics = g.Average(x => x.Hermonics)
//                })
//                .OrderBy(x => x.HourStartTime)
//                .ToListAsync();
//        }



//        // HeatMap
//        public async Task<List<EnergyHeatmapDto>> GetEnergyHeatmap(
//            int factoryId,
//            int? transformerId,
//            int? zoneId,
//            DateTime startTime,
//            DateTime endTime,
//            bool isCurrentShift)
//        {
//            bool groupByHour = ShouldGroupByHour(startTime, endTime, isCurrentShift);

//            if (!transformerId.HasValue && !zoneId.HasValue)
//            {
//                return await GetTransformerHeatmap(
//                    factoryId,
//                    startTime,
//                    endTime,
//                    isCurrentShift,
//                    groupByHour);
//            }

//            if (transformerId.HasValue && !zoneId.HasValue)
//            {
//                return await GetZoneHeatmap(
//                    factoryId,
//                    transformerId.Value,
//                    startTime,
//                    endTime,
//                    isCurrentShift,
//                    groupByHour);
//            }

//            if (transformerId.HasValue && zoneId.HasValue)
//            {
//                return await GetLineHeatmap(
//                    factoryId,
//                    transformerId.Value,
//                    zoneId.Value,
//                    startTime,
//                    endTime,
//                    isCurrentShift,
//                    groupByHour);
//            }

//            throw new Exception("Invalid filters.");
//        }

//        public async Task<List<TransformerEnergyDto>> GetTotalEnergyPerTransformer(
//    int factoryId,
//    int? transformerId,
//    int? zoneId,
//    DateTime startTime,
//    DateTime endTime,
//    bool isCurrentShift)
//        {
//            // Factory Level: return Transformers
//            if (!transformerId.HasValue && !zoneId.HasValue)
//            {
//                if (isCurrentShift)
//                {
//                    return await _emsContext.VW_TransformerAnalysis
//                        .Where(x => x.FactoryId == factoryId)
//                        .GroupBy(x => new { x.TransformerId, x.TransformerName })
//                        .Select(g => new TransformerEnergyDto
//                        {
//                            TransformerId = g.Key.TransformerId,
//                            TransformerName = g.Key.TransformerName,
//                            TotalEnergyConsumption = g.Sum(x => x.TotalEnergyConsumption)
//                        })
//                        .ToListAsync();
//                }
//                else
//                {
//                    return await _emsContext.TransformerAnalysis
//                        .Where(x =>
//                            x.FactoryId == factoryId &&
//                            x.ShiftStartTime >= startTime &&
//                            x.ShiftStartTime <= endTime)
//                        .GroupBy(x => new { x.TransformerId, x.TransformerName })
//                        .Select(g => new TransformerEnergyDto
//                        {
//                            TransformerId = g.Key.TransformerId,
//                            TransformerName = g.Key.TransformerName,
//                            TotalEnergyConsumption = g.Sum(x => x.TotalEnergyConsumption)
//                        })
//                        .ToListAsync();
//                }
//            }

//            // Transformer Level: return Zones
//            if (transformerId.HasValue && !zoneId.HasValue)
//            {
//                var zonesRaw = await _emsContext.Zones
//                    .Where(x =>
//                        x.FactoryId == factoryId &&
//                        x.TransformerId == transformerId.Value)
//                    .Select(x => new
//                    {
//                        x.Id,
//                        x.Name,
//                        x.RatioFromParent
//                    })
//                    .ToListAsync();

//                var zones = zonesRaw.Select(x => new
//                {
//                    x.Id,
//                    x.Name,
//                    Ratio = NormalizeRatio(x.RatioFromParent ?? 0)
//                }).ToList();

//                decimal transformerTotalEnergy;

//                if (isCurrentShift)
//                {
//                    transformerTotalEnergy = await _emsContext.VW_TransformerAnalysis
//                        .Where(x =>
//                            x.FactoryId == factoryId &&
//                            x.TransformerId == transformerId.Value)
//                        .SumAsync(x => x.TotalEnergyConsumption);
//                }
//                else
//                {
//                    transformerTotalEnergy = await _emsContext.TransformerAnalysis
//                        .Where(x =>
//                            x.FactoryId == factoryId &&
//                            x.TransformerId == transformerId.Value &&
//                            x.ShiftStartTime >= startTime &&
//                            x.ShiftStartTime <= endTime)
//                        .SumAsync(x => x.TotalEnergyConsumption);
//                }

//                return zones
//                    .Select(z => new TransformerEnergyDto
//                    {
//                        TransformerId = z.Id,
//                        TransformerName = z.Name,
//                        TotalEnergyConsumption = transformerTotalEnergy * z.Ratio
//                    })
//                    .ToList();
//            }

//            // Zone Level: return Lines
//            if (transformerId.HasValue && zoneId.HasValue)
//            {
//                var zoneRaw = await _emsContext.Zones
//                    .Where(x =>
//                        x.Id == zoneId.Value &&
//                        x.FactoryId == factoryId &&
//                        x.TransformerId == transformerId.Value)
//                    .Select(x => new
//                    {
//                        x.RatioFromParent
//                    })
//                    .FirstOrDefaultAsync();

//                if (zoneRaw == null)
//                    throw new Exception("Zone does not belong to this transformer or factory.");

//                decimal zoneRatio = NormalizeRatio(zoneRaw.RatioFromParent ?? 0);

//                var linesRaw = await _emsContext.Lines
//                    .Where(x =>
//                        x.FactoryId == factoryId &&
//                        x.ZoneId == zoneId.Value)
//                    .Select(x => new
//                    {
//                        x.Id,
//                        x.Name,
//                        x.RatioFromParent
//                    })
//                    .ToListAsync();

//                var lines = linesRaw.Select(x => new
//                {
//                    x.Id,
//                    x.Name,
//                    Ratio = NormalizeRatio(x.RatioFromParent ?? 0)
//                }).ToList();

//                decimal transformerTotalEnergy;

//                if (isCurrentShift)
//                {
//                    transformerTotalEnergy = await _emsContext.VW_TransformerAnalysis
//                        .Where(x =>
//                            x.FactoryId == factoryId &&
//                            x.TransformerId == transformerId.Value)
//                        .SumAsync(x => x.TotalEnergyConsumption);
//                }
//                else
//                {
//                    transformerTotalEnergy = await _emsContext.TransformerAnalysis
//                        .Where(x =>
//                            x.FactoryId == factoryId &&
//                            x.TransformerId == transformerId.Value &&
//                            x.ShiftStartTime >= startTime &&
//                            x.ShiftStartTime <= endTime)
//                        .SumAsync(x => x.TotalEnergyConsumption);
//                }

//                decimal zoneTotalEnergy = transformerTotalEnergy * zoneRatio;

//                return lines
//                    .Select(l => new TransformerEnergyDto
//                    {
//                        TransformerId = l.Id,
//                        TransformerName = l.Name,
//                        TotalEnergyConsumption = zoneTotalEnergy * l.Ratio
//                    })
//                    .ToList();
//            }

//            throw new Exception("Invalid filters.");
//        }

//        public async Task<List<TransformerHourlyEnergyDto>> GetHourlyEnergyPerTransformer(
//    int factoryId,
//    int? transformerId,
//    int? zoneId,
//    DateTime startTime,
//    DateTime endTime,
//    bool isCurrentShift)
//        {
//            // Factory Level: return hourly energy per Transformer
//            if (!transformerId.HasValue && !zoneId.HasValue)
//            {
//                if (isCurrentShift)
//                {
//                    return await _emsContext.VW_TransformerHourlyAnalysis
//                        .Where(x => x.FactoryId == factoryId)
//                        .GroupBy(x => new
//                        {
//                            x.FactoryId,
//                            x.TransformerId,
//                            x.TransformerName,
//                            x.HourStartTime
//                        })
//                        .Select(g => new TransformerHourlyEnergyDto
//                        {
//                            FactoryId = g.Key.FactoryId,
//                            TransformerId = g.Key.TransformerId,
//                            TransformerName = g.Key.TransformerName,
//                            HourStartTime = g.Key.HourStartTime,
//                            TotalEnergyConsumption = g.Sum(x => x.TotalEnergyConsumption)
//                        })
//                        .OrderBy(x => x.HourStartTime)
//                        .ToListAsync();
//                }
//                else
//                {
//                    return await _emsContext.TransformerHourlyAnalysis
//                        .Where(x =>
//                            x.FactoryId == factoryId &&
//                            x.ShiftStartTime >= startTime &&
//                            x.ShiftStartTime <= endTime)
//                        .GroupBy(x => new
//                        {
//                            x.FactoryId,
//                            x.TransformerId,
//                            x.TransformerName,
//                            x.HourStartTime
//                        })
//                        .Select(g => new TransformerHourlyEnergyDto
//                        {
//                            FactoryId = g.Key.FactoryId,
//                            TransformerId = g.Key.TransformerId,
//                            TransformerName = g.Key.TransformerName,
//                            HourStartTime = g.Key.HourStartTime,
//                            TotalEnergyConsumption = g.Sum(x => x.TotalEnergyConsumption)
//                        })
//                        .OrderBy(x => x.HourStartTime)
//                        .ToListAsync();
//                }
//            }

//            // Transformer Level: return hourly energy per Zone
//            if (transformerId.HasValue && !zoneId.HasValue)
//            {
//                var zonesRaw = await _emsContext.Zones
//                    .Where(x =>
//                        x.FactoryId == factoryId &&
//                        x.TransformerId == transformerId.Value)
//                    .Select(x => new
//                    {
//                        x.Id,
//                        x.Name,
//                        x.RatioFromParent
//                    })
//                    .ToListAsync();

//                var zones = zonesRaw.Select(x => new
//                {
//                    x.Id,
//                    x.Name,
//                    Ratio = NormalizeRatio(x.RatioFromParent ?? 0)
//                }).ToList();

//                List<TransformerHourlyEnergyDto> transformerHourlyData;

//                if (isCurrentShift)
//                {
//                    transformerHourlyData = await _emsContext.VW_TransformerHourlyAnalysis
//                        .Where(x =>
//                            x.FactoryId == factoryId &&
//                            x.TransformerId == transformerId.Value)
//                        .GroupBy(x => new
//                        {
//                            x.FactoryId,
//                            x.TransformerId,
//                            x.TransformerName,
//                            x.HourStartTime
//                        })
//                        .Select(g => new TransformerHourlyEnergyDto
//                        {
//                            FactoryId = g.Key.FactoryId,
//                            TransformerId = g.Key.TransformerId,
//                            TransformerName = g.Key.TransformerName,
//                            HourStartTime = g.Key.HourStartTime,
//                            TotalEnergyConsumption = g.Sum(x => x.TotalEnergyConsumption)
//                        })
//                        .OrderBy(x => x.HourStartTime)
//                        .ToListAsync();
//                }
//                else
//                {
//                    transformerHourlyData = await _emsContext.TransformerHourlyAnalysis
//                        .Where(x =>
//                            x.FactoryId == factoryId &&
//                            x.TransformerId == transformerId.Value &&
//                            x.ShiftStartTime >= startTime &&
//                            x.ShiftStartTime <= endTime)
//                        .GroupBy(x => new
//                        {
//                            x.FactoryId,
//                            x.TransformerId,
//                            x.TransformerName,
//                            x.HourStartTime
//                        })
//                        .Select(g => new TransformerHourlyEnergyDto
//                        {
//                            FactoryId = g.Key.FactoryId,
//                            TransformerId = g.Key.TransformerId,
//                            TransformerName = g.Key.TransformerName,
//                            HourStartTime = g.Key.HourStartTime,
//                            TotalEnergyConsumption = g.Sum(x => x.TotalEnergyConsumption)
//                        })
//                        .OrderBy(x => x.HourStartTime)
//                        .ToListAsync();
//                }

//                var result = new List<TransformerHourlyEnergyDto>();

//                foreach (var hour in transformerHourlyData)
//                {
//                    foreach (var zone in zones)
//                    {
//                        result.Add(new TransformerHourlyEnergyDto
//                        {
//                            FactoryId = factoryId,
//                            TransformerId = zone.Id,
//                            TransformerName = zone.Name,
//                            HourStartTime = hour.HourStartTime,
//                            TotalEnergyConsumption = hour.TotalEnergyConsumption * zone.Ratio
//                        });
//                    }
//                }

//                return result
//                    .OrderBy(x => x.HourStartTime)
//                    .ToList();
//            }

//            // Zone Level: return hourly energy per Line
//            if (transformerId.HasValue && zoneId.HasValue)
//            {
//                var zoneRaw = await _emsContext.Zones
//                    .Where(x =>
//                        x.Id == zoneId.Value &&
//                        x.FactoryId == factoryId &&
//                        x.TransformerId == transformerId.Value)
//                    .Select(x => new
//                    {
//                        x.RatioFromParent
//                    })
//                    .FirstOrDefaultAsync();

//                if (zoneRaw == null)
//                    throw new Exception("Zone does not belong to this transformer or factory.");

//                decimal zoneRatio = NormalizeRatio(zoneRaw.RatioFromParent ?? 0);

//                var linesRaw = await _emsContext.Lines
//                    .Where(x =>
//                        x.FactoryId == factoryId &&
//                        x.ZoneId == zoneId.Value &&
//                        x.LineTransformers.Any(lt => lt.TransformerId == transformerId.Value))
//                    .Select(x => new
//                    {
//                        x.Id,
//                        x.Name,
//                        x.RatioFromParent
//                    })
//                    .ToListAsync();

//                var lines = linesRaw.Select(x => new
//                {
//                    x.Id,
//                    x.Name,
//                    Ratio = NormalizeRatio(x.RatioFromParent ?? 0)
//                }).ToList();

//                List<TransformerHourlyEnergyDto> transformerHourlyData;

//                if (isCurrentShift)
//                {
//                    transformerHourlyData = await _emsContext.VW_TransformerHourlyAnalysis
//                        .Where(x =>
//                            x.FactoryId == factoryId &&
//                            x.TransformerId == transformerId.Value)
//                        .GroupBy(x => new
//                        {
//                            x.FactoryId,
//                            x.TransformerId,
//                            x.TransformerName,
//                            x.HourStartTime
//                        })
//                        .Select(g => new TransformerHourlyEnergyDto
//                        {
//                            FactoryId = g.Key.FactoryId,
//                            TransformerId = g.Key.TransformerId,
//                            TransformerName = g.Key.TransformerName,
//                            HourStartTime = g.Key.HourStartTime,
//                            TotalEnergyConsumption = g.Sum(x => x.TotalEnergyConsumption)
//                        })
//                        .OrderBy(x => x.HourStartTime)
//                        .ToListAsync();
//                }
//                else
//                {
//                    transformerHourlyData = await _emsContext.TransformerHourlyAnalysis
//                        .Where(x =>
//                            x.FactoryId == factoryId &&
//                            x.TransformerId == transformerId.Value &&
//                            x.ShiftStartTime >= startTime &&
//                            x.ShiftStartTime <= endTime)
//                        .GroupBy(x => new
//                        {
//                            x.FactoryId,
//                            x.TransformerId,
//                            x.TransformerName,
//                            x.HourStartTime
//                        })
//                        .Select(g => new TransformerHourlyEnergyDto
//                        {
//                            FactoryId = g.Key.FactoryId,
//                            TransformerId = g.Key.TransformerId,
//                            TransformerName = g.Key.TransformerName,
//                            HourStartTime = g.Key.HourStartTime,
//                            TotalEnergyConsumption = g.Sum(x => x.TotalEnergyConsumption)
//                        })
//                        .OrderBy(x => x.HourStartTime)
//                        .ToListAsync();
//                }

//                var result = new List<TransformerHourlyEnergyDto>();

//                foreach (var hour in transformerHourlyData)
//                {
//                    decimal zoneHourEnergy = hour.TotalEnergyConsumption * zoneRatio;

//                    foreach (var line in lines)
//                    {
//                        result.Add(new TransformerHourlyEnergyDto
//                        {
//                            FactoryId = factoryId,
//                            TransformerId = line.Id,
//                            TransformerName = line.Name,
//                            HourStartTime = hour.HourStartTime,
//                            TotalEnergyConsumption = zoneHourEnergy * line.Ratio
//                        });
//                    }
//                }

//                return result
//                    .OrderBy(x => x.HourStartTime)
//                    .ToList();
//            }

//            throw new Exception("Invalid filters.");
//        }
//        public async Task<List<TopEnergyConsumerDto>> GetTopEnergyConsumers(
//    int factoryId,
//    int? transformerId,
//    int? zoneId,
//    DateTime startTime,
//    DateTime endTime,
//    bool isCurrentShift,
//    int top)
//        {
//            var data = await GetTotalEnergyPerTransformer(
//                factoryId,
//                transformerId,
//                zoneId,
//                startTime,
//                endTime,
//                isCurrentShift
//            );

//            decimal totalEnergy = data.Sum(x => x.TotalEnergyConsumption);

//            return data
//                .OrderByDescending(x => x.TotalEnergyConsumption)
//                .Take(top)
//                .Select((x, index) => new TopEnergyConsumerDto
//                {
//                    Rank = index + 1,
//                    TransformerId = x.TransformerId,
//                    TransformerName = x.TransformerName,
//                    TotalEnergyConsumption = x.TotalEnergyConsumption,
//                    PercentageOfTotal = totalEnergy > 0
//                        ? Math.Round(x.TotalEnergyConsumption / totalEnergy * 100, 0)
//                        : 0
//                })
//                .ToList();
//        }

//        private static decimal NormalizeRatio(decimal ratio)
//        {
//            return ratio > 1 ? ratio / 100 : ratio;
//        }



//        #region Helper Methods for GetSummary endpoint
//        private Task<List<int>> GetFactoryIds(int? factoryId)
//        {
//            if (factoryId.HasValue && factoryId.Value > 0)
//                return Task.FromResult(new List<int> { factoryId.Value });

//            return Task.FromResult(new List<int> { 2, 3 });
//        }

//        private async Task<TransformerSummaryResult> GetTransformersSummary(
//            List<int> factoryIds,
//            int? transformerId,
//            DateTime startTime,
//            DateTime endTime,
//            bool isCurrentShift)
//        {
//            int totalTransformers = await _emsContext.Transformers
//                .Where(x => x.FactoryId.HasValue && factoryIds.Contains(x.FactoryId.Value))
//                .CountAsync();

//            if (isCurrentShift)
//            {
//                var data = await _emsContext.VW_TransformerAnalysis
//                    .Where(x => factoryIds.Contains(x.FactoryId))
//                    .ToListAsync();

//                int onlineCount = data.Count(x => x.Status == "Online");

//                return new TransformerSummaryResult
//                {
//                    TotalEnergy = data.Sum(x => x.TotalEnergyConsumption),

//                    TotalCount = totalTransformers,
//                    OnlineCount = onlineCount,
//                    OfflineCount = totalTransformers - onlineCount,

//                    AvgPowerFactor = data.Any(x => x.Status == "Online")
//                        ? data.Where(x => x.Status == "Online").Average(x => x.PowerFactor)
//                        : 0
//                };
//            }

//            var historicalData = await _emsContext.TransformerAnalysis
//                .Where(x => factoryIds.Contains(x.FactoryId))
//                .Where(x => x.ShiftStartTime >= startTime && x.ShiftStartTime <= endTime)
//                .ToListAsync();

//            var latestPerTransformer = historicalData
//                .GroupBy(x => x.TransformerId)
//                .Select(g => g.OrderByDescending(x => x.ShiftStartTime).First())
//                .ToList();

//            int historicalOnlineCount = latestPerTransformer.Count(x => x.Status == "Online");

//            return new TransformerSummaryResult
//            {
//                TotalEnergy = historicalData.Sum(x => x.TotalEnergyConsumption),

//                TotalCount = totalTransformers,
//                OnlineCount = historicalOnlineCount,
//                OfflineCount = totalTransformers - historicalOnlineCount,

//                AvgPowerFactor = latestPerTransformer.Any(x => x.Status == "Online")
//                    ? latestPerTransformer.Where(x => x.Status == "Online").Average(x => x.PowerFactor)
//                    : 0
//            };
//        }



//        private class TransformerSummaryResult
//        {
//            public decimal TotalEnergy { get; set; }

//            public int TotalCount { get; set; }
//            public int OnlineCount { get; set; }
//            public int OfflineCount { get; set; }

//            public decimal AvgPowerFactor { get; set; }
//        }


//        private async Task<CountStatusDto> GetLinesSummary(
//            List<int> factoryIds,
//            int? zoneId)
//        {
//            DateTime onlineThreshold = DateTime.Now.AddHours(-3);

//            var lines = await _emsContext.Lines
//                .Where(x => x.FactoryId.HasValue && factoryIds.Contains(x.FactoryId.Value))
//                .Select(x => new
//                {
//                    x.Id,
//                    x.FactoryId,
//                    x.ZoneId
//                })
//                .ToListAsync();

//            var lineIds = lines.Select(x => x.Id).ToList();

//            var onlineLineIds = await _emsContext.Signals
//                .Where(x => x.LineId.HasValue)
//                .Where(x => lineIds.Contains(x.LineId.Value))
//                .Where(x => x.TimeStamp >= onlineThreshold)
//                .Select(x => x.LineId.Value)
//                .Distinct()
//                .ToListAsync();

//            int onlineCount = lines.Count(x => onlineLineIds.Contains(x.Id));

//            return new CountStatusDto
//            {
//                TotalCount = lines.Count,
//                OnlineCount = onlineCount,
//                OfflineCount = lines.Count - onlineCount
//            };
//        }
//        private async Task<CountStatusDto> GetZonesSummary(
//            List<int> factoryIds,
//            int? zoneId)
//        {
//            DateTime onlineThreshold = DateTime.Now.AddHours(-3);

//            var zones = await _emsContext.Zones
//                .Where(x => factoryIds.Contains(x.FactoryId))
//                .Select(x => new
//                {
//                    x.Id,
//                    x.FactoryId
//                })
//                .ToListAsync();

//            var zoneIds = zones.Select(x => x.Id).ToList();

//            var lines = await _emsContext.Lines
//                .Where(x => x.FactoryId.HasValue && factoryIds.Contains(x.FactoryId.Value))
//                .Where(x => x.ZoneId.HasValue)
//                .Where(x => zoneIds.Contains(x.ZoneId.Value))
//                .Select(x => new
//                {
//                    x.Id,
//                    ZoneId = x.ZoneId.Value
//                })
//                .ToListAsync();

//            var lineIds = lines.Select(x => x.Id).ToList();

//            var onlineLineIds = await _emsContext.Signals
//                .Where(x => x.LineId.HasValue)
//                .Where(x => lineIds.Contains(x.LineId.Value))
//                .Where(x => x.TimeStamp >= onlineThreshold)
//                .Select(x => x.LineId.Value)
//                .Distinct()
//                .ToListAsync();

//            var onlineZoneIds = lines
//                .Where(line => onlineLineIds.Contains(line.Id))
//                .Select(line => line.ZoneId)
//                .Distinct()
//                .ToList();

//            int onlineZones = zones.Count(x => onlineZoneIds.Contains(x.Id));

//            return new CountStatusDto
//            {
//                TotalCount = zones.Count,
//                OnlineCount = onlineZones,
//                OfflineCount = zones.Count - onlineZones
//            };
//        }
//        #endregion


//        #region Helper method for Heatmap status
//        private async Task<List<EnergyHeatmapDto>> GetTransformerHeatmap(
//            int factoryId,
//            DateTime startTime,
//            DateTime endTime,
//            bool isCurrentShift,
//            bool groupByHour)
//        {
//            var threshold = await GetThreshold("Transformer");

//            if (groupByHour)
//            {
//                var hourlyData = isCurrentShift
//                    ? await _emsContext.VW_TransformerHourlyAnalysis
//                        .Where(x => x.FactoryId == factoryId)
//                        .GroupBy(x => new
//                        {
//                            Period = x.HourStartTime,
//                            x.TransformerId,
//                            x.TransformerName
//                        })
//                        .Select(g => new
//                        {
//                            Date = g.Key.Period,
//                            TransformerId = g.Key.TransformerId,
//                            TransformerName = g.Key.TransformerName,
//                            Energy = g.Sum(x => x.TotalEnergyConsumption)
//                        })
//                        .ToListAsync()
//                    : await _emsContext.TransformerHourlyAnalysis
//                        .Where(x =>
//                            x.FactoryId == factoryId &&
//                            x.HourStartTime >= startTime &&
//                            x.HourStartTime <= endTime)
//                        .GroupBy(x => new
//                        {
//                            Period = x.HourStartTime,
//                            x.TransformerId,
//                            x.TransformerName
//                        })
//                        .Select(g => new
//                        {
//                            Date = g.Key.Period,
//                            TransformerId = g.Key.TransformerId,
//                            TransformerName = g.Key.TransformerName,
//                            Energy = g.Sum(x => x.TotalEnergyConsumption)
//                        })
//                        .ToListAsync();

//                return hourlyData
//                    .Select(x => new EnergyHeatmapDto
//                    {
//                        Date = x.Date,
//                        PeriodType = "Hour",

//                        Level = "Transformer",
//                        TransformerId = x.TransformerId,
//                        TransformerName = x.TransformerName,

//                        Energy = x.Energy,
//                        Status = GetEnergyStatus(x.Energy, threshold)
//                    })
//                    .OrderBy(x => x.Date)
//                    .ThenBy(x => x.TransformerName)
//                    .ToList();
//            }

//            var dailyData = await _emsContext.TransformerAnalysis
//                .Where(x =>
//                    x.FactoryId == factoryId &&
//                    x.ShiftStartTime >= startTime &&
//                    x.ShiftStartTime <= endTime)
//                .GroupBy(x => new
//                {
//                    Date = x.ShiftStartTime.Date,
//                    x.TransformerId,
//                    x.TransformerName
//                })
//                .Select(g => new
//                {
//                    g.Key.Date,
//                    TransformerId = g.Key.TransformerId,
//                    TransformerName = g.Key.TransformerName,
//                    Energy = g.Sum(x => x.TotalEnergyConsumption)
//                })
//                .ToListAsync();

//            return dailyData
//                .Select(x => new EnergyHeatmapDto
//                {
//                    Date = x.Date,
//                    PeriodType = "Day",

//                    Level = "Transformer",
//                    TransformerId = x.TransformerId,
//                    TransformerName = x.TransformerName,

//                    Energy = x.Energy,
//                    Status = GetEnergyStatus(x.Energy, threshold)
//                })
//                .OrderBy(x => x.Date)
//                .ThenBy(x => x.TransformerName)
//                .ToList();
//        }


//        private async Task<List<EnergyHeatmapDto>> GetZoneHeatmap(
//            int factoryId,
//            int transformerId,
//            DateTime startTime,
//            DateTime endTime,
//            bool isCurrentShift,
//            bool groupByHour)
//        {
//            var threshold = await GetThreshold("Zone");

//            var zonesRaw = await _emsContext.Zones
//                .Where(x =>
//                    x.FactoryId == factoryId &&
//                    x.TransformerId == transformerId)
//                .Select(x => new
//                {
//                    x.Id,
//                    x.Name,
//                    x.RatioFromParent
//                })
//                .ToListAsync();

//            var zones = zonesRaw
//                .Select(x => new
//                {
//                    x.Id,
//                    x.Name,
//                    Ratio = NormalizeRatio(x.RatioFromParent ?? 0)
//                })
//                .ToList();

//            var transformerEnergy = groupByHour
//                ? isCurrentShift
//                    ? await _emsContext.VW_TransformerHourlyAnalysis
//                        .Where(x =>
//                            x.FactoryId == factoryId &&
//                            x.TransformerId == transformerId)
//                        .GroupBy(x => x.HourStartTime)
//                        .Select(g => new
//                        {
//                            Date = g.Key,
//                            Energy = g.Sum(x => x.TotalEnergyConsumption)
//                        })
//                        .ToListAsync()
//                    : await _emsContext.TransformerHourlyAnalysis
//                        .Where(x =>
//                            x.FactoryId == factoryId &&
//                            x.TransformerId == transformerId &&
//                            x.HourStartTime >= startTime &&
//                            x.HourStartTime <= endTime)
//                        .GroupBy(x => x.HourStartTime)
//                        .Select(g => new
//                        {
//                            Date = g.Key,
//                            Energy = g.Sum(x => x.TotalEnergyConsumption)
//                        })
//                        .ToListAsync()
//                : await _emsContext.TransformerAnalysis
//                    .Where(x =>
//                        x.FactoryId == factoryId &&
//                        x.TransformerId == transformerId &&
//                        x.ShiftStartTime >= startTime &&
//                        x.ShiftStartTime <= endTime)
//                    .GroupBy(x => x.ShiftStartTime.Date)
//                    .Select(g => new
//                    {
//                        Date = g.Key,
//                        Energy = g.Sum(x => x.TotalEnergyConsumption)
//                    })
//                    .ToListAsync();

//            var result = new List<EnergyHeatmapDto>();

//            foreach (var period in transformerEnergy)
//            {
//                foreach (var zone in zones)
//                {
//                    decimal zoneEnergy = period.Energy * zone.Ratio;

//                    result.Add(new EnergyHeatmapDto
//                    {
//                        Date = period.Date,
//                        PeriodType = groupByHour ? "Hour" : "Day",

//                        Level = "Zone",

//                        TransformerId = transformerId,
//                        ZoneId = zone.Id,
//                        ZoneName = zone.Name,

//                        Energy = zoneEnergy,
//                        Status = GetEnergyStatus(zoneEnergy, threshold)
//                    });
//                }
//            }

//            return result
//                .OrderBy(x => x.Date)
//                .ThenBy(x => x.ZoneName)
//                .ToList();
//        }


//        private async Task<List<EnergyHeatmapDto>> GetLineHeatmap(
//            int factoryId,
//            int transformerId,
//            int zoneId,
//            DateTime startTime,
//            DateTime endTime,
//            bool isCurrentShift,
//            bool groupByHour)
//        {
//            var threshold = await GetThreshold("Line");

//            var zoneRaw = await _emsContext.Zones
//                .Where(x =>
//                    x.Id == zoneId &&
//                    x.FactoryId == factoryId &&
//                    x.TransformerId == transformerId)
//                .Select(x => new
//                {
//                    x.RatioFromParent
//                })
//                .FirstOrDefaultAsync();

//            if (zoneRaw == null)
//                throw new Exception("Zone does not belong to this transformer or factory.");

//            decimal zoneRatio = NormalizeRatio(zoneRaw.RatioFromParent ?? 0);

//            var linesRaw = await _emsContext.Lines
//                .Where(x =>
//                    x.FactoryId == factoryId &&
//                    x.ZoneId == zoneId)
//                .Select(x => new
//                {
//                    x.Id,
//                    x.Name,
//                    x.RatioFromParent
//                })
//                .ToListAsync();

//            var lines = linesRaw
//                .Select(x => new
//                {
//                    x.Id,
//                    x.Name,
//                    Ratio = NormalizeRatio(x.RatioFromParent ?? 0)
//                })
//                .ToList();

//            var transformerEnergy = groupByHour
//                ? isCurrentShift
//                    ? await _emsContext.VW_TransformerHourlyAnalysis
//                        .Where(x =>
//                            x.FactoryId == factoryId &&
//                            x.TransformerId == transformerId)
//                        .GroupBy(x => x.HourStartTime)
//                        .Select(g => new
//                        {
//                            Date = g.Key,
//                            Energy = g.Sum(x => x.TotalEnergyConsumption)
//                        })
//                        .ToListAsync()
//                    : await _emsContext.TransformerHourlyAnalysis
//                        .Where(x =>
//                            x.FactoryId == factoryId &&
//                            x.TransformerId == transformerId &&
//                            x.HourStartTime >= startTime &&
//                            x.HourStartTime <= endTime)
//                        .GroupBy(x => x.HourStartTime)
//                        .Select(g => new
//                        {
//                            Date = g.Key,
//                            Energy = g.Sum(x => x.TotalEnergyConsumption)
//                        })
//                        .ToListAsync()
//                : await _emsContext.TransformerAnalysis
//                    .Where(x =>
//                        x.FactoryId == factoryId &&
//                        x.TransformerId == transformerId &&
//                        x.ShiftStartTime >= startTime &&
//                        x.ShiftStartTime <= endTime)
//                    .GroupBy(x => x.ShiftStartTime.Date)
//                    .Select(g => new
//                    {
//                        Date = g.Key,
//                        Energy = g.Sum(x => x.TotalEnergyConsumption)
//                    })
//                    .ToListAsync();

//            var result = new List<EnergyHeatmapDto>();

//            foreach (var period in transformerEnergy)
//            {
//                decimal zoneEnergy = period.Energy * zoneRatio;

//                foreach (var line in lines)
//                {
//                    decimal lineEnergy = zoneEnergy * line.Ratio;

//                    result.Add(new EnergyHeatmapDto
//                    {
//                        Date = period.Date,
//                        PeriodType = groupByHour ? "Hour" : "Day",

//                        Level = "Line",

//                        TransformerId = transformerId,
//                        ZoneId = zoneId,

//                        LineId = line.Id,
//                        LineName = line.Name,

//                        Energy = lineEnergy,
//                        Status = GetEnergyStatus(lineEnergy, threshold)
//                    });
//                }
//            }

//            return result
//                .OrderBy(x => x.Date)
//                .ThenBy(x => x.LineName)
//                .ToList();
//        }

//        private async Task<EnergyHeatmapThreshold> GetThreshold(string level)
//        {
//            var threshold = await _emsContext.EnergyHeatmapThresholds
//                .Where(x => x.Level == level && x.IsActive)
//                .FirstOrDefaultAsync();

//            if (threshold == null)
//                throw new Exception($"Energy heatmap threshold for level '{level}' is not configured.");

//            return threshold;
//        }

//        private static EnergyStatus GetEnergyStatus(
//            decimal energy,
//            EnergyHeatmapThreshold threshold)
//        {
//            if (energy >= threshold.LowFrom && energy <= threshold.LowTo)
//                return EnergyStatus.Low;

//            if (energy >= threshold.MediumFrom && energy <= threshold.MediumTo)
//                return EnergyStatus.Medium;

//            if (energy >= threshold.HighFrom && energy <= threshold.HighTo)
//                return EnergyStatus.High;

//            return EnergyStatus.Unknown;
//        }


//        private static bool ShouldGroupByHour(
//    DateTime startTime,
//    DateTime endTime,
//    bool isCurrentShift)
//        {
//            if (isCurrentShift)
//                return true;

//            return (endTime - startTime).TotalHours <= 24;
//        }


//        #endregion

//    }
//}

using BLL.Dtos;
using DAL.Models.Threshold;
using DAL.Repositories.Interface;
using Shared.Dtos;
using Shared.Dtos.EnergyDashboard;
using Shared.Dtos.Heatmap;
using Shared.Enum;

namespace DAL.Repositories.Implementation
{
    public class EnergyDashboardRepository : IEnergyDashboardRepository
    {
        private readonly EmsContext _emsContext;

        public EnergyDashboardRepository(EmsContext emsContext)
        {
            _emsContext = emsContext;
        }

        #region Summary


        public async Task<TransformerSummaryDto> GetSummary(int? factoryId, DateTime startTime, DateTime endTime, bool isCurrentShift)
        {
            // TotalCount from Definitions (always fast)
            var totalTransformers = await _emsContext.Transformers
                .Where(x => !factoryId.HasValue || x.FactoryId == factoryId)
                .CountAsync();

            var totalZones = await _emsContext.Zones
                .Where(x => !factoryId.HasValue || x.FactoryId == factoryId)
                .CountAsync();

            var totalLines = await _emsContext.Lines
                .Where(x => !factoryId.HasValue || x.FactoryId == factoryId)
                .CountAsync();

            if (isCurrentShift)
            {
                var transformerData = await _emsContext.VW_TransformerAnalysis
                    .Where(x => !factoryId.HasValue || x.FactoryId == factoryId)
                    .ToListAsync();

                var onlineTransformers = transformerData.Where(x => x.Status == "Online").ToList();

                return new TransformerSummaryDto
                {
                    TotalEnergy = transformerData.Sum(x => x.TotalEnergyConsumption),
                    Transformers = new EntityCountDto { TotalCount = totalTransformers },
                    Zones = new EntityCountDto { TotalCount = totalZones },
                    Lines = new EntityCountDto { TotalCount = totalLines },
                    AvgPowerFactor = onlineTransformers.Any()
                        ? onlineTransformers.Average(x => x.PowerFactor)
                        : 0
                };
            }
            else
            {
                var transformerData = await _emsContext.TransformerAnalysis
                    .Where(x => x.ShiftStartTime >= startTime && x.ShiftStartTime <= endTime
                        && (!factoryId.HasValue || x.FactoryId == factoryId))
                    .ToListAsync();

                var onlineTransformers = transformerData.Where(x => x.Status == "Online").ToList();

                return new TransformerSummaryDto
                {
                    TotalEnergy = transformerData.Sum(x => x.TotalEnergyConsumption),
                    Transformers = new EntityCountDto { TotalCount = totalTransformers },
                    Zones = new EntityCountDto { TotalCount = totalZones },
                    Lines = new EntityCountDto { TotalCount = totalLines },
                    AvgPowerFactor = onlineTransformers.Any()
                        ? onlineTransformers.Average(x => x.PowerFactor)
                        : 0
                };
            }
        }

        public async Task<OnlineStatusDto> GetOnlineStatus(int? factoryId, DateTime startTime, DateTime endTime, bool isCurrentShift)
        {
            if (isCurrentShift)
            {
                var transformerData = await _emsContext.VW_TransformerAnalysis
                    .Where(x => !factoryId.HasValue || x.FactoryId == factoryId)
                    .Select(x => x.Status)
                    .ToListAsync();

                var zoneData = await _emsContext.VW_ZoneAnalysis
                    .Where(x => !factoryId.HasValue || x.FactoryId == factoryId)
                    .Select(x => x.Status)
                    .ToListAsync();

                var lineData = await _emsContext.VW_LineAnalysis
                    .Where(x => !factoryId.HasValue || x.FactoryId == factoryId)
                    .Select(x => x.Status)
                    .ToListAsync();

                return new OnlineStatusDto
                {
                    Transformers = new EntityStatusDto
                    {
                        OnlineCount = transformerData.Count(x => x == "Online"),
                        OfflineCount = transformerData.Count(x => x == "Offline")
                    },
                    Zones = new EntityStatusDto
                    {
                        OnlineCount = zoneData.Count(x => x == "Online"),
                        OfflineCount = zoneData.Count(x => x == "Offline")
                    },
                    Lines = new EntityStatusDto
                    {
                        OnlineCount = lineData.Count(x => x == "Online"),
                        OfflineCount = lineData.Count(x => x == "Offline")
                    }
                };
            }
            else
            {
                var transformerData = await _emsContext.TransformerAnalysis
                    .Where(x => x.ShiftStartTime >= startTime && x.ShiftStartTime <= endTime
                        && (!factoryId.HasValue || x.FactoryId == factoryId))
                    .Select(x => x.Status)
                    .ToListAsync();

                var zoneData = await _emsContext.ZoneAnalysis
                    .Where(x => x.ShiftStartTime >= startTime && x.ShiftStartTime <= endTime
                        && (!factoryId.HasValue || x.FactoryId == factoryId))
                    .Select(x => x.Status)
                    .ToListAsync();

                var lineData = await _emsContext.LineAnalysis
                    .Where(x => x.ShiftStartTime >= startTime && x.ShiftStartTime <= endTime
                        && (!factoryId.HasValue || x.FactoryId == factoryId))
                    .Select(x => x.Status)
                    .ToListAsync();

                return new OnlineStatusDto
                {
                    Transformers = new EntityStatusDto
                    {
                        OnlineCount = transformerData.Count(x => x == "Online"),
                        OfflineCount = transformerData.Count(x => x == "Offline")
                    },
                    Zones = new EntityStatusDto
                    {
                        OnlineCount = zoneData.Count(x => x == "Online"),
                        OfflineCount = zoneData.Count(x => x == "Offline")
                    },
                    Lines = new EntityStatusDto
                    {
                        OnlineCount = lineData.Count(x => x == "Online"),
                        OfflineCount = lineData.Count(x => x == "Offline")
                    }
                };
            }
        }
        #endregion

        #region Graphs

        public async Task<List<VoltageGraphDto>> GetVoltageGraph(
            int? factoryId,
            DateTime startTime,
            DateTime endTime,
            bool isCurrentShift)
        {
            var factoryIds = await GetFactoryIds(factoryId);

            if (isCurrentShift)
            {
                return await _emsContext.VW_TransformerHourlyAnalysis
                    .Where(x => factoryIds.Contains(x.FactoryId) && x.Status == "Online")
                    .GroupBy(x => x.HourStartTime)
                    .Select(g => new VoltageGraphDto
                    {
                        FactoryId = factoryId ?? 0,
                        HourStartTime = g.Key,
                        AvgVoltage = g.Average(x => x.Voltage)
                    })
                    .OrderBy(x => x.HourStartTime)
                    .ToListAsync();
            }

            return await _emsContext.TransformerHourlyAnalysis
                .Where(x =>
                    factoryIds.Contains(x.FactoryId) &&
                    x.Status == "Online" &&
                    x.ShiftStartTime >= startTime &&
                    x.ShiftStartTime <= endTime)
                .GroupBy(x => x.HourStartTime)
                .Select(g => new VoltageGraphDto
                {
                    FactoryId = factoryId ?? 0,
                    HourStartTime = g.Key,
                    AvgVoltage = g.Average(x => x.Voltage)
                })
                .OrderBy(x => x.HourStartTime)
                .ToListAsync();
        }

        public async Task<List<CurrentGraphDto>> GetCurrentGraph(
            int? factoryId,
            DateTime startTime,
            DateTime endTime,
            bool isCurrentShift)
        {
            var factoryIds = await GetFactoryIds(factoryId);

            if (isCurrentShift)
            {
                return await _emsContext.VW_TransformerHourlyAnalysis
                    .Where(x => factoryIds.Contains(x.FactoryId) && x.Status == "Online")
                    .GroupBy(x => x.HourStartTime)
                    .Select(g => new CurrentGraphDto
                    {
                        FactoryId = factoryId ?? 0,
                        HourStartTime = g.Key,
                        AvgCurrent = g.Average(x => x.Current)
                    })
                    .OrderBy(x => x.HourStartTime)
                    .ToListAsync();
            }

            return await _emsContext.TransformerHourlyAnalysis
                .Where(x =>
                    factoryIds.Contains(x.FactoryId) &&
                    x.Status == "Online" &&
                    x.ShiftStartTime >= startTime &&
                    x.ShiftStartTime <= endTime)
                .GroupBy(x => x.HourStartTime)
                .Select(g => new CurrentGraphDto
                {
                    FactoryId = factoryId ?? 0,
                    HourStartTime = g.Key,
                    AvgCurrent = g.Average(x => x.Current)
                })
                .OrderBy(x => x.HourStartTime)
                .ToListAsync();
        }

        public async Task<List<HarmonicsGraphDto>> GetHarmonicsGraph(
            int? factoryId,
            DateTime startTime,
            DateTime endTime,
            bool isCurrentShift)
        {
            var factoryIds = await GetFactoryIds(factoryId);

            if (isCurrentShift)
            {
                return await _emsContext.VW_TransformerHourlyAnalysis
                    .Where(x => factoryIds.Contains(x.FactoryId))
                    .GroupBy(x => x.HourStartTime)
                    .Select(g => new HarmonicsGraphDto
                    {
                        FactoryId = factoryId ?? 0,
                        HourStartTime = g.Key,
                        AvgHarmonics = g.Average(x => x.Hermonics)
                    })
                    .OrderBy(x => x.HourStartTime)
                    .ToListAsync();
            }

            return await _emsContext.TransformerHourlyAnalysis
                .Where(x =>
                    factoryIds.Contains(x.FactoryId) &&
                    x.ShiftStartTime >= startTime &&
                    x.ShiftStartTime <= endTime)
                .GroupBy(x => x.HourStartTime)
                .Select(g => new HarmonicsGraphDto
                {
                    FactoryId = factoryId ?? 0,
                    HourStartTime = g.Key,
                    AvgHarmonics = g.Average(x => x.Hermonics)
                })
                .OrderBy(x => x.HourStartTime)
                .ToListAsync();
        }

        #endregion

        #region Energy

        //public async Task<List<TransformerEnergyDto>> GetTotalEnergyPerTransformer(
        //    int factoryId,
        //    int? transformerId,
        //    int? zoneId,
        //    DateTime startTime,
        //    DateTime endTime,
        //    bool isCurrentShift)
        //{
        //    // Factory level: return transformers.
        //    if (!transformerId.HasValue && !zoneId.HasValue)
        //    {
        //        return await GetTransformerTotalEnergyList(
        //            factoryId,
        //            startTime,
        //            endTime,
        //            isCurrentShift);
        //    }

        //    // Transformer level: return zones.
        //    if (transformerId.HasValue && !zoneId.HasValue)
        //    {
        //        var zones = await GetZonesByTransformer(factoryId, transformerId.Value);

        //        decimal transformerEnergy = await GetTransformerTotalEnergy(
        //            factoryId,
        //            transformerId.Value,
        //            startTime,
        //            endTime,
        //            isCurrentShift);

        //        return zones.Select(z => new TransformerEnergyDto
        //        {
        //            TransformerId = z.Id,
        //            TransformerName = z.Name,
        //            TotalEnergyConsumption = transformerEnergy * z.Ratio
        //        }).ToList();
        //    }

        //    // Zone level: return lines.
        //    if (transformerId.HasValue && zoneId.HasValue)
        //    {
        //        decimal zoneRatio = await GetZoneRatio(
        //            factoryId,
        //            transformerId.Value,
        //            zoneId.Value);

        //        var lines = await GetLinesByZone(factoryId, zoneId.Value);

        //        decimal transformerEnergy = await GetTransformerTotalEnergy(
        //            factoryId,
        //            transformerId.Value,
        //            startTime,
        //            endTime,
        //            isCurrentShift);

        //        decimal zoneEnergy = transformerEnergy * zoneRatio;

        //        return lines.Select(l => new TransformerEnergyDto
        //        {
        //            TransformerId = l.Id,
        //            TransformerName = l.Name,
        //            TotalEnergyConsumption = zoneEnergy * l.Ratio
        //        }).ToList();
        //    }

        //    throw new Exception("Invalid filters.");
        //}

        //public async Task<List<TransformerHourlyEnergyDto>> GetHourlyEnergyPerTransformer(
        //    int factoryId,
        //    int? transformerId,
        //    int? zoneId,
        //    DateTime startTime,
        //    DateTime endTime,
        //    bool isCurrentShift)
        //{
        //    // Factory level: hourly energy per transformer.
        //    if (!transformerId.HasValue && !zoneId.HasValue)
        //    {
        //        return await GetTransformerHourlyEnergyList(
        //            factoryId,
        //            null,
        //            startTime,
        //            endTime,
        //            isCurrentShift);
        //    }

        //    // Transformer level: hourly energy per zone.
        //    if (transformerId.HasValue && !zoneId.HasValue)
        //    {
        //        var zones = await GetZonesByTransformer(factoryId, transformerId.Value);

        //        var transformerHourlyData = await GetTransformerHourlyEnergyList(
        //            factoryId,
        //            transformerId.Value,
        //            startTime,
        //            endTime,
        //            isCurrentShift);

        //        return transformerHourlyData
        //            .SelectMany(hour => zones.Select(zone => new TransformerHourlyEnergyDto
        //            {
        //                FactoryId = factoryId,
        //                TransformerId = zone.Id,
        //                TransformerName = zone.Name,
        //                HourStartTime = hour.HourStartTime,
        //                TotalEnergyConsumption = hour.TotalEnergyConsumption * zone.Ratio
        //            }))
        //            .OrderBy(x => x.HourStartTime)
        //            .ToList();
        //    }

        //    // Zone level: hourly energy per line.
        //    if (transformerId.HasValue && zoneId.HasValue)
        //    {
        //        decimal zoneRatio = await GetZoneRatio(
        //            factoryId,
        //            transformerId.Value,
        //            zoneId.Value);

        //        var lines = await GetLinesByZone(factoryId, zoneId.Value);

        //        var transformerHourlyData = await GetTransformerHourlyEnergyList(
        //            factoryId,
        //            transformerId.Value,
        //            startTime,
        //            endTime,
        //            isCurrentShift);

        //        return transformerHourlyData
        //            .SelectMany(hour =>
        //            {
        //                decimal zoneHourEnergy = hour.TotalEnergyConsumption * zoneRatio;

        //                return lines.Select(line => new TransformerHourlyEnergyDto
        //                {
        //                    FactoryId = factoryId,
        //                    TransformerId = line.Id,
        //                    TransformerName = line.Name,
        //                    HourStartTime = hour.HourStartTime,
        //                    TotalEnergyConsumption = zoneHourEnergy * line.Ratio
        //                });
        //            })
        //            .OrderBy(x => x.HourStartTime)
        //            .ToList();
        //    }

        //    throw new Exception("Invalid filters.");
        //}

        //public async Task<List<TopEnergyConsumerDto>> GetTopEnergyConsumers(
        //    int factoryId,
        //    int? transformerId,
        //    int? zoneId,
        //    DateTime startTime,
        //    DateTime endTime,
        //    bool isCurrentShift,
        //    int top)
        //{
        //    var data = await GetTotalEnergyPerTransformer(
        //        factoryId,
        //        transformerId,
        //        zoneId,
        //        startTime,
        //        endTime,
        //        isCurrentShift);

        //    decimal totalEnergy = data.Sum(x => x.TotalEnergyConsumption);

        //    return data
        //        .OrderByDescending(x => x.TotalEnergyConsumption)
        //        .Take(top)
        //        .Select((x, index) => new TopEnergyConsumerDto
        //        {
        //            Rank = index + 1,
        //            TransformerId = x.TransformerId,
        //            TransformerName = x.TransformerName,
        //            TotalEnergyConsumption = x.TotalEnergyConsumption,
        //            PercentageOfTotal = totalEnergy > 0
        //                ? Math.Round(x.TotalEnergyConsumption / totalEnergy * 100, 0)
        //                : 0
        //        })
        //        .ToList();
        //}



        //    public async Task<List<EnergyConsumptionDto>> GetTotalEnergyPerTransformer(
        //int factoryId,
        //int? transformerId,
        //int? zoneId,
        //DateTime startTime,
        //DateTime endTime,
        //bool isCurrentShift)
        //    {
        //        // Factory level: return transformers.
        //        if (!transformerId.HasValue && !zoneId.HasValue)
        //        {
        //            var transformers = await GetTransformerTotalEnergyList(
        //                factoryId,
        //                startTime,
        //                endTime,
        //                isCurrentShift);

        //            return transformers.Select(x => new EnergyConsumptionDto
        //            {
        //                Level = "Transformer",

        //                TransformerId = x.TransformerId,
        //                TransformerName = x.TransformerName,

        //                Energy = x.TotalEnergyConsumption
        //            }).ToList();
        //        }

        //        // Transformer level: return zones.
        //        if (transformerId.HasValue && !zoneId.HasValue)
        //        {
        //            var zones = await GetZonesByTransformer(factoryId, transformerId.Value);

        //            decimal transformerEnergy = await GetTransformerTotalEnergy(
        //                factoryId,
        //                transformerId.Value,
        //                startTime,
        //                endTime,
        //                isCurrentShift);

        //            return zones.Select(z => new EnergyConsumptionDto
        //            {
        //                Level = "Zone",

        //                TransformerId = transformerId.Value,

        //                ZoneId = z.Id,
        //                ZoneName = z.Name,

        //                Energy = transformerEnergy * z.Ratio
        //            }).ToList();
        //        }

        //        // Zone level: return lines.
        //        if (transformerId.HasValue && zoneId.HasValue)
        //        {
        //            decimal zoneRatio = await GetZoneRatio(
        //                factoryId,
        //                transformerId.Value,
        //                zoneId.Value);

        //            var lines = await GetLinesByZone(factoryId, zoneId.Value);

        //            decimal transformerEnergy = await GetTransformerTotalEnergy(
        //                factoryId,
        //                transformerId.Value,
        //                startTime,
        //                endTime,
        //                isCurrentShift);

        //            decimal zoneEnergy = transformerEnergy * zoneRatio;

        //            return lines.Select(l => new EnergyConsumptionDto
        //            {
        //                Level = "Line",

        //                TransformerId = transformerId.Value,
        //                ZoneId = zoneId.Value,

        //                LineId = l.Id,
        //                LineName = l.Name,

        //                Energy = zoneEnergy * l.Ratio
        //            }).ToList();
        //        }

        //        throw new ArgumentException("Invalid filters.");
        //    }


        //    public async Task<List<HourlyEnergyConsumptionDto>> GetHourlyEnergyPerTransformer(
        //int factoryId,
        //int? transformerId,
        //int? zoneId,
        //DateTime startTime,
        //DateTime endTime,
        //bool isCurrentShift)
        //    {
        //        // Factory level: hourly energy per transformer.
        //        if (!transformerId.HasValue && !zoneId.HasValue)
        //        {
        //            var transformerHourlyData = await GetTransformerHourlyEnergyList(
        //                factoryId,
        //                null,
        //                startTime,
        //                endTime,
        //                isCurrentShift);

        //            return transformerHourlyData.Select(x => new HourlyEnergyConsumptionDto
        //            {
        //                FactoryId = x.FactoryId,
        //                HourStartTime = x.HourStartTime,

        //                Level = "Transformer",

        //                TransformerId = x.TransformerId,
        //                TransformerName = x.TransformerName,

        //                Energy = x.TotalEnergyConsumption
        //            }).ToList();
        //        }

        //        // Transformer level: hourly energy per zone.
        //        if (transformerId.HasValue && !zoneId.HasValue)
        //        {
        //            var zones = await GetZonesByTransformer(factoryId, transformerId.Value);

        //            var transformerHourlyData = await GetTransformerHourlyEnergyList(
        //                factoryId,
        //                transformerId.Value,
        //                startTime,
        //                endTime,
        //                isCurrentShift);

        //            return transformerHourlyData
        //                .SelectMany(hour => zones.Select(zone => new HourlyEnergyConsumptionDto
        //                {
        //                    FactoryId = factoryId,
        //                    HourStartTime = hour.HourStartTime,

        //                    Level = "Zone",

        //                    TransformerId = transformerId.Value,

        //                    ZoneId = zone.Id,
        //                    ZoneName = zone.Name,

        //                    Energy = hour.TotalEnergyConsumption * zone.Ratio
        //                }))
        //                .OrderBy(x => x.HourStartTime)
        //                .ToList();
        //        }

        //        // Zone level: hourly energy per line.
        //        if (transformerId.HasValue && zoneId.HasValue)
        //        {
        //            decimal zoneRatio = await GetZoneRatio(
        //                factoryId,
        //                transformerId.Value,
        //                zoneId.Value);

        //            var lines = await GetLinesByZone(factoryId, zoneId.Value);

        //            var transformerHourlyData = await GetTransformerHourlyEnergyList(
        //                factoryId,
        //                transformerId.Value,
        //                startTime,
        //                endTime,
        //                isCurrentShift);

        //            return transformerHourlyData
        //                .SelectMany(hour =>
        //                {
        //                    decimal zoneHourEnergy = hour.TotalEnergyConsumption * zoneRatio;

        //                    return lines.Select(line => new HourlyEnergyConsumptionDto
        //                    {
        //                        FactoryId = factoryId,
        //                        HourStartTime = hour.HourStartTime,

        //                        Level = "Line",

        //                        TransformerId = transformerId.Value,
        //                        ZoneId = zoneId.Value,

        //                        LineId = line.Id,
        //                        LineName = line.Name,

        //                        Energy = zoneHourEnergy * line.Ratio
        //                    });
        //                })
        //                .OrderBy(x => x.HourStartTime)
        //                .ToList();
        //        }

        //        throw new ArgumentException("Invalid filters.");
        //    }


        public async Task<List<EnergyConsumptionDto>> GetTotalEnergyPerTransformer(
    int factoryId,
    int? transformerId,
    int? zoneId,
    DateTime startTime,
    DateTime endTime,
    bool isCurrentShift)
        {
            // Factory level: return transformers
            if (!transformerId.HasValue && !zoneId.HasValue)
            {
                if (isCurrentShift)
                {
                    var data = await _emsContext.VW_TransformerAnalysis
                        .Where(x => x.FactoryId == factoryId)
                        .ToListAsync();

                    return data.Select(x => new EnergyConsumptionDto
                    {
                        Level = "Transformer",
                        TransformerId = x.TransformerId,
                        TransformerName = x.TransformerName,
                        Energy = x.TotalEnergyConsumption
                    }).ToList();
                }
                else
                {
                    var data = await _emsContext.TransformerAnalysis
                        .Where(x =>
                            x.FactoryId == factoryId &&
                            x.ShiftStartTime >= startTime &&
                            x.ShiftStartTime <= endTime)
                        .GroupBy(x => new { x.TransformerId, x.TransformerName })
                        .Select(g => new EnergyConsumptionDto
                        {
                            Level = "Transformer",
                            TransformerId = g.Key.TransformerId,
                            TransformerName = g.Key.TransformerName,
                            Energy = g.Sum(x => x.TotalEnergyConsumption)
                        })
                        .ToListAsync();

                    return data;
                }
            }

            // Transformer level: return zones
            if (transformerId.HasValue && !zoneId.HasValue)
            {
                if (isCurrentShift)
                {
                    var data = await _emsContext.VW_ZoneAnalysis
                        .Where(x =>
                            x.FactoryId == factoryId &&
                            x.TransformerId == transformerId)
                        .ToListAsync();

                    return data.Select(x => new EnergyConsumptionDto
                    {
                        Level = "Zone",
                        TransformerId = transformerId.Value,
                        ZoneId = x.ZoneId,
                        ZoneName = x.ZoneName,
                        Energy = x.EnergyConsumption
                    }).ToList();
                }
                else
                {
                    var data = await _emsContext.ZoneAnalysis
                        .Where(x =>
                            x.FactoryId == factoryId &&
                            x.TransformerId == transformerId &&
                            x.ShiftStartTime >= startTime &&
                            x.ShiftStartTime <= endTime)
                        .GroupBy(x => new { x.ZoneId, x.ZoneName })
                        .Select(g => new EnergyConsumptionDto
                        {
                            Level = "Zone",
                            TransformerId = transformerId.Value,
                            ZoneId = g.Key.ZoneId,
                            ZoneName = g.Key.ZoneName,
                            Energy = g.Sum(x => x.EnergyConsumption)
                        })
                        .ToListAsync();

                    return data;
                }
            }

            // Zone level: return lines
            if (transformerId.HasValue && zoneId.HasValue)
            {
                if (isCurrentShift)
                {
                    var data = await _emsContext.VW_LineAnalysis
                        .Where(x =>
                            x.FactoryId == factoryId &&
                            x.ZoneId == zoneId)
                        .ToListAsync();

                    return data.Select(x => new EnergyConsumptionDto
                    {
                        Level = "Line",
                        TransformerId = transformerId.Value,
                        ZoneId = zoneId.Value,
                        LineId = x.LineId,
                        LineName = x.LineName,
                        Energy = x.EnergyConsumption
                    }).ToList();
                }
                else
                {
                    var data = await _emsContext.LineAnalysis
                        .Where(x =>
                            x.FactoryId == factoryId &&
                            x.ZoneId == zoneId &&
                            x.ShiftStartTime >= startTime &&
                            x.ShiftStartTime <= endTime)
                        .GroupBy(x => new { x.LineId, x.LineName })
                        .Select(g => new EnergyConsumptionDto
                        {
                            Level = "Line",
                            TransformerId = transformerId.Value,
                            ZoneId = zoneId.Value,
                            LineId = g.Key.LineId,
                            LineName = g.Key.LineName,
                            Energy = g.Sum(x => x.EnergyConsumption)
                        })
                        .ToListAsync();

                    return data;
                }
            }

            throw new ArgumentException("Invalid filters.");
        }

        public async Task<List<HourlyEnergyConsumptionDto>> GetHourlyEnergyPerTransformer(int factoryId, int? transformerId, int? zoneId, DateTime startTime, DateTime endTime, bool isCurrentShift)
        {
            bool groupByHour = !isCurrentShift ? (endTime - startTime).TotalHours <= 48 : true;

            // Factory level: hourly energy per transformer
            if (!transformerId.HasValue && !zoneId.HasValue)
            {
                if (groupByHour)
                {
                    if (isCurrentShift)
                    {
                        var data = await _emsContext.TransformerHourlyAnalysisCurrent
                            .Where(x => x.FactoryId == factoryId)
                            .ToListAsync();

                        return data.Select(x => new HourlyEnergyConsumptionDto
                        {
                            FactoryId = x.FactoryId,
                            HourStartTime = x.HourStartTime,
                            Level = "Transformer",
                            TransformerId = x.TransformerId,
                            TransformerName = x.TransformerName,
                            Energy = x.TotalEnergyConsumption
                        })
                        .OrderBy(x => x.HourStartTime)
                        .ToList();
                    }
                    else
                    {
                        var data = await _emsContext.TransformerHourlyAnalysis
                            .Where(x =>
                                x.FactoryId == factoryId &&
                                x.HourStartTime >= startTime &&
                                x.HourStartTime <= endTime)
                            .ToListAsync();

                        return data.Select(x => new HourlyEnergyConsumptionDto
                        {
                            FactoryId = x.FactoryId,
                            HourStartTime = x.HourStartTime,
                            Level = "Transformer",
                            TransformerId = x.TransformerId,
                            TransformerName = x.TransformerName,
                            Energy = x.TotalEnergyConsumption
                        })
                        .OrderBy(x => x.HourStartTime)
                        .ToList();
                    }
                }
                else
                {
                    var data = await _emsContext.TransformerAnalysis
                        .Where(x =>
                            x.FactoryId == factoryId &&
                            x.ShiftStartTime >= startTime &&
                            x.ShiftStartTime <= endTime)
                        .ToListAsync();

                    return data.Select(x => new HourlyEnergyConsumptionDto
                    {
                        FactoryId = x.FactoryId,
                        HourStartTime = x.ShiftStartTime,
                        Level = "Transformer",
                        TransformerId = x.TransformerId,
                        TransformerName = x.TransformerName,
                        Energy = x.TotalEnergyConsumption
                    })
                    .OrderBy(x => x.HourStartTime)
                    .ToList();
                }
            }

            // Transformer level: hourly energy per zone
            if (transformerId.HasValue && !zoneId.HasValue)
            {
                if (groupByHour)
                {
                    if (isCurrentShift)
                    {
                        var data = await _emsContext.ZoneHourlyAnalysisCurrent
                            .Where(x =>
                                x.FactoryId == factoryId &&
                                x.TransformerId == transformerId)
                            .ToListAsync();

                        return data.Select(x => new HourlyEnergyConsumptionDto
                        {
                            FactoryId = factoryId,
                            HourStartTime = x.HourStartTime,
                            Level = "Zone",
                            TransformerId = transformerId.Value,
                            ZoneId = x.ZoneId,
                            ZoneName = x.ZoneName,
                            Energy = x.EnergyConsumption
                        })
                        .OrderBy(x => x.HourStartTime)
                        .ToList();
                    }
                    else
                    {
                        var data = await _emsContext.ZoneHourlyAnalysis
                            .Where(x =>
                                x.FactoryId == factoryId &&
                                x.TransformerId == transformerId &&
                                x.HourStartTime >= startTime &&
                                x.HourStartTime <= endTime)
                            .ToListAsync();

                        return data.Select(x => new HourlyEnergyConsumptionDto
                        {
                            FactoryId = factoryId,
                            HourStartTime = x.HourStartTime,
                            Level = "Zone",
                            TransformerId = transformerId.Value,
                            ZoneId = x.ZoneId,
                            ZoneName = x.ZoneName,
                            Energy = x.EnergyConsumption
                        })
                        .OrderBy(x => x.HourStartTime)
                        .ToList();
                    }
                }
                else
                {
                    var data = await _emsContext.ZoneAnalysis
                        .Where(x =>
                            x.FactoryId == factoryId &&
                            x.TransformerId == transformerId &&
                            x.ShiftStartTime >= startTime &&
                            x.ShiftStartTime <= endTime)
                        .ToListAsync();

                    return data.Select(x => new HourlyEnergyConsumptionDto
                    {
                        FactoryId = factoryId,
                        HourStartTime = x.ShiftStartTime,
                        Level = "Zone",
                        TransformerId = transformerId.Value,
                        ZoneId = x.ZoneId,
                        ZoneName = x.ZoneName,
                        Energy = x.EnergyConsumption
                    })
                    .OrderBy(x => x.HourStartTime)
                    .ToList();
                }
            }

            // Zone level: hourly energy per line
            if (transformerId.HasValue && zoneId.HasValue)
            {
                if (groupByHour)
                {
                    if (isCurrentShift)
                    {
                        var data = await _emsContext.LineHourlyAnalysisCurrent
                            .Where(x =>
                                x.FactoryId == factoryId &&
                                x.ZoneId == zoneId)
                            .ToListAsync();

                        return data.Select(x => new HourlyEnergyConsumptionDto
                        {
                            FactoryId = factoryId,
                            HourStartTime = x.HourStartTime,
                            Level = "Line",
                            TransformerId = transformerId.Value,
                            ZoneId = zoneId.Value,
                            LineId = x.LineId,
                            LineName = x.LineName,
                            Energy = x.EnergyConsumption
                        })
                        .OrderBy(x => x.HourStartTime)
                        .ToList();
                    }
                    else
                    {
                        var data = await _emsContext.LineHourlyAnalysis
                            .Where(x =>
                                x.FactoryId == factoryId &&
                                x.ZoneId == zoneId &&
                                x.HourStartTime >= startTime &&
                                x.HourStartTime <= endTime)
                            .ToListAsync();

                        return data.Select(x => new HourlyEnergyConsumptionDto
                        {
                            FactoryId = factoryId,
                            HourStartTime = x.HourStartTime,
                            Level = "Line",
                            TransformerId = transformerId.Value,
                            ZoneId = zoneId.Value,
                            LineId = x.LineId,
                            LineName = x.LineName,
                            Energy = x.EnergyConsumption
                        })
                        .OrderBy(x => x.HourStartTime)
                        .ToList();
                    }
                }
                else
                {
                    var data = await _emsContext.LineAnalysis
                        .Where(x =>
                            x.FactoryId == factoryId &&
                            x.ZoneId == zoneId &&
                            x.ShiftStartTime >= startTime &&
                            x.ShiftStartTime <= endTime)
                        .ToListAsync();

                    return data.Select(x => new HourlyEnergyConsumptionDto
                    {
                        FactoryId = factoryId,
                        HourStartTime = x.ShiftStartTime,
                        Level = "Line",
                        TransformerId = transformerId.Value,
                        ZoneId = zoneId.Value,
                        LineId = x.LineId,
                        LineName = x.LineName,
                        Energy = x.EnergyConsumption
                    })
                    .OrderBy(x => x.HourStartTime)
                    .ToList();
                }
            }

            throw new ArgumentException("Invalid filters.");
        }
        public async Task<List<TopEnergyConsumerDtoV1>> GetTopEnergyConsumers(int factoryId, int? transformerId, int? zoneId, DateTime startTime, DateTime endTime, bool isCurrentShift, int top)
        {
            var data = await GetTotalEnergyPerTransformer(
                factoryId,
                transformerId,
                zoneId,
                startTime,
                endTime,
                isCurrentShift);

            decimal totalEnergy = data.Sum(x => x.Energy);

            return data
                .OrderByDescending(x => x.Energy)
                .Take(top)
                .Select((x, index) => new TopEnergyConsumerDtoV1
                {
                    Rank = index + 1,

                    Level = x.Level,

                    TransformerId = x.TransformerId,
                    TransformerName = x.TransformerName,

                    ZoneId = x.ZoneId,
                    ZoneName = x.ZoneName,

                    LineId = x.LineId,
                    LineName = x.LineName,

                    Energy = x.Energy,

                    PercentageOfTotal = totalEnergy > 0
                        ? Math.Round(x.Energy / totalEnergy * 100, 0)
                        : 0
                })
                .ToList();
        }



        #endregion





        #region Heatmap

        public async Task<List<EnergyHeatmapDto>> GetEnergyHeatmap(
            int factoryId,
            int? transformerId,
            int? zoneId,
            DateTime startTime,
            DateTime endTime,
            bool isCurrentShift)
        {
            bool groupByHour = ShouldGroupByHour(startTime, endTime, isCurrentShift);

            // Factory level: show transformers.
            if (!transformerId.HasValue && !zoneId.HasValue)
            {
                return await GetTransformerHeatmap(
                    factoryId,
                    startTime,
                    endTime,
                    isCurrentShift,
                    groupByHour);
            }

            // Transformer level: show zones.
            if (transformerId.HasValue && !zoneId.HasValue)
            {
                return await GetZoneHeatmap(
                    factoryId,
                    transformerId.Value,
                    startTime,
                    endTime,
                    isCurrentShift,
                    groupByHour);
            }

            // Zone level: show lines.
            if (transformerId.HasValue && zoneId.HasValue)
            {
                return await GetLineHeatmap(
                    factoryId,
                    transformerId.Value,
                    zoneId.Value,
                    startTime,
                    endTime,
                    isCurrentShift,
                    groupByHour);
            }

            throw new ArgumentException("Invalid filters.");
        }

        private async Task<List<EnergyHeatmapDto>> GetTransformerHeatmap(
            int factoryId,
            DateTime startTime,
            DateTime endTime,
            bool isCurrentShift,
            bool groupByHour)
        {
            var threshold = await GetThreshold("Transformer");

            var data = groupByHour
                ? await GetTransformerPeriodEnergy(
                    factoryId,
                    null,
                    startTime,
                    endTime,
                    isCurrentShift,
                    groupByHour: true)
                : await GetTransformerPeriodEnergy(
                    factoryId,
                    null,
                    startTime,
                    endTime,
                    isCurrentShift,
                    groupByHour: false);

            return data
                .Select(x => new EnergyHeatmapDto
                {
                    Date = x.Period,
                    PeriodType = groupByHour ? "Hour" : "Day",

                    Level = "Transformer",
                    TransformerId = x.TransformerId,
                    TransformerName = x.TransformerName,

                    Energy = x.Energy,
                    Status = GetEnergyStatus(x.Energy, threshold)
                })
                .OrderBy(x => x.Date)
                .ThenBy(x => x.TransformerName)
                .ToList();
        }

        //private async Task<List<EnergyHeatmapDto>> GetZoneHeatmap(
        //    int factoryId,
        //    int transformerId,
        //    DateTime startTime,
        //    DateTime endTime,
        //    bool isCurrentShift,
        //    bool groupByHour)
        //{
        //    var threshold = await GetThreshold("Zone");
        //    var zones = await GetZonesByTransformer(factoryId, transformerId);

        //    var transformerEnergy = await GetTransformerPeriodEnergy(
        //        factoryId,
        //        transformerId,
        //        startTime,
        //        endTime,
        //        isCurrentShift,
        //        groupByHour);

        //    return transformerEnergy
        //        .SelectMany(period => zones.Select(zone =>
        //        {
        //            decimal zoneEnergy = period.Energy * zone.Ratio;

        //            return new EnergyHeatmapDto
        //            {
        //                Date = period.Period,
        //                PeriodType = groupByHour ? "Hour" : "Day",

        //                Level = "Zone",
        //                TransformerId = transformerId,
        //                ZoneId = zone.Id,
        //                ZoneName = zone.Name,

        //                Energy = zoneEnergy,
        //                Status = GetEnergyStatus(zoneEnergy, threshold)
        //            };
        //        }))
        //        .OrderBy(x => x.Date)
        //        .ThenBy(x => x.ZoneName)
        //        .ToList();
        //}
        private async Task<List<EnergyHeatmapDto>> GetZoneHeatmap(
    int factoryId,
    int transformerId,
    DateTime startTime,
    DateTime endTime,
    bool isCurrentShift,
    bool groupByHour)
        {
            var threshold = await GetThreshold("Zone");

            if (groupByHour)
            {
                if (isCurrentShift)
                {
                    var data = await _emsContext.ZoneHourlyAnalysisCurrent
                        .Where(x => x.FactoryId == factoryId && x.TransformerId == transformerId)
                        .ToListAsync();

                    return data.Select(x => new EnergyHeatmapDto
                    {
                        Date = x.HourStartTime,
                        PeriodType = "Hour",
                        Level = "Zone",
                        TransformerId = transformerId,
                        ZoneId = x.ZoneId,
                        ZoneName = x.ZoneName,
                        Energy = x.EnergyConsumption,
                        Status = GetEnergyStatus(x.EnergyConsumption, threshold)
                    })
                    .OrderBy(x => x.Date)
                    .ThenBy(x => x.ZoneName)
                    .ToList();
                }
                else
                {
                    var data = await _emsContext.ZoneHourlyAnalysis
                        .Where(x =>
                            x.FactoryId == factoryId &&
                            x.TransformerId == transformerId &&
                            x.HourStartTime >= startTime &&
                            x.HourStartTime <= endTime)
                        .ToListAsync();

                    return data.Select(x => new EnergyHeatmapDto
                    {
                        Date = x.HourStartTime,
                        PeriodType = "Hour",
                        Level = "Zone",
                        TransformerId = transformerId,
                        ZoneId = x.ZoneId,
                        ZoneName = x.ZoneName,
                        Energy = x.EnergyConsumption,
                        Status = GetEnergyStatus(x.EnergyConsumption, threshold)
                    })
                    .OrderBy(x => x.Date)
                    .ThenBy(x => x.ZoneName)
                    .ToList();
                }
            }
            else
            {
                var data = await _emsContext.ZoneAnalysis
                    .Where(x =>
                        x.FactoryId == factoryId &&
                        x.TransformerId == transformerId &&
                        x.ShiftStartTime >= startTime &&
                        x.ShiftStartTime <= endTime)
                    .ToListAsync();

                return data.Select(x => new EnergyHeatmapDto
                {
                    Date = x.ShiftStartTime,
                    PeriodType = "Shift",
                    Level = "Zone",
                    TransformerId = transformerId,
                    ZoneId = x.ZoneId,
                    ZoneName = x.ZoneName,
                    Energy = x.EnergyConsumption,
                    Status = GetEnergyStatus(x.EnergyConsumption, threshold)
                })
                .OrderBy(x => x.Date)
                .ThenBy(x => x.ZoneName)
                .ToList();
            }
        }

        //private async Task<List<EnergyHeatmapDto>> GetLineHeatmap(
        //    int factoryId,
        //    int transformerId,
        //    int zoneId,
        //    DateTime startTime,
        //    DateTime endTime,
        //    bool isCurrentShift,
        //    bool groupByHour)
        //{
        //    var threshold = await GetThreshold("Line");

        //    decimal zoneRatio = await GetZoneRatio(factoryId, transformerId, zoneId);
        //    var lines = await GetLinesByZone(factoryId, zoneId);

        //    var transformerEnergy = await GetTransformerPeriodEnergy(
        //        factoryId,
        //        transformerId,
        //        startTime,
        //        endTime,
        //        isCurrentShift,
        //        groupByHour);

        //    return transformerEnergy
        //        .SelectMany(period =>
        //        {
        //            decimal zoneEnergy = period.Energy * zoneRatio;

        //            return lines.Select(line =>
        //            {
        //                decimal lineEnergy = zoneEnergy * line.Ratio;

        //                return new EnergyHeatmapDto
        //                {
        //                    Date = period.Period,
        //                    PeriodType = groupByHour ? "Hour" : "Day",

        //                    Level = "Line",
        //                    TransformerId = transformerId,
        //                    ZoneId = zoneId,
        //                    LineId = line.Id,
        //                    LineName = line.Name,

        //                    Energy = lineEnergy,
        //                    Status = GetEnergyStatus(lineEnergy, threshold)
        //                };
        //            });
        //        })
        //        .OrderBy(x => x.Date)
        //        .ThenBy(x => x.LineName)
        //        .ToList();
        //}


        private async Task<List<EnergyHeatmapDto>> GetLineHeatmap(
    int factoryId,
    int transformerId,
    int zoneId,
    DateTime startTime,
    DateTime endTime,
    bool isCurrentShift,
    bool groupByHour)
        {
            var threshold = await GetThreshold("Line");

            if (groupByHour)
            {
                if (isCurrentShift)
                {
                    var data = await _emsContext.LineHourlyAnalysisCurrent
                        .Where(x =>
                            x.FactoryId == factoryId &&
                            x.ZoneId == zoneId)
                        .ToListAsync();

                    return data.Select(x => new EnergyHeatmapDto
                    {
                        Date = x.HourStartTime,
                        PeriodType = "Hour",
                        Level = "Line",
                        TransformerId = transformerId,
                        ZoneId = zoneId,
                        LineId = x.LineId,
                        LineName = x.LineName,
                        Energy = x.EnergyConsumption,
                        Status = GetEnergyStatus(x.EnergyConsumption, threshold)
                    })
                    .OrderBy(x => x.Date)
                    .ThenBy(x => x.LineName)
                    .ToList();
                }
                else
                {
                    var data = await _emsContext.LineHourlyAnalysis
                        .Where(x =>
                            x.FactoryId == factoryId &&
                            x.ZoneId == zoneId &&
                            x.HourStartTime >= startTime &&
                            x.HourStartTime <= endTime)
                        .ToListAsync();

                    return data.Select(x => new EnergyHeatmapDto
                    {
                        Date = x.HourStartTime,
                        PeriodType = "Hour",
                        Level = "Line",
                        TransformerId = transformerId,
                        ZoneId = zoneId,
                        LineId = x.LineId,
                        LineName = x.LineName,
                        Energy = x.EnergyConsumption,
                        Status = GetEnergyStatus(x.EnergyConsumption, threshold)
                    })
                    .OrderBy(x => x.Date)
                    .ThenBy(x => x.LineName)
                    .ToList();
                }
            }
            else
            {
                var data = await _emsContext.LineAnalysis
                    .Where(x =>
                        x.FactoryId == factoryId &&
                        x.ZoneId == zoneId &&
                        x.ShiftStartTime >= startTime &&
                        x.ShiftStartTime <= endTime)
                    .ToListAsync();

                return data.Select(x => new EnergyHeatmapDto
                {
                    Date = x.ShiftStartTime,
                    PeriodType = "Shift",
                    Level = "Line",
                    TransformerId = transformerId,
                    ZoneId = zoneId,
                    LineId = x.LineId,
                    LineName = x.LineName,
                    Energy = x.EnergyConsumption,
                    Status = GetEnergyStatus(x.EnergyConsumption, threshold)
                })
                .OrderBy(x => x.Date)
                .ThenBy(x => x.LineName)
                .ToList();
            }
        }

        #endregion













        #region Shared Data Helpers

        private async Task<List<TransformerEnergyDto>> GetTransformerTotalEnergyList(
            int factoryId,
            DateTime startTime,
            DateTime endTime,
            bool isCurrentShift)
        {
            if (isCurrentShift)
            {
                return await _emsContext.VW_TransformerAnalysis
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

            return await _emsContext.TransformerAnalysis
                .Where(x =>
                    x.FactoryId == factoryId &&
                    x.ShiftStartTime >= startTime &&
                    x.ShiftStartTime <= endTime)
                .GroupBy(x => new { x.TransformerId, x.TransformerName })
                .Select(g => new TransformerEnergyDto
                {
                    TransformerId = g.Key.TransformerId,
                    TransformerName = g.Key.TransformerName,
                    TotalEnergyConsumption = g.Sum(x => x.TotalEnergyConsumption)
                })
                .ToListAsync();
        }

        private async Task<decimal> GetTransformerTotalEnergy(
            int factoryId,
            int transformerId,
            DateTime startTime,
            DateTime endTime,
            bool isCurrentShift)
        {
            if (isCurrentShift)
            {
                return await _emsContext.VW_TransformerAnalysis
                    .Where(x =>
                        x.FactoryId == factoryId &&
                        x.TransformerId == transformerId)
                    .SumAsync(x => x.TotalEnergyConsumption);
            }

            return await _emsContext.TransformerAnalysis
                .Where(x =>
                    x.FactoryId == factoryId &&
                    x.TransformerId == transformerId &&
                    x.ShiftStartTime >= startTime &&
                    x.ShiftStartTime <= endTime)
                .SumAsync(x => x.TotalEnergyConsumption);
        }

        private async Task<List<TransformerHourlyEnergyDto>> GetTransformerHourlyEnergyList(
            int factoryId,
            int? transformerId,
            DateTime startTime,
            DateTime endTime,
            bool isCurrentShift)
        {
            if (isCurrentShift)
            {
                var query = _emsContext.VW_TransformerHourlyAnalysis
                    .Where(x => x.FactoryId == factoryId);

                if (transformerId.HasValue)
                    query = query.Where(x => x.TransformerId == transformerId.Value);

                return await query
                    .GroupBy(x => new
                    {
                        x.FactoryId,
                        x.TransformerId,
                        x.TransformerName,
                        x.HourStartTime
                    })
                    .Select(g => new TransformerHourlyEnergyDto
                    {
                        FactoryId = g.Key.FactoryId,
                        TransformerId = g.Key.TransformerId,
                        TransformerName = g.Key.TransformerName,
                        HourStartTime = g.Key.HourStartTime,
                        TotalEnergyConsumption = g.Sum(x => x.TotalEnergyConsumption)
                    })
                    .OrderBy(x => x.HourStartTime)
                    .ToListAsync();
            }

            var historicalQuery = _emsContext.TransformerHourlyAnalysis
                .Where(x =>
                    x.FactoryId == factoryId &&
                    x.ShiftStartTime >= startTime &&
                    x.ShiftStartTime <= endTime);

            if (transformerId.HasValue)
                historicalQuery = historicalQuery.Where(x => x.TransformerId == transformerId.Value);

            return await historicalQuery
                .GroupBy(x => new
                {
                    x.FactoryId,
                    x.TransformerId,
                    x.TransformerName,
                    x.HourStartTime
                })
                .Select(g => new TransformerHourlyEnergyDto
                {
                    FactoryId = g.Key.FactoryId,
                    TransformerId = g.Key.TransformerId,
                    TransformerName = g.Key.TransformerName,
                    HourStartTime = g.Key.HourStartTime,
                    TotalEnergyConsumption = g.Sum(x => x.TotalEnergyConsumption)
                })
                .OrderBy(x => x.HourStartTime)
                .ToListAsync();
        }

        private async Task<List<TransformerPeriodEnergyRow>> GetTransformerPeriodEnergy(
            int factoryId,
            int? transformerId,
            DateTime startTime,
            DateTime endTime,
            bool isCurrentShift,
            bool groupByHour)
        {
            if (groupByHour)
            {
                if (isCurrentShift)
                {
                    var query = _emsContext.VW_TransformerHourlyAnalysis
                        .Where(x => x.FactoryId == factoryId);

                    if (transformerId.HasValue)
                        query = query.Where(x => x.TransformerId == transformerId.Value);

                    return await query
                        .GroupBy(x => new
                        {
                            Period = x.HourStartTime,
                            x.TransformerId,
                            x.TransformerName
                        })
                        .Select(g => new TransformerPeriodEnergyRow
                        {
                            Period = g.Key.Period,
                            TransformerId = g.Key.TransformerId,
                            TransformerName = g.Key.TransformerName,
                            Energy = g.Sum(x => x.TotalEnergyConsumption)
                        })
                        .ToListAsync();
                }

                var hourlyQuery = _emsContext.TransformerHourlyAnalysis
                    .Where(x =>
                        x.FactoryId == factoryId &&
                        x.HourStartTime >= startTime &&
                        x.HourStartTime <= endTime);

                if (transformerId.HasValue)
                    hourlyQuery = hourlyQuery.Where(x => x.TransformerId == transformerId.Value);

                return await hourlyQuery
                    .GroupBy(x => new
                    {
                        Period = x.HourStartTime,
                        x.TransformerId,
                        x.TransformerName
                    })
                    .Select(g => new TransformerPeriodEnergyRow
                    {
                        Period = g.Key.Period,
                        TransformerId = g.Key.TransformerId,
                        TransformerName = g.Key.TransformerName,
                        Energy = g.Sum(x => x.TotalEnergyConsumption)
                    })
                    .ToListAsync();
            }

            var dailyQuery = _emsContext.TransformerAnalysis
                .Where(x =>
                    x.FactoryId == factoryId &&
                    x.ShiftStartTime >= startTime &&
                    x.ShiftStartTime <= endTime);

            if (transformerId.HasValue)
                dailyQuery = dailyQuery.Where(x => x.TransformerId == transformerId.Value);

            return await dailyQuery
                .GroupBy(x => new
                {
                    Period = x.ShiftStartTime.Date,
                    x.TransformerId,
                    x.TransformerName
                })
                .Select(g => new TransformerPeriodEnergyRow
                {
                    Period = g.Key.Period,
                    TransformerId = g.Key.TransformerId,
                    TransformerName = g.Key.TransformerName,
                    Energy = g.Sum(x => x.TotalEnergyConsumption)
                })
                .ToListAsync();
        }

        private async Task<List<RatioItem>> GetZonesByTransformer(
            int factoryId,
            int transformerId)
        {
            return await _emsContext.Zones
                .Where(x =>
                    x.FactoryId == factoryId &&
                    x.TransformerId == transformerId)
                .Select(x => new RatioItem
                {
                    Id = x.Id,
                    Name = x.Name,
                    Ratio = NormalizeRatio(x.RatioFromParent ?? 0)
                })
                .ToListAsync();
        }

        private async Task<List<RatioItem>> GetLinesByZone(
            int factoryId,
            int zoneId)
        {
            // Lines are linked to zones, not directly to transformers.
            return await _emsContext.Lines
                .Where(x =>
                    x.FactoryId == factoryId &&
                    x.ZoneId == zoneId)
                .Select(x => new RatioItem
                {
                    Id = x.Id,
                    Name = x.Name,
                    Ratio = NormalizeRatio(x.RatioFromParent ?? 0)
                })
                .ToListAsync();
        }

        private async Task<decimal> GetZoneRatio(
            int factoryId,
            int transformerId,
            int zoneId)
        {
            var zone = await _emsContext.Zones
                .Where(x =>
                    x.Id == zoneId &&
                    x.FactoryId == factoryId &&
                    x.TransformerId == transformerId)
                .Select(x => new
                {
                    x.RatioFromParent
                })
                .FirstOrDefaultAsync();

            if (zone == null)
                throw new ArgumentException("Zone does not belong to this transformer or factory.");

            return NormalizeRatio(zone.RatioFromParent ?? 0);
        }

        #endregion











        #region Summary Helpers

        private Task<List<int>> GetFactoryIds(int? factoryId)
        {
            if (factoryId.HasValue && factoryId.Value > 0)
                return Task.FromResult(new List<int> { factoryId.Value });

            return Task.FromResult(new List<int> { 2, 3 });
        }

        private async Task<TransformerSummaryResult> GetTransformersSummary(
            List<int> factoryIds,
            DateTime startTime,
            DateTime endTime,
            bool isCurrentShift)
        {
            int totalTransformers = await _emsContext.Transformers
                .Where(x => x.FactoryId.HasValue && factoryIds.Contains(x.FactoryId.Value))
                .CountAsync();

            if (isCurrentShift)
            {
                var data = await _emsContext.VW_TransformerAnalysis
                    .Where(x => factoryIds.Contains(x.FactoryId))
                    .ToListAsync();

                int onlineCount = data.Count(x => x.Status == "Online");

                return new TransformerSummaryResult
                {
                    TotalEnergy = data.Sum(x => x.TotalEnergyConsumption),
                    TotalCount = totalTransformers,
                    OnlineCount = onlineCount,
                    OfflineCount = totalTransformers - onlineCount,
                    AvgPowerFactor = data.Any(x => x.Status == "Online")
                        ? data.Where(x => x.Status == "Online").Average(x => x.PowerFactor)
                        : 0
                };
            }

            var historicalData = await _emsContext.TransformerAnalysis
                .Where(x => factoryIds.Contains(x.FactoryId))
                .Where(x => x.ShiftStartTime >= startTime && x.ShiftStartTime <= endTime)
                .ToListAsync();

            var latestPerTransformer = historicalData
                .GroupBy(x => x.TransformerId)
                .Select(g => g.OrderByDescending(x => x.ShiftStartTime).First())
                .ToList();

            int historicalOnlineCount = latestPerTransformer.Count(x => x.Status == "Online");

            return new TransformerSummaryResult
            {
                TotalEnergy = historicalData.Sum(x => x.TotalEnergyConsumption),
                TotalCount = totalTransformers,
                OnlineCount = historicalOnlineCount,
                OfflineCount = totalTransformers - historicalOnlineCount,
                AvgPowerFactor = latestPerTransformer.Any(x => x.Status == "Online")
                    ? latestPerTransformer.Where(x => x.Status == "Online").Average(x => x.PowerFactor)
                    : 0
            };
        }

        private async Task<CountStatusDto> GetLinesSummary(List<int> factoryIds)
        {
            DateTime onlineThreshold = DateTime.Now.AddHours(-3);

            var lines = await _emsContext.Lines
                .Where(x => x.FactoryId.HasValue && factoryIds.Contains(x.FactoryId.Value))
                .Select(x => new
                {
                    x.Id
                })
                .ToListAsync();

            var lineIds = lines.Select(x => x.Id).ToList();

            var onlineLineIds = await _emsContext.Signals
                .Where(x => x.LineId.HasValue)
                .Where(x => lineIds.Contains(x.LineId.Value))
                .Where(x => x.TimeStamp >= onlineThreshold)
                .Select(x => x.LineId.Value)
                .Distinct()
                .ToListAsync();

            int onlineCount = lines.Count(x => onlineLineIds.Contains(x.Id));

            return new CountStatusDto
            {
                TotalCount = lines.Count,
                OnlineCount = onlineCount,
                OfflineCount = lines.Count - onlineCount
            };
        }

        private async Task<CountStatusDto> GetZonesSummary(List<int> factoryIds)
        {
            DateTime onlineThreshold = DateTime.Now.AddHours(-3);

            var zones = await _emsContext.Zones
                .Where(x => factoryIds.Contains(x.FactoryId))
                .Select(x => new
                {
                    x.Id
                })
                .ToListAsync();

            var zoneIds = zones.Select(x => x.Id).ToList();

            var lines = await _emsContext.Lines
                .Where(x => x.FactoryId.HasValue && factoryIds.Contains(x.FactoryId.Value))
                .Where(x => x.ZoneId.HasValue)
                .Where(x => zoneIds.Contains(x.ZoneId.Value))
                .Select(x => new
                {
                    x.Id,
                    ZoneId = x.ZoneId.Value
                })
                .ToListAsync();

            var lineIds = lines.Select(x => x.Id).ToList();

            var onlineLineIds = await _emsContext.Signals
                .Where(x => x.LineId.HasValue)
                .Where(x => lineIds.Contains(x.LineId.Value))
                .Where(x => x.TimeStamp >= onlineThreshold)
                .Select(x => x.LineId.Value)
                .Distinct()
                .ToListAsync();

            var onlineZoneIds = lines
                .Where(line => onlineLineIds.Contains(line.Id))
                .Select(line => line.ZoneId)
                .Distinct()
                .ToList();

            int onlineZones = zones.Count(x => onlineZoneIds.Contains(x.Id));

            return new CountStatusDto
            {
                TotalCount = zones.Count,
                OnlineCount = onlineZones,
                OfflineCount = zones.Count - onlineZones
            };
        }

        #endregion









        #region Threshold Helpers

        private async Task<EnergyHeatmapThreshold> GetThreshold(string level)
        {
            var threshold = await _emsContext.EnergyHeatmapThresholds
                .Where(x => x.Level == level && x.IsActive)
                .FirstOrDefaultAsync();

            if (threshold == null)
                throw new ArgumentException($"Energy heatmap threshold for level '{level}' is not configured.");

            return threshold;
        }

        private static EnergyStatus GetEnergyStatus(
            decimal energy,
            EnergyHeatmapThreshold threshold)
        {
            if (energy >= threshold.LowFrom && energy <= threshold.LowTo)
                return EnergyStatus.Low;

            if (energy >= threshold.MediumFrom && energy <= threshold.MediumTo)
                return EnergyStatus.Medium;

            if (energy >= threshold.HighFrom && energy <= threshold.HighTo)
                return EnergyStatus.High;

            return EnergyStatus.Unknown;
        }

        #endregion







        #region Utility Helpers

        private static decimal NormalizeRatio(decimal ratio)
        {
            return ratio > 1 ? ratio / 100 : ratio;
        }

        private static bool ShouldGroupByHour(
            DateTime startTime,
            DateTime endTime,
            bool isCurrentShift)
        {
            if (isCurrentShift)
                return true;

            return (endTime - startTime).TotalHours <= 24;
        }

        #endregion











        #region Private Models

        private class RatioItem
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public decimal Ratio { get; set; }
        }

        private class TransformerPeriodEnergyRow
        {
            public DateTime Period { get; set; }
            public int TransformerId { get; set; }
            public string TransformerName { get; set; } = string.Empty;
            public decimal Energy { get; set; }
        }

        private class TransformerSummaryResult
        {
            public decimal TotalEnergy { get; set; }

            public int TotalCount { get; set; }
            public int OnlineCount { get; set; }
            public int OfflineCount { get; set; }

            public decimal AvgPowerFactor { get; set; }
        }

        #endregion
    }
}