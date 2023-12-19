using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.PowerPlatform.Dataverse.Client.Model;
using Microsoft.Xrm.Sdk;
using Moq;
using MSMConfigUtil.Logic;
using NUnit.Framework;
using System;
using System.Security;

namespace MSMConfigUtil.CLI.Tests
{
    public class OrganizationServiceFromCLIOptionsFactoryTests
    {
        private OrganizationServiceFromCLIOptionsFactory factory;
        private Mock<IConnectionOptionsFactory> connectionOptionsFactoryMock;
        private Mock<IUserInterfaceHandler> uiHandlerMock;

        [SetUp]
        public void Setup()
        {
            connectionOptionsFactoryMock = new Mock<IConnectionOptionsFactory>();
            uiHandlerMock = new Mock<IUserInterfaceHandler>();
            factory = new OrganizationServiceFromCLIOptionsFactory(connectionOptionsFactoryMock.Object, uiHandlerMock.Object);
        }

        [Test]
        public void CreateDestinationOrgService_ShouldReturnIOrganizationServiceWithCorrectUri()
        {
            // Arrange
            var opts = new GlobalCLIOptions
            {
                DestinationUri = new Uri("https://destination-uri/"),
                SourceUri = new Uri("https://source-uri/"),
                AuthType = AuthTypes.Interactive,
                ClientId = "clientId",
                ClientSecret = new SecureString()
            };

            // Act
            try
            {
                factory.CreateDestinationOrgService(opts);
            }
            catch { }

            // Assert
            connectionOptionsFactoryMock.Verify(m => m.Create(opts.DestinationUri, opts.AuthType, opts.ClientId, opts.ClientSecret));
        }

        [Test]
        public void CreateSourceOrgService_ShouldReturnIOrganizationServiceWithCorrectUri()
        {
            // Arrange
            var opts = new GlobalCLIOptions
            {
                DestinationUri = new Uri("https://destination-uri/"),
                SourceUri = new Uri("https://source-uri/"),
                AuthType = AuthTypes.Interactive,
                ClientId = "clientId",
                ClientSecret = new SecureString()
            };

            // Act
            try
            {
                factory.CreateSourceOrgService(opts);
            }
            catch { }

            // Assert
            connectionOptionsFactoryMock.Verify(m => m.Create(opts.SourceUri, opts.AuthType, opts.ClientId, opts.ClientSecret));
        }
    }
}
