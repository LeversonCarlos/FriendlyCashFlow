import { IAccountSelectorService } from "./account-selector.interface";
import { AccountSelectorService } from "./account-selector.service";

export const AccountSelectorProvider = {
   provide: IAccountSelectorService,
   useClass: AccountSelectorService
};
