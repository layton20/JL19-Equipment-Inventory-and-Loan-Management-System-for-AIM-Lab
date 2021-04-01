using Bogus;
using ELMS.WEB.Entities.Equipment;
using ELMS.WEB.Enums.Equipment;
using ELMS.WEB.Repositories.Equipment.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ELMS.XUNIT
{
    public class EquipmentUnitTest
    {
        private readonly Mock<IEquipmentRepository> __EquipmentRepositoryMock = new Mock<IEquipmentRepository>();

        [Fact]
        public async Task GetByUIDsAsync_ShouldReturnEquipment_WhenEquipmentExists()
        {
            // Arrange
            Faker<EquipmentEntity> _EquipmentFaker = new Faker<EquipmentEntity>()
                .RuleFor(o => o.Name, f => f.Commerce.ProductName())
                .RuleFor(o => o.Description, f => f.Commerce.ProductMaterial())
                .RuleFor(o => o.SerialNumber, f => f.Commerce.Ean8())
                .RuleFor(o => o.PurchasePrice, f => f.Random.Number(50, 2000))
                .RuleFor(o => o.ReplacementPrice, f => f.Random.Number(50, 2000))
                .RuleFor(o => o.Status, f => f.PickRandom<Status>())
                .RuleFor(o => o.WarrantyExpirationDate, f => f.Date.Future(5, DateTime.Today.AddDays(1)))
                .RuleFor(o => o.PurchaseDate, f => f.Date.Past(1, DateTime.Today));

            IList<EquipmentEntity> _EquipmentFakes = _EquipmentFaker.Generate(5);
            IList<Guid> _EquipmentUIDsFakes = _EquipmentFakes.Select(x => x.UID).ToList();
            __EquipmentRepositoryMock.Setup(x => x.GetAsync(It.IsAny<List<Guid>>())).ReturnsAsync(_EquipmentFakes);

            // Act
            IList<EquipmentEntity> _Equipment = await __EquipmentRepositoryMock.Object.GetAsync(_EquipmentUIDsFakes);

            // Assert
            Assert.True(_Equipment.All(_EquipmentFakes.Contains));
        }

        [Fact]
        public async Task GetAsync_ShouldNotReturnEquipment_WhenEquipmentDoesNotExist()
        {
            // Arrange
            __EquipmentRepositoryMock.Setup(x => x.GetAsync(It.IsAny<List<Guid>>())).ReturnsAsync(() => null);

            // Setup
            IList<EquipmentEntity> _Equipment = await __EquipmentRepositoryMock.Object.GetAsync();

            // Assert
            Assert.Null(_Equipment);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEquipment_WhenEquipmentExists()
        {
            // Arrange
            Guid _EquipmentUID = Guid.NewGuid();
            Guid _OwnerUID = Guid.NewGuid();

            EquipmentEntity _EquipmentDto = new EquipmentEntity
            {
                UID = _EquipmentUID,
                Name = "TestName",
                WarrantyExpirationDate = DateTime.Today,
                Description = "TestDescription",
                PurchaseDate = DateTime.Today,
                PurchasePrice = 1000,
                ReplacementPrice = 1000,
                SerialNumber = "TestSerialNumber",
                Status = Status.Available,
                OwnerUID = _OwnerUID
            };
            __EquipmentRepositoryMock.Setup(x => x.GetAsync(_EquipmentUID)).ReturnsAsync(_EquipmentDto);

            // Act
            var _Equipment = await __EquipmentRepositoryMock.Object.GetAsync(_EquipmentUID);

            // Assert
            Assert.Equal(_EquipmentUID, _EquipmentDto.UID);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNothing_WhenEquipmentDoesNotExist()
        {
            // Arrange
            __EquipmentRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(() => null);

            // Act
            EquipmentEntity _Equipment = await __EquipmentRepositoryMock.Object.GetAsync(Guid.NewGuid());

            // Assert
            Assert.Null(_Equipment);
        }

        [Fact]
        public async Task GetByStatusAsync_ShouldReturnEquipment_WhenEquipmentForStatusExists()
        {
            // Arrange
            List<EquipmentEntity> _EquipmentFakes = new List<EquipmentEntity>();
            Faker<EquipmentEntity> _EquipmentFaker = new Faker<EquipmentEntity>()
                .RuleFor(o => o.Name, f => f.Commerce.ProductName())
                .RuleFor(o => o.Description, f => f.Commerce.ProductMaterial())
                .RuleFor(o => o.SerialNumber, f => f.Commerce.Ean8())
                .RuleFor(o => o.PurchasePrice, f => f.Random.Number(50, 2000))
                .RuleFor(o => o.ReplacementPrice, f => f.Random.Number(50, 2000))
                .RuleFor(o => o.Status, f => f.PickRandom<Status>())
                .RuleFor(o => o.WarrantyExpirationDate, f => f.Date.Future(5, DateTime.Today.AddDays(1)))
                .RuleFor(o => o.PurchaseDate, f => f.Date.Past(1, DateTime.Today));

            _EquipmentFakes.AddRange(_EquipmentFaker.Generate(5));

            int _AvailableCount = _EquipmentFakes.Where(x => x.Status == Status.Available).Count();

            __EquipmentRepositoryMock.Setup(x => x.GetByStatusAsync(It.IsAny<Status>()))
                .ReturnsAsync((Status status) => _EquipmentFakes.Where(x => x.Status == status).ToList());

            // Act
            IList<EquipmentEntity> _Equipment = await __EquipmentRepositoryMock.Object.GetByStatusAsync(Status.Available);

            // Assert
            Assert.Equal(_AvailableCount, _Equipment.Count());
            Assert.Empty(_Equipment.Where(x => x.Status == Status.ActiveLoan));
            Assert.Empty(_Equipment.Where(x => x.Status == Status.WrittenOff));
        }

        [Fact]
        public async Task GetByStatusAsync_ShouldNotReturnEquipment_WhenEquipmentForStatusDoesNotExist()
        {
            // Arrange
            List<EquipmentEntity> _EquipmentFakes = new List<EquipmentEntity>();
            Faker<EquipmentEntity> _EquipmentFaker = new Faker<EquipmentEntity>()
                .RuleFor(o => o.Name, f => f.Commerce.ProductName())
                .RuleFor(o => o.Description, f => f.Commerce.ProductMaterial())
                .RuleFor(o => o.SerialNumber, f => f.Commerce.Ean8())
                .RuleFor(o => o.PurchasePrice, f => f.Random.Number(50, 2000))
                .RuleFor(o => o.ReplacementPrice, f => f.Random.Number(50, 2000))
                .RuleFor(o => o.Status, f => Status.Available)
                .RuleFor(o => o.WarrantyExpirationDate, f => f.Date.Future(5, DateTime.Today.AddDays(1)))
                .RuleFor(o => o.PurchaseDate, f => f.Date.Past(1, DateTime.Today));

            _EquipmentFakes.AddRange(_EquipmentFaker.Generate(5));

            __EquipmentRepositoryMock.Setup(x => x.GetByStatusAsync(It.IsAny<Status>()))
                .ReturnsAsync((Status status) => _EquipmentFakes.Where(x => x.Status == status).ToList());

            // Act
            IList<EquipmentEntity> _Equipment = await __EquipmentRepositoryMock.Object.GetByStatusAsync(Status.Available);

            // Assert
            Assert.Empty(_Equipment.Where(x => x.Status == Status.PendingLoan));
            Assert.Empty(_Equipment.Where(x => x.Status == Status.WrittenOff));
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnEquipment_WhenEquipmentValid()
        {
            // Arrange
            Faker<EquipmentEntity> _EquipmentFaker = new Faker<EquipmentEntity>()
                .RuleFor(o => o.Name, f => f.Commerce.ProductName())
                .RuleFor(o => o.Description, f => f.Commerce.ProductMaterial())
                .RuleFor(o => o.SerialNumber, f => f.Commerce.Ean8())
                .RuleFor(o => o.PurchasePrice, f => f.Random.Number(50, 2000))
                .RuleFor(o => o.ReplacementPrice, f => f.Random.Number(50, 2000))
                .RuleFor(o => o.Status, f => f.PickRandom<Status>())
                .RuleFor(o => o.WarrantyExpirationDate, f => f.Date.Future(5, DateTime.Today.AddDays(1)))
                .RuleFor(o => o.PurchaseDate, f => f.Date.Past(1, DateTime.Today));

            EquipmentEntity _EquipmentFake = _EquipmentFaker.Generate();
            __EquipmentRepositoryMock.Setup(x => x.CreateAsync(_EquipmentFake)).ReturnsAsync(_EquipmentFake);

            // Act
            EquipmentEntity _Equipment = await __EquipmentRepositoryMock.Object.CreateAsync(_EquipmentFake);

            // Assert
            Assert.Equal(_EquipmentFake, _Equipment);
        }

        [Fact]
        public async Task CreateAsync_ShouldNotReturnEquipment_WhenEquipmentNotValid()
        {
            // Arrange
            EquipmentEntity _EquipmentFake = new EquipmentEntity
            {
                UID = Guid.Empty
            };

            __EquipmentRepositoryMock.Setup(x => x.CreateAsync(null)).ReturnsAsync(() => null);
            __EquipmentRepositoryMock.Setup(x => x.CreateAsync(_EquipmentFake)).ReturnsAsync(() => null);

            // Act
            EquipmentEntity _EquipmentNull = await __EquipmentRepositoryMock.Object.CreateAsync(null);
            EquipmentEntity _EquipmentEmptyGuid = await __EquipmentRepositoryMock.Object.CreateAsync(_EquipmentFake);

            // Assert
            Assert.Null(_EquipmentNull);
            Assert.Null(_EquipmentEmptyGuid);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnTrue_WhenEquipmentValid()
        {
            // Arrange
            Faker<EquipmentEntity> _EquipmentFaker = new Faker<EquipmentEntity>()
                .RuleFor(o => o.Name, f => f.Commerce.ProductName())
                .RuleFor(o => o.Description, f => f.Commerce.ProductMaterial())
                .RuleFor(o => o.SerialNumber, f => f.Commerce.Ean8())
                .RuleFor(o => o.PurchasePrice, f => f.Random.Number(50, 2000))
                .RuleFor(o => o.ReplacementPrice, f => f.Random.Number(50, 2000))
                .RuleFor(o => o.Status, f => f.PickRandom<Status>())
                .RuleFor(o => o.WarrantyExpirationDate, f => f.Date.Future(5, DateTime.Today.AddDays(1)))
                .RuleFor(o => o.PurchaseDate, f => f.Date.Past(1, DateTime.Today));

            EquipmentEntity _EquipmentFake = _EquipmentFaker.Generate();

            __EquipmentRepositoryMock.Setup(x => x.UpdateAsync(_EquipmentFake)).ReturnsAsync(true);

            // Act
            bool _Result = await __EquipmentRepositoryMock.Object.UpdateAsync(_EquipmentFake);

            // Assert
            Assert.True(_Result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnFalse_WhenEquipmentGuidInvalid()
        {
            // Arrange
            EquipmentEntity _EquipmentFake = new EquipmentEntity()
            {
                UID = Guid.Empty
            };
            __EquipmentRepositoryMock.Setup(x => x.UpdateAsync(_EquipmentFake)).ReturnsAsync(false);

            // Act
            bool _Result = await __EquipmentRepositoryMock.Object.UpdateAsync(_EquipmentFake);

            // Assert
            Assert.False(_Result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnFalse_WhenEquipmentIsNull()
        {
            // Arrange
            __EquipmentRepositoryMock.Setup(x => x.UpdateAsync(null)).ReturnsAsync(false);

            // Act
            bool _Result = await __EquipmentRepositoryMock.Object.UpdateAsync(null);

            // Assert
            Assert.False(_Result);
        }

        [Fact]
        public async Task BulkCreateAsync_ShouldReturnEquipment_WhenEquipmentValid()
        {
            // Arrange
            Faker<EquipmentEntity> _EquipmentFaker = new Faker<EquipmentEntity>()
                .RuleFor(o => o.Name, f => f.Commerce.ProductName())
                .RuleFor(o => o.Description, f => f.Commerce.ProductMaterial())
                .RuleFor(o => o.SerialNumber, f => f.Commerce.Ean8())
                .RuleFor(o => o.PurchasePrice, f => f.Random.Number(50, 2000))
                .RuleFor(o => o.ReplacementPrice, f => f.Random.Number(50, 2000))
                .RuleFor(o => o.Status, f => f.PickRandom<Status>())
                .RuleFor(o => o.WarrantyExpirationDate, f => f.Date.Future(5, DateTime.Today.AddDays(1)))
                .RuleFor(o => o.PurchaseDate, f => f.Date.Past(1, DateTime.Today));

            EquipmentEntity _EquipmentTemplateFake = _EquipmentFaker.Generate();
            IList<EquipmentEntity> _EquipmentFakes = new List<EquipmentEntity>();
            int _GenerateCount = 5;

            for (int i = 0; i < _GenerateCount; i++)
            {
                _EquipmentFakes.Add(new EquipmentEntity
                {
                    UID = Guid.NewGuid(),
                    CreatedTimestamp = _EquipmentTemplateFake.CreatedTimestamp,
                    AmendedTimestamp = _EquipmentTemplateFake.AmendedTimestamp,
                    WarrantyExpirationDate = _EquipmentTemplateFake.WarrantyExpirationDate,
                    Description = _EquipmentTemplateFake.Description,
                    Name = _EquipmentTemplateFake.Name,
                    OwnerUID = _EquipmentTemplateFake.OwnerUID,
                    PurchaseDate = _EquipmentTemplateFake.PurchaseDate,
                    PurchasePrice = _EquipmentTemplateFake.PurchasePrice,
                    ReplacementPrice = _EquipmentTemplateFake.ReplacementPrice,
                    SerialNumber = _EquipmentTemplateFake.SerialNumber,
                    Status = _EquipmentTemplateFake.Status
                });
            }


            __EquipmentRepositoryMock.Setup(x => x.BulkCreateAsync(_EquipmentTemplateFake, 5)).ReturnsAsync(_EquipmentFakes);

            // Act
            IList<EquipmentEntity> _Equipment = await __EquipmentRepositoryMock.Object.BulkCreateAsync(_EquipmentTemplateFake, 5);

            // Assert
            Assert.True(_Equipment.All(_EquipmentFakes.Contains));
        }

        [Fact]
        public async Task BulkCreateAsync_ShouldNotReturnEquipment_WhenEquipmentInvalid()
        {
            // Arrange
            EquipmentEntity _EquipmentFake = null;
            __EquipmentRepositoryMock.Setup(x => x.BulkCreateAsync(_EquipmentFake, 5)).ReturnsAsync(() => null);

            // Act
            IList<EquipmentEntity> _Equipment = await __EquipmentRepositoryMock.Object.BulkCreateAsync(null, 5);

            // Assert
            Assert.Null(_Equipment);
        }

        [Fact]
        public async Task BulkCreateAsync_ShouldNotReturnEquipment_WhenEquipmentGuidInvalid()
        {
            // Arrange
            EquipmentEntity _Entity = new EquipmentEntity
            {
                UID = Guid.Empty
            };

            __EquipmentRepositoryMock.Setup(x => x.BulkCreateAsync(_Entity, 5)).ReturnsAsync(() => null);

            // Act
            IList<EquipmentEntity> _Equipment = await __EquipmentRepositoryMock.Object.BulkCreateAsync(_Entity, 5);

            // Assert
            Assert.Null(_Equipment);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenEquipmentValid()
        {
            // Arrange
            Guid _EquipmentUID = Guid.NewGuid();

            __EquipmentRepositoryMock.Setup(x => x.DeleteAsync(_EquipmentUID)).ReturnsAsync(true);

            // Act
            bool _Result = await __EquipmentRepositoryMock.Object.DeleteAsync(_EquipmentUID);

            // Assert
            Assert.True(_Result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenEquipmentUIDInvalid()
        {
            // Arrange
            Guid _EquipmentUID = Guid.Empty;

            __EquipmentRepositoryMock.Setup(x => x.DeleteAsync(_EquipmentUID)).ReturnsAsync(false);

            // Act
            bool _Result = await __EquipmentRepositoryMock.Object.DeleteAsync(_EquipmentUID);

            // Assert
            Assert.False(_Result);
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldReturnTrue_WhenEquipmentExists()
        {
            // Arrange
            Guid _EquipmentUID = Guid.NewGuid();

            EquipmentEntity _EquipmentDto = new EquipmentEntity
            {
                UID = _EquipmentUID,
                Name = "TestName",
                WarrantyExpirationDate = DateTime.Today,
                Description = "TestDescription",
                PurchaseDate = DateTime.Today,
                PurchasePrice = 1000,
                ReplacementPrice = 1000,
                SerialNumber = "TestSerialNumber",
                Status = Status.Available,
                OwnerUID = Guid.NewGuid()
            };

            __EquipmentRepositoryMock.Setup(x => x.UpdateStatusAsync(_EquipmentUID, Status.ActiveLoan)).ReturnsAsync(true);

            // Act
            bool _Result = await __EquipmentRepositoryMock.Object.UpdateStatusAsync(_EquipmentUID, Status.ActiveLoan);

            // Assert
            Assert.True(_Result);
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldReturnFalse_WhenEquipmentDoesNotExist()
        {
            // Arrange
            Guid _EquipmentUID = Guid.Empty;

            __EquipmentRepositoryMock.Setup(x => x.UpdateStatusAsync(_EquipmentUID, Status.ActiveLoan)).ReturnsAsync(false);

            // Act
            bool _Result = await __EquipmentRepositoryMock.Object.UpdateStatusAsync(Guid.Empty, Status.ActiveLoan);

            // Assert
            Assert.False(_Result);
        }
    }
}
