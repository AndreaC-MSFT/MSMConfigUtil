using MSM.ConfigUtil.Logic;

namespace MSMConfigUtil
{
    public interface INodeConverterCollectionFactory
    {
        IEnumerable<IModelDefinitionNodeConverter> Create(IIdConverter idConverter, IModelDefinitionHelper modelDefinitionHelper);
    }
}