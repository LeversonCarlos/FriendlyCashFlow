export class User {
   UserID: string;
   Text: string;
}

export class SignUp {
   Description: string;
   UserName: string;
   Password: string;
}

export class SignIn {
   UserName: string;
   Password: string;
   GrantType: string = 'password';
}

export class SignRefresh {
   RefreshToken: string;
   GrantType: string = 'refresh_token';
}
