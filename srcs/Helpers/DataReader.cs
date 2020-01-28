using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow.Helpers
{

   internal class DataReaderService
   {

      private readonly API.Base.dbContext dbContext;
      public DataReaderService([FromServices] API.Base.dbContext dbContext) { this.dbContext = dbContext; }

      public DataReader GetDataReader(string commandText)
      { return new DataReader(this.dbContext.Database.GetDbConnection(), commandText); }

   }

   internal class DataReader : IDisposable
   {

      private readonly DbConnection conn;
      private readonly List<DbParameter> parameters;
      private DbCommand cmd { get; set; }
      private DbDataReader reader { get; set; }

      internal DataReader(DbConnection conn, string commandText)
      {
         this.conn = conn;
         this.cmd = conn.CreateCommand();
         this.cmd.CommandText = commandText;
         this.parameters = new List<DbParameter>();
      }


      public void AddParameter<T>(string name, T value)
      {
         var param = this.cmd.CreateParameter();
         param.ParameterName = name;
         param.Value = value;
         this.parameters.Add(param);
      }

      public async Task<bool> ExecuteReaderAsync()
      {
         try
         {
            if (this.cmd.Connection.State == ConnectionState.Closed)
            { await this.cmd.Connection.OpenAsync(); }
            if (this.parameters != null && this.parameters.Count != 0)
            { this.cmd.Parameters.AddRange(this.parameters.ToArray()); }
            this.reader = await cmd.ExecuteReaderAsync();
            return true;
         }
         catch (Exception) { throw; }
      }

      private bool HasNextResult = true;
      public async Task<List<T>> GetDataResultAsync<T>() where T : new()
      {
         try
         {
            if (!HasNextResult) { return null; }

            var result = new List<T>();
            while (await this.reader.ReadAsync())
            {
               var record = (IDataRecord)reader;
               var item = new T();
               foreach (var prop in typeof(T).GetProperties())
               {
                  try
                  {
                     var propExists = Enumerable.Range(0, record.FieldCount).Any(x => record.GetName(x) == prop.Name);
                     if (!propExists) { continue; }

                     var val = record[prop.Name];
                     if (val == DBNull.Value)
                        prop.SetValue(item, null);
                     else
                        prop.SetValue(item, val);
                  }
                  catch (Exception) { }
               }
               result.Add(item);
            }

            this.HasNextResult = await this.reader.NextResultAsync();

            return result;
         }
         catch (Exception) { throw; }
      }

      public async Task<List<T>> GetValueResultAsync<T>()
      {
         try
         {
            if (!HasNextResult) { return null; }

            var result = new List<T>();
            while (await this.reader.ReadAsync())
            {
               var record = (IDataRecord)reader;
               var value = record.GetValue(0);
               if (value != DBNull.Value)
               {
                  result.Add((T)value);
               }
            }

            this.HasNextResult = await this.reader.NextResultAsync();
            return result;
         }
         catch (Exception) { throw; }
      }

      public void Dispose()
      {
         this.cmd.Connection.Close();
         this.parameters.Clear();
         if (this.reader != null) { this.reader.Close(); this.reader = null; }
         if (this.cmd != null) { this.cmd.Dispose(); this.cmd = null; }

      }

   }

}
