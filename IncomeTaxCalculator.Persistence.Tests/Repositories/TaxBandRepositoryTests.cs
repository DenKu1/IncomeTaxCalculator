using IncomeTaxCalculator.Persistence.EF;
using IncomeTaxCalculator.Persistence.Entities;
using IncomeTaxCalculator.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IncomeTaxCalculator.Persistence.Tests.Repositories
{
    public class TaxBandRepositoryTests
    {
        private readonly Fixture _fixture;
        private readonly IncomeTaxDbContext _context;

        public TaxBandRepositoryTests()
        {
            _fixture = new Fixture();

            var options = new DbContextOptionsBuilder<IncomeTaxDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new IncomeTaxDbContext(options);
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_All_TaxBands()
        {
            // Arrange
            var sut = CreateSut();

            var taxBands = _fixture
                .CreateMany<TaxBand>(2);

            _context.TaxBands.AddRange(taxBands);
            _context.SaveChanges();

            // Act
            var result = await sut.GetAllAsync();

            // Assert
            result.Should().BeEquivalentTo(taxBands);
        }

        [Fact]
        public async Task GetSingleOrDefaultAsync_Should_Find_Correct_TaxBand()
        {
            // Arrange
            var sut = CreateSut();

            var idToFind = Guid.NewGuid();

            var taxBands = _fixture
                .CreateMany<TaxBand>(2);

            var taxBandToFind = _fixture
                .Build<TaxBand>()
                .With(x => x.Id, idToFind)
                .Create();

            _context.TaxBands.AddRange(taxBands);
            _context.TaxBands.Add(taxBandToFind);
            _context.SaveChanges();

            // Act
            var result = await sut.GetSingleOrDefaultAsync(x => x.Id == idToFind);

            // Assert
            result.Should().BeEquivalentTo(taxBandToFind);
        }

        [Fact]
        public async Task AddAsync_Should_Add_TaxBand()
        {
            // Arrange
            var sut = CreateSut();

            var idToAdd = Guid.NewGuid();

            var taxBands = _fixture
                .CreateMany<TaxBand>(2);

            var taxBandToAdd = _fixture
                .Build<TaxBand>()
                .With(x => x.Id, idToAdd)
                .Create();

            _context.TaxBands.AddRange(taxBands);
            _context.SaveChanges();

            // Act
            await sut.AddAsync(taxBandToAdd);
            _context.SaveChanges();

            // Assert
            _context.TaxBands.Should().Contain(taxBandToAdd);
        }

        [Fact]
        public void Remove_Should_Remove_TaxBand()
        {
            // Arrange
            var sut = CreateSut();

            var idToRemove = Guid.NewGuid();

            var taxBands = _fixture
                .CreateMany<TaxBand>(2);

            var taxBandToRemove = _fixture
                .Build<TaxBand>()
                .With(x => x.Id, idToRemove)
                .Create();

            _context.TaxBands.AddRange(taxBands);
            _context.TaxBands.Add(taxBandToRemove);

            _context.SaveChanges();

            // Act
            sut.Remove(taxBandToRemove);
            _context.SaveChanges();

            // Assert
            _context.TaxBands.Should().NotContain(taxBandToRemove);
        }

        private TaxBandRepository CreateSut()
        {
            return new TaxBandRepository(_context);
        }
    }
}
