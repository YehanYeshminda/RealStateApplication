import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CallSegregationComponent } from './call-segregation.component';

describe('CallSegregationComponent', () => {
  let component: CallSegregationComponent;
  let fixture: ComponentFixture<CallSegregationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CallSegregationComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CallSegregationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
