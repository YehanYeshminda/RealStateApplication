import { TestBed } from '@angular/core/testing';

import { UpdateCountHttpService } from './update-count-http.service';

describe('UpdateCountHttpService', () => {
  let service: UpdateCountHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UpdateCountHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
