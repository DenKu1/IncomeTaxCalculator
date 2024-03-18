using IncomeTaxCalculator.Persistence.Entities;
using IncomeTaxCalculator.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IncomeTaxCalculator.Persistence.Repositories
{
    public class TaxBandRepository : ITaxBandRepository
    {
        private readonly DbContext _context;

        public TaxBandRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaxBand>> GetAllAsync()
        {
            return await _context.Set<TaxBand>().ToListAsync();
        }

        public async Task<TaxBand> GetByIdAsync(Guid id)
        {
            return await _context.Set<TaxBand>().FindAsync(id);
        }

        public async Task AddAsync(TaxBand taxBand)
        {
            await _context.Set<TaxBand>().AddAsync(taxBand);
        }

        public void Remove(TaxBand taxBand)
        {
            _context.Set<TaxBand>().Remove(taxBand);
        }
    }
}
