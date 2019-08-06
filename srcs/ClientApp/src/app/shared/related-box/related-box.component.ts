import { Component, OnInit, OnDestroy, Optional, Self, Input, ElementRef } from '@angular/core';
import { ControlValueAccessor, NgControl } from '@angular/forms';
import { fromEvent, Observable } from 'rxjs';
import { map, debounceTime, distinctUntilChanged } from 'rxjs/operators';

@Component({
   selector: 'related-box',
   templateUrl: './related-box.component.html',
   styleUrls: ['./related-box.component.scss']
})
export class RelatedBoxComponent implements OnInit, OnDestroy, ControlValueAccessor {

   constructor(@Optional() @Self() private ngControl: NgControl, private elRef: ElementRef<HTMLElement>) {
      if (this.ngControl != null) {
         this.ngControl.valueAccessor = this;
      }
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
   @Input() public placeholder: string = '';
   @Input() public disabled: boolean = false;
   @Input() public delay: number = 500;
   @Input() public value: string;
   public inputValue: string
   private eventStream: Observable<string>;

   /* VALUE ACCESSOR  */
   writeValue(val: string): void {
      this.value = val
      this.onChange(val);
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

   public ngOnDestroy(): void {
      this.eventStream = null;
   }

}
