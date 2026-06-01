using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models.Definitions;
using Domain.Models.Definitions;

namespace DAL.Repositories.Interface.Definitions
{
    public interface ITransformerRepository
    {
        Task<IEnumerable<Transformer>> GetAllAsync();

        Task<Transformer?> GetByIdAsync(int id);

        Task AddAsync(Transformer transformer);

        void Update(Transformer transformer);

        void Delete(Transformer transformer);
    }
}
