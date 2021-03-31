<?php
namespace SampleProject\Protocol\Body\SomeCategory;

class ApiSomeCategoryOp1Request extends \SampleProject\Protocol\Body\AbstractReturn {
    public string $Op1Data;

    public function __construct()
    {
        parent::__construct();
        $this->Op1Data = "";
    }
}
