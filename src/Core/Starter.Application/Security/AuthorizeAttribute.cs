namespace Starter.Application.Security;

/// <summary>
/// Specifies the class this attribute is applied to requires authorization.
/// </summary>
[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
public class AuthorizeAttribute : Attribute
{
    public AuthorizeAttribute() { }

    public string Roles { get; set; } = string.Empty;

    //   public ModuleType Modules { get; set; } = ModuleType.TODOITEM;

    public string ModuleType { get; set; } = string.Empty;


    public string Actions { get; set; } = string.Empty;



    public string Policy { get; set; } = string.Empty;
}

