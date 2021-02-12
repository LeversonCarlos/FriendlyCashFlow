import { Component, Input, OnInit } from '@angular/core';
import { CategoriesData, enCategoryType } from '@elesse/categories';
import { ResponsiveService } from '@elesse/shared';
import { Observable } from 'rxjs';
import { filter, map, reduce, switchMap } from 'rxjs/operators';
import { AccountEntries, EntryEntity } from '../../model/entries.model';

@Component({
   selector: 'entries-list-body',
   templateUrl: './list-body.component.html',
   styleUrls: ['./list-body.component.scss']
})
export class ListBodyComponent implements OnInit {

   constructor(private responsive: ResponsiveService, private categoriesData: CategoriesData) { }

   @Input()
   public AccountEntries: AccountEntries;
   public get IsMobile(): boolean { return this.responsive && this.responsive.IsMobile; }

   ngOnInit(): void {
   }

   public OnPaidClick(entry: EntryEntity) {
      entry.Paid = !entry.Paid
   }

   public GetCategoryText(categoryType: enCategoryType, categoryID: string): string {
      return this.categoriesData.GetCategories(categoryType)
         .filter(cat => cat.CategoryID == categoryID)
         .map(cat => cat.HierarchyText)
         .reduce((acc, cur) => cur, '');
   }

}
