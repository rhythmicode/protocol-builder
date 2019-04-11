
<?php
namespace Virta\SampleProject\Protocol\Body;

class ApiUserRegisterArg {
    /** @var string */ public $UserName = "";
    /** @var int */ public $UserNameKind = 0;
    /** @var string */ public $Password = "";

    /** @var string|null */ public $FirstName = null;

    /** @var string */ public $LastName = "";
    /** @var bool */ public $IsGuest = false;
    /** @var string */ public $MobileMSISDN = "";
}
