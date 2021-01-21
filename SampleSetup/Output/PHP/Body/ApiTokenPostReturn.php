<?php
namespace SampleProject\Protocol\Body;

class ApiTokenPostReturn {
    /**
     * @var string
     */
    public $access_token;
    /**
     * @var string
     */
    public $ExpireDateIso;
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
