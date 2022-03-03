export interface IVerifiedUser {
    username: string;
    email: string;
    token: string | undefined;
}

export class VerifiedUserClass implements IVerifiedUser {
    username = '';
    email = '';
    token = window.localStorage.getItem('jwt') ?? undefined;
}

export interface IUserLogin {
    email: string;
    password: number;
}
