using AbstractModelWithId = SampleProject.Protocol.Model.AbstractModelWithId;
using ApiCity = SampleProject.Protocol.Model.ApiCity;
using ApiAddress = SampleProject.Protocol.Model.ApiAddress;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleProject.Protocol.Model
{
    public class ApiCountry : AbstractModelWithId
    {
        public string TitleShort { get; set; }

        public string TitleLong { get; set; }

        public string PhoneCode { get; set; }

        public string Code { get; set; }

        public List<ApiCity> Cities { get; set; } = new List<ApiCity>();

        public List<ApiAddress> Addresses { get; set; } = new List<ApiAddress>();

    }
}
