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
    public class EmailTemplateUnitTest
    {
        private readonly Mock<IEmailTemplateRepository> __EmailTemplateRepositoryMock = new Mock<IEmailTemplateRepository>();

        [Fact]
        public async Task CreateAsync_ShouldReturnEmailTemplate_WhenEmailTemplateIsValid()
        {
            // Arrange
            Faker<EmailTemplateEntity> _EmailTemplateFaker = new Faker<EmailTemplateEntity>()
                .RuleFor(o => o.Name, f => f.Lorem.Word())
                .RuleFor(o => o.TemplateType, f => EmailTemplateType.Formatted)
                .RuleFor(o => o.Subject, f => f.Lorem.Sentence())
                .RuleFor(o => o.Header, f => f.Lorem.Sentence())
                .RuleFor(o => o.Subheader, f => f.Lorem.Sentence())
                .RuleFor(o => o.Body, f => f.Lorem.Sentences())
                .RuleFor(o => o.Footer, f => f.Lorem.Sentence(0, 10))
                .RuleFor(o => o.OwnerUID, f => f.Random.Guid());

            EmailTemplateEntity _EmailTemplateFake = _EmailTemplateFaker.Generate();

            __EmailTemplateRepositoryMock.Setup(x => x.CreateAsync(_EmailTemplateFake)).ReturnsAsync(_EmailTemplateFake);

            // Act
            EmailTemplateEntity _EmailTemplate = await __EmailTemplateRepositoryMock.Object.CreateAsync(_EmailTemplateFake);

            // Assert
            Assert.Equal(_EmailTemplateFake, _EmailTemplate);
        }

        [Fact]
        public async Task CreateAsync_ShouldNotReturnEmailTemplate_WhenEmailTemplateIsNotValid()
        {
            // Arrange
            __EmailTemplateRepositoryMock.Setup(x => x.CreateAsync(It.Is<EmailTemplateEntity>(x => x.UID == Guid.Empty))).ReturnsAsync(() => null);
            __EmailTemplateRepositoryMock.Setup(x => x.CreateAsync(null)).ReturnsAsync(() => null);

            // Act
            EmailTemplateEntity _EmailTemplateEmptyUID = await __EmailTemplateRepositoryMock.Object.CreateAsync(new EmailTemplateEntity { UID = Guid.Empty });
            EmailTemplateEntity _EmailTemplateNull = await __EmailTemplateRepositoryMock.Object.CreateAsync(null);

            // Assert
            Assert.Null(_EmailTemplateEmptyUID);
            Assert.Null(_EmailTemplateNull);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnEmailTemplates_WhenEmailTemplatesExists()
        {
            // Arrange
            Faker<EmailTemplateEntity> _EmailTemplateFaker = new Faker<EmailTemplateEntity>()
                .RuleFor(o => o.Name, f => f.Lorem.Word())
                .RuleFor(o => o.TemplateType, f => EmailTemplateType.Formatted)
                .RuleFor(o => o.Subject, f => f.Lorem.Sentence())
                .RuleFor(o => o.Header, f => f.Lorem.Sentence())
                .RuleFor(o => o.Subheader, f => f.Lorem.Sentence())
                .RuleFor(o => o.Body, f => f.Lorem.Sentences())
                .RuleFor(o => o.Footer, f => f.Lorem.Sentence(0, 10))
                .RuleFor(o => o.OwnerUID, f => f.Random.Guid());

            IList<EmailTemplateEntity> _EmailTemplateFakes = _EmailTemplateFaker.Generate(5);

            __EmailTemplateRepositoryMock.Setup(x => x.GetAsync()).ReturnsAsync(_EmailTemplateFakes);

            // Act
            IList<EmailTemplateEntity> _EmailTemplates = await __EmailTemplateRepositoryMock.Object.GetAsync();

            // Assert
            Assert.Equal(_EmailTemplateFakes, _EmailTemplates);
        }

        [Fact]
        public async Task GetAsync_ShouldNotReturnEmailTemplates_WhenEmailTemplatesDoesNotExist()
        {
            // Arrange
            __EmailTemplateRepositoryMock.Setup(x => x.GetAsync()).ReturnsAsync(() => null);

            // Act
            IList<EmailTemplateEntity> _EmailTemplates = await __EmailTemplateRepositoryMock.Object.GetAsync();

            // Assert
            Assert.Null(_EmailTemplates);
        }

        [Fact]
        public async Task GetByUIDAsync_ShouldReturnEmailTemplate_WhenEmailTemplateExists()
        {
            // Arrange
            Faker<EmailTemplateEntity> _EmailTemplateFaker = new Faker<EmailTemplateEntity>()
                .RuleFor(o => o.Name, f => f.Lorem.Word())
                .RuleFor(o => o.TemplateType, f => EmailTemplateType.Formatted)
                .RuleFor(o => o.Subject, f => f.Lorem.Sentence())
                .RuleFor(o => o.Header, f => f.Lorem.Sentence())
                .RuleFor(o => o.Subheader, f => f.Lorem.Sentence())
                .RuleFor(o => o.Body, f => f.Lorem.Sentences())
                .RuleFor(o => o.Footer, f => f.Lorem.Sentence(0, 10))
                .RuleFor(o => o.OwnerUID, f => f.Random.Guid());

            IList<EmailTemplateEntity> _EmailTemplateFakes = _EmailTemplateFaker.Generate(5);

            __EmailTemplateRepositoryMock.Setup(x => x.GetByUIDAsync(It.IsAny<Guid>())).ReturnsAsync((Guid uid) => _EmailTemplateFakes.FirstOrDefault(x => x.UID == uid));

            // Act
            EmailTemplateEntity _EmailTemplateNonExisting = await __EmailTemplateRepositoryMock.Object.GetByUIDAsync(_EmailTemplateFakes[0].UID);

            // Assert
            Assert.Equal(_EmailTemplateFakes[0], _EmailTemplateNonExisting);
        }

        [Fact]
        public async Task GetByUIDAsync_ShouldNotReturnEmailTemplate_WhenEmailTemplateDoesNotExists()
        {
            // Arrange
            Faker<EmailTemplateEntity> _EmailTemplateFaker = new Faker<EmailTemplateEntity>()
                .RuleFor(o => o.Name, f => f.Lorem.Word())
                .RuleFor(o => o.TemplateType, f => EmailTemplateType.Formatted)
                .RuleFor(o => o.Subject, f => f.Lorem.Sentence())
                .RuleFor(o => o.Header, f => f.Lorem.Sentence())
                .RuleFor(o => o.Subheader, f => f.Lorem.Sentence())
                .RuleFor(o => o.Body, f => f.Lorem.Sentences())
                .RuleFor(o => o.Footer, f => f.Lorem.Sentence(0, 10))
                .RuleFor(o => o.OwnerUID, f => f.Random.Guid());

            IList<EmailTemplateEntity> _EmailTemplateFakes = _EmailTemplateFaker.Generate(5);

            __EmailTemplateRepositoryMock.Setup(x => x.GetByUIDAsync(It.IsAny<Guid>())).ReturnsAsync((Guid uid) => _EmailTemplateFakes.FirstOrDefault(x => x.UID == uid));

            // Act
            EmailTemplateEntity _EmailTemplateNonExisting = await __EmailTemplateRepositoryMock.Object.GetByUIDAsync(Guid.NewGuid());

            // Assert
            Assert.Null(_EmailTemplateNonExisting);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnTrue_WhenEmailTemplateIsValid()
        {
            // Arrange
            Faker<EmailTemplateEntity> _EmailTemplateFaker = new Faker<EmailTemplateEntity>()
                .RuleFor(o => o.Name, f => f.Lorem.Word())
                .RuleFor(o => o.TemplateType, f => EmailTemplateType.Formatted)
                .RuleFor(o => o.Subject, f => f.Lorem.Sentence())
                .RuleFor(o => o.Header, f => f.Lorem.Sentence())
                .RuleFor(o => o.Subheader, f => f.Lorem.Sentence())
                .RuleFor(o => o.Body, f => f.Lorem.Sentences())
                .RuleFor(o => o.Footer, f => f.Lorem.Sentence(0, 10))
                .RuleFor(o => o.OwnerUID, f => f.Random.Guid());

            EmailTemplateEntity _EmailTemplateFake = _EmailTemplateFaker.Generate();

            __EmailTemplateRepositoryMock.Setup(x => x.UpdateAsync(_EmailTemplateFake)).ReturnsAsync(true);

            // Act
            bool _Result = await __EmailTemplateRepositoryMock.Object.UpdateAsync(_EmailTemplateFake);

            // Assert
            Assert.True(_Result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnFalse_WhenEmailTemplateIsNotValid()
        {
            // Arrange
            EmailTemplateEntity _EmailTemplateFake = new EmailTemplateEntity { UID = Guid.Empty };

            __EmailTemplateRepositoryMock.Setup(x => x.UpdateAsync(It.Is<EmailTemplateEntity>(x => x.UID == Guid.Empty))).ReturnsAsync(false);

            // Act
            bool _Result = await __EmailTemplateRepositoryMock.Object.UpdateAsync(_EmailTemplateFake);

            // Assert
            Assert.False(_Result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenEmailTemplateExists()
        {
            // Arrange
            Faker<EmailTemplateEntity> _EmailTemplateFaker = new Faker<EmailTemplateEntity>()
                .RuleFor(o => o.Name, f => f.Lorem.Word())
                .RuleFor(o => o.TemplateType, f => EmailTemplateType.Formatted)
                .RuleFor(o => o.Subject, f => f.Lorem.Sentence())
                .RuleFor(o => o.Header, f => f.Lorem.Sentence())
                .RuleFor(o => o.Subheader, f => f.Lorem.Sentence())
                .RuleFor(o => o.Body, f => f.Lorem.Sentences())
                .RuleFor(o => o.Footer, f => f.Lorem.Sentence(0, 10))
                .RuleFor(o => o.OwnerUID, f => f.Random.Guid());

            EmailTemplateEntity _EmailTemplateFake = _EmailTemplateFaker.Generate();

            __EmailTemplateRepositoryMock.Setup(x => x.DeleteAsync(_EmailTemplateFake.UID)).ReturnsAsync(true);

            // Act
            bool _ResultExistingTemplate = await __EmailTemplateRepositoryMock.Object.DeleteAsync(_EmailTemplateFake.UID);
            bool _ResultNonExistingTemplate = await __EmailTemplateRepositoryMock.Object.DeleteAsync(Guid.NewGuid());

            // Assert
            Assert.True(_ResultExistingTemplate);
            Assert.False(_ResultNonExistingTemplate);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenEmailTemplateDoesNotExist()
        {
            // Arrange
            __EmailTemplateRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(false);

            // Act
            bool _Result = await __EmailTemplateRepositoryMock.Object.DeleteAsync(Guid.NewGuid());

            // Assert
            Assert.False(_Result);
        }
    }
}
