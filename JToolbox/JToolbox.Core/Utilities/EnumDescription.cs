using System;

namespace JToolbox.Core.Utilities
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class EnumDescription : Attribute
    {
        public EnumDescription(string description)
        {
            Description = description;
        }

        public string Description { get; set; }

        public static string Get(Enum @enum)
        {
            var field = @enum.GetType().GetField(@enum.ToString());
            var attribute = GetCustomAttribute(field, typeof(EnumDescription)) as EnumDescription;
            return attribute == null ? @enum.ToString() : attribute.Description;
        }
    }
}