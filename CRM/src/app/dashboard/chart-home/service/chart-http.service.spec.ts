import { TestBed } from '@angular/core/testing';

import { ChartHttpService } from './chart-http.service';

describe('ChartHttpService', () => {
  let service: ChartHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ChartHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
