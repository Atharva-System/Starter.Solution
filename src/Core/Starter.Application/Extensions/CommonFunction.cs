using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Starter.Application.Features.Common;
using Starter.Domain.Enums;

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

    public static List<EnumTypeViewDto> GetTaskPriorityList()
    {
        return Enum.GetValues(typeof(TaskPriority))
                   .Cast<TaskPriority>()
                   .Select(t => new EnumTypeViewDto
                   {
                       Id = (int)t,
                       Name = GetEnumMemberValue(t),
                       DisplayName = GetEnumDisplayName(t),
                   }).ToList();
    }

    public static List<EnumTypeViewDto> GetTaskStatusList()
    {
        return Enum.GetValues(typeof(TasksStatus))
                   .Cast<TasksStatus>()
                   .Select(t => new EnumTypeViewDto
                   {
                       Id = (int)t,
                       Name = GetEnumMemberValue(t),
                       DisplayName = GetEnumDisplayName(t),
                   }).ToList();
    }

    public static string GetEnumMemberValue(Enum value)
    {
        var enumType = value.GetType();
        var memberInfo = enumType.GetMember(value.ToString());

        if (memberInfo.Length > 0)
        {
            var maybeNullWhenAttribute = memberInfo[0].GetCustomAttributes(typeof(MaybeNullWhenAttribute), false)
                                                    .FirstOrDefault() as MaybeNullWhenAttribute;

            if (maybeNullWhenAttribute != null)
            {
                var returnValue = maybeNullWhenAttribute.ReturnValue;

                if (returnValue is bool boolValue)
                {
                    return boolValue ? value.ToString() : "";
                }
            }
        }
        return value.ToString();
    }

    public static string GetEnumDisplayName(Enum status)
    {
        var fieldInfo = status.GetType().GetField(status.ToString());
        var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute));

        return attribute != null ? attribute.Description : status.ToString();
    }
}
