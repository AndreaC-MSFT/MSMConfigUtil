using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;
using MSMConfigUtil.Logic.CalculationModelMigration;

namespace MSMConfigUtil.Logic.UnitTests
{
    [TestFixture]
    public class CalculationModelReaderTests
    {
        private CalculationModelReader calculationModelReader;
        private Mock<IDataverseReader> dataverseReaderMock;
        private static int calculationModelType_Custom = 700610000;

        [SetUp]
        public void Setup()
        {
            dataverseReaderMock = new Mock<IDataverseReader>();
            calculationModelReader = new CalculationModelReader(dataverseReaderMock.Object);
        }

        [Test]
        public void GetAll_ShouldReturnAllCalculationModels()
        {
            // Arrange
            var expectedResult = new List<CalculationModel>
            {
                new CalculationModel { Id = "00000000-0000-0000-0000-000000000001", Name = "Model 1", JsonDefinition = "Definition 1" },
                new CalculationModel { Id = "00000000-0000-0000-0000-000000000002", Name = "Model 2", JsonDefinition = "Definition 2" }
            };

            var fakeDataContent = new Entity[]
            {
                new() { Id = new Guid(expectedResult[0].Id),
                    Attributes = [
                        new ("msdyn_type",calculationModelType_Custom),
                        new ("msdyn_name",expectedResult[0].Name),
                        new ("msdyn_calculationflowjson",expectedResult[0].JsonDefinition)
                    ]},
                new() { Id = new Guid(expectedResult[1].Id),
                    Attributes = [
                        new ("msdyn_type",calculationModelType_Custom),
                        new ("msdyn_name",expectedResult[1].Name),
                        new ("msdyn_calculationflowjson",expectedResult[1].JsonDefinition)
                    ]}
            };
            dataverseReaderMock.Setup(q => q.CreateQuery("msdyn_emissioncalculation"))
                .Returns(fakeDataContent.AsQueryable());

            // Act
            var result = calculationModelReader.GetAll();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(expectedResult.Count()));
            var arResult = result.ToArray();
            for (int i = 0; i < arResult.Length; i++)
            {
                Assert.That(arResult[i].Id, Is.EqualTo(expectedResult[i].Id));
                Assert.That(arResult[i].Name, Is.EqualTo(expectedResult[i].Name));
                Assert.That(arResult[i].JsonDefinition, Is.EqualTo(expectedResult[i].JsonDefinition));

            }
        }

        [Test]
        public void GetAll_Should_NotReturnNonCustomCalculationModels()
        {
            // Arrange
            var expectedResult = new List<CalculationModel>
            {
                new CalculationModel { Id = "00000000-0000-0000-0000-000000000001", Name = "Model 1", JsonDefinition = "Definition 1" },
                new CalculationModel { Id = "00000000-0000-0000-0000-000000000002", Name = "Model 2", JsonDefinition = "Definition 2" }
            };

            var fakeDataContent = new Entity[]
            {
                new() { Id = new Guid(expectedResult[0].Id),
                    Attributes = [
                        new ("msdyn_type",calculationModelType_Custom),
                        new ("msdyn_name",expectedResult[0].Name),
                        new ("msdyn_calculationflowjson",expectedResult[0].JsonDefinition)
                    ]},
                new() { Id = new Guid(expectedResult[1].Id),
                    Attributes = [
                        new ("msdyn_type",calculationModelType_Custom),
                        new ("msdyn_name",expectedResult[1].Name),
                        new ("msdyn_calculationflowjson",expectedResult[1].JsonDefinition)
                    ]},
                new() { Id = Guid.NewGuid(),
                    Attributes = [
                        new ("msdyn_type",11111),
                        new ("msdyn_name","Something else"),
                        new ("msdyn_calculationflowjson","def 3")
                    ]}
            };
            dataverseReaderMock.Setup(q => q.CreateQuery("msdyn_emissioncalculation"))
                .Returns(fakeDataContent.AsQueryable());

            // Act
            var result = calculationModelReader.GetAll();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(expectedResult.Count()));
            var arResult = result.ToArray();
            for (int i = 0; i < arResult.Length; i++)
                Assert.That(arResult[i].Id, Is.EqualTo(expectedResult[i].Id));
        }

        [Test]
        public void GetAll_Should_ReturnAdditionalAttributes()
        {
            var fakeDataContent = new Entity[]
            {
                new() { Id = Guid.NewGuid(),
                    Attributes = [
                        new ("msdyn_type",calculationModelType_Custom),
                        new ("msdyn_name","any"),
                        new ("msdyn_calculationflowjson","any"),
                        new ("additionalAttributeA","valueA1"),
                        new ("additionalAttributeB","valueB1")
                    ]},
                new() { Id = Guid.NewGuid(),
                    Attributes = [
                        new ("msdyn_type",calculationModelType_Custom),
                        new ("msdyn_name","any"),
                        new ("msdyn_calculationflowjson","any"),
                        new ("additionalAttributeA","valueA2")
                    ]}
            };
            dataverseReaderMock.Setup(q => q.CreateQuery("msdyn_emissioncalculation"))
                .Returns(fakeDataContent.AsQueryable());

            // Act
            var result = calculationModelReader.GetAll();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            var arResult = result.ToArray();
            Assert.That(arResult[0].AdditionalAttributes, Is.Not.Null);
            Assert.That(arResult[0].AdditionalAttributes.Count(a => a.Key == "additionalAttributeA" && a.Value.ToString() == "valueA1"), Is.EqualTo(1));
            Assert.That(arResult[0].AdditionalAttributes.Count(a => a.Key == "additionalAttributeB" && a.Value.ToString() == "valueB1"), Is.EqualTo(1));
            Assert.That(arResult[1].AdditionalAttributes, Is.Not.Null);
            Assert.That(arResult[1].AdditionalAttributes.Count(a => a.Key == "additionalAttributeA" && a.Value.ToString() == "valueA2"), Is.EqualTo(1));
        }

        [Test]
        public void GetAll_Should_NotReturnStandardAttributeInAdditionalAttributes()
        {
            var fakeDataContent = new Entity[]
            {
                new() { Id = Guid.NewGuid(),
                    Attributes = [
                        new ("msdyn_type",calculationModelType_Custom),
                        new ("msdyn_emissioncalculationid","id1"),
                        new ("msdyn_name","any"),
                        new ("msdyn_calculationflowjson","any"),
                        new ("additionalAttributeA","valueA1"),
                        new ("additionalAttributeB","valueB1")
                    ]},
                new() { Id = Guid.NewGuid(),
                    Attributes = [
                        new ("msdyn_type",calculationModelType_Custom),
                        new ("msdyn_emissioncalculationid","id2"),
                        new ("msdyn_name","any"),
                        new ("msdyn_calculationflowjson","any"),
                        new ("additionalAttributeA","valueA2"),
                        new ("additionalAttributeB","valueB2")
                    ]}
            };
            dataverseReaderMock.Setup(q => q.CreateQuery("msdyn_emissioncalculation"))
                .Returns(fakeDataContent.AsQueryable());

            // Act
            var result = calculationModelReader.GetAll();

            // Assert
            foreach (var resultItem in result)
            {
                Assert.That(resultItem.AdditionalAttributes, Is.Not.Null);
                Assert.That(resultItem.AdditionalAttributes.Any(a => a.Key == "msdyn_emissioncalculationid"), Is.False);
                Assert.That(resultItem.AdditionalAttributes.Any(a => a.Key == "msdyn_name"), Is.False);
                Assert.That(resultItem.AdditionalAttributes.Any(a => a.Key == "msdyn_calculationflowjson"), Is.False);
                Assert.That(resultItem.AdditionalAttributes.Count(), Is.EqualTo(3));
            }
        }




        [Test]
        public void Exists_Should_ReturnsTrue_WhenNameExists()
        {
            // Arrange
            string name = "exampleName";
            dataverseReaderMock.Setup(m => m.GetRowIdByKey<string>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Guid.NewGuid());

            // Act
            bool result = calculationModelReader.Exists(name);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void Exists_Should_ReturnsFalse_WhenNameDoesNotExist()
        {
            // Arrange
            string name = "exampleName";
            dataverseReaderMock.Setup(m => m.GetRowIdByKey<string>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns((Guid?)null);

            // Act
            bool result = calculationModelReader.Exists(name);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Exists_Should_PassCorrectParametersToDataverseReader()
        {
            // Arrange
            string name = "exampleName";
            dataverseReaderMock.Setup(m => m.GetRowIdByKey<string>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Guid.NewGuid());

            // Act
            bool result = calculationModelReader.Exists(name);

            // Assert
            dataverseReaderMock.Verify(m => m.GetRowIdByKey<string>("msdyn_emissioncalculation", "msdyn_name", name));
        }

        [Test]
        public void Get_Should_NotReturnNonCustomCalculationModels()
        {
            // Arrange
            var expectedResult = new List<CalculationModel>
            {
                new CalculationModel { Id = "00000000-0000-0000-0000-000000000001", Name = "Model 1", JsonDefinition = "Definition 1" },
                new CalculationModel { Id = "00000000-0000-0000-0000-000000000002", Name = "Model 2", JsonDefinition = "Definition 2" }
            };

            var fakeDataContent = new Entity[]
            {
                new() { Id = new Guid(expectedResult[0].Id),
                    Attributes = [
                        new ("msdyn_type",calculationModelType_Custom),
                        new ("msdyn_name",expectedResult[0].Name),
                        new ("msdyn_calculationflowjson",expectedResult[0].JsonDefinition)
                    ]},
                new() { Id = new Guid(expectedResult[1].Id),
                    Attributes = [
                        new ("msdyn_type",calculationModelType_Custom),
                        new ("msdyn_name",expectedResult[1].Name),
                        new ("msdyn_calculationflowjson",expectedResult[1].JsonDefinition)
                    ]},
                new() { Id = Guid.NewGuid(),
                    Attributes = [
                        new ("msdyn_type",11111),
                        new ("msdyn_name","Non custom model"),
                        new ("msdyn_calculationflowjson","def 3")
                    ]}
            };
            dataverseReaderMock.Setup(q => q.CreateQuery("msdyn_emissioncalculation"))
                .Returns(fakeDataContent.AsQueryable());

            // Act
            var result = calculationModelReader.Get("Non custom model");

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Get_Should_ReturnModelSelectedByName()
        {
            // Arrange
            var expectedResult = new List<CalculationModel>
            {
                new CalculationModel { Id = "00000000-0000-0000-0000-000000000001", Name = "Model 1", JsonDefinition = "Definition 1" },
                new CalculationModel { Id = "00000000-0000-0000-0000-000000000002", Name = "Model 2", JsonDefinition = "Definition 2" }
            };

            var fakeDataContent = new Entity[]
            {
                new() { Id = new Guid(expectedResult[0].Id),
                    Attributes = [
                        new ("msdyn_type",calculationModelType_Custom),
                        new ("msdyn_name",expectedResult[0].Name),
                        new ("msdyn_calculationflowjson",expectedResult[0].JsonDefinition)
                    ]},
                new() { Id = new Guid(expectedResult[1].Id),
                    Attributes = [
                        new ("msdyn_type",calculationModelType_Custom),
                        new ("msdyn_name",expectedResult[1].Name),
                        new ("msdyn_calculationflowjson",expectedResult[1].JsonDefinition)
                    ]},
                new() { Id = Guid.NewGuid(),
                    Attributes = [
                        new ("msdyn_type",11111),
                        new ("msdyn_name","Non custom model"),
                        new ("msdyn_calculationflowjson","def 3")
                    ]}
            };
            dataverseReaderMock.Setup(q => q.CreateQuery("msdyn_emissioncalculation"))
                .Returns(fakeDataContent.AsQueryable());

            // Act
            var result = calculationModelReader.Get(expectedResult[1].Name);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(expectedResult[1].Id));
            Assert.That(result.Name, Is.EqualTo(expectedResult[1].Name));
            Assert.That(result.JsonDefinition, Is.EqualTo(expectedResult[1].JsonDefinition));
        }
    }
}
