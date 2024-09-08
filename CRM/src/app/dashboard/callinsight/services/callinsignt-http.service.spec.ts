import { TestBed } from '@angular/core/testing';

import { CallinsigntHttpService } from './callinsignt-http.service';

describe('CallinsigntHttpService', () => {
  let service: CallinsigntHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CallinsigntHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
