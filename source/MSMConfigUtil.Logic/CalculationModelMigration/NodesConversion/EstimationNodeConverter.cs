using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMConfigUtil.Logic.CalculationModelMigration.NodesConversion
{
    public class EstimationNodeConverter : IModelDefinitionNodeConverter
    {
        private static readonly string[] supportedActionTypes = ["EstimationFactor"];
        private readonly IIdConverter idConverter;
        private readonly IModelDefinitionHelper modelDefinitionHelper;

        private const string estimationFactorIdJsonPropertyName = "estimationFactorId";
        private const string estimationLibraryIdJsonPropertyName = "estimationFactorLibraryId";

        public EstimationNodeConverter(IIdConverter idConverter, IModelDefinitionHelper modelDefinitionHelper)
        {
            this.idConverter = idConverter;
            this.modelDefinitionHelper = modelDefinitionHelper;
        }

        public IEnumerable<string> SupportedActionTypes => supportedActionTypes;
        public bool SupportsAnyNode { get { return false; } }


        public void InspectAndConvert(JObject node)
        {
            var sourceLibraryId = modelDefinitionHelper.GetPropertyValue(node, estimationLibraryIdJsonPropertyName);
            if (sourceLibraryId != null && modelDefinitionHelper.IsGuid(sourceLibraryId))
            {
                var destinationLibraryId = idConverter.ConvertIdToDestinationEnvironment<string>(CalculationModelsConstants.estimationLibraryTableName, new Guid(sourceLibraryId), CalculationModelsConstants.msdyn_name);
                modelDefinitionHelper.SetPropertyValue(node, estimationLibraryIdJsonPropertyName, destinationLibraryId.ToString());

                var sourceEstimationFactorId = modelDefinitionHelper.GetPropertyValue(node, estimationFactorIdJsonPropertyName);
                if (sourceEstimationFactorId != null && modelDefinitionHelper.IsGuid(sourceEstimationFactorId))
                {
                    var destinationEstimationFactorId = idConverter.ConvertIdToDestinationEnvironment<string>(CalculationModelsConstants.estimationFactorTableName,
                        new Guid(sourceEstimationFactorId), CalculationModelsConstants.msdyn_name,
                        new KeyValuePair<string, object>[] { new(CalculationModelsConstants.estimationLibraryIdFieldName, destinationLibraryId) });
                    modelDefinitionHelper.SetPropertyValue(node, estimationFactorIdJsonPropertyName, destinationEstimationFactorId.ToString());
                }
            }


        }
    }
}
