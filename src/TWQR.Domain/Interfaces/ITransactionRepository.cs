using System;
using System.Threading.Tasks;
using TWQR.Domain.Entities;

namespace TWQR.Domain.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction?> GetByIdAsync(Guid id);
        Task<Transaction?> GetByIdempotencyKeyAsync(string idempotencyKey);
        Task AddAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction);
    }
}
