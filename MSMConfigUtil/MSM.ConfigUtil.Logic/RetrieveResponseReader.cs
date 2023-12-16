using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;

namespace MSM.ConfigUtil.Logic
{
    /// <summary>
    /// To avoid the testability limitations of the sealed RetrieveResponse class
    /// </summary>
    public class RetrieveResponseReader : IRetrieveResponseReader
    {
        public Entity GetEntity(RetrieveResponse retrieveResponse)
        {
            if (retrieveResponse == null) return null;
            return retrieveResponse.Entity;
        }
    }
}
