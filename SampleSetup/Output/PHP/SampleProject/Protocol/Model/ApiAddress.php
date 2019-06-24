<?php
namespace SampleProject\Protocol\Model;

class ApiAddress extends \SampleProject\Protocol\Model\AbstractModelWithId {
    /** @var string */ public $CountryId;

    /** @var \SampleProject\Protocol\Model\ApiCountry */ public $Country;

    /** @var string */ public $CityId;

    /** @var \SampleProject\Protocol\Model\ApiCity */ public $City;

    /** @var string */ public $PostalCode;

    /** @var string */ public $AddressLine1;

    /** @var string */ public $AddressLine2;

    /** @var float|null */ public $GeoLat;

    /** @var float|null */ public $GeoLon;

    /** @var string */ public $Description;
}
