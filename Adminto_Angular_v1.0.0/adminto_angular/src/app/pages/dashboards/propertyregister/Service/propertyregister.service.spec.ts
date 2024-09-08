import { TestBed } from '@angular/core/testing';

import { PropertyregisterService } from './propertyregister.service';

describe('PropertyregisterService', () => {
  let service: PropertyregisterService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PropertyregisterService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
