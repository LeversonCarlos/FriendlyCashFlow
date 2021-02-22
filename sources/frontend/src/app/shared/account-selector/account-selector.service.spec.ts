import { TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { AccountSelectorService } from './account-selector.service';

describe('AccountSelectorService', () => {
   let service: AccountSelectorService;

   beforeEach(() => {
      TestBed.configureTestingModule({
         imports: [TestsModule]
      });
      service = TestBed.inject(AccountSelectorService);
   });

   it('should be created', () => {
      expect(service).toBeTruthy();
   });

});
