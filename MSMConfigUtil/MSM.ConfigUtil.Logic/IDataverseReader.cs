using Microsoft.Xrm.Sdk;
using System;

namespace MSM.ConfigUtil.Logic
{
    public interface IDataverseReader
    {
        public T GetRowValueById<T>(string logicalTableName, Guid rowId, string fieldName);
        public Guid GetRowIdByKey<TKeyField>(string logicalTableName, string keyFieldName, TKeyField keyFieldValue);
        public Guid GetRowIdByKey(string logicalTableName, IEnumerable<KeyValuePair<string, object>> keyFieldValueList);
    }
}
