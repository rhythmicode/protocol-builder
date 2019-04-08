using AbstractModelWithId = SampleProject.Protocol.Model.AbstractModelWithId;
using ApiCountry = SampleProject.Protocol.Model.ApiCountry;
using ApiCity = SampleProject.Protocol.Model.ApiCity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleProject.Protocol.Model
{
    public class ApiAddress : AbstractModelWithId
    {
        public string CountryId { get; set; }

        public ApiCountry Country { get; set; }

        public string CityId { get; set; }

        public ApiCity City { get; set; }

        public string PostalCode { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public double? GeoLat { get; set; }

        public double? GeoLon { get; set; }

        public string Description { get; set; }
    }
}
