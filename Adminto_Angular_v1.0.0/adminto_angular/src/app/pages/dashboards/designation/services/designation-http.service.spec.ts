import { TestBed } from '@angular/core/testing';

import { DesignationHttpService } from './designation-http.service';

describe('DesignationHttpService', () => {
  let service: DesignationHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DesignationHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
