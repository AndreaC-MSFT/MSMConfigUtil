using Microsoft.Identity.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace MSMConfigUtil.Logic
{
    public class DataverseReader : IDataverseReader
    {
        private readonly IOrganizationService organizationService;
        private readonly IRetrieveResponseReader retrieveResponseReader;
        private readonly OrganizationServiceContext organizationServiceContext;

        public DataverseReader(IOrganizationService organizationService, IRetrieveResponseReader retrieveResponseReader)
        {
            this.organizationService = organizationService;
            this.retrieveResponseReader = retrieveResponseReader;
            this.organizationServiceContext = new OrganizationServiceContext(organizationService);
        }

        public IQueryable<Entity> CreateQuery(string entityLogicalName)
        {
            return organizationServiceContext.CreateQuery(entityLogicalName);
        }

        public T GetRowValueById<T>(string logicalTableName, Guid rowId, string fieldName)
        {
            Entity entity = organizationService.Retrieve(logicalTableName, rowId, new ColumnSet(fieldName));
            if (entity == null)
                throw new InvalidOperationException($"Cannot find {logicalTableName} row with id '{rowId}'");
            
            var resultValue = entity.Attributes[fieldName];

            try
            {
                return (T)resultValue;
            }
            catch (Exception ex)
            {
                throw new InvalidCastException($"Failed to convert {resultValue} found in {logicalTableName}.{fieldName} to type {typeof(T)}", ex);
            }
        }

        public Guid? GetRowIdByKey<TKeyField>(string logicalTableName, string keyFieldName, TKeyField keyFieldValue)
        {
            var request = new RetrieveRequest()
            {
                Target = new EntityReference(logicalTableName, keyFieldName, keyFieldValue),
                ColumnSet = new ColumnSet()
            };
            RetrieveResponse response;
            try
            {
                response = (RetrieveResponse)organizationService.Execute(request);
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                if (ex.Detail?.ErrorCode  == DataverseConstants.ErrorCode_RecordNotFoundByEntityKey)
                    response = null;
                else
                    throw;
            }
            var responseEntity = retrieveResponseReader.GetEntity(response);
            return responseEntity?.Id;
        }

        public Guid? GetRowIdByKey(string logicalTableName, IEnumerable<KeyValuePair<string,object>> keyFieldValueList)
        {
            var keyAttributeCollection = new KeyAttributeCollection();
            keyAttributeCollection.AddRange(keyFieldValueList);
            var request = new RetrieveRequest()
            {
                Target = new EntityReference(logicalTableName, keyAttributeCollection),
                ColumnSet = new ColumnSet()
            };
            RetrieveResponse response;
            try
            {
                response = (RetrieveResponse)organizationService.Execute(request);
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                if (ex.Detail?.ErrorCode == DataverseConstants.ErrorCode_RecordNotFoundByEntityKey)
                    response = null;
                else
                    throw;
            }
            var responseEntity = retrieveResponseReader.GetEntity(response);
            return responseEntity?.Id;
        }

    }
}
