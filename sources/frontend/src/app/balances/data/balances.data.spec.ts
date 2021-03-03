import { TestBed } from '@angular/core/testing';
import { BalanceData } from './balances.data';

describe('BalanceData', () => {
   let service: BalanceData;

   beforeEach(() => {
      TestBed.configureTestingModule({});
      service = TestBed.inject(BalanceData);
   });

   it('should be created', () => {
      expect(service).toBeTruthy();
   });

});
