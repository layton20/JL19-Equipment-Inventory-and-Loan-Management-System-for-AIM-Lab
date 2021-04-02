using Bogus;
using ELMS.WEB.Entities.Admin;
using ELMS.WEB.Enums.Admin;
using ELMS.WEB.Repositories.Admin.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ELMS.XUNIT.Admin
{
    public class BlacklistUnitTest
    {
        private readonly Mock<IBlacklistRepository> __BlacklistRepositoryMock = new Mock<IBlacklistRepository>();

        [Fact]
        public async Task CreateAsync_ShouldReturnBlacklist_WhenBlacklistIsValid()
        {
            // Arrange
            Faker<BlacklistEntity> _BlacklistFaker = new Faker<BlacklistEntity>()
                .RuleFor(o => o.Email, f => f.Person.Email)
                .RuleFor(o => o.Reason, f => f.Lorem.Sentence())
                .RuleFor(o => o.Active, f => f.Random.Bool())
                .RuleFor(o => o.Type, f => f.PickRandom<BlacklistTypeEnum>());

            BlacklistEntity _BlacklistFake = _BlacklistFaker.Generate();


            __BlacklistRepositoryMock.Setup(x => x.CreateAsync(_BlacklistFake)).ReturnsAsync(_BlacklistFake);

            // Act
            BlacklistEntity _Blacklist = await __BlacklistRepositoryMock.Object.CreateAsync(_BlacklistFake);

            // Assert
            Assert.Equal(_BlacklistFake, _Blacklist);
        }

        [Fact]
        public async Task CreateAsync_ShouldNotReturnBlacklist_WhenBlacklistIsNotValid()
        {
            // Arrange
            __BlacklistRepositoryMock.Setup(x => x.CreateAsync(It.Is<BlacklistEntity>(x => x.UID == Guid.Empty))).ReturnsAsync(() => null);

            // Act
            BlacklistEntity _Blacklist = await __BlacklistRepositoryMock.Object.CreateAsync(new BlacklistEntity { UID = Guid.Empty });

            // Assert
            Assert.Null(_Blacklist);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenBlacklistExists()
        {
            // Arrange
            Faker<BlacklistEntity> _BlacklistFaker = new Faker<BlacklistEntity>()
                .RuleFor(o => o.Email, f => f.Person.Email)
                .RuleFor(o => o.Reason, f => f.Lorem.Sentence())
                .RuleFor(o => o.Active, f => f.Random.Bool())
                .RuleFor(o => o.Type, f => f.PickRandom<BlacklistTypeEnum>());

            IList<BlacklistEntity> _BlacklistFakes = _BlacklistFaker.Generate(5);

            __BlacklistRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync((Guid uid) => _BlacklistFakes.Select(x => x.UID).Contains(uid));

            // Act
            bool _Result = await __BlacklistRepositoryMock.Object.DeleteAsync(_BlacklistFakes[0].UID);

            // Assert
            Assert.True(_Result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenBlacklistDoesNotExist()
        {
            // Arrange
            Faker<BlacklistEntity> _BlacklistFaker = new Faker<BlacklistEntity>()
                .RuleFor(o => o.Email, f => f.Person.Email)
                .RuleFor(o => o.Reason, f => f.Lorem.Sentence())
                .RuleFor(o => o.Active, f => f.Random.Bool())
                .RuleFor(o => o.Type, f => f.PickRandom<BlacklistTypeEnum>());

            IList<BlacklistEntity> _BlacklistFakes = _BlacklistFaker.Generate(5);

            __BlacklistRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync((Guid uid) => _BlacklistFakes.Select(x => x.UID).Contains(uid));

            // Act
            bool _Result = await __BlacklistRepositoryMock.Object.DeleteAsync(Guid.NewGuid());

            // Assert
            Assert.False(_Result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenBlacklistIsInvalid()
        {
            // Arrange
            __BlacklistRepositoryMock.Setup(x => x.DeleteAsync(Guid.Empty)).ReturnsAsync(false);

            // Act
            bool _Result = await __BlacklistRepositoryMock.Object.DeleteAsync(Guid.Empty);

            // Assert
            Assert.False(_Result);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnBlacklists_WhenBlacklistsExists()
        {
            // Arrange
            Faker<BlacklistEntity> _BlacklistFaker = new Faker<BlacklistEntity>()
                .RuleFor(o => o.Email, f => f.Person.Email)
                .RuleFor(o => o.Reason, f => f.Lorem.Sentence())
                .RuleFor(o => o.Active, f => f.Random.Bool())
                .RuleFor(o => o.Type, f => f.PickRandom<BlacklistTypeEnum>());

            IList<BlacklistEntity> _BlacklistFakes = _BlacklistFaker.Generate(5);

            __BlacklistRepositoryMock.Setup(x => x.GetAsync()).ReturnsAsync(_BlacklistFakes);

            // Act
            IList<BlacklistEntity> _Blacklists = await __BlacklistRepositoryMock.Object.GetAsync();

            // Assert
            Assert.Equal(_BlacklistFakes, _Blacklists);
        }

        [Fact]
        public async Task GetByUIDAsync_ShouldReturnBlacklist_WhenBlacklistExists()
        {
            // Arrange
            Faker<BlacklistEntity> _BlacklistFaker = new Faker<BlacklistEntity>()
                .RuleFor(o => o.Email, f => f.Person.Email)
                .RuleFor(o => o.Reason, f => f.Lorem.Sentence())
                .RuleFor(o => o.Active, f => f.Random.Bool())
                .RuleFor(o => o.Type, f => f.PickRandom<BlacklistTypeEnum>());

            IList<BlacklistEntity> _BlacklistFakes = _BlacklistFaker.Generate(5);

            __BlacklistRepositoryMock.Setup(x => x.GetByUIDAsync(It.IsAny<Guid>())).ReturnsAsync((Guid uid) => _BlacklistFakes.FirstOrDefault(x => x.UID == uid));

            // Act
            BlacklistEntity _Blacklist = await __BlacklistRepositoryMock.Object.GetByUIDAsync(_BlacklistFakes[0].UID);

            // Assert
            Assert.Equal(_BlacklistFakes[0], _Blacklist);
        }

        [Fact]
        public async Task GetByUIDAsync_ShouldNotReturnBlacklist_WhenBlacklistDoesNotExist()
        {
            // Arrange
            Faker<BlacklistEntity> _BlacklistFaker = new Faker<BlacklistEntity>()
                .RuleFor(o => o.Email, f => f.Person.Email)
                .RuleFor(o => o.Reason, f => f.Lorem.Sentence())
                .RuleFor(o => o.Active, f => f.Random.Bool())
                .RuleFor(o => o.Type, f => f.PickRandom<BlacklistTypeEnum>());

            IList<BlacklistEntity> _BlacklistFakes = _BlacklistFaker.Generate(5);

            __BlacklistRepositoryMock.Setup(x => x.GetByUIDAsync(Guid.Empty)).ReturnsAsync(() => null);
            __BlacklistRepositoryMock.Setup(x => x.GetByUIDAsync(It.Is<Guid>(x => _BlacklistFakes.Select(b => b.UID).Contains(x)))).ReturnsAsync(() => null);

            // Act
            BlacklistEntity _BlacklistEmptyUID = await __BlacklistRepositoryMock.Object.GetByUIDAsync(Guid.Empty);
            BlacklistEntity _BlacklistNonExisting = await __BlacklistRepositoryMock.Object.GetByUIDAsync(Guid.NewGuid());

            // Assert
            Assert.Null(_BlacklistEmptyUID);
            Assert.Null(_BlacklistNonExisting);
        }

        [Fact]
        public async Task GetStateAsync_ShouldReturnState_WhenBlacklistExists()
        {
            // Arrange
            string _Email = $"{Guid.NewGuid()}@gmail.com";

            BlacklistEntity _BlacklistFake = new BlacklistEntity
            {
                Email = _Email,
                Reason = "Chipped screen",
                Active = true,
                Type = BlacklistTypeEnum.Loan
            };

            __BlacklistRepositoryMock.Setup(x => x.GetStateAsync(_Email)).ReturnsAsync(BlacklistStateEnum.ActiveBlacklist);

            // Act
            BlacklistStateEnum _State = await __BlacklistRepositoryMock.Object.GetStateAsync(_Email);

            // Assert
            Assert.Equal(BlacklistStateEnum.ActiveBlacklist, _State);
        }

        [Fact]
        public async Task GetByEmailAsync_ShouldReturnBlacklist_WhenBlacklistExists()
        {
            // Arrange
            string _Email = $"{Guid.NewGuid()}@gmail.com";

            Faker<BlacklistEntity> _BlacklistFaker = new Faker<BlacklistEntity>()
                .RuleFor(o => o.Email, f => _Email)
                .RuleFor(o => o.Reason, f => f.Lorem.Sentence())
                .RuleFor(o => o.Active, f => f.Random.Bool())
                .RuleFor(o => o.Type, f => f.PickRandom<BlacklistTypeEnum>());

            BlacklistEntity _BlacklistFake = _BlacklistFaker.Generate();

            __BlacklistRepositoryMock.Setup(x => x.GetAsync(_Email)).ReturnsAsync(new List<BlacklistEntity> { _BlacklistFake });

            // Act
            IList<BlacklistEntity> _Blacklists = await __BlacklistRepositoryMock.Object.GetAsync(_Email);

            // Assert
            Assert.NotEmpty(_Blacklists);
            Assert.Equal(_BlacklistFake, _Blacklists.First());
        }

        [Fact]
        public async Task GetByEmailAsync_ShouldNotReturnBlacklist_WhenBlacklistDoesNotExist()
        {
            // Arrange
            Faker<BlacklistEntity> _BlacklistFaker = new Faker<BlacklistEntity>()
                .RuleFor(o => o.Email, f => f.Person.Email)
                .RuleFor(o => o.Reason, f => f.Lorem.Sentence())
                .RuleFor(o => o.Active, f => f.Random.Bool())
                .RuleFor(o => o.Type, f => f.PickRandom<BlacklistTypeEnum>());

            IList<BlacklistEntity> _BlacklistFakes = _BlacklistFaker.Generate(6);

            __BlacklistRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync((string email) => _BlacklistFakes.Where(x => x.Email == email).ToList());

            // Act
            IList<BlacklistEntity> _Blacklists = await __BlacklistRepositoryMock.Object.GetAsync($"{Guid.NewGuid()}@gmail.com");

            // Assert
            Assert.Empty(_Blacklists);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnTrue_WhenBlacklistExists()
        {
            // Arrange
            Faker<BlacklistEntity> _BlacklistFaker = new Faker<BlacklistEntity>()
                .RuleFor(o => o.Email, f => f.Person.Email)
                .RuleFor(o => o.Reason, f => f.Lorem.Sentence())
                .RuleFor(o => o.Active, f => f.Random.Bool())
                .RuleFor(o => o.Type, f => f.PickRandom<BlacklistTypeEnum>());

            BlacklistEntity _BlacklistFake = _BlacklistFaker.Generate();

            __BlacklistRepositoryMock.Setup(x => x.UpdateAsync(_BlacklistFake)).ReturnsAsync(true);

            // Act
            bool _Result = await __BlacklistRepositoryMock.Object.UpdateAsync(_BlacklistFake);

            // Assert
            Assert.True(_Result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnFalse_WhenBlacklistDoesNotExist()
        {
            // Arrange
            __BlacklistRepositoryMock.Setup(x => x.UpdateAsync(It.Is<BlacklistEntity>(x => x.UID == Guid.Empty))).ReturnsAsync(false);

            // Act
            bool _Result = await __BlacklistRepositoryMock.Object.UpdateAsync(new BlacklistEntity { UID = Guid.Empty });

            // Assert
            Assert.False(_Result);
        }
    }
}
