import { TransferEntity } from './transfers.model';

describe('TransferEntity', () => {

   it('should create an instance', () => {
      expect(new TransferEntity()).toBeTruthy();
   });

   it('parser factory should create instance', () => {
      const param = {
         TransferID: 'TransferID',
         ExpenseAccountID: 'ExpenseAccountID',
         IncomeAccountID: 'IncomeAccountID',
         Date: '2021-02-16',
         Value: 12.34
      };
      const entity = TransferEntity.Parse(param);
      expect(entity.TransferID).toEqual(param.TransferID);
      expect(entity.ExpenseAccountID).toEqual(param.ExpenseAccountID);
      expect(entity.IncomeAccountID).toEqual(param.IncomeAccountID);
      expect(entity.Date).toEqual(new Date(param.Date));
      expect(entity.Value).toEqual(param.Value);
   });

   it('parser factory with empty param should create empty instance', () => {
      const param = {};
      const entity = TransferEntity.Parse(param);
      expect(entity.TransferID).toEqual(null);
      expect(entity.ExpenseAccountID).toEqual(null);
      expect(entity.IncomeAccountID).toEqual(null);
      expect(entity.Date).toEqual(null);
      expect(entity.Value).toEqual(null);
   });

});
