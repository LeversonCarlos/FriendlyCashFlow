import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AnonymousContainerComponent } from './anonymous-container.component';

describe('AnonymousContainerComponent', () => {
  let component: AnonymousContainerComponent;
  let fixture: ComponentFixture<AnonymousContainerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AnonymousContainerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AnonymousContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
