using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMConfigUtil.Logic.CalculationModelMigration.NodesConversion
{
    public interface IModelDefinitionNodeConverter
    {
        public IEnumerable<string> SupportedActionTypes { get; }
        public bool SupportsAnyNode { get; }
        public void InspectAndConvert(JObject node);
    }
}
