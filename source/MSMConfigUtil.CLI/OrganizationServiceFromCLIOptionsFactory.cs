using Microsoft.Extensions.Logging;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.PowerPlatform.Dataverse.Client.Model;
using Microsoft.Xrm.Sdk;
using MSMConfigUtil.Logic;
using System.Security;

namespace MSMConfigUtil.CLI
{
    public class OrganizationServiceFromCLIOptionsFactory : IOrganizationServiceFromCLIOptionsFactory
    {
        private readonly IConnectionOptionsFactory connectionOptionsFactory;
        private readonly IUserInterfaceHandler uiHandler;

        public OrganizationServiceFromCLIOptionsFactory(IConnectionOptionsFactory connectionOptionsFactory, IUserInterfaceHandler uiHandler)
        {
            this.connectionOptionsFactory = connectionOptionsFactory;
            this.uiHandler = uiHandler;
        }

        public IOrganizationService CreateDestinationOrgService(GlobalCLIOptions globalOptions)
        {
            uiHandler.ShowInformation($"Connecting to destination Dataverse environment at {globalOptions.DestinationUri}");
            return CreateOrgService(globalOptions.DestinationUri, globalOptions.AuthType, globalOptions.ClientId, globalOptions.ClientSecret);
        }

        public IOrganizationService CreateSourceOrgService(GlobalCLIOptions globalOptions)
        {
            uiHandler.ShowInformation($"Connecting to source Dataverse environment at {globalOptions.SourceUri}");
            return CreateOrgService(globalOptions.SourceUri, globalOptions.AuthType, globalOptions.ClientId, globalOptions.ClientSecret);
        }

        public IOrganizationService CreateOrgService(Uri environmemntUrl, AuthTypes authType, string? clientId, SecureString? clientSecret)
        {
            if (authType == AuthTypes.Interactive)
            {
                uiHandler.ShowInformation("Interactive authentication to dataverse. You might be redirected to a browser window to authenticate.");
                uiHandler.PromptForConfirmation();
            }
            var connectionOptions = connectionOptionsFactory.Create(environmemntUrl, authType, clientId, clientSecret);
            return new ServiceClient(connectionOptions);
        }

    }
}
