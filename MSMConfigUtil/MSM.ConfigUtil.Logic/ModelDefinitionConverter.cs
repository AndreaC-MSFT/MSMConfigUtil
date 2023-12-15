using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace MSM.ConfigUtil.Logic
{
    public class ModelDefinitionConverter
    {
        private readonly IEnumerable<IModelDefinitionNodeConverter> nodeConverters;
        private readonly IModelDefinitionHelper modelDefinitionHelper;
        public ModelDefinitionConverter(IEnumerable<IModelDefinitionNodeConverter> nodeConverters, IModelDefinitionHelper modelDefinitionHelper)
        {
            this.nodeConverters = nodeConverters;
            this.modelDefinitionHelper = modelDefinitionHelper;
        }
        private void Convert()
        {
            
        }

        public void FindInstances(JToken token)
        {
            if (token is JObject node)
            {
                foreach (var converter in SelectMatchingConverters(node))
                    converter.InspectAndConvert(node);
                foreach (var property in node.Properties())
                    FindInstances(property.Value);
            }
            else if (token is JArray array)
            {
                foreach (var item in array)
                    FindInstances(item);
            }
        }

        private IEnumerable<IModelDefinitionNodeConverter> SelectMatchingConverters(JObject node)
        {
            var nodeType = modelDefinitionHelper.GetActionType(node);
            return nodeConverters.Where(n => n.SupportsAnyNode || n.SupportedActionTypes.Contains(nodeType, StringComparer.OrdinalIgnoreCase));
        }
    }
}
