using Bogus;
using ELMS.WEB.Entities.Loan;
using ELMS.WEB.Enums.Loan;
using ELMS.WEB.Repositories.Loan.Interface;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ELMS.XUNIT.Loans
{
    public class LoanExtensionUnitTest
    {
        private readonly Mock<ILoanExtensionRepository> __LoanRepositoryMock = new Mock<ILoanExtensionRepository>();

        [Fact]
        public async Task CreateAsync_ShouldReturnLoanExtension_WhenLoanExtensionIsValid()
        {
            // Arrange
            Faker<LoanEntity> _LoanFaker = new Faker<LoanEntity>()
                .RuleFor(o => o.LoanerEmail, f => f.Person.Email)
                .RuleFor(o => o.LoaneeEmail, f => f.Person.Email)
                .RuleFor(o => o.FromTimestamp, f => f.Date.Between(DateTime.Now, DateTime.Now.AddDays(14)))
                .RuleFor(o => o.ExpiryTimestamp, f => f.Date.Between(DateTime.Now.AddDays(14), DateTime.Now.AddDays(56)))
                .RuleFor(o => o.Status, f => Status.Pending)
                .RuleFor(o => o.AcceptedTermsAndConditions, false);

            LoanEntity _LoanFake = _LoanFaker.Generate();

            Faker<LoanExtensionEntity> _LoanExtensionFaker = new Faker<LoanExtensionEntity>()
                .RuleFor(o => o.LoanUID, f => _LoanFake.UID)
                .RuleFor(o => o.ExtenderEmail, f => f.Person.Email)
                .RuleFor(o => o.PreviousExpiryDate, f => f.Date.Between(DateTime.Today.AddDays(1), DateTime.Today.AddMonths(1)))
                .RuleFor(o => o.NewExpiryDate, f => f.Date.Between(DateTime.Today.AddMonths(1), DateTime.Today.AddMonths(5)))
                .RuleFor(o => o.Loan, f => _LoanFake);

            LoanExtensionEntity _LoanExtensionFake = _LoanExtensionFaker.Generate();

            __LoanRepositoryMock.Setup(x => x.CreateAsync(_LoanExtensionFake)).ReturnsAsync(_LoanExtensionFake);

            // Act
            LoanExtensionEntity _LoanExtension = await __LoanRepositoryMock.Object.CreateAsync(_LoanExtensionFake);

            // Assert
            Assert.Equal(_LoanExtensionFake, _LoanExtension);
        }

        [Fact]
        public async Task CreateAsync_ShouldNotReturnLoanExtension_WhenLoanExtensionInvalid()
        {
            // Arrange
            LoanExtensionEntity _LoanExtensionFake = new LoanExtensionEntity { UID = Guid.Empty };
            __LoanRepositoryMock.Setup(x => x.CreateAsync(_LoanExtensionFake)).ReturnsAsync(() => null);
            __LoanRepositoryMock.Setup(x => x.CreateAsync(null)).ReturnsAsync(() => null);

            // Act
            LoanExtensionEntity _LoanExtension = await __LoanRepositoryMock.Object.CreateAsync(_LoanExtensionFake);
            LoanExtensionEntity _LoanExtensionNull = await __LoanRepositoryMock.Object.CreateAsync(null);

            // Assert 
            Assert.Null(_LoanExtension);
            Assert.Null(_LoanExtensionNull);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenLoanExtensionExists()
        {
            // Arrange
            Faker<LoanEntity> _LoanFaker = new Faker<LoanEntity>()
                .RuleFor(o => o.LoanerEmail, f => f.Person.Email)
                .RuleFor(o => o.LoaneeEmail, f => f.Person.Email)
                .RuleFor(o => o.FromTimestamp, f => f.Date.Between(DateTime.Now, DateTime.Now.AddDays(14)))
                .RuleFor(o => o.ExpiryTimestamp, f => f.Date.Between(DateTime.Now.AddDays(14), DateTime.Now.AddDays(56)))
                .RuleFor(o => o.Status, f => Status.Pending)
                .RuleFor(o => o.AcceptedTermsAndConditions, false);

            LoanEntity _LoanFake = _LoanFaker.Generate();

            Faker<LoanExtensionEntity> _LoanExtensionFaker = new Faker<LoanExtensionEntity>()
                .RuleFor(o => o.LoanUID, f => _LoanFake.UID)
                .RuleFor(o => o.ExtenderEmail, f => f.Person.Email)
                .RuleFor(o => o.PreviousExpiryDate, f => f.Date.Between(DateTime.Today.AddDays(1), DateTime.Today.AddMonths(1)))
                .RuleFor(o => o.NewExpiryDate, f => f.Date.Between(DateTime.Today.AddMonths(1), DateTime.Today.AddMonths(5)))
                .RuleFor(o => o.Loan, f => _LoanFake);

            LoanExtensionEntity _LoanExtensionFake = _LoanExtensionFaker.Generate();

            __LoanRepositoryMock.Setup(x => x.DeleteAsync(_LoanExtensionFake.UID)).ReturnsAsync(true);

            // Act
            bool _Result = await __LoanRepositoryMock.Object.DeleteAsync(_LoanExtensionFake.UID);

            // Assert
            Assert.True(_Result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenLoanExtensionDoesNotExist()
        {
            // Arrange
            __LoanRepositoryMock.Setup(x => x.DeleteAsync(Guid.Empty)).ReturnsAsync(false);

            // Act
            bool _Result = await __LoanRepositoryMock.Object.DeleteAsync(Guid.Empty);

            // Assert
            Assert.False(_Result);
        }

        [Fact]
        public async Task GetByLoanUIDAsync_ShouldReturnLoanExtensions_WhenLoanExtensionsExists()
        {
            // Arrange
            Faker<LoanEntity> _LoanFaker = new Faker<LoanEntity>()
                .RuleFor(o => o.LoanerEmail, f => f.Person.Email)
                .RuleFor(o => o.LoaneeEmail, f => f.Person.Email)
                .RuleFor(o => o.FromTimestamp, f => f.Date.Between(DateTime.Now, DateTime.Now.AddDays(14)))
                .RuleFor(o => o.ExpiryTimestamp, f => f.Date.Between(DateTime.Now.AddDays(14), DateTime.Now.AddDays(56)))
                .RuleFor(o => o.Status, f => Status.Pending)
                .RuleFor(o => o.AcceptedTermsAndConditions, false);

            LoanEntity _LoanFake = _LoanFaker.Generate();

            Faker<LoanExtensionEntity> _LoanExtensionFaker = new Faker<LoanExtensionEntity>()
                .RuleFor(o => o.LoanUID, f => _LoanFake.UID)
                .RuleFor(o => o.ExtenderEmail, f => f.Person.Email)
                .RuleFor(o => o.PreviousExpiryDate, f => f.Date.Between(DateTime.Today.AddDays(1), DateTime.Today.AddMonths(1)))
                .RuleFor(o => o.NewExpiryDate, f => f.Date.Between(DateTime.Today.AddMonths(1), DateTime.Today.AddMonths(5)))
                .RuleFor(o => o.Loan, f => _LoanFake);

            LoanExtensionEntity _LoanExtensionFake = _LoanExtensionFaker.Generate();

            __LoanRepositoryMock.Setup(x => x.GetAsync(_LoanFake.UID)).ReturnsAsync(new List<LoanExtensionEntity> { _LoanExtensionFake });

            // Act
            IList<LoanExtensionEntity> _LoanExtensions = await __LoanRepositoryMock.Object.GetAsync(_LoanFake.UID);

            // Assert
            Assert.NotEmpty(_LoanExtensions);
        }

        [Fact]
        public async Task GetByLoanUIDAsync_ShouldNotReturnLoanExtensions_WhenLoanExtensionsDoesNotExist()
        {
            // Arrange
            __LoanRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(() => new List<LoanExtensionEntity>());

            // Act
            IList<LoanExtensionEntity> _LoanExtensions =  await __LoanRepositoryMock.Object.GetAsync(Guid.NewGuid());

            // Assert
            Assert.Empty(_LoanExtensions);
        }

        [Fact]
        public async Task GetByUIDAsync_ShouldReturnLoanExtension_WhenLoanExtension()
        {
            // Arrange
            Faker<LoanEntity> _LoanFaker = new Faker<LoanEntity>()
                .RuleFor(o => o.LoanerEmail, f => f.Person.Email)
                .RuleFor(o => o.LoaneeEmail, f => f.Person.Email)
                .RuleFor(o => o.FromTimestamp, f => f.Date.Between(DateTime.Now, DateTime.Now.AddDays(14)))
                .RuleFor(o => o.ExpiryTimestamp, f => f.Date.Between(DateTime.Now.AddDays(14), DateTime.Now.AddDays(56)))
                .RuleFor(o => o.Status, f => Status.Pending)
                .RuleFor(o => o.AcceptedTermsAndConditions, false);

            LoanEntity _LoanFake = _LoanFaker.Generate();

            Faker<LoanExtensionEntity> _LoanExtensionFaker = new Faker<LoanExtensionEntity>()
                .RuleFor(o => o.LoanUID, f => _LoanFake.UID)
                .RuleFor(o => o.ExtenderEmail, f => f.Person.Email)
                .RuleFor(o => o.PreviousExpiryDate, f => f.Date.Between(DateTime.Today.AddDays(1), DateTime.Today.AddMonths(1)))
                .RuleFor(o => o.NewExpiryDate, f => f.Date.Between(DateTime.Today.AddMonths(1), DateTime.Today.AddMonths(5)))
                .RuleFor(o => o.Loan, f => _LoanFake);

            LoanExtensionEntity _LoanExtensionFake = _LoanExtensionFaker.Generate();

            __LoanRepositoryMock.Setup(x => x.GetByUIDAsync(_LoanExtensionFake.UID)).ReturnsAsync(_LoanExtensionFake);

            // Act
            LoanExtensionEntity _LoanExtension = await __LoanRepositoryMock.Object.GetByUIDAsync(_LoanExtensionFake.UID);

            // Assert
            Assert.Equal(_LoanExtensionFake, _LoanExtension);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnTrue_WhenLoanExtensionExists()
        {
            // Arrange
            Faker<LoanEntity> _LoanFaker = new Faker<LoanEntity>()
                .RuleFor(o => o.LoanerEmail, f => f.Person.Email)
                .RuleFor(o => o.LoaneeEmail, f => f.Person.Email)
                .RuleFor(o => o.FromTimestamp, f => f.Date.Between(DateTime.Now, DateTime.Now.AddDays(14)))
                .RuleFor(o => o.ExpiryTimestamp, f => f.Date.Between(DateTime.Now.AddDays(14), DateTime.Now.AddDays(56)))
                .RuleFor(o => o.Status, f => Status.Pending)
                .RuleFor(o => o.AcceptedTermsAndConditions, false);

            LoanEntity _LoanFake = _LoanFaker.Generate();

            Faker<LoanExtensionEntity> _LoanExtensionFaker = new Faker<LoanExtensionEntity>()
                .RuleFor(o => o.LoanUID, f => _LoanFake.UID)
                .RuleFor(o => o.ExtenderEmail, f => f.Person.Email)
                .RuleFor(o => o.PreviousExpiryDate, f => f.Date.Between(DateTime.Today.AddDays(1), DateTime.Today.AddMonths(1)))
                .RuleFor(o => o.NewExpiryDate, f => f.Date.Between(DateTime.Today.AddMonths(1), DateTime.Today.AddMonths(5)))
                .RuleFor(o => o.Loan, f => _LoanFake);

            IList<LoanExtensionEntity> _LoanExtensionFakes = _LoanExtensionFaker.Generate(5);

            __LoanRepositoryMock.Setup(x => x.UpdateAsync(It.Is<LoanExtensionEntity>(x => _LoanExtensionFakes.Select(l => l.UID).Contains(x.UID)))).ReturnsAsync(true);

            // Act
            bool _Result = await __LoanRepositoryMock.Object.UpdateAsync(_LoanExtensionFakes[0]);

            // Assert
            Assert.True(_Result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnFalse_WhenLoanExtensionDoesNotExist()
        {
            // Arrange
            Faker<LoanEntity> _LoanFaker = new Faker<LoanEntity>()
                .RuleFor(o => o.LoanerEmail, f => f.Person.Email)
                .RuleFor(o => o.LoaneeEmail, f => f.Person.Email)
                .RuleFor(o => o.FromTimestamp, f => f.Date.Between(DateTime.Now, DateTime.Now.AddDays(14)))
                .RuleFor(o => o.ExpiryTimestamp, f => f.Date.Between(DateTime.Now.AddDays(14), DateTime.Now.AddDays(56)))
                .RuleFor(o => o.Status, f => Status.Pending)
                .RuleFor(o => o.AcceptedTermsAndConditions, false);

            LoanEntity _LoanFake = _LoanFaker.Generate();

            Faker<LoanExtensionEntity> _LoanExtensionFaker = new Faker<LoanExtensionEntity>()
                .RuleFor(o => o.LoanUID, f => _LoanFake.UID)
                .RuleFor(o => o.ExtenderEmail, f => f.Person.Email)
                .RuleFor(o => o.PreviousExpiryDate, f => f.Date.Between(DateTime.Today.AddDays(1), DateTime.Today.AddMonths(1)))
                .RuleFor(o => o.NewExpiryDate, f => f.Date.Between(DateTime.Today.AddMonths(1), DateTime.Today.AddMonths(5)))
                .RuleFor(o => o.Loan, f => _LoanFake);

            IList<LoanExtensionEntity> _LoanExtensionFakes = _LoanExtensionFaker.Generate(5);

            __LoanRepositoryMock.Setup(x => x.UpdateAsync(It.Is<LoanExtensionEntity>(x => !_LoanExtensionFakes.Select(l => l.UID).Contains(x.UID)))).ReturnsAsync(false);

            // Act
            bool _Result = await __LoanRepositoryMock.Object.UpdateAsync(_LoanExtensionFaker.Generate());

            // Assert
            Assert.False(_Result);
        }
    }
}
