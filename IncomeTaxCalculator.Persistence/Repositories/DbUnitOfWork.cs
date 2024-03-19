using IncomeTaxCalculator.Persistence.EF;
using IncomeTaxCalculator.Persistence.Repositories.Interfaces;

namespace IncomeTaxCalculator.Persistence.Repositories
{
    public class DbUnitOfWork : IDbUnitOfWork
    {
        private readonly IncomeTaxDbContext _context;

        public DbUnitOfWork(IncomeTaxDbContext context)
        {
            _context = context;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
