using System;
using System.Collections.Generic;
using System.Text;

namespace SampleProject.Protocol.Body
{
    public class ApiUserResetPasswordArg
    {
        public string UserId { get; set; }

        public string Token { get; set; }

        public string NewPassword { get; set; }
    }
}
