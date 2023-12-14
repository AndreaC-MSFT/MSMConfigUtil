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
            return destinationDataverseReader.GetRowIdByAlternativeKey(logicalTableName, fieldToMatchInDestination, valueToMatch);
        }
    }
}
