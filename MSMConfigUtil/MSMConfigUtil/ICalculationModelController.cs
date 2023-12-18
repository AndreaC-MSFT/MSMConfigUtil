namespace MSMConfigUtil
{
    public interface ICalculationModelController
    {
        void MigrateCalculationModel(GlobalCLIOptions globalOptions, MigrateModelsCLIOptions migrateModelsOptions);
    }
}
