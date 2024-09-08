import { TestBed } from '@angular/core/testing';

import { PaymentscheduleService } from './paymentschedule.service';

describe('PaymentscheduleService', () => {
  let service: PaymentscheduleService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PaymentscheduleService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
