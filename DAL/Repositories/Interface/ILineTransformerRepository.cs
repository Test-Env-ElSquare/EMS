using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Models.Definitions;

namespace DAL.Repositories.Interface.Definitions
{
    public interface ILineTransformerRepository
    {
        Task<IEnumerable<LineTransformer>> GetAllAsync();

        Task<LineTransformer?> GetByIdAsync(int id);

        Task AddAsync(LineTransformer entity);

        void Update(LineTransformer entity);

        void Delete(LineTransformer entity);
    }
}
