import { Component, ElementRef, EventEmitter, OnDestroy, OnInit, Optional, Output, Self } from '@angular/core';
import { ControlValueAccessor, NgControl } from '@angular/forms';

@Component({
   selector: 'elesse-relatedbox',
   templateUrl: './relatedbox.component.html',
   styleUrls: ['./relatedbox.component.scss']
})
export class RelatedboxComponent implements OnInit, OnDestroy, ControlValueAccessor {

   constructor(@Optional() @Self() private ngControl: NgControl, private elRef: ElementRef<HTMLElement>) {
      this.optionsChanging = new EventEmitter<string>();
      if (this.ngControl != null) {
         this.ngControl.valueAccessor = this;
      }
   }

   ngOnInit(): void {
   }

   @Output() public optionsChanging: EventEmitter<string>;

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
