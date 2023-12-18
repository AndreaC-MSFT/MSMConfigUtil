namespace MSMConfigUtil.CLI
{

    public interface ICalculationModelMigratorFactory
    {
        ICalculationModelMigrator Create(GlobalCLIOptions globalOptions);
    }
}
