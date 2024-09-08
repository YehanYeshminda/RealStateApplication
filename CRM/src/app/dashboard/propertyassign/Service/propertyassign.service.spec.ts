import { TestBed } from '@angular/core/testing';

import { PropertyassignService } from './propertyassign.service';

describe('PropertyassignService', () => {
  let service: PropertyassignService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PropertyassignService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
