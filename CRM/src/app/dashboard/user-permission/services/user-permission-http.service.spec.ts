import { TestBed } from '@angular/core/testing';

import { UserPermissionHttpService } from './user-permission-http.service';

describe('UserPermissionHttpService', () => {
  let service: UserPermissionHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UserPermissionHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
