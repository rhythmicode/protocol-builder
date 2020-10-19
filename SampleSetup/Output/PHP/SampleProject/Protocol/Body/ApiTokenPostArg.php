<?php
namespace SampleProject\Protocol\Body;

class ApiTokenPostArg {
    /**
     * @deprecated Message: Use the other one
     * @var string
     */
    public $UserName;

    /**
     * A sample summary.
     * @var int
     */
    public $UserNameKind;

    /**
     * A multiline summary.
     * test explanations :)
     * @var string
     */
    public $Password;

    public function __construct()
    {
        $this->UserName = "";
        $this->UserNameKind = 0;
        $this->Password = "";
    }
}
