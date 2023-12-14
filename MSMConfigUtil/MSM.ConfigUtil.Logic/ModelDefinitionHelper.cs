using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSM.ConfigUtil.Logic
{
    public class ModelDefinitionHelper
    {
        public string GetActionType(JObject node)
        {
            return GetPropertyValue(node,"actionType") ?? string.Empty;
        }

        public string? GetPropertyValue(JObject node, string propertyName)
        {
            return node.Properties().FirstOrDefault(n => n.Name == propertyName)?.Value.ToString();
        }

        public void SetPropertyValue(JObject node, string propertyName, string propertyValue)
        {
            var property = node.Properties().FirstOrDefault(n => n.Name == propertyName);
            property.Value.Replace(propertyValue);
        }

        public bool IsGuid(string value)
        {
            return Guid.TryParse(value, out _);
        }
    }


}
