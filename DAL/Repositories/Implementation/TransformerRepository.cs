using DAL.Context;
using DAL.Models.Definitions;
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
    public class TransformerRepository : ITransformerRepository
    {
        private readonly EmsContext _context;

        public TransformerRepository(EmsContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Transformer>> GetAllAsync()
        {
            return await _context.Set<Transformer>()
                .Include(x => x.LineTransformers)
                .Include(x => x.factory)
                .ToListAsync();
        }

        public async Task<Transformer?> GetByIdAsync(int id)
        {
            return await _context.Set<Transformer>()
                .Include(x => x.LineTransformers)
                .Include(x => x.factory)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(Transformer transformer)
        {
            await _context.Set<Transformer>().AddAsync(transformer);
        }

        public void Update(Transformer transformer)
        {
            _context.Set<Transformer>().Update(transformer);
        }

        public void Delete(Transformer transformer)
        {
            _context.Set<Transformer>().Remove(transformer);
        }
    }
}
