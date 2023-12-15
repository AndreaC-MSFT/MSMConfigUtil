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
            return destinationDataverseReader.GetRowIdByKey(logicalTableName, fieldToMatchInDestination, valueToMatch);
        }

        public Guid ConvertIdToDestinationEnvironment<TMatchingFieldType>(string logicalTableName, Guid sourceRowId, string fieldToMatchInDestination, IEnumerable<KeyValuePair<string,object>> additionalDestinationFilterCriteria)
        {
            var valueToMatch = sourceDataverseReader.GetRowValueById<TMatchingFieldType>(logicalTableName, sourceRowId, fieldToMatchInDestination);
            var destinationFilter = new List<KeyValuePair<string, object>>
            {
                new(fieldToMatchInDestination, valueToMatch)
            };
            destinationFilter.AddRange(additionalDestinationFilterCriteria);
            return destinationDataverseReader.GetRowIdByKey(logicalTableName, destinationFilter);
        }
    }
}
