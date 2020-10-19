<?php
namespace SampleProject\Protocol\Body;

class AbstractReturn {
    /**
     * @var string
     */
    public $InsertDate;

    /**
     * @var int
     */
    public $ResultEnumId;

    /**
     * @var string
     */
    public $ResultDescription;

    public function __construct()
    {
        $this->InsertDate = "";
        $this->ResultEnumId = 0;
        $this->ResultDescription = "";
    }
}
