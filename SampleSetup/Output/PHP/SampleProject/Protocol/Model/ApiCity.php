<?php
namespace SampleProject\Protocol\Model;

use SampleProject\Protocol\Model\AbstractModelWithId;
use SampleProject\Protocol\Model\ApiCountry;
use SampleProject\Protocol\Model\ApiAddress;
class ApiCity extends AbstractModelWithId {
    /** @var string */ public $CountryId;

    /** @var ApiCountry */ public $Country;

    /** @var string */ public $TitleShort;

    /** @var string */ public $TitleLong;

    /** @var ApiAddress[] */ public $Addresses;

}
