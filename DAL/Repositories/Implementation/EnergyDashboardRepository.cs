using BLL.Dtos;
using DAL.Models.Threshold;
using DAL.Repositories.Interface;
using Shared.Dtos;
using Shared.Dtos.EnergyDashboard;
using Shared.Dtos.Heatmap;

namespace DAL.Repositories.Implementation
{
    public class EnergyDashboardRepository : IEnergyDashboardRepository
    {
        private readonly EmsContext _emsContext;

        public EnergyDashboardRepository(EmsContext emsContext)
        {
            _emsContext = emsContext;
        }

        public async Task<DashboardSummaryDto> GetSummary(
            int? factoryId,
            DateTime startTime,
            DateTime endTime,
            bool isCurrentShift)
        {
            var factoryIds = await GetFactoryIds(factoryId);

            var transformerSummary = await GetTransformersSummary(
                factoryIds,
                null,
                startTime,
                endTime,
                isCurrentShift
            );

            var lineSummary = await GetLinesSummary(
                factoryIds,
                null
            );

            var zoneSummary = await GetZonesSummary(
                factoryIds,
                null
            );

            return new DashboardSummaryDto
            {
                Level = factoryId.HasValue && factoryId.Value > 0
                    ? "Factory"
                    : "Main",

                TotalEnergy = transformerSummary.TotalEnergy,
                AvgPowerFactor = transformerSummary.AvgPowerFactor,

                Transformers = new CountStatusDto
                {
                    TotalCount = transformerSummary.TotalCount,
                    OnlineCount = transformerSummary.OnlineCount,
                    OfflineCount = transformerSummary.OfflineCount
                },

                Lines = lineSummary,

                Zones = zoneSummary
            };
        }

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
                    .Where(x =>
                        factoryIds.Contains(x.FactoryId) &&
                        x.Status == "Online")
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
                    .Where(x =>
                        factoryIds.Contains(x.FactoryId) &&
                        x.Status == "Online")
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



        // HeatMap
        public async Task<List<EnergyHeatmapDto>> GetEnergyHeatmap(
            int factoryId,
            int? transformerId,
            int? zoneId,
            DateTime startTime,
            DateTime endTime,
            bool isCurrentShift)
        {
            if (!transformerId.HasValue && !zoneId.HasValue)
            {
                return await GetTransformerHeatmap(
                    factoryId,
                    startTime,
                    endTime,
                    isCurrentShift);
            }

            if (transformerId.HasValue && !zoneId.HasValue)
            {
                return await GetZoneHeatmap(
                    factoryId,
                    transformerId.Value,
                    startTime,
                    endTime,
                    isCurrentShift);
            }

            if (transformerId.HasValue && zoneId.HasValue)
            {
                return await GetLineHeatmap(
                    factoryId,
                    transformerId.Value,
                    zoneId.Value,
                    startTime,
                    endTime,
                    isCurrentShift);
            }

            throw new Exception("Invalid filters.");
        }


        public async Task<List<TransformerEnergyDto>> GetTotalEnergyPerTransformer(
    int factoryId,
    int? transformerId,
    int? zoneId,
    DateTime startTime,
    DateTime endTime,
    bool isCurrentShift)
        {
            // Factory Level: return Transformers
            if (!transformerId.HasValue && !zoneId.HasValue)
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
                else
                {
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
            }

            // Transformer Level: return Zones
            if (transformerId.HasValue && !zoneId.HasValue)
            {
                var zonesRaw = await _emsContext.Zones
                    .Where(x =>
                        x.FactoryId == factoryId &&
                        x.TransformerId == transformerId.Value)
                    .Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.RatioFromParent
                    })
                    .ToListAsync();

                var zones = zonesRaw.Select(x => new
                {
                    x.Id,
                    x.Name,
                    Ratio = NormalizeRatio(x.RatioFromParent ?? 0)
                }).ToList();

                decimal transformerTotalEnergy;

                if (isCurrentShift)
                {
                    transformerTotalEnergy = await _emsContext.VW_TransformerAnalysis
                        .Where(x =>
                            x.FactoryId == factoryId &&
                            x.TransformerId == transformerId.Value)
                        .SumAsync(x => x.TotalEnergyConsumption);
                }
                else
                {
                    transformerTotalEnergy = await _emsContext.TransformerAnalysis
                        .Where(x =>
                            x.FactoryId == factoryId &&
                            x.TransformerId == transformerId.Value &&
                            x.ShiftStartTime >= startTime &&
                            x.ShiftStartTime <= endTime)
                        .SumAsync(x => x.TotalEnergyConsumption);
                }

                return zones
                    .Select(z => new TransformerEnergyDto
                    {
                        TransformerId = z.Id,
                        TransformerName = z.Name,
                        TotalEnergyConsumption = transformerTotalEnergy * z.Ratio
                    })
                    .ToList();
            }

            // Zone Level: return Lines
            if (transformerId.HasValue && zoneId.HasValue)
            {
                var zoneRaw = await _emsContext.Zones
                    .Where(x =>
                        x.Id == zoneId.Value &&
                        x.FactoryId == factoryId &&
                        x.TransformerId == transformerId.Value)
                    .Select(x => new
                    {
                        x.RatioFromParent
                    })
                    .FirstOrDefaultAsync();

                if (zoneRaw == null)
                    throw new Exception("Zone does not belong to this transformer or factory.");

                decimal zoneRatio = NormalizeRatio(zoneRaw.RatioFromParent ?? 0);

                var linesRaw = await _emsContext.Lines
                    .Where(x =>
                        x.FactoryId == factoryId &&
                        x.ZoneId == zoneId.Value &&
                        x.LineTransformers.Any(lt => lt.TransformerId == transformerId.Value))
                    .Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.RatioFromParent
                    })
                    .ToListAsync();

                var lines = linesRaw.Select(x => new
                {
                    x.Id,
                    x.Name,
                    Ratio = NormalizeRatio(x.RatioFromParent ?? 0)
                }).ToList();

                decimal transformerTotalEnergy;

                if (isCurrentShift)
                {
                    transformerTotalEnergy = await _emsContext.VW_TransformerAnalysis
                        .Where(x =>
                            x.FactoryId == factoryId &&
                            x.TransformerId == transformerId.Value)
                        .SumAsync(x => x.TotalEnergyConsumption);
                }
                else
                {
                    transformerTotalEnergy = await _emsContext.TransformerAnalysis
                        .Where(x =>
                            x.FactoryId == factoryId &&
                            x.TransformerId == transformerId.Value &&
                            x.ShiftStartTime >= startTime &&
                            x.ShiftStartTime <= endTime)
                        .SumAsync(x => x.TotalEnergyConsumption);
                }

                decimal zoneTotalEnergy = transformerTotalEnergy * zoneRatio;

                return lines
                    .Select(l => new TransformerEnergyDto
                    {
                        TransformerId = l.Id,
                        TransformerName = l.Name,
                        TotalEnergyConsumption = zoneTotalEnergy * l.Ratio
                    })
                    .ToList();
            }

            throw new Exception("Invalid filters.");
        }

        public async Task<List<TransformerHourlyEnergyDto>> GetHourlyEnergyPerTransformer(
    int factoryId,
    int? transformerId,
    int? zoneId,
    DateTime startTime,
    DateTime endTime,
    bool isCurrentShift)
        {
            // Factory Level: return hourly energy per Transformer
            if (!transformerId.HasValue && !zoneId.HasValue)
            {
                if (isCurrentShift)
                {
                    return await _emsContext.VW_TransformerHourlyAnalysis
                        .Where(x => x.FactoryId == factoryId)
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
                else
                {
                    return await _emsContext.TransformerHourlyAnalysis
                        .Where(x =>
                            x.FactoryId == factoryId &&
                            x.ShiftStartTime >= startTime &&
                            x.ShiftStartTime <= endTime)
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
            }

            // Transformer Level: return hourly energy per Zone
            if (transformerId.HasValue && !zoneId.HasValue)
            {
                var zonesRaw = await _emsContext.Zones
                    .Where(x =>
                        x.FactoryId == factoryId &&
                        x.TransformerId == transformerId.Value)
                    .Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.RatioFromParent
                    })
                    .ToListAsync();

                var zones = zonesRaw.Select(x => new
                {
                    x.Id,
                    x.Name,
                    Ratio = NormalizeRatio(x.RatioFromParent ?? 0)
                }).ToList();

                List<TransformerHourlyEnergyDto> transformerHourlyData;

                if (isCurrentShift)
                {
                    transformerHourlyData = await _emsContext.VW_TransformerHourlyAnalysis
                        .Where(x =>
                            x.FactoryId == factoryId &&
                            x.TransformerId == transformerId.Value)
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
                else
                {
                    transformerHourlyData = await _emsContext.TransformerHourlyAnalysis
                        .Where(x =>
                            x.FactoryId == factoryId &&
                            x.TransformerId == transformerId.Value &&
                            x.ShiftStartTime >= startTime &&
                            x.ShiftStartTime <= endTime)
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

                var result = new List<TransformerHourlyEnergyDto>();

                foreach (var hour in transformerHourlyData)
                {
                    foreach (var zone in zones)
                    {
                        result.Add(new TransformerHourlyEnergyDto
                        {
                            FactoryId = factoryId,
                            TransformerId = zone.Id,
                            TransformerName = zone.Name,
                            HourStartTime = hour.HourStartTime,
                            TotalEnergyConsumption = hour.TotalEnergyConsumption * zone.Ratio
                        });
                    }
                }

                return result
                    .OrderBy(x => x.HourStartTime)
                    .ToList();
            }

            // Zone Level: return hourly energy per Line
            if (transformerId.HasValue && zoneId.HasValue)
            {
                var zoneRaw = await _emsContext.Zones
                    .Where(x =>
                        x.Id == zoneId.Value &&
                        x.FactoryId == factoryId &&
                        x.TransformerId == transformerId.Value)
                    .Select(x => new
                    {
                        x.RatioFromParent
                    })
                    .FirstOrDefaultAsync();

                if (zoneRaw == null)
                    throw new Exception("Zone does not belong to this transformer or factory.");

                decimal zoneRatio = NormalizeRatio(zoneRaw.RatioFromParent ?? 0);

                var linesRaw = await _emsContext.Lines
                    .Where(x =>
                        x.FactoryId == factoryId &&
                        x.ZoneId == zoneId.Value &&
                        x.LineTransformers.Any(lt => lt.TransformerId == transformerId.Value))
                    .Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.RatioFromParent
                    })
                    .ToListAsync();

                var lines = linesRaw.Select(x => new
                {
                    x.Id,
                    x.Name,
                    Ratio = NormalizeRatio(x.RatioFromParent ?? 0)
                }).ToList();

                List<TransformerHourlyEnergyDto> transformerHourlyData;

                if (isCurrentShift)
                {
                    transformerHourlyData = await _emsContext.VW_TransformerHourlyAnalysis
                        .Where(x =>
                            x.FactoryId == factoryId &&
                            x.TransformerId == transformerId.Value)
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
                else
                {
                    transformerHourlyData = await _emsContext.TransformerHourlyAnalysis
                        .Where(x =>
                            x.FactoryId == factoryId &&
                            x.TransformerId == transformerId.Value &&
                            x.ShiftStartTime >= startTime &&
                            x.ShiftStartTime <= endTime)
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

                var result = new List<TransformerHourlyEnergyDto>();

                foreach (var hour in transformerHourlyData)
                {
                    decimal zoneHourEnergy = hour.TotalEnergyConsumption * zoneRatio;

                    foreach (var line in lines)
                    {
                        result.Add(new TransformerHourlyEnergyDto
                        {
                            FactoryId = factoryId,
                            TransformerId = line.Id,
                            TransformerName = line.Name,
                            HourStartTime = hour.HourStartTime,
                            TotalEnergyConsumption = zoneHourEnergy * line.Ratio
                        });
                    }
                }

                return result
                    .OrderBy(x => x.HourStartTime)
                    .ToList();
            }

            throw new Exception("Invalid filters.");
        }
        public async Task<List<TopEnergyConsumerDto>> GetTopEnergyConsumers(
    int factoryId,
    int? transformerId,
    int? zoneId,
    DateTime startTime,
    DateTime endTime,
    bool isCurrentShift,
    int top)
        {
            var data = await GetTotalEnergyPerTransformer(
                factoryId,
                transformerId,
                zoneId,
                startTime,
                endTime,
                isCurrentShift
            );

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

        private static decimal NormalizeRatio(decimal ratio)
        {
            return ratio > 1 ? ratio / 100 : ratio;
        }



        #region Helper Methods for GetSummary endpoint
        private Task<List<int>> GetFactoryIds(int? factoryId)
        {
            if (factoryId.HasValue && factoryId.Value > 0)
                return Task.FromResult(new List<int> { factoryId.Value });

            return Task.FromResult(new List<int> { 2, 3 });
        }

        private async Task<TransformerSummaryResult> GetTransformersSummary(
            List<int> factoryIds,
            int? transformerId,
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



        private class TransformerSummaryResult
        {
            public decimal TotalEnergy { get; set; }

            public int TotalCount { get; set; }
            public int OnlineCount { get; set; }
            public int OfflineCount { get; set; }

            public decimal AvgPowerFactor { get; set; }
        }


        private async Task<CountStatusDto> GetLinesSummary(
            List<int> factoryIds,
            int? zoneId)
        {
            DateTime onlineThreshold = DateTime.Now.AddHours(-3);

            var lines = await _emsContext.Lines
                .Where(x => x.FactoryId.HasValue && factoryIds.Contains(x.FactoryId.Value))
                .Select(x => new
                {
                    x.Id,
                    x.FactoryId,
                    x.ZoneId
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
        private async Task<CountStatusDto> GetZonesSummary(
            List<int> factoryIds,
            int? zoneId)
        {
            DateTime onlineThreshold = DateTime.Now.AddHours(-3);

            var zones = await _emsContext.Zones
                .Where(x => factoryIds.Contains(x.FactoryId))
                .Select(x => new
                {
                    x.Id,
                    x.FactoryId
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

        private string GetLevel(int? transformerId, int? zoneId)
        {
            if (!transformerId.HasValue && !zoneId.HasValue)
                return "Factory";

            if (transformerId.HasValue && !zoneId.HasValue)
                return "Transformer";

            if (zoneId.HasValue)
                return "Zone";

            return "Main";
        }


        #endregion


        #region Helper method for Heatmap status
        private async Task<List<EnergyHeatmapDto>> GetTransformerHeatmap(
    int factoryId,
    DateTime startTime,
    DateTime endTime,
    bool isCurrentShift)
        {
            var threshold = await GetThreshold("Transformer");

            var query = isCurrentShift
                ? _emsContext.VW_TransformerAnalysis
                    .Where(x => x.FactoryId == factoryId)
                    .Select(x => new
                    {
                        x.TransformerId,
                        x.TransformerName,
                        Date = x.ShiftStartTime.Date,
                        Energy = x.TotalEnergyConsumption
                    })
                : _emsContext.TransformerAnalysis
                    .Where(x =>
                        x.FactoryId == factoryId &&
                        x.ShiftStartTime >= startTime &&
                        x.ShiftStartTime <= endTime)
                    .Select(x => new
                    {
                        x.TransformerId,
                        x.TransformerName,
                        Date = x.ShiftStartTime.Date,
                        Energy = x.TotalEnergyConsumption
                    });

            var data = await query
                .GroupBy(x => new
                {
                    x.Date,
                    x.TransformerId,
                    x.TransformerName
                })
                .Select(g => new
                {
                    g.Key.Date,
                    ItemId = g.Key.TransformerId,
                    ItemName = g.Key.TransformerName,
                    Energy = g.Sum(x => x.Energy)
                })
                .ToListAsync();

            return data
                .Select(x => new EnergyHeatmapDto
                {
                    Date = x.Date,
                    ItemId = x.ItemId,
                    ItemName = x.ItemName,
                    Level = "Transformer",
                    Energy = x.Energy,
                    Status = GetEnergyStatus(x.Energy, threshold)
                })
                .OrderBy(x => x.Date)
                .ThenBy(x => x.ItemName)
                .ToList();
        }


        private async Task<List<EnergyHeatmapDto>> GetZoneHeatmap(
    int factoryId,
    int transformerId,
    DateTime startTime,
    DateTime endTime,
    bool isCurrentShift)
        {
            var threshold = await GetThreshold("Zone");

            var zonesRaw = await _emsContext.Zones
                .Where(x =>
                    x.FactoryId == factoryId &&
                    x.TransformerId == transformerId)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.RatioFromParent
                })
                .ToListAsync();

            var zones = zonesRaw
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    Ratio = NormalizeRatio(x.RatioFromParent ?? 0)
                })
                .ToList();

            var transformerDailyEnergy = isCurrentShift
                ? await _emsContext.VW_TransformerAnalysis
                    .Where(x =>
                        x.FactoryId == factoryId &&
                        x.TransformerId == transformerId)
                    .GroupBy(x => x.ShiftStartTime.Date)
                    .Select(g => new
                    {
                        Date = g.Key,
                        Energy = g.Sum(x => x.TotalEnergyConsumption)
                    })
                    .ToListAsync()
                : await _emsContext.TransformerAnalysis
                    .Where(x =>
                        x.FactoryId == factoryId &&
                        x.TransformerId == transformerId &&
                        x.ShiftStartTime >= startTime &&
                        x.ShiftStartTime <= endTime)
                    .GroupBy(x => x.ShiftStartTime.Date)
                    .Select(g => new
                    {
                        Date = g.Key,
                        Energy = g.Sum(x => x.TotalEnergyConsumption)
                    })
                    .ToListAsync();

            var result = new List<EnergyHeatmapDto>();

            foreach (var day in transformerDailyEnergy)
            {
                foreach (var zone in zones)
                {
                    decimal zoneEnergy = day.Energy * zone.Ratio;

                    result.Add(new EnergyHeatmapDto
                    {
                        Date = day.Date,
                        ItemId = zone.Id,
                        ItemName = zone.Name,
                        Level = "Zone",
                        Energy = zoneEnergy,
                        Status = GetEnergyStatus(zoneEnergy, threshold)
                    });
                }
            }

            return result
                .OrderBy(x => x.Date)
                .ThenBy(x => x.ItemName)
                .ToList();
        }


        private async Task<List<EnergyHeatmapDto>> GetLineHeatmap(
    int factoryId,
    int transformerId,
    int zoneId,
    DateTime startTime,
    DateTime endTime,
    bool isCurrentShift)
        {
            var threshold = await GetThreshold("Line");

            var zoneRaw = await _emsContext.Zones
                .Where(x =>
                    x.Id == zoneId &&
                    x.FactoryId == factoryId &&
                    x.TransformerId == transformerId)
                .Select(x => new
                {
                    x.RatioFromParent
                })
                .FirstOrDefaultAsync();

            if (zoneRaw == null)
                throw new Exception("Zone does not belong to this transformer or factory.");

            decimal zoneRatio = NormalizeRatio(zoneRaw.RatioFromParent ?? 0);

            var linesRaw = await _emsContext.Lines
                .Where(x =>
                    x.FactoryId == factoryId &&
                    x.ZoneId == zoneId &&
                    x.LineTransformers.Any(lt => lt.TransformerId == transformerId))
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.RatioFromParent
                })
                .ToListAsync();

            var lines = linesRaw
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    Ratio = NormalizeRatio(x.RatioFromParent ?? 0)
                })
                .ToList();

            var transformerDailyEnergy = isCurrentShift
                ? await _emsContext.VW_TransformerAnalysis
                    .Where(x =>
                        x.FactoryId == factoryId &&
                        x.TransformerId == transformerId)
                    .GroupBy(x => x.ShiftStartTime.Date)
                    .Select(g => new
                    {
                        Date = g.Key,
                        Energy = g.Sum(x => x.TotalEnergyConsumption)
                    })
                    .ToListAsync()
                : await _emsContext.TransformerAnalysis
                    .Where(x =>
                        x.FactoryId == factoryId &&
                        x.TransformerId == transformerId &&
                        x.ShiftStartTime >= startTime &&
                        x.ShiftStartTime <= endTime)
                    .GroupBy(x => x.ShiftStartTime.Date)
                    .Select(g => new
                    {
                        Date = g.Key,
                        Energy = g.Sum(x => x.TotalEnergyConsumption)
                    })
                    .ToListAsync();

            var result = new List<EnergyHeatmapDto>();

            foreach (var day in transformerDailyEnergy)
            {
                decimal zoneDailyEnergy = day.Energy * zoneRatio;

                foreach (var line in lines)
                {
                    decimal lineEnergy = zoneDailyEnergy * line.Ratio;

                    result.Add(new EnergyHeatmapDto
                    {
                        Date = day.Date,
                        ItemId = line.Id,
                        ItemName = line.Name,
                        Level = "Line",
                        Energy = lineEnergy,
                        Status = GetEnergyStatus(lineEnergy, threshold)
                    });
                }
            }

            return result
                .OrderBy(x => x.Date)
                .ThenBy(x => x.ItemName)
                .ToList();
        }

        private async Task<EnergyHeatmapThreshold> GetThreshold(string level)
        {
            var threshold = await _emsContext.EnergyHeatmapThresholds
                .Where(x => x.Level == level && x.IsActive)
                .FirstOrDefaultAsync();

            if (threshold == null)
                throw new Exception($"Energy heatmap threshold for level '{level}' is not configured.");

            return threshold;
        }

        private static string GetEnergyStatus(
    decimal energy,
    EnergyHeatmapThreshold threshold)
        {
            if (energy >= threshold.LowFrom && energy <= threshold.LowTo)
                return "Low";

            if (energy >= threshold.MediumFrom && energy <= threshold.MediumTo)
                return "Medium";

            if (energy >= threshold.HighFrom && energy <= threshold.HighTo)
                return "High";

            return "Unknown";
        }


        #endregion

    }
}