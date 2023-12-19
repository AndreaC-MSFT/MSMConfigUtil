using Moq;
using MSMConfigUtil.Logic.CalculationModelMigration;
using NUnit.Framework;

namespace MSMConfigUtil.Logic.UnitTests
{
    [TestFixture]
    public class CalculationModelMigratorTests
    {
        private Mock<ICalculationModelReader> sourceCalculationModelReaderMock;
        private Mock<ICalculationModelReader> destinationCalculationModelReaderMock;
        private Mock<IModelDefinitionConverter> modelDefinitionConverterMock;
        private Mock<ICalculationModelWriter> calculationModelWriterMock;
        private Mock<IUserInterfaceHandler> userInterfaceHandlerMock;
        private CalculationModelMigrator calculationModelMigrator;

        [SetUp]
        public void Setup()
        {
            sourceCalculationModelReaderMock = new Mock<ICalculationModelReader>();
            destinationCalculationModelReaderMock = new Mock<ICalculationModelReader>();
            modelDefinitionConverterMock = new Mock<IModelDefinitionConverter>();
            calculationModelWriterMock = new Mock<ICalculationModelWriter>();
            userInterfaceHandlerMock = new Mock<IUserInterfaceHandler>();

            calculationModelMigrator = new CalculationModelMigrator(
                sourceCalculationModelReaderMock.Object,
                destinationCalculationModelReaderMock.Object,
                modelDefinitionConverterMock.Object,
                calculationModelWriterMock.Object,
                userInterfaceHandlerMock.Object
            );
        }

        [Test]
        public void Migrate_All_Should_SendsRecusriveCallsForEachModel()
        {
            // Arrange
            var sourceModels = new CalculationModel[] {
                new() { Name = "Model 1"},
                new() { Name = "Model 2"} };
            sourceCalculationModelReaderMock.Setup(r => r.GetAll()).Returns(sourceModels);

            // Act
            calculationModelMigrator.Migrate(true);

            // Assert
            destinationCalculationModelReaderMock.Verify(m => m.GetId("Model 1"), Times.Once);
            destinationCalculationModelReaderMock.Verify(m => m.GetId("Model 2"), Times.Once);
        }

        [Test]
        public void Migrate_ByName_Should_GetModelFromReader()
        {
            // Arrange
            var sourceModel = new CalculationModel { Name = "Model 1"};
            sourceCalculationModelReaderMock.Setup(r => r.Get(It.IsAny<string>())).Returns(sourceModel);

            // Act
            calculationModelMigrator.Migrate("Model 1", true);

            // Assert
            sourceCalculationModelReaderMock.Verify(r => r.Get("Model 1"));
        }

        [Test]
        public void Migrate_ByModel_Should_CreateWhenModelDoesNotExists()
        {
            // Arrange
            var sourceModel = new CalculationModel { Name = "Model 1" };
            destinationCalculationModelReaderMock.Setup(m => m.GetId(It.IsAny<string>())).Returns((Guid?)null);
            modelDefinitionConverterMock.Setup(m => m.Convert(It.IsAny<string>())).Returns("ConvertedJson");

            // Act
            calculationModelMigrator.Migrate(sourceModel, false);

            // Assert
            calculationModelWriterMock.Verify(r => r.Create(sourceModel));
        }

        [Test]
        public void Migrate_ByModel_Should_UpdateWhenModelExistsAndReplaceParameterIsTrue()
        {
            // Arrange
            var sourceModel = new CalculationModel { Name = "Model 1" };
            destinationCalculationModelReaderMock.Setup(m => m.GetId(It.IsAny<string>())).Returns(Guid.NewGuid());
            modelDefinitionConverterMock.Setup(m => m.Convert(It.IsAny<string>())).Returns("ConvertedJson");

            // Act
            calculationModelMigrator.Migrate(sourceModel, true);

            // Assert
            calculationModelWriterMock.Verify(r => r.Update(sourceModel));
        }

        [Test]
        public void Migrate_ByModel_Should_PassDestinationIdToUpdate()
        {
            // Arrange
            var sourceModel = new CalculationModel { Id = Guid.NewGuid().ToString(), Name = "Model 1" };
            var destinationId = Guid.NewGuid();
            destinationCalculationModelReaderMock.Setup(m => m.GetId(It.IsAny<string>())).Returns(destinationId);
            modelDefinitionConverterMock.Setup(m => m.Convert(It.IsAny<string>())).Returns("ConvertedJson");

            // Act
            calculationModelMigrator.Migrate(sourceModel, true);

            // Assert
            calculationModelWriterMock.Verify(r => r.Update(It.Is<CalculationModel>(m => m.Id == destinationId.ToString())));
        }

        [Test]
        public void Migrate_ByModel_Should_NotUpdateWhenModelExistsAndReplaceParameterIsFalse()
        {
            // Arrange
            var sourceModel = new CalculationModel { Name = "Model 1" };
            var destinationId = Guid.NewGuid();
            destinationCalculationModelReaderMock.Setup(m => m.GetId(It.IsAny<string>())).Returns(destinationId);
            modelDefinitionConverterMock.Setup(m => m.Convert(It.IsAny<string>())).Returns("ConvertedJson");

            // Act
            calculationModelMigrator.Migrate(sourceModel, false);

            // Assert
            calculationModelWriterMock.Verify(r => r.Update(It.IsAny<CalculationModel>()), Times.Never);
        }

        [Test]
        public void Migrate_ByModel_Should_PassConvertedJsonDefinitionToWriter()
        {
            // Arrange
            var sourceModel = new CalculationModel { Name = "Model 1" };
            destinationCalculationModelReaderMock.Setup(m => m.GetId(It.IsAny<string>())).Returns((Guid?)null);
            modelDefinitionConverterMock.Setup(m => m.Convert(It.IsAny<string>())).Returns("ConvertedJson");

            // Act
            calculationModelMigrator.Migrate(sourceModel, false);

            // Assert
            calculationModelWriterMock.Verify(r => r.Create(It.Is<CalculationModel>(c => c.JsonDefinition == "ConvertedJson")));
        }

        [Test]
        public void Migrate_ByModel_Should_CheckModelExistenceByName()
        {
            // Arrange
            var sourceModel = new CalculationModel { Name = "Model 1" };
            destinationCalculationModelReaderMock.Setup(m => m.GetId(It.IsAny<string>())).Returns((Guid?)null);
            modelDefinitionConverterMock.Setup(m => m.Convert(It.IsAny<string>())).Returns("ConvertedJson");

            // Act
            calculationModelMigrator.Migrate(sourceModel, false);

            // Assert
            destinationCalculationModelReaderMock.Verify(m => m.GetId("Model 1"));
        }
    }
}
