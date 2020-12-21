import { TestBed } from '@angular/core/testing';

import { ElesseIdentityService } from './elesse-identity.service';

describe('ElesseIdentityService', () => {
  let service: ElesseIdentityService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ElesseIdentityService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
