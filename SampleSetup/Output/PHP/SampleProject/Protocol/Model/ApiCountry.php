
<?php
namespace Virta\SampleProject\Protocol\Model;

use Virta\SampleProject\Protocol\Model\AbstractModelWithId;
use Virta\SampleProject\Protocol\Model\ApiCity;
use Virta\SampleProject\Protocol\Model\ApiAddress;
class ApiCountry extends AbstractModelWithId {
    /** @var string */ public $TitleShort;

    /** @var string */ public $TitleLong;

    /** @var string */ public $PhoneCode;

    /** @var string */ public $Code;

    /** @var ApiCity[] */ public $Cities;

    /** @var ApiAddress[] */ public $Addresses;

}
