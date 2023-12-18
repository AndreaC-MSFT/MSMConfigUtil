using Moq;
using MSMConfigUtil.Logic.CalculationModelMigration.NodesConversion;
using Newtonsoft.Json.Linq;
using System;

namespace MSMConfigUtil.Logic.UnitTests
{
    [TestFixture]
    public class ReportGasNodeConverterTests
    {
        private ReportGasNodeConverter reportGasConverter;
        private Mock<IIdConverter> idConverterMock;
        private Mock<IModelDefinitionHelper> modelDefinitionHelperMock;

        [SetUp]
        public void Setup()
        {
            // Initialize dependencies
            idConverterMock = new Mock<IIdConverter>();
            modelDefinitionHelperMock = new Mock<IModelDefinitionHelper>();
            reportGasConverter = new ReportGasNodeConverter(idConverterMock.Object, modelDefinitionHelperMock.Object);
        }

        /*
         string json = @"
            {
                ""id"": ""aa92abd2-7232-4715-8859-f739c98a5f34"",
                ""name"": ""Emission Quantity * GWP"",
                ""actionType"": ""ReportGas"",
                ""isValid"": true,
                ""description"": """",
                ""isPowerExperience"": false,
                ""valueExpression"": ""Activity.msdyn_quantity"",
                ""greenhouseGasId"": ""577dea1e-fc4a-4c9e-b346-f5f94df9b80c"",
                ""unitOfMeasureId"": ""Activity.msdyn_quantityunit""
            }";
         */

        [Test]
        public void SupportedActionTypes_ReturnsCorrectValue()
        {
            // Act
            var result = reportGasConverter.SupportedActionTypes;

            // Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First(), Is.EqualTo("ReportGas"));
        }

        [Test]
        public void SupportsAnyNode_ReturnsFalse()
        {
            // Act
            var result = reportGasConverter.SupportsAnyNode;
            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void InspectAndConvert_SetsCorrectPropertyValue()
        {
            // Arrange
            string expecteddestinationId = "00000000-0000-0000-0000-000000000001";
   
            var node = new JObject();
            modelDefinitionHelperMock.Setup(m => m.GetPropertyValue(It.IsAny<JObject>(), It.IsAny<string>())).Returns(Guid.Empty.ToString());
            modelDefinitionHelperMock.Setup(m => m.IsGuid(It.IsAny<string>())).Returns(true);
            idConverterMock.Setup(m => m.ConvertIdToDestinationEnvironment<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>())).Returns(new Guid(expecteddestinationId));

            // Act
            reportGasConverter.InspectAndConvert(node);

            // Assert
            modelDefinitionHelperMock.Verify(m => m.SetPropertyValue(node, "greenhouseGasId", expecteddestinationId));
        }

        [Test]
        public void InspectAndConvert_GetsCorrectPropertyFromSource()
        {
            // Arrange
            string expecteddestinationId = "00000000-0000-0000-0000-000000000001";
            var node = new JObject();
            modelDefinitionHelperMock.Setup(m => m.GetPropertyValue(It.IsAny<JObject>(), It.IsAny<string>())).Returns(Guid.Empty.ToString());
            modelDefinitionHelperMock.Setup(m => m.IsGuid(It.IsAny<string>())).Returns(true);
            idConverterMock.Setup(m => m.ConvertIdToDestinationEnvironment<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>())).Returns(new Guid(expecteddestinationId));

            // Act
            reportGasConverter.InspectAndConvert(node);

            // Assert
            modelDefinitionHelperMock.Verify(m => m.GetPropertyValue(node, "greenhouseGasId"));
        }

        [Test]
        public void InspectAndConvert_SendsCorrectIdForGuidCheck()
        {
            // Arrange
            string expecteddestinationId = "00000000-0000-0000-0000-000000000001";
            string sourceId = "00000000-0000-0000-0000-000000000002";
            var node = new JObject();
            modelDefinitionHelperMock.Setup(m => m.GetPropertyValue(It.IsAny<JObject>(), It.IsAny<string>())).Returns(sourceId);
            modelDefinitionHelperMock.Setup(m => m.IsGuid(It.IsAny<string>())).Returns(true);
            idConverterMock.Setup(m => m.ConvertIdToDestinationEnvironment<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>())).Returns(new Guid(expecteddestinationId));

            // Act
            reportGasConverter.InspectAndConvert(node);

            // Assert
            modelDefinitionHelperMock.Verify(m => m.IsGuid(sourceId));
        }

        [Test]
        public void InspectAndConvert_DoesNotConvertWhenSourceIdIsNotGuid()
        {
            // Arrange
            string expecteddestinationId = "00000000-0000-0000-0000-000000000001";
            string sourceId = "00000000-0000-0000-0000-000000000002";
            var node = new JObject();
            modelDefinitionHelperMock.Setup(m => m.GetPropertyValue(It.IsAny<JObject>(), It.IsAny<string>())).Returns(sourceId);
            modelDefinitionHelperMock.Setup(m => m.IsGuid(It.IsAny<string>())).Returns(false);
            idConverterMock.Setup(m => m.ConvertIdToDestinationEnvironment<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>())).Returns(new Guid(expecteddestinationId));

            // Act
            reportGasConverter.InspectAndConvert(node);

            // Assert
            idConverterMock.Verify(m => m.ConvertIdToDestinationEnvironment<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>()), Times.Never);
            modelDefinitionHelperMock.Verify(m => m.SetPropertyValue(It.IsAny<JObject>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void InspectAndConvert_CallsConvertIdToDestinationEnvironmentWithCorrectParameters()
        {
            // Arrange
            string expecteddestinationId = "00000000-0000-0000-0000-000000000001";
            string sourceId = "00000000-0000-0000-0000-000000000002";

            var node = new JObject();
            modelDefinitionHelperMock.Setup(m => m.GetPropertyValue(It.IsAny<JObject>(), It.IsAny<string>())).Returns(sourceId);
            modelDefinitionHelperMock.Setup(m => m.IsGuid(It.IsAny<string>())).Returns(true);
            idConverterMock.Setup(m => m.ConvertIdToDestinationEnvironment<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>())).Returns(new Guid(expecteddestinationId));

            // Act
            reportGasConverter.InspectAndConvert(node);

            // Assert
            idConverterMock.Verify(m => m.ConvertIdToDestinationEnvironment<string>("msdyn_greenhousegas", new Guid(sourceId), "msdyn_name"));
        }

    }
}
