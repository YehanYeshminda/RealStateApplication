import { TestBed } from '@angular/core/testing';

import { LeadSegregationHttpService } from './lead-segregation-http.service';

describe('LeadSegregationHttpService', () => {
  let service: LeadSegregationHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LeadSegregationHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
