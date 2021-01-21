<?php
namespace SampleProject\Protocol\Body;

class ApiBodyWithNestedRef {
    /**
     * @var ApiBodyWithNested.Nested_1.Nested_1_2[]|null
     */
    public $PropNestedRefs;

    public function __construct()
    {
        $this->PropNestedRefs = null;
    }
}
