import { enPatternType, EntryEntity, PatternEntity } from './entries.data';

describe('EntryEntity', () => {

   it('should create an instance', () => {
      expect(new EntryEntity()).toBeTruthy();
   });

   it('should parse anonymous type and materialize properties', () => {
      const EntryID = "EntryID";
      const Pattern = PatternEntityMocker.Create().Build();

      const entry = EntryEntity.Parse({ EntryID, Pattern });

      expect(entry.EntryID).toEqual(EntryID);
   });

});

export class PatternEntityMocker {
   public static Create(): PatternEntityMocker { return new PatternEntityMocker(); }

   private _PatternEntity: PatternEntity = PatternEntity.Parse({
      PatternID: 'PatternID',
      Type: enPatternType.Income,
      CategoryID: 'CategoryID',
      Text: 'Some Pattern Text'
   });

   public WithPatternID(patternID: string): PatternEntityMocker {
      this._PatternEntity.PatternID = patternID;
      return this;
   }

   public WithType(type: enPatternType): PatternEntityMocker {
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
