export class AnalyticsColors {

   public Restart() { this.LastCategoryColorIndex = -1; }

   /* COLORS */
   /* https://vis4.net/palettes */
   private get Colors(): string[] {
      return ['#2196F3', '#795548', '#FF5722', '#FFC107', '#4CAF50', '#3F51B5', '#f44336', '#FFEB3B',
         '#00BCD4', '#607D8B', '#8BC34A', '#9C27B0', '#3F51B5', '#FF9800', '#009688'];
   }

   /* CATEGORIES */
   private CategoriesColor: { [categoryID: number]: number } = {}
   private LastCategoryColorIndex: number = -1;
   public GetCategoryColor(categoryID: number): string {

      // TRY TO LOCATE A COLOR INDEX FOR THE CATEGORY ON THE DICTIONARY
      let colorIndex = this.CategoriesColor[categoryID];

      // HAVENT FOUND
      if (colorIndex == undefined) {

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
