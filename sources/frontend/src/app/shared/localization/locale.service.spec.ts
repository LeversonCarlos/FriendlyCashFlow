import { TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { LocaleService } from './locale.service';

describe('LocaleService', () => {
   let service: LocaleService;

   beforeEach(() => {
      TestBed.configureTestingModule({
         imports: [TestsModule]
      });
      service = TestBed.inject(LocaleService);
   });

   it('should be created', () => {
      expect(service).toBeTruthy();
   });

});
