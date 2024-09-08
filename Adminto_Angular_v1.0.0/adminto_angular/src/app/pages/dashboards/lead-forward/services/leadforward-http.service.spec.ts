import { TestBed } from '@angular/core/testing';

import { LeadforwardHttpService } from './leadforward-http.service';

describe('LeadforwardHttpService', () => {
  let service: LeadforwardHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LeadforwardHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
