using System;

namespace ProtocolBuilder
{
    public class OutputAsAttribute : Attribute
    {
        public string Output { get; set; }
        public Languages Language { get; set; }

        public OutputAsAttribute()
        {
        }
    }
}