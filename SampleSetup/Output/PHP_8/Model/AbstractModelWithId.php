<?php
namespace SampleProject\Protocol\Model;

class AbstractModelWithId {
    public string $Id;

    public string $InsertUserName;
    public ?string $InsertDate;

    public string $UpdateUserName;
    public ?string $UpdateDate;

    public bool $RemoveIs;
    public string $RemoveUserName;
    public ?string $RemoveDate;
}
