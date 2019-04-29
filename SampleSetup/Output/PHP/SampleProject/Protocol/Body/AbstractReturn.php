<?php
namespace SampleProject\Protocol\Body;

class AbstractReturn {
    /** @var int */ public $ResultEnumId;

    /** @var string */ public $ResultDescription;

    public function __construct()
    {
        $this->ResultEnumId = 0;
        $this->ResultDescription = "";
    }
}
