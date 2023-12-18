using Moq;
using MSMConfigUtil.Logic.CalculationModelMigration.NodesConversion;
using Newtonsoft.Json.Linq;
using System;

namespace MSMConfigUtil.Logic.UnitTests
{
    [TestFixture]
    public class ReportNodeConverterTests
    {
        private ReportNodeConverter reportNodeConverter;
        private Mock<IIdConverter> idConverterMock;
        private Mock<IModelDefinitionHelper> modelDefinitionHelperMock;

        string sourceLibraryId = "00000000-0000-0000-0000-000000000001";
        string sourceFactorId = "00000000-0000-0000-0000-000000000002";
        string expectedDestinationLibraryId = "00000000-0000-0000-0000-000000000003";
        string expectedDestinationEmissionFactorId = "00000000-0000-0000-0000-000000000004";

        [SetUp]
        public void Setup()
        {
            // Initialize dependencies
            idConverterMock = new Mock<IIdConverter>();
            modelDefinitionHelperMock = new Mock<IModelDefinitionHelper>();
            reportNodeConverter = new ReportNodeConverter(idConverterMock.Object, modelDefinitionHelperMock.Object);
        }

        [Test]
        public void SupportedActionTypes_ReturnsCorrectValue()
        {
            // Act
            var result = reportNodeConverter.SupportedActionTypes;

            // Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First(), Is.EqualTo("Report"));
        }
        [Test]
        public void SupportsAnyNode_ReturnsFalse()
        {
            // Act
            var result = reportNodeConverter.SupportsAnyNode;
            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void InspectAndConvert_SetsCorrectPropertyValueForCalculationLibrary()
        {
            // Arrange
            var node = new JObject();
            modelDefinitionHelperMock.Setup(m => m.GetPropertyValue(It.IsAny<JObject>(), "calculationLibraryId")).Returns(sourceLibraryId);
            modelDefinitionHelperMock.Setup(m => m.GetPropertyValue(It.IsAny<JObject>(), "emissionFactorId")).Returns(sourceFactorId);
            modelDefinitionHelperMock.Setup(m => m.IsGuid(sourceLibraryId)).Returns(true);
            modelDefinitionHelperMock.Setup(m => m.IsGuid(sourceFactorId)).Returns(true);
            idConverterMock.Setup(m => m.ConvertIdToDestinationEnvironment<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(new Guid(expectedDestinationLibraryId));
            idConverterMock.Setup(m => m.ConvertIdToDestinationEnvironment<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<IEnumerable<KeyValuePair<string, object>>>()))
                .Returns(new Guid(expectedDestinationEmissionFactorId));

            // Act
            reportNodeConverter.InspectAndConvert(node);

            // Assert
            modelDefinitionHelperMock.Verify(m => m.SetPropertyValue(node, "calculationLibraryId", expectedDestinationLibraryId));
        }

        [Test]
        public void InspectAndConvert_GetsCorrectPropertiesFromSource()
        {
            // Arrange
            var node = new JObject();
            modelDefinitionHelperMock.Setup(m => m.GetPropertyValue(It.IsAny<JObject>(), "calculationLibraryId")).Returns(sourceLibraryId);
            modelDefinitionHelperMock.Setup(m => m.GetPropertyValue(It.IsAny<JObject>(), "emissionFactorId")).Returns(sourceFactorId);
            modelDefinitionHelperMock.Setup(m => m.IsGuid(sourceLibraryId)).Returns(true);
            modelDefinitionHelperMock.Setup(m => m.IsGuid(sourceFactorId)).Returns(true);
            idConverterMock.Setup(m => m.ConvertIdToDestinationEnvironment<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(new Guid(expectedDestinationLibraryId));
            idConverterMock.Setup(m => m.ConvertIdToDestinationEnvironment<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<IEnumerable<KeyValuePair<string, object>>>()))
                .Returns(new Guid(expectedDestinationEmissionFactorId));

            // Act
            reportNodeConverter.InspectAndConvert(node);

            // Assert
            modelDefinitionHelperMock.Verify(m => m.GetPropertyValue(node, "calculationLibraryId"));
            modelDefinitionHelperMock.Verify(m => m.GetPropertyValue(node, "emissionFactorId"));
        }



        [Test]
        public void InspectAndConvert_SendsCorrectIdForGuidCheck()
        {
            // Arrange
            var node = new JObject();
            modelDefinitionHelperMock.Setup(m => m.GetPropertyValue(It.IsAny<JObject>(), "calculationLibraryId")).Returns(sourceLibraryId);
            modelDefinitionHelperMock.Setup(m => m.GetPropertyValue(It.IsAny<JObject>(), "emissionFactorId")).Returns(sourceFactorId);
            modelDefinitionHelperMock.Setup(m => m.IsGuid(sourceLibraryId)).Returns(true);
            modelDefinitionHelperMock.Setup(m => m.IsGuid(sourceFactorId)).Returns(true);
            idConverterMock.Setup(m => m.ConvertIdToDestinationEnvironment<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(new Guid(expectedDestinationLibraryId));
            idConverterMock.Setup(m => m.ConvertIdToDestinationEnvironment<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<IEnumerable<KeyValuePair<string, object>>>()))
                .Returns(new Guid(expectedDestinationEmissionFactorId));

            // Act
            reportNodeConverter.InspectAndConvert(node);

            // Assert
            modelDefinitionHelperMock.Verify(m => m.IsGuid(sourceLibraryId));
            modelDefinitionHelperMock.Verify(m => m.IsGuid(sourceFactorId));
        }

        [Test]
        public void InspectAndConvert_DoesNotConvertWhensourceFactorIdIsNotGuid()
        {
            // Arrange
            var node = new JObject();
            modelDefinitionHelperMock.Setup(m => m.GetPropertyValue(It.IsAny<JObject>(), "calculationLibraryId")).Returns(sourceLibraryId);
            modelDefinitionHelperMock.Setup(m => m.GetPropertyValue(It.IsAny<JObject>(), "emissionFactorId")).Returns(sourceFactorId);
            modelDefinitionHelperMock.Setup(m => m.IsGuid(sourceLibraryId)).Returns(true);
            modelDefinitionHelperMock.Setup(m => m.IsGuid(sourceFactorId)).Returns(false);
            idConverterMock.Setup(m => m.ConvertIdToDestinationEnvironment<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(new Guid(expectedDestinationLibraryId));
            idConverterMock.Setup(m => m.ConvertIdToDestinationEnvironment<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<IEnumerable<KeyValuePair<string, object>>>()))
                .Returns(new Guid(expectedDestinationEmissionFactorId));

            // Act
            reportNodeConverter.InspectAndConvert(node);

            // Assert
            idConverterMock.Verify(m => m.ConvertIdToDestinationEnvironment<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<IEnumerable<KeyValuePair<string, object>>>()),
                Times.Never);
            modelDefinitionHelperMock.Verify(m => m.SetPropertyValue(It.IsAny<JObject>(), "emissionFactorId", It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void InspectAndConvert_CallsConvertIdToDestinationEnvironmentWithCorrectParameters()
        {
            // Arrange
            var node = new JObject();
            modelDefinitionHelperMock.Setup(m => m.GetPropertyValue(It.IsAny<JObject>(), "calculationLibraryId")).Returns(sourceLibraryId);
            modelDefinitionHelperMock.Setup(m => m.GetPropertyValue(It.IsAny<JObject>(), "emissionFactorId")).Returns(sourceFactorId);
            modelDefinitionHelperMock.Setup(m => m.IsGuid(sourceLibraryId)).Returns(true);
            modelDefinitionHelperMock.Setup(m => m.IsGuid(sourceFactorId)).Returns(true);
            idConverterMock.Setup(m => m.ConvertIdToDestinationEnvironment<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(new Guid(expectedDestinationLibraryId));
            idConverterMock.Setup(m => m.ConvertIdToDestinationEnvironment<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<IEnumerable<KeyValuePair<string, object>>>()))
                .Returns(new Guid(expectedDestinationEmissionFactorId));

            // Act
            reportNodeConverter.InspectAndConvert(node);

            // Assert
            idConverterMock.Verify(m => m.ConvertIdToDestinationEnvironment<string>("msdyn_calculationlibrary", It.Is<Guid>(g => g.ToString() == sourceLibraryId), "msdyn_name"));

            idConverterMock.Verify(m => m.ConvertIdToDestinationEnvironment<string>("msdyn_emissionfactor", It.Is<Guid>(g => g.ToString() == sourceFactorId), "msdyn_name",
                It.Is<IEnumerable<KeyValuePair<string, object>>>(e =>
                    e.All(i => i.Key == "msdyn_calculationlibraryid" && i.Value.ToString() == expectedDestinationLibraryId))));

        }

    }
}
