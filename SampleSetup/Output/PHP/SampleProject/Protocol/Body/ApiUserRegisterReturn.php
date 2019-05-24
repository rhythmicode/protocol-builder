<?php
namespace SampleProject\Protocol\Body;

use SampleProject\Protocol\Body\AbstractReturn;
use SampleProject\Protocol\Body\ApiTokenPostReturn;
use SampleProject\Protocol\Context\CurrencyTypes;
class ApiUserRegisterReturn extends AbstractReturn {
    /** @var int */ public $UserCurrencyTypeEnumId;

    /** @var ApiTokenPostReturn */ public $Token;

    /** @var string[] */ public $ErrorCodes;

    public function __construct()
    {
        $this->UserCurrencyTypeEnumId = CurrencyTypes::EUR;
    }
}
