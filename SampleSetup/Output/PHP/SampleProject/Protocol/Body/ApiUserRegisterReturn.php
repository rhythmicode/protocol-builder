<?php
require_once(dirname(__FILE__).'/./AbstractReturn.php');
require_once(dirname(__FILE__).'/./ApiTokenPostReturn.php');
class ApiUserRegisterReturn extends AbstractReturn {
    /** @var ApiTokenPostReturn */ public $Token;

    /** @var string[] */ public $ErrorCodes;
}
