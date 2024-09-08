import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PaymentscheduleComponent } from './paymentschedule.component';

describe('PaymentscheduleComponent', () => {
  let component: PaymentscheduleComponent;
  let fixture: ComponentFixture<PaymentscheduleComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PaymentscheduleComponent]
    });
    fixture = TestBed.createComponent(PaymentscheduleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
