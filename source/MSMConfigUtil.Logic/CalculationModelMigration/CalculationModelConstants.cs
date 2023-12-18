using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMConfigUtil.Logic.CalculationModelMigration
{
    internal static class CalculationModelsConstants
    {
        internal static int calculationModelType_Custom = 700610000;

        internal static string msdyn_emissioncalculationid = "msdyn_emissioncalculationid";
        internal static string msdyn_name = "msdyn_name";
        internal static string msdyn_calculationflowjson = "msdyn_calculationflowjson";
        internal static string msdyn_emissioncalculation = "msdyn_emissioncalculation";
        internal static string msdyn_type = "msdyn_type";

        internal const string estimationLibraryTableName = "msdyn_calculationlibrary";
        internal const string estimationFactorTableName = "msdyn_estimationfactor";
        internal const string estimationLibraryIdFieldName = "msdyn_factorlibrary";

        internal const string greenhouseGasTableName = "msdyn_greenhousegas";

        internal const string calculationLibraryTableName = "msdyn_calculationlibrary";
        internal const string emissionFactorTableName = "msdyn_emissionfactor";
        internal const string calculationLibraryIdFieldName = "msdyn_calculationlibraryid";

        internal const string unitTableName = "msdyn_unit";
    }
}
