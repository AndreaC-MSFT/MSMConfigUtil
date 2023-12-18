using Microsoft.PowerPlatform.Dataverse.Client.Model;
using Moq;
using NUnit.Framework;
using System;
using System.Security;

namespace MSMConfigUtil.CLI.Tests
{
    public class ConnectionOptionsFactoryTests
    {
        private ConnectionOptionsFactory _connectionOptionsFactory;

        [SetUp]
        public void Setup()
        {
            _connectionOptionsFactory = new ConnectionOptionsFactory();
        }

        [Test]
        public void Create_Should_Return_ConnectionOptionsWithInteractiveAuth()
        {
            // Arrange
            Uri environmemntUrl = new Uri("http://example.com");
            AuthTypes authType = AuthTypes.Interactive;
            string clientId = "testClientId";
            SecureString clientSecret = null;

            // Act
            var result = _connectionOptionsFactory.Create(environmemntUrl, authType, clientId, clientSecret);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ServiceUri, Is.EqualTo(environmemntUrl));
            Assert.That(result.RequireNewInstance, Is.EqualTo(true));
            Assert.That(result.RedirectUri, Is.EqualTo(new Uri("http://localhost")));
            Assert.That(result.LoginPrompt, Is.EqualTo(Microsoft.PowerPlatform.Dataverse.Client.Auth.PromptBehavior.Auto));
            Assert.That(result.AuthenticationType, Is.EqualTo(Microsoft.PowerPlatform.Dataverse.Client.AuthenticationType.OAuth));
            Assert.That(result.ClientId, Is.EqualTo(clientId));
        }

        [Test]
        public void Create_Should_Return_ConnectionOptionsWithClientSecretAuth()
        {
            // Arrange
            Uri environmemntUrl = new Uri("http://example.com");
            AuthTypes authType = AuthTypes.ClientIdAndSecret;
            string clientId = "testClientId";
            SecureString clientSecret = new SecureString();
            clientSecret.AppendChar('a');
            clientSecret.MakeReadOnly();

            // Act
            var result = _connectionOptionsFactory.Create(environmemntUrl, authType, clientId, clientSecret);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ServiceUri, Is.EqualTo(environmemntUrl));
            Assert.That(result.RequireNewInstance, Is.EqualTo(true));
            Assert.That(result.RedirectUri, Is.EqualTo(new Uri("http://localhost")));
            Assert.That(result.AuthenticationType, Is.EqualTo(Microsoft.PowerPlatform.Dataverse.Client.AuthenticationType.ClientSecret));
            Assert.That(result.ClientId, Is.EqualTo(clientId));
        }

        [Test]
        public void Create_Should_Throw_Exception_When_AuthType_Is_ClientIdAndSecret_And_Required_Parameters_Are_Missing()
        {
            // Arrange
            Uri environmemntUrl = new Uri("http://example.com");
            AuthTypes authType = AuthTypes.ClientIdAndSecret;
            string clientId = null;
            SecureString clientSecret = null;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _connectionOptionsFactory.Create(environmemntUrl, authType, clientId, clientSecret));
        }
    }
}
