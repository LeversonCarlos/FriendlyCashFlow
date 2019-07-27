#region Using
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace FriendCash.Service.Imports
{

   internal class Data
   {
      public enum enDataType : short { Transfer = 0, Expense = 1, Income = 2 }

      #region Properties
      public enDataType Type { get; set; }
      public string Date { get; set; }
      public string Paid { get; set; }
      public decimal? Value{ get; set; }
      public string Description { get; set; }
      public string Category { get; set; }
      public string Account { get; set; }
      public string AccountFrom { get; set; }
      public string AccountTo { get; set; }
      #endregion

      #region GetData

      public static List<Data> GetData(Model.bindImport importModel, string filePath, enDataType type)
      {
         try
         {

            // SHEET
            var sheetName = "";
            if (type == enDataType.Income) { sheetName = "Receitas"; }
            else if (type == enDataType.Expense) { sheetName = "Despesas"; }
            else if (type == enDataType.Transfer) { sheetName = "Transferencias"; }

            // CONNECTION
            var connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", filePath);
            var queryString = string.Format("SELECT * FROM [{0}$]", sheetName);
            var dataAdapter = new OleDbDataAdapter(queryString, connectionString);
            var dataSet = new System.Data.DataSet();

            // EXECUTE
            try
            { dataAdapter.Fill(dataSet, "tableName"); }
            catch (Exception innerEx) {
               importModel.Message += Environment.NewLine;
               importModel.Message += "Exception [" + innerEx.Message + "]" + Environment.NewLine;
               importModel.Message += "Reason[oledb for excell requires 32bits config on IIS's application pool]";
               return new List<Data>();
            }

            // ADJUST
            var dataTable = dataSet.Tables["tableName"];
            dataTable.Rows.RemoveAt(0);

            // EXECUTE
            if (type == enDataType.Transfer)
            { return GetData_Transfers(dataTable); }
            else { return GetData_Entries(dataTable, type); }

         }
         catch (Exception ex) { throw ex; }
      }

      private static List<Data> GetData_Entries(System.Data.DataTable dataTable, enDataType type) {
         try
         {

            // COLUMNS
            dataTable.Columns[0].ColumnName = "Date";
            dataTable.Columns[1].ColumnName = "Paid";
            dataTable.Columns[2].ColumnName = "Value";
            dataTable.Columns[3].ColumnName = "Description";
            dataTable.Columns[4].ColumnName = "Category";
            dataTable.Columns[5].ColumnName = "Account";

            // CONVERT
            var dataEnumerable = dataTable.AsEnumerable();
            var dataList = dataEnumerable
               .Where(x => x.Field<string>("Description") != string.Empty)
               .Select(x => new Data()
               {
                  Type = type,
                  Date = x["Date"].ToString(),
                  Paid = x["Paid"].ToString(),
                  Value = decimal.Parse(x["Value"].ToString()),
                  Description = x["Description"].ToString(),
                  Category = x["Category"].ToString(),
                  Account = x["Account"].ToString()
               })
               .ToList();

            // RESULT
            return dataList;

         }
         catch (Exception ex) { throw; }
      }

      private static List<Data> GetData_Transfers(System.Data.DataTable dataTable)
      {
         try
         {

            // COLUMNS
            dataTable.Columns[0].ColumnName = "Date";
            dataTable.Columns[1].ColumnName = "Value";
            dataTable.Columns[2].ColumnName = "AccountFrom";
            dataTable.Columns[3].ColumnName = "AccountTo";

            // CONVERT
            var dataEnumerable = dataTable.AsEnumerable();
            var dataList = dataEnumerable
               .Select(x => new Data()
               {
                  Type = enDataType.Transfer,
                  Date = x["Date"].ToString(),
                  Paid = "SIM",
                  Value = decimal.Parse(x["Value"].ToString()),
                  Description = "TRANSFER",
                  AccountFrom = x["AccountFrom"].ToString(),
                  AccountTo = x["AccountTo"].ToString()
               })
               .ToList();

            // RESULT
            return dataList;

         }
         catch (Exception ex) { /*throw ex;*/ return new List<Data>(); }
      }

      #endregion

   }

}
