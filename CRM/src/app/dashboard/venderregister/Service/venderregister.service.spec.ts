import { TestBed } from '@angular/core/testing';

import { VenderregisterService } from './venderregister.service';

describe('VenderregisterService', () => {
  let service: VenderregisterService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(VenderregisterService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
