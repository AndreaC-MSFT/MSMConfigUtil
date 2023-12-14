namespace MSM.ConfigUtil.Logic
{
    public interface IIdConverter
    {
        public Guid ConvertIdToDestinationEnvironment<TMatchingFieldType>(string logicalTableName, Guid sourceRowId, string fieldToMatchInDestination);
    }
}
