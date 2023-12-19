using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MSMConfigUtil.Logic.Exceptions
{
    public class SourceToDestinationIdConversionException : Exception
    {
        public SourceToDestinationIdConversionException(string logicalTableName, string fieldToMatchInDestination, object valueToMatch)
            : base($"Cannot find a matching value for {logicalTableName}.{fieldToMatchInDestination} = '{valueToMatch}' in the destination environment.")
        { }

        public SourceToDestinationIdConversionException(string logicalTableName, List<KeyValuePair<string, object>> destinationFilter)
            : base($"Cannot find a matching value in destination environment for {logicalTableName} row with {FormatFilter(destinationFilter)}")
        { }

        private static string FormatFilter(List<KeyValuePair<string, object>> filter)
        {
            string formattedFilter = "(Filter Error)";
            try
            {
                formattedFilter = string.Join(" AND ", filter.Select(kv => $"{kv.Key} = {kv.Value}"));
            }
            catch { }
            return formattedFilter;
        }
    }
}
