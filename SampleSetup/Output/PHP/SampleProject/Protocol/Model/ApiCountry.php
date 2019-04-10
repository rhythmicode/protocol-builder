
<?php
require_once(dirname(__FILE__).'/./AbstractModelWithId.php');
require_once(dirname(__FILE__).'/./ApiCity.php');
require_once(dirname(__FILE__).'/./ApiAddress.php');
class ApiCountry extends AbstractModelWithId {
    /** @var string */ public $TitleShort;

    /** @var string */ public $TitleLong;

    /** @var string */ public $PhoneCode;

    /** @var string */ public $Code;

    /** @var ApiCity[] */ public $Cities;

    /** @var ApiAddress[] */ public $Addresses;

}
