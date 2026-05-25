using BLL.Dtos;
using BLL.Helpers;
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
    }
}
