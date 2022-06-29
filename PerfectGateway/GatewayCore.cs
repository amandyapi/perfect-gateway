// Decompiled with JetBrains decompiler
// Type: PerfectGateway.GatewayCore
// Assembly: PerfectGateway, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E14048F0-83C7-4E08-BAE8-B5E0CA532BD7
// Assembly location: D:\Work\adec\PerfectGateway\PerfectGateway.dll

using ConsoleApp5;
using IBP.SDKGatewayLibrary;
using Newtonsoft.Json;
using PerfectGateway.Models;
using PerfectGateway.CustomModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Net;
using System.Net.Security;
using System.Text;

namespace PerfectGateway
{
  public class GatewayCore : GatewayCoreBase
  {
    private bool IsClientInfo;
    private DetailCompteModel[] AccountList;
    public string canWihtdraw = "0";
    private List<string> _traces = new List<string>();
    private Dictionary<string, IBP.SDKGatewayLibrary.State> _statesMap = new Dictionary<string, IBP.SDKGatewayLibrary.State>();

    private Dictionary<string, string> _services = new Dictionary<string, string>();

    private void FillStates()
    {
      this._statesMap.Add("200", IBP.SDKGatewayLibrary.State.AccountExists);
      this._statesMap.Add("401", IBP.SDKGatewayLibrary.State.Rejected);
      this._statesMap.Add("500", IBP.SDKGatewayLibrary.State.DenialOfService);
      this._statesMap.Add("202", IBP.SDKGatewayLibrary.State.Finalized);
      this._statesMap.Add("400", IBP.SDKGatewayLibrary.State.Rejected);
      this._statesMap.Add("409", IBP.SDKGatewayLibrary.State.Rejected);
      this._statesMap.Add("404", IBP.SDKGatewayLibrary.State.AccountNotExists);
      this._statesMap.Add("100000", IBP.SDKGatewayLibrary.State.Finalized);
      this._statesMap.Add("100003", IBP.SDKGatewayLibrary.State.Rejected);
      this._statesMap.Add("100009", IBP.SDKGatewayLibrary.State.Rejected);
    }

    private string GetOperationStatus(IBP.SDKGatewayLibrary.State _state)
    {
       var operationState = string.Empty;

       switch (_state)
       {
         case IBP.SDKGatewayLibrary.State.AccountExists:
            operationState = "0";
            break;
         case IBP.SDKGatewayLibrary.State.Rejected:
            operationState = "1";
            break;
         case IBP.SDKGatewayLibrary.State.DenialOfService:
            operationState = "1";
            break;
         case IBP.SDKGatewayLibrary.State.Finalized:
            operationState = "0";
            break;
         case IBP.SDKGatewayLibrary.State.AccountNotExists:
            operationState = "1";
            break;
         default:
            //Logger.Instance.WriteMessage("\r\n\r\nerreur ope", 1);
            throw new Exception("Ope State not found");
       }
       return operationState;
     }

    private string GetServiceId(RequestMethod requestMethod) 
    {
       string serviceId = string.Empty;

       switch (requestMethod) 
        {
          case RequestMethod.ConsultationSolde:
               serviceId = "49CE3FED-A477-48D3-9DC9-6BE85FD60DD7";
               break;
          case RequestMethod.Releve:
               serviceId = "8AF11B1E-C63C-4A96-80B8-2B2C58551EFB";
               break;
          case RequestMethod.Depot:
               serviceId = "E90A271E-95FC-4207-9DFA-766934BC4A13";
               break;
          case RequestMethod.Retrait:
               serviceId = "2D2A7668-DF75-4D9F-8F55-E528CBCF3A6F";
               break;
          case RequestMethod.TransfertEntreCompte:
               serviceId = "79405C40-2861-42CC-9440-5DBC26AE1360";
               break;
          default:
               //Logger.Instance.WriteMessage("\r\n\r\nerreur ope", 1);
               throw new Exception("Ope Service Id not found");
         }
       return serviceId;
    }

    private object GetContextValue(Context context, string key) => context[(object) key];

    private object GetFilterValue(Context context, string key) => context[(object) key];

    private void InitBaseContext(Hashtable settings)
    {
      BaseContext.ApiUrl = string.Format("{0}", settings[(object) "ApiUrl"]);
      BaseContext.CodeAgent = string.Format("{0}", settings[(object) "CodeAgent"]);
      BaseContext.PassAgent = string.Format("{0}", settings[(object) "PassAgent"]);
      BaseContext.CodeApi = string.Format("{0}", settings[(object) "CodeApi"]);
      BaseContext.NoAgent = string.Format("{0}", settings[(object) "NoAgent"]);
      BaseContext.Password = string.Format("{0}", settings[(object) "Password"]);
      BaseContext.UserName = string.Format("{0}", settings[(object) "UserName"]);
      BaseContext.Method = string.Format("{0}", settings[(object) "Method"]);
    }

    public override void InitGateway(Hashtable settings)
    {
      this.InitBaseContext(settings);
      this.FillStates();
    }

    public override void CheckAccount(ref Context context)
    {
            //Logger.Instance.WriteMessage("\r\n\r\n---Perfect Start check account---", 1);
            //Logger.Instance.WriteMessage("\r\n\r\nPerfect Start check account kIOSK ID: " + string.Format("{0}", context[(object) "PaymentContext.Payment.Point.Id"]), 1);
       //var MykioskId = string.Format("{0}", context[(object)"PaymentContext.Payment.Point.Id"]);
       //var borne = BillHelper.GetBorne(MykioskId);
       //var numero = 0;
      try
      {
        if (context[(object) "nom"].ToString() == "0")
        {
          this.GetClientDetails(ref context);
        }
        else
        {
          string pinotp = string.Format("{0}", context[(object)"PinOtp"]);
          //Logger.Instance.WriteMessage("\r\n\r\nPerfect variable pinotp: " + pinotp, 1);
          
          int index = (int) short.Parse(context[(object) "SelectedAccount"].ToString()) - 1;
          //Logger.Instance.WriteMessage("\r\n\r\nPerfect variable selected account: " + index, 1);
          string numerocompte = this.AccountList[index].numerocompte;
          //Logger.Instance.WriteMessage("\r\n\r\nPerfect variable numero compte: " + numerocompte, 1);
          context[(object) "numeroCompte"] = (object) numerocompte;
          string CodeClient1 = string.Format("{0}", context[(object) "PaymentContext.Payment.Account"]);
          //Logger.Instance.WriteMessage("\r\n\r\nPerfect variable code client: " + CodeClient1, 1);
          RequestMethod requestMethod = (RequestMethod) int.Parse(string.Format("{0}", context[(object) "Method"]).ToString());
          //Logger.Instance.WriteMessage("\r\n\r\nPerfect variable request method: " + requestMethod, 1);
          Decimal num = Decimal.Parse(string.Format("{0}", context[(object)"Retrait"]));
          //Logger.Instance.WriteMessage("\r\n\r\nPerfect variable num " + context[(object)"Retrait"], 1);
          string str1 = num.ToString("0.#");
          //Logger.Instance.WriteMessage("\r\n\r\nPerfect variable str1" + str1, 1);
          string montant = "0";
          //Logger.Instance.WriteMessage("\r\n\r\nPerfect variable montant1 " + montant, 1);
          string str2 = Guid.NewGuid().ToString();
          //Logger.Instance.WriteMessage("\r\n\r\nPerfect variable str2" + str2, 1);

          switch (requestMethod)
          {
            case RequestMethod.ConsultationSolde:
              if (pinotp == "0" || pinotp == "")
              {
                this.GenerateCode("SOLDECOMPTECLIENT", numerocompte);
                context.Status = IBP.SDKGatewayLibrary.State.AccountExists;
                break;
              }
              ResponseModel<ClientInfoResponse> responseModel1 = Service.ClientInfo(str2, numerocompte, pinotp);
              if (responseModel1.Data.CodeReponse == "100000")
              {
                context.Status = IBP.SDKGatewayLibrary.State.AccountExists;
                //Logger.Instance.WriteMessage("\r\n\r\nDescription3 = " + responseModel1.Data.DetailReponse.ToString(), 1);
                context.Description = responseModel1.Data.DetailReponse;
                context[(object) "SoldeDisponible"] = (object)  responseModel1.Data.SoldeDisponible;
                context[(object) "ReferenceReponse"] = (object) responseModel1.Data.ReferenceReponse;
                //Logger.Instance.WriteMessage("\r\n\r\nConsultation de solde: " + (pinotp + " " + numerocompte + " " + context.Description + " " + responseModel1.Data.ReferenceReponse), 1);
                //Logger.Instance.WriteMessage("\r\n\r\nRequest status code3 = " + responseModel1.Data.CodeReponse.ToString(), 1);
                //Logger.Instance.WriteMessage("\r\n\r\nOpération réussie solde disponible3= " + responseModel1.Data.SoldeDisponible, 1);
                context[(object) "erreur"] = (object) context.Description;
                //Logger.Instance.WriteMessage("\r\n\r\nReq 1= " + (CodeClient1 + " " + numerocompte + "   " + (object) 0 + " " + (object) 0 + " " + (object) 0 + " " + (object) 0 + " " + (object) 0 + " " + (object) new Guid(string.Format("{0}", context[(object) "PaymentContext.Payment.Point.Id"])) + " " + str2 + " " + context.Status.ToString() + " " + (object) 6 + " " + context.Description), 1);

                var transactionResult = BillHelper.Transaction(Guid.NewGuid(), CodeClient1, numerocompte, " ", 0, 0, 0, 0, 0, new Guid(string.Format("{0}", context[(object) "PaymentContext.Payment.Point.Id"])), str2, GetOperationStatus(context.Status), 1, context.Description, GetServiceId(RequestMethod.ConsultationSolde));
                //var stop = 0;
                //Logger.Instance.WriteMessage("\r\n\r\nReq 1= " + responseModel1.Data.SoldeDisponible, 1);
                break;
              }
              context.Status = IBP.SDKGatewayLibrary.State.AccountNotExists;
              context.Description = responseModel1.Data.DetailReponse;
              context[(object) "erreur"] = (object) context.Description;
              BillHelper.Transaction(Guid.NewGuid(), CodeClient1, numerocompte, " ", 0, 0, 0, 0, int.Parse(string.Format("{0}", context[(object)"SoldeDisponible"])), new Guid(string.Format("{0}", context[(object) "PaymentContext.Payment.Point.Id"])), str2, GetOperationStatus(context.Status), 1, context.Description, GetServiceId(RequestMethod.ConsultationSolde));
              break;
            case RequestMethod.Releve:
              string datedebut = context[(object) "Datedebut"].ToString();
              string datefin = context[(object) "Datefin"].ToString();
              ResponseModel<ReleveClientResponse> responseModel2 = Service.ReleveClient(str2, numerocompte, datedebut, datefin);
              context.Status = this._statesMap.ContainsKey(responseModel2.Data.CodeReponse.ToString()) ? this._statesMap[responseModel2.Data.CodeReponse.ToString()] : IBP.SDKGatewayLibrary.State.Rejected;
              context.Description = responseModel2.Data.DetailReponse;
              context[(object) "ReferenceReponse"] = (object) responseModel2.Data.ReferenceReponse;
              //Logger.Instance.WriteMessage("\r\n\r\nOpération réussie response releve" + (object) responseModel2, 1);
              break;
            case RequestMethod.Depot:
              context.Status = IBP.SDKGatewayLibrary.State.AccountExists;
              context[(object) "PaymentContext.Payment.Account"] = (object) numerocompte;
              //Logger.Instance.WriteMessage("\r\n\r\nC'est un depot " + (requestMethod.ToString() + " " + pinotp), 1);
              break;
            case RequestMethod.Retrait:
              if (pinotp == "a" || pinotp == "")
              {
                this.GenerateCode("RETRAITCOMPTE", numerocompte);
                //Logger.Instance.WriteMessage("\r\n\r\nRetrait otp Generation done2 ", 1);
                context.Status = IBP.SDKGatewayLibrary.State.AccountExists;
                break;
              }
              //Logger.Instance.WriteMessage("\r\n\r\nRetrait started Pinotp : " + (pinotp + " Montant1: " + str1 + " Montant2: " + string.Format("{0}", context[(object) "Retrait"])), 1);
              GatewayCore.BillCountResponse billCountToDispense = this.GetBillCountToDispense(string.Format("{0}", context[(object) "PaymentContext.Payment.Point.Id"]), str1);
              if (billCountToDispense.CanDispense)
              {
                string fraisOperation = "0";
                //Logger.Instance.WriteMessage("\r\n\r\nRetrait Data: " + (str2 + " " + numerocompte + " " + str1 + " " + pinotp), 1);
                ResponseModel<RetraitCompteClientResponse> responseModel3 = Service.RetraitCompeClient(str2, numerocompte, str1, 22, fraisOperation, pinotp, "Retrait");
                if (responseModel3.Data.DetailReponse == "SUCCESS")
                {
                  //Logger.Instance.WriteMessage("\r\n\r\nAppel de l'api reussi  : ", 1);
                  context.Status = IBP.SDKGatewayLibrary.State.AccountExists;
                  context[(object) "PaymentContext.Payment.Account"] = (object) numerocompte;
                  //Logger.Instance.WriteMessage("\r\n\r\n before Withdraws status  " + context.Status.ToString(), 1);
                  context.Description = responseModel3.Data.DetailReponse;
                  //Logger.Instance.WriteMessage("\r\n\r\nWithdraws description  " + context.Description, 1);
                  context[(object) "SoldeCompte"] = (object) responseModel3.Data.SoldeCompte;
                  context[(object) "ReferenceReponse"] = (object) responseModel3.Data.ReferenceReponse;
                  context[(object) "Note_10000"] = (object) billCountToDispense.BillCount.Bill_10000;
                  context[(object) "Note_5000"] = (object) billCountToDispense.BillCount.Bill_5000;
                  context[(object) "Note_2000"] = (object) billCountToDispense.BillCount.Bill_2000;
                  context[(object) "Note_1000"] = (object) billCountToDispense.BillCount.Bill_1000;
                  //this.UpdateBillCountFile(string.Format("{0}", context[(object) "PaymentContext.Payment.Point.Id"]), billCountToDispense.BillCount.Bill_10000, billCountToDispense.BillCount.Bill_5000, billCountToDispense.BillCount.Bill_2000, billCountToDispense.BillCount.Bill_1000);
                  BillHelper.Transaction(Guid.NewGuid(), CodeClient1, numerocompte, " ", billCountToDispense.BillCount.Bill_10000, billCountToDispense.BillCount.Bill_5000, billCountToDispense.BillCount.Bill_2000, billCountToDispense.BillCount.Bill_1000, Convert.ToInt32(str1), new Guid(string.Format("{0}", context[(object) "PaymentContext.Payment.Point.Id"])), str2, GetOperationStatus(context.Status), 8, context.Description, GetServiceId(RequestMethod.Retrait));
                  //Logger.Instance.WriteMessage("\r\n\r\n AfterWithdraws status  " + context.Status.ToString(), 1);
                  this.canWihtdraw = "1";
                  //Logger.Instance.WriteMessage("\r\n\r\nAppel de l'api reussi  : " + this.canWihtdraw, 1);
                  //Logger.Instance.WriteMessage("\r\n\r\nSolde compte: " + context[(object) "SoldeCompte"], 1);
                  context[(object) "erreur"] = (object) context.Description;
                  break;
                }
                context.Status = IBP.SDKGatewayLibrary.State.AccountNotExists;
                //Logger instance = //Logger.Instance;
                IBP.SDKGatewayLibrary.State status1 = context.Status;
                string mess = "\r\n\r\n before Withdraws status  " + status1.ToString();
                //instance.WriteMessage(mess, 1);
                context.Description = responseModel3.Data.DetailReponse;
                //Logger.Instance.WriteMessage("\r\n\r\nWithdraws description  " + context.Description, 1);
                context[(object) "erreur"] = (object) context.Description;
                Guid Id = Guid.NewGuid();
                string CodeClient2 = CodeClient1;
                string NumeroCompte = numerocompte;
                int int32 = Convert.ToInt32(num);
                Guid kioskId = new Guid(string.Format("{0}", context[(object) "PaymentContext.Payment.Point.Id"]));
                string OpeRef = str2;
                status1 = context.Status;
                string status2 = status1.ToString();
                string description = context.Description;
                BillHelper.Transaction(Id, CodeClient2, NumeroCompte, " ", billCountToDispense.BillCount.Bill_10000, billCountToDispense.BillCount.Bill_5000, billCountToDispense.BillCount.Bill_2000, billCountToDispense.BillCount.Bill_1000, Convert.ToInt32(str1), kioskId, OpeRef, GetOperationStatus(context.Status), 8, description, GetServiceId(RequestMethod.Retrait));
                break;
              }
              context.Status = IBP.SDKGatewayLibrary.State.AccountNotExists;
              //Logger instance1 = //Logger.Instance;
              IBP.SDKGatewayLibrary.State status3 = context.Status;
              string mess1 = "\r\n\r\n before Withdraws status  " + status3.ToString();
              //instance1.WriteMessage(mess1, 1);
              context.Description = "Solde guichetSolde guichet insuffisant";
              //Logger.Instance.WriteMessage("\r\n\r\nWithdraws description  " + context.Description, 1);
              context[(object) "erreur"] = (object) context.Description;
              Guid Id1 = Guid.NewGuid();
              string CodeClient3 = CodeClient1;
              string NumeroCompte1 = numerocompte;
              int int32_1 = Convert.ToInt32(num);
              Guid kioskId1 = new Guid(string.Format("{0}", context[(object) "PaymentContext.Payment.Point.Id"]));
              string OpeRef1 = str2;
              status3 = context.Status;
              string status4 = status3.ToString();
              string description1 = context.Description;
              BillHelper.Transaction(Id1, CodeClient3, NumeroCompte1, " ", 0, 0, 0, 0, int32_1, kioskId1, OpeRef1, GetOperationStatus(context.Status), 8, description1, GetServiceId(RequestMethod.Retrait));
              break;
            case RequestMethod.TransfertEntreCompte:
              //Logger.Instance.WriteMessage("\r\n\r\nPerfect virement ", 1);

              if (pinotp == "0" || pinotp == "")
              {
                //Logger.Instance.WriteMessage("\r\n\r\nPerfect virement pinotp: " + pinotp, 1);

                this.GenerateCode("TRANSFERTCOMPTE", numerocompte);
                context.Status = IBP.SDKGatewayLibrary.State.AccountExists;
                break;
              }
              string _str3 = this.GetContextValue(context, "PaymentContext.Payment.Value").ToString();
              //Logger.Instance.WriteMessage("\r\n\r\nPerfect virement numero compte test: " + _str3, 1);

              string str3 = string.Format("{0}", context[(object)"numeroCompteCredit"]);
              //Logger.Instance.WriteMessage("\r\n\r\nPerfect virement numero compte credit: " + str3, 1);

              Decimal amount1 = Decimal.Parse(this.GetContextValue(context, "PaymentContext.Payment.Value").ToString());
              //Logger.Instance.WriteMessage("\r\n\r\nPerfect virement amount1: " + amount1, 1);
              Decimal amount2 = Decimal.Parse(this.GetContextValue(context, "Retrait").ToString());
              //Logger.Instance.WriteMessage("\r\n\r\nPerfect virement amount2: " + amount2, 1);
              if (amount1 == 0)
              {
                montant = amount2.ToString("0.#");
              } else
              {
                montant = amount1.ToString("0.#");
              }

              //Logger.Instance.WriteMessage("\r\n\r\nTransfert entre compte  Etape 1", 1);
              //Logger.Instance.WriteMessage("\r\n\r\nvar1  "+ str2, 1);
              //Logger.Instance.WriteMessage("\r\n\r\nvar2  " + numerocompte, 1);
              //Logger.Instance.WriteMessage("\r\n\r\nvar3  " + str3, 1);
              //Logger.Instance.WriteMessage("\r\n\r\nvar4  " + montant, 1);
              //Logger.Instance.WriteMessage("\r\n\r\nvar4  " + pinotp, 1);

              ResponseModel<TransfertEntreCompteResponse> responseModel4 = Service.TransfertEntreCompte(str2, numerocompte, str3, montant, 25, "0", pinotp, "Transfert entre comptes ");
              if (responseModel4.Data.CodeReponse == "100000")
              {
                context.Status = IBP.SDKGatewayLibrary.State.AccountExists;
                context.Description = responseModel4.Data.DetailReponse;
               //Logger.Instance.WriteMessage("\r\n\r\ncontext.Description transfert entre compte: " + context.Description, 1);
                context[(object) "erreur"] = (object) context.Description;
                BillHelper.Transaction(Guid.NewGuid(), CodeClient1, numerocompte, str3, 0, 0, 0, 0, Convert.ToInt32(num), new Guid(string.Format("{0}", context[(object) "PaymentContext.Payment.Point.Id"])), str2, GetOperationStatus(context.Status), 14, context.Description, GetServiceId(RequestMethod.TransfertEntreCompte));
                break;
              }
              context.Status = IBP.SDKGatewayLibrary.State.AccountNotExists;
              context.Description = responseModel4.Data.DetailReponse;
             //Logger.Instance.WriteMessage("\r\n\r\nErreur virement compte à compte: " + responseModel4.Data.DetailReponse, 1);
              context[(object) "erreur"] = (object) context.Description;
              //Logger.Instance.WriteMessage("\r\n\r\nTransfert en tre compte erreur  " + responseModel4.Data.DetailReponse, 1);

              BillHelper.Transaction(Guid.NewGuid(), CodeClient1, numerocompte, " ", 0, 0, 0, 0, Convert.ToInt32(num), new Guid(string.Format("{0}", context[(object) "PaymentContext.Payment.Point.Id"])), str2, GetOperationStatus(context.Status), 14, context.Description, GetServiceId(RequestMethod.TransfertEntreCompte));
              break;
            default:
              //Logger.Instance.WriteMessage("\r\n\r\nerreur ope", 1);

              throw new Exception("Ope Type not found");

          }
        }
        //Logger.Instance.WriteMessage("\r\n\r\nPerfect End check account ", 1);
      }
      catch (Exception ex)
      {
       //Logger.Instance.WriteMessage("\r\n\r\n ERROR check account" + (object) ex, 1);
        context.Status = IBP.SDKGatewayLibrary.State.AccountNotExists;
       //Logger.Instance.WriteMessage(ex.Message, 1);
        context[(object) "erreur"] = (object) context.Description;
      }
       //Logger.Instance.WriteMessage("\r\n\r\n---Perfect End check account---", 1);
    }

    public override void CheckProcessStatus(ref Context context)
    {
      context.Status = IBP.SDKGatewayLibrary.State.Rejected;
      context.Description = "Check status is NOT Supported by Service Provider";
    }

    public override void CheckRecallStatus(ref Context context) => throw new NotSupportedException("Recall payment is NOT  Supported");

    public override void Dispose()
    {
    }

    public override void Process(ref Context context)
    {
     //Logger.Instance.WriteMessage("\r\n\r\nProcess Started: ", 1);
      try
      {
        string str1 = string.Format("{0}", context[(object) "PaymentContext.Payment.Number"]);
        string CodeClient = string.Format("{0}", context[(object) "PaymentContext.Payment.Account"]);
        int index = (int) short.Parse(context[(object) "SelectedAccount"].ToString()) - 1;
        //Logger.Instance.WriteMessage("\r\n\r\nPerfect get check account _SelectedAccount" + (object) index, 1);
        string numerocompte = this.AccountList[index].numerocompte;
        Decimal num = Decimal.Parse(this.GetContextValue(context, "PaymentContext.Payment.Value").ToString());
        string montant = num.ToString("0.#");
        string msisdn = string.Format("{0}", context[(object) "Msisdn"]);
        string fraisOperation = string.Format("{0}", context[(object) "FraisOperation"]);
        string pinotp = string.Format("{0}", context[(object) "PinOtp"]);
        string.Format("{0}", context[(object) "PaymentContext.Payment.Account"]);
        string.Format("{0}", context[(object) "numeroCompteCredit"]);
        string str2 = string.Format("{0}", context[(object) "Method"]);
        CodeOperation codeOperation = (CodeOperation) int.Parse(str2.ToString());
       //Logger.Instance.WriteMessage("\r\n\r\nProcess method2: " + str2, 1);
       //Logger.Instance.WriteMessage("\r\n\r\nProcess method2: " + (object) codeOperation, 1);
        this._traces.Add(string.Format("Method: {0}, numero de compte: {1}, referenceOperation: {2}", (object) codeOperation, (object) numerocompte, (object) str1));
        switch (codeOperation)
        {
          case CodeOperation.ConsultationSolde:
            context.Status = IBP.SDKGatewayLibrary.State.Finalized;
            context[(object) "PaymentContext.Payment.Account"] = (object) numerocompte;
           //Logger.Instance.WriteMessage("\r\n\r\nConsultation solde process status: " + (context.Status.ToString() + "  Ok "), 1);
            context[(object) "erreur"] = (object) context.Description;
            break;
          case CodeOperation.Depot:
            ResponseModel<DepotCompteClientResponse> responseModel1 = Service.DepotCompteClient(str1, numerocompte, montant, 21, fraisOperation, "Depot");
            context.Status = this._statesMap.ContainsKey(responseModel1.Data.CodeReponse.ToString()) ? this._statesMap[responseModel1.Data.CodeReponse.ToString()] : IBP.SDKGatewayLibrary.State.Rejected;
            context.Description = responseModel1.Data.DetailReponse;
            context[(object) "SoldeCompte"] = (object) responseModel1.Data.SoldeCompte;
            context[(object) "numeroCompte"] = (object) numerocompte;
            context[(object) "ReferenceReponse"] = (object) responseModel1.Data.ReferenceReponse;
            context[(object) "erreur"] = (object) context.Description;
           //Logger.Instance.WriteMessage("\r\n\r\nDepot description: " + context.Description, 1);
            BillHelper.Transaction(Guid.NewGuid(), CodeClient, numerocompte, " ", 0, 0, 0, 0, Convert.ToInt32(montant), new Guid(string.Format("{0}", context[(object) "PaymentContext.Payment.Point.Id"])), str1, GetOperationStatus(context.Status), 3, context.Description, GetServiceId(RequestMethod.Depot));
            break;
          case CodeOperation.WalletToBank:
            ResponseModel<WalletToBankResponse> bank = Service.WalletToBank(str1, numerocompte, montant, msisdn, fraisOperation, pinotp, "Wallet to bank");
            context.Status = this._statesMap.ContainsKey(bank.Data.CodeReponse.ToString()) ? this._statesMap[bank.Data.CodeReponse.ToString()] : IBP.SDKGatewayLibrary.State.Rejected;
            context.Description = bank.Data.DetailReponse;
            context[(object) "SoldeCompte"] = (object) bank.Data.SoldeCompte;
            context[(object) "ReferenceReponse"] = (object) bank.Data.ReferenceReponse;
            break;
          case CodeOperation.BankToWallet:
            ResponseModel<BankToWalletResponse> wallet = Service.BankToWallet(str1, numerocompte, montant, msisdn, fraisOperation, pinotp, "Bank to wallet");
            context.Status = this._statesMap.ContainsKey(wallet.Data.CodeReponse.ToString()) ? this._statesMap[wallet.Data.CodeReponse.ToString()] : IBP.SDKGatewayLibrary.State.Rejected;
            context.Description = wallet.Data.DetailReponse;
            context[(object) "SoldeCompte"] = (object) wallet.Data.SoldeCompte;
            context[(object) "ReferenceReponse"] = (object) wallet.Data.ReferenceReponse;
            break;
          case CodeOperation.Retrait:
           //Logger.Instance.WriteMessage("\r\n\r\nRetrait effectué Ok" + (context.Status.ToString() + " " + this.canWihtdraw), 1);
            context.Status = IBP.SDKGatewayLibrary.State.Finalized;
           //Logger.Instance.WriteMessage("\r\n\r\nRetrait effectué Ok" + context.Description, 1);
            context[(object) "erreur"] = (object) context.Description;
            break;
          case CodeOperation.Annulation:
            ResponseModel<AnnulationOperationResponse> responseModel2 = Service.AnnulationOperation(str1, numerocompte, montant, 4);
            context.Status = this._statesMap.ContainsKey(responseModel2.Data.CodeReponse.ToString()) ? this._statesMap[responseModel2.Data.CodeReponse.ToString()] : IBP.SDKGatewayLibrary.State.Rejected;
            context.Description = responseModel2.Data.DetailReponse;
            context[(object) "ReferenceReponse"] = (object) responseModel2.Data.ReferenceReponse;
            break;
          case CodeOperation.TransfertEntreCompte:
           //Logger.Instance.WriteMessage("\r\n\r\nTransfert entre compte process status: " + (context.Status.ToString() + "  Ok "), 1);
            context[(object) "erreur"] = (object) context.Description;
            context.Status = IBP.SDKGatewayLibrary.State.Finalized;
            break;
          case CodeOperation.PayementPartenaire:
            ResponseModel<PayementPartenaireResponse> responseModel3 = Service.PayementPartenaire(str1, numerocompte, montant, 15, fraisOperation, pinotp, "Payement partenaire");
            context.Status = this._statesMap.ContainsKey(responseModel3.Data.CodeReponse.ToString()) ? this._statesMap[responseModel3.Data.CodeReponse.ToString()] : IBP.SDKGatewayLibrary.State.Rejected;
            context.Description = responseModel3.Data.DetailReponse;
            context[(object) "SoldeCompte"] = (object) responseModel3.Data.SoldeCompte;
            context[(object) "ReferenceReponse"] = (object) responseModel3.Data.ReferenceReponse;
            break;
          case CodeOperation.PayementMarchand:
            ResponseModel<PayementMarchandResponse> responseModel4 = Service.PayementMarchand(str1, numerocompte, montant, 16, fraisOperation, pinotp, "Payement marchand");
            context.Status = this._statesMap.ContainsKey(responseModel4.Data.CodeReponse.ToString()) ? this._statesMap[responseModel4.Data.CodeReponse.ToString()] : IBP.SDKGatewayLibrary.State.Rejected;
            context.Description = responseModel4.Data.DetailReponse;
            context[(object) "SoldeCompte"] = (object) responseModel4.Data.SoldeCompte;
            context[(object) "ReferenceReponse"] = (object) responseModel4.Data.ReferenceReponse;
            break;
          default:
            throw new Exception("Operaton not found");
        }
      }
      catch (Exception ex)
      {
       //Logger.Instance.WriteMessage("\r\n\r\nUne erreur est survenue " + (object) ex, 1);
        foreach (string trace in this._traces)
         //Logger.Instance.WriteMessage(trace, 1);
        throw;
      }
    }

    private void GetClientDetails(ref Context context)
    {
     //Logger.Instance.WriteMessage("\r\n\r\nPerfect Start get client details", 1);
      try
      {
        string str1 = string.Format("{0}", context[(object) "PaymentContext.Payment.Account"]);
        //Logger.Instance.WriteMessage("\r\n\r\nPerfect Start get client try" + str1, 1);
        string apiUrl = BaseContext.ApiUrl;
        string base64String = Convert.ToBase64String(Encoding.UTF8.GetBytes(BaseContext.UserName + ":" + BaseContext.Password));
        string data = JsonConvert.SerializeObject((object) new
        {
          codeapi = BaseContext.CodeApi,
          nomagent = BaseContext.CodeAgent,
          apikey = "U0tBVVRPOlBhc3NlQDIwMjI=",//Convert.ToBase64String(Encoding.UTF8.GetBytes(BaseContext.CodeAgent + ":" + BaseContext.PassAgent)),
          methode = "signaletique", 
          referenceope = Guid.NewGuid(),
          codeclient = str1
        });
        //Logger.Instance.WriteMessage("\r\n\r\nPerfect get client body" + data, 1);
        using (WebClient webClient = new WebClient())
        {
          ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback) ((_param1, _param2, _param3, _param4) => true);
          webClient.Headers.Add("content-type", "application/json");
          webClient.Headers.Add("Authorization", "Basic " + base64String);
          //Logger.Instance.WriteMessage("\r\n\r\nPerfect get client basicAuthHeader" + (object) webClient.Headers, 1);
          //Logger.Instance.WriteMessage("\r\n\r\nPerfect get client basicAuthHeader" + base64String, 1);
          string str2 = webClient.UploadString(apiUrl, "POST", data);
          ClientInfoResponseModel infoResponseModel = JsonConvert.DeserializeObject<ClientInfoResponseModel>(str2);
          //Logger.Instance.WriteMessage("\r\n\r\nPerfect get client response" + str2, 1);
          if (infoResponseModel.codereponse == "100000")
          {
            context[(object) "nom"] = (object) infoResponseModel.nom;
            context[(object) "prenom"] = (object) infoResponseModel.prenom;
            context[(object) "numtel"] = (object) infoResponseModel.numtel;
            this.AccountList = infoResponseModel.detailcompte;
            int length = this.AccountList.Length;
            //Logger.Instance.WriteMessage("\r\n\r\nPerfect get client prenom" + context[(object) "prenom"], 1);
            for (int index = 0; index < infoResponseModel.detailcompte.Length; ++index)
            {
              int num = index + 1;
              context[(object) ("numerocompte" + (object) num)] = (object) infoResponseModel.detailcompte[index].numerocompte;
              context[(object) ("nomproduit" + (object) num)] = (object) infoResponseModel.detailcompte[index].nomproduit;
              context[(object) ("nomcompte" + (object) num)] = (object) infoResponseModel.detailcompte[index].nomcompte;
              context[(object) ("nomdevise" + (object) num)] = (object) infoResponseModel.detailcompte[index].nomdevise;
            }
            //Logger.Instance.WriteMessage("\r\n\r\nGet client details done", 1);
            this.IsClientInfo = true;
            context.Status = IBP.SDKGatewayLibrary.State.AccountExists;
          }
          else
          {
            context.Description = infoResponseModel.detailreponse;
            this.IsClientInfo = false;
            context.Status = IBP.SDKGatewayLibrary.State.AccountNotExists;
            context[(object) "erreur"] = (object) context.Description;
            //Logger.Instance.WriteMessage("\r\n\r\nErreur" + context.Description, 1);
          }
        }
        //Logger.Instance.WriteMessage("\r\n\r\nGet client details try done", 1);
      }
      catch (Exception ex)
      {
       //Logger.Instance.WriteMessage("\r\n\r\n perfect get client details error " + (object) ex, 1);
      }
      //Logger.Instance.WriteMessage("\r\n\r\nPerfect End get client details", 1);
    }

    private void GenerateCode(string typeOpe, string _numeroCompte)
    {
     //Logger.Instance.WriteMessage("\r\n\r\n perfect Start generate code ", 1);
      string referenceOpe = Guid.NewGuid().ToString();
      string numerocompte = _numeroCompte;
      try
      {
        ResponseModel<GenerationCodeResponse> responseModel = Service.GenerationCode(referenceOpe, numerocompte, typeOpe);
        string referenceReponse = responseModel.Data.ReferenceReponse;
       //Logger.Instance.WriteMessage("\r\n\r\nOtp Sent description1" + responseModel.Data.ReferenceReponse, 1);
       //Logger.Instance.WriteMessage("\r\n\r\nOtp Sent description1" + responseModel.Data.DetailReponse, 1);
      }
      catch (Exception ex)
      {
       //Logger.Instance.WriteMessage("\r\n\r\nCode generation error1  " + (object) ex, 1);
       //Logger.Instance.WriteMessage("\r\n\r\n ReferanceOPe1 " + referenceOpe, 1);
      }
       //Logger.Instance.WriteMessage("\r\n\r\n perfect End generate code ", 1);
    }

    public override void RecallPayment(ref Context context) => throw new NotSupportedException("Recall payment is NOT  Supported");

    public override Hashtable SaveSettings() => (Hashtable) null;

    public override bool SendRoll(Context[] payments, Hashtable filterContext) => true;

    private GatewayCore.BillCountResponse GetBillCountToDispense(
      string Id,
      string amount)
    {
      //Logger.Instance.WriteMessage("\r\n\r\nBillCountResponse amount " + amount, 1);
      int result;
      if (!int.TryParse(amount, out result))
      {
       //Logger.Instance.WriteMessage("\r\n\r\nBillCountResponse error false" + (result.ToString() + " " + amount), 1);
        return new GatewayCore.BillCountResponse()
        {
          CanDispense = false
        };
      }
      KiosksModel borne = BillHelper.GetBorne(Id);
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
     //Logger.Instance.WriteMessage("\r\n\r\nATM Stock: " + (object) borne, 1);
     //Logger.Instance.WriteMessage("\r\n\r\nATM Stock 10000: " + (object) borne.Note_10000, 1);
     //Logger.Instance.WriteMessage("\r\n\r\nATM Stock 5000: " + (object) borne.Note_5000, 1);
     //Logger.Instance.WriteMessage("\r\n\r\nATM Stock 2000: " + (object) borne.Note_2000, 1);
     //Logger.Instance.WriteMessage("\r\n\r\nATM Stock 1000: " + (object) borne.Note_1000, 1);
      int num4 = result / 10000;
      int num5 = result % 10000;
      if (num4 > borne.Note_10000)
      {
        num1 += (num4 - borne.Note_10000) * 2;
        num4 = borne.Note_10000;
      }
      int num6 = num1 + num5 / 5000;
      int num7 = num5 % 5000;
      if (num6 > borne.Note_5000)
      {
        num2 += (num6 - borne.Note_5000) * 5000 / 2000;
        num3 += (num6 - borne.Note_5000) * 5000 % 2000 / 1000;
        num6 = borne.Note_5000;
      }
      int num8 = num2 + num7 / 2000;
      int num9 = num7 % 2000;
      if (num8 > borne.Note_2000)
      {
        num3 += (num8 - borne.Note_2000) * 2;
        num8 = borne.Note_2000;
      }
      int num10 = num3 + num9 / 1000;
      int num11 = num9 % 1000;
      if (num10 > borne.Note_1000 || num11 - num10 * 1000 > 0)
        return new GatewayCore.BillCountResponse()
        {
          CanDispense = false
        };
     //Logger.Instance.WriteMessage("\r\n\r\nATM To dispense 10000: " + (object) num4, 1);
     //Logger.Instance.WriteMessage("\r\n\r\nATM To dispense 5000: " + (object) num6, 1);
     //Logger.Instance.WriteMessage("\r\n\r\nATM To dispense 2000: " + (object) num8, 1);
     //Logger.Instance.WriteMessage("\r\n\r\nATM To dispense 1000: " + (object) num10, 1);
      return new GatewayCore.BillCountResponse()
      {
        CanDispense = true,
        BillCount = new GatewayCore.BillCount()
        {
          Bill_10000 = num4,
          Bill_5000 = num6,
          Bill_2000 = num8,
          Bill_1000 = num10
        }
      };
    }

    public void UpdateBillCountFile(
      string Id,
      int Bill_10000,
      int Bill_5000,
      int Bill_2000,
      int Bill_1000)
    {
      KiosksModel borne = BillHelper.GetBorne(Id);
     //Logger.Instance.WriteMessage("\r\n\r\n Dispensed ATM Stock: " + (Bill_10000.ToString() + " " + (object) Bill_5000 + " " + (object) Bill_2000 + " " + (object) Bill_1000), 1);
     //Logger.Instance.WriteMessage("\r\n\r\nOld ATM Stock: " + (object) borne, 1);
      GatewayCore.BillCount billCount = new GatewayCore.BillCount()
      {
        Bill_10000 = borne.Note_10000 - Bill_10000,
        Bill_5000 = borne.Note_5000 - Bill_5000,
        Bill_2000 = borne.Note_2000 - Bill_2000,
        Bill_1000 = borne.Note_1000 - Bill_1000
      };
      BillHelper.UpdateBorne(Id, billCount.Bill_1000, billCount.Bill_2000, billCount.Bill_5000, billCount.Bill_10000, 0);
     //Logger.Instance.WriteMessage("\r\n\r\nNew ATM Stock: " + (object) billCount, 1);
    }

    private void MultiDispense(int Bill_10000, int Bill_5000, int Bill_2000, int Bill_1000)
    {
      int int32_1 = Convert.ToInt32("09", 16);
      int int32_2 = Convert.ToInt32("03", 16);
      int int32_3 = Convert.ToInt32("3a", 16);
      int num1 = Bill_2000;
      int num2 = Bill_5000;
      int num3 = Bill_10000;
      int num4 = Bill_1000;
      int int32_4 = Convert.ToInt32("00", 16);
      int int32_5 = Convert.ToInt32("00", 16);
      int int32_6 = Convert.ToInt32("00", 16);
      int int32_7 = Convert.ToInt32("00", 16);
      int num5 = int32_3 ^ num1 ^ num2 ^ num3 ^ num4 ^ int32_4 ^ int32_5 ^ int32_6 ^ int32_7;
      int num6 = int32_1 ^ num5 ^ int32_2;
      SerialPort serialPort = new SerialPort();
      serialPort.PortName = "COM1";
      serialPort.Parity = Parity.None;
      serialPort.BaudRate = 9600;
      serialPort.DataBits = 8;
      serialPort.StopBits = StopBits.One;
      if (serialPort.IsOpen)
      {
        serialPort.Close();
        serialPort.Dispose();
      }
      serialPort.Open();
      byte[] buffer1 = new byte[13]
      {
        (byte) 2,
        (byte) 9,
        (byte) 58,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 3,
        (byte) 49
      };
      byte[] buffer2 = new byte[13]
      {
        (byte) 2,
        (byte) 9,
        (byte) int32_3,
        (byte) num1,
        (byte) num2,
        (byte) num3,
        (byte) num4,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 3,
        (byte) num6
      };
      serialPort.Write(buffer1, 0, buffer1.Length);
      serialPort.Write(buffer2, 0, buffer2.Length);
      serialPort.Close();
      serialPort.Dispose();
    }

    private class BillCount
    {
      public int Bill_10000 { get; set; }

      public int Bill_5000 { get; set; }

      public int Bill_2000 { get; set; }

      public int Bill_1000 { get; set; }
    }

    private class BillCountResponse
    {
      public GatewayCore.BillCount BillCount { get; set; }

      public bool CanDispense { get; set; }
    }
  }
}
