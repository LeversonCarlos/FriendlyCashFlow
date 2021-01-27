namespace Elesse.Patterns
{
   partial class PatternEntity
   {
      internal static Tests.PatternEntityBuilder Builder() => new Tests.PatternEntityBuilder();
   }
}
namespace Elesse.Patterns.Tests
{
   internal class PatternEntityBuilder
   {

      Shared.EntityID _PatternID = Shared.EntityID.MockerID();
      public PatternEntityBuilder WithPatternID(Shared.EntityID patternID)
      {
         _PatternID = patternID;
         return this;
      }

      enPatternType _Type = Shared.Faker.GetFaker().PickRandom<enPatternType>();
      public PatternEntityBuilder WithType(enPatternType type)
      {
         _Type = type;
         return this;
      }

      Shared.EntityID _CategoryID = Shared.EntityID.MockerID();
      public PatternEntityBuilder WithCategoryID(Shared.EntityID categoryID)
      {
         _CategoryID = categoryID;
         return this;
      }

      string _Text = Shared.Faker.GetFaker().Commerce.ProductName();
      public PatternEntityBuilder WithText(string text)
      {
         _Text = text;
         return this;
      }

      public PatternEntity Build() =>
         PatternEntity.Restore(_PatternID, _Type, _CategoryID, _Text);

   }
}
