using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMConfigUtil.Logic.CalculationModelMigration.NodesConversion
{
    public class ReportNodeConverter : IModelDefinitionNodeConverter
    {
        private static readonly string[] supportedActionTypes = ["Report"];
        private readonly IIdConverter idConverter;
        private readonly IModelDefinitionHelper modelDefinitionHelper;

        private const string emissionFactorIdJsonPropertyName = "emissionFactorId";
        private const string calculationLibraryIdJsonPropertyName = "calculationLibraryId";

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
                var destinationCalculationLibraryId = idConverter.ConvertIdToDestinationEnvironment<string>(CalculationModelsConstants.calculationLibraryTableName, new Guid(sourceCalculationLibraryId), CalculationModelsConstants.msdyn_name);
                modelDefinitionHelper.SetPropertyValue(node, calculationLibraryIdJsonPropertyName, destinationCalculationLibraryId.ToString());

                var sourceEmissionFactorId = modelDefinitionHelper.GetPropertyValue(node, emissionFactorIdJsonPropertyName);
                if (sourceEmissionFactorId != null && modelDefinitionHelper.IsGuid(sourceEmissionFactorId))
                {
                    var destinationEmissionFactorId = idConverter.ConvertIdToDestinationEnvironment<string>(CalculationModelsConstants.emissionFactorTableName,
                        new Guid(sourceEmissionFactorId), CalculationModelsConstants.msdyn_name,
                        new KeyValuePair<string, object>[] { new(CalculationModelsConstants.calculationLibraryIdFieldName, destinationCalculationLibraryId) });
                    modelDefinitionHelper.SetPropertyValue(node, emissionFactorIdJsonPropertyName, destinationEmissionFactorId.ToString());
                }
            }


        }
    }
}
