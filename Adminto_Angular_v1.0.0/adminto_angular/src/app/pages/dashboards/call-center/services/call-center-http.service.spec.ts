import { TestBed } from '@angular/core/testing';

import { CallCenterHttpService } from './call-center-http.service';

describe('CallCenterHttpService', () => {
  let service: CallCenterHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CallCenterHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
