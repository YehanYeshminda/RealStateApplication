import { TestBed } from '@angular/core/testing';

import { VendertoserviceService } from './vendertoservice.service';

describe('VendertoserviceService', () => {
  let service: VendertoserviceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(VendertoserviceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
