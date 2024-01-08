namespace Starter.Application.Extensions;
public static class CommonFunction
{
    public static string GetTimestamp(DateTime value)
    {
        return value.ToString("yyyyMMddHHmmssffff");
    }

    public static string ConvertDateToStringForDisplay(DateTime date)
    {
        return date.ToString("MMM dd, yyyy");
    }

    public static string ConvertDateToShortMonthYear(DateTime date)
    {
        return date.ToString("MMMM yyyy");
    }
}
