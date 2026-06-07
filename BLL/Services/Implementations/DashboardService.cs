using BLL.Dtos;
using BLL.Services.Abstractions;
using DAL.Repositories.Interface;
using Shared.Dtos;

namespace BLL.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboradRepository _dashboradRepository;

        public DashboardService(IDashboradRepository dashboradRepository)
        {
            _dashboradRepository = dashboradRepository;
        }
        public async Task<List<TransformerEnergyDto>> GetTotalEnergyPerTransformer(int factoryId, int duration, DateTime? from, DateTime? to)
        {
            if (duration == 5)
            {
                // TODO: implement duration 5 case
                throw new NotImplementedException("Duration 5 not implemented yet.");
            }

            var durationDto = TimeUtilities.GetDurationStartTime(duration, from, to);
            bool isCurrentShift = duration == 0;

            return await _dashboradRepository.GetTotalEnergyPerTransformer(factoryId, durationDto.fromTime, durationDto.toTime, isCurrentShift);
        }

        public async Task<List<TransformerHourlyEnergyDto>> GetHourlyEnergyPerTransformer(int factoryId, int duration, DateTime? from, DateTime? to)
        {
            if (duration == 5)
            {
                throw new NotImplementedException("Duration 5 not implemented yet.");
            }

            var durationDto = TimeUtilities.GetDurationStartTime(duration, from, to);
            bool isCurrentShift = duration == 0;

            return await _dashboradRepository.GetHourlyEnergyPerTransformer(factoryId, durationDto.fromTime, durationDto.toTime, isCurrentShift);
        }
        public async Task<List<TopEnergyConsumerDto>> GetTopEnergyConsumers(int factoryId, int duration, DateTime? from, DateTime? to, int top)
        {
            if (duration == 5)
                throw new NotImplementedException("Duration 5 not implemented yet.");

            var durationDto = TimeUtilities.GetDurationStartTime(duration, from, to);
            bool isCurrentShift = duration == 0;

            return await _dashboradRepository.GetTopEnergyConsumers(factoryId, durationDto.fromTime, durationDto.toTime, isCurrentShift, top);
        }

        public async Task<List<VoltageStabilityDto>> GetVoltageStability(int factoryId, int duration, DateTime? from, DateTime? to)
        {
            if (duration == 5)
                throw new NotImplementedException("Duration 5 not implemented yet.");

            var durationDto = TimeUtilities.GetDurationStartTime(duration, from, to);
            bool isCurrentShift = duration == 0;

            return await _dashboradRepository.GetVoltageStability(factoryId, durationDto.fromTime, durationDto.toTime, isCurrentShift);
        }

        public async Task<List<CurrentFluctuationDto>> GetCurrentFluctuation(int factoryId, int duration, DateTime? from, DateTime? to)
        {
            if (duration == 5)
                throw new NotImplementedException("Duration 5 not implemented yet.");

            var durationDto = TimeUtilities.GetDurationStartTime(duration, from, to);
            bool isCurrentShift = duration == 0;

            return await _dashboradRepository.GetCurrentFluctuation(factoryId, durationDto.fromTime, durationDto.toTime, isCurrentShift);
        }

        public async Task<List<HarmonicsLevelDto>> GetHarmonicsLevel(int factoryId, int duration, DateTime? from, DateTime? to)
        {
            if (duration == 5)
                throw new NotImplementedException("Duration 5 not implemented yet.");

            var durationDto = TimeUtilities.GetDurationStartTime(duration, from, to);
            bool isCurrentShift = duration == 0;

            return await _dashboradRepository.GetHarmonicsLevel(factoryId, durationDto.fromTime, durationDto.toTime, isCurrentShift);
        }
        //public async Task<TransformerSummaryDto> GetTransformerSummary(int factoryId, int duration, DateTime? from, DateTime? to)
        //{
        //    if (duration == 5)
        //        throw new NotImplementedException("Duration 5 not implemented yet.");

        //    var durationDto = TimeUtilities.GetDurationStartTime(duration, from, to);
        //    bool isCurrentShift = duration == 0;

        //    return await _dashboradRepository.GetTransformerSummary(factoryId, durationDto.fromTime, durationDto.toTime, isCurrentShift);
        //}
        //public async Task<TransformerSummaryDto> GetTransformerSummary(int? factoryId, int duration, DateTime? from, DateTime? to)
        //{
        //    if (duration == 5)
        //        throw new NotImplementedException("Duration 5 not implemented yet.");

        //    var durationDto = TimeUtilities.GetDurationStartTime(duration, from, to);
        //    bool isCurrentShift = duration == 0;
        //    string level = factoryId.HasValue ? "Factory" : "Main";

        //    var result = await _dashboradRepository.GetTransformerSummary(factoryId, durationDto.fromTime, durationDto.toTime, isCurrentShift);
        //    result.Level = level;
        //    return result;
        //}

    }
}
