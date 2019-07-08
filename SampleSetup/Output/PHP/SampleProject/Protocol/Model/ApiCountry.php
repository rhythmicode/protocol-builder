<?php
namespace SampleProject\Protocol\Model;

class ApiCountry extends \SampleProject\Protocol\Model\AbstractModelWithId {
    /** @var string */ public $TitleShort;

    /** @var string */ public $TitleLong;

    /** @var string */ public $PhoneCode;

    /** @var string */ public $Code;

    /** @var \SampleProject\Protocol\Model\ApiCity[] */ public $Cities;

    /** @var \SampleProject\Protocol\Model\ApiAddress[] */ public $Addresses;

    public function __construct()
    {
        parent::__construct();
        $this->Cities = [];
        $this->Addresses = [];
    }

}
