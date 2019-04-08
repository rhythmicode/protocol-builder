using AbstractReturn = SampleProject.Protocol.Body.AbstractReturn;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleProject.Protocol.Body
{
    public class ApiUserResetPasswordReturn : AbstractReturn
    {
        public List<string> ErrorCodes { get; set; }
    }
}
