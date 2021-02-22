export class Month {

   constructor(private date: Date) { }

   public get Date(): Date { return this.date; }
   public get Year(): number { return this.date.getFullYear(); }
   public get Month(): number { return this.date.getMonth() + 1; }

   public ToCode(): string {
      const iSOString = this.date.toISOString();
      return `${iSOString.substring(0, 4)}${iSOString.substring(5, 7)}`;
   }

   public static GetToday(): Month {
      const today = new Date();
      return new Month(new Date(today.getFullYear(), today.getMonth(), 1, 12));
   }

}
