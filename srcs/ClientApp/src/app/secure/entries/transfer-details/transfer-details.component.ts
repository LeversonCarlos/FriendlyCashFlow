import { Component, OnInit } from '@angular/core';
import { EntriesService } from '../entries.service';

@Component({
   selector: 'fs-transfer-details',
   templateUrl: './transfer-details.component.html',
   styleUrls: ['./transfer-details.component.scss']
})
export class TransferDetailsComponent implements OnInit {

   constructor(private service: EntriesService) { }

   public ngOnInit() {
   }

}
