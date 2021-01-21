using System;
using System.Collections.Generic;
using System.Text;

namespace SampleProject.Protocol.Body
{
    public class ApiTokenPostReturn
    {
        public string access_token = "";
        public string ExpireDateIso = "";
        public Dictionary<string, string> TestDict { get; set; } = new Dictionary<string, string>();
    }
}
