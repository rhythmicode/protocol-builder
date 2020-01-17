using System;
using System.Collections.Generic;
using System.Text;

namespace SampleProject.Protocol.Body
{
    public class ApiBodyWithNested
    {
        public string Prop1 { get; set; } = "";
        public int? Prop2 { get; set; } = null;

        public class Nested_1
        {
            public string Prop_1_1 { get; set; } = "";
            public int? Prop_1_2 { get; set; } = null;

            public class Nested_1_2
            {
                public string Prop_1_2_1 { get; set; } = "";
                public int? Prop_1_2_2 { get; set; } = null;
            }
        }
    }
}
