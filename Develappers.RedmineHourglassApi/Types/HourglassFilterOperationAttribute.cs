using System;

namespace Develappers.RedmineHourglassApi.Types;

[AttributeUsage(AttributeTargets.Property)]
internal class HourglassFilterOperationAttribute(string fieldName) : Attribute
{
    public string FieldName { get; } = fieldName;
}