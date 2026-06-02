using DAL.Context;
using DAL.Repositories.Implementation.Definitions;
using DAL.Repositories.Interface;
using DAL.Repositories.Interface.Definitions;

namespace DAL.Repositories.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EmsContext _context;

        public IFactoryRepository FactoryRepository { get; }
        public ILineRepository LineRepository { get; }
        public ILineTransformerRepository LineTransformerRepository { get; }
        public ITransformerRepository TransformerRepository { get; }

        public UnitOfWork(EmsContext context)
        {
            _context = context;
            FactoryRepository = new FactoryRepository(_context);
            LineRepository = new LineRepository(_context);
            LineTransformerRepository = new LineTransformerRepository(_context);
            TransformerRepository = new TransformerRepository(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}