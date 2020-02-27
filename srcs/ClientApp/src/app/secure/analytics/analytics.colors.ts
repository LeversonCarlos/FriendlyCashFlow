export class AnalyticsColors {

   /* COLORS */
   /* https://vis4.net/palettes */
   private get Colors(): string[] {
      return ['#2196F3', '#4CAF50', '#f44336', '#FFC107', '#3F51B5', '#795548', '#009688', '#FF5722',
         '#FFEB3B', '#3F51B5', '#607D8B', '#8BC34A', '#9C27B0', '#00BCD4', '#FF9800'];
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
