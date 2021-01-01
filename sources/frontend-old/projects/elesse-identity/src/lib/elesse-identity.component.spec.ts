import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ElesseIdentityComponent } from './elesse-identity.component';

describe('ElesseIdentityComponent', () => {
   let component: ElesseIdentityComponent;
   let fixture: ComponentFixture<ElesseIdentityComponent>;

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         declarations: [ElesseIdentityComponent]
      })
         .compileComponents();
   });

   beforeEach(() => {
      fixture = TestBed.createComponent(ElesseIdentityComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
   });

   it('should create', () => {
      expect(component).toBeTruthy();
   });
});
