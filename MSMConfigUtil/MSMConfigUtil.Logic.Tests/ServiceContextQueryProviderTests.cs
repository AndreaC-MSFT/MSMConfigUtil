using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Moq;
using NUnit.Framework;

namespace MSM.ConfigUtil.Logic.Tests
{
    [TestFixture]
    public class ServiceContextQueryProviderTests
    {
        private Mock<IOrganizationService> mockOrganizationService;
        private ServiceContextQueryProvider queryProvider;

        [SetUp]
        public void Setup()
        {
            mockOrganizationService = new Mock<IOrganizationService>();
            queryProvider = new ServiceContextQueryProvider(mockOrganizationService.Object);
        }

        [Test]
        public void CreateQuery_ReturnsQueryableEntity()
        {
            // Arrange
            string entityLogicalName = "account";

            // Act
            var result = queryProvider.CreateQuery(entityLogicalName);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<IQueryable<Entity>>());
        }
    }
}
