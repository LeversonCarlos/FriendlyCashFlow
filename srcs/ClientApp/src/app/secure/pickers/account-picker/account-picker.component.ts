import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Account, AccountsService } from '../../accounts/accounts.service';
import { TranslationService } from 'src/app/shared/translation/translation.service';

@Component({
   selector: 'fs-account-picker',
   templateUrl: './account-picker.component.html',
   styleUrls: ['./account-picker.component.scss']
})
export class AccountPickerComponent implements OnInit {

   constructor(private accountsService: AccountsService, private translationService: TranslationService) { }
   public Accounts: Account[];

   public async ngOnInit() {
      this.Accounts = await this.accountsService.getAccounts();
      this.Accounts.splice(0, 0, Object.assign(new Account, { AccountID: 0, Text: '' }));
      this.translationService.getValue('PICKERS_DEFAULT_ACCOUNT_TEXT').then(x => this.Accounts[0].Text = x)
   }


   /* CURRENT ACCOUNT */
   private currentAccount: number
   public get CurrentAccount() {
      return this.currentAccount;
   }
   @Input() public set CurrentAccount(val: number) {
      this.currentAccount = val;
   }
   @Output() public CurrentAccountChanged: EventEmitter<number> = new EventEmitter<number>()

   public get CurrentText(): string {
      return this.Accounts && this.Accounts
         .filter(x => x.AccountID == this.CurrentAccount)
         .reduce((prev, curr) => curr.Text, '')
   }


   public OnItemClick(account: Account) {
      this.CurrentAccount = account.AccountID;
      this.CurrentAccountChanged.emit(this.CurrentAccount)
   }

}
