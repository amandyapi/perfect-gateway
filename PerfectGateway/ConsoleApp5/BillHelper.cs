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
using System.Net;
using System.IO;
using ConsoleApp5;
using PerfectGateway.Models;
using PerfectGateway.ConsoleApp5;
using Newtonsoft.Json;
using System.IO.Ports;
using System.Net.Security;

namespace ConsoleApp5
{
  public static class BillHelper
  {
        public static string _baseUrl = "https://sk-automate.tech/paymentsboapi";
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

    public static string Transaction(
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
          string Description,
          string ServiceId
            )
        {
            try
            {
                var num = 1;
                num = 8;
                var body = new TransactionModel
                {
                    clientCode = CodeClient,
                    clientAccount = NumeroCompte,
                    creditAccount = NumeroCompteCredit,
                    operationRef = OpeRef,
                    status = Int32.Parse(status),
                    note_10000 = Note_10000,
                    note_5000 = Note_5000,
                    note_2000 = Note_2000,
                    note_1000 = Note_1000,
                    kioskId = kioskId.ToString(),
                    operationType = OpeType,
                    serviceId = ServiceId//consultation de solde
                };

                var basicToken = CustomBillHelper.GetBasicToken();

                var url = $"{_baseUrl}/transactions";
                WebRequest request = WebRequest.Create(url);
                //var basicAuth = "Basic ZmluZWxsZTpGMW4zbExFQDJ+Iw==";

                //request.Headers.Add("Authorization", basicAuth);
                request.Headers.Add("Authorization: Basic ZmluZWxsZTpGMW4zbExFQDJ+Iw==");
                num = 1;
                var response = request.Execute<String>(body);

                var transactionId = response.Data;
                return transactionId;
                num = 2;
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteMessage("\r\n\r\nUne erreur est survenue " + (object)ex, 1);
                //foreach (string trace in this._traces)
                    //Logger.Instance.WriteMessage(trace, 1);
                throw;
            }
        }

    public static string SaveTransaction(
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
          string Description
        )
    {
            try
            {
                string apiUrl = $"{_baseUrl}/transactions";

                string body = JsonConvert.SerializeObject((object)new
                {
                    clientCode = CodeClient,
                    clientAccount = NumeroCompte,
                    creditAccount = NumeroCompteCredit,
                    operationRef = OpeRef,
                    status = Int32.Parse(status),
                    note_10000 = Note_10000,
                    note_5000 = Note_5000,
                    note_2000 = Note_2000,
                    note_1000 = Note_1000,
                    kioskId = kioskId.ToString(),
                    operationType = OpeType,
                    serviceId = "49CE3FED-A477-48D3-9DC9-6BE85FD60DD7"//???
                });

                using (WebClient webClient = new WebClient())
                {
                    ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)((_param1, _param2, _param3, _param4) => true);
                    //webClient.Headers.Add("content-type", "application/json");
                    webClient.Headers.Add("Authorization", "Basic " + "ZmluZWxsZTpGMW4zbExFQDJ+Iw==");

                    string transactionStr2 = webClient.UploadString(apiUrl, "POST", body);
                    ClientInfoResponseModel infoResponseModel = JsonConvert.DeserializeObject<ClientInfoResponseModel>(transactionStr2);

                    return transactionStr2;
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteMessage("\r\n\r\nUne erreur est survenue " + (object)ex, 1);
                //foreach (string trace in this._traces)
                //Logger.Instance.WriteMessage(trace, 1);
                throw;
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
