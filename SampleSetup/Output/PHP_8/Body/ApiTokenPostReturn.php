<?php
namespace SampleProject\Protocol\Body;

class ApiTokenPostReturn {
    public string $access_token;
    public string $ExpireDateIso;
    /**
     * @var array
     */
    public $TestDict;

    public function __construct()
    {
        $this->access_token = "";
        $this->ExpireDateIso = "";
        $this->TestDict = [];
    }
}
