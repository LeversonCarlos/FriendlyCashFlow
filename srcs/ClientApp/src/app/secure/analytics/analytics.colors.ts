export class AnalyticsColors {

   public Restart() {
      this.LastCategoryColorIndex = -1;
      this.LastAccountColorIndex = -1;
   }

   /* COLORS */
   /* https://vis4.net/palettes */
   public get Colors(): string[] {
      return ['#16A085', '#C0392B', '#2980B9', '#8E44AD', '#FA8072', '#825A2C', '#F39C12', '#D35400', '#27AE60', '#7F8C8D'];
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

   /* ACCOUNTS */
   private AccountsColor: { [accountText: string]: number } = {}
   private LastAccountColorIndex: number = -1;
   public GetAccountColor(accountText: string): string {

      // TRY TO LOCATE A COLOR INDEX FOR THE ACCOUNT ON THE DICTIONARY
      let colorIndex = this.AccountsColor[accountText];

      // HAVENT FOUND
      if (colorIndex == undefined) {

         // ADD ONE TO THE  LAST COLOR INDEX GIVEN, AND KEEP IT INSIDE ARRAY BOUNDS
         this.LastAccountColorIndex++;
         if (this.LastAccountColorIndex >= this.Colors.length) {
            this.LastAccountColorIndex = 0;
         }

         // STORE THE GIVEN COLOR INDEX ON THE DICTIONARY FOR FUTURE
         colorIndex = this.LastAccountColorIndex;
         this.AccountsColor[accountText] = colorIndex;

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
