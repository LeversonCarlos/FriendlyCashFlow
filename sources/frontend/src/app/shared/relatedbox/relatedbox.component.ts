import { Component, ElementRef, EventEmitter, Input, OnDestroy, OnInit, Optional, Output, Self, ViewChild } from '@angular/core';
import { ControlValueAccessor, NgControl } from '@angular/forms';
import { MatAutocompleteTrigger } from '@angular/material/autocomplete';
import { fromEvent, Observable } from 'rxjs';
import { debounceTime, map } from 'rxjs/operators';
import { RelatedData } from './relatedbox.models';

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
      this.inputValue = this.value && this.value.description;
      this.inputValueChanged = fromEvent(this.elRef.nativeElement, 'keyup')
         .pipe(
            map(() => this.inputValue),
            debounceTime(this.delay)
         );
      this.inputValueChanged.subscribe(val => this.OnInputValueChanging(val));
   }

   /* PROPERTIES */
   @ViewChild(MatAutocompleteTrigger, { read: MatAutocompleteTrigger }) autoComplete: MatAutocompleteTrigger;
   @Output() public optionsChanging: EventEmitter<string>;
   @Input() public placeholder: string = '';
   @Input() public disabled: boolean = false;
   @Input() public delay: number = 500;
   @Input() public value: RelatedData<any>;
   @Input() public minSize: number = 0;
   @Input() public textSuffix: string = '';

   /* INPUT VALUE */
   public inputValue: string;
   private inputValueChanged: Observable<string>;

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
