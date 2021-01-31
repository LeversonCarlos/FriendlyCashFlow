import { TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { DataService } from './data.service';

describe('DataService', () => {
   let service: DataService;

   beforeEach(() => {
      TestBed.configureTestingModule({
         imports: [TestsModule]
      });
      service = TestBed.inject(DataService);
   });

   it('should be created', () => {
      expect(service).toBeTruthy();
   });

});
