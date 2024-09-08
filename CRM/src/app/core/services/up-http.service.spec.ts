import { TestBed } from '@angular/core/testing';

import { UpHttpService } from './up-http.service';

describe('UpHttpService', () => {
  let service: UpHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UpHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
