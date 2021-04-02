using Bogus;
using ELMS.WEB.Entities.General;
using ELMS.WEB.Repositories.General.Interface;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ELMS.XUNIT.General
{
    public class BlobUnitTest
    {
        private readonly Mock<IBlobRepository> __BlobRepository = new Mock<IBlobRepository>();

        [Fact]
        public async Task CreateAsync_ShouldReturnBlob_WhenBlobIsValid()
        {
            // Arrange
            Faker<BlobEntity> _BlobFaker = new Faker<BlobEntity>()
                .RuleFor(o => o.Name, f => f.System.FileName())
                .RuleFor(o => o.Path, f => f.Image.PicsumUrl());

            BlobEntity _BlobFake = _BlobFaker.Generate();

            __BlobRepository.Setup(x => x.CreateAsync(_BlobFake)).ReturnsAsync(_BlobFake);

            // Act
            BlobEntity _Blob = await __BlobRepository.Object.CreateAsync(_BlobFake);

            // Assert
            Assert.Equal(_BlobFake, _Blob);
        }

        [Fact]
        public async Task CreateAsync_ShouldNotReturnBlob_WhenBlobIsNotValid()
        {
            // Arrange
            __BlobRepository.Setup(x => x.CreateAsync(It.Is<BlobEntity>(x => x.UID == Guid.Empty))).ReturnsAsync(() => null);

            // Act
            BlobEntity _Blob = await __BlobRepository.Object.CreateAsync(new BlobEntity { UID = Guid.Empty });

            // Assert
            Assert.Null(_Blob);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnBlobs_WhenBlobsExists()
        {
            // Arrange
            Faker<BlobEntity> _BlobFaker = new Faker<BlobEntity>()
                .RuleFor(o => o.Name, f => f.System.FileName())
                .RuleFor(o => o.Path, f => f.Image.PicsumUrl());

            IList<BlobEntity> _BlobFakes = _BlobFaker.Generate(5);

            __BlobRepository.Setup(x => x.GetAsync()).ReturnsAsync(_BlobFakes);

            // Act
            IList<BlobEntity> _Blobs = await __BlobRepository.Object.GetAsync();

            // Assert
            Assert.Equal(5, _Blobs.Count());
        }

        [Fact]
        public async Task GetAsync_ShouldNotReturnBlobs_WhenBlobsDoNotExist()
        {
            // Arrange
            __BlobRepository.Setup(x => x.GetAsync()).ReturnsAsync(() => null);

            // Act
            IList<BlobEntity> _Blobs = await __BlobRepository.Object.GetAsync();

            // Assert
            Assert.Null(_Blobs);
        }

        [Fact]
        public async Task GetByUIDAsync_ShouldReturnBlob_WhenBlobExists()
        {
            // Arrange
            Faker<BlobEntity> _BlobFaker = new Faker<BlobEntity>()
                .RuleFor(o => o.Name, f => f.System.FileName())
                .RuleFor(o => o.Path, f => f.Image.PicsumUrl());

            IList<BlobEntity> _BlobFakes = _BlobFaker.Generate(5);

            __BlobRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync((Guid uid) => _BlobFakes.FirstOrDefault(x => x.UID == uid));

            // Act
            BlobEntity _Blob = await __BlobRepository.Object.GetAsync(_BlobFakes[0].UID);

            // Assert
            Assert.Equal(_BlobFakes[0], _Blob);
        }

        [Fact]
        public async Task GetByUIDAsync_ShouldNotReturnBlob_WhenBlobDoesNotExist()
        {
            // Arrange
            Faker<BlobEntity> _BlobFaker = new Faker<BlobEntity>()
                .RuleFor(o => o.Name, f => f.System.FileName())
                .RuleFor(o => o.Path, f => f.Image.PicsumUrl());

            IList<BlobEntity> _BlobFakes = _BlobFaker.Generate(5);

            __BlobRepository.Setup(x => x.GetAsync(Guid.Empty)).ReturnsAsync(() => null);
            __BlobRepository.Setup(x => x.GetAsync(It.Is<Guid>(x => !_BlobFakes.Select(b => b.UID).Contains(x)))).ReturnsAsync(() => null);

            // Act
            BlobEntity _ResultEmptyGuid = await __BlobRepository.Object.GetAsync(Guid.Empty);
            BlobEntity _ResultNonExisting = await __BlobRepository.Object.GetAsync(Guid.NewGuid());

            // Assert
            Assert.Null(_ResultEmptyGuid);
            Assert.Null(_ResultNonExisting);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnBlob_WhenBlobExists()
        {
            // Arrange
            Faker<BlobEntity> _BlobFaker = new Faker<BlobEntity>()
                .RuleFor(o => o.Name, f => f.System.FileName())
                .RuleFor(o => o.Path, f => f.Image.PicsumUrl());

            IList<BlobEntity> _BlobFakes = _BlobFaker.Generate(5);

            __BlobRepository.Setup(x => x.DeleteAsync(It.Is<Guid>(x => _BlobFakes.Select(b => b.UID).Contains(x)))).ReturnsAsync(true);

            // Act
            bool _Result = await __BlobRepository.Object.DeleteAsync(_BlobFakes[0].UID);

            // Assert
            Assert.True(_Result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldNotReturnBlob_WhenBlobDoesNotExist()
        {
            // Arrange
            Faker<BlobEntity> _BlobFaker = new Faker<BlobEntity>()
                .RuleFor(o => o.Name, f => f.System.FileName())
                .RuleFor(o => o.Path, f => f.Image.PicsumUrl());

            IList<BlobEntity> _BlobFakes = _BlobFaker.Generate(5);

            __BlobRepository.Setup(x => x.DeleteAsync(Guid.Empty)).ReturnsAsync(false);
            __BlobRepository.Setup(x => x.DeleteAsync(It.Is<Guid>(x => !_BlobFakes.Select(b => b.UID).Contains(x)))).ReturnsAsync(false);

            // Act
            bool _ResultEmptyGuid = await __BlobRepository.Object.DeleteAsync(Guid.Empty);
            bool _ResultNonExisting = await __BlobRepository.Object.DeleteAsync(Guid.NewGuid());

            // Assert
            Assert.False(_ResultEmptyGuid);
            Assert.False(_ResultNonExisting);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnTrue_WhenBlobIsValid()
        {
            // Arrange
            Faker<BlobEntity> _BlobFaker = new Faker<BlobEntity>()
                .RuleFor(o => o.Name, f => f.System.FileName())
                .RuleFor(o => o.Path, f => f.Image.PicsumUrl());

            IList<BlobEntity> _BlobFakes = _BlobFaker.Generate(5);

            __BlobRepository.Setup(x => x.UpdateAsync(It.Is<BlobEntity>(x => _BlobFakes.Contains(x)))).ReturnsAsync(true);

            // Act
            bool _Result = await __BlobRepository.Object.UpdateAsync(_BlobFakes[0]);

            // Assert
            Assert.True(_Result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnFalse_WhenBlobIsNotValid()
        {
            // Arrange
            Faker<BlobEntity> _BlobFaker = new Faker<BlobEntity>()
                .RuleFor(o => o.Name, f => f.System.FileName())
                .RuleFor(o => o.Path, f => f.Image.PicsumUrl());

            IList<BlobEntity> _BlobFakes = _BlobFaker.Generate(5);

            __BlobRepository.Setup(x => x.UpdateAsync(It.Is<BlobEntity>(x => x.UID == Guid.Empty))).ReturnsAsync(false);
            __BlobRepository.Setup(x => x.UpdateAsync(It.Is<BlobEntity>(x => !_BlobFakes.Contains(x)))).ReturnsAsync(false);

            // Act
            bool _ResultEmptyGuid = await __BlobRepository.Object.UpdateAsync(new BlobEntity { UID = Guid.Empty });
            bool _ResultNonExisting = await __BlobRepository.Object.UpdateAsync(_BlobFaker.Generate());

            // Assert
            Assert.False(_ResultEmptyGuid);
            Assert.False(_ResultNonExisting);
        }
    }
}
