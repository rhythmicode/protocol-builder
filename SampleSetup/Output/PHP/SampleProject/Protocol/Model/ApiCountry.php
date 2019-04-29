<?php
namespace SampleProject\Protocol\Model;

use SampleProject\Protocol\Model\AbstractModelWithId;
use SampleProject\Protocol\Model\ApiCity;
use SampleProject\Protocol\Model\ApiAddress;
class ApiCountry extends AbstractModelWithId {
    /** @var string */ public $TitleShort;

    /** @var string */ public $TitleLong;

    /** @var string */ public $PhoneCode;

    /** @var string */ public $Code;

    /** @var ApiCity[] */ public $Cities;

    /** @var ApiAddress[] */ public $Addresses;

    public function __construct()
    {
        $this->Cities = [];
        $this->Addresses = [];
    }

}
