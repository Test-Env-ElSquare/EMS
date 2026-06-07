

using BLL.Dtos;
using Shared.Dtos;
using Shared.Dtos.EnergyDashboard;
using Shared.Dtos.Heatmap;

namespace DAL.Repositories.Interface
{
    public interface IEnergyDashboardRepository
    {
        //Task<DashboardSummaryDto> GetSummary(
        //    int? factoryId,
        //    DateTime startTime,
        //    DateTime endTime,
        //    bool isCurrentShift);
        Task<TransformerSummaryDto> GetSummary(int? factoryId, DateTime startTime, DateTime endTime, bool isCurrentShift);
        Task<OnlineStatusDto> GetOnlineStatus(int? factoryId, DateTime startTime, DateTime endTime, bool isCurrentShift);
        Task<List<VoltageGraphDto>> GetVoltageGraph(
            int? factoryId,
            DateTime startTime,
            DateTime endTime,
            bool isCurrentShift);

        Task<List<CurrentGraphDto>> GetCurrentGraph(
            int? factoryId,
            DateTime startTime,
            DateTime endTime,
            bool isCurrentShift);

        Task<List<HarmonicsGraphDto>> GetHarmonicsGraph(
            int? factoryId,
            DateTime startTime,
            DateTime endTime,
            bool isCurrentShift);




        Task<List<EnergyHeatmapDto>> GetEnergyHeatmap(
           int factoryId,
           int? transformerId,
           int? zoneId,
           DateTime startTime,
           DateTime endTime,
           bool isCurrentShift);


        Task<List<TransformerEnergyDto>> GetTotalEnergyPerTransformer(
    int factoryId,
    int? transformerId,
    int? zoneId,
    DateTime startTime,
    DateTime endTime,
    bool isCurrentShift
);

        Task<List<TransformerHourlyEnergyDto>> GetHourlyEnergyPerTransformer(
            int factoryId,
            int? transformerId,
            int? zoneId,
            DateTime startTime,
            DateTime endTime,
            bool isCurrentShift
        );

        Task<List<TopEnergyConsumerDto>> GetTopEnergyConsumers(
            int factoryId,
            int? transformerId,
            int? zoneId,
            DateTime startTime,
            DateTime endTime,
            bool isCurrentShift,
            int top
        );
    }
}