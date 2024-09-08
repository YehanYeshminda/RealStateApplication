import { TestBed } from '@angular/core/testing';

import { AdvancepaymentService } from './advancepayment.service';

describe('AdvancepaymentService', () => {
  let service: AdvancepaymentService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AdvancepaymentService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
