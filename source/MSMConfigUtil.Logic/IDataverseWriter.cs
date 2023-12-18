using Microsoft.Xrm.Sdk;

namespace MSMConfigUtil.Logic
{
    public interface IDataverseWriter
    {
        void Upsert(Entity entity);
        void Create(Entity entity);
        void Update(Entity entity);

    }
}