using BLL.Dtos;
using Shared.Dtos;

namespace BLL.Services.Abstractions
{
    public interface IDashboardService
    {
        public Task<List<TransformerEnergyDto>> GetTotalEnergyPerTransformer(int factoryId, int duration, DateTime? from, DateTime? to);
        public Task<List<TransformerHourlyEnergyDto>> GetHourlyEnergyPerTransformer(int factoryId, int duration, DateTime? from, DateTime? to);

    }
}
