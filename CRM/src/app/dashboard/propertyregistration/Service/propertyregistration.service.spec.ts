import { TestBed } from '@angular/core/testing';

import { PropertyregistrationService } from './propertyregistration.service';

describe('PropertyregistrationService', () => {
  let service: PropertyregistrationService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PropertyregistrationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
