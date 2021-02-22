import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AccountEntity, AccountsData } from '@elesse/accounts';
import { RelatedData } from '@elesse/shared';
import { TransferEntity } from 'src/app/transfers/model/transfers.model';
import { ControlNames } from '../../details.control-names';

@Component({
   selector: 'transfers-details-income-account',
   templateUrl: './income-account.component.html',
   styleUrls: ['./income-account.component.scss']
})
export class IncomeAccountComponent implements OnInit {

   constructor(private accountsData: AccountsData) { }

   @Output() OnChange: EventEmitter<void> = new EventEmitter<void>();
   @Input() data: TransferEntity;
   @Input() form: FormGroup;
   public formControlName: string = ControlNames.IncomeAccountRow;
   public AccountOptions: RelatedData<AccountEntity>[] = [];
   public AccountFiltered: RelatedData<AccountEntity>[] = [];

   ngOnInit(): void {
      if (!this.data)
         return;
      this.accountsData.OnObservableFirstPush(() => {
         this.OnDataInit();
         this.OnFormInit();
      });
   }

   private OnDataInit() {
      this.AccountOptions = this.accountsData.GetAccounts(true)
         .map(entity => Object.assign(new RelatedData, {
            id: entity.AccountID,
            description: entity.Text,
            value: entity
         }));
      if (this.data.IncomeAccountID)
         this.AccountFiltered = this.AccountOptions
            .filter(entity => entity.value.AccountID == this.data.IncomeAccountID)
   }

   private OnFormInit() {
      if (!this.form)
         return;
      this.form.addControl(ControlNames.IncomeAccountID, new FormControl(this.data.IncomeAccountID ?? null));
      this.form.addControl(ControlNames.IncomeAccountRow, new FormControl(this.GetFirstAccount(), [Validators.required]));
      this.form.get(ControlNames.IncomeAccountRow).valueChanges.subscribe((row: RelatedData<AccountEntity>) => {
         this.data.IncomeAccountID = row?.value?.AccountID ?? null;
         this.OnChange.emit();
      });
   }

   public OnAccountChanging(val: string) {
      this.AccountFiltered = this.AccountOptions
         .filter(entity => entity.value.Text.search(new RegExp(val, 'i')) != -1)
         .sort((a, b) => a.description > b.description ? 1 : -1)
   }

   private GetFirstAccount = (): RelatedData<AccountEntity> =>
      this.AccountFiltered?.length == 1 ? this.AccountFiltered[0] : null;

}
