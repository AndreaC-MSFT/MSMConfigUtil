using Moq;
using MSMConfigUtil.Logic.CalculationModelMigration.NodesConversion;
using Newtonsoft.Json.Linq;
using System;

namespace MSMConfigUtil.Logic.UnitTests
{
    [TestFixture]
    public class UnitOfMeasureNodeConverterTests
    {
        private UnitOfMeasureNodeConverter unitConverter;
        private Mock<IIdConverter> idConverterMock;
        private Mock<IModelDefinitionHelper> modelDefinitionHelperMock;

        string sourceId = "00000000-0000-0000-0000-000000000001";
        string destinationId = "00000000-0000-0000-0000-000000000002";


        [SetUp]
        public void Setup()
        {
            // Initialize dependencies
            idConverterMock = new Mock<IIdConverter>();
            modelDefinitionHelperMock = new Mock<IModelDefinitionHelper>();
            unitConverter = new UnitOfMeasureNodeConverter(idConverterMock.Object, modelDefinitionHelperMock.Object);
        }


        [Test]
        public void SupportedActionTypes_ReturnsEmptyArray()
        {
            // Act
            var result = unitConverter.SupportedActionTypes;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(0));
        }

        [Test]
        public void SupportsAnyNode_ReturnsTrue()
        {
            // Act
            var result = unitConverter.SupportsAnyNode;
            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void InspectAndConvert_SetsCorrectPropertyValue()
        {
            // Arrange
            var node = new JObject();
            modelDefinitionHelperMock.Setup(m => m.GetPropertyValue(It.IsAny<JObject>(), It.IsAny<string>())).Returns(sourceId);
            modelDefinitionHelperMock.Setup(m => m.IsGuid(It.IsAny<string>())).Returns(true);
            idConverterMock.Setup(m => m.ConvertIdToDestinationEnvironment<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>())).Returns(new Guid(destinationId));

            // Act
            unitConverter.InspectAndConvert(node);

            // Assert
            modelDefinitionHelperMock.Verify(m => m.SetPropertyValue(node, "unitOfMeasureId", destinationId));
            modelDefinitionHelperMock.Verify(m => m.SetPropertyValue(node, "unit", destinationId));
            modelDefinitionHelperMock.Verify(m => m.SetPropertyValue(node, "unitOfMeasureIdForQuantity", destinationId));
        }

        [Test]
        public void InspectAndConvert_DoesNotContaminateValueBetweenJsonProperies()
        {
            // Arrange
            string sourceId1 = "00000000-0000-0000-0000-000000000001";
            string sourceId2 = "00000000-0000-0000-0000-000000000002";
            string sourceId3 = "00000000-0000-0000-0000-000000000003";
            string destinationId1 = "00000000-0000-0000-0000-000000000004";
            string destinationId2 = "00000000-0000-0000-0000-000000000005";
            string destinationId3 = "00000000-0000-0000-0000-000000000006";

            var node = new JObject();
            modelDefinitionHelperMock.Setup(m => m.GetPropertyValue(It.IsAny<JObject>(), "unitOfMeasureId")).Returns(sourceId1);
            modelDefinitionHelperMock.Setup(m => m.GetPropertyValue(It.IsAny<JObject>(), "unit")).Returns(sourceId2);
            modelDefinitionHelperMock.Setup(m => m.GetPropertyValue(It.IsAny<JObject>(), "unitOfMeasureIdForQuantity")).Returns(sourceId3);
            modelDefinitionHelperMock.Setup(m => m.IsGuid(It.IsAny<string>())).Returns(true);
            idConverterMock.Setup(m => m.ConvertIdToDestinationEnvironment<string>(It.IsAny<string>(), It.Is<Guid>(g => g.ToString() == sourceId1), It.IsAny<string>())).Returns(new Guid(destinationId1));
            idConverterMock.Setup(m => m.ConvertIdToDestinationEnvironment<string>(It.IsAny<string>(), It.Is<Guid>(g => g.ToString() == sourceId2), It.IsAny<string>())).Returns(new Guid(destinationId2));
            idConverterMock.Setup(m => m.ConvertIdToDestinationEnvironment<string>(It.IsAny<string>(), It.Is<Guid>(g => g.ToString() == sourceId3), It.IsAny<string>())).Returns(new Guid(destinationId3));

            // Act
            unitConverter.InspectAndConvert(node);

            // Assert
            modelDefinitionHelperMock.Verify(m => m.SetPropertyValue(node, "unitOfMeasureId", destinationId1));
            modelDefinitionHelperMock.Verify(m => m.SetPropertyValue(node, "unit", destinationId2));
            modelDefinitionHelperMock.Verify(m => m.SetPropertyValue(node, "unitOfMeasureIdForQuantity", destinationId3));
        }

        [Test]
        public void InspectAndConvert_GetsCorrectPropertyFromSource()
        {
            // Arrange
            var node = new JObject();
            modelDefinitionHelperMock.Setup(m => m.GetPropertyValue(It.IsAny<JObject>(), It.IsAny<string>())).Returns(sourceId);
            modelDefinitionHelperMock.Setup(m => m.IsGuid(It.IsAny<string>())).Returns(true);
            idConverterMock.Setup(m => m.ConvertIdToDestinationEnvironment<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>())).Returns(new Guid(destinationId));

            // Act
            unitConverter.InspectAndConvert(node);

            // Assert
            modelDefinitionHelperMock.Verify(m => m.GetPropertyValue(node, "unitOfMeasureId"));
            modelDefinitionHelperMock.Verify(m => m.GetPropertyValue(node, "unit"));
            modelDefinitionHelperMock.Verify(m => m.GetPropertyValue(node, "unitOfMeasureIdForQuantity"));
        }

        [Test]
        public void InspectAndConvert_SendsCorrectIdForGuidCheck()
        {
            // Arrange
            string sourceId1 = "00000000-0000-0000-0000-000000000001";
            string sourceId2 = "00000000-0000-0000-0000-000000000002";
            string sourceId3 = "00000000-0000-0000-0000-000000000003";
            var node = new JObject();
            modelDefinitionHelperMock.Setup(m => m.GetPropertyValue(It.IsAny<JObject>(), "unitOfMeasureId")).Returns(sourceId1);
            modelDefinitionHelperMock.Setup(m => m.GetPropertyValue(It.IsAny<JObject>(), "unit")).Returns(sourceId2);
            modelDefinitionHelperMock.Setup(m => m.GetPropertyValue(It.IsAny<JObject>(), "unitOfMeasureIdForQuantity")).Returns(sourceId3);
            modelDefinitionHelperMock.Setup(m => m.IsGuid(It.IsAny<string>())).Returns(true);
            idConverterMock.Setup(m => m.ConvertIdToDestinationEnvironment<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>())).Returns(new Guid(destinationId));

            // Act
            unitConverter.InspectAndConvert(node);

            // Assert
            modelDefinitionHelperMock.Verify(m => m.IsGuid(sourceId1));
            modelDefinitionHelperMock.Verify(m => m.IsGuid(sourceId2));
            modelDefinitionHelperMock.Verify(m => m.IsGuid(sourceId3));
        }

        [Test]
        public void InspectAndConvert_DoesNotConvertWhenSourceIdIsNotGuid()
        {
            // Arrange

            var node = new JObject();
            modelDefinitionHelperMock.Setup(m => m.GetPropertyValue(It.IsAny<JObject>(), It.IsAny<string>())).Returns(sourceId);
            modelDefinitionHelperMock.Setup(m => m.IsGuid(It.IsAny<string>())).Returns(false);
            idConverterMock.Setup(m => m.ConvertIdToDestinationEnvironment<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>())).Returns(new Guid(destinationId));

            // Act
            unitConverter.InspectAndConvert(node);

            // Assert
            idConverterMock.Verify(m => m.ConvertIdToDestinationEnvironment<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>()), Times.Never);
            modelDefinitionHelperMock.Verify(m => m.SetPropertyValue(It.IsAny<JObject>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void InspectAndConvert_CallsConvertIdToDestinationEnvironmentWithCorrectParameters()
        {
            // Arrange
            // Arrange
            string sourceId1 = "00000000-0000-0000-0000-000000000001";
            string sourceId2 = "00000000-0000-0000-0000-000000000002";
            string sourceId3 = "00000000-0000-0000-0000-000000000003";
            var node = new JObject();
            modelDefinitionHelperMock.Setup(m => m.GetPropertyValue(It.IsAny<JObject>(), "unitOfMeasureId")).Returns(sourceId1);
            modelDefinitionHelperMock.Setup(m => m.GetPropertyValue(It.IsAny<JObject>(), "unit")).Returns(sourceId2);
            modelDefinitionHelperMock.Setup(m => m.GetPropertyValue(It.IsAny<JObject>(), "unitOfMeasureIdForQuantity")).Returns(sourceId3);
            modelDefinitionHelperMock.Setup(m => m.IsGuid(It.IsAny<string>())).Returns(true);
            idConverterMock.Setup(m => m.ConvertIdToDestinationEnvironment<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>())).Returns(new Guid(destinationId));

            // Act
            unitConverter.InspectAndConvert(node);

            // Assert
            idConverterMock.Verify(m => m.ConvertIdToDestinationEnvironment<string>("msdyn_unit", new Guid(sourceId1), "msdyn_name"));
            idConverterMock.Verify(m => m.ConvertIdToDestinationEnvironment<string>("msdyn_unit", new Guid(sourceId2), "msdyn_name"));
            idConverterMock.Verify(m => m.ConvertIdToDestinationEnvironment<string>("msdyn_unit", new Guid(sourceId3), "msdyn_name"));
        }

        [Test]
        public void InspectAndConvert_HandlesInexistingJsonProperties()
        {
            // Arrange

            var node = new JObject();
            modelDefinitionHelperMock.Setup(m => m.GetPropertyValue(It.IsAny<JObject>(), It.IsAny<string>())).Returns((string?)null);

            // Act
            unitConverter.InspectAndConvert(node);

            // Assert
            idConverterMock.Verify(m => m.ConvertIdToDestinationEnvironment<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>()), Times.Never);
            modelDefinitionHelperMock.Verify(m => m.SetPropertyValue(It.IsAny<JObject>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }


    }
}
