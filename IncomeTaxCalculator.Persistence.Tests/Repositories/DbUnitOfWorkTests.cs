using IncomeTaxCalculator.Persistence.EF;
using IncomeTaxCalculator.Persistence.Entities;
using IncomeTaxCalculator.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IncomeTaxCalculator.Persistence.Tests.Repositories
{
    public class DbUnitOfWorkTests
    {
        private readonly Fixture _fixture;
        private readonly IncomeTaxDbContext _context;

        public DbUnitOfWorkTests()
        {
            _fixture = new Fixture();

            var options = new DbContextOptionsBuilder<IncomeTaxDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new IncomeTaxDbContext(options);
        }

        [Fact]
        public async Task SaveChangesAsync_Should_Save_Data_To_Context()
        {
            // Arrange
            var sut = CreateSut();

            var taxBandToAdd = _fixture
                .Build<TaxBand>()
                .With(x => x.Id, Guid.NewGuid())
                .Create();

            _context.TaxBands.Add(taxBandToAdd);

            // Act
            await sut.SaveChangesAsync();

            // Assert
            var savedRecord = await _context.TaxBands.FirstOrDefaultAsync(x => x.Id == taxBandToAdd.Id);
            savedRecord.Should().NotBeNull();
        }

        private DbUnitOfWork CreateSut()
        {
            return new DbUnitOfWork(_context);
        }
    }
}
