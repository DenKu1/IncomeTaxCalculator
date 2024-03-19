namespace IncomeTaxCalculator.Persistence.Repositories.Interfaces
{
    public interface IDbUnitOfWork
    {
        public Task SaveChangesAsync();
    }
}
