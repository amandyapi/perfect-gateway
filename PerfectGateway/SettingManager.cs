// Decompiled with JetBrains decompiler
// Type: PerfectGateway.SettingManager
// Assembly: PerfectGateway, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E14048F0-83C7-4E08-BAE8-B5E0CA532BD7
// Assembly location: D:\Work\adec\PerfectGateway\PerfectGateway.dll

using IBP.SDKGatewayLibrary;
using System;

namespace PerfectGateway
{
  public class SettingManager : SettingManagerBase
  {
    public override string[] GetFilterContextKeys() => new string[11]
    {
      "Filter.Roll.Serial",
      "Filter.Roll.Total",
      "Filter.Roll.Comission",
      "Filter.Roll.Value",
      "Filter.Roll.Count",
      "Filter.Roll.CreationTime",
      "Filter.Roll.StartTime",
      "Filter.Roll.EndTime",
      "Filter.Roll",
      "Filter.Roll.Recipient.DealNumber",
      "Filter.Roll.Recipient.Email"
    };

    public override string[] GetPaymentContextKeys(Operation operation)
    {
      if (!Enum.IsDefined(typeof (Operation), (object) operation))
        return new string[0];
      return new string[19]
      {
        "PaymentContext.Payment.Account",
        "PaymentContext.Payment.Value",
        "PaymentContext.Payment.Serial",
        "PaymentContext.Payment.InputDate",
        "PaymentContext.Payment.Number",
        "PaymentContext.Payment.Point.Id",
        "Datefin",
        "Retrait",
        "Datedebut",
        "Method",
        "Msisdn",
        "FraisOperation",
        "PinOtp",
        "numeroCompteDebit",
        "numeroCompteCredit",
        "CodeClient",
        "SelectedAccount",
        "numeroCompte",
        "erreur"
      };
    }

    public override string[] GetSettingsKey() => new string[8]
    {
      "UserName",
      "Password",
      "ApiUrl",
      "NoAgent",
      "CodeAgent",
      "PassAgent",
      "CodeApi",
      "Method"
    };

    public override void InitSettingManadger(string initString)
    {
    }

    public override string[] SaveSettingKey() => new string[0];

    public override string[] SetPaymentContextKeys(Operation operation)
    {
      switch (operation)
      {
        case Operation.CheckAccount:
          return new string[24]
          {
            "SoldeDisponible",
            "ReferenceReponse",
            "nom",
            "prenom",
            "numtel",
            "numerocompte1",
            "nomproduit1",
            "nomcompte1",
            "nomdevise1",
            "numerocompte2",
            "nomproduit2",
            "nomcompte2",
            "nomdevise2",
            "numerocompte3",
            "nomproduit3",
            "nomcompte3",
            "nomdevise3",
            "numeroCompte",
            "erreur",
            "Retrait",
            "Note_10000",
            "Note_5000",
            "Note_2000",
            "Note_1000"
          };
        case Operation.Process:
          return new string[8]
          {
            "SoldeCompte",
            "ReferenceReponse",
            "Datefin",
            "Datedebut",
            "Method",
            "numeroCompte",
            "erreur",
            "Retrait"
          };
        case Operation.CheckProcessStatus:
          return (string[]) null;
        case Operation.RecallPayment:
          return (string[]) null;
        default:
          return (string[]) null;
      }
    }
  }
}
