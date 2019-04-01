using System;

namespace Develappers.RedmineHourglassApi.Types
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class HourglassFilterOperationAttribute : Attribute
    {
        public HourglassFilterOperationAttribute(string fieldName)
        {
            FieldName = fieldName;
        }

        public string FieldName { get; }   
    }
}