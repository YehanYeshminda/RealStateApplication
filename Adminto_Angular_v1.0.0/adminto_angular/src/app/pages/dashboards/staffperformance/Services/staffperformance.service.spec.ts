import { TestBed } from '@angular/core/testing';

import { StaffperformanceService } from './staffperformance.service';

describe('StaffperformanceService', () => {
  let service: StaffperformanceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(StaffperformanceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
