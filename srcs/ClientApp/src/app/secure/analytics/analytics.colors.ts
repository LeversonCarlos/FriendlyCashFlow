export class AnalyticsColors {

   public Restart() { this.LastCategoryColorIndex = -1; }

   /* COLORS */
   /* https://vis4.net/palettes */
   private get Colors(): string[] {
      return ['#73a8f0', '#318047', '#f5c533', '#004789', '#f18302', '#8f5699', '#894a00', '#3ed662',
         '#808080', '#50ebe8', '#e44d98', '#adff2f', '#ffa07a'];
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

   public GetForecolorSchemeSensitive(): string {
      let color = '#333333';
      if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
         color = '#dddddd'
      }
      return color;
   }

}
