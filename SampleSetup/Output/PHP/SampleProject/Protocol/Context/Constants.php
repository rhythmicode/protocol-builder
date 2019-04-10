
<?php
require_once(dirname(__FILE__).'/./CurrencyTypes.php');
require_once(dirname(__FILE__).'/./Languages.php');
class Constants {
    /** @var int */ public const DefaultCurrencyEnumId = CurrencyTypes::EUR;
    /** @var string */ public const DefaultLanguage = Languages::English;
    /** @var string */ public const HttpHeaderUserLanguage = "User-Language";
}
