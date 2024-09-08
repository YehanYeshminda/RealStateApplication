import { TestBed } from '@angular/core/testing';

import { CreditpaymentService } from './creditpayment.service';

describe('CreditpaymentService', () => {
  let service: CreditpaymentService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CreditpaymentService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
