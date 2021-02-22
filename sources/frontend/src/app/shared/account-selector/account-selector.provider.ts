import { IAccountSelectorService } from "./account-selector.interface";
import { AccountSelectorService } from "./account-selector.service";

export const AccountSelectorServiceProvider = {
   provide: IAccountSelectorService,
   useClass: AccountSelectorService
};
