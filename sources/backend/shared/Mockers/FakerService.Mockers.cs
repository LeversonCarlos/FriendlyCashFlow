namespace Elesse.Shared
{
   public class Faker
   {

      static Bogus.Faker _Faker;
      public static Bogus.Faker GetFaker()
      {
         if (_Faker == null)
            _Faker = new Bogus.Faker("en_US");
         return _Faker;
      }

   }
}
