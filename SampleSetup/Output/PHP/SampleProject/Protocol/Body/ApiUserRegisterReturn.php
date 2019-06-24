<?php
namespace SampleProject\Protocol\Body;

class ApiUserRegisterReturn extends \SampleProject\Protocol\Body\AbstractReturn {
    /** @var int */ public $UserCurrencyTypeEnumId;

    /** @var \SampleProject\Protocol\Body\ApiTokenPostReturn */ public $Token;

    /** @var string[] */ public $ErrorCodes;

    public function __construct()
    {
        $this->UserCurrencyTypeEnumId = \SampleProject\Protocol\Context\CurrencyTypes::EUR;
    }
}
