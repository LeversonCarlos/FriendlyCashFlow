import { Injectable, Output, Input, EventEmitter, Directive } from '@angular/core';

@Directive()
@Injectable({
   providedIn: 'root'
})
export class BusyService {

   constructor() {
      this.BusyChange = new EventEmitter<boolean>();
      this._IsBusy = false;
   }

   @Output()
   public BusyChange: EventEmitter<boolean>;

   @Input()
   public get IsBusy(): boolean { return this._IsBusy; }
   public set IsBusy(val: boolean) {
      this._IsBusy = val;
   }
   private _IsBusy: boolean;

   public show() {
      this.IsBusy = true;
   }

   public hide() {
      this.IsBusy = false;
   }

}
