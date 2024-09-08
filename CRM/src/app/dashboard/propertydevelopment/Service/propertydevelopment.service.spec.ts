import { TestBed } from '@angular/core/testing';

import { PropertydevelopmentService } from './propertydevelopment.service';

describe('PropertydevelopmentService', () => {
  let service: PropertydevelopmentService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PropertydevelopmentService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
