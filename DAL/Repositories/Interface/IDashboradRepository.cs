using BLL.Dtos;
using Shared.Dtos;

namespace DAL.Repositories.Interface
{
    public interface IDashboradRepository
    {
        public Task<List<TransformerEnergyDto>> GetTotalEnergyPerTransformer(int factoryId, DateTime startTime, DateTime endTime, bool isCurrentShift);
        public Task<List<TransformerHourlyEnergyDto>> GetHourlyEnergyPerTransformer(int factoryId, DateTime startTime, DateTime endTime, bool isCurrentShift);
    }
}
