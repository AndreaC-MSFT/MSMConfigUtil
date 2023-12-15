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

namespace MSM.ConfigUtil.Logic
{
    public class DataverseReader
    {
        private readonly IOrganizationService organizationService;

        public DataverseReader(IOrganizationService organizationService)
        {
            this.organizationService = organizationService;
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

        public Guid GetRowIdByKey<TKeyField>(string logicalTableName, string keyFieldName, TKeyField keyFieldValue)
        {
            var request = new RetrieveRequest()
            {
                Target = new EntityReference(logicalTableName, keyFieldName, keyFieldValue)
            };
            var response = (RetrieveResponse)organizationService.Execute(request);
            if (response == null || response.Entity == null)
                throw new InvalidOperationException($"Cannot find {logicalTableName} row with {keyFieldName} = '{keyFieldValue}'");
            return response.Entity.Id;
        }

        public Guid GetRowIdByKey(string logicalTableName, IEnumerable<KeyValuePair<string,object>> keyFieldValueList)
        {
            var keyAttributeCollection = new KeyAttributeCollection();
            keyAttributeCollection.AddRange(keyFieldValueList);
            var request = new RetrieveRequest()
            {
                Target = new EntityReference(logicalTableName, keyAttributeCollection)
            };
            var response = (RetrieveResponse)organizationService.Execute(request);
            if (response == null || response.Entity == null)
            {
                string formattedFilter = "(Filter Error)";
                try
                {
                    formattedFilter = string.Join(" AND ", keyFieldValueList.Select(kv => $"{kv.Key} = {kv.Value}"));
                }
                catch { }
                throw new InvalidOperationException($"Cannot find {logicalTableName} row with {formattedFilter}");
            }
            return response.Entity.Id;
        }

    }
}
