import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EntryEntity } from '../../entries.data';
import { EntriesService } from '../../entries.service';

@Component({
   selector: 'entries-details-route-view',
   templateUrl: './route-view.component.html',
   styleUrls: ['./route-view.component.scss']
})
export class DetailsRouteViewComponent implements OnInit {

   constructor(private service: EntriesService,
      private activatedRoute: ActivatedRoute, private router: Router) { }

   public data: EntryEntity;

   public async ngOnInit(): Promise<void> {
      const paramID = this.activatedRoute.snapshot.params.id;

      this.data = await this.service.LoadEntry(paramID);
      if (!this.data)
         this.router.navigate(["/entries/list"])

      return null;
   }

}
