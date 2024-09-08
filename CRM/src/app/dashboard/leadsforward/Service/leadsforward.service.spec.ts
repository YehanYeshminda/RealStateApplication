import { TestBed } from '@angular/core/testing';

import { LeadsforwardService } from './leadsforward.service';

describe('LeadsforwardService', () => {
  let service: LeadsforwardService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LeadsforwardService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
