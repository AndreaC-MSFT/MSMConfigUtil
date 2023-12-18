using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMConfigUtil.Logic.CalculationModelMigration.NodesConversion
{
    public class UnitOfMeasureNodeConverter : IModelDefinitionNodeConverter
    {
        private static readonly string[] supportedActionTypes = [];
        private readonly IIdConverter idConverter;
        private readonly IModelDefinitionHelper modelDefinitionHelper;


        private readonly string[] unitOfMeasureJsonProperyNames = ["unitOfMeasureId", "unit", "unitOfMeasureIdForQuantity"];

        public UnitOfMeasureNodeConverter(IIdConverter idConverter, IModelDefinitionHelper modelDefinitionHelper)
        {
            this.idConverter = idConverter;
            this.modelDefinitionHelper = modelDefinitionHelper;
        }

        public IEnumerable<string> SupportedActionTypes => supportedActionTypes;
        public bool SupportsAnyNode { get { return true; } }


        public void InspectAndConvert(JObject node)
        {
            foreach (var unitJsonProperty in unitOfMeasureJsonProperyNames)
            {
                var sourceUnitId = modelDefinitionHelper.GetPropertyValue(node, unitJsonProperty);
                if (sourceUnitId != null && modelDefinitionHelper.IsGuid(sourceUnitId))
                {
                    var destinationUnitId = idConverter.ConvertIdToDestinationEnvironment<string>(CalculationModelsConstants.unitTableName, new Guid(sourceUnitId), CalculationModelsConstants.msdyn_name);
                    modelDefinitionHelper.SetPropertyValue(node, unitJsonProperty, destinationUnitId.ToString());
                }
            }
        }
    }
}
