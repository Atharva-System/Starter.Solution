namespace Starter.Domain.ValueObjects;

public class ModuleType : ValueObject
{
    static ModuleType()
    {
    }

    private ModuleType()
    {
    }

    private ModuleType(string code)
    {
        Code = code;
    }



    public static ModuleType TODOITEM => new("TodoItem");

    public string Code { get; private set; } = "#000000";

    public static implicit operator string(ModuleType moduleType)
    {
        return moduleType.ToString();
    }


    public override string ToString()
    {
        return Code;
    }

    protected static IEnumerable<ModuleType> SupportedModules
    {
        get
        {
            yield return TODOITEM;

        }
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
    }
}
