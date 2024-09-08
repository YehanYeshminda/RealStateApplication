import { TestBed } from '@angular/core/testing';

import { CallListHttpService } from './call-list-http.service';

describe('CallListHttpService', () => {
  let service: CallListHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CallListHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
