using BLL.Dtos;
using BLL.Services.Abstractions;
using DAL.Repositories.Interface;
using Shared.Dtos;
using Shared.Dtos.EnergyDashboard;
using Shared.Dtos.Heatmap;

namespace BLL.Services.Implementations
{
    public class EnergyDashboardService : IEnergyDashboardService
    {
        private readonly IEnergyDashboardRepository _energyDashboardRepo;

        public EnergyDashboardService(IEnergyDashboardRepository energyDashboardRepo)
        {
            _energyDashboardRepo = energyDashboardRepo;
        }


        public async Task<TransformerSummaryDto> GetSummary(int? factoryId, int duration, DateTime? from, DateTime? to)
        {
            if (duration == 5)
                throw new NotImplementedException("Duration 5 not implemented yet.");
            var durationDto = TimeUtilities.GetDurationStartTime(duration, from, to);
            bool isCurrentShift = duration == 0;
            string level = factoryId.HasValue ? "Factory" : "Main";
            var result = await _energyDashboardRepo.GetSummary(factoryId, durationDto.fromTime, durationDto.toTime, isCurrentShift);
            result.Level = level;
            return result;
        }

        public async Task<OnlineStatusDto> GetOnlineStatus(int? factoryId, int duration, DateTime? from, DateTime? to)
        {
            if (duration == 5)
                throw new NotImplementedException("Duration 5 not implemented yet.");
            var durationDto = TimeUtilities.GetDurationStartTime(duration, from, to);
            bool isCurrentShift = duration == 0;
            string level = factoryId.HasValue ? "Factory" : "Main";
            var result = await _energyDashboardRepo.GetOnlineStatus(factoryId, durationDto.fromTime, durationDto.toTime, isCurrentShift);
            result.Level = level;
            return result;
        }


        private void ValidateFilters(int? factoryId, int? transformerId, int? zoneId)
        {
            if (factoryId.HasValue && factoryId.Value < 0)
                throw new Exception("Invalid factory id.");

            if (transformerId.HasValue && transformerId.Value <= 0)
                throw new Exception("Invalid transformer id.");

            if (zoneId.HasValue && zoneId.Value <= 0)
                throw new Exception("Invalid zone id.");

            if (zoneId.HasValue && !factoryId.HasValue)
                throw new Exception("FactoryId is required when filtering by Zone.");
        }
        public async Task<List<VoltageGraphDto>> GetVoltageGraph(
            int factoryId,
            int duration,
            DateTime? from,
            DateTime? to)
        {
            if (duration == 5)
                throw new NotImplementedException("Custom duration is not implemented yet.");

            ValidateFilters(factoryId, null, null);

            var durationDto = TimeUtilities.GetDurationStartTime(duration, from, to);
            bool isCurrentShift = duration == 0;

            return await _energyDashboardRepo.GetVoltageGraph(
                factoryId,
                durationDto.fromTime,
                durationDto.toTime,
                isCurrentShift
            );
        }

        public async Task<List<CurrentGraphDto>> GetCurrentGraph(
            int factoryId,
            int duration,
            DateTime? from,
            DateTime? to)
        {
            if (duration == 5)
                throw new NotImplementedException("Custom duration is not implemented yet.");

            ValidateFilters(factoryId, null, null);

            var durationDto = TimeUtilities.GetDurationStartTime(duration, from, to);
            bool isCurrentShift = duration == 0;

            return await _energyDashboardRepo.GetCurrentGraph(
                factoryId,
                durationDto.fromTime,
                durationDto.toTime,
                isCurrentShift
            );
        }

        public async Task<List<HarmonicsGraphDto>> GetHarmonicsGraph(
            int factoryId,
            int duration,
            DateTime? from,
            DateTime? to)
        {
            if (duration == 5)
                throw new NotImplementedException("Custom duration is not implemented yet.");

            ValidateFilters(factoryId, null, null);

            var durationDto = TimeUtilities.GetDurationStartTime(duration, from, to);
            bool isCurrentShift = duration == 0;

            return await _energyDashboardRepo.GetHarmonicsGraph(
                factoryId,
                durationDto.fromTime,
                durationDto.toTime,
                isCurrentShift
            );
        }





        //Heatmap
        public async Task<List<EnergyHeatmapDto>> GetEnergyHeatmap(
    int factoryId,
    int? transformerId,
    int? zoneId,
    int duration,
    DateTime? from,
    DateTime? to)
        {
            if (duration == 5 && (!from.HasValue || !to.HasValue))
                throw new ArgumentException("From and To are required when duration is custom.");

            ValidateFilters(factoryId, transformerId, zoneId);

            var durationDto = TimeUtilities.GetDurationStartTime(duration, from, to);

            bool isCurrentShift = duration == 0;

            return await _energyDashboardRepo.GetEnergyHeatmap(
                factoryId,
                transformerId,
                zoneId,
                durationDto.fromTime,
                durationDto.toTime,
                isCurrentShift
            );
        }


        public async Task<List<TransformerEnergyDto>> GetTotalEnergyPerTransformer(
    int factoryId,
    int? transformerId,
    int? zoneId,
    int duration,
    DateTime? from,
    DateTime? to)
        {
            if (duration == 5)
                throw new NotImplementedException("Duration 5 not implemented yet.");

            ValidateFilters(factoryId, transformerId, zoneId);

            var durationDto = TimeUtilities.GetDurationStartTime(duration, from, to);
            bool isCurrentShift = duration == 0;

            return await _energyDashboardRepo.GetTotalEnergyPerTransformer(
                factoryId,
                transformerId,
                zoneId,
                durationDto.fromTime,
                durationDto.toTime,
                isCurrentShift
            );
        }

        public async Task<List<TransformerHourlyEnergyDto>> GetHourlyEnergyPerTransformer(
            int factoryId,
            int? transformerId,
            int? zoneId,
            int duration,
            DateTime? from,
            DateTime? to)
        {
            if (duration == 5)
                throw new NotImplementedException("Duration 5 not implemented yet.");

            ValidateFilters(factoryId, transformerId, zoneId);

            var durationDto = TimeUtilities.GetDurationStartTime(duration, from, to);
            bool isCurrentShift = duration == 0;

            return await _energyDashboardRepo.GetHourlyEnergyPerTransformer(
                factoryId,
                transformerId,
                zoneId,
                durationDto.fromTime,
                durationDto.toTime,
                isCurrentShift
            );
        }

        public async Task<List<TopEnergyConsumerDto>> GetTopEnergyConsumers(
            int factoryId,
            int? transformerId,
            int? zoneId,
            int duration,
            DateTime? from,
            DateTime? to,
            int top)
        {
            if (duration == 5)
                throw new NotImplementedException("Duration 5 not implemented yet.");

            ValidateFilters(factoryId, transformerId, zoneId);

            var durationDto = TimeUtilities.GetDurationStartTime(duration, from, to);
            bool isCurrentShift = duration == 0;

            return await _energyDashboardRepo.GetTopEnergyConsumers(
                factoryId,
                transformerId,
                zoneId,
                durationDto.fromTime,
                durationDto.toTime,
                isCurrentShift,
                top
            );
        }


        private static void ValidateFilters(int factoryId, int? transformerId, int? zoneId)
        {
            if (factoryId <= 0)
                throw new ArgumentException("FactoryId is required.");

            if (zoneId.HasValue && !transformerId.HasValue)
                throw new ArgumentException("TransformerId is required when ZoneId is provided.");
        }
    }
}