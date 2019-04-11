
<?php
namespace Virta\SampleProject\Protocol\Model;

class AbstractModelWithId {
    /** @var string */ public $Id;

    /** @var string */ public $InsertUserName;
    /** @var string|null */ public $InsertDate;

    /** @var string */ public $UpdateUserName;
    /** @var string|null */ public $UpdateDate;

    /** @var bool */ public $RemoveIs;
    /** @var string */ public $RemoveUserName;
    /** @var string|null */ public $RemoveDate;
}
