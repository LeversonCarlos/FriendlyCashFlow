import { Component, OnInit, OnDestroy, Input, Optional, Self, ElementRef, HostBinding } from '@angular/core';
import { ControlValueAccessor, NgControl } from '@angular/forms';
import { fromEvent, Observable, Subject } from 'rxjs';
import { map, debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { MatFormFieldControl } from '@angular/material/form-field';
import { FocusMonitor } from '@angular/cdk/a11y';
import { coerceBooleanProperty } from '@angular/cdk/coercion';

@Component({
   selector: 'related-box',
   templateUrl: './related-box.component.html',
   styleUrls: ['./related-box.component.scss'],
   providers: [{ provide: MatFormFieldControl, useExisting: RelatedBoxComponent }]
})
export class RelatedBoxComponent implements OnInit, OnDestroy, ControlValueAccessor, MatFormFieldControl<string> {

   constructor(@Optional() @Self() public ngControl: NgControl, private elRef: ElementRef<HTMLElement>, private fm: FocusMonitor) {
      if (this.ngControl != null) {
         this.ngControl.valueAccessor = this;
      }
      this.fm.monitor(elRef.nativeElement, true).subscribe(origin => {
         this.focused = !!origin;
         this.stateChanges.next();
      });
   }

   public ngOnInit() {
      this.inputValue = this.value;
      this.eventStream = fromEvent(this.elRef.nativeElement, 'keyup')
         .pipe(
            map(() => this.inputValue),
            debounceTime(this.delay),
            distinctUntilChanged()
         );
      this.eventStream.subscribe(val => this.writeValue(val))
   }

   /* PROPERTIES */
   @Input() public delay: number = 500;
   @Input() public value: string;
   public inputValue: string
   private eventStream: Observable<string>;

   /* VALUE ACCESSOR  */
   writeValue(val: string): void {
      this.value = val
      this.onChange(val);
      this.stateChanges.next();
   }
   onChange = (val: string) => { };
   registerOnChange(fn: any): void {
      this.onChange = fn;
   }
   registerOnTouched(fn: any): void {
      // throw new Error("Method not implemented.");
   }
   setDisabledState?(isDisabled: boolean): void {
      // throw new Error("Method not implemented.");
   }

   /* STATE CHANGES */
   stateChanges = new Subject<void>();

   /* ID */
   private static nextID = 0;
   @HostBinding() id: string = `related-box-${RelatedBoxComponent.nextID++}`;

   /* PLACEHOLDER */
   @Input()
   public get placeholder() {
      return this._placeholder;
   }
   public set placeholder(val: string) {
      this._placeholder = val;
      this.stateChanges.next();
   }
   private _placeholder: string;

   /* FOCUSED */
   focused: boolean = false;

   /* EMPTY */
   public get empty(): boolean {
      return !this.value;
   }

   /* SHOULD LABEL FLOAT */
   public get shouldLabelFloat(): boolean {
      return this.focused || !this.empty;
   }

   /* REQUIRED */
   @Input()
   get required() {
      return this._required;
   }
   set required(req) {
      this._required = coerceBooleanProperty(req);
      this.stateChanges.next();
   }
   private _required = false;

   /* DISABLED */
   @Input()
   get disabled(): boolean { return this._disabled; }
   set disabled(value: boolean) {
      this._disabled = coerceBooleanProperty(value);
      this.stateChanges.next();
   }
   private _disabled = false;

   /* ERROR STATE */
   public get errorState(): boolean {
      return !this.ngControl.valid;
   }

   controlType?: string;
   autofilled?: boolean;
   setDescribedByIds(ids: string[]): void {
      throw new Error("Method not implemented.");
   }
   onContainerClick(event: MouseEvent): void {
      throw new Error("Method not implemented.");
   }

   // DESTROY
   public ngOnDestroy(): void {
      this.eventStream = null;
      this.stateChanges.complete();
      this.stateChanges = null;
      this.fm.stopMonitoring(this.elRef.nativeElement);
   }

}
