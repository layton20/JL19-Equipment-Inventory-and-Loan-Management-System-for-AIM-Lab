using Bogus;
using ELMS.WEB.Repositories.Identity.Interface;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ELMS.XUNIT.Identity
{
    public class UserUnitTests
    {
        private readonly Mock<IUserRepository> __NoteRepositoryMock = new Mock<IUserRepository>();

        [Fact]
        public async Task GetAsync_ShouldReturnUsers_WhenUsersExists()
        {
            // Arrange
            Faker<IdentityUser> _IdentityFaker = new Faker<IdentityUser>()
                .RuleFor(o => o.Email, f => f.Person.Email)
                .RuleFor(o => o.UserName, f => f.Person.UserName)
                .RuleFor(o => o.EmailConfirmed, f => f.Random.Bool())
                .RuleFor(o => o.Id, f => f.Random.Guid().ToString());

            IList<IdentityUser> _UserFakes = _IdentityFaker.Generate(5);

            __NoteRepositoryMock.Setup(x => x.GetAsync()).ReturnsAsync(_UserFakes);

            // Act
            IList<IdentityUser> _Users = await __NoteRepositoryMock.Object.GetAsync();

            // Assert
            Assert.Equal(_UserFakes, _Users);
        }

        [Fact]
        public async Task GetAsync_ShouldNotReturnUsers_WhenUsersDoNotExist()
        {
            // Arrange
            __NoteRepositoryMock.Setup(x => x.GetAsync()).ReturnsAsync(() => null);

            // Act
            IList<IdentityUser> _Users = await __NoteRepositoryMock.Object.GetAsync();

            // Assert
            Assert.Null(_Users);
        }

        [Fact]
        public async Task GetByEmailAsync_ShouldReturnUsers_WhenUsersExists()
        {
            // Arrange
            Faker<IdentityUser> _IdentityFaker = new Faker<IdentityUser>()
                .RuleFor(o => o.Email, f => f.Person.Email)
                .RuleFor(o => o.UserName, f => f.Person.UserName)
                .RuleFor(o => o.EmailConfirmed, f => f.Random.Bool())
                .RuleFor(o => o.Id, f => f.Random.Guid().ToString());

            IList<IdentityUser> _UserFakes = _IdentityFaker.Generate(5);

            __NoteRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync((string email) => _UserFakes.FirstOrDefault(x => x.Email.ToUpper() == email.ToUpper()));

            // Act
            IdentityUser _User = await __NoteRepositoryMock.Object.GetAsync(_UserFakes[0].Email);

            // Assert
            Assert.Equal(_UserFakes[0], _User);
        }

        [Fact]
        public async Task GetByEmailAsync_ShouldNotReturnUsers_WhenUsersDoNotExist()
        {
            // Arrange
            Faker<IdentityUser> _IdentityFaker = new Faker<IdentityUser>()
                .RuleFor(o => o.Email, f => f.Person.Email)
                .RuleFor(o => o.UserName, f => f.Person.UserName)
                .RuleFor(o => o.EmailConfirmed, f => f.Random.Bool())
                .RuleFor(o => o.Id, f => f.Random.Guid().ToString());

            IList<IdentityUser> _UserFakes = _IdentityFaker.Generate(5);

            __NoteRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync((string email) => _UserFakes.FirstOrDefault(x => x.Email.ToUpper() == email.ToUpper()));

            // Act
            IdentityUser _User = await __NoteRepositoryMock.Object.GetAsync($"{Guid.NewGuid()}");

            // Assert
            Assert.Null(_User);
        }

        [Fact]
        public async Task GetByUIDAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            Faker<IdentityUser> _IdentityFaker = new Faker<IdentityUser>()
                .RuleFor(o => o.Email, f => f.Person.Email)
                .RuleFor(o => o.UserName, f => f.Person.UserName)
                .RuleFor(o => o.EmailConfirmed, f => f.Random.Bool())
                .RuleFor(o => o.Id, f => f.Random.Guid().ToString());

            IList<IdentityUser> _UserFakes = _IdentityFaker.Generate(5);

            __NoteRepositoryMock.Setup(x => x.GetByUIDAsync(It.IsAny<Guid>())).ReturnsAsync((Guid uid) => _UserFakes.FirstOrDefault(x => x.Id == uid.ToString()));

            // Act
            Guid.TryParse(_UserFakes[0].Id, out Guid _Uid);
            IdentityUser _User = await __NoteRepositoryMock.Object.GetByUIDAsync(_Uid);

            // Assert
            Assert.Equal(_UserFakes[0], _User);
        }

        [Fact]
        public async Task GetByUIDAsync_ShouldNotReturnUser_WhenUserDoesNotExist()
        {
            // Arrange
            Faker<IdentityUser> _IdentityFaker = new Faker<IdentityUser>()
                .RuleFor(o => o.Email, f => f.Person.Email)
                .RuleFor(o => o.UserName, f => f.Person.UserName)
                .RuleFor(o => o.EmailConfirmed, f => f.Random.Bool())
                .RuleFor(o => o.Id, f => f.Random.Guid().ToString());

            IList<IdentityUser> _UserFakes = _IdentityFaker.Generate(5);

            __NoteRepositoryMock.Setup(x => x.GetByUIDAsync(It.IsAny<Guid>())).ReturnsAsync((Guid uid) => _UserFakes.FirstOrDefault(x => x.Id == uid.ToString()));

            // Act
            IdentityUser _User = await __NoteRepositoryMock.Object.GetByUIDAsync(Guid.NewGuid());

            // Assert
            Assert.Null(_User);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenUserExists()
        {
            // Arrange
            Faker<IdentityUser> _IdentityFaker = new Faker<IdentityUser>()
                .RuleFor(o => o.Email, f => f.Person.Email)
                .RuleFor(o => o.UserName, f => f.Person.UserName)
                .RuleFor(o => o.EmailConfirmed, f => f.Random.Bool())
                .RuleFor(o => o.Id, f => f.Random.Guid().ToString());

            IList<IdentityUser> _UserFakes = _IdentityFaker.Generate(5);

            __NoteRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync((Guid uid) => _UserFakes.Select(x => x.Id).Contains(uid.ToString()));

            // Act
            Guid.TryParse(_UserFakes[0].Id, out Guid uid);
            bool _Result = await __NoteRepositoryMock.Object.DeleteAsync(uid);

            // Assert
            Assert.True(_Result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenUserDoesNotExist()
        {
            // Arrange
            Faker<IdentityUser> _IdentityFaker = new Faker<IdentityUser>()
                .RuleFor(o => o.Email, f => f.Person.Email)
                .RuleFor(o => o.UserName, f => f.Person.UserName)
                .RuleFor(o => o.EmailConfirmed, f => f.Random.Bool())
                .RuleFor(o => o.Id, f => f.Random.Guid().ToString());

            IList<IdentityUser> _UserFakes = _IdentityFaker.Generate(5);

            __NoteRepositoryMock.Setup(x => x.DeleteAsync(Guid.Empty)).ReturnsAsync(false);
            __NoteRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync((Guid uid) => _UserFakes.Select(x => x.Id).Contains(uid.ToString()));

            // Act
            bool _ResultEmptyGuid = await __NoteRepositoryMock.Object.DeleteAsync(Guid.Empty);
            bool _ResultNonExisting = await __NoteRepositoryMock.Object.DeleteAsync(Guid.NewGuid());

            // Assert
            Assert.False(_ResultNonExisting);
            Assert.False(_ResultEmptyGuid);
        }
    }
}
