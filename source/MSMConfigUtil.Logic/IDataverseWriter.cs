using Microsoft.Xrm.Sdk;

namespace MSMConfigUtil.Logic
{
    public interface IDataverseWriter
    {
        public void Upsert(Entity entity);
    }
}