using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MSMConfigUtil.Logic.CalculationModelMigration
{
    public interface IModelDefinitionConverter
    {
        public string Convert(string modeldefinitionJson);
    }
}
