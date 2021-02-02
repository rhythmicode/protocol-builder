<?php
namespace SampleProject\Protocol\Body;

class ApiBodyWithNested {
    public string $Prop1;
    public ?int $Prop2;

    class Nested_1 {
        public string $Prop_1_1;
        public ?int $Prop_1_2;

        class Nested_1_2 {
            public string $Prop_1_2_1;
            public ?int $Prop_1_2_2;

    public function __construct()
    {
        $this->Prop_1_2_1 = "";
        $this->Prop_1_2_2 = null;
    }
        }

    public function __construct()
    {
        $this->Prop_1_1 = "";
        $this->Prop_1_2 = null;
    }
    }

    public function __construct()
    {
        $this->Prop1 = "";
        $this->Prop2 = null;
    }
}
