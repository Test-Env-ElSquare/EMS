using DAL.Context;
using DAL.Repositories.Interface.Definitions;
using Domain.Models.Definitions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Implementation.Definitions
{
    public class FactoryRepository : IFactoryRepository
    {
        private readonly EmsContext _context;

        public FactoryRepository(EmsContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Factory>> GetAllAsync()
        {
            return await _context.Factories.ToListAsync();
        }

        public async Task<Factory?> GetByIdAsync(int id)
        {
            return await _context.Factories.FindAsync(id);
        }

        public async Task AddAsync(Factory factory)
        {
            await _context.Factories.AddAsync(factory);
        }

        public void Update(Factory factory)
        {
            _context.Factories.Update(factory);
        }

        public void Delete(Factory factory)
        {
            _context.Factories.Remove(factory);
        }
    }
}
