<?php
namespace SampleProject\Protocol\Context;

use SampleProject\Protocol\Context\CurrencyTypes;
use SampleProject\Protocol\Context\Languages;
class Constants {
    const DefaultCurrencyEnumId = CurrencyTypes::EUR;
    const DefaultLanguage = Languages::English;
    const HttpHeaderUserLanguage = "User-Language";
}
