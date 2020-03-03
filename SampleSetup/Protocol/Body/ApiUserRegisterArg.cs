using System;
using System.Collections.Generic;
using System.Text;

namespace SampleProject.Protocol.Body
{
    public class ApiUserRegisterArg
    {
        public string UserName { get; set; } = "";

        public int UserNameKind { get; set; } = 0;

        public string Password { get; set; } = "";

        [Nullable]
        public string FirstName { get; set; } = null;

        public string LastName { get; set; } = "";

        public bool IsGuest { get; set; } = false;

        [Obsolete("Please use MobileMSISDN")]
        public string Mobile { get; set; } = "";

        public string MobileMSISDN { get; set; } = "";
    }
}
