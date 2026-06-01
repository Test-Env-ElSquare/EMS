using Domain.Models.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Repositories.Interface.Definitions;

namespace DAL.Repositories.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IFactoryRepository FactoryRepository { get; }
        ILineRepository LineRepository { get; }
        ILineTransformerRepository LineTransformerRepository { get; }
        ITransformerRepository TransformerRepository { get; }
        Task<int> CompleteAsync();
    }
}
