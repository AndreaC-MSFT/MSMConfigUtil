using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MSM.ConfigUtil.Logic
{
    public interface IModelDefinitionConverter
    {
        public string Convert(string modeldefinitionJson);
    }
}
