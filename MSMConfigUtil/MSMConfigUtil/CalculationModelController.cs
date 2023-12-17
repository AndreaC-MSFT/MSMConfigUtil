using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MSMConfigUtil
{
    internal class CalculationModelController
    {
        public void MigrateCalculationModel(string calculationModelName, bool replaceIfExisting, Uri sourceEnvironmentUri, Uri destinationEnvironmentUri, AuthTypes authType, string clientId, SecureString clientSecret)
        { }
        public void MigrateCalculationModelAll(bool replaceIfExisting, Uri sourceEnvironmentUri, Uri destinationEnvironmentUri, AuthTypes authType, string clientId, SecureString clientSecret)
        { }

    }
}
