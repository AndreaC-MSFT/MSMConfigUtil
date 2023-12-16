using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using MSM.ConfigUtil.Logic;
using Microsoft.Xrm.Sdk;

namespace MSM.ConfigUtil.Logic.Tests
{
    [TestFixture]
    public class CalculationModelRepositoryTests
    {
        private CalculationModelRepository calculationModelRepository;
        private Mock<IQueryProvider> queryProviderMock;

        [SetUp]
        public void Setup()
        {
            queryProviderMock = new Mock<IQueryProvider>();
            calculationModelRepository = new CalculationModelRepository(queryProviderMock.Object);
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
                        new ("msdyn_name",expectedResult[0].Name),
                        new ("msdyn_calculationflowjson",expectedResult[0].JsonDefinition)
                    ]},
                new() { Id = new Guid(expectedResult[1].Id),
                    Attributes = [
                        new ("msdyn_name",expectedResult[1].Name),
                        new ("msdyn_calculationflowjson",expectedResult[1].JsonDefinition)
                    ]}
            };
            queryProviderMock.Setup(q => q.CreateQuery("msdyn_emissioncalculation"))
                .Returns(fakeDataContent.AsQueryable());

            // Act
            var result = calculationModelRepository.GetAll();

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
    }
}
