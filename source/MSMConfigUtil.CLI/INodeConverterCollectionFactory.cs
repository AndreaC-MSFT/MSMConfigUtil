using MSMConfigUtil.Logic;
using MSMConfigUtil.Logic.CalculationModelMigration.NodesConversion;

namespace MSMConfigUtil.CLI
{
    public interface INodeConverterCollectionFactory
    {
        IEnumerable<IModelDefinitionNodeConverter> Create(IIdConverter idConverter, IModelDefinitionHelper modelDefinitionHelper);
    }
}