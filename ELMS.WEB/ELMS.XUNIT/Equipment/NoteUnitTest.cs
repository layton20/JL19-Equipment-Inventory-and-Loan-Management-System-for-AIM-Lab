using Bogus;
using ELMS.WEB.Entities.Equipment;
using ELMS.WEB.Enums.Equipment;
using ELMS.WEB.Repositories.Equipment.Interfaces;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ELMS.XUNIT.Equipment
{
    public class NoteUnitTest
    {
        private readonly Mock<INoteRepository> __NoteRepositoryMock = new Mock<INoteRepository>();

        [Fact]
        public async Task GetByUIDAsync_ShouldReturnNote_WhenNoteExists()
        {
            // Assert
            Guid _NoteUID = Guid.NewGuid();

            Faker<EquipmentEntity> _EquipmentFaker = new Faker<EquipmentEntity>()
                .RuleFor(o => o.Name, f => f.Commerce.ProductName())
                .RuleFor(o => o.Description, f => f.Commerce.ProductMaterial())
                .RuleFor(o => o.SerialNumber, f => f.Commerce.Ean8())
                .RuleFor(o => o.PurchasePrice, f => f.Random.Number(50, 2000))
                .RuleFor(o => o.ReplacementPrice, f => f.Random.Number(50, 2000))
                .RuleFor(o => o.Status, f => f.PickRandom<Status>())
                .RuleFor(o => o.WarrantyExpirationDate, f => f.Date.Future(5, DateTime.Today.AddDays(1)))
                .RuleFor(o => o.PurchaseDate, f => f.Date.Past(1, DateTime.Today));

            Faker<IdentityUser> _IdentityFaker = new Faker<IdentityUser>()
                .RuleFor(o => o.Email, f => f.Person.Email)
                .RuleFor(o => o.UserName, f => f.Person.UserName)
                .RuleFor(o => o.EmailConfirmed, f => f.Random.Bool())
                .RuleFor(o => o.Id, f => f.Random.Guid().ToString());

            EquipmentEntity _EquipmentFake = _EquipmentFaker.Generate();
            IdentityUser _UserFake = _IdentityFaker.Generate();

            Faker<NoteEntity> _NoteFaker = new Faker<NoteEntity>()
                .RuleFor(o => o.UID, f => _NoteUID)
                .RuleFor(o => o.Name, f => f.Lorem.Sentence())
                .RuleFor(o => o.Description, f => f.Lorem.Sentence(10, 20))
                .RuleFor(o => o.EquipmentUID, f => _EquipmentFake.UID)
                .RuleFor(o => o.Equipment, f => _EquipmentFake)
                .RuleFor(o => o.OwnerUID, f => _UserFake.Id)
                .RuleFor(o => o.Owner, f => _UserFake);

            NoteEntity _NoteFake = _NoteFaker.Generate();

            __NoteRepositoryMock.Setup(x => x.GetByUIDAsync(_NoteUID)).ReturnsAsync(_NoteFake);

            // Arrange
            NoteEntity _Note = await __NoteRepositoryMock.Object.GetByUIDAsync(_NoteUID);

            // Act
            Assert.NotNull(_Note);
            Assert.Equal(_NoteUID, _Note.UID);
        }

        [Fact]
        public async Task GetByUIDAsync_ShouldNotReturnNote_WhenNoteDoesNotExist()
        {
            // Assert
            __NoteRepositoryMock.Setup(x => x.GetByUIDAsync(It.IsAny<Guid>())).ReturnsAsync(() => null);

            // Arrange
            NoteEntity _Note = await __NoteRepositoryMock.Object.GetByUIDAsync(Guid.NewGuid());

            // Act
            Assert.Null(_Note);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnNotes_WhenNotesExist()
        {
            // Assert
            Faker<EquipmentEntity> _EquipmentFaker = new Faker<EquipmentEntity>()
                .RuleFor(o => o.Name, f => f.Commerce.ProductName())
                .RuleFor(o => o.Description, f => f.Commerce.ProductMaterial())
                .RuleFor(o => o.SerialNumber, f => f.Commerce.Ean8())
                .RuleFor(o => o.PurchasePrice, f => f.Random.Number(50, 2000))
                .RuleFor(o => o.ReplacementPrice, f => f.Random.Number(50, 2000))
                .RuleFor(o => o.Status, f => f.PickRandom<Status>())
                .RuleFor(o => o.WarrantyExpirationDate, f => f.Date.Future(5, DateTime.Today.AddDays(1)))
                .RuleFor(o => o.PurchaseDate, f => f.Date.Past(1, DateTime.Today));

            Faker<IdentityUser> _IdentityFaker = new Faker<IdentityUser>()
                .RuleFor(o => o.Email, f => f.Person.Email)
                .RuleFor(o => o.UserName, f => f.Person.UserName)
                .RuleFor(o => o.EmailConfirmed, f => f.Random.Bool())
                .RuleFor(o => o.Id, f => f.Random.Guid().ToString());

            EquipmentEntity _EquipmentFake = _EquipmentFaker.Generate();
            IdentityUser _UserFake = _IdentityFaker.Generate();

            Faker<NoteEntity> _NoteFaker = new Faker<NoteEntity>()
                .RuleFor(o => o.UID, f => f.Random.Guid())
                .RuleFor(o => o.Name, f => f.Lorem.Sentence())
                .RuleFor(o => o.Description, f => f.Lorem.Sentence(10, 20))
                .RuleFor(o => o.EquipmentUID, f => _EquipmentFake.UID)
                .RuleFor(o => o.Equipment, f => _EquipmentFake)
                .RuleFor(o => o.OwnerUID, f => _UserFake.Id)
                .RuleFor(o => o.Owner, f => _UserFake);

            IList<NoteEntity> _NoteFakes = _NoteFaker.Generate(5);
            __NoteRepositoryMock.Setup(x => x.GetAsync(_EquipmentFake.UID)).ReturnsAsync(_NoteFakes);

            // Arrange
            IList<NoteEntity> _Notes = await __NoteRepositoryMock.Object.GetAsync(_EquipmentFake.UID);

            // Act
            Assert.True(_Notes.All(_NoteFakes.Contains));
        }

        [Fact]
        public async Task GetAsync_ShouldNotReturnNotes_WhenNotesDoesNotExist()
        {
            // Assert
            __NoteRepositoryMock.Setup(x => x.GetByUIDAsync(It.IsAny<Guid>())).ReturnsAsync(() => null);

            // Arrange
            IList<NoteEntity> _Notes = await __NoteRepositoryMock.Object.GetAsync(Guid.NewGuid());

            // Act
            Assert.Null(_Notes);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenNoteExists()
        {
            // Arrange
            __NoteRepositoryMock.Setup(x => x.DeleteAsync(It.Is<Guid>(x => x == Guid.Empty))).ReturnsAsync(false);
            __NoteRepositoryMock.Setup(x => x.DeleteAsync(It.Is<Guid>(x => x != Guid.Empty))).ReturnsAsync(true);

            // Act
            bool _ResultValidGuid = await __NoteRepositoryMock.Object.DeleteAsync(Guid.NewGuid());
            bool _ResultEmptyGuid = await __NoteRepositoryMock.Object.DeleteAsync(Guid.Empty);

            // Assert
            Assert.True(_ResultValidGuid);
            Assert.False(_ResultEmptyGuid);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnTrue_WhenNoteExists()
        {
            // Assert
            Faker<EquipmentEntity> _EquipmentFaker = new Faker<EquipmentEntity>()
                .RuleFor(o => o.Name, f => f.Commerce.ProductName())
                .RuleFor(o => o.Description, f => f.Commerce.ProductMaterial())
                .RuleFor(o => o.SerialNumber, f => f.Commerce.Ean8())
                .RuleFor(o => o.PurchasePrice, f => f.Random.Number(50, 2000))
                .RuleFor(o => o.ReplacementPrice, f => f.Random.Number(50, 2000))
                .RuleFor(o => o.Status, f => f.PickRandom<Status>())
                .RuleFor(o => o.WarrantyExpirationDate, f => f.Date.Future(5, DateTime.Today.AddDays(1)))
                .RuleFor(o => o.PurchaseDate, f => f.Date.Past(1, DateTime.Today));

            Faker<IdentityUser> _IdentityFaker = new Faker<IdentityUser>()
                .RuleFor(o => o.Email, f => f.Person.Email)
                .RuleFor(o => o.UserName, f => f.Person.UserName)
                .RuleFor(o => o.EmailConfirmed, f => f.Random.Bool())
                .RuleFor(o => o.Id, f => f.Random.Guid().ToString());

            EquipmentEntity _EquipmentFake = _EquipmentFaker.Generate();
            IdentityUser _UserFake = _IdentityFaker.Generate();

            Faker<NoteEntity> _NoteFaker = new Faker<NoteEntity>()
                .RuleFor(o => o.UID, f => f.Random.Guid())
                .RuleFor(o => o.Name, f => f.Lorem.Sentence())
                .RuleFor(o => o.Description, f => f.Lorem.Sentence(10, 20))
                .RuleFor(o => o.EquipmentUID, f => _EquipmentFake.UID)
                .RuleFor(o => o.Equipment, f => _EquipmentFake)
                .RuleFor(o => o.OwnerUID, f => _UserFake.Id)
                .RuleFor(o => o.Owner, f => _UserFake);

            IList<NoteEntity> _NoteFakes = _NoteFaker.Generate(3);

            __NoteRepositoryMock.Setup(x => x.UpdateAsync(It.Is<NoteEntity>(x => _NoteFakes.Contains(x)))).ReturnsAsync(true);
            __NoteRepositoryMock.Setup(x => x.UpdateAsync(It.Is<NoteEntity>(x => x.UID == Guid.Empty || !_NoteFakes.Contains(x)))).ReturnsAsync(false);

            // Arrange
            bool _UpdateExistingNote0Result = await __NoteRepositoryMock.Object.UpdateAsync(_NoteFakes[0]);
            bool _UpdateExistingNote1Result = await __NoteRepositoryMock.Object.UpdateAsync(_NoteFakes[1]);
            bool _UpdateExistingNote2Result = await __NoteRepositoryMock.Object.UpdateAsync(_NoteFakes[2]);

            // Act
            Assert.True(_UpdateExistingNote0Result);
            Assert.True(_UpdateExistingNote1Result);
            Assert.True(_UpdateExistingNote2Result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnFalse_WhenNoteDoesNotExist()
        {
            // Assert
            Faker<EquipmentEntity> _EquipmentFaker = new Faker<EquipmentEntity>()
                .RuleFor(o => o.Name, f => f.Commerce.ProductName())
                .RuleFor(o => o.Description, f => f.Commerce.ProductMaterial())
                .RuleFor(o => o.SerialNumber, f => f.Commerce.Ean8())
                .RuleFor(o => o.PurchasePrice, f => f.Random.Number(50, 2000))
                .RuleFor(o => o.ReplacementPrice, f => f.Random.Number(50, 2000))
                .RuleFor(o => o.Status, f => f.PickRandom<Status>())
                .RuleFor(o => o.WarrantyExpirationDate, f => f.Date.Future(5, DateTime.Today.AddDays(1)))
                .RuleFor(o => o.PurchaseDate, f => f.Date.Past(1, DateTime.Today));

            Faker<IdentityUser> _IdentityFaker = new Faker<IdentityUser>()
                .RuleFor(o => o.Email, f => f.Person.Email)
                .RuleFor(o => o.UserName, f => f.Person.UserName)
                .RuleFor(o => o.EmailConfirmed, f => f.Random.Bool())
                .RuleFor(o => o.Id, f => f.Random.Guid().ToString());

            EquipmentEntity _EquipmentFake = _EquipmentFaker.Generate();
            IdentityUser _UserFake = _IdentityFaker.Generate();

            Faker<NoteEntity> _NoteFaker = new Faker<NoteEntity>()
                .RuleFor(o => o.UID, f => f.Random.Guid())
                .RuleFor(o => o.Name, f => f.Lorem.Sentence())
                .RuleFor(o => o.Description, f => f.Lorem.Sentence(10, 20))
                .RuleFor(o => o.EquipmentUID, f => _EquipmentFake.UID)
                .RuleFor(o => o.Equipment, f => _EquipmentFake)
                .RuleFor(o => o.OwnerUID, f => _UserFake.Id)
                .RuleFor(o => o.Owner, f => _UserFake);

            IList<NoteEntity> _NoteFakes = _NoteFaker.Generate(5);
            __NoteRepositoryMock.Setup(x => x.UpdateAsync(It.Is<NoteEntity>(x => _NoteFakes.Contains(x)))).ReturnsAsync(true);
            __NoteRepositoryMock.Setup(x => x.UpdateAsync(It.Is<NoteEntity>(x => x.UID == Guid.Empty || !_NoteFakes.Contains(x)))).ReturnsAsync(false);

            // Arrange
            bool _UpdateNonExistingNoteResult = await __NoteRepositoryMock.Object.UpdateAsync(_NoteFaker.Generate());

            // Act
            Assert.False(_UpdateNonExistingNoteResult);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnTrue_WhenNoteIsValid()
        {
            // Assert
            Faker<EquipmentEntity> _EquipmentFaker = new Faker<EquipmentEntity>()
                .RuleFor(o => o.Name, f => f.Commerce.ProductName())
                .RuleFor(o => o.Description, f => f.Commerce.ProductMaterial())
                .RuleFor(o => o.SerialNumber, f => f.Commerce.Ean8())
                .RuleFor(o => o.PurchasePrice, f => f.Random.Number(50, 2000))
                .RuleFor(o => o.ReplacementPrice, f => f.Random.Number(50, 2000))
                .RuleFor(o => o.Status, f => f.PickRandom<Status>())
                .RuleFor(o => o.WarrantyExpirationDate, f => f.Date.Future(5, DateTime.Today.AddDays(1)))
                .RuleFor(o => o.PurchaseDate, f => f.Date.Past(1, DateTime.Today));

            Faker<IdentityUser> _IdentityFaker = new Faker<IdentityUser>()
                .RuleFor(o => o.Email, f => f.Person.Email)
                .RuleFor(o => o.UserName, f => f.Person.UserName)
                .RuleFor(o => o.EmailConfirmed, f => f.Random.Bool())
                .RuleFor(o => o.Id, f => f.Random.Guid().ToString());

            EquipmentEntity _EquipmentFake = _EquipmentFaker.Generate();
            IdentityUser _UserFake = _IdentityFaker.Generate();

            Faker<NoteEntity> _NoteFaker = new Faker<NoteEntity>()
                .RuleFor(o => o.UID, f => f.Random.Guid())
                .RuleFor(o => o.Name, f => f.Lorem.Sentence())
                .RuleFor(o => o.Description, f => f.Lorem.Sentence(10, 20))
                .RuleFor(o => o.EquipmentUID, f => _EquipmentFake.UID)
                .RuleFor(o => o.Equipment, f => _EquipmentFake)
                .RuleFor(o => o.OwnerUID, f => _UserFake.Id)
                .RuleFor(o => o.Owner, f => _UserFake);

            NoteEntity _NoteFake = _NoteFaker.Generate();

            __NoteRepositoryMock.Setup(x => x.CreateAsync(_NoteFake)).ReturnsAsync(_NoteFake);

            // Arrange
            NoteEntity _Note = await __NoteRepositoryMock.Object.CreateAsync(_NoteFake);

            // Act
            Assert.Equal(_NoteFake.UID, _Note.UID);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnFalse_WhenNoteGuidIsInvalid()
        {
            // Arrange
            NoteEntity _NoteFake = new NoteEntity()
            {
                UID = Guid.Empty
            };

            __NoteRepositoryMock.Setup(x => x.CreateAsync(It.Is<NoteEntity>(x => x.UID == Guid.Empty))).ReturnsAsync(() => null);
            __NoteRepositoryMock.Setup(x => x.CreateAsync(null)).ReturnsAsync(() => null);

            // Act
            NoteEntity _NoteEmptyGuid = await __NoteRepositoryMock.Object.CreateAsync(_NoteFake);
            NoteEntity _NoteNull = await __NoteRepositoryMock.Object.CreateAsync(null);

            // Assert
            Assert.Null(_NoteEmptyGuid);
            Assert.Null(_NoteNull);

        }
    }
}
