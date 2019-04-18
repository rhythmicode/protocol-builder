<?php
namespace SampleProject\Protocol\Context;

use SampleProject\Protocol\Context\CurrencyTypes;
use SampleProject\Protocol\Context\Languages;
class Constants {
    /** @var int */ public const DefaultCurrencyEnumId = CurrencyTypes::EUR;
    /** @var string */ public const DefaultLanguage = Languages::English;
    /** @var string */ public const HttpHeaderUserLanguage = "User-Language";
}
