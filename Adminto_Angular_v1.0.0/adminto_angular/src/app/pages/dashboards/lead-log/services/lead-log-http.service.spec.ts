import { TestBed } from '@angular/core/testing';

import { LeadLogHttpService } from './lead-log-http.service';

describe('LeadLogHttpService', () => {
  let service: LeadLogHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LeadLogHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
