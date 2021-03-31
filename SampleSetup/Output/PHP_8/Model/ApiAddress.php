<?php
namespace SampleProject\Protocol\Model;

class ApiAddress extends \SampleProject\Protocol\Model\AbstractModelWithId {
    public string $CountryId;

    public \SampleProject\Protocol\Model\ApiCountry $Country;

    public string $CityId;

    public \SampleProject\Protocol\Model\ApiCity $City;

    public string $PostalCode;

    public string $AddressLine1;

    public string $AddressLine2;

    public ?float $GeoLat;

    public ?float $GeoLon;

    public string $Description;
}
