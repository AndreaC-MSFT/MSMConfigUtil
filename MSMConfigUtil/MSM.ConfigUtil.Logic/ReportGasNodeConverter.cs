using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSM.ConfigUtil.Logic
{
    public class ReportGasNodeConverter : IModelDefinitionNodeConverter
    {
        private static readonly string[] supportedActionTypes = ["ReportGas"];
        private readonly IIdConverter idConverter;
        private readonly IModelDefinitionHelper modelDefinitionHelper;


        private const string greenhouseGasIdJsonPropertyName = "greenhouseGasId";
        private const string greenhouseGasTableName = "msdyn_greenhousegas";
        private const string nameFieldName = "msdyn_name";

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
                var destinationId = idConverter.ConvertIdToDestinationEnvironment<string>(greenhouseGasTableName, new Guid(greenhouseGasId), nameFieldName);
                modelDefinitionHelper.SetPropertyValue(node, greenhouseGasIdJsonPropertyName, destinationId.ToString());
            }
        }
    }
}
