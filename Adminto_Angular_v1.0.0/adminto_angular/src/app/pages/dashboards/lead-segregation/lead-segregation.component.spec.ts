import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LeadSegregationComponent } from './lead-segregation.component';

describe('LeadSegregationComponent', () => {
  let component: LeadSegregationComponent;
  let fixture: ComponentFixture<LeadSegregationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LeadSegregationComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LeadSegregationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
