using MSMConfigUtil.Logic.CalculationModelMigration;

public interface ICalculationModelMigrator
{
    void Migrate(bool replaceExisting);
    void Migrate(string calculationModelName, bool replaceExisting);
    void Migrate(CalculationModel sourceModel, bool replaceExisting);
}
