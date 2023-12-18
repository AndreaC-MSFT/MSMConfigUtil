using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Moq;
using NUnit.Framework;
using System;

namespace MSMConfigUtil.Logic.UnitTests
{
    [TestFixture]
    public class DataverseWriterTests
    {
        private Mock<IOrganizationService> organizationServiceMock;
        private DataverseWriter dataverseWriter;

        [SetUp]
        public void Setup()
        {
            organizationServiceMock = new Mock<IOrganizationService>();
            dataverseWriter = new DataverseWriter(organizationServiceMock.Object);
        }

        [Test]
        public void Upsert_Should_Execute_UpsertRequest_With_Target_Entity()
        {
            // Arrange
            Entity entity = new Entity();

            // Act
            dataverseWriter.Upsert(entity);

            // Assert
            organizationServiceMock.Verify(x => x.Execute(It.Is<UpsertRequest>(r => r.Target == entity)), Times.Once);
        }

        [Test]
        public void Create_Should_PassEntityToOrgService()
        {
            // Arrange
            Entity entity = new Entity();

            // Act
            dataverseWriter.Create(entity);

            // Assert
            organizationServiceMock.Verify(x => x.Create(entity), Times.Once);
        }

        [Test]
        public void Update_Should_PassEntityToOrgService()
        {
            // Arrange
            Entity entity = new Entity();

            // Act
            dataverseWriter.Update(entity);

            // Assert
            organizationServiceMock.Verify(x => x.Update(entity), Times.Once);
        }
    }
}
