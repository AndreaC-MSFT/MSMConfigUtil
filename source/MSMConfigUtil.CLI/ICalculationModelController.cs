namespace MSMConfigUtil.CLI
{
    public interface ICalculationModelController
    {
        void MigrateCalculationModel(GlobalCLIOptions globalOptions, MigrateModelsCLIOptions migrateModelsOptions);
    }
}
