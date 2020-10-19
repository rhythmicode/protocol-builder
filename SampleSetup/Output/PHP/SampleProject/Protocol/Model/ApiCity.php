<?php
namespace SampleProject\Protocol\Model;

class ApiCity extends \SampleProject\Protocol\Model\AbstractModelWithId {
    /**
     * @var string
     */
    public $CountryId;

    /**
     * @var \SampleProject\Protocol\Model\ApiCountry
     */
    public $Country;

    /**
     * @var string
     */
    public $TitleShort;

    /**
     * @var string
     */
    public $TitleLong;

    /**
     * @var \SampleProject\Protocol\Model\ApiAddress[]
     */
    public $Addresses;

}
