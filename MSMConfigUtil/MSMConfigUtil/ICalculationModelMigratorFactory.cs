using MSMConfigUtil;

public interface ICalculationModelMigratorFactory
{
    ICalculationModelMigrator Create(GlobalCLIOptions globalOptions);
}
