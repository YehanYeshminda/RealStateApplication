import { TestBed } from '@angular/core/testing';

import { MeetingupdateService } from './meetingupdate.service';

describe('MeetingupdateService', () => {
  let service: MeetingupdateService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MeetingupdateService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
