<?php
namespace SampleProject\Protocol\Model;

class ApiCity extends \SampleProject\Protocol\Model\AbstractModelWithId {
    public string $CountryId;

    public \SampleProject\Protocol\Model\ApiCountry $Country;

    public string $TitleShort;

    public string $TitleLong;

    /**
     * @var \SampleProject\Protocol\Model\ApiAddress[]
     */
    public $Addresses;

}
