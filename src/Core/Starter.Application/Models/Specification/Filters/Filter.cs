namespace Starter.Application.Models.Specification.Filters;

public static class FilterOperator
{
    public const string EQ = "equal";
    public const string NEQ = "not_equal";
    public const string LT = "lt";
    public const string LTE = "lte";
    public const string GT = "gt";
    public const string GTE = "gte";
    public const string STARTSWITH = "start_with";
    public const string ENDSWITH = "end_with";
    public const string CONTAINS = "contain";
    public const string NOTCONTAINS = "not_contain";
    public const string ISNULL = "is_null";
    public const string ISNOTNULL = "is_not_null";
}

public static class FilterLogic
{
    public const string AND = "and";
    public const string OR = "or";
    public const string XOR = "xor";
}

public class Filter
{
    public string? Logic { get; set; }

    public IEnumerable<Filter>? Filters { get; set; }

    public string? Field { get; set; }

    public string? Operator { get; set; }

    public object? Value { get; set; }
}
