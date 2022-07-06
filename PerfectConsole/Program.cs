using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PerfectGateway;
using IBP.SDKGatewayLibrary;


namespace PerfectConsole
{
  class Program
  {
    static void Main(string[] args)
    {
      var gatewayCore = new GatewayCore();
      var settigs = new Hashtable();
      var context = new Context();

      // settings
      //settigs["UserName"] = "wsuser";
      //settigs["Password"] = "Passews@2020";
      //settigs["CodeApi"] = "API_MOBILE";
      //settigs["ApiUrl"] = "https://e-bankingdemo.cagecfi.pro/API_SKAUTO_NEW_WEB/FR/APIEXTERNE/pmobileapi.awp";
      //settigs["NoAgent"] = "0000";
      //settigs["CodeAgent"] = "SKAUTO";
      //settigs["PassAgent"] = "0000";

      settigs["UserName"] = "skautoadec";
      settigs["Password"] = "Ncdc847P";
      settigs["CodeApi"] = "API_MOBILE";
      settigs["ApiUrl"] = "https://145.239.180.49/API_SKAUTO_ADEC_WEB/FR/APIEXTERNE/pmobileapi.awp";
      settigs["NoAgent"] = "0000";
      settigs["CodeAgent"] = "SKAUTO";
      settigs["PassAgent"] = "0000";

      gatewayCore.InitGateway(settigs);


      // cosultation solde
      //context["PaymentContext.Payment.Account"] = "A0104631";A0108128  //"2511106025121000A01038460101";
      context["PaymentContext.Payment.Account"] = "A0108476"; //A0105069
      context["PaymentContext.Payment.Number"] = Guid.NewGuid().ToString();
      context["Datedebut"] = "20200727";
      context["Datefin"] = "20201006";
      context["PaymentContext.Payment.Point.Id"] = "84AB2908-6CE2-4927-BAFA-5C7B0FBB14B1";
      context["PaymentContext.Payment.Value"] = "0";
      context["Codeoperation"] = "22";
      context["Method"] = "22";
      //settigs["21"] = "21";
      context["numeroCompteCredit"] = "25110300A010533701";
        
      context["FraisOperation"] = "0";
      context["PaymentContext.Payment.Number"] = Guid.NewGuid().ToString();
      context["CodeClient"] = "A0108476";//A0108128
            context["Retrait"] = 1000;
      context["SelectedAccount"] = 1;
      context["nom"] = 0;
      context["PinOtp"] = "";
      gatewayCore.CheckAccount(ref context);
      //gatewayCore.Process(ref context);
      gatewayCore.CheckAccount(ref context);
      gatewayCore.CheckAccount(ref context);
      //gatewayCore.CheckAccount(ref context);

      //gatewayCore.CheckAccount(ref context);
      //gatewayCore.CheckAccount(ref context);
      //gatewayCore.CheckAccount(ref context);
      //gatewayCore.Process(ref context);
      //gatewayCore.CheckAccount(ref context);

      var xxx = 0;
    }


  }
}
