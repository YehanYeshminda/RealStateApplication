import { TestBed } from '@angular/core/testing';

import { CampaindetailsService } from './campaindetails.service';

describe('CampaindetailsService', () => {
  let service: CampaindetailsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CampaindetailsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
