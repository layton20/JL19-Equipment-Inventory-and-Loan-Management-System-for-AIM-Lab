using Bogus;
using ELMS.WEB.Entities.Equipment;
using ELMS.WEB.Entities.General;
using ELMS.WEB.Enums.Equipment;
using ELMS.WEB.Repositories.Equipment.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ELMS.XUNIT.Equipment
{
    public class EquipmentBlobUnitTests
    {
        private readonly Mock<IEquipmentBlobRepository> __EquipmentBlobRepository = new Mock<IEquipmentBlobRepository>();

        [Fact]
        public async Task CreateAsync_ShouldReturnBlobs_WhenBlobIsValid()
        {
            // Arrange
            Faker<BlobEntity> _BlobFaker = new Faker<BlobEntity>()
                .RuleFor(o => o.Name, f => f.Commerce.ProductName())
                .RuleFor(o => o.Path, f => f.Image.PicsumUrl());

            Faker<EquipmentEntity> _EquipmentFaker = new Faker<EquipmentEntity>()
                .RuleFor(o => o.Name, f => f.Commerce.ProductName())
                .RuleFor(o => o.Description, f => f.Commerce.ProductMaterial())
                .RuleFor(o => o.SerialNumber, f => f.Commerce.Ean8())
                .RuleFor(o => o.PurchasePrice, f => f.Random.Number(50, 2000))
                .RuleFor(o => o.ReplacementPrice, f => f.Random.Number(50, 2000))
                .RuleFor(o => o.Status, f => f.PickRandom<Status>())
                .RuleFor(o => o.WarrantyExpirationDate, f => f.Date.Future(5, DateTime.Today.AddDays(1)))
                .RuleFor(o => o.PurchaseDate, f => f.Date.Past(1, DateTime.Today));

            EquipmentEntity _Equipment = _EquipmentFaker.Generate();
            BlobEntity _Blob = _BlobFaker.Generate();

            Faker<EquipmentBlobEntity> _EquipmentBlobFaker = new Faker<EquipmentBlobEntity>()
                .RuleFor(o => o.BlobUID, f => _Blob.UID)
                .RuleFor(o => o.Blob, f => _Blob)
                .RuleFor(o => o.EquipmentUID, f => _Equipment.UID)
                .RuleFor(o => o.Equipment, f => _Equipment);

            EquipmentBlobEntity _EquipmentBlobFake = _EquipmentBlobFaker.Generate();

            __EquipmentBlobRepository.Setup(x => x.CreateAsync(_EquipmentBlobFake)).ReturnsAsync(_EquipmentBlobFake);

            // Act
            EquipmentBlobEntity _EquipmentBlob = await __EquipmentBlobRepository.Object.CreateAsync(_EquipmentBlobFake);

            // Assert
            Assert.Equal(_EquipmentBlob.UID, _EquipmentBlobFake.UID);
        }

        [Fact]
        public async Task CreateAsync_ShouldNotReturnBlobs_WhenEquipmentIsNotValid()
        {
            // Arrange
            EquipmentBlobEntity _EquipmentBlobFake = new EquipmentBlobEntity
            {
                UID = Guid.Empty
            };
            __EquipmentBlobRepository.Setup(x => x.CreateAsync(It.Is<EquipmentBlobEntity>(x => x == null || x.UID == Guid.Empty))).ReturnsAsync(() => null);

            // Act
            EquipmentBlobEntity _EquipmentBlobNull = await __EquipmentBlobRepository.Object.CreateAsync(null);
            EquipmentBlobEntity _EquipmentBlobEmptyGuid = await __EquipmentBlobRepository.Object.CreateAsync(_EquipmentBlobFake);

            // Assert
            Assert.Null(_EquipmentBlobNull);
            Assert.Null(_EquipmentBlobEmptyGuid);
        }

        [Fact]
        public async Task GetByUIDAsync_ShouldReturnBlobs_WhenEquipmentExists()
        {
            // Arrange
            Guid _EquipmentBlobUID = Guid.NewGuid();

            Faker<BlobEntity> _BlobFaker = new Faker<BlobEntity>()
            .RuleFor(o => o.Name, f => f.Commerce.ProductName())
            .RuleFor(o => o.Path, f => f.Image.PicsumUrl());

            Faker<EquipmentEntity> _EquipmentFaker = new Faker<EquipmentEntity>()
                .RuleFor(o => o.Name, f => f.Commerce.ProductName())
                .RuleFor(o => o.Description, f => f.Commerce.ProductMaterial())
                .RuleFor(o => o.SerialNumber, f => f.Commerce.Ean8())
                .RuleFor(o => o.PurchasePrice, f => f.Random.Number(50, 2000))
                .RuleFor(o => o.ReplacementPrice, f => f.Random.Number(50, 2000))
                .RuleFor(o => o.Status, f => f.PickRandom<Status>())
                .RuleFor(o => o.WarrantyExpirationDate, f => f.Date.Future(5, DateTime.Today.AddDays(1)))
                .RuleFor(o => o.PurchaseDate, f => f.Date.Past(1, DateTime.Today));

            EquipmentEntity _Equipment = _EquipmentFaker.Generate();
            BlobEntity _Blob = _BlobFaker.Generate();

            Faker<EquipmentBlobEntity> _EquipmentBlobFaker = new Faker<EquipmentBlobEntity>()
                .RuleFor(o => o.UID, f => _EquipmentBlobUID)
                .RuleFor(o => o.BlobUID, f => _Blob.UID)
                .RuleFor(o => o.Blob, f => _Blob)
                .RuleFor(o => o.EquipmentUID, f => _Equipment.UID)
                .RuleFor(o => o.Equipment, f => _Equipment);

            EquipmentBlobEntity _EquipmentBlobFake = _EquipmentBlobFaker.Generate();

            __EquipmentBlobRepository.Setup(x => x.GetByUIDAsync(_EquipmentBlobUID)).ReturnsAsync(_EquipmentBlobFake);

            // Act
            EquipmentBlobEntity _EquipmentBlob = await __EquipmentBlobRepository.Object.GetByUIDAsync(_EquipmentBlobUID);

            // Assert
            Assert.Equal(_EquipmentBlobUID, _EquipmentBlob.UID);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnBlobs_WhenBlobsExists()
        {
            // Arrange
            Faker<BlobEntity> _BlobFaker = new Faker<BlobEntity>()
                .RuleFor(o => o.Name, f => f.Commerce.ProductName())
                .RuleFor(o => o.Path, f => f.Image.PicsumUrl());

            Faker<EquipmentEntity> _EquipmentFaker = new Faker<EquipmentEntity>()
                .RuleFor(o => o.Name, f => f.Commerce.ProductName())
                .RuleFor(o => o.Description, f => f.Commerce.ProductMaterial())
                .RuleFor(o => o.SerialNumber, f => f.Commerce.Ean8())
                .RuleFor(o => o.PurchasePrice, f => f.Random.Number(50, 2000))
                .RuleFor(o => o.ReplacementPrice, f => f.Random.Number(50, 2000))
                .RuleFor(o => o.Status, f => f.PickRandom<Status>())
                .RuleFor(o => o.WarrantyExpirationDate, f => f.Date.Future(5, DateTime.Today.AddDays(1)))
                .RuleFor(o => o.PurchaseDate, f => f.Date.Past(1, DateTime.Today));

            EquipmentEntity _Equipment = _EquipmentFaker.Generate();
            BlobEntity _Blob = _BlobFaker.Generate();

            Faker<EquipmentBlobEntity> _EquipmentBlobFaker = new Faker<EquipmentBlobEntity>()
                .RuleFor(o => o.BlobUID, f => _Blob.UID)
                .RuleFor(o => o.Blob, f => _Blob)
                .RuleFor(o => o.EquipmentUID, f => _Equipment.UID)
                .RuleFor(o => o.Equipment, f => _Equipment);

            IList<EquipmentBlobEntity> _EquipmentBlobFakes = _EquipmentBlobFaker.Generate(5);

            __EquipmentBlobRepository.Setup(x => x.GetAsync()).ReturnsAsync(_EquipmentBlobFakes);

            // Act
            IList<EquipmentBlobEntity> _EquipmentBlobs = await __EquipmentBlobRepository.Object.GetAsync();

            // Assert
            Assert.Equal(5, _EquipmentBlobs.Count());
        }

        [Fact]
        public async Task GetByUIDAsync_ShouldNotReturnBlobs_WhenBlobsDoesNotExist()
        {
            // Arrange
            __EquipmentBlobRepository.Setup(x => x.GetAsync()).ReturnsAsync(() => null);

            // Act
            IList<EquipmentBlobEntity> _EquipmentBlobs = await __EquipmentBlobRepository.Object.GetAsync();

            // Assert
            Assert.Null(_EquipmentBlobs);
        }

        [Fact]
        public async Task GetByEquipmentUIDAsync_ShouldNotReturnBlobs_WhenEquipmentDoesNotExist()
        {
            // Arrange
            Guid _EquipmentUID = Guid.NewGuid();

            Faker<BlobEntity> _BlobFaker = new Faker<BlobEntity>()
                .RuleFor(o => o.Name, f => f.Commerce.ProductName())
                .RuleFor(o => o.Path, f => f.Image.PicsumUrl());

            Faker<EquipmentEntity> _EquipmentFaker = new Faker<EquipmentEntity>()
                .RuleFor(o => o.UID, f => _EquipmentUID)
                .RuleFor(o => o.Name, f => f.Commerce.ProductName())
                .RuleFor(o => o.Description, f => f.Commerce.ProductMaterial())
                .RuleFor(o => o.SerialNumber, f => f.Commerce.Ean8())
                .RuleFor(o => o.PurchasePrice, f => f.Random.Number(50, 2000))
                .RuleFor(o => o.ReplacementPrice, f => f.Random.Number(50, 2000))
                .RuleFor(o => o.Status, f => f.PickRandom<Status>())
                .RuleFor(o => o.WarrantyExpirationDate, f => f.Date.Future(5, DateTime.Today.AddDays(1)))
                .RuleFor(o => o.PurchaseDate, f => f.Date.Past(1, DateTime.Today));

            EquipmentEntity _Equipment = _EquipmentFaker.Generate();
            BlobEntity _Blob = _BlobFaker.Generate();

            Faker<EquipmentBlobEntity> _EquipmentBlobFaker = new Faker<EquipmentBlobEntity>()
                .RuleFor(o => o.BlobUID, f => _Blob.UID)
                .RuleFor(o => o.Blob, f => _Blob)
                .RuleFor(o => o.EquipmentUID, f => _Equipment.UID)
                .RuleFor(o => o.Equipment, f => _Equipment);

            IList<EquipmentBlobEntity> _EquipmentBlobFake = _EquipmentBlobFaker.Generate(5);
            __EquipmentBlobRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync((Guid uid) => _EquipmentBlobFake.Where(x => x.EquipmentUID == uid).ToList());

            // Act
            IList<EquipmentBlobEntity> _NonExistingEquipmentBlobs = await __EquipmentBlobRepository.Object.GetAsync(Guid.NewGuid());

            // Assert
            Assert.Empty(_NonExistingEquipmentBlobs);
        }

        [Fact]
        public async Task GetByEquipmentUIDAsync_ShouldReturnBlobs_WhenEquipmentExists()
        {
            // Arrange
            Guid _EquipmentUID = Guid.NewGuid();

            Faker<BlobEntity> _BlobFaker = new Faker<BlobEntity>()
                .RuleFor(o => o.Name, f => f.Commerce.ProductName())
                .RuleFor(o => o.Path, f => f.Image.PicsumUrl());

            Faker<EquipmentEntity> _EquipmentFaker = new Faker<EquipmentEntity>()
                .RuleFor(o => o.UID, f => _EquipmentUID)
                .RuleFor(o => o.Name, f => f.Commerce.ProductName())
                .RuleFor(o => o.Description, f => f.Commerce.ProductMaterial())
                .RuleFor(o => o.SerialNumber, f => f.Commerce.Ean8())
                .RuleFor(o => o.PurchasePrice, f => f.Random.Number(50, 2000))
                .RuleFor(o => o.ReplacementPrice, f => f.Random.Number(50, 2000))
                .RuleFor(o => o.Status, f => f.PickRandom<Status>())
                .RuleFor(o => o.WarrantyExpirationDate, f => f.Date.Future(5, DateTime.Today.AddDays(1)))
                .RuleFor(o => o.PurchaseDate, f => f.Date.Past(1, DateTime.Today));

            EquipmentEntity _Equipment = _EquipmentFaker.Generate();
            BlobEntity _Blob = _BlobFaker.Generate();

            Faker<EquipmentBlobEntity> _EquipmentBlobFaker = new Faker<EquipmentBlobEntity>()
                .RuleFor(o => o.BlobUID, f => _Blob.UID)
                .RuleFor(o => o.Blob, f => _Blob)
                .RuleFor(o => o.EquipmentUID, f => _Equipment.UID)
                .RuleFor(o => o.Equipment, f => _Equipment);

            IList<EquipmentBlobEntity> _EquipmentBlobFake = _EquipmentBlobFaker.Generate(5);
            __EquipmentBlobRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync((Guid uid) => _EquipmentBlobFake.Where(x => x.EquipmentUID == uid).ToList());

            // Act
            IList<EquipmentBlobEntity> _ExistingEquipmentBlobs = await __EquipmentBlobRepository.Object.GetAsync(_EquipmentUID);

            // Assert
            Assert.Equal(5, _ExistingEquipmentBlobs.Count());
        }
    }
}
