import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AccountsService } from '../../accounts/accounts.service';
import { Account } from '../../accounts/accounts.viewmodels';
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
      if (!this.Accounts) { this.Accounts = [] }
      this.Accounts = this.Accounts
         .filter(x => x.Active)
         .sort((a, b) => a.Text < b.Text ? -1 : 1);
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
