using BLL.Dtos;
using Shared.Dtos;

namespace DAL.Repositories.Interface
{
    public interface IDashboradRepository
    {
        public Task<List<TransformerEnergyDto>> GetTotalEnergyPerTransformer(int factoryId, DateTime startTime, DateTime endTime, bool isCurrentShift);
        public Task<List<TransformerHourlyEnergyDto>> GetHourlyEnergyPerTransformer(int factoryId, DateTime startTime, DateTime endTime, bool isCurrentShift);
        public Task<List<VoltageStabilityDto>> GetVoltageStability(int factoryId, DateTime startTime, DateTime endTime, bool isCurrentShift);
        public Task<List<CurrentFluctuationDto>> GetCurrentFluctuation(int factoryId, DateTime startTime, DateTime endTime, bool isCurrentShift);
        public Task<List<HarmonicsLevelDto>> GetHarmonicsLevel(int factoryId, DateTime startTime, DateTime endTime, bool isCurrentShift);
        public Task<TransformerSummaryDto> GetTransformerSummary(int factoryId, DateTime startTime, DateTime endTime, bool isCurrentShift);
        public Task<List<TopEnergyConsumerDto>> GetTopEnergyConsumers(int factoryId, DateTime startTime, DateTime endTime, bool isCurrentShift, int top);


    }
}
