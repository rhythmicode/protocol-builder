using System;
using System.Collections.Generic;
using System.Text;

namespace SampleProject.Protocol.Model
{
    public abstract class AbstractModelWithId
    {
        public string Id;

        public string InsertUserName;
        public DateTime? InsertDate;

        public string UpdateUserName;
        public DateTime? UpdateDate;

        public bool RemoveIs;
        public string RemoveUserName;
        public DateTime? RemoveDate;
    }
}
