import { Component, OnInit } from '@angular/core';
import { AnalyticsService } from '../analytics.service';
import { CategoryGoalsVM } from '../analytics.viewmodels';

@Component({
   selector: 'fs-category-goals',
   templateUrl: './category-goals.component.html',
   styleUrls: ['./category-goals.component.scss']
})
export class CategoryGoalsComponent implements OnInit {

   constructor(private service: AnalyticsService) { }

   public get CategoryGoals(): CategoryGoalsVM[] { return this.service.CategoryGoals }

   ngOnInit() {
   }

}
