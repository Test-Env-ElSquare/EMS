using Domain.Models.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interface.Definitions
{
    public interface IFactoryRepository
    {
        Task<IEnumerable<Factory>> GetAllAsync();

        Task<Factory?> GetByIdAsync(int id);

        Task AddAsync(Factory factory);

        void Update(Factory factory);

        void Delete(Factory factory);
    }
}