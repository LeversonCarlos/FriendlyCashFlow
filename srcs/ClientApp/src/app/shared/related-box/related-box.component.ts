import { Component, OnInit, OnDestroy } from '@angular/core';
import { ControlValueAccessor } from '@angular/forms';

@Component({
   selector: 'related-box',
   templateUrl: './related-box.component.html',
   styleUrls: ['./related-box.component.scss']
})
export class RelatedBoxComponent implements OnInit, OnDestroy, ControlValueAccessor {

   constructor() { }

   public ngOnInit() {
   }

   writeValue(obj: any): void {
      throw new Error("Method not implemented.");
   }
   registerOnChange(fn: any): void {
      throw new Error("Method not implemented.");
   }
   registerOnTouched(fn: any): void {
      throw new Error("Method not implemented.");
   }
   setDisabledState?(isDisabled: boolean): void {
      throw new Error("Method not implemented.");
   }

   public ngOnDestroy(): void {
      throw new Error("Method not implemented.");
   }

}
