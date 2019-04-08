using AbstractReturn = SampleProject.Protocol.Body.AbstractReturn;
using ApiTokenPostReturn = SampleProject.Protocol.Body.ApiTokenPostReturn;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleProject.Protocol.Body
{
    public class ApiUserRegisterReturn : AbstractReturn
    {
        public ApiTokenPostReturn Token { get; set; }

        public List<string> ErrorCodes { get; set; }
    }
}
