using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutFitMaker.Domain.Interfaces.Base
{
    public interface IDatabaseTransaction : IDisposable
    {
        Task CommitAsync();
        Task RollbackAsync();
    }
}
