using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using System;

namespace MSMConfigUtil.Logic
{
    public class DataverseWriter : IDataverseWriter
    {
        private readonly IOrganizationService organizationService;
        public DataverseWriter(IOrganizationService organizationService) 
        {
            this.organizationService = organizationService;
        }

        public void Upsert(Entity entity)
        {
            UpsertRequest request = new()
            {
                Target = entity
            };
            organizationService.Execute(request);
        }

        public void Create(Entity entity)
        {
            organizationService.Create(entity);
        }

        public void Update(Entity entity)
        {
            organizationService.Update(entity);
        }
    }
}
