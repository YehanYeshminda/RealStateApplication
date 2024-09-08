import { TestBed } from '@angular/core/testing';

import { EmployeePerformanceHttpService } from './employee-performance-http.service';

describe('EmployeePerformanceHttpService', () => {
  let service: EmployeePerformanceHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EmployeePerformanceHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
