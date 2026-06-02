using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.Definitions;

namespace DAL.Repositories.Interface.Definitions
{
    public interface ILineRepository
    {
        Task<IEnumerable<Line>> GetAllAsync();

        Task<Line?> GetByIdAsync(int id);

        Task AddAsync(Line line);

        void Update(Line line);

        void Delete(Line line);
    }
}
