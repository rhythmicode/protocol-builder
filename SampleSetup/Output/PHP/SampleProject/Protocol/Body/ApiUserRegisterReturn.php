
<?php
namespace Virta\SampleProject\Protocol\Body;

use Virta\SampleProject\Protocol\Body\AbstractReturn;
use Virta\SampleProject\Protocol\Body\ApiTokenPostReturn;
class ApiUserRegisterReturn extends AbstractReturn {
    /** @var ApiTokenPostReturn */ public $Token;

    /** @var string[] */ public $ErrorCodes;
}
