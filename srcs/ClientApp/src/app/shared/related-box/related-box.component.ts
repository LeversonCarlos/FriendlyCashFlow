import { Component, OnInit, OnDestroy, Optional, Self, Input, ElementRef, Output, EventEmitter } from '@angular/core';
import { ControlValueAccessor, NgControl } from '@angular/forms';
import { fromEvent, Observable } from 'rxjs';
import { map, debounceTime } from 'rxjs/operators';
import { RelatedData } from './related-box.models';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';

@Component({
   selector: 'related-box',
   templateUrl: './related-box.component.html',
   styleUrls: ['./related-box.component.scss']
})
export class RelatedBoxComponent implements OnInit, OnDestroy, ControlValueAccessor {

   constructor(@Optional() @Self() private ngControl: NgControl, private elRef: ElementRef<HTMLElement>) {
      this.optionsChanging = new EventEmitter<string>();
      if (this.ngControl != null) {
         this.ngControl.valueAccessor = this;
      }
   }

   public ngOnInit() {
      this.inputValue = this.value && this.value.description;
      this.inputValueChanged = fromEvent(this.elRef.nativeElement, 'keyup')
         .pipe(
            map(() => this.inputValue),
            debounceTime(this.delay)
         );
      this.inputValueChanged.subscribe(val => this.OnInputValueChanging(val));
   }

   /* PROPERTIES */
   @Input() public placeholder: string = '';
   @Input() public disabled: boolean = false;
   @Input() public delay: number = 500;
   @Input() public value: RelatedData<any>;
   @Input() public minSize: number = 0;

   /* INPUT VALUE */
   public inputValue: string;
   private inputValueChanged: Observable<string>;
   private OnInputValueChanging(val: string) {
      if (typeof val !== 'string') { return; }
      this.writeValue(null);
      if (val.length < this.minSize) { return; }
      this.optionsChanging.emit(val);
   }

   /* OPTIONS */
   @Input() public options: RelatedData<any>[] = [];
   @Output() public optionsChanging: EventEmitter<string>;
   public OnOptionSelected(val: MatAutocompleteSelectedEvent) {
      this.writeValue(val.option.value);
   }
   public OnDisplayWith(option?: RelatedData<any>): string {
      if (!option) { return ''; }
      else if (typeof option === 'string') { return option; }
      else { return option.description; }
   }

   public OnFocus() {
      this.OnInputValueChanging(this.inputValue);
   }

   /* VALUE ACCESSOR  */
   public writeValue(val: RelatedData<any>): void {
      this.value = val
      this.onChange(val);
   }
   onChange = (val: RelatedData<any>) => { };
   registerOnChange(fn: any): void {
      this.onChange = fn;
   }
   registerOnTouched(fn: any): void {
      // throw new Error("Method not implemented.");
   }
   setDisabledState?(isDisabled: boolean): void {
      // throw new Error("Method not implemented.");
   }

   public ngOnDestroy(): void {
      this.inputValueChanged = null;
      this.optionsChanging.complete();
      this.optionsChanging = null;
   }

}
