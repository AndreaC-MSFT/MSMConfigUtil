using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Moq;
using MSMConfigUtil.Logic.CalculationModelMigration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMConfigUtil.Logic.UnitTests
{
    [TestFixture]
    public class CalculationModelWriterTests
    {
        private CalculationModelWriter calculationModelWriter;
        private Mock<IDataverseWriter> dataverseWriterMock;
        private KeyValuePair<string, object>[] excludedAttributes;

        [SetUp]
        public void Setup()
        {
            dataverseWriterMock = new Mock<IDataverseWriter>();
            calculationModelWriter = new CalculationModelWriter(dataverseWriterMock.Object);
            excludedAttributes =
                    [
                        new("createdby", "Value"),
                        new("createdonbehalfby", "Value"),
                        new("createdon", "Value"),
                        new("importsequencenumber", "Value"),
                        new("modifiedby", "Value"),
                        new("modifiedonbehalfby", "Value"),
                        new("modifiedon", "Value"),
                        new("ownerid", "Value"),
                        new("owningbusinessunit", "Value"),
                        new("owningteam", "Value"),
                        new("owninguser", "Value"),
                        new("overriddencreatedon", "Value"),
                        new("statecode", "Value"),
                        new("statuscode", "Value"),
                        new("timezoneruleversionnumber", "Value"),
                        new("utcconversiontimezonecode", "Value")
                    ];
        }

        [Test]
        public void Create_Should_PassEntityWithStandardAttributesToDataverseWriter()
        {
            // Arrange
            var sourceModel = new CalculationModel()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "My model name",
                JsonDefinition = "Json definition",
                AdditionalAttributes = new KeyValuePair<string, object>[0]
            };

            var expectedAttributes = new KeyValuePair<string, object>[]
            {
                new("msdyn_name", sourceModel.Name),
                new("msdyn_calculationflowjson", sourceModel.JsonDefinition)
            };

            // Act
            calculationModelWriter.Create(sourceModel);

            // Assert
            dataverseWriterMock.Verify(m => m.Create(It.Is<Entity>(e => TestUtils.KeyValuePairListsAreEquivalent(expectedAttributes, e.Attributes))));
        }

        [Test]
        public void Create_Should_PassEntityWithAdditionalAttributesToDataverseWriter()
        {
            // Arrange
            var sourceModel = new CalculationModel()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "My model name",
                JsonDefinition = "Json definition",
                AdditionalAttributes = new KeyValuePair<string, object>[]
                    {
                        new("AdditionalAttribute1", "Value1"),
                        new("AdditionalAttribute2", "Value2")
                    }
            };

            var expectedStandardAttributes = new KeyValuePair<string, object>[]
            {
                new("msdyn_name", sourceModel.Name),
                new("msdyn_calculationflowjson", sourceModel.JsonDefinition)
            };
            var expectedAttributes = expectedStandardAttributes.Concat(sourceModel.AdditionalAttributes);

            // Act
            calculationModelWriter.Create(sourceModel);

            // Assert
            dataverseWriterMock.Verify(m => m.Create(It.Is<Entity>(e => TestUtils.KeyValuePairListsAreEquivalent(expectedAttributes, e.Attributes))));
        }

        [Test]
        public void Create_Should_NotPassSourceModelIdToDataverseWriter()
        {
            // Arrange
            var sourceModel = new CalculationModel()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "My model name",
                JsonDefinition = "Json definition",
                AdditionalAttributes = new KeyValuePair<string, object>[0]
            };

            // Act
            calculationModelWriter.Create(sourceModel);

            // Assert
            dataverseWriterMock.Verify(m => m.Create(It.Is<Entity>(e => !e.Attributes.Any(a => a.Value.ToString() == sourceModel.Id))));
        }

        [Test]
        public void Create_Should_NotPassExcludedAttributesToDataverseWriter()
        {
            // Arrange
            var sourceModel = new CalculationModel()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "My model name",
                JsonDefinition = "Json definition",
                AdditionalAttributes = excludedAttributes
            };

            // Act
            calculationModelWriter.Create(sourceModel);

            // Assert
            dataverseWriterMock.Verify(m => m.Create(It.Is<Entity>(e => 
                !e.Attributes.Any(a =>
                    sourceModel.AdditionalAttributes.Any(aa =>
                        aa.Key == a.Key)))));

        }




        [Test]
        public void Update_Should_PassEntityWithStandardAttributesToDataverseWriter()
        {
            // Arrange
            var sourceModel = new CalculationModel()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "My model name",
                JsonDefinition = "Json definition",
                AdditionalAttributes = new KeyValuePair<string, object>[0]
            };

            var expectedAttributes = new KeyValuePair<string, object>[]
            {
                new("msdyn_name", sourceModel.Name),
                new("msdyn_calculationflowjson", sourceModel.JsonDefinition)
            };

            // Act
            calculationModelWriter.Update(sourceModel);

            // Assert
            dataverseWriterMock.Verify(m => m.Update(It.Is<Entity>(e => TestUtils.KeyValuePairListsAreEquivalent(expectedAttributes, e.Attributes))));
        }

        [Test]
        public void Update_Should_PassEntityWithAdditionalAttributesToDataverseWriter()
        {
            // Arrange
            var sourceModel = new CalculationModel()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "My model name",
                JsonDefinition = "Json definition",
                AdditionalAttributes = new KeyValuePair<string, object>[]
                    {
                        new("AdditionalAttribute1", "Value1"),
                        new("AdditionalAttribute2", "Value2")
                    }
            };

            var expectedStandardAttributes = new KeyValuePair<string, object>[]
            {
                new("msdyn_name", sourceModel.Name),
                new("msdyn_calculationflowjson", sourceModel.JsonDefinition)
            };
            var expectedAttributes = expectedStandardAttributes.Concat(sourceModel.AdditionalAttributes);

            // Act
            calculationModelWriter.Update(sourceModel);

            // Assert
            dataverseWriterMock.Verify(m => m.Update(It.Is<Entity>(e => TestUtils.KeyValuePairListsAreEquivalent(expectedAttributes, e.Attributes))));
        }

        [Test]
        public void Update_Should_PassSourceModelIdToDataverseWriter()
        {
            // Arrange
            var sourceModel = new CalculationModel()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "My model name",
                JsonDefinition = "Json definition",
                AdditionalAttributes = new KeyValuePair<string, object>[0]
            };

            // Act
            calculationModelWriter.Update(sourceModel);

            // Assert
            dataverseWriterMock.Verify(m => m.Update(It.Is<Entity>(e => e.Id.ToString() == sourceModel.Id)));
        }

        [Test]
        public void Update_Should_NotPassExcludedAttributesToDataverseWriter()
        {
            // Arrange
            var sourceModel = new CalculationModel()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "My model name",
                JsonDefinition = "Json definition",
                AdditionalAttributes = excludedAttributes
            };

            // Act
            calculationModelWriter.Update(sourceModel);

            // Assert
            dataverseWriterMock.Verify(m => m.Update(It.Is<Entity>(e =>
                !e.Attributes.Any(a =>
                    sourceModel.AdditionalAttributes.Any(aa =>
                        aa.Key == a.Key)))));

        }
    }
}
