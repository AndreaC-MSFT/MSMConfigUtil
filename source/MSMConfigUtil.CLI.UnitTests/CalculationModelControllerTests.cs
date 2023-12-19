using Microsoft.Xrm.Sdk;
using MSMConfigUtil.Logic;
using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MSMConfigUtil.CLI.Tests
{
    [TestFixture]
    public class CalculationModelControllerTests
    {
        private Mock<ICalculationModelMigratorFactory> calcModelMigratorFactoryMock;
        private Mock<ICalculationModelMigrator> calcModelMigratorMock;
        private Mock<IUserInterfaceHandler> userInterfaceHandlerMock;
        private CalculationModelController calculationModelController;

        [SetUp]
        public void Setup()
        {
            calcModelMigratorFactoryMock = new Mock<ICalculationModelMigratorFactory>();
            calcModelMigratorMock = new Mock<ICalculationModelMigrator>();
            userInterfaceHandlerMock = new Mock<IUserInterfaceHandler>();
            calculationModelController = new CalculationModelController(calcModelMigratorFactoryMock.Object, userInterfaceHandlerMock.Object);
        }

        [Test]
        public void MigrateCalculationModel_Should_CallCorrectMigrateOverload_When_MigrateAllModelsIsSpecified()
        {
            // Arrange
            var globalOptions = new GlobalCLIOptions();
            var migrateModelsOptions = new MigrateModelsCLIOptions
            {
                MigrateAllModels = true,
                ReplaceExistingModels = true
            };
            calcModelMigratorFactoryMock.Setup(m => m.Create(globalOptions)).Returns(calcModelMigratorMock.Object);

            // Act
            calculationModelController.MigrateCalculationModel(globalOptions, migrateModelsOptions);

            // Assert
            calcModelMigratorMock.Verify(x => x.Migrate(migrateModelsOptions.ReplaceExistingModels), Times.Once);
        }

        [Test]
        public void MigrateCalculationModel_Should_CallCorrectMigrateOverload_When_ModelNameIsSpecified()
        {
            // Arrange
            var globalOptions = new GlobalCLIOptions();
            var migrateModelsOptions = new MigrateModelsCLIOptions
            {
                MigrateAllModels = false,
                ReplaceExistingModels = true,
                CalculationModelName = "Test model"
            };
            calcModelMigratorFactoryMock.Setup(m => m.Create(globalOptions)).Returns(calcModelMigratorMock.Object);

            // Act
            calculationModelController.MigrateCalculationModel(globalOptions, migrateModelsOptions);

            // Assert
            calcModelMigratorMock.Verify(x => x.Migrate("Test model", migrateModelsOptions.ReplaceExistingModels), Times.Once);
        }

        [Test]
        public void MigrateCalculationModel_Should_GenerateUIError_When_MigrateAllIsFalseAndModelNameIsNotSpecified()
        {
            // Arrange
            var globalOptions = new GlobalCLIOptions();
            var migrateModelsOptions = new MigrateModelsCLIOptions
            {
                MigrateAllModels = false,
                ReplaceExistingModels = true
            };
            calcModelMigratorFactoryMock.Setup(m => m.Create(globalOptions)).Returns(calcModelMigratorMock.Object);

            // Act
            calculationModelController.MigrateCalculationModel(globalOptions, migrateModelsOptions);

            // Assert
            userInterfaceHandlerMock.Verify(m => m.ShowError(It.IsAny<string>()));
        }

    }
}
