using Microsoft.Extensions.Logging;
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
        private readonly IUserInterfaceHandler uiHandler;
        public CalculationModelController(ICalculationModelMigratorFactory calcModelMigratorFactory, IUserInterfaceHandler uiHandler)
        {
            this.calcModelMigratorFactory = calcModelMigratorFactory;
            this.uiHandler = uiHandler;
        }
        public void MigrateCalculationModel(GlobalCLIOptions globalOptions, MigrateModelsCLIOptions migrateModelsOptions)
        {
            var calcModelMigrator = calcModelMigratorFactory.Create(globalOptions);
            if (migrateModelsOptions.MigrateAllModels)
                calcModelMigrator.Migrate(migrateModelsOptions.ReplaceExistingModels);
            else
            {
                if (string.IsNullOrEmpty(migrateModelsOptions.CalculationModelName))
                    uiHandler.ShowError("Please specify either --calculation-model-name or --all");
                else
                    calcModelMigrator.Migrate(migrateModelsOptions.CalculationModelName, migrateModelsOptions.ReplaceExistingModels);
            }
        }

    }
}
