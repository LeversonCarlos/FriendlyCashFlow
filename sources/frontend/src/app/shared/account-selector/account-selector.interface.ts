import { Injectable } from "@angular/core";

export interface AccountSelectorServiceInterface {
   readonly TabIndex: number;
   readonly AccountID: string;
   SetTab(tabIndex: number, accountID: string): void;
}

@Injectable({
   providedIn: 'root'
})
export abstract class IAccountSelectorService implements AccountSelectorServiceInterface {
   abstract get TabIndex(): number;
   abstract get AccountID(): string;
   abstract SetTab(tabIndex: number, accountID: string): void;
}
