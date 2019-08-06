import { Component, OnInit } from '@angular/core';
import { RelatedData } from 'src/app/shared/related-box/related-box.models';
import { Account } from '../accounts/accounts.service';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Component({
   selector: 'fs-categories',
   templateUrl: './categories.component.html',
   styleUrls: ['./categories.component.scss']
})
export class CategoriesComponent implements OnInit {

   constructor(private http: HttpClient) { }

   public selectedValue: RelatedData<Account>;

   public async optionsChanging(val: string) {
      this.options = await this.http.get<Account[]>(`api/accounts/${val || ''}`)
         .pipe(map(items => items.map(item => Object.assign(new Account, item))))
         .pipe(map(items => items.map(item => Object.assign(new RelatedData, { id: item.AccountID, description: item.Text, value: item }))))
         .toPromise();
   }

   public options: RelatedData<Account>[] = []

   ngOnInit() {
   }

}
