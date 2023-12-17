using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Binding;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MSMConfigUtil
{
    public class MigrateModelsCLIOptions
    {
        public string? CalculationModelName {  get; set; }
        public bool MigrateAllModels { get; set; }
        public bool ReplaceExistingModels {  get; set; }
    }

    public class MigrateModelsCLIOptionsBinder : BinderBase<MigrateModelsCLIOptions>
    {
        private readonly Option<string?> calculationModelNameOption;
        private readonly Option<bool> migrateAllModelsOption;
        private readonly Option<bool> replaceExistingModelsOption;

        public MigrateModelsCLIOptionsBinder(Option<string?> calculationModelNameOption, Option<bool> migrateAllModelsOption, Option<bool> replaceExistingModelsOption)
        {
            this.calculationModelNameOption = calculationModelNameOption;
            this.migrateAllModelsOption = migrateAllModelsOption;
            this.replaceExistingModelsOption = replaceExistingModelsOption;
        }
        protected override MigrateModelsCLIOptions GetBoundValue(BindingContext bindingContext)
        {
            return new MigrateModelsCLIOptions
            {
                CalculationModelName = bindingContext.ParseResult.GetValueForOption(calculationModelNameOption),
                MigrateAllModels = bindingContext.ParseResult.GetValueForOption(migrateAllModelsOption),
                ReplaceExistingModels = bindingContext.ParseResult.GetValueForOption(replaceExistingModelsOption)
            };
        }
    }
}
