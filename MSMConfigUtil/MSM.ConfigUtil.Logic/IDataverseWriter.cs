using Microsoft.Xrm.Sdk;

namespace MSM.ConfigUtil.Logic
{
    public interface IDataverseWriter
    {
        public void Upsert(Entity entity);
    }
}