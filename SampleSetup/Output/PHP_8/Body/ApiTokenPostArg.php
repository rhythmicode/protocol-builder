<?php
namespace SampleProject\Protocol\Body;

class ApiTokenPostArg {
    /**
     * @deprecated Message: Use the other one
     */
    public string $UserName;

    /**
     * A sample summary.
     */
    public int $UserNameKind;

    /**
     * A multiline summary.
     * test explanations :)
     */
    public string $Password;

    public function __construct()
    {
        $this->UserName = "";
        $this->UserNameKind = 0;
        $this->Password = "";
    }
}
