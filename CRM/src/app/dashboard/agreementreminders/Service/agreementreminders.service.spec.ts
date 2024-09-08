import { TestBed } from '@angular/core/testing';

import { AgreementremindersService } from './agreementreminders.service';

describe('AgreementremindersService', () => {
  let service: AgreementremindersService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AgreementremindersService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
