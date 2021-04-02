using Bogus;
using ELMS.WEB.Entities.Equipment;
using ELMS.WEB.Entities.Loan;
using ELMS.WEB.Repositories.Loan.Interface;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using NsEquipment = ELMS.WEB.Enums.Equipment;
using NsLoan = ELMS.WEB.Enums.Loan;

namespace ELMS.XUNIT.Loan
{
    public class LoanEquipmentRepository
    {
        private readonly Mock<ILoanEquipmentRepository> __EquipmentRepositoryMock = new Mock<ILoanEquipmentRepository>();

        [Fact]
        public async Task GetAsync_ShouldReturnLoanEquipments_WhenLoanEquipmentsExist()
        {
            // Arrange
            Faker<EquipmentEntity> _EquipmentFaker = new Faker<EquipmentEntity>()
                .RuleFor(o => o.Name, f => f.Commerce.ProductName())
                .RuleFor(o => o.Description, f => f.Commerce.ProductMaterial())
                .RuleFor(o => o.SerialNumber, f => f.Commerce.Ean8())
                .RuleFor(o => o.PurchasePrice, f => f.Random.Number(50, 2000))
                .RuleFor(o => o.ReplacementPrice, f => f.Random.Number(50, 2000))
                .RuleFor(o => o.Status, f => f.PickRandom<NsEquipment.Status>())
                .RuleFor(o => o.WarrantyExpirationDate, f => f.Date.Future(5, DateTime.Today.AddDays(1)))
                .RuleFor(o => o.PurchaseDate, f => f.Date.Past(1, DateTime.Today));

            Faker<LoanEntity> _LoanFaker = new Faker<LoanEntity>()
                .RuleFor(o => o.LoanerEmail, f => f.Person.Email)
                .RuleFor(o => o.LoaneeEmail, f => f.Person.Email)
                .RuleFor(o => o.FromTimestamp, f => f.Date.Between(DateTime.Now, DateTime.Now.AddDays(14)))
                .RuleFor(o => o.ExpiryTimestamp, f => f.Date.Between(DateTime.Now.AddDays(14), DateTime.Now.AddDays(56)))
                .RuleFor(o => o.Status, f => NsLoan.Status.Pending)
                .RuleFor(o => o.AcceptedTermsAndConditions, false);

            EquipmentEntity _Equipment = _EquipmentFaker.Generate();
            LoanEntity _Loan = _LoanFaker.Generate();

            Faker<LoanEquipmentEntity> _LoanEquipmentFaker = new Faker<LoanEquipmentEntity>()
                .RuleFor(o => o.Equipment, f => _Equipment)
                .RuleFor(o => o.Loan, f => _Loan)
                .RuleFor(o => o.EquipmentUID, f => _Equipment.UID)
                .RuleFor(o => o.LoanUID, f => _Loan.UID);

            IList<LoanEquipmentEntity> _LoanEquipmentFakes = _LoanEquipmentFaker.Generate(5);

            __EquipmentRepositoryMock.Setup(x => x.GetAsync()).ReturnsAsync(_LoanEquipmentFakes);

            // Act
            IList<LoanEquipmentEntity> _LoanEquipment = await __EquipmentRepositoryMock.Object.GetAsync();

            // Assert
            Assert.Equal(5, _LoanEquipment.Count());
        }

        [Fact]
        public async Task GetAsync_ShouldNotReturnLoanEquipments_WhenLoanEquipmentsDoNotExist()
        {
            // Arrange
            __EquipmentRepositoryMock.Setup(x => x.GetAsync()).ReturnsAsync(() => null);

            // Act
            IList<LoanEquipmentEntity> _LoanEquipment = await __EquipmentRepositoryMock.Object.GetAsync();

            // Assert
            Assert.Null(_LoanEquipment);
        }

        [Fact]
        public async Task GetByEquipmentAsync_ShouldReturnLoanEquipments_WhenEquipmentIsValid()
        {
            // Arrange
            Faker<EquipmentEntity> _EquipmentFaker = new Faker<EquipmentEntity>()
                .RuleFor(o => o.Name, f => f.Commerce.ProductName())
                .RuleFor(o => o.Description, f => f.Commerce.ProductMaterial())
                .RuleFor(o => o.SerialNumber, f => f.Commerce.Ean8())
                .RuleFor(o => o.PurchasePrice, f => f.Random.Number(50, 2000))
                .RuleFor(o => o.ReplacementPrice, f => f.Random.Number(50, 2000))
                .RuleFor(o => o.Status, f => f.PickRandom<NsEquipment.Status>())
                .RuleFor(o => o.WarrantyExpirationDate, f => f.Date.Future(5, DateTime.Today.AddDays(1)))
                .RuleFor(o => o.PurchaseDate, f => f.Date.Past(1, DateTime.Today));

            Faker<LoanEntity> _LoanFaker = new Faker<LoanEntity>()
                .RuleFor(o => o.LoanerEmail, f => f.Person.Email)
                .RuleFor(o => o.LoaneeEmail, f => f.Person.Email)
                .RuleFor(o => o.FromTimestamp, f => f.Date.Between(DateTime.Now, DateTime.Now.AddDays(14)))
                .RuleFor(o => o.ExpiryTimestamp, f => f.Date.Between(DateTime.Now.AddDays(14), DateTime.Now.AddDays(56)))
                .RuleFor(o => o.Status, f => NsLoan.Status.Pending)
                .RuleFor(o => o.AcceptedTermsAndConditions, false);

            EquipmentEntity _Equipment = _EquipmentFaker.Generate();
            LoanEntity _Loan = _LoanFaker.Generate();

            Faker<LoanEquipmentEntity> _LoanEquipmentFaker = new Faker<LoanEquipmentEntity>()
                .RuleFor(o => o.Equipment, f => _Equipment)
                .RuleFor(o => o.Loan, f => _Loan)
                .RuleFor(o => o.EquipmentUID, f => _Equipment.UID)
                .RuleFor(o => o.LoanUID, f => _Loan.UID);

            IList<LoanEquipmentEntity> _LoanEquipmentFakes = _LoanEquipmentFaker.Generate(5);

            __EquipmentRepositoryMock.Setup(x => x.GetByEquipmentAsync(It.IsAny<Guid>())).ReturnsAsync((Guid equipmentUID) => _LoanEquipmentFakes.Where(x => x.EquipmentUID == equipmentUID).ToList());

            // Act
            IList<LoanEquipmentEntity> _LoanEquipment = await __EquipmentRepositoryMock.Object.GetByEquipmentAsync(_LoanEquipmentFakes[0].EquipmentUID);

            // Assert
            Assert.NotEmpty(_LoanEquipment);
        }

        [Fact]
        public async Task GetByEquipmentAsync_ShouldNotReturnLoanEquipments_WhenLoanEquipmentsDoesNotExist()
        {
            // Arrange
            __EquipmentRepositoryMock.Setup(x => x.GetByEquipmentAsync(It.IsAny<Guid>())).ReturnsAsync(() => null);

            // Act
            IList<LoanEquipmentEntity> _LoanEquipment = await __EquipmentRepositoryMock.Object.GetByEquipmentAsync(Guid.NewGuid());

            // Assert
            Assert.Null(_LoanEquipment);
        }

        [Fact]
        public async Task GetByLoanUID_ShouldReturnLoanEquipments_WhenLoanExists()
        {
            // Arrange
            Faker<EquipmentEntity> _EquipmentFaker = new Faker<EquipmentEntity>()
                .RuleFor(o => o.Name, f => f.Commerce.ProductName())
                .RuleFor(o => o.Description, f => f.Commerce.ProductMaterial())
                .RuleFor(o => o.SerialNumber, f => f.Commerce.Ean8())
                .RuleFor(o => o.PurchasePrice, f => f.Random.Number(50, 2000))
                .RuleFor(o => o.ReplacementPrice, f => f.Random.Number(50, 2000))
                .RuleFor(o => o.Status, f => f.PickRandom<NsEquipment.Status>())
                .RuleFor(o => o.WarrantyExpirationDate, f => f.Date.Future(5, DateTime.Today.AddDays(1)))
                .RuleFor(o => o.PurchaseDate, f => f.Date.Past(1, DateTime.Today));

            Faker<LoanEntity> _LoanFaker = new Faker<LoanEntity>()
                .RuleFor(o => o.LoanerEmail, f => f.Person.Email)
                .RuleFor(o => o.LoaneeEmail, f => f.Person.Email)
                .RuleFor(o => o.FromTimestamp, f => f.Date.Between(DateTime.Now, DateTime.Now.AddDays(14)))
                .RuleFor(o => o.ExpiryTimestamp, f => f.Date.Between(DateTime.Now.AddDays(14), DateTime.Now.AddDays(56)))
                .RuleFor(o => o.Status, f => NsLoan.Status.Pending)
                .RuleFor(o => o.AcceptedTermsAndConditions, false);

            EquipmentEntity _Equipment = _EquipmentFaker.Generate();
            LoanEntity _Loan = _LoanFaker.Generate();

            Faker<LoanEquipmentEntity> _LoanEquipmentFaker = new Faker<LoanEquipmentEntity>()
                .RuleFor(o => o.Equipment, f => _Equipment)
                .RuleFor(o => o.Loan, f => _Loan)
                .RuleFor(o => o.EquipmentUID, f => _Equipment.UID)
                .RuleFor(o => o.LoanUID, f => _Loan.UID);

            IList<LoanEquipmentEntity> _LoanEquipmentFakes = _LoanEquipmentFaker.Generate(5);

            __EquipmentRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync((Guid loanUID) => _LoanEquipmentFakes.Where(x => x.LoanUID == loanUID).ToList());

            // Act
            IList<LoanEquipmentEntity> _LoanEquipment = await __EquipmentRepositoryMock.Object.GetAsync(_LoanEquipmentFakes[0].LoanUID);

            // Assert
            Assert.Equal(_LoanEquipmentFakes[0], _LoanEquipment[0]);
        }

        [Fact]
        public async Task GetByLoanUID_ShouldNotReturnLoanEquipments_WhenLoanDoesNotExists()
        {
            // Arrange
            __EquipmentRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(() => null);

            // Act
            IList<LoanEquipmentEntity> _LoanEquipment = await __EquipmentRepositoryMock.Object.GetAsync(Guid.NewGuid());

            // Assert
            Assert.Null(_LoanEquipment);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenLoanEquipmentExists()
        {
            // Arrange
            Faker<EquipmentEntity> _EquipmentFaker = new Faker<EquipmentEntity>()
                .RuleFor(o => o.Name, f => f.Commerce.ProductName())
                .RuleFor(o => o.Description, f => f.Commerce.ProductMaterial())
                .RuleFor(o => o.SerialNumber, f => f.Commerce.Ean8())
                .RuleFor(o => o.PurchasePrice, f => f.Random.Number(50, 2000))
                .RuleFor(o => o.ReplacementPrice, f => f.Random.Number(50, 2000))
                .RuleFor(o => o.Status, f => f.PickRandom<NsEquipment.Status>())
                .RuleFor(o => o.WarrantyExpirationDate, f => f.Date.Future(5, DateTime.Today.AddDays(1)))
                .RuleFor(o => o.PurchaseDate, f => f.Date.Past(1, DateTime.Today));

            Faker<LoanEntity> _LoanFaker = new Faker<LoanEntity>()
                .RuleFor(o => o.LoanerEmail, f => f.Person.Email)
                .RuleFor(o => o.LoaneeEmail, f => f.Person.Email)
                .RuleFor(o => o.FromTimestamp, f => f.Date.Between(DateTime.Now, DateTime.Now.AddDays(14)))
                .RuleFor(o => o.ExpiryTimestamp, f => f.Date.Between(DateTime.Now.AddDays(14), DateTime.Now.AddDays(56)))
                .RuleFor(o => o.Status, f => NsLoan.Status.Pending)
                .RuleFor(o => o.AcceptedTermsAndConditions, false);

            EquipmentEntity _Equipment = _EquipmentFaker.Generate();
            LoanEntity _Loan = _LoanFaker.Generate();

            Faker<LoanEquipmentEntity> _LoanEquipmentFaker = new Faker<LoanEquipmentEntity>()
                .RuleFor(o => o.Equipment, f => _Equipment)
                .RuleFor(o => o.Loan, f => _Loan)
                .RuleFor(o => o.EquipmentUID, f => _Equipment.UID)
                .RuleFor(o => o.LoanUID, f => _Loan.UID);

            LoanEquipmentEntity _LoanEquipmentFake = _LoanEquipmentFaker.Generate();

            __EquipmentRepositoryMock.Setup(x => x.DeleteAsync(_LoanEquipmentFake.UID)).ReturnsAsync(true);

            // Act
            bool _Result = await __EquipmentRepositoryMock.Object.DeleteAsync(_LoanEquipmentFake.UID);

            // Assert
            Assert.True(_Result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenLoanEquimentDoesNotExist()
        {
            // Arrange
            __EquipmentRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(false);

            // Act
            bool _Result = await __EquipmentRepositoryMock.Object.DeleteAsync(Guid.NewGuid());

            // Assert
            Assert.False(_Result);
        }
    }
}
