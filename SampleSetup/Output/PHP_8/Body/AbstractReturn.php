<?php
namespace SampleProject\Protocol\Body;

class AbstractReturn {
    public string $InsertDate;

    public int $ResultEnumId;

    public string $ResultDescription;

    public function __construct()
    {
        $this->InsertDate = "";
        $this->ResultEnumId = 0;
        $this->ResultDescription = "";
    }
}
