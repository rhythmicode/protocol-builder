﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SampleProject.Protocol.Body
{
    public abstract class AbstractReturn
    {
        public DateTime InsertDate = DateTime.UtcNow;
        
        public int ResultEnumId = 0;

        public string ResultDescription = "";
    }
}
