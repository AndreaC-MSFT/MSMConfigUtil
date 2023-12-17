using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSM.ConfigUtil.Logic
{
    internal static class DataverseConstants
    {
        internal static string[] fieldsToExclude = {
            "createdby", "createdonbehalfby", "createdon", "importsequencenumber", "modifiedby",
            "modifiedonbehalfby", "modifiedon", "ownerid", "owningbusinessunit", "owningteam", "owninguser",
            "overriddencreatedon", "statecode", "statuscode", "timezoneruleversionnumber", "utcconversiontimezonecode"
        };
    }
}
