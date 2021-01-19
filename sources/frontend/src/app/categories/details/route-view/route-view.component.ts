import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BusyService, MessageService } from '@elesse/shared';
import { CategoryEntity } from '../../categories.data';
import { CategoriesService } from '../../categories.service';

@Component({
   selector: 'categories-details-route-view',
   templateUrl: './route-view.component.html',
   styleUrls: ['./route-view.component.scss']
})
export class DetailsRouteViewComponent implements OnInit {

   constructor(private service: CategoriesService, private busy: BusyService, private msg: MessageService,
      private fb: FormBuilder, private activatedRoute: ActivatedRoute, private router: Router) { }

   public inputForm: FormGroup;
   public get IsBusy(): boolean { return this.busy.IsBusy; }

   public async ngOnInit(): Promise<void> {
      const paramID = this.activatedRoute.snapshot.params.id;
      const data = await this.service.LoadCategory(paramID);
      if (!data)
         this.router.navigate(["/categories/list"])
      this.OnFormCreate(data);
   }

   private OnFormCreate(data: CategoryEntity) {
      this.inputForm = this.fb.group({
         AccountID: [data?.CategoryID],
         Text: [data?.Text, Validators.required],
         Type: [data?.Type, Validators.required],
      });
   }

   public async OnCancelClick() {
      if (!this.inputForm.pristine)
         if (!await this.msg.Confirm('shared.ROLLBACK_TEXT', 'shared.ROLLBACK_CONFIRM_COMMAND', 'shared.ROLLBACK_ABORT_COMMAND'))
            return;
      this.router.navigate(["/categories/list"])
   }

   public async OnSaveClick() {
      if (!this.inputForm.valid)
         return;
      const data: CategoryEntity = Object.assign(new CategoryEntity, this.inputForm.value);
      if (!await this.service.SaveCategory(data))
         return;
      this.router.navigate(["/categories/list"])
   }

}
