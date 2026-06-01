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
    public class LineTransformerRepository : ILineTransformerRepository
    {
        private readonly EmsContext _context;

        public LineTransformerRepository(EmsContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LineTransformer>> GetAllAsync()
        {
            return await _context.Set<LineTransformer>()
                .Include(x => x.Line)
                .Include(x => x.Transformer)
                .ToListAsync();
        }

        public async Task<LineTransformer?> GetByIdAsync(int id)
        {
            return await _context.Set<LineTransformer>()
                .Include(x => x.Line)
                .Include(x => x.Transformer)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(LineTransformer entity)
        {
            await _context.Set<LineTransformer>().AddAsync(entity);
        }

        public void Update(LineTransformer entity)
        {
            _context.Set<LineTransformer>().Update(entity);
        }

        public void Delete(LineTransformer entity)
        {
            _context.Set<LineTransformer>().Remove(entity);
        }
    }
}
