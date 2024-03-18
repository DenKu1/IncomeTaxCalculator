using IncomeTaxCalculator.Persistence.Entities;

namespace IncomeTaxCalculator.Persistence.Repositories.Interfaces
{
    public interface ITaxBandRepository
    {
        Task AddAsync(TaxBand taxBand);
        Task<IEnumerable<TaxBand>> GetAllAsync();
        Task<TaxBand> GetByIdAsync(Guid id);
        void Remove(TaxBand taxBand);
    }
}
