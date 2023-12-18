using Moq;
using MSMConfigUtil.Logic.CalculationModelMigration.NodesConversion;
using Newtonsoft.Json.Linq;
using System;

namespace MSMConfigUtil.Logic.UnitTests
{
    [TestFixture]
    public class EstimationNodeConverterTests
    {
        private EstimationNodeConverter estimationNodeConverter;
        private Mock<IIdConverter> idConverterMock;
        private Mock<IModelDefinitionHelper> modelDefinitionHelperMock;

        string sourceLibraryId = "00000000-0000-0000-0000-000000000001";
        string sourceFactorId = "00000000-0000-0000-0000-000000000002";
        string expectedDestinationLibraryId = "00000000-0000-0000-0000-000000000003";
        string expectedDestinationEstimationFactorId = "00000000-0000-0000-0000-000000000004";

        [SetUp]
        public void Setup()
        {
            // Initialize dependencies
            idConverterMock = new Mock<IIdConverter>();
            modelDefinitionHelperMock = new Mock<IModelDefinitionHelper>();
            estimationNodeConverter = new EstimationNodeConverter(idConverterMock.Object, modelDefinitionHelperMock.Object);
        }

        [Test]
        public void SupportedActionTypes_ReturnsCorrectValue()
        {
            // Act
            var result = estimationNodeConverter.SupportedActionTypes;

            // Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First(), Is.EqualTo("EstimationFactor"));
        }

        [Test]
        public void SupportsAnyNode_ReturnsFalse()
        {
            // Act
            var result = estimationNodeConverter.SupportsAnyNode;
            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void InspectAndConvert_SetsCorrectPropertyValueForCalculationLibrary()
        {
            // Arrange
            var node = new JObject();
            modelDefinitionHelperMock.Setup(m => m.GetPropertyValue(It.IsAny<JObject>(), "estimationFactorLibraryId")).Returns(sourceLibraryId);
            modelDefinitionHelperMock.Setup(m => m.GetPropertyValue(It.IsAny<JObject>(), "estimationFactorId")).Returns(sourceFactorId);
            modelDefinitionHelperMock.Setup(m => m.IsGuid(sourceLibraryId)).Returns(true);
            modelDefinitionHelperMock.Setup(m => m.IsGuid(sourceFactorId)).Returns(true);
            idConverterMock.Setup(m => m.ConvertIdToDestinationEnvironment<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(new Guid(expectedDestinationLibraryId));
            idConverterMock.Setup(m => m.ConvertIdToDestinationEnvironment<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<IEnumerable<KeyValuePair<string, object>>>()))
                .Returns(new Guid(expectedDestinationEstimationFactorId));

            // Act
            estimationNodeConverter.InspectAndConvert(node);

            // Assert
            modelDefinitionHelperMock.Verify(m => m.SetPropertyValue(node, "estimationFactorLibraryId", expectedDestinationLibraryId));
        }

        [Test]
        public void InspectAndConvert_GetsCorrectPropertiesFromSource()
        {
            // Arrange
            var node = new JObject();
            modelDefinitionHelperMock.Setup(m => m.GetPropertyValue(It.IsAny<JObject>(), "estimationFactorLibraryId")).Returns(sourceLibraryId);
            modelDefinitionHelperMock.Setup(m => m.GetPropertyValue(It.IsAny<JObject>(), "estimationFactorId")).Returns(sourceFactorId);
            modelDefinitionHelperMock.Setup(m => m.IsGuid(sourceLibraryId)).Returns(true);
            modelDefinitionHelperMock.Setup(m => m.IsGuid(sourceFactorId)).Returns(true);
            idConverterMock.Setup(m => m.ConvertIdToDestinationEnvironment<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(new Guid(expectedDestinationLibraryId));
            idConverterMock.Setup(m => m.ConvertIdToDestinationEnvironment<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<IEnumerable<KeyValuePair<string, object>>>()))
                .Returns(new Guid(expectedDestinationEstimationFactorId));

            // Act
            estimationNodeConverter.InspectAndConvert(node);

            // Assert
            modelDefinitionHelperMock.Verify(m => m.GetPropertyValue(node, "estimationFactorLibraryId"));
            modelDefinitionHelperMock.Verify(m => m.GetPropertyValue(node, "estimationFactorId"));
        }



        [Test]
        public void InspectAndConvert_SendsCorrectIdForGuidCheck()
        {
            // Arrange
            var node = new JObject();
            modelDefinitionHelperMock.Setup(m => m.GetPropertyValue(It.IsAny<JObject>(), "estimationFactorLibraryId")).Returns(sourceLibraryId);
            modelDefinitionHelperMock.Setup(m => m.GetPropertyValue(It.IsAny<JObject>(), "estimationFactorId")).Returns(sourceFactorId);
            modelDefinitionHelperMock.Setup(m => m.IsGuid(sourceLibraryId)).Returns(true);
            modelDefinitionHelperMock.Setup(m => m.IsGuid(sourceFactorId)).Returns(true);
            idConverterMock.Setup(m => m.ConvertIdToDestinationEnvironment<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(new Guid(expectedDestinationLibraryId));
            idConverterMock.Setup(m => m.ConvertIdToDestinationEnvironment<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<IEnumerable<KeyValuePair<string, object>>>()))
                .Returns(new Guid(expectedDestinationEstimationFactorId));

            // Act
            estimationNodeConverter.InspectAndConvert(node);

            // Assert
            modelDefinitionHelperMock.Verify(m => m.IsGuid(sourceLibraryId));
            modelDefinitionHelperMock.Verify(m => m.IsGuid(sourceFactorId));
        }

        [Test]
        public void InspectAndConvert_DoesNotConvertWhensourceFactorIdIsNotGuid()
        {
            // Arrange
            var node = new JObject();
            modelDefinitionHelperMock.Setup(m => m.GetPropertyValue(It.IsAny<JObject>(), "estimationFactorLibraryId")).Returns(sourceLibraryId);
            modelDefinitionHelperMock.Setup(m => m.GetPropertyValue(It.IsAny<JObject>(), "estimationFactorId")).Returns(sourceFactorId);
            modelDefinitionHelperMock.Setup(m => m.IsGuid(sourceLibraryId)).Returns(true);
            modelDefinitionHelperMock.Setup(m => m.IsGuid(sourceFactorId)).Returns(false);
            idConverterMock.Setup(m => m.ConvertIdToDestinationEnvironment<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(new Guid(expectedDestinationLibraryId));
            idConverterMock.Setup(m => m.ConvertIdToDestinationEnvironment<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<IEnumerable<KeyValuePair<string, object>>>()))
                .Returns(new Guid(expectedDestinationEstimationFactorId));

            // Act
            estimationNodeConverter.InspectAndConvert(node);

            // Assert
            idConverterMock.Verify(m => m.ConvertIdToDestinationEnvironment<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<IEnumerable<KeyValuePair<string, object>>>()),
                Times.Never);
            modelDefinitionHelperMock.Verify(m => m.SetPropertyValue(It.IsAny<JObject>(), "estimationFactorId", It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void InspectAndConvert_CallsConvertIdToDestinationEnvironmentWithCorrectParameters()
        {
            // Arrange
            var node = new JObject();
            modelDefinitionHelperMock.Setup(m => m.GetPropertyValue(It.IsAny<JObject>(), "estimationFactorLibraryId")).Returns(sourceLibraryId);
            modelDefinitionHelperMock.Setup(m => m.GetPropertyValue(It.IsAny<JObject>(), "estimationFactorId")).Returns(sourceFactorId);
            modelDefinitionHelperMock.Setup(m => m.IsGuid(sourceLibraryId)).Returns(true);
            modelDefinitionHelperMock.Setup(m => m.IsGuid(sourceFactorId)).Returns(true);
            idConverterMock.Setup(m => m.ConvertIdToDestinationEnvironment<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(new Guid(expectedDestinationLibraryId));
            idConverterMock.Setup(m => m.ConvertIdToDestinationEnvironment<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<IEnumerable<KeyValuePair<string, object>>>()))
                .Returns(new Guid(expectedDestinationEstimationFactorId));

            // Act
            estimationNodeConverter.InspectAndConvert(node);

            // Assert
            idConverterMock.Verify(m => m.ConvertIdToDestinationEnvironment<string>("msdyn_calculationlibrary", It.Is<Guid>(g => g.ToString() == sourceLibraryId), "msdyn_name"));

            idConverterMock.Verify(m => m.ConvertIdToDestinationEnvironment<string>("msdyn_estimationfactor", It.Is<Guid>(g => g.ToString() == sourceFactorId), "msdyn_name",
                It.Is<IEnumerable<KeyValuePair<string, object>>>(e =>
                    e.All(i => i.Key == "msdyn_factorlibrary" && i.Value.ToString() == expectedDestinationLibraryId))));

        }

    }
}
