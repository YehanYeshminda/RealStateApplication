import { TestBed } from '@angular/core/testing';

import { NotificationNewHttpService } from './notification-new-http.service';

describe('NotificationNewHttpService', () => {
  let service: NotificationNewHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(NotificationNewHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
