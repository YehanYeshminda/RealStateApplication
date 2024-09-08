import { TestBed } from '@angular/core/testing';

import { ChartDataHttpService } from './chart-data-http.service';

describe('ChartDataHttpService', () => {
  let service: ChartDataHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ChartDataHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
