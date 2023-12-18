using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.PowerPlatform.Dataverse.Client.Model;
using Microsoft.Xrm.Sdk;
using System.Security;

namespace MSMConfigUtil.CLI
{
    public class OrganizationServiceFromCLIOptionsFactory : IOrganizationServiceFromCLIOptionsFactory
    {
        private readonly IConnectionOptionsFactory connectionOptionsFactory;

        public OrganizationServiceFromCLIOptionsFactory(IConnectionOptionsFactory connectionOptionsFactory)
        {
            this.connectionOptionsFactory = connectionOptionsFactory;
        }

        public IOrganizationService CreateDestinationOrgService(GlobalCLIOptions globalOptions)
        {
            return CreateOrgService(globalOptions.DestinationUri, globalOptions.AuthType, globalOptions.ClientId, globalOptions.ClientSecret);
        }

        public IOrganizationService CreateSourceOrgService(GlobalCLIOptions globalOptions)
        {
            return CreateOrgService(globalOptions.SourceUri, globalOptions.AuthType, globalOptions.ClientId, globalOptions.ClientSecret);
        }

        public IOrganizationService CreateOrgService(Uri environmemntUrl, AuthTypes authType, string? clientId, SecureString? clientSecret)
        {
            var connectionOptions = connectionOptionsFactory.Create(environmemntUrl, authType, clientId, clientSecret);
            return new ServiceClient(connectionOptions);
        }

    }
}
