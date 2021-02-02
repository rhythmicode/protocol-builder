<?php
namespace SampleProject\Protocol\Body;

class ApiUserResetPasswordArg {
    public string $UserId;

    public string $Token;

    public string $NewPassword;
}
