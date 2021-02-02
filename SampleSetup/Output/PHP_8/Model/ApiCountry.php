<?php
namespace SampleProject\Protocol\Model;

class ApiCountry extends \SampleProject\Protocol\Model\AbstractModelWithId {
    public string $TitleShort;

    public string $TitleLong;

    public string $PhoneCode;

    public string $Code;

    /**
     * @var \SampleProject\Protocol\Model\ApiCity[]
     */
    public $Cities;

    /**
     * @var \SampleProject\Protocol\Model\ApiAddress[]
     */
    public $Addresses;

    public function __construct()
    {
        parent::__construct();
        $this->Cities = [];
        $this->Addresses = [];
    }

}
