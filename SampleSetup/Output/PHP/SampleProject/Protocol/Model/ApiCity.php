
<?php
require_once(dirname(__FILE__).'/./AbstractModelWithId.php');
require_once(dirname(__FILE__).'/./ApiCountry.php');
require_once(dirname(__FILE__).'/./ApiAddress.php');
class ApiCity extends AbstractModelWithId {
    /** @var string */ public $CountryId;

    /** @var ApiCountry */ public $Country;

    /** @var string */ public $TitleShort;

    /** @var string */ public $TitleLong;

    /** @var ApiAddress[] */ public $Addresses;

}
