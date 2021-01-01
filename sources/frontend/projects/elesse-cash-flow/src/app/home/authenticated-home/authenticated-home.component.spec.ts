import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuthenticatedHomeComponent } from './authenticated-home.component';

describe('AuthenticatedHomeComponent', () => {
  let component: AuthenticatedHomeComponent;
  let fixture: ComponentFixture<AuthenticatedHomeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AuthenticatedHomeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AuthenticatedHomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
