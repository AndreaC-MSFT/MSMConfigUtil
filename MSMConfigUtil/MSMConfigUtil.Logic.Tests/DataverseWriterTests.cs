using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Moq;
using NUnit.Framework;
using System;

namespace MSM.ConfigUtil.Logic.Tests
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
    }
}
