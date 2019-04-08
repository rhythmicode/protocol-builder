
export class ApiUserRegisterArg {
    UserName: string = "";
    UserNameKind: number = 0;
    Password: string = "";

    FirstName: string | null = null;

    LastName: string = "";
    IsGuest: boolean = false;
    MobileMSISDN: string = "";
}
