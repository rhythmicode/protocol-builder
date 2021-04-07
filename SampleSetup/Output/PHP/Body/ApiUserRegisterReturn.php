<?php
namespace SampleProject\Protocol\Body;

class ApiUserRegisterReturn extends \SampleProject\Protocol\Body\AbstractReturn {
    /**
     * @var int
     */
    public $UserCurrencyTypeEnumId;

    /**
     * @var \SampleProject\Protocol\Body\ApiTokenPostReturn|null
     */
    public $Token;

    /**
     * @var string[]|null
     */
    public $ErrorCodes;

    public function __construct()
    {
        parent::__construct();
        $this->UserCurrencyTypeEnumId = \SampleProject\Protocol\Context\CurrencyTypes::EUR;
        $this->Token = null;
        $this->ErrorCodes = null;
    }
}
