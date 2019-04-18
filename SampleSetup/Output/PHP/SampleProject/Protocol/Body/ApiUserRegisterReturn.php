<?php
namespace SampleProject\Protocol\Body;

use SampleProject\Protocol\Body\AbstractReturn;
use SampleProject\Protocol\Body\ApiTokenPostReturn;
class ApiUserRegisterReturn extends AbstractReturn {
    /** @var ApiTokenPostReturn */ public $Token;

    /** @var string[] */ public $ErrorCodes;
}
