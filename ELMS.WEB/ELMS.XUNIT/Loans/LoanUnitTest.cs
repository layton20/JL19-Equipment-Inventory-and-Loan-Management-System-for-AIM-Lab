using Bogus;
using ELMS.WEB.Entities.Equipment;
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
    public class LoanUnitTest
    {
        private readonly Mock<ILoanRepository> __LoanRepositoryMock = new Mock<ILoanRepository>();

        [Fact]
        public async Task CreateAsync_ShouldReturnLoan_WhenLoanIsValid()
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

            Faker<EquipmentEntity> _EquipmentFaker = new Faker<EquipmentEntity>()
                .RuleFor(o => o.Name, f => f.Commerce.ProductName())
                .RuleFor(o => o.Description, f => f.Commerce.ProductMaterial())
                .RuleFor(o => o.SerialNumber, f => f.Commerce.Ean8())
                .RuleFor(o => o.PurchasePrice, f => f.Random.Number(50, 2000))
                .RuleFor(o => o.ReplacementPrice, f => f.Random.Number(50, 2000))
                .RuleFor(o => o.Status, f => f.PickRandom<WEB.Enums.Equipment.Status>())
                .RuleFor(o => o.WarrantyExpirationDate, f => f.Date.Future(5, DateTime.Today.AddDays(1)))
                .RuleFor(o => o.PurchaseDate, f => f.Date.Past(1, DateTime.Today));

            IList<Guid> _EquipmentFakes = _EquipmentFaker.Generate(3).Select(x => x.UID).ToList();

            __LoanRepositoryMock.Setup(x => x.CreateAsync(_LoanFake, It.IsAny<List<Guid>>())).ReturnsAsync(_LoanFake);

            // Act
            LoanEntity _LoanEntity = await __LoanRepositoryMock.Object.CreateAsync(_LoanFake, _EquipmentFakes.ToList());

            // Assert
            Assert.Equal(_LoanFake, _LoanEntity);
        }

        [Fact]
        public async Task CreateAsync_ShouldNotReturnLoan_WhenLoanUIDIsEmpty()
        {
            // Arrange
            LoanEntity _LoanFake = new LoanEntity { UID = Guid.Empty };
            __LoanRepositoryMock.Setup(x => x.CreateAsync(It.Is<LoanEntity>(x => x.UID == Guid.Empty), It.IsAny<List<Guid>>())).ReturnsAsync(() => null);

            Faker _UIDFaker = new Faker();

            // Act
            LoanEntity _Loan = await __LoanRepositoryMock.Object.CreateAsync(_LoanFake, Enumerable.Range(1, 5)
                .Select(_ => _UIDFaker.Random.Guid()).ToList());

            // Assert
            Assert.Null(_Loan);
        }

        [Fact]
        public async Task CreateAsync_ShouldNotReturnLoan_WhenEquipmentListIsEmpty()
        {
            // Arrange
            LoanEntity _LoanFake = new LoanEntity { UID = Guid.Empty };
            __LoanRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<LoanEntity>(), null)).ReturnsAsync(() => null);
            __LoanRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<LoanEntity>(), new List<Guid>())).ReturnsAsync(() => null);

            // Act
            LoanEntity _LoanNullEquipmentList = await __LoanRepositoryMock.Object.CreateAsync(_LoanFake, null);
            LoanEntity _LoanEmptyEquipmentList = await __LoanRepositoryMock.Object.CreateAsync(_LoanFake, new List<Guid>());

            // Assert
            Assert.Null(_LoanNullEquipmentList);
            Assert.Null(_LoanEmptyEquipmentList);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenLoanExists()
        {
            // Arrange
            Guid _LoanUID = Guid.NewGuid();
            __LoanRepositoryMock.Setup(x => x.DeleteAsync(It.Is<Guid>(x => x != Guid.Empty))).ReturnsAsync(true);

            // Act
            bool _Result = await __LoanRepositoryMock.Object.DeleteAsync(_LoanUID);

            // Assert
            Assert.True(_Result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenLoanDoesNotExist()
        {
            // Arrange
            Guid _LoanUID = Guid.Empty;
            __LoanRepositoryMock.Setup(x => x.DeleteAsync(It.Is<Guid>(x => x != Guid.Empty))).ReturnsAsync(true);

            // Act
            bool _Result = await __LoanRepositoryMock.Object.DeleteAsync(_LoanUID);

            // Assert
            Assert.False(_Result);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnAllLoan_WhenLoansExists()
        {
            // Arrange
            Faker<LoanEntity> _LoanFaker = new Faker<LoanEntity>()
                .RuleFor(o => o.LoanerEmail, f => f.Person.Email)
                .RuleFor(o => o.LoaneeEmail, f => f.Person.Email)
                .RuleFor(o => o.FromTimestamp, f => f.Date.Between(DateTime.Now, DateTime.Now.AddDays(14)))
                .RuleFor(o => o.ExpiryTimestamp, f => f.Date.Between(DateTime.Now.AddDays(14), DateTime.Now.AddDays(56)))
                .RuleFor(o => o.Status, f => Status.Pending)
                .RuleFor(o => o.AcceptedTermsAndConditions, false);

            IList<LoanEntity> _LoanFakes = _LoanFaker.Generate(5);

            __LoanRepositoryMock.Setup(x => x.GetAsync()).ReturnsAsync(_LoanFakes);
            __LoanRepositoryMock.Setup(x => x.GetAsync(true)).ReturnsAsync(_LoanFakes);

            // Act
            IList<LoanEntity> _LoansTrueParameter = await __LoanRepositoryMock.Object.GetAsync(true);
            IList<LoanEntity> _LoansEmptyParameter = await __LoanRepositoryMock.Object.GetAsync();

            // Assert
            Assert.Equal(5, _LoansTrueParameter.Count);
            Assert.Equal(5, _LoansEmptyParameter.Count);
        }

        [Fact]
        public async Task GetAsync_ShouldNotReturnLoans_WhenLoansDoesNotExist()
        {
            // Arrange
            __LoanRepositoryMock.Setup(x => x.GetAsync()).ReturnsAsync(() => null);

            // Act
            IList<LoanEntity> _Loans = await __LoanRepositoryMock.Object.GetAsync();

            // Assert
            Assert.Null(_Loans);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnActiveLoans_WhenActiveLoansExist()
        {
            // Arrange
            Faker<LoanEntity> _LoanFaker = new Faker<LoanEntity>()
                .RuleFor(o => o.LoanerEmail, f => f.Person.Email)
                .RuleFor(o => o.LoaneeEmail, f => f.Person.Email)
                .RuleFor(o => o.FromTimestamp, f => f.Date.Between(DateTime.Now, DateTime.Now.AddDays(14)))
                .RuleFor(o => o.ExpiryTimestamp, f => f.Date.Between(DateTime.Now.AddDays(14), DateTime.Now.AddDays(56)))
                .RuleFor(o => o.Status, f => Status.Pending)
                .RuleFor(o => o.AcceptedTermsAndConditions, false);

            IList<LoanEntity> _LoanFakes = _LoanFaker.Generate(10);
            __LoanRepositoryMock.Setup(x => x.GetAsync(false)).ReturnsAsync(_LoanFakes.Where(x => x.Status == Status.ActiveLoan).ToList());

            // Act
            IList<LoanEntity> _Loans = await __LoanRepositoryMock.Object.GetAsync(false);

            // Assert
            Assert.Equal(_LoanFakes.Where(x => x.Status == Status.ActiveLoan), _Loans.Where(x => x.Status == Status.ActiveLoan));
        }

        [Fact]
        public async Task AcceptTermsAndConditionsAsync_ShouldReturnTrue_WhenLoanExists_And_WhenTermsAndConditionsNotAccepted()
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

            __LoanRepositoryMock.Setup(x => x.AcceptTermsAndConditionsAsync(_LoanFake.UID)).ReturnsAsync(true);

            // Act
            bool _Result = await __LoanRepositoryMock.Object.AcceptTermsAndConditionsAsync(_LoanFake.UID);

            // Assert
            Assert.True(_Result);
        }

        [Fact]
        public async Task AcceptTermsAndConditionsAsync_ShouldReturnFalse_WhenTermsAndConditionsAlreadyAccepted()
        {
            // Arrange
            Faker<LoanEntity> _LoanFaker = new Faker<LoanEntity>()
                .RuleFor(o => o.LoanerEmail, f => f.Person.Email)
                .RuleFor(o => o.LoaneeEmail, f => f.Person.Email)
                .RuleFor(o => o.FromTimestamp, f => f.Date.Between(DateTime.Now, DateTime.Now.AddDays(14)))
                .RuleFor(o => o.ExpiryTimestamp, f => f.Date.Between(DateTime.Now.AddDays(14), DateTime.Now.AddDays(56)))
                .RuleFor(o => o.Status, f => Status.Pending)
                .RuleFor(o => o.AcceptedTermsAndConditions, true);

            LoanEntity _LoanFake = _LoanFaker.Generate();
            __LoanRepositoryMock.Setup(x => x.AcceptTermsAndConditionsAsync(_LoanFake.UID)).ReturnsAsync(false);

            // Act
            bool _Result = await __LoanRepositoryMock.Object.AcceptTermsAndConditionsAsync(_LoanFake.UID);

            // Assert
            Assert.False(_Result);
        }

        [Fact]
        public async Task ChangeStatusAsync_ShouldReturnTrue_WhenLoanExists()
        {
            // Arrange
            Faker<LoanEntity> _LoanFaker = new Faker<LoanEntity>()
                .RuleFor(o => o.LoanerEmail, f => f.Person.Email)
                .RuleFor(o => o.LoaneeEmail, f => f.Person.Email)
                .RuleFor(o => o.FromTimestamp, f => f.Date.Between(DateTime.Now, DateTime.Now.AddDays(14)))
                .RuleFor(o => o.ExpiryTimestamp, f => f.Date.Between(DateTime.Now.AddDays(14), DateTime.Now.AddDays(56)))
                .RuleFor(o => o.Status, f => f.PickRandom<Status>())
                .RuleFor(o => o.AcceptedTermsAndConditions, true);

            IList<LoanEntity> _LoanFakes = _LoanFaker.Generate(5);
            __LoanRepositoryMock.Setup(x => x.ChangeStatusAsync(It.Is<Guid>(x => _LoanFakes.Select(l => l.UID).Contains(x)), It.IsAny<Status>())).ReturnsAsync(true);

            // Act
            bool _Result = await __LoanRepositoryMock.Object.ChangeStatusAsync(_LoanFakes[0].UID, Status.Complete);

            // Assert
            Assert.True(_Result);
        }

        [Fact]
        public async Task ChangeStatusAsync_ShouldReturnFalse_WhenLoanDoesNotExist()
        {
            // Arrange
            Faker<LoanEntity> _LoanFaker = new Faker<LoanEntity>()
                .RuleFor(o => o.LoanerEmail, f => f.Person.Email)
                .RuleFor(o => o.LoaneeEmail, f => f.Person.Email)
                .RuleFor(o => o.FromTimestamp, f => f.Date.Between(DateTime.Now, DateTime.Now.AddDays(14)))
                .RuleFor(o => o.ExpiryTimestamp, f => f.Date.Between(DateTime.Now.AddDays(14), DateTime.Now.AddDays(56)))
                .RuleFor(o => o.Status, f => f.PickRandom<Status>())
                .RuleFor(o => o.AcceptedTermsAndConditions, true);

            IList<LoanEntity> _LoanFakes = _LoanFaker.Generate(5);

            __LoanRepositoryMock.Setup(x => x.ChangeStatusAsync(It.Is<Guid>(x => _LoanFakes.Select(l => l.UID).Contains(x)), It.IsAny<Status>())).ReturnsAsync(true);
            __LoanRepositoryMock.Setup(x => x.ChangeStatusAsync(It.Is<Guid>(x => !_LoanFakes.Select(l => l.UID).Contains(x)), It.IsAny<Status>())).ReturnsAsync(false);

            // Act
            bool _Result = await __LoanRepositoryMock.Object.ChangeStatusAsync(Guid.NewGuid(), Status.Complete);

            // Assert
            Assert.False(_Result);
        }

        [Fact]
        public async Task CompleteLoanAsync_ShouldReturnTrue_WhenLoanExists()
        {
            // Arrange
            Faker<LoanEntity> _LoanFaker = new Faker<LoanEntity>()
                .RuleFor(o => o.LoanerEmail, f => f.Person.Email)
                .RuleFor(o => o.LoaneeEmail, f => f.Person.Email)
                .RuleFor(o => o.FromTimestamp, f => f.Date.Between(DateTime.Now, DateTime.Now.AddDays(14)))
                .RuleFor(o => o.ExpiryTimestamp, f => f.Date.Between(DateTime.Now.AddDays(14), DateTime.Now.AddDays(56)))
                .RuleFor(o => o.Status, f => f.PickRandom<Status>())
                .RuleFor(o => o.AcceptedTermsAndConditions, true);

            IList<LoanEntity> _LoanFakes = _LoanFaker.Generate(5);

            __LoanRepositoryMock.Setup(x => x.CompleteLoanAsync(It.Is<Guid>(x => _LoanFakes.Select(l => l.UID).Contains(x)))).ReturnsAsync(true);

            // Act
            bool _Response = await __LoanRepositoryMock.Object.CompleteLoanAsync(_LoanFakes[0].UID);

            // Assert
            Assert.True(_Response);
        }

        [Fact]
        public async Task CompleteLoanAsync_ShouldReturnFalse_WhenLoanDoesNotExist()
        {
            // Arrange
            Faker<LoanEntity> _LoanFaker = new Faker<LoanEntity>()
                .RuleFor(o => o.LoanerEmail, f => f.Person.Email)
                .RuleFor(o => o.LoaneeEmail, f => f.Person.Email)
                .RuleFor(o => o.FromTimestamp, f => f.Date.Between(DateTime.Now, DateTime.Now.AddDays(14)))
                .RuleFor(o => o.ExpiryTimestamp, f => f.Date.Between(DateTime.Now.AddDays(14), DateTime.Now.AddDays(56)))
                .RuleFor(o => o.Status, f => f.PickRandom<Status>())
                .RuleFor(o => o.AcceptedTermsAndConditions, true);

            IList<LoanEntity> _LoanFakes = _LoanFaker.Generate(5);

            __LoanRepositoryMock.Setup(x => x.CompleteLoanAsync(It.Is<Guid>(x => _LoanFakes.Select(l => l.UID).Contains(x)))).ReturnsAsync(true);

            // Act
            bool _Response = await __LoanRepositoryMock.Object.CompleteLoanAsync(Guid.NewGuid());

            // Assert
            Assert.False(_Response);
        }

        [Fact]
        public async Task CompleteLoanAsync_ShouldReturnFalse_WhenLoanIsNotValid()
        {
            // Arrange
            __LoanRepositoryMock.Setup(x => x.CompleteLoanAsync(Guid.Empty)).ReturnsAsync(false);

            // Act
            bool _Result = await __LoanRepositoryMock.Object.CompleteLoanAsync(Guid.Empty);

            // Assert
            Assert.False(_Result);
        }

        [Fact]
        public async Task GetByLoaneeAsync_ShouldReturnLoans_WhenLoansWithLoaneeExists()
        {
            // Arrange
            Faker<LoanEntity> _LoanFaker = new Faker<LoanEntity>()
                .RuleFor(o => o.LoanerEmail, f => f.Person.Email)
                .RuleFor(o => o.LoaneeEmail, f => f.Person.Email)
                .RuleFor(o => o.FromTimestamp, f => f.Date.Between(DateTime.Now, DateTime.Now.AddDays(14)))
                .RuleFor(o => o.ExpiryTimestamp, f => f.Date.Between(DateTime.Now.AddDays(14), DateTime.Now.AddDays(56)))
                .RuleFor(o => o.Status, f => f.PickRandom<Status>())
                .RuleFor(o => o.AcceptedTermsAndConditions, true);

            LoanEntity _LoanFake = _LoanFaker.Generate();

            __LoanRepositoryMock.Setup(x => x.GetByLoaneeAsync(_LoanFake.LoaneeEmail)).ReturnsAsync(new List<LoanEntity> { _LoanFake });

            // Act
            IList<LoanEntity> _Loans = await __LoanRepositoryMock.Object.GetByLoaneeAsync(_LoanFake.LoaneeEmail);

            // Assert
            Assert.NotEmpty(_Loans);
        }

        [Fact]
        public async Task GetByLoaneeAsync_ShouldNotReturnLoans_WhenLoansWithLoaneeDoesNotExist() 
        {
            // Arrange
            Faker<LoanEntity> _LoanFaker = new Faker<LoanEntity>()
                .RuleFor(o => o.LoanerEmail, f => f.Person.Email)
                .RuleFor(o => o.LoaneeEmail, f => f.Person.Email)
                .RuleFor(o => o.FromTimestamp, f => f.Date.Between(DateTime.Now, DateTime.Now.AddDays(14)))
                .RuleFor(o => o.ExpiryTimestamp, f => f.Date.Between(DateTime.Now.AddDays(14), DateTime.Now.AddDays(56)))
                .RuleFor(o => o.Status, f => f.PickRandom<Status>())
                .RuleFor(o => o.AcceptedTermsAndConditions, true);

            IList<LoanEntity> _LoanFakes = _LoanFaker.Generate(5);

            __LoanRepositoryMock.Setup(x => x.GetByLoaneeAsync(It.IsAny<string>())).ReturnsAsync((string email) => _LoanFakes.Where(x => x.LoaneeEmail == email).ToList());

            // Act
            IList<LoanEntity> _Loans = await __LoanRepositoryMock.Object.GetByLoaneeAsync($"{Guid.Empty}@gmail.com");

            // Assert
            Assert.Empty(_Loans);
        }

        [Fact]
        public async Task GetByUIDAsync_ShouldReturnLoan_WhenLoanExists()
        {
            // Arrange
            Faker<LoanEntity> _LoanFaker = new Faker<LoanEntity>()
                .RuleFor(o => o.LoanerEmail, f => f.Person.Email)
                .RuleFor(o => o.LoaneeEmail, f => f.Person.Email)
                .RuleFor(o => o.FromTimestamp, f => f.Date.Between(DateTime.Now, DateTime.Now.AddDays(14)))
                .RuleFor(o => o.ExpiryTimestamp, f => f.Date.Between(DateTime.Now.AddDays(14), DateTime.Now.AddDays(56)))
                .RuleFor(o => o.Status, f => f.PickRandom<Status>())
                .RuleFor(o => o.AcceptedTermsAndConditions, true);

            IList<LoanEntity> _LoanFakes = _LoanFaker.Generate(5);

            __LoanRepositoryMock.Setup(x => x.GetByUIDAsync(It.IsAny<Guid>())).ReturnsAsync((Guid uid) => _LoanFakes.FirstOrDefault(x => x.UID == uid));

            // Act
            LoanEntity _Loan = await __LoanRepositoryMock.Object.GetByUIDAsync(_LoanFakes[0].UID);

            // Assert
            Assert.Equal(_LoanFakes[0].UID, _Loan.UID);
        }

        [Fact]
        public async Task GetByUIDAsync_ShouldNotReturnLoan_WhenLoanDoesNotExist()
        {
            // Arrange
            __LoanRepositoryMock.Setup(x => x.GetByUIDAsync(It.IsAny<Guid>())).ReturnsAsync(() => null);

            // Act
            LoanEntity _Loan = await __LoanRepositoryMock.Object.GetByUIDAsync(Guid.NewGuid());

            // Assert
            Assert.Null(_Loan);
        }

        [Fact]
        public async Task GetCountByStatus_ShouldReturnCount_WhenLoansOfStatusExists()
        {
            // Arrange
            Faker<LoanEntity> _LoanFaker = new Faker<LoanEntity>()
                .RuleFor(o => o.LoanerEmail, f => f.Person.Email)
                .RuleFor(o => o.LoaneeEmail, f => f.Person.Email)
                .RuleFor(o => o.FromTimestamp, f => f.Date.Between(DateTime.Now, DateTime.Now.AddDays(14)))
                .RuleFor(o => o.ExpiryTimestamp, f => f.Date.Between(DateTime.Now.AddDays(14), DateTime.Now.AddDays(56)))
                .RuleFor(o => o.Status, f => f.PickRandom<Status>())
                .RuleFor(o => o.AcceptedTermsAndConditions, true);

            IList<LoanEntity> _LoanFakes = _LoanFaker.Generate(5);

            __LoanRepositoryMock.Setup(x => x.GetCountByStatus(It.IsAny<Status>())).ReturnsAsync((Status status) => _LoanFakes.Where(x => x.Status == status).Count());

            // Act
            int _ActiveLoansCount = await __LoanRepositoryMock.Object.GetCountByStatus(Status.ActiveLoan);
            int _CompleteLoansCount = await __LoanRepositoryMock.Object.GetCountByStatus(Status.Complete);
            int _ExpiredLoansCount = await __LoanRepositoryMock.Object.GetCountByStatus(Status.Expired);

            // Assert
            Assert.Equal(_ActiveLoansCount, _LoanFakes.Where(x => x.Status == Status.ActiveLoan).Count());
            Assert.Equal(_CompleteLoansCount, _LoanFakes.Where(x => x.Status == Status.Complete).Count());
            Assert.Equal(_ExpiredLoansCount, _LoanFakes.Where(x => x.Status == Status.Expired).Count());
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnTrue_WhenLoanExists()
        {
            // Arrange
            Faker<LoanEntity> _LoanFaker = new Faker<LoanEntity>()
                .RuleFor(o => o.LoanerEmail, f => f.Person.Email)
                .RuleFor(o => o.LoaneeEmail, f => f.Person.Email)
                .RuleFor(o => o.FromTimestamp, f => f.Date.Between(DateTime.Now, DateTime.Now.AddDays(14)))
                .RuleFor(o => o.ExpiryTimestamp, f => f.Date.Between(DateTime.Now.AddDays(14), DateTime.Now.AddDays(56)))
                .RuleFor(o => o.Status, f => f.PickRandom<Status>())
                .RuleFor(o => o.AcceptedTermsAndConditions, true);

            IList<LoanEntity> _LoanFakes = _LoanFaker.Generate(5);

            __LoanRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<LoanEntity>())).ReturnsAsync((LoanEntity loan) => _LoanFakes.FirstOrDefault(x => x.UID == loan.UID) != null);


            // Act
            bool _Result = await __LoanRepositoryMock.Object.UpdateAsync(_LoanFakes[0]);

            // Assert
            Assert.True(_Result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnFalse_WhenLoanDoesNotExist()
        {
            // Arrange
            Faker<LoanEntity> _LoanFaker = new Faker<LoanEntity>()
                .RuleFor(o => o.LoanerEmail, f => f.Person.Email)
                .RuleFor(o => o.LoaneeEmail, f => f.Person.Email)
                .RuleFor(o => o.FromTimestamp, f => f.Date.Between(DateTime.Now, DateTime.Now.AddDays(14)))
                .RuleFor(o => o.ExpiryTimestamp, f => f.Date.Between(DateTime.Now.AddDays(14), DateTime.Now.AddDays(56)))
                .RuleFor(o => o.Status, f => f.PickRandom<Status>())
                .RuleFor(o => o.AcceptedTermsAndConditions, true);

            IList<LoanEntity> _LoanFakes = _LoanFaker.Generate(5);

            __LoanRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<LoanEntity>())).ReturnsAsync((LoanEntity loan) => _LoanFakes.FirstOrDefault(x => x.UID == loan.UID) != null);


            // Act
            bool _Result = await __LoanRepositoryMock.Object.UpdateAsync(_LoanFaker.Generate());

            // Assert
            Assert.False(_Result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnFalse_WhenLoanGuidIsEmpty()
        {
            // Arrange
            __LoanRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<LoanEntity>())).ReturnsAsync((LoanEntity loan) => loan.UID == Guid.Empty);

            // Act
            bool _Result = await __LoanRepositoryMock.Object.UpdateAsync(new LoanEntity { UID = Guid.Empty });

            // Assert
            Assert.True(_Result);
        }
    }
}
