using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MSM.ConfigUtil.Logic
{
    public class IdConverter
    {
        private readonly IDataverseReader sourceDataverseReader;
        private readonly IDataverseReader destinationDataverseReader;
        public IdConverter(IDataverseReader sourceDataverseReader, IDataverseReader destinationDataverseReader)
        {
            this.sourceDataverseReader = sourceDataverseReader;
            this.destinationDataverseReader = destinationDataverseReader;
        }

        public Guid ConvertIdToDestinationEnvironment<TMatchingFieldType>(string logicalTableName, Guid sourceRowId, string fieldToMatchInDestination)
        {
            var valueToMatch = sourceDataverseReader.GetRowValueById<TMatchingFieldType>(logicalTableName, sourceRowId, fieldToMatchInDestination);
            var destinationId = destinationDataverseReader.GetRowIdByKey(logicalTableName, fieldToMatchInDestination, valueToMatch);
            if (!destinationId.HasValue)
                throw new InvalidOperationException($"Cannot find a matching value for {logicalTableName}.{fieldToMatchInDestination} = '{valueToMatch}' in the destination environment");
            return destinationId.Value;
        }

        public Guid ConvertIdToDestinationEnvironment<TMatchingFieldType>(string logicalTableName, Guid sourceRowId, string fieldToMatchInDestination, IEnumerable<KeyValuePair<string,object>> additionalDestinationFilterCriteria)
        {
            var valueToMatch = sourceDataverseReader.GetRowValueById<TMatchingFieldType>(logicalTableName, sourceRowId, fieldToMatchInDestination);
            var destinationFilter = new List<KeyValuePair<string, object>>
            {
                new(fieldToMatchInDestination, valueToMatch)
            };
            destinationFilter.AddRange(additionalDestinationFilterCriteria);
            var destinationId = destinationDataverseReader.GetRowIdByKey(logicalTableName, destinationFilter);
            if (!destinationId.HasValue)
            {
                string formattedFilter = "(Filter Error)";
                try
                {
                    formattedFilter = string.Join(" AND ", destinationFilter.Select(kv => $"{kv.Key} = {kv.Value}"));
                }
                catch { }
                throw new InvalidOperationException($"Cannot find a matching value in destination environment for {logicalTableName} row with {formattedFilter}");
            }
            return destinationId.Value;
        }
    }
}
