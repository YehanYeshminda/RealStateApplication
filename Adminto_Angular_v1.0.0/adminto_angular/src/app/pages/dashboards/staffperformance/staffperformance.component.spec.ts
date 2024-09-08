import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StaffperformanceComponent } from './staffperformance.component';

describe('StaffperformanceComponent', () => {
  let component: StaffperformanceComponent;
  let fixture: ComponentFixture<StaffperformanceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StaffperformanceComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StaffperformanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
