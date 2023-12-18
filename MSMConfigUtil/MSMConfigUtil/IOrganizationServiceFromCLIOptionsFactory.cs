using Microsoft.Xrm.Sdk;

namespace MSMConfigUtil
{
    public interface IOrganizationServiceFromCLIOptionsFactory
    {
        IOrganizationService CreateDestinationOrgService(GlobalCLIOptions globalOptions);
        IOrganizationService CreateSourceOrgService(GlobalCLIOptions globalOptions);
    }
}