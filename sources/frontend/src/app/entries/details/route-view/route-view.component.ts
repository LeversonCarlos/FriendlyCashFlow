import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
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
      private activatedRoute: ActivatedRoute, private router: Router, private fb: FormBuilder) { }

   public inputForm: FormGroup;

   public async ngOnInit(): Promise<void> {
      const paramID = this.activatedRoute.snapshot.params.id;

      const data = await this.service.LoadEntry(paramID);
      if (!data)
         this.router.navigate(["/entries/list"])

      this.OnFormCreate(data);
   }

   private OnFormCreate(data: EntryEntity) {
      this.inputForm = this.fb.group({
         PatternID: [data.Pattern.CategoryID],
         Text: [data.Pattern.Text, Validators.required],
         Type: [data.Pattern.Type],
         DueDate: [data.DueDate, Validators.required],
         EntryValue: [data.EntryValue, Validators.required],
         Paid: [data.Paid],
         PayDate: [data.PayDate],
      });
   }

}
