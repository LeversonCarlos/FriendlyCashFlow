#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
#endregion

namespace FriendCash.Service.Analysis
{
   partial class AnalysyController
   {

      #region GetData_Accounts
      private List<Model.viewDimension<long>> GetData_Accounts(List<Model.viewEntry> dailyData, List<Model.viewEntry> monthlyData)
      {
         try
         {
            using (var dataController = new Accounts.AccountController())
            {
               var dataIDs = monthlyData.GroupBy(x => x.idAccount).Select(x => x.Key).ToList();

               var dataList = dataController.QueryView()
                 .Where(x => dataIDs.Contains(x.idAccount))
                 .OrderBy(x => x.Text)
                 .ToList();
               if (dataIDs.Contains(0)) { dataList.Insert(0, new Accounts.Model.viewAccount() { idAccount = 0, Text = "N/A" }); }

               var dataView = dataList
                 .Select(x => new Model.viewDimension<long>()
                 {
                    keyID = x.idAccount,
                    Text = x.Text
                 })
                 .ToList();

               var dataID = 0;
               dataView.ForEach(x => {
                  x.ID = dataID;

                  dailyData
                     .Where(d => d.idAccount == x.keyID).ToList()
                     .ForEach(d => d.idAccount = x.ID);

                  monthlyData
                     .Where(d => d.idAccount == x.keyID).ToList()
                     .ForEach(d => d.idAccount = x.ID);

                  dataID++;
               });

               return dataView;
            }
         }
         catch (Exception ex) { throw ex; }
      }
      #endregion

      #region GetData_Categories
      private List<Model.viewDimension<long>> GetData_Categories(List<Model.viewEntry> dailyData, List<Model.viewEntry> monthlyData)
      {
         try
         {
            using (var dataController = new Categories.CategoryController())
            {
               var dataIDs = monthlyData
                  .GroupBy(x => x.idCategory)
                  .Select(x => x.Key)
                  .ToList();

               var dataList = dataController.QueryView()
                 .Where(x => dataIDs.Contains(x.idCategory))
                 .OrderBy(x => x.HierarchyText)
                 .ToList();
               if (dataIDs.Contains(0)) { dataList.Insert(0, new Categories.Model.viewCategory() { idCategory = 0, Text = "N/A", HierarchyText = "N/A", Type = Categories.Model.enCategoryType.Expense }); }

               var dataView = dataList
                 .Select(x => new Model.viewDimension<long>()
                 {
                    keyID = x.idCategory,
                    Type = (short)x.Type,
                    Text = x.HierarchyText
                 })
                 .ToList();

               var dataID = 0;
               dataView.ForEach(x =>
               {
                  x.ID = dataID;

                  var parentText = x.Text.Split(new string[] { " \\ " }, StringSplitOptions.RemoveEmptyEntries);
                  if (parentText != null && parentText.Length >= 2)
                  {
                     x.ParentText = parentText[0];
                  }
                  else { x.ParentText = x.Text; }

                  dailyData
                     .Where(d => d.idCategory == x.keyID).ToList()
                     .ForEach(d => d.idCategory = x.ID);

                  monthlyData
                     .Where(d => d.idCategory == x.keyID).ToList()
                     .ForEach(d => d.idCategory = x.ID);

                  dataID++;
               });

               // UNFOUND CATEGORIES
               var categoryList = dataView.Select(x => x.ID).ToList();
               dailyData.Where(x => !categoryList.Contains(x.idCategory)).ToList().ForEach(x => x.idCategory = -1);
               monthlyData.Where(x => !categoryList.Contains(x.idCategory)).ToList().ForEach(x => x.idCategory = -1);

               return dataView;
            }
         }
         catch (Exception ex) { throw ex; }
      }
      #endregion

      #region GetData_Patterns
      private List<Model.viewDimension<long>> GetData_Patterns(List<Model.viewEntry> dailyData, List<Model.viewEntry> monthlyData)
      {
         try
         {
            using (var dataController = new Entries.PatternController())
            {
               var dataIDs = monthlyData.GroupBy(x => x.idPattern).Select(x => x.Key).ToList();

               var dataList = dataController.QueryData()
                 .Where(x => dataIDs.Contains(x.idPattern))
                 .OrderBy(x => x.Text)
                 .ToList();
               if (dataIDs.Contains(0)) { dataList.Insert(0, new Entries.Model.bindPattern() { idPattern = 0, Text = "N/A" }); }

               var dataView = dataList
                 .Select(x => new Model.viewDimension<long>()
                 {
                    keyID = x.idPattern,
                    Text = x.Text
                 })
                 .ToList();

               var dataID = 0;
               dataView.ForEach(x => {
                  x.ID = dataID;

                  dailyData
                     .Where(d => d.idPattern == x.keyID).ToList()
                     .ForEach(d => d.idPattern = x.ID);

                  monthlyData
                     .Where(d => d.idPattern == x.keyID).ToList()
                     .ForEach(d => d.idPattern = x.ID);

                  dataID++;
               });

               return dataView;
            }
         }
         catch (Exception ex) { throw ex; }
      }
      #endregion

      #region GetData_Planned
      private List<Model.viewDimension<short>> GetData_Planned()
      {
         try
         {
            var dataList = new Dictionary<short, string>();
            dataList.Add(0, this.GetTranslation("LABEL_ANALYSIS_DIMENSION_UNPLANNED"));
            dataList.Add(1, this.GetTranslation("LABEL_ANALYSIS_DIMENSION_PLANNED"));

            var dataView = dataList
               .Select(x => new Model.viewDimension<short>()
               {
                  ID = x.Key,
                  Text = x.Value
               })
               .ToList();
            return dataView;
         }
         catch (Exception ex) { throw ex; }
      }
      #endregion

      #region GetData_Paid
      private List<Model.viewDimension<short>> GetData_Paid()
      {
         try
         {
            var dataList = new Dictionary<short, string>();
            dataList.Add(0, this.GetTranslation("LABEL_ANALYSIS_DIMENSION_UNPAID"));
            dataList.Add(1, this.GetTranslation("LABEL_ANALYSIS_DIMENSION_PAID"));

            var dataView = dataList
               .Select(x => new Model.viewDimension<short>()
               {
                  ID = x.Key,
                  Text = x.Value
               })
               .ToList();
            return dataView;
         }
         catch (Exception ex) { throw ex; }
      }
      #endregion

      #region GetData_Type
      private List<Model.viewDimension<short>> GetData_Type()
      {
         try
         {
            var dataList = new Dictionary<short, string>();
            dataList.Add((short)Categories.Model.enCategoryType.Expense, this.GetTranslation("ENUM_CATEGORYTYPE_EXPENSE"));
            dataList.Add((short)Categories.Model.enCategoryType.Income, this.GetTranslation("ENUM_CATEGORYTYPE_INCOME"));

            var dataView = dataList
               .Select(x => new Model.viewDimension<short>()
               {
                  keyID = x.Key,
                  ID = (short)(x.Key - 1),
                  Text = x.Value
               })
               .ToList();
            return dataView;
         }
         catch (Exception ex) { throw ex; }
      }
      #endregion

   }
}
