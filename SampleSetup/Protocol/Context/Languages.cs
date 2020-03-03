using System;
using System.Collections.Generic;
using System.Text;

namespace SampleProject.Protocol.Context
{
    [EnumAsString]
    public static class Languages
    {
        public const string English = "en";
        
        [Obsolete("Please use Finnish")]
        public const string Suomi = "fi-old";
        
        public const string Finnish = "fi";
    }
}
