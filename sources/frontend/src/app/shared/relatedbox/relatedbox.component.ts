import { Component, OnDestroy, OnInit } from '@angular/core';
import { ControlValueAccessor } from '@angular/forms';

@Component({
   selector: 'elesse-relatedbox',
   templateUrl: './relatedbox.component.html',
   styleUrls: ['./relatedbox.component.scss']
})
export class RelatedboxComponent implements OnInit, OnDestroy, ControlValueAccessor {

   constructor() { }

   ngOnInit(): void {
   }

   writeValue(obj: any): void {
      throw new Error('Method not implemented.');
   }
   registerOnChange(fn: any): void {
      throw new Error('Method not implemented.');
   }
   registerOnTouched(fn: any): void {
      throw new Error('Method not implemented.');
   }
   setDisabledState?(isDisabled: boolean): void {
      throw new Error('Method not implemented.');
   }

   ngOnDestroy(): void {
      throw new Error('Method not implemented.');
   }

}
