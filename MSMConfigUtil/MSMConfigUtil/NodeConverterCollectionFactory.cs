namespace MSMConfigUtil
{
    using MSM.ConfigUtil.Logic;
    using Newtonsoft.Json.Linq;
    using System.Reflection;

    public class NodeConverterCollectionFactory : INodeConverterCollectionFactory
    {
        public IEnumerable<IModelDefinitionNodeConverter> Create(IIdConverter idConverter, IModelDefinitionHelper modelDefinitionHelper)
        {
            var converters = new List<IModelDefinitionNodeConverter>();

            // Get all types that implement IModelDefinitionNodeConverter
            IEnumerable<Type> converterTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(IModelDefinitionNodeConverter).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

            foreach (Type converterType in converterTypes)
            {
                IModelDefinitionNodeConverter converterInstance = null;
                ConstructorInfo constructor;
                // Converters with constructor that accepts IIdConverter, IModelDefinitionHelper
                constructor = converterType.GetConstructor([typeof(IIdConverter), typeof(IModelDefinitionHelper)]);
                converterInstance = (IModelDefinitionNodeConverter)constructor?.Invoke(new object[] { idConverter, modelDefinitionHelper });
                if (converterInstance == null)
                {
                    // Converters with constructor that accepts IIdConverter
                    constructor = converterType.GetConstructor([typeof(IIdConverter)]);
                    converterInstance = (IModelDefinitionNodeConverter)constructor?.Invoke(new object[] { idConverter });
                }
                if (converterInstance == null)
                {
                    // Converters with constructor that accepts IModelDefinitionHelper
                    constructor = converterType.GetConstructor([typeof(IModelDefinitionHelper)]);
                    converterInstance = (IModelDefinitionNodeConverter)constructor?.Invoke(new object[] { modelDefinitionHelper });
                }

                converters.Add(converterInstance);
            }

            return converters;
        }
    }
}
