using System;
using System.Collections.Generic;
using System.Text;

namespace SampleProject.Protocol.Body.SomeCategory
{
    [UsingRef(nameof(AbstractReturn), "..")]
    public class ApiSomeCategoryOp1Request : AbstractReturn
    {
        public string Op1Data = "";
    }
}
