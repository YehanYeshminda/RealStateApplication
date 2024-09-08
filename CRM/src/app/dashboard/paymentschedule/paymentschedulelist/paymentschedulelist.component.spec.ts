import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PaymentschedulelistComponent } from './paymentschedulelist.component';

describe('PaymentschedulelistComponent', () => {
  let component: PaymentschedulelistComponent;
  let fixture: ComponentFixture<PaymentschedulelistComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PaymentschedulelistComponent]
    });
    fixture = TestBed.createComponent(PaymentschedulelistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
