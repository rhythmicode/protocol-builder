<?php
namespace SampleProject\Protocol\Body;

class ApiBodyWithNested {
    /**
     * @var string
     */
    public $Prop1;
    /**
     * @var int|null
     */
    public $Prop2;

    class Nested_1 {
        /**
         * @var string
         */
        public $Prop_1_1;
        /**
         * @var int|null
         */
        public $Prop_1_2;

        class Nested_1_2 {
            /**
             * @var string
             */
            public $Prop_1_2_1;
            /**
             * @var int|null
             */
            public $Prop_1_2_2;

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
