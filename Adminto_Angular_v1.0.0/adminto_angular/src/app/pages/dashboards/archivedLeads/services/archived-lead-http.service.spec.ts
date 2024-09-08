import { TestBed } from '@angular/core/testing';

import { ArchivedLeadHttpService } from './archived-lead-http.service';

describe('ArchivedLeadHttpService', () => {
  let service: ArchivedLeadHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ArchivedLeadHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
