import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ElesseSharedComponent } from './elesse-shared.component';

describe('ElesseSharedComponent', () => {
  let component: ElesseSharedComponent;
  let fixture: ComponentFixture<ElesseSharedComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ElesseSharedComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ElesseSharedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
