using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using OutFitMaker.DataAccess.DbContext;
using OutFitMaker.Domain.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutFitMaker.DataAccess.Repositories.Base
{
    public class EntityDatabaseTransaction(OutFitMakerDbContext context) : IDatabaseTransaction
    {
        private readonly IDbContextTransaction _transaction = context.Database.BeginTransactionAsync().Result;
        public async Task CommitAsync() =>
      await _transaction.CommitAsync();

        public async Task RollbackAsync() =>
            await _transaction.RollbackAsync();

        public async Task DisposeAsync() =>
            await _transaction.DisposeAsync();

        public void Dispose() =>
            _transaction.Dispose();
    }
}
