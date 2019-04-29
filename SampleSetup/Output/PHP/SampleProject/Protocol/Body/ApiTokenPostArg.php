<?php
namespace SampleProject\Protocol\Body;

class ApiTokenPostArg {
    /** @var string */ public $UserName;
    /** @var int */ public $UserNameKind;
    /** @var string */ public $Password;

    public function __construct()
    {
        $this->UserName = "";
        $this->UserNameKind = 0;
        $this->Password = "";
    }
}
