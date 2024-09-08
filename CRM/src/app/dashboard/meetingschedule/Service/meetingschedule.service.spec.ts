import { TestBed } from '@angular/core/testing';

import { MeetingscheduleService } from './meetingschedule.service';

describe('MeetingscheduleService', () => {
  let service: MeetingscheduleService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MeetingscheduleService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
