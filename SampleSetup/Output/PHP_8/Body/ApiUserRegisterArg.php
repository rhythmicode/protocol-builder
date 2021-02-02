<?php
namespace SampleProject\Protocol\Body;

class ApiUserRegisterArg {
    public string $UserName;

    public int $UserNameKind;

    public string $Password;

    public ?string $FirstName;

    public string $LastName;

    public bool $IsGuest;

    /**
     * @deprecated Message: Please use MobileMSISDN
     */
    public string $Mobile;

    public string $MobileMSISDN;

    public function __construct()
    {
        $this->UserName = "";
        $this->UserNameKind = 0;
        $this->Password = "";
        $this->FirstName = null;
        $this->LastName = "";
        $this->IsGuest = false;
        $this->Mobile = "";
        $this->MobileMSISDN = "";
    }
}
