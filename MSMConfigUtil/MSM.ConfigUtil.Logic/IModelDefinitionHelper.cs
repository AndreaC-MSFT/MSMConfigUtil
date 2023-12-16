using Newtonsoft.Json.Linq;

public interface IModelDefinitionHelper
{
    public string GetActionType(JObject node);
    string? GetPropertyValue(JObject node, string propertyName);
    public void SetPropertyValue(JObject node, string propertyName, string propertyValue);

    public bool IsGuid(string value);
}