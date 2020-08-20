using System;
using System.Collections.Generic;
using System.Text;

namespace SampleProject.Protocol.Body
{
    public class ApiTokenPostArg
    {
        [Obsolete("Use the other one")]
        public string UserName = "";

        /// <summary>
        /// A sample summary.
        /// </summary>
        public int UserNameKind = 0;

        /// <summary>
        /// A multiline summary.
        /// test explanations :)
        /// </summary>
        public string Password = "";
    }
}
