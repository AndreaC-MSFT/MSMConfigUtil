using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace MSM.ConfigUtil.Logic
{
    public class ServiceContextQueryProvider : IQueryProvider
    {
        private readonly OrganizationServiceContext organizationServiceContext;
        public ServiceContextQueryProvider(IOrganizationService organizationService)
        {
            organizationServiceContext = new OrganizationServiceContext(organizationService);
        }
        public IQueryable<Entity> CreateQuery(string entityLogicalName)
        {
            return organizationServiceContext.CreateQuery(entityLogicalName);
        }
    }
}
