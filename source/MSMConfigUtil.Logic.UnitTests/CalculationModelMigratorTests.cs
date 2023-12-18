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
        private CalculationModelMigrator calculationModelMigrator;

        [SetUp]
        public void Setup()
        {
            sourceCalculationModelReaderMock = new Mock<ICalculationModelReader>();
            destinationCalculationModelReaderMock = new Mock<ICalculationModelReader>();
            modelDefinitionConverterMock = new Mock<IModelDefinitionConverter>();
            calculationModelWriterMock = new Mock<ICalculationModelWriter>();

            calculationModelMigrator = new CalculationModelMigrator(
                sourceCalculationModelReaderMock.Object,
                destinationCalculationModelReaderMock.Object,
                modelDefinitionConverterMock.Object,
                calculationModelWriterMock.Object
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
            destinationCalculationModelReaderMock.Verify(m => m.Exists("Model 1"), Times.Once);
            destinationCalculationModelReaderMock.Verify(m => m.Exists("Model 2"), Times.Once);
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
        public void Migrate_ByModel_Should_UpsertWhenModelDoesNotExists()
        {
            // Arrange
            var sourceModel = new CalculationModel { Name = "Model 1" };
            destinationCalculationModelReaderMock.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);
            modelDefinitionConverterMock.Setup(m => m.Convert(It.IsAny<string>())).Returns("ConvertedJson");

            // Act
            calculationModelMigrator.Migrate(sourceModel, false);

            // Assert
            calculationModelWriterMock.Verify(r => r.Upsert(sourceModel));
        }

        [Test]
        public void Migrate_ByModel_Should_UpsertWhenModelExistsAndReplaceParameterIsTrue()
        {
            // Arrange
            var sourceModel = new CalculationModel { Name = "Model 1" };
            destinationCalculationModelReaderMock.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);
            modelDefinitionConverterMock.Setup(m => m.Convert(It.IsAny<string>())).Returns("ConvertedJson");

            // Act
            calculationModelMigrator.Migrate(sourceModel, true);

            // Assert
            calculationModelWriterMock.Verify(r => r.Upsert(sourceModel));
        }

        [Test]
        public void Migrate_ByModel_Should_NotUpsertWhenModelExistsAndReplaceParameterIsFalse()
        {
            // Arrange
            var sourceModel = new CalculationModel { Name = "Model 1" };
            destinationCalculationModelReaderMock.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);
            modelDefinitionConverterMock.Setup(m => m.Convert(It.IsAny<string>())).Returns("ConvertedJson");

            // Act
            calculationModelMigrator.Migrate(sourceModel, false);

            // Assert
            calculationModelWriterMock.Verify(r => r.Upsert(It.IsAny<CalculationModel>()), Times.Never);
        }

        [Test]
        public void Migrate_ByModel_Should_PassConvertedJsonDefinitionToWriter()
        {
            // Arrange
            var sourceModel = new CalculationModel { Name = "Model 1" };
            destinationCalculationModelReaderMock.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);
            modelDefinitionConverterMock.Setup(m => m.Convert(It.IsAny<string>())).Returns("ConvertedJson");

            // Act
            calculationModelMigrator.Migrate(sourceModel, false);

            // Assert
            calculationModelWriterMock.Verify(r => r.Upsert(It.Is<CalculationModel>(c => c.JsonDefinition == "ConvertedJson")));
        }

        [Test]
        public void Migrate_ByModel_Should_CheckModelExistenceByName()
        {
            // Arrange
            var sourceModel = new CalculationModel { Name = "Model 1" };
            destinationCalculationModelReaderMock.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);
            modelDefinitionConverterMock.Setup(m => m.Convert(It.IsAny<string>())).Returns("ConvertedJson");

            // Act
            calculationModelMigrator.Migrate(sourceModel, false);

            // Assert
            destinationCalculationModelReaderMock.Verify(m => m.Exists("Model 1"));
        }
    }
}
