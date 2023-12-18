using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;

namespace MSMConfigUtil.Logic
{

    /// <summary>
    /// To avoid the testability limitations of the sealed RetrieveResponse class
    /// </summary>
    public interface IRetrieveResponseReader
    {
        public Entity GetEntity(RetrieveResponse retrieveResponse);
    }
}