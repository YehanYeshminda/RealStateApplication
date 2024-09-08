import { TestBed } from '@angular/core/testing';

import { LeadsHttpService } from './leads-http.service';

describe('LeadsHttpService', () => {
  let service: LeadsHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LeadsHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
