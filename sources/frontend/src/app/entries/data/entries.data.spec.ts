import { TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { EntriesData } from './entries.data';

describe('EntriesData', () => {
   let service: EntriesData;

   beforeEach(() => {
      TestBed.configureTestingModule({
         imports: [TestsModule]
      });
      service = TestBed.inject(EntriesData);
   });

   it('should be created', () => {
      expect(service).toBeTruthy();
   });

});
