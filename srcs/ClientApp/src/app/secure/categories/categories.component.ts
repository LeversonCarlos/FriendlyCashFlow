import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { RelatedData } from 'src/app/shared/related-box/related-box.models';
import { Account } from '../accounts/accounts.service';
import { CategoriesService, enCategoryType, CategoryType } from './categories.service';
import { async } from 'rxjs/internal/scheduler/async';

@Component({
   selector: 'fs-categories',
   templateUrl: './categories.component.html',
   styleUrls: ['./categories.component.scss']
})
export class CategoriesComponent implements OnInit {

   constructor(private service: CategoriesService,
      private http: HttpClient) { }
   public Data: CategoryType[] = [];

   public async ngOnInit() {
      this.Data = await this.service.getCategoryTypes();
      for (const item of this.Data) {
         item.Categories = await this.service.getCategories(item.Value);
      }
   }

   public OnTypeSelected(tabIndex: number) {
      console.log(this.Data[tabIndex]);
   }

   public selectedValue: RelatedData<Account>;

   public async optionsChanging(val: string) {
      this.options = await this.http.get<Account[]>(`api/accounts/${val || ''}`)
         .pipe(map(items => items.map(item => Object.assign(new Account, item))))
         .pipe(map(items => items.map(item => Object.assign(new RelatedData, { id: item.AccountID, description: item.Text, value: item }))))
         .toPromise();
   }

   public options: RelatedData<Account>[] = []


}
