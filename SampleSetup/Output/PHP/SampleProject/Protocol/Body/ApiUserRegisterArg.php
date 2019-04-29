<?php
namespace SampleProject\Protocol\Body;

class ApiUserRegisterArg {
    /** @var string */ public $UserName;
    /** @var int */ public $UserNameKind;
    /** @var string */ public $Password;

    /** @var string|null */ public $FirstName;

    /** @var string */ public $LastName;
    /** @var bool */ public $IsGuest;
    /** @var string */ public $MobileMSISDN;

    public function __construct()
    {
        $this->UserName = "";
        $this->UserNameKind = 0;
        $this->Password = "";
        $this->FirstName = null;
        $this->LastName = "";
        $this->IsGuest = false;
        $this->MobileMSISDN = "";
    }
}
