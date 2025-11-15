namespace SixShaded.Aleph.ICLI;

public class ConfigKeyException(string configKey, string message) : Exception($"Invalid value for configuration key '{configKey}': {message}.")
{

}