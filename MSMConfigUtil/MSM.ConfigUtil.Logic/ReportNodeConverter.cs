using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSM.ConfigUtil.Logic
{
    public class ReportNodeConverter : IModelDefinitionNodeConverter
    {
        private static readonly string[] supportedActionTypes = ["Report"];
        private readonly IIdConverter idConverter;
        private readonly IModelDefinitionHelper modelDefinitionHelper;


        private const string emissionFactorIdJsonPropertyName = "emissionFactorId";
        private const string calculationLibraryIdJsonPropertyName = "calculationLibraryId";
        private const string calculationLibraryTableName = "msdyn_calculationlibrary";
        private const string emissionFactorTableName = "msdyn_emissionfactor";
        private const string calculationLibraryIdFieldName = "msdyn_calculationlibraryid";


        private const string nameFieldName = "msdyn_name";

        public ReportNodeConverter(IIdConverter idConverter, IModelDefinitionHelper modelDefinitionHelper)
        {
            this.idConverter = idConverter;
            this.modelDefinitionHelper = modelDefinitionHelper;
        }

        public IEnumerable<string> SupportedActionTypes => supportedActionTypes;
        public bool SupportsAnyNode { get { return false; } }


        public void InspectAndConvert(JObject node)
        {
            var sourceCalculationLibraryId = modelDefinitionHelper.GetPropertyValue(node, calculationLibraryIdJsonPropertyName);
            if (sourceCalculationLibraryId != null && modelDefinitionHelper.IsGuid(sourceCalculationLibraryId))
            {
                var destinationCalculationLibraryId = idConverter.ConvertIdToDestinationEnvironment<string>(calculationLibraryTableName, new Guid(sourceCalculationLibraryId), nameFieldName);
                modelDefinitionHelper.SetPropertyValue(node, calculationLibraryIdJsonPropertyName, destinationCalculationLibraryId.ToString());

                var sourceEmissionFactorId = modelDefinitionHelper.GetPropertyValue(node, emissionFactorIdJsonPropertyName);
                if (sourceEmissionFactorId != null && modelDefinitionHelper.IsGuid(sourceEmissionFactorId))
                {
                    var destinationEmissionFactorId = idConverter.ConvertIdToDestinationEnvironment<string>(emissionFactorTableName,
                        new Guid(sourceEmissionFactorId), nameFieldName,
                        new KeyValuePair<string, object>[] { new(calculationLibraryIdFieldName, destinationCalculationLibraryId) });
                    modelDefinitionHelper.SetPropertyValue(node, emissionFactorIdJsonPropertyName, destinationEmissionFactorId.ToString());
                }
            }


        }
    }
}
