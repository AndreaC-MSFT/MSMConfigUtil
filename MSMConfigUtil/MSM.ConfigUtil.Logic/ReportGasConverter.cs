using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSM.ConfigUtil.Logic
{
    public class ReportGasConverter : IModelDefinitionNodeConverter
    {
        private static readonly string[] supportedActionTypes = ["ReportGas"];
        private readonly IIdConverter idConverter;
        private readonly IModelDefinitionHelper modelDefinitionHelper;


        private const string greenhouseGasIdFieldName = "greenhouseGasId";
        private const string greenhouseGasTableName = "msdyn_greenhousegas";
        private const string nameFieldName = "msdyn_name";

        public ReportGasConverter(IIdConverter idConverter, IModelDefinitionHelper modelDefinitionHelper)
        {
            this.idConverter = idConverter;
            this.modelDefinitionHelper = modelDefinitionHelper;
        }

        public IEnumerable<string> SupportedActionTypes => supportedActionTypes;


        public void InspectAndConvert(JObject node)
        {
            var greenhouseGasId = modelDefinitionHelper.GetPropertyValue(node, greenhouseGasIdFieldName);
            if (greenhouseGasId != null && modelDefinitionHelper.IsGuid(greenhouseGasId))
            {
                var destinationId = idConverter.ConvertIdToDestinationEnvironment<string>(greenhouseGasTableName, new Guid(greenhouseGasId), nameFieldName);
                modelDefinitionHelper.SetPropertyValue(node, greenhouseGasIdFieldName, destinationId.ToString());
            }
        }
    }
}
