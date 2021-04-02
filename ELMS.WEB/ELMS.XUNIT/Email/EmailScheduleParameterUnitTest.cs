using Bogus;
using ELMS.WEB.Entities.Email;
using ELMS.WEB.Enums.Email;
using ELMS.WEB.Repositories.Email.Interface;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ELMS.XUNIT.Email
{
    public class EmailScheduleParameterUnitTest
    {
        private readonly Mock<IEmailScheduleParameterRepository> __EmailScheduleRepositoryMock = new Mock<IEmailScheduleParameterRepository>();

        [Fact]
        public async Task CreateAsync_ShouldReturnEmailScheduleParameter_WhereEmailScheduleParameterIsValid()
        {
            // Arrange
            Faker<EmailScheduleEntity> _ScheduleFaker = new Faker<EmailScheduleEntity>()
               .RuleFor(o => o.EmailTemplateUID, f => f.Random.Guid())
               .RuleFor(o => o.RecipientEmailAddress, f => f.Person.Email)
               .RuleFor(o => o.EmailType, f => f.PickRandom<EmailType>())
               .RuleFor(o => o.SendTimestamp, f => f.Date.Between(DateTime.Now.AddMonths(-3), DateTime.Now.AddMonths(3)))
               .RuleFor(o => o.Sent, f => f.Random.Bool());

            EmailScheduleEntity _EmailScheduleFake = _ScheduleFaker.Generate();

            Faker<EmailScheduleParameterEntity> _EmailScheduleParameterFaker = new Faker<EmailScheduleParameterEntity>()
                .RuleFor(o => o.EmailScheduleUID, f => _EmailScheduleFake.UID)
                .RuleFor(o => o.EmailSchedule, f => _EmailScheduleFake)
                .RuleFor(o => o.Name, f => f.Lorem.Word())
                .RuleFor(o => o.Value, f => f.Lorem.Word());

            EmailScheduleParameterEntity _EmailScheduleParameterFake = _EmailScheduleParameterFaker.Generate();

            __EmailScheduleRepositoryMock.Setup(x => x.CreateAsync(_EmailScheduleParameterFake)).ReturnsAsync(_EmailScheduleParameterFake);

            // Act
            EmailScheduleParameterEntity _EmailScheduleParameter = await __EmailScheduleRepositoryMock.Object.CreateAsync(_EmailScheduleParameterFake);

            // Assert
            Assert.Equal(_EmailScheduleParameterFake, _EmailScheduleParameter);
        }

        [Fact]
        public async Task CreateAsync_ShouldNotReturnEmailScheduleParameter_WhereEmailScheduleParamterIsNotValid()
        {
            // Arrange
            __EmailScheduleRepositoryMock.Setup(x => x.CreateAsync(It.Is<EmailScheduleParameterEntity>(x => x.UID == Guid.Empty))).ReturnsAsync(() => null);

            // Act
            EmailScheduleParameterEntity _EmailScheduleParameter = await __EmailScheduleRepositoryMock.Object.CreateAsync(new EmailScheduleParameterEntity { UID = Guid.Empty });

            // Assert
            Assert.Null(_EmailScheduleParameter);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnEmailScheduleParameters_WhereEmailScheduleParametersExist()
        {
            // Arrange
            Guid _ScheduleUID = Guid.NewGuid();

            Faker<EmailScheduleEntity> _ScheduleFaker = new Faker<EmailScheduleEntity>()
                .RuleFor(o => o.UID, f => _ScheduleUID)
                .RuleFor(o => o.EmailTemplateUID, f => f.Random.Guid())
                .RuleFor(o => o.RecipientEmailAddress, f => f.Person.Email)
                .RuleFor(o => o.EmailType, f => f.PickRandom<EmailType>())
                .RuleFor(o => o.SendTimestamp, f => f.Date.Between(DateTime.Now.AddMonths(-3), DateTime.Now.AddMonths(3)))
                .RuleFor(o => o.Sent, f => f.Random.Bool());

            EmailScheduleEntity _EmailScheduleFake = _ScheduleFaker.Generate();

            Faker<EmailScheduleParameterEntity> _EmailScheduleParameterFaker = new Faker<EmailScheduleParameterEntity>()
                .RuleFor(o => o.EmailScheduleUID, f => _ScheduleUID)
                .RuleFor(o => o.EmailSchedule, f => _EmailScheduleFake)
                .RuleFor(o => o.Name, f => f.Lorem.Word())
                .RuleFor(o => o.Value, f => f.Lorem.Word());

            IList<EmailScheduleParameterEntity> _EmailScheduleParameterFakes = _EmailScheduleParameterFaker.Generate(5);

            __EmailScheduleRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync((Guid uid) => _EmailScheduleParameterFakes.Where(x => x.EmailScheduleUID == uid).ToList());

            // Act
            IList<EmailScheduleParameterEntity> _EmailScheduleParameters = await __EmailScheduleRepositoryMock.Object.GetAsync(_ScheduleUID);

            // Assert
            Assert.Equal(_EmailScheduleParameterFakes, _EmailScheduleParameters);
        }

        [Fact]
        public async Task GetAsync_ShouldNotReturnEmailScheduleParameters_WhereEmailScheduleParametersDoesNotExist()
        {
            // Arrange
            // Arrange
            Guid _ScheduleUID = Guid.NewGuid();

            Faker<EmailScheduleEntity> _ScheduleFaker = new Faker<EmailScheduleEntity>()
                .RuleFor(o => o.UID, f => _ScheduleUID)
                .RuleFor(o => o.EmailTemplateUID, f => f.Random.Guid())
                .RuleFor(o => o.RecipientEmailAddress, f => f.Person.Email)
                .RuleFor(o => o.EmailType, f => f.PickRandom<EmailType>())
                .RuleFor(o => o.SendTimestamp, f => f.Date.Between(DateTime.Now.AddMonths(-3), DateTime.Now.AddMonths(3)))
                .RuleFor(o => o.Sent, f => f.Random.Bool());

            EmailScheduleEntity _EmailScheduleFake = _ScheduleFaker.Generate();

            Faker<EmailScheduleParameterEntity> _EmailScheduleParameterFaker = new Faker<EmailScheduleParameterEntity>()
                .RuleFor(o => o.EmailScheduleUID, f => _ScheduleUID)
                .RuleFor(o => o.EmailSchedule, f => _EmailScheduleFake)
                .RuleFor(o => o.Name, f => f.Lorem.Word())
                .RuleFor(o => o.Value, f => f.Lorem.Word());

            IList<EmailScheduleParameterEntity> _EmailScheduleParameterFakes = _EmailScheduleParameterFaker.Generate(5);

            __EmailScheduleRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync((Guid uid) => _EmailScheduleParameterFakes.Where(x => x.EmailScheduleUID == uid).ToList());

            // Act
            IList<EmailScheduleParameterEntity> _EmailScheduleParameters = await __EmailScheduleRepositoryMock.Object.GetAsync(Guid.NewGuid());

            // Assert
            Assert.NotEqual(_EmailScheduleParameterFakes, _EmailScheduleParameters);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenEmailScheduleParameterExists()
        {
            // Arrange
            Faker<EmailScheduleEntity> _ScheduleFaker = new Faker<EmailScheduleEntity>()
                .RuleFor(o => o.UID, f => f.Random.Guid())
                .RuleFor(o => o.EmailTemplateUID, f => f.Random.Guid())
                .RuleFor(o => o.RecipientEmailAddress, f => f.Person.Email)
                .RuleFor(o => o.EmailType, f => f.PickRandom<EmailType>())
                .RuleFor(o => o.SendTimestamp, f => f.Date.Between(DateTime.Now.AddMonths(-3), DateTime.Now.AddMonths(3)))
                .RuleFor(o => o.Sent, f => f.Random.Bool());

            EmailScheduleEntity _EmailScheduleFake = _ScheduleFaker.Generate();

            Faker<EmailScheduleParameterEntity> _EmailScheduleParameterFaker = new Faker<EmailScheduleParameterEntity>()
                .RuleFor(o => o.EmailScheduleUID, f => _EmailScheduleFake.UID)
                .RuleFor(o => o.EmailSchedule, f => _EmailScheduleFake)
                .RuleFor(o => o.Name, f => f.Lorem.Word())
                .RuleFor(o => o.Value, f => f.Lorem.Word());

            EmailScheduleParameterEntity _EmailScheduleParameterFake = _EmailScheduleParameterFaker.Generate();

            __EmailScheduleRepositoryMock.Setup(x => x.DeleteAsync(_EmailScheduleFake.UID)).ReturnsAsync(true);

            // Act
            bool _Result = await __EmailScheduleRepositoryMock.Object.DeleteAsync(_EmailScheduleParameterFake.EmailScheduleUID);

            // Assert
            Assert.True(_Result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenEmailScheduleParametersDoesNotExist()
        {
            // Arrange
            Faker<EmailScheduleEntity> _ScheduleFaker = new Faker<EmailScheduleEntity>()
                .RuleFor(o => o.UID, f => f.Random.Guid())
                .RuleFor(o => o.EmailTemplateUID, f => f.Random.Guid())
                .RuleFor(o => o.RecipientEmailAddress, f => f.Person.Email)
                .RuleFor(o => o.EmailType, f => f.PickRandom<EmailType>())
                .RuleFor(o => o.SendTimestamp, f => f.Date.Between(DateTime.Now.AddMonths(-3), DateTime.Now.AddMonths(3)))
                .RuleFor(o => o.Sent, f => f.Random.Bool());

            EmailScheduleEntity _EmailScheduleFake = _ScheduleFaker.Generate();

            Faker<EmailScheduleParameterEntity> _EmailScheduleParameterFaker = new Faker<EmailScheduleParameterEntity>()
                .RuleFor(o => o.EmailScheduleUID, f => _EmailScheduleFake.UID)
                .RuleFor(o => o.EmailSchedule, f => _EmailScheduleFake)
                .RuleFor(o => o.Name, f => f.Lorem.Word())
                .RuleFor(o => o.Value, f => f.Lorem.Word());

            IList<EmailScheduleParameterEntity> _EmailScheduleParameterFakes = _EmailScheduleParameterFaker.Generate(5);

            __EmailScheduleRepositoryMock.Setup(x => x.DeleteAsync(It.Is<Guid>(x => x == Guid.Empty))).ReturnsAsync(false);
            __EmailScheduleRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync((Guid uid) => _EmailScheduleParameterFakes.Select(x => x.EmailScheduleUID).Contains(uid));

            // Act
            bool _ResultEmptyUID = await __EmailScheduleRepositoryMock.Object.DeleteAsync(Guid.Empty);
            bool _ResultNonExisting = await __EmailScheduleRepositoryMock.Object.DeleteAsync(Guid.NewGuid());

            // Assert
            Assert.False(_ResultEmptyUID);
            Assert.False(_ResultNonExisting);
        }
    }
}
