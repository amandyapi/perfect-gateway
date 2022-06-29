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
using PerfectGateway.CustomModels;
using PerfectGateway.ConsoleApp5;
using Newtonsoft.Json;
using System.IO.Ports;
using System.Net.Security;

namespace ConsoleApp5
{
    public static class BillHelper
    {
        public static string _baseUrl = "https://sk-automate.tech/paymentsboapi";

        public static bool UpdateBorne(string Id, int Note_1000, int Note_2000, int Note_5000, int Note_10000, int UpdateType)
        {
            try
            {
                var num = 1;
                var body = new KiosksUpdateModel
                {
                    ClientId = Id,
                    Note_10000 = Note_10000,
                    Note_5000 = Note_5000,
                    Note_2000 = Note_2000,
                    Note_1000 = Note_1000,
                    UpdateType = UpdateType,
                };

                var url = $"{_baseUrl}/kiosks/{Id}";
                WebRequest request = WebRequest.Create(url);

                request.Headers.Add("Authorization: Basic ZmluZWxsZTpGMW4zbExFQDJ+Iw==");
                num = 1;
                var response = request.Execute<Boolean>(body);

                var updateResult = response.Data;
                return updateResult;
            }
            catch (Exception ex)
            {
                ////Logger.Instance.WriteMessage("\r\n\r\nUne erreur est survenue " + (object)ex, 1);
                //foreach (string trace in this._traces)
                //////Logger.Instance.WriteMessage(trace, 1);
                throw;
            }

            return false;
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
                    amount = montant,
                    note_10000 = Note_10000,
                    note_5000 = Note_5000,
                    note_2000 = Note_2000,
                    note_1000 = Note_1000,
                    kioskId = kioskId.ToString(),
                    operationType = OpeType,
                    description = Description,
                    serviceId = ServiceId //consultation de solde
                };

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
                ////Logger.Instance.WriteMessage("\r\n\r\nUne erreur est survenue " + (object)ex, 1);
                //foreach (string trace in this._traces)
                //////Logger.Instance.WriteMessage(trace, 1);
                throw;
            }
        }

        public static KiosksModel GetBorne(string id)
        {
            try
            {
                var num = 1;

                var url = $"{_baseUrl}/kiosks/{id}";
                WebRequest request = WebRequest.Create(url);

                request.Headers.Add("Authorization: Basic ZmluZWxsZTpGMW4zbExFQDJ+Iw==");
                num = 1;
                var response = request.Execute<KiosksModel>(null, "GET");

                var kiosk = response.Data;
                return kiosk;
            }
            catch (Exception ex)
            {
                ////Logger.Instance.WriteMessage("\r\n\r\nUne erreur est survenue " + (object)ex, 1);
                //foreach (string trace in this._traces)
                //////Logger.Instance.WriteMessage(trace, 1);
                throw;
            }
        }

    }
}