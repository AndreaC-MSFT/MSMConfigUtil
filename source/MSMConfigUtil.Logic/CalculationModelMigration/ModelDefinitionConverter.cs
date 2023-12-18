using MSMConfigUtil.Logic.CalculationModelMigration.NodesConversion;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace MSMConfigUtil.Logic.CalculationModelMigration
{
    public class ModelDefinitionConverter : IModelDefinitionConverter
    {
        private readonly IEnumerable<IModelDefinitionNodeConverter> nodeConverters;
        private readonly IModelDefinitionHelper modelDefinitionHelper;
        public ModelDefinitionConverter(IEnumerable<IModelDefinitionNodeConverter> nodeConverters, IModelDefinitionHelper modelDefinitionHelper)
        {
            this.nodeConverters = nodeConverters;
            this.modelDefinitionHelper = modelDefinitionHelper;
        }
        public string Convert(string modeldefinitionJson)
        {
            var token = JToken.Parse(modeldefinitionJson);
            ConvertNode(token);
            return token.ToString();
        }

        private void ConvertNode(JToken token)
        {
            if (token is JObject node)
            {
                foreach (var converter in SelectMatchingConverters(node))
                    converter.InspectAndConvert(node);
                foreach (var property in node.Properties())
                    ConvertNode(property.Value);
            }
            else if (token is JArray array)
            {
                foreach (var item in array)
                    ConvertNode(item);
            }
        }

        private IEnumerable<IModelDefinitionNodeConverter> SelectMatchingConverters(JObject node)
        {
            var nodeType = modelDefinitionHelper.GetActionType(node);
            return nodeConverters.Where(n => n.SupportsAnyNode || n.SupportedActionTypes.Contains(nodeType, StringComparer.InvariantCultureIgnoreCase));
        }
    }
}
