
<?php
namespace Virta\SampleProject\Protocol\Model;

use Virta\SampleProject\Protocol\Model\AbstractModelWithId;
use Virta\SampleProject\Protocol\Model\ApiCountry;
use Virta\SampleProject\Protocol\Model\ApiAddress;
class ApiCity extends AbstractModelWithId {
    /** @var string */ public $CountryId;

    /** @var ApiCountry */ public $Country;

    /** @var string */ public $TitleShort;

    /** @var string */ public $TitleLong;

    /** @var ApiAddress[] */ public $Addresses;

}
