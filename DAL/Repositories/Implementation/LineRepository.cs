using DAL.Repositories.Interface.Definitions;
using Domain.Models.Definitions;
using Microsoft.EntityFrameworkCore;
using DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Implementation.Definitions
{
    public class LineRepository : ILineRepository
    {
        private readonly EmsContext _context;

        public LineRepository(EmsContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Line>> GetAllAsync()
        {
            return await _context.Set<Line>()
                .Include(x => x.Factory)
                .Include(x => x.LineTransformers)
                .ToListAsync();
        }

        public async Task<Line?> GetByIdAsync(int id)
        {
            return await _context.Set<Line>()
                .Include(x => x.Factory)
                .Include(x => x.LineTransformers)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(Line line)
        {
            await _context.Set<Line>().AddAsync(line);
        }

        public void Update(Line line)
        {
            _context.Set<Line>().Update(line);
        }

        public void Delete(Line line)
        {
            _context.Set<Line>().Remove(line);
        }
    }
}