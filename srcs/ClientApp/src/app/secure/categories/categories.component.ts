import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { RelatedData } from 'src/app/shared/related-box/related-box.models';
import { Account } from '../accounts/accounts.service';
import { CategoriesService, enCategoryType } from './categories.service';

@Component({
   selector: 'fs-categories',
   templateUrl: './categories.component.html',
   styleUrls: ['./categories.component.scss']
})
export class CategoriesComponent implements OnInit {

   constructor(private service: CategoriesService,
      private http: HttpClient) { }

   public selectedValue: RelatedData<Account>;

   public async optionsChanging(val: string) {
      this.options = await this.http.get<Account[]>(`api/accounts/${val || ''}`)
         .pipe(map(items => items.map(item => Object.assign(new Account, item))))
         .pipe(map(items => items.map(item => Object.assign(new RelatedData, { id: item.AccountID, description: item.Text, value: item }))))
         .toPromise();
   }

   public options: RelatedData<Account>[] = []

   public async ngOnInit() {
      console.log('categoryTypes', await this.service.getCategoryTypes());
      console.log('categoriesExpense', await this.service.getCategories(enCategoryType.Expense));
      console.log('categoriesIncome', await this.service.getCategories(enCategoryType.Income));
   }

}
