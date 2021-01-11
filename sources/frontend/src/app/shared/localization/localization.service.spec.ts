import { TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { LocalizationService } from './localization.service';

describe('LocalizationService', () => {
   let service: LocalizationService;

   beforeEach(() => {
      TestBed.configureTestingModule({
         imports: [TestsModule]
      });
      service = TestBed.inject(LocalizationService);
   });

   it('should be created', () => {
      expect(service).toBeTruthy();
   });

});
