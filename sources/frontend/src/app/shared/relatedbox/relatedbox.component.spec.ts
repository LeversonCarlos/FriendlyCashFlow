import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RelatedboxComponent } from './relatedbox.component';

describe('RelatedboxComponent', () => {
  let component: RelatedboxComponent;
  let fixture: ComponentFixture<RelatedboxComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RelatedboxComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RelatedboxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
