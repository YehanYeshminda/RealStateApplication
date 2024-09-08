import { TestBed } from '@angular/core/testing';

import { IoureturnService } from './ioureturn.service';

describe('IoureturnService', () => {
  let service: IoureturnService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(IoureturnService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
