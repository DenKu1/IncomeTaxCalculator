using IncomeTaxCalculator.Persistence.EF;
using IncomeTaxCalculator.Persistence.Entities;
using IncomeTaxCalculator.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IncomeTaxCalculator.Persistence.Repositories
{
    public class TaxBandRepository : ITaxBandRepository
    {
        private readonly IncomeTaxDbContext _context;

        public TaxBandRepository(IncomeTaxDbContext context)
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

        public async Task<TaxBand> GetSingleOrDefaultAsync(Expression<Func<TaxBand, bool>> predicate)
        {
            return await _context.Set<TaxBand>().SingleOrDefaultAsync(predicate);
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
