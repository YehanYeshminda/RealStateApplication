import { TestBed } from '@angular/core/testing';

import { StaffHttpService } from './staff-http.service';

describe('StaffHttpService', () => {
  let service: StaffHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(StaffHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
