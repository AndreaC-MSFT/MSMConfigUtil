using Microsoft.Xrm.Sdk;

namespace MSMConfigUtil.CLI
{
    public interface IOrganizationServiceFromCLIOptionsFactory
    {
        IOrganizationService CreateDestinationOrgService(GlobalCLIOptions globalOptions);
        IOrganizationService CreateSourceOrgService(GlobalCLIOptions globalOptions);
    }
}