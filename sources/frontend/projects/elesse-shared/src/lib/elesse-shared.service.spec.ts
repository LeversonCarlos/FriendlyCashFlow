import { TestBed } from '@angular/core/testing';

import { ElesseSharedService } from './elesse-shared.service';

describe('ElesseSharedService', () => {
  let service: ElesseSharedService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ElesseSharedService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
