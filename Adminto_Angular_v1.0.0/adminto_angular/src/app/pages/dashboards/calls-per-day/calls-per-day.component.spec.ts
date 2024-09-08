import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CallsPerDayComponent } from './calls-per-day.component';

describe('CallsPerDayComponent', () => {
  let component: CallsPerDayComponent;
  let fixture: ComponentFixture<CallsPerDayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CallsPerDayComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CallsPerDayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
