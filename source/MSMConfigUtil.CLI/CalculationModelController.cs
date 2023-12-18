using Microsoft.Xrm.Sdk;
using MSMConfigUtil.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MSMConfigUtil.CLI
{
    public class CalculationModelController : ICalculationModelController
    {
        private readonly ICalculationModelMigratorFactory calcModelMigratorFactory;
        public CalculationModelController(ICalculationModelMigratorFactory calcModelMigratorFactory)
        {
            this.calcModelMigratorFactory = calcModelMigratorFactory;
        }
        public void MigrateCalculationModel(GlobalCLIOptions globalOptions, MigrateModelsCLIOptions migrateModelsOptions)
        {
            var calcModelMigrator = calcModelMigratorFactory.Create(globalOptions);
            if (migrateModelsOptions.MigrateAllModels)
                calcModelMigrator.Migrate(migrateModelsOptions.ReplaceExistingModels);
            else
            {
                if (string.IsNullOrEmpty(migrateModelsOptions.CalculationModelName))
                    throw new ArgumentException("Please specify either --calculation-model-name or --all");
                else
                    calcModelMigrator.Migrate(migrateModelsOptions.CalculationModelName, migrateModelsOptions.ReplaceExistingModels);
            }
        }

    }
}
