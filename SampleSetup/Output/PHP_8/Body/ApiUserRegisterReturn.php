<?php
namespace SampleProject\Protocol\Body;

class ApiUserRegisterReturn extends \SampleProject\Protocol\Body\AbstractReturn {
    public int $UserCurrencyTypeEnumId;

    public ?\SampleProject\Protocol\Body\ApiTokenPostReturn $Token;

    public ?string[] $ErrorCodes;

    public function __construct()
    {
        parent::__construct();
        $this->UserCurrencyTypeEnumId = \SampleProject\Protocol\Context\CurrencyTypes::EUR;
        $this->Token = null;
        $this->ErrorCodes = null;
    }
}
