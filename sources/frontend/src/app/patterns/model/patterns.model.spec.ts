import { enCategoryType } from '@elesse/categories';
import { PatternEntity } from './patterns.model';

describe('PatternEntity', () => {

   it('should create an instance', () => {
      expect(new PatternEntity()).toBeTruthy();
   });

   it('should parse to correct an instance', () => {
      const patternID = "my pattern id";
      const type = enCategoryType.Income;
      const categoryID = "my category id";
      const text = "my pattern text";

      const pattern = PatternEntity.Parse({
         PatternID: patternID,
         Type: type,
         CategoryID: categoryID,
         Text: text
      });

      expect(pattern.PatternID).toEqual(patternID);
      expect(pattern.Type).toEqual(type);
      expect(pattern.CategoryID).toEqual(categoryID);
      expect(pattern.Text).toEqual(text);
   });

});
