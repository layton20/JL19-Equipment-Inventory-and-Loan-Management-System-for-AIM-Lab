using Bogus;
using ELMS.WEB.Entities.Admin;
using ELMS.WEB.Enums.Configuration;
using ELMS.WEB.Repositories.Admin.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ELMS.XUNIT.Admin
{
    public class ConfigurationUnitTest
    {
        private readonly Mock<IConfigurationRepository> __ConfigurationRepositoryMock = new Mock<IConfigurationRepository>();

        [Fact]
        public async Task CreateAsync_ShouldReturnConfiguration_WhenConfigurationIsValid()
        {
            // Arrange
            Faker<ConfigurationEntity> _ConfigurationFaker = new Faker<ConfigurationEntity>()
                 .RuleFor(o => o.Name, f => f.Lorem.Word())
                 .RuleFor(o => o.Description, f => f.Lorem.Sentence())
                 .RuleFor(o => o.Value, f => f.Lorem.Sentence())
                 .RuleFor(o => o.Type, f => f.PickRandom<ConfigurationType>());

            ConfigurationEntity _ConfigurationFake = _ConfigurationFaker.Generate();

            __ConfigurationRepositoryMock.Setup(x => x.CreateAsync(_ConfigurationFake)).ReturnsAsync(_ConfigurationFake);

            // Act
            ConfigurationEntity _Configuration = await __ConfigurationRepositoryMock.Object.CreateAsync(_ConfigurationFake);

            // Assert
            Assert.Equal(_ConfigurationFake, _Configuration);
        }

        [Fact]
        public async Task CreateAsync_ShouldNotReturnConfiguration_WhenConfigurationIsNotValid()
        {
            // Arrange
            __ConfigurationRepositoryMock.Setup(x => x.CreateAsync(It.Is<ConfigurationEntity>(x => x.UID == Guid.Empty))).ReturnsAsync(() => null);

            // Act
            ConfigurationEntity _Configuration = await __ConfigurationRepositoryMock.Object.CreateAsync(new ConfigurationEntity { UID = Guid.Empty });

            // Assert
            Assert.Null(_Configuration);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenConfigurationExists()
        {
            // Arrange
            Faker<ConfigurationEntity> _ConfigurationFaker = new Faker<ConfigurationEntity>()
                 .RuleFor(o => o.Name, f => f.Lorem.Word())
                 .RuleFor(o => o.Description, f => f.Lorem.Sentence())
                 .RuleFor(o => o.Value, f => f.Lorem.Sentence())
                 .RuleFor(o => o.Type, f => f.PickRandom<ConfigurationType>());

            IList<ConfigurationEntity> _ConfigurationFakes = _ConfigurationFaker.Generate(5);

            __ConfigurationRepositoryMock.Setup(x => x.DeleteAsync(It.Is<Guid>(x => _ConfigurationFakes.Select(c => c.UID).Contains(x)))).ReturnsAsync(true);

            // Act
            bool _Result = await __ConfigurationRepositoryMock.Object.DeleteAsync(_ConfigurationFakes[0].UID);

            // Assert
            Assert.True(_Result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenConfigurationDoesNotExist()
        {
            // Arrange
            Faker<ConfigurationEntity> _ConfigurationFaker = new Faker<ConfigurationEntity>()
                 .RuleFor(o => o.Name, f => f.Lorem.Word())
                 .RuleFor(o => o.Description, f => f.Lorem.Sentence())
                 .RuleFor(o => o.Value, f => f.Lorem.Sentence())
                 .RuleFor(o => o.Type, f => f.PickRandom<ConfigurationType>());

            IList<ConfigurationEntity> _ConfigurationFakes = _ConfigurationFaker.Generate(5);

            __ConfigurationRepositoryMock.Setup(x => x.DeleteAsync(Guid.Empty)).ReturnsAsync(false);
            __ConfigurationRepositoryMock.Setup(x => x.DeleteAsync(It.Is<Guid>(x => !_ConfigurationFakes.Select(c => c.UID).Contains(x)))).ReturnsAsync(false);

            // Act
            bool _ResultNonExisting = await __ConfigurationRepositoryMock.Object.DeleteAsync(Guid.NewGuid());
            bool _ResultEmptyGuid = await __ConfigurationRepositoryMock.Object.DeleteAsync(Guid.Empty);

            // Assert
            Assert.False(_ResultNonExisting);
            Assert.False(_ResultEmptyGuid);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnConfigurations_WhenConfigurationsExists()
        {
            // Arrange
            Faker<ConfigurationEntity> _ConfigurationFaker = new Faker<ConfigurationEntity>()
                 .RuleFor(o => o.Name, f => f.Lorem.Word())
                 .RuleFor(o => o.Description, f => f.Lorem.Sentence())
                 .RuleFor(o => o.Value, f => f.Lorem.Sentence())
                 .RuleFor(o => o.Type, f => f.PickRandom<ConfigurationType>());

            IList<ConfigurationEntity> _ConfigurationFakes = _ConfigurationFaker.Generate(5);

            __ConfigurationRepositoryMock.Setup(x => x.GetAsync()).ReturnsAsync(_ConfigurationFakes);

            // Act
            IList<ConfigurationEntity> _Configurations = await __ConfigurationRepositoryMock.Object.GetAsync();

            // Assert
            Assert.Equal(_ConfigurationFakes, _Configurations);
        }

        [Fact]
        public async Task GetAsync_ShouldNotReturnConfigurations_WhenConfigurationsDoNotExist()
        {
            // Arrange
            __ConfigurationRepositoryMock.Setup(x => x.GetAsync()).ReturnsAsync(() => null);

            // Act
            IList<ConfigurationEntity> _Configurations = await __ConfigurationRepositoryMock.Object.GetAsync();

            // Assert
            Assert.Null(_Configurations);
        }

        [Fact]
        public async Task GetByUIDAsync_ShouldReturnConfiguration_WhenConfigurationExists()
        {
            // Arrange
            Faker<ConfigurationEntity> _ConfigurationFaker = new Faker<ConfigurationEntity>()
                 .RuleFor(o => o.Name, f => f.Lorem.Word())
                 .RuleFor(o => o.Description, f => f.Lorem.Sentence())
                 .RuleFor(o => o.Value, f => f.Lorem.Sentence())
                 .RuleFor(o => o.Type, f => f.PickRandom<ConfigurationType>());

            ConfigurationEntity _ConfigurationFake = _ConfigurationFaker.Generate();

            __ConfigurationRepositoryMock.Setup(x => x.GetByUIDAsync(_ConfigurationFake.UID)).ReturnsAsync(_ConfigurationFake);

            // Act
            ConfigurationEntity _Configuration = await __ConfigurationRepositoryMock.Object.GetByUIDAsync(_ConfigurationFake.UID);

            // Assert
            Assert.Equal(_ConfigurationFake, _Configuration);
        }

        [Fact]
        public async Task GetByUIDAsync_ShouldNotReturnConfiguration_WhenConfigurationDoesNotExist()
        {
            // Arrange
            Faker<ConfigurationEntity> _ConfigurationFaker = new Faker<ConfigurationEntity>()
                 .RuleFor(o => o.Name, f => f.Lorem.Word())
                 .RuleFor(o => o.Description, f => f.Lorem.Sentence())
                 .RuleFor(o => o.Value, f => f.Lorem.Sentence())
                 .RuleFor(o => o.Type, f => f.PickRandom<ConfigurationType>());

            IList<ConfigurationEntity> _ConfigurationFakes = _ConfigurationFaker.Generate(5);

            __ConfigurationRepositoryMock.Setup(x => x.GetByUIDAsync(It.IsAny<Guid>())).ReturnsAsync((Guid uid) => _ConfigurationFakes.FirstOrDefault(x => x.UID == uid));

            // Act
            ConfigurationEntity _Configuration = await __ConfigurationRepositoryMock.Object.GetByUIDAsync(Guid.NewGuid());

            // Assert
            Assert.Null(_Configuration);
        }

        [Fact]
        public async Task GetByNormalizedNameAsync_ShouldReturnConfiguration_WhenConfigurationExists()
        {
            // Arrange
            Faker<ConfigurationEntity> _ConfigurationFaker = new Faker<ConfigurationEntity>()
                .RuleFor(o => o.Name, f => f.Lorem.Word())
                .RuleFor(o => o.Description, f => f.Lorem.Sentence())
                .RuleFor(o => o.Value, f => f.Lorem.Sentence())
                .RuleFor(o => o.Type, f => f.PickRandom<ConfigurationType>());

            ConfigurationEntity _ConfigurationFake = _ConfigurationFaker.Generate();

            __ConfigurationRepositoryMock.Setup(x => x.GetByNormalizedNameAsync(_ConfigurationFake.Name)).ReturnsAsync(_ConfigurationFake);

            // Act
            ConfigurationEntity _Configuration = await __ConfigurationRepositoryMock.Object.GetByNormalizedNameAsync(_ConfigurationFake.Name);

            // Assert
            Assert.Equal(_ConfigurationFake, _Configuration);
        }

        [Fact]
        public async Task GetByNormalizedNameAsync_ShouldNotReturnConfiguration_WhenConfigurationDoesNotExist()
        {
            // Arrange
            Faker<ConfigurationEntity> _ConfigurationFaker = new Faker<ConfigurationEntity>()
                .RuleFor(o => o.Name, f => f.Lorem.Word())
                .RuleFor(o => o.Description, f => f.Lorem.Sentence())
                .RuleFor(o => o.Value, f => f.Lorem.Sentence())
                .RuleFor(o => o.Type, f => f.PickRandom<ConfigurationType>());

            IList<ConfigurationEntity> _ConfigurationFakes = _ConfigurationFaker.Generate(5);

            __ConfigurationRepositoryMock.Setup(x => x.GetByNormalizedNameAsync(It.IsAny<string>())).ReturnsAsync((string name) => _ConfigurationFakes.FirstOrDefault(x => x.Name == name));

            // Act
            ConfigurationEntity _Configuration = await __ConfigurationRepositoryMock.Object.GetByNormalizedNameAsync(Guid.NewGuid().ToString());

            // Assert
            Assert.Null(_Configuration);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnTrue_WhenConfigurationExists()
        {
            // Arrange
            Faker<ConfigurationEntity> _ConfigurationFaker = new Faker<ConfigurationEntity>()
                .RuleFor(o => o.Name, f => f.Lorem.Word())
                .RuleFor(o => o.Description, f => f.Lorem.Sentence())
                .RuleFor(o => o.Value, f => f.Lorem.Sentence())
                .RuleFor(o => o.Type, f => f.PickRandom<ConfigurationType>());

            ConfigurationEntity _ConfigurationFake = _ConfigurationFaker.Generate();

            __ConfigurationRepositoryMock.Setup(x => x.UpdateAsync(_ConfigurationFake)).ReturnsAsync(true);

            // Act
            bool _Result = await __ConfigurationRepositoryMock.Object.UpdateAsync(_ConfigurationFake);

            // Assert
            Assert.True(_Result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnFalse_WhenConfigurationDoesNotExist()
        {
            // Arrange
            __ConfigurationRepositoryMock.Setup(x => x.UpdateAsync(It.Is<ConfigurationEntity>(x => x.UID == Guid.Empty))).ReturnsAsync(false);

            // Act
            bool _Result = await __ConfigurationRepositoryMock.Object.UpdateAsync(new ConfigurationEntity { UID = Guid.Empty });

            // Assert
            Assert.False(_Result);
        }
    }
}
