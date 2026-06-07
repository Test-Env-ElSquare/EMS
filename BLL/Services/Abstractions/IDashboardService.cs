using BLL.Dtos;
using Shared.Dtos;

namespace BLL.Services.Abstractions
{
    public interface IDashboardService
    {
        public Task<List<TransformerEnergyDto>> GetTotalEnergyPerTransformer(int factoryId, int duration, DateTime? from, DateTime? to);
        public Task<List<TransformerHourlyEnergyDto>> GetHourlyEnergyPerTransformer(int factoryId, int duration, DateTime? from, DateTime? to);
        public Task<List<VoltageStabilityDto>> GetVoltageStability(int factoryId, int duration, DateTime? from, DateTime? to);
        public Task<List<CurrentFluctuationDto>> GetCurrentFluctuation(int factoryId, int duration, DateTime? from, DateTime? to);
        public Task<List<HarmonicsLevelDto>> GetHarmonicsLevel(int factoryId, int duration, DateTime? from, DateTime? to);
        //public Task<TransformerSummaryDto> GetTransformerSummary(int? factoryId, int duration, DateTime? from, DateTime? to);
        public Task<List<TopEnergyConsumerDto>> GetTopEnergyConsumers(int factoryId, int duration, DateTime? from, DateTime? to, int top);





    }
}
