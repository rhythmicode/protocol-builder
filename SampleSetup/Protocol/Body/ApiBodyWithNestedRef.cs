using System;
using System.Collections.Generic;
using System.Text;

namespace SampleProject.Protocol.Body
{
    public class ApiBodyWithNestedRef
    {
        public List<ApiBodyWithNested.Nested_1.Nested_1_2> PropNestedRefs { get; set; } = null;
    }
}
