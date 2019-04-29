<?php
namespace SampleProject\Protocol\Context;

use SampleProject\Protocol\Context\CurrencyTypes;
use SampleProject\Protocol\Context\Languages;
class Constants {
    /** @var int */ const DefaultCurrencyEnumId;
    /** @var string */ const DefaultLanguage;
    /** @var string */ const HttpHeaderUserLanguage;

    public function __construct()
    {
        $this->DefaultCurrencyEnumId = CurrencyTypes::EUR;
        $this->DefaultLanguage = Languages::English;
        $this->HttpHeaderUserLanguage = "User-Language";
    }
}
