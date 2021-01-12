import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { BusyComponent } from './busy.component';
import { BusyService } from './busy.service';

describe('BusyComponent', () => {
   let fixture: ComponentFixture<BusyComponent>;
   let component: BusyComponent;
   let busyService: BusyService;

   beforeEach(async () => {
      await TestBed
         .configureTestingModule({
            declarations: [BusyComponent],
            imports: [TestsModule],
            providers: [BusyService]
         })
         .compileComponents();
   });

   beforeEach(() => {
      fixture = TestBed.createComponent(BusyComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
      busyService = TestBed.inject(BusyService);
   });

   it('should create', () => {
      expect(component).toBeTruthy();
   });

   it('initial render must be null', () => {
      fixture.detectChanges();
      const compiled = fixture.nativeElement;
      expect(compiled.querySelector('p')).toBeNull();
   });

   it('after service.show call render must result busy element', () => {
      busyService.hide();
      busyService.show();
      fixture.detectChanges();
      const compiled = fixture.nativeElement;
      expect(compiled.querySelector('mat-progress-bar')).toBeTruthy();
   });

   it('after service.hide call render must be null', () => {
      busyService.show();
      busyService.hide();
      fixture.detectChanges();
      const compiled = fixture.nativeElement;
      expect(compiled.querySelector('mat-progress-bar')).toBeNull();
   });

});
