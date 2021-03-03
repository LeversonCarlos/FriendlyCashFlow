import { TestBed } from '@angular/core/testing';
import { EntriesData } from './entries.data';

describe('EntriesData', () => {
   let service: EntriesData;

   beforeEach(() => {
      TestBed.configureTestingModule({});
      service = TestBed.inject(EntriesData);
   });

   it('should be created', () => {
      expect(service).toBeTruthy();
   });

});
