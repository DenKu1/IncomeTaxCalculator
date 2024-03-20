using IncomeTaxCalculator.Persistence.Entities;
using System.Linq.Expressions;

namespace IncomeTaxCalculator.Persistence.Repositories.Interfaces
{
    public interface ITaxBandRepository
    {
        Task AddAsync(TaxBand taxBand);
        Task<IEnumerable<TaxBand>> GetAllAsync();
        Task<TaxBand> GetSingleOrDefaultAsync(Expression<Func<TaxBand, bool>> predicate);
        void Remove(TaxBand taxBand);
    }
}
