using BLL.Dtos;
using Shared.Dtos;
using Shared.Dtos.EnergyDashboard;
using Shared.Dtos.Heatmap;

namespace BLL.Services.Abstractions
{
    public interface IEnergyDashboardService
    {
        //Task<DashboardSummaryDto> GetSummary(
        //            int? factoryId,
        //            int duration,
        //            DateTime? from,
        //            DateTime? to);
        Task<TransformerSummaryDto> GetSummary(int? factoryId, int duration, DateTime? from, DateTime? to);
        Task<OnlineStatusDto> GetOnlineStatus(int? factoryId, int duration, DateTime? from, DateTime? to);
        Task<List<VoltageGraphDto>> GetVoltageGraph(
            int factoryId,
            int duration,
            DateTime? from,
            DateTime? to
        );

        Task<List<CurrentGraphDto>> GetCurrentGraph(
            int factoryId,
            int duration,
            DateTime? from,
            DateTime? to
        );

        Task<List<HarmonicsGraphDto>> GetHarmonicsGraph(
            int factoryId,
            int duration,
            DateTime? from,
            DateTime? to
        );




        Task<List<EnergyHeatmapDto>> GetEnergyHeatmap(
    int factoryId,
    int? transformerId,
    int? zoneId,
    int duration,
    DateTime? from,
    DateTime? to);



        Task<List<TransformerEnergyDto>> GetTotalEnergyPerTransformer(
    int factoryId,
    int? transformerId,
    int? zoneId,
    int duration,
    DateTime? from,
    DateTime? to);

        Task<List<TransformerHourlyEnergyDto>> GetHourlyEnergyPerTransformer(
            int factoryId,
            int? transformerId,
            int? zoneId,
            int duration,
            DateTime? from,
            DateTime? to);

        Task<List<TopEnergyConsumerDto>> GetTopEnergyConsumers(
            int factoryId,
            int? transformerId,
            int? zoneId,
            int duration,
            DateTime? from,
            DateTime? to,
            int top);
    }
}