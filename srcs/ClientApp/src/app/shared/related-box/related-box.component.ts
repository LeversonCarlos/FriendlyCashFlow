import { Component, OnInit, OnDestroy } from '@angular/core';

@Component({
   selector: 'related-box',
   templateUrl: './related-box.component.html',
   styleUrls: ['./related-box.component.scss']
})
export class RelatedBoxComponent implements OnInit, OnDestroy {

   constructor() { }

   public ngOnInit() {
   }

   public ngOnDestroy(): void {
      throw new Error("Method not implemented.");
   }

}
