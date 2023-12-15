using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSM.ConfigUtil.Logic
{
    public interface IModelDefinitionNodeConverter
    {
        public IEnumerable<string> SupportedActionTypes { get; }
        public bool SupportsAnyNode { get; }
        public void InspectAndConvert(JObject node);
    }
}
