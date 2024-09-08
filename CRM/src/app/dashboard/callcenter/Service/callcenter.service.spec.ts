import { TestBed } from '@angular/core/testing';

import { CallcenterService } from './callcenter.service';

describe('CallcenterService', () => {
  let service: CallcenterService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CallcenterService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
