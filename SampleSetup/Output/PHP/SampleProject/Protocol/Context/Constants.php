
<?php
namespace Virta\SampleProject\Protocol\Context;

use Virta\SampleProject\Protocol\Context\CurrencyTypes;
use Virta\SampleProject\Protocol\Context\Languages;
class Constants {
    /** @var int */ public const DefaultCurrencyEnumId = CurrencyTypes::EUR;
    /** @var string */ public const DefaultLanguage = Languages::English;
    /** @var string */ public const HttpHeaderUserLanguage = "User-Language";
}
