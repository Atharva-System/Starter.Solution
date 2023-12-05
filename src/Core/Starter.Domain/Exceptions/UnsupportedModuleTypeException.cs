namespace Starter.Domain.Exceptions;

public class UnsupportedModuleTypeException : Exception
{
    public UnsupportedModuleTypeException(string code)
        : base($"Module Type \"{code}\" is unsupported.")
    {
    }
}
