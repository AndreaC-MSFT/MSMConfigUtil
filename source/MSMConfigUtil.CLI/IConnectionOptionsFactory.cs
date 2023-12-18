using Microsoft.PowerPlatform.Dataverse.Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MSMConfigUtil.CLI
{
    public interface IConnectionOptionsFactory
    {
        ConnectionOptions Create(Uri environmemntUrl, AuthTypes authType, string? clientId, SecureString? clientSecret);
    }
}
