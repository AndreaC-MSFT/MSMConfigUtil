using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMConfigUtil.Logic.CalculationModelMigration.NodesConversion
{
    public class ReportGasNodeConverter : IModelDefinitionNodeConverter
    {
        private static readonly string[] supportedActionTypes = ["ReportGas"];
        private readonly IIdConverter idConverter;
        private readonly IModelDefinitionHelper modelDefinitionHelper;


        private const string greenhouseGasIdJsonPropertyName = "greenhouseGasId";


        public ReportGasNodeConverter(IIdConverter idConverter, IModelDefinitionHelper modelDefinitionHelper)
        {
            this.idConverter = idConverter;
            this.modelDefinitionHelper = modelDefinitionHelper;
        }

        public IEnumerable<string> SupportedActionTypes => supportedActionTypes;
        public bool SupportsAnyNode { get { return false; } }


        public void InspectAndConvert(JObject node)
        {
            var greenhouseGasId = modelDefinitionHelper.GetPropertyValue(node, greenhouseGasIdJsonPropertyName);
            if (greenhouseGasId != null && modelDefinitionHelper.IsGuid(greenhouseGasId))
            {
                var destinationId = idConverter.ConvertIdToDestinationEnvironment<string>(CalculationModelsConstants.greenhouseGasTableName, new Guid(greenhouseGasId), CalculationModelsConstants.msdyn_name);
                modelDefinitionHelper.SetPropertyValue(node, greenhouseGasIdJsonPropertyName, destinationId.ToString());
            }
        }
    }
}
