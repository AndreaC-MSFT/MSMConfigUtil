using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSM.ConfigUtil.Logic
{
    public class CalculationModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string JsonDefinition { get; set; }
        public IEnumerable<KeyValuePair<string, object>> AdditionalAttributes { get; set; }
    }
}
