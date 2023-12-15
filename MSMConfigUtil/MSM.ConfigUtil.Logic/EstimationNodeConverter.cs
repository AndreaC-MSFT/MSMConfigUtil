using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSM.ConfigUtil.Logic
{
    public class EstimationNodeConverter : IModelDefinitionNodeConverter
    {
        private static readonly string[] supportedActionTypes = ["EstimationFactor"];
        private readonly IIdConverter idConverter;
        private readonly IModelDefinitionHelper modelDefinitionHelper;


        private const string estimationFactorIdJsonPropertyName = "estimationFactorId";
        private const string estimationLibraryIdJsonPropertyName = "estimationFactorLibraryId";
        private const string estimationLibraryTableName = "msdyn_calculationlibrary";
        private const string emissionFactorTableName = "msdyn_estimationfactor";
        private const string estimationLibraryIdFieldName = "msdyn_factorlibrary";


        private const string nameFieldName = "msdyn_name";

        public EstimationNodeConverter(IIdConverter idConverter, IModelDefinitionHelper modelDefinitionHelper)
        {
            this.idConverter = idConverter;
            this.modelDefinitionHelper = modelDefinitionHelper;
        }

        public IEnumerable<string> SupportedActionTypes => supportedActionTypes;
        public bool SupportsAnyNode { get {  return false; } }


        public void InspectAndConvert(JObject node)
        {
            var sourceLibraryId = modelDefinitionHelper.GetPropertyValue(node, estimationLibraryIdJsonPropertyName);
            if (sourceLibraryId != null && modelDefinitionHelper.IsGuid(sourceLibraryId))
            {
                var destinationLibraryId = idConverter.ConvertIdToDestinationEnvironment<string>(estimationLibraryTableName, new Guid(sourceLibraryId), nameFieldName);
                modelDefinitionHelper.SetPropertyValue(node, estimationLibraryIdJsonPropertyName, destinationLibraryId.ToString());

                var sourceEstimationFactorId = modelDefinitionHelper.GetPropertyValue(node, estimationFactorIdJsonPropertyName);
                if (sourceEstimationFactorId != null && modelDefinitionHelper.IsGuid(sourceEstimationFactorId))
                {
                    var destinationEstimationFactorId = idConverter.ConvertIdToDestinationEnvironment<string>(emissionFactorTableName,
                        new Guid(sourceEstimationFactorId), nameFieldName,
                        new KeyValuePair<string, object>[] { new(estimationLibraryIdFieldName, destinationLibraryId) });
                    modelDefinitionHelper.SetPropertyValue(node, estimationFactorIdJsonPropertyName, destinationEstimationFactorId.ToString());
                }
            }


        }
    }
}
