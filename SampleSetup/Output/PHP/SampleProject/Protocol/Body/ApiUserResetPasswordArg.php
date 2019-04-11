
<?php
namespace Virta\SampleProject\Protocol\Body;

class ApiUserResetPasswordArg {
    /** @var string */ public $UserId;

    /** @var string */ public $Token;

    /** @var string */ public $NewPassword;
}
