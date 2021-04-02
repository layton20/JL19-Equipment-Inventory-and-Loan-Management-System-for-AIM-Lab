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
    public class EmailScheduleUnitTest
    {
        private readonly Mock<IEmailScheduleRepository> __EmailScheduleRepositoryMock = new Mock<IEmailScheduleRepository>();

        [Fact]
        public async Task CreateAsync_ShouldReturnEmailSchedule_WhenEmailScheduleIsValid()
        {
            // Arrange
            Faker<EmailScheduleEntity> _ScheduleFaker = new Faker<EmailScheduleEntity>()
               .RuleFor(o => o.EmailTemplateUID, f => f.Random.Guid())
               .RuleFor(o => o.RecipientEmailAddress, f => f.Person.Email)
               .RuleFor(o => o.EmailType, f => f.PickRandom<EmailType>())
               .RuleFor(o => o.SendTimestamp, f => f.Date.Between(DateTime.Now.AddMonths(-3), DateTime.Now.AddMonths(3)))
               .RuleFor(o => o.Sent, f => f.Random.Bool());

            EmailScheduleEntity _EmailScheduleFake = _ScheduleFaker.Generate();

            __EmailScheduleRepositoryMock.Setup(x => x.CreateAsync(_EmailScheduleFake)).ReturnsAsync(_EmailScheduleFake);

            // Act
            EmailScheduleEntity _EmailSchedule = await __EmailScheduleRepositoryMock.Object.CreateAsync(_EmailScheduleFake);

            // Assert
            Assert.Equal(_EmailScheduleFake, _EmailSchedule);
        }

        [Fact]
        public async Task CreateAsync_ShouldNotReturnEmailSchedule_WhenEmailScheduleIsNotValid()
        {
            // Arrange
            __EmailScheduleRepositoryMock.Setup(x => x.CreateAsync(It.Is<EmailScheduleEntity>(x => x.UID == Guid.Empty))).ReturnsAsync(() => null);
            __EmailScheduleRepositoryMock.Setup(x => x.CreateAsync(null)).ReturnsAsync(() => null);

            // Act
            EmailScheduleEntity _EmailScheduleEmptyUID = await __EmailScheduleRepositoryMock.Object.CreateAsync(new EmailScheduleEntity { UID = Guid.Empty });
            EmailScheduleEntity _EmailScheduleNull = await __EmailScheduleRepositoryMock.Object.CreateAsync(null);

            // Assert
            Assert.Null(_EmailScheduleEmptyUID);
            Assert.Null(_EmailScheduleNull);
        }

        [Fact]
        public async Task BulkCreateAsync_ShouldReturnEmailSchedules_WhenEmailSchedulesAreValid()
        {
            // Arrange
            Faker<EmailScheduleEntity> _ScheduleFaker = new Faker<EmailScheduleEntity>()
               .RuleFor(o => o.EmailTemplateUID, f => f.Random.Guid())
               .RuleFor(o => o.RecipientEmailAddress, f => f.Person.Email)
               .RuleFor(o => o.EmailType, f => f.PickRandom<EmailType>())
               .RuleFor(o => o.SendTimestamp, f => f.Date.Between(DateTime.Now.AddMonths(-3), DateTime.Now.AddMonths(3)))
               .RuleFor(o => o.Sent, f => f.Random.Bool());

            IList<EmailScheduleEntity> _EmailScheduleFakes = _ScheduleFaker.Generate(5);

            __EmailScheduleRepositoryMock.Setup(x => x.BulkCreateAsync(_EmailScheduleFakes)).ReturnsAsync(_EmailScheduleFakes);

            // Act
            IList<EmailScheduleEntity> _EmailSchedules = await __EmailScheduleRepositoryMock.Object.BulkCreateAsync(_EmailScheduleFakes);

            // Assert
            Assert.Equal(_EmailScheduleFakes, _EmailSchedules);
        }

        [Fact]
        public async Task BulkCreateAsync_ShouldNotReturnEmailSchedules_WhenEmailSchedulesAreNotValid()
        {
            // Arrange
            Faker<EmailScheduleEntity> _ScheduleFaker = new Faker<EmailScheduleEntity>()
               .RuleFor(o => o.EmailTemplateUID, f => f.Random.Guid())
               .RuleFor(o => o.RecipientEmailAddress, f => f.Person.Email)
               .RuleFor(o => o.EmailType, f => f.PickRandom<EmailType>())
               .RuleFor(o => o.SendTimestamp, f => f.Date.Between(DateTime.Now.AddMonths(-3), DateTime.Now.AddMonths(3)))
               .RuleFor(o => o.Sent, f => f.Random.Bool());

            IList<EmailScheduleEntity> _EmailScheduleFakes = _ScheduleFaker.Generate(5);
            _EmailScheduleFakes.Add(new EmailScheduleEntity { UID = Guid.Empty });

            __EmailScheduleRepositoryMock.Setup(x => x.BulkCreateAsync(It.IsAny<IList<EmailScheduleEntity>>()))
                .ReturnsAsync((IList<EmailScheduleEntity> schedules) => schedules.Where(x => x.UID != Guid.Empty).ToList());


            // Act
            IList<EmailScheduleEntity> _EmailSchedules = await __EmailScheduleRepositoryMock.Object.BulkCreateAsync(_EmailScheduleFakes);

            // Assert
            Assert.Equal(5, _EmailSchedules.Count());
        }

        [Fact]
        public async Task GetAsync_ShouldReturnEmailSchedules_WhenEmailSchedulesExist()
        {
            // Arrange
            Faker<EmailScheduleEntity> _ScheduleFaker = new Faker<EmailScheduleEntity>()
               .RuleFor(o => o.EmailTemplateUID, f => f.Random.Guid())
               .RuleFor(o => o.RecipientEmailAddress, f => f.Person.Email)
               .RuleFor(o => o.EmailType, f => f.PickRandom<EmailType>())
               .RuleFor(o => o.SendTimestamp, f => f.Date.Between(DateTime.Now.AddMonths(-3), DateTime.Now.AddMonths(3)))
               .RuleFor(o => o.Sent, f => f.Random.Bool());

            IList<EmailScheduleEntity> _EmailScheduleFakes = _ScheduleFaker.Generate(5);

            __EmailScheduleRepositoryMock.Setup(x => x.GetAsync()).ReturnsAsync(_EmailScheduleFakes);

            // Act
            IList<EmailScheduleEntity> _EmailSchedules = await __EmailScheduleRepositoryMock.Object.GetAsync();

            // Assert
            Assert.NotEmpty(_EmailSchedules);
        }

        [Fact]
        public async Task GetAsync_ShouldNotReturnEmailSchedules_WhenEmailSchedulesDoNotExist()
        {
            // Arrange
            __EmailScheduleRepositoryMock.Setup(x => x.GetAsync()).ReturnsAsync(() => null);

            // Act
            IList<EmailScheduleEntity> _EmailSchedules = await __EmailScheduleRepositoryMock.Object.GetAsync();

            // Assert
            Assert.Null(_EmailSchedules);
        }

        [Fact]
        public async Task GetEmailsToSendAsync_ShouldReturnEmailSchedules_WhenEmailSchedulesExist()
        {
            // Arrange
            Faker<EmailScheduleEntity> _ScheduleFaker = new Faker<EmailScheduleEntity>()
               .RuleFor(o => o.EmailTemplateUID, f => f.Random.Guid())
               .RuleFor(o => o.RecipientEmailAddress, f => f.Person.Email)
               .RuleFor(o => o.EmailType, f => f.PickRandom<EmailType>())
               .RuleFor(o => o.SendTimestamp, f => f.Date.Between(DateTime.Now.AddMonths(-3), DateTime.Now.AddMonths(3)))
               .RuleFor(o => o.Sent, f => f.Random.Bool());

            IList<EmailScheduleEntity> _EmailScheduleFakes = _ScheduleFaker.Generate(10);

            __EmailScheduleRepositoryMock.Setup(x => x.GetEmailsToSendAsync())
                .ReturnsAsync(_EmailScheduleFakes.Where(x => !x.Sent && x.SendTimestamp > DateTime.Now).ToList());

            // Act
            IList<EmailScheduleEntity> _EmailSchedules = await __EmailScheduleRepositoryMock.Object.GetEmailsToSendAsync();

            // Assert
            Assert.Equal(_EmailScheduleFakes.Where(x => !x.Sent && x.SendTimestamp > DateTime.Now), _EmailSchedules);
        }

        [Fact]
        public async Task GetByUIDAsync_ShouldReturnEmailSchedule_WhenEmailScheduleExists()
        {
            // Arrange
            Faker<EmailScheduleEntity> _ScheduleFaker = new Faker<EmailScheduleEntity>()
               .RuleFor(o => o.EmailTemplateUID, f => f.Random.Guid())
               .RuleFor(o => o.RecipientEmailAddress, f => f.Person.Email)
               .RuleFor(o => o.EmailType, f => f.PickRandom<EmailType>())
               .RuleFor(o => o.SendTimestamp, f => f.Date.Between(DateTime.Now.AddMonths(-3), DateTime.Now.AddMonths(3)))
               .RuleFor(o => o.Sent, f => f.Random.Bool());

            IList<EmailScheduleEntity> _EmailScheduleFakes = _ScheduleFaker.Generate(10);

            __EmailScheduleRepositoryMock.Setup(x => x.GetByUIDAsync(It.IsAny<Guid>())).ReturnsAsync((Guid uid) => _EmailScheduleFakes.FirstOrDefault(x => x.UID == uid));

            // Act
            EmailScheduleEntity _EmailSchedule = await __EmailScheduleRepositoryMock.Object.GetByUIDAsync(_EmailScheduleFakes[0].UID);

            // Assert
            Assert.Equal(_EmailScheduleFakes[0], _EmailSchedule);
        }

        [Fact]
        public async Task GetByUIDAsync_ShouldNotReturnEmailSchedule_WhenEmailScheduleDoesNotExist()
        {
            // Arrange
            Faker<EmailScheduleEntity> _ScheduleFaker = new Faker<EmailScheduleEntity>()
               .RuleFor(o => o.EmailTemplateUID, f => f.Random.Guid())
               .RuleFor(o => o.RecipientEmailAddress, f => f.Person.Email)
               .RuleFor(o => o.EmailType, f => f.PickRandom<EmailType>())
               .RuleFor(o => o.SendTimestamp, f => f.Date.Between(DateTime.Now.AddMonths(-3), DateTime.Now.AddMonths(3)))
               .RuleFor(o => o.Sent, f => f.Random.Bool());

            IList<EmailScheduleEntity> _EmailScheduleFakes = _ScheduleFaker.Generate(10);

            __EmailScheduleRepositoryMock.Setup(x => x.GetByUIDAsync(It.IsAny<Guid>())).ReturnsAsync((Guid uid) => _EmailScheduleFakes.FirstOrDefault(x => x.UID == uid));

            // Act
            EmailScheduleEntity _EmailSchedule = await __EmailScheduleRepositoryMock.Object.GetByUIDAsync(Guid.NewGuid());

            // Assert
            Assert.Null(_EmailSchedule);
        }

        [Fact]
        public async Task UpdateSentAsync_ShouldReturnTrue_WhenEmailScheduleExists()
        {
            // Arrange
            Faker<EmailScheduleEntity> _ScheduleFaker = new Faker<EmailScheduleEntity>()
               .RuleFor(o => o.EmailTemplateUID, f => f.Random.Guid())
               .RuleFor(o => o.RecipientEmailAddress, f => f.Person.Email)
               .RuleFor(o => o.EmailType, f => f.PickRandom<EmailType>())
               .RuleFor(o => o.SendTimestamp, f => f.Date.Between(DateTime.Now.AddMonths(-3), DateTime.Now.AddMonths(3)))
               .RuleFor(o => o.Sent, f => f.Random.Bool());

            IList<EmailScheduleEntity> _EmailScheduleFakes = _ScheduleFaker.Generate(10);

            __EmailScheduleRepositoryMock.Setup(x => x.UpdateSentAsync(It.IsAny<Guid>(), It.IsAny<bool>())).ReturnsAsync((Guid uid, bool sent) => _EmailScheduleFakes.Select(x => x.UID).Contains(uid));

            // Act
            bool _Result = await __EmailScheduleRepositoryMock.Object.UpdateSentAsync(_EmailScheduleFakes[0].UID, true);

            // Assert
            Assert.True(_Result);
        }

        [Fact]
        public async Task UpdateSentAsync_ShouldReturnFalse_WhenEmailScheduleDoesNotExist()
        {
            // Arrange
            Faker<EmailScheduleEntity> _ScheduleFaker = new Faker<EmailScheduleEntity>()
               .RuleFor(o => o.EmailTemplateUID, f => f.Random.Guid())
               .RuleFor(o => o.RecipientEmailAddress, f => f.Person.Email)
               .RuleFor(o => o.EmailType, f => f.PickRandom<EmailType>())
               .RuleFor(o => o.SendTimestamp, f => f.Date.Between(DateTime.Now.AddMonths(-3), DateTime.Now.AddMonths(3)))
               .RuleFor(o => o.Sent, f => f.Random.Bool());

            IList<EmailScheduleEntity> _EmailScheduleFakes = _ScheduleFaker.Generate(10);

            __EmailScheduleRepositoryMock.Setup(x => x.UpdateSentAsync(It.IsAny<Guid>(), It.IsAny<bool>())).ReturnsAsync((Guid uid, bool sent) => _EmailScheduleFakes.Select(x => x.UID).Contains(uid));

            // Act
            bool _Result = await __EmailScheduleRepositoryMock.Object.UpdateSentAsync(Guid.NewGuid(), true);

            // Assert
            Assert.False(_Result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenEmailScheduleExists()
        {
            // Arrange
            Faker<EmailScheduleEntity> _ScheduleFaker = new Faker<EmailScheduleEntity>()
                .RuleFor(o => o.EmailTemplateUID, f => f.Random.Guid())
                .RuleFor(o => o.RecipientEmailAddress, f => f.Person.Email)
                .RuleFor(o => o.EmailType, f => f.PickRandom<EmailType>())
                .RuleFor(o => o.SendTimestamp, f => f.Date.Between(DateTime.Now.AddMonths(-3), DateTime.Now.AddMonths(3)))
                .RuleFor(o => o.Sent, f => f.Random.Bool());

            IList<EmailScheduleEntity> _EmailScheduleFakes = _ScheduleFaker.Generate(10);

            __EmailScheduleRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync((Guid uid) => _EmailScheduleFakes.Select(x => x.UID).Contains(uid));

            // Act
            bool _Result = await __EmailScheduleRepositoryMock.Object.DeleteAsync(_EmailScheduleFakes[0].UID);

            // Assert
            Assert.True(_Result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenEmailScheduleDoesNotExist()
        {
            // Arrange
            Faker<EmailScheduleEntity> _ScheduleFaker = new Faker<EmailScheduleEntity>()
                .RuleFor(o => o.EmailTemplateUID, f => f.Random.Guid())
                .RuleFor(o => o.RecipientEmailAddress, f => f.Person.Email)
                .RuleFor(o => o.EmailType, f => f.PickRandom<EmailType>())
                .RuleFor(o => o.SendTimestamp, f => f.Date.Between(DateTime.Now.AddMonths(-3), DateTime.Now.AddMonths(3)))
                .RuleFor(o => o.Sent, f => f.Random.Bool());

            IList<EmailScheduleEntity> _EmailScheduleFakes = _ScheduleFaker.Generate(10);

            __EmailScheduleRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync((Guid uid) => _EmailScheduleFakes.Select(x => x.UID).Contains(uid));

            // Act
            bool _Result = await __EmailScheduleRepositoryMock.Object.DeleteAsync(Guid.NewGuid());

            // Assert
            Assert.False(_Result);
        }
    }
}
