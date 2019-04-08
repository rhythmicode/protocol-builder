using AbstractModelWithId = SampleProject.Protocol.Model.AbstractModelWithId;
using ApiCountry = SampleProject.Protocol.Model.ApiCountry;
using ApiAddress = SampleProject.Protocol.Model.ApiAddress;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleProject.Protocol.Model
{
    public class ApiCity : AbstractModelWithId
    {
        public string CountryId { get; set; }

        public ApiCountry Country { get; set; }

        public string TitleShort { get; set; }

        public string TitleLong { get; set; }

        public List<ApiAddress> Addresses { get; set; }

    }
}
