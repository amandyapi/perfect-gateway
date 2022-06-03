// Decompiled with JetBrains decompiler
// Type: ConsoleApp5.BillHelper
// Assembly: PerfectGateway, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E14048F0-83C7-4E08-BAE8-B5E0CA532BD7
// Assembly location: D:\Work\adec\PerfectGateway\PerfectGateway.dll

using IBP.SDKGatewayLibrary;
using PerfectGateway;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ConsoleApp5
{
  public static class BillHelper
  {
    public static int UpdateBorne(
      string id,
      int Note_1000,
      int Note_2000,
      int Note_5000,
      int Note_10000)
    {
      DateTime dateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd h:mm tt"));
      string connectionString = BillHelper.GetConnectionString();
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("UPDATE [AdecDb].[monitoring].[Kiosks] SET ");
      stringBuilder.Append(string.Format("Note_1000 = {0}, ", (object) Note_1000));
      stringBuilder.Append(string.Format("Note_2000 = {0}, ", (object) Note_2000));
      stringBuilder.Append(string.Format("Note_5000 = {0}, ", (object) Note_5000));
      stringBuilder.Append(string.Format("Note_10000 = {0}, ", (object) Note_10000));
      stringBuilder.Append(string.Format("Modified = '{0}-{1}-{2} {3}:{4}' ", (object) dateTime.Year, (object) dateTime.Month, (object) dateTime.Day, (object) dateTime.Hour, (object) dateTime.Minute));
      stringBuilder.Append("WHERE Id = '" + id + "'");
      string cmdText = stringBuilder.ToString();
      Logger.Instance.WriteMessage("\r\n\r\nSQL request " + cmdText, 1);
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        using (SqlCommand sqlCommand = new SqlCommand(cmdText, connection))
        {
          connection.Open();
          int num = sqlCommand.ExecuteNonQuery();
          connection.Close();
          return num;
        }
      }
    }


    public static int Transaction(
      Guid Id,
      string CodeClient,
      string NumeroCompte,
      string NumeroCompteCredit,
      int Note_10000,
      int Note_5000,
      int Note_2000,
      int Note_1000,
      int montant,
      Guid kioskId,
      string OpeRef,
      string status,
      int OpeType,
      string Description)
    {
      DateTime dateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd h:mm tt"));
      string connectionString = BillHelper.GetConnectionString();
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("INSERT INTO [AdecDb].[monitoring].[Transactions] VALUES ( ");
      stringBuilder.Append(string.Format("'{0}', ", (object) Id));
      stringBuilder.Append("'" + CodeClient + "', ");
      stringBuilder.Append("'" + NumeroCompte + "', ");
      stringBuilder.Append("'" + NumeroCompteCredit + "', ");
      stringBuilder.Append(string.Format("{0}, ", (object) Note_10000));
      stringBuilder.Append(string.Format("{0}, ", (object) Note_5000));
      stringBuilder.Append(string.Format("{0}, ", (object) Note_2000));
      stringBuilder.Append(string.Format("{0}, ", (object) Note_1000));
      stringBuilder.Append(string.Format("{0}, ", (object) montant));
      stringBuilder.Append(string.Format("'{0}-{1}-{2} {3}:{4}', ", (object) dateTime.Year, (object) dateTime.Month, (object) dateTime.Day, (object) dateTime.Hour, (object) dateTime.Minute));
      stringBuilder.Append(string.Format("'{0}', ", (object) kioskId));
      stringBuilder.Append("'07B06B9A-13D8-4CB7-9099-149C92D8B671', ");
      stringBuilder.Append("'" + OpeRef + "', ");
      stringBuilder.Append("'" + status + "', ");
      stringBuilder.Append(string.Format("{0},", (object) OpeType));
      stringBuilder.Append("'" + Description + "', NULL, NULL, NULL, NULL, NULL ) ");
      string cmdText = stringBuilder.ToString();
      Logger.Instance.WriteMessage("\r\n\r\nSQL request " + cmdText, 1);
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        using (SqlCommand sqlCommand = new SqlCommand(cmdText, connection))
        {
          connection.Open();
          int num = sqlCommand.ExecuteNonQuery();
          connection.Close();
          return num;
        }
      }
    }

    public static KioskModel GetBorne(Guid id)
    {
      string sqlQuery = string.Format("SELECT * FROM [AdecDb].[monitoring].[Kiosks] WHERE Id = '{0}'", (object) id);
      Logger.Instance.WriteMessage("\r\n\r\nSQL request " + sqlQuery, 1);
      List<KioskModel> bornesData = BillHelper.GetBornesData(sqlQuery);
      return bornesData == null || !bornesData.Any<KioskModel>() ? (KioskModel) null : bornesData.FirstOrDefault<KioskModel>();
    }

    public static KioskModel GetBorne(string skId)
    {
      List<KioskModel> bornesData = BillHelper.GetBornesData("SELECT * FROM [AdecDb].[monitoring].[Kiosks] WHERE SkId = '" + skId + "'");
      return bornesData == null || !bornesData.Any<KioskModel>() ? (KioskModel) null : bornesData.FirstOrDefault<KioskModel>();
    }

    public static List<KioskModel> GetBornes()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("SELECT * FROM [AdecDb].[monitoring].[Kiosks]");
      string sqlQuery = stringBuilder.ToString();
      BillHelper.GetBornesData(sqlQuery);
      return BillHelper.GetBornesData(sqlQuery);
    }

    private static List<KioskModel> GetBornesData(string sqlQuery)
    {
      List<KioskModel> bornesData = new List<KioskModel>();
      using (SqlConnection connection = new SqlConnection(BillHelper.GetConnectionString()))
      {
        using (SqlCommand sqlCommand = new SqlCommand(sqlQuery, connection))
        {
          connection.Open();
          SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
          while (sqlDataReader.Read())
          {
            Type type = typeof (KioskModel);
            KioskModel instance = (KioskModel) Activator.CreateInstance(type);
            foreach (PropertyInfo property in type.GetProperties())
            {
              try
              {
                object obj = sqlDataReader[property.Name];
                if (obj != null)
                  property.SetValue((object) instance, Convert.ChangeType(obj, property.PropertyType), (object[]) null);
              }
              catch (Exception ex)
              {
              }
            }
            bornesData.Add(instance);
          }
          connection.Close();
        }
      }
      return bornesData;
    }

    private static string GetConnectionString() => "Data Source=" + "sk-dbs-001.skautomate.com" + ";Initial Catalog=" + "AdecDb" + ";Persist Security Info=True;User ID=" + "connexionString" + ";Password=" + "AZERTY225@a";
  }
}
