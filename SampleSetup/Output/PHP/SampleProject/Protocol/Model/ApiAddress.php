
<?php
require_once(dirname(__FILE__).'/./AbstractModelWithId.php');
require_once(dirname(__FILE__).'/./ApiCountry.php');
require_once(dirname(__FILE__).'/./ApiCity.php');
class ApiAddress extends AbstractModelWithId {
    /** @var string */ public $CountryId;

    /** @var ApiCountry */ public $Country;

    /** @var string */ public $CityId;

    /** @var ApiCity */ public $City;

    /** @var string */ public $PostalCode;

    /** @var string */ public $AddressLine1;

    /** @var string */ public $AddressLine2;

    /** @var float|null */ public $GeoLat;

    /** @var float|null */ public $GeoLon;

    /** @var string */ public $Description;
}
