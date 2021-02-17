import { Component, ElementRef, EventEmitter, Input, OnDestroy, OnInit, Optional, Output, Self, ViewChild } from '@angular/core';
import { ControlValueAccessor, NgControl } from '@angular/forms';
import { MatAutocompleteSelectedEvent, MatAutocompleteTrigger } from '@angular/material/autocomplete';
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
   @Input() public placeholder: string = '';
   @Input() public disabled: boolean = false;
   @Input() public delay: number = 500;
   @Input() public value: RelatedData<any>;
   @Input() public minSize: number = 0;
   @Input() public textSuffix: string = '';

   /* INPUT VALUE */
   public inputValue: string;
   private inputValueChanged: Observable<string>;
   private OnInputValueChanging(option: any) {
      const value = this.OnDisplayWith(option);
      this.writeValue(null);
      if (value.length < this.minSize) { return; }
      this.optionsChanging.emit(value);
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

   /* FOCUS */
   public OnFocus() {
      this.OnInputValueChanging(this.inputValue);
      this.autoComplete.openPanel();
   }
   public OnBlur() {
      this.onTouched();
   }

   /* VALUE ACCESSOR */
   writeValue(val: RelatedData<any>): void {
      if (val) { this.inputValue = val.description; }
      this.value = val
      this.onChange(val);
   }

   onChange = (val: RelatedData<any>) => { };
   registerOnChange(fn: any): void {
      this.onChange = fn;
   }

   onTouched = () => { };
   registerOnTouched(fn: any): void {
      this.onTouched = fn;
   }
   setDisabledState?(isDisabled: boolean): void {
      // throw new Error('Method not implemented.');
   }

   /* DESTROY */
   ngOnDestroy(): void {
      this.inputValueChanged = null;
      this.optionsChanging.complete();
      this.optionsChanging = null;
   }

}
