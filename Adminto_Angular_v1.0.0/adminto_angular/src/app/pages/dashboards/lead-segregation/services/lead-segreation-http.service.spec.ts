import { TestBed } from '@angular/core/testing';

import { LeadSegreationHttpService } from './lead-segreation-http.service';

describe('LeadSegreationHttpService', () => {
  let service: LeadSegreationHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LeadSegreationHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
