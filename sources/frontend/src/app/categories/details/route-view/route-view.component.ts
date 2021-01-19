import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CategoryEntity } from '../../categories.data';
import { CategoriesService } from '../../categories.service';

@Component({
   selector: 'categories-details-route-view',
   templateUrl: './route-view.component.html',
   styleUrls: ['./route-view.component.scss']
})
export class DetailsRouteViewComponent implements OnInit {

   constructor(private service: CategoriesService,
      private activatedRoute: ActivatedRoute, private router: Router) { }

   public async ngOnInit(): Promise<void> {
      const paramID = this.activatedRoute.snapshot.params.id;
      const data = await this.service.LoadCategory(paramID);
      if (!data)
         this.router.navigate(["/categories/list"])
      // this.AccountTypes = await this.service.GetAccountTypes();
      // this.OnFormCreate(data);
      this.TMP = data;
   }

   public TMP: CategoryEntity

}
