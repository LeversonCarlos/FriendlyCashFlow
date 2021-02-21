import { enCategoryType } from '@elesse/categories';
import { PatternEntity } from '@elesse/patterns';
import { EntryEntity } from './entries.model';

describe('EntryEntity', () => {

   it('should create an instance', () => {
      expect(new EntryEntity()).toBeTruthy();
   });

   it('should parse anonymous type and materialize properties', () => {
      const patternID = "my pattern id";
      const type = enCategoryType.Income;
      const categoryID = "my category id";
      const text = "my pattern text";
      const pattern = PatternEntityMocker
         .Create()
         .WithPatternID(patternID)
         .WithType(type)
         .WithCategoryID(categoryID)
         .WithText(text)
         .Build();

      const entryID = "my entry id";
      const accountID = "my account id";
      const dueDate = new Date("2021-01-23");
      const value = 12.34;
      const payDate = new Date("2021-01-24");
      const entry = EntryEntityMocker
         .Create()
         .WithEntryID(entryID)
         .WithPattern(pattern)
         .WithAccountID(accountID)
         .WithDueDate(dueDate)
         .WithValue(value)
         .WithPayDate(payDate)
         .Build();

      expect(entry.EntryID).toEqual(entryID);
      expect(entry.Pattern.PatternID).toEqual(patternID);
      expect(entry.Pattern.Type).toEqual(type);
      expect(entry.Pattern.CategoryID).toEqual(categoryID);
      expect(entry.Pattern.Text).toEqual(text);
      expect(entry.AccountID).toEqual(accountID);
      expect(entry.DueDate).toEqual(dueDate);
      expect(entry.Value).toEqual(value);
      expect(entry.PayDate).toEqual(payDate);
      expect(entry.Paid).toEqual(true);
   });

});

export class EntryEntityMocker {
   public static Create(): EntryEntityMocker { return new EntryEntityMocker(); }

   private _EntryEntity: EntryEntity = EntryEntity.Parse({
      EntryID: 'EntryID',
      Pattern: PatternEntityMocker.Create().Build(),
      AccountID: 'AccountID',
      DueDate: new Date(),
      Value: 1234.56
   });

   public WithEntryID(entryID: string): EntryEntityMocker {
      this._EntryEntity.EntryID = entryID;
      return this;
   }

   public WithPattern(pattern: PatternEntity): EntryEntityMocker {
      this._EntryEntity.Pattern = pattern;
      return this;
   }

   public WithAccountID(accountID: string): EntryEntityMocker {
      this._EntryEntity.AccountID = accountID;
      return this;
   }

   public WithDueDate(dueDate: Date): EntryEntityMocker {
      this._EntryEntity.DueDate = dueDate;
      return this;
   }

   public WithValue(value: number): EntryEntityMocker {
      this._EntryEntity.Value = value;
      return this;
   }

   public WithPayDate(payDate: Date): EntryEntityMocker {
      this._EntryEntity.Paid = true;
      this._EntryEntity.PayDate = payDate;
      return this;
   }

   public Build(): EntryEntity { return this._EntryEntity; }
}

export class PatternEntityMocker {
   public static Create(): PatternEntityMocker { return new PatternEntityMocker(); }

   private _PatternEntity: PatternEntity = PatternEntity.Parse({
      PatternID: 'PatternID',
      Type: enCategoryType.Income,
      CategoryID: 'CategoryID',
      Text: 'Some Pattern Text'
   });

   public WithPatternID(patternID: string): PatternEntityMocker {
      this._PatternEntity.PatternID = patternID;
      return this;
   }

   public WithType(type: enCategoryType): PatternEntityMocker {
      this._PatternEntity.Type = type;
      return this;
   }

   public WithCategoryID(categoryID: string): PatternEntityMocker {
      this._PatternEntity.CategoryID = categoryID;
      return this;
   }

   public WithText(text: string): PatternEntityMocker {
      this._PatternEntity.Text = text;
      return this;
   }

   public Build(): PatternEntity { return this._PatternEntity; }
}
