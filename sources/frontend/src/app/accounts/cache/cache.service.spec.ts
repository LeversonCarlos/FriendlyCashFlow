import { TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { AccountsCache } from './cache.service';

describe('AccountsCache', () => {
   let service: AccountsCache;

   beforeEach(() => {
      TestBed.configureTestingModule({
         imports: [TestsModule]
      });
      service = TestBed.inject(AccountsCache);
   });

   it('should be created', () => {
      expect(service).toBeTruthy();
   });

});
