using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MSMConfigUtil.Logic
{
    internal static class DataverseConstants
    {
        internal static string[] fieldsToExclude = {
            "createdby", "createdonbehalfby", "createdon", "importsequencenumber", "modifiedby",
            "modifiedonbehalfby", "modifiedon", "ownerid", "owningbusinessunit", "owningteam", "owninguser",
            "overriddencreatedon", "statecode", "statuscode", "timezoneruleversionnumber", "utcconversiontimezonecode"
        };

        internal static int ErrorCode_RecordNotFoundByEntityKey = Convert.ToInt32("0x80060891", 16);
    }
}
