using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Elesse.Categories
{

   internal class CategoryEntity : ICategoryEntity
   {

      public CategoryEntity(string text, enCategoryType type, Shared.EntityID parentID)
         : this(Shared.EntityID.NewID(), text, type, parentID)
      {
         RowStatus = true;
      }

      public CategoryEntity(Shared.EntityID categoryID, string text, enCategoryType type, Shared.EntityID parentID)
      {
         CategoryID = categoryID;
         Text = text;
         Type = type;
         ParentID = parentID;
      }

      Shared.EntityID _CategoryID;
      [BsonId]
      public Shared.EntityID CategoryID
      {
         get => _CategoryID;
         private set
         {
            if (value == null)
               throw new ArgumentException(WARNINGS.INVALID_CATEGORYID);
            _CategoryID = value;
         }
      }

      string _Text;
      public string Text
      {
         get => _Text;
         internal set
         {
            if (string.IsNullOrWhiteSpace(value) || value.Length > 100)
               throw new ArgumentException(WARNINGS.INVALID_TEXT);
            _Text = value;
         }
      }

      enCategoryType _Type;
      public enCategoryType Type
      {
         get => _Type;
         internal set
         {
            _Type = value;
         }
      }

      Shared.EntityID _ParentID;
      public Shared.EntityID ParentID
      {
         get => _ParentID;
         private set
         {
            _ParentID = value;
         }
      }

      public bool RowStatus { get; set; }
      public string HierarchyText { get; set; }

   }

   partial struct WARNINGS
   {
      internal const string INVALID_CATEGORYID = "INVALID_CATEGORYID_PARAMETER";
      internal const string INVALID_TEXT = "INVALID_TEXT_PARAMETER";
   }

}
