export class AnalyticsColors {

   /* COLORS */
   /* https://vis4.net/palettes */
   private get Colors(): string[] {
      return ['#1a237e', '#4b4c8f', '#71789d', '#96a5a8', '#c0d3ab', '#ffc988', '#f7937e', '#e0616d', '#bf3057', '#93003a'];
   }

   /* CATEGORIES */
   private CategoriesColor: { [categoryID: number]: number } = {}
   private LastCategoryColorIndex: number = -1;
   public GetCategoryColor(categoryID: number): string {

      // TRY TO LOCATE A COLOR INDEX FOR THE CATEGORY ON THE DICTIONARY
      let colorIndex = this.CategoriesColor[categoryID];

      // HAVENT FOUND
      if (!colorIndex) {

         // ADD ONE TO THE  LAST COLOR INDEX GIVEN, AND KEEP IT INSIDE ARRAY BOUNDS
         this.LastCategoryColorIndex++;
         if (this.LastCategoryColorIndex >= this.Colors.length) {
            this.LastCategoryColorIndex = 0;
         }

         // STORE THE GIVEN COLOR INDEX ON THE DICTIONARY FOR FUTURE
         colorIndex = this.LastCategoryColorIndex;
         this.CategoriesColor[categoryID] = colorIndex;

      }

      // RETURN THE COLOR ON THE COLORS ARRAY
      return this.Colors[colorIndex];
   }

}
