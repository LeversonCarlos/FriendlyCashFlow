export class RelatedData<T> {
   value: T
   id: string
   description: string
   badgeText: string

   public static Parse(id: string, description: string, value: any): RelatedData<any> {
      return Object.assign(new RelatedData, {
         id: id,
         description: description,
         value: value
      });
   }
}
