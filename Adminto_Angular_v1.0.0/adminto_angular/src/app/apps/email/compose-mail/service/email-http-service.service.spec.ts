import { TestBed } from '@angular/core/testing';

import { EmailHttpServiceService } from './email-http-service.service';

describe('EmailHttpServiceService', () => {
  let service: EmailHttpServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EmailHttpServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
