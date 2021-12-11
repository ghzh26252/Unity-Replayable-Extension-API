using System;

namespace ReplayableExtension
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ReplayableAttribute : Attribute
    {
        public string id { get; set; }

    }

    
}
