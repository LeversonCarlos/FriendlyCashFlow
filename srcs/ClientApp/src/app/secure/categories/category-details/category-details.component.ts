import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { enCategoryType } from '../categories.service';

@Component({
   selector: 'fs-category-details',
   templateUrl: './category-details.component.html',
   styleUrls: ['./category-details.component.scss']
})
export class CategoryDetailsComponent implements OnInit {

   constructor(private route: ActivatedRoute) { }

   ngOnInit() {

      const paramID: string = this.route.snapshot.params.id;
      if (paramID.startsWith('new-')) {
         const categoryType: enCategoryType = (paramID.replace('new-', '') as any);
         console.log('categoryType', categoryType);
      }
      else {
         console.log('categoryID', paramID);
      }


   }

}
