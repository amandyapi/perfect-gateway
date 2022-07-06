// Decompiled with JetBrains decompiler
// Type: PerfectGateway.Service
// Assembly: PerfectGateway, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E14048F0-83C7-4E08-BAE8-B5E0CA532BD7
// Assembly location: D:\Work\adec\PerfectGateway\PerfectGateway.dll

using Newtonsoft.Json;
using PerfectGateway.Models;
using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Text;
using ConsoleApp5;
using IBP.SDKGatewayLibrary;

namespace PerfectGateway
{
  public static class Service
  {
    public static ResponseModel<ClientInfoResponse> ClientInfo(
      string referenceOpe,
      string numeroCompte,
      string pinotp)
    {
      return Service.Execute<ClientInfoResponse>((object) new ConsultationSoldeRequest(referenceOpe, numeroCompte, pinotp));
    }

    public static ResponseModel<ConsultationSoldeResponse> ConsultationSolde(
      string referenceOpe,
      string numeroCompte,
      string pinotp)
    {
      return Service.Execute<ConsultationSoldeResponse>((object) new ConsultationSoldeRequest(referenceOpe, numeroCompte, pinotp));
    }

    public static ResponseModel<ReleveClientResponse> ReleveClient(
      string referenceOpe,
      string numeroCompte,
      string datedebut,
      string datefin,
      int nbreligne = 5)
    {
      return Service.Execute<ReleveClientResponse>((object) new ReleveClientRequest(referenceOpe, numeroCompte, datedebut, datefin, nbreligne));
    }

    public static ResponseModel<DepotCompteClientResponse> DepotCompteClient(
      string referenceOpe,
      string numeroCompte,
      string montant,
      int codeOperation,
      string fraisOperation,
      string descriptionOperation = "")
    {
      return Service.Execute<DepotCompteClientResponse>((object) new DepotCompteClientRequest(referenceOpe, numeroCompte, montant, codeOperation, descriptionOperation, fraisOperation));
    }

    public static ResponseModel<RetraitCompteClientResponse> RetraitCompeClient(
      string referenceOpe,
      string numeroCompte,
      string montant,
      int codeOperation,
      string fraisOperation,
      string pinotp,
      string descriptionOperation = "")
    {
      return Service.Execute<RetraitCompteClientResponse>((object) new RetraitCompteClientRequest(referenceOpe, numeroCompte, montant, codeOperation, descriptionOperation, fraisOperation, pinotp));
    }

    public static ResponseModel<WalletToBankResponse> WalletToBank(
      string referenceOpe,
      string numeroCompte,
      string montant,
      string msisdn,
      string fraisOperation,
      string pinotp,
      string descriptionOperation = "")
    {
      return Service.Execute<WalletToBankResponse>((object) new WalletToBankRequest(referenceOpe, numeroCompte, montant, msisdn, descriptionOperation, fraisOperation, pinotp));
    }

    public static ResponseModel<BankToWalletResponse> BankToWallet(
      string referenceOpe,
      string numeroCompte,
      string montant,
      string msisdn,
      string fraisOperation,
      string pinotp,
      string descriptionOperation = "")
    {
      return Service.Execute<BankToWalletResponse>((object) new BankToWalletRequest(referenceOpe, numeroCompte, montant, msisdn, descriptionOperation, fraisOperation, pinotp));
    }

    public static ResponseModel<PayementMarchandResponse> PayementMarchand(
      string referenceOpe,
      string numeroCompte,
      string montant,
      int codeOperation,
      string fraisOperation,
      string pinotp,
      string descriptionOperation = "")
    {
      return Service.Execute<PayementMarchandResponse>((object) new PayementMarchandRequest(referenceOpe, numeroCompte, montant, codeOperation, fraisOperation, pinotp, descriptionOperation));
    }

    public static ResponseModel<PayementPartenaireResponse> PayementPartenaire(
      string referenceOpe,
      string numeroCompte,
      string montant,
      int codeOperation,
      string fraisOperation,
      string pinotp,
      string descriptionOperation = "")
    {
      return Service.Execute<PayementPartenaireResponse>((object) new PayementPartenaireRequest(referenceOpe, numeroCompte, montant, codeOperation, fraisOperation, pinotp, descriptionOperation));
    }

    public static ResponseModel<TransfertEntreCompteResponse> TransfertEntreCompte(
      string referenceOpe,
      string numerocomptedebit,
      string numerocomptecredit,
      string montant,
      int codeOperation,
      string fraisOperation,
      string pinotp,
      string descriptionOperation = "")
    {
      return Service.Execute<TransfertEntreCompteResponse>((object) new TransfertEntreCompteRequest(referenceOpe, numerocomptedebit, numerocomptecredit, montant, codeOperation, fraisOperation, pinotp, descriptionOperation));
    }

    public static ResponseModel<GenerationCodeResponse> GenerationCode(
      string referenceOpe,
      string numerocompte,
      string typeoperation)
    {
      return Service.Execute<GenerationCodeResponse>((object) new GenerationCodeRequest(referenceOpe, numerocompte, typeoperation));
    }

    public static ResponseModel<VerificationOperationResponse> VerificationOperation(
      string referenceOpe,
      string numeroCompte,
      string montant,
      int codeOperation)
    {
      return Service.Execute<VerificationOperationResponse>((object) new VerificationOperationRequest(referenceOpe, numeroCompte, montant, codeOperation));
    }

    public static ResponseModel<AnnulationOperationResponse> AnnulationOperation(
      string referenceOpe,
      string numeroCompte,
      string montant,
      int codeOperation)
    {
      return Service.Execute<AnnulationOperationResponse>((object) new AnnulationOperationRequest(referenceOpe, numeroCompte, montant, codeOperation));
    }

    private static ResponseModel<T> Execute<T>(object body)
    {
      try
      {
        ServicePointManager.ServerCertificateValidationCallback += (RemoteCertificateValidationCallback) ((sender, certificate, chain, errors) => true);
        WebRequest webRequest = WebRequest.Create(BaseContext.ApiUrl);
        ResponseModel<T> responseModel = new ResponseModel<T>();
        byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(body));
        webRequest.Headers.Add("Authorization", "Basic " + BaseContext.BasicToken);
        webRequest.Method = "POST";
        webRequest.ContentType = "application/json";
        webRequest.ContentLength = (long) bytes.Length;
        Stream requestStream = webRequest.GetRequestStream();
        requestStream.Write(bytes, 0, bytes.Length);
        requestStream.Close();
        using (WebResponse response = webRequest.GetResponse())
        {
          HttpWebResponse httpWebResponse = (HttpWebResponse) response;
          responseModel.Status = new StatusModel()
          {
            Code = (int) httpWebResponse.StatusCode,
            Description = httpWebResponse.StatusDescription
          };
          Stream responseStream;
          using (responseStream = response.GetResponseStream())
          {
            StreamReader streamReader = new StreamReader(responseStream);
            string end = streamReader.ReadToEnd();
            responseModel.Data = JsonConvert.DeserializeObject<T>(end);
            //Logger.Instance.WriteMessage("\r\n\r\nResponse model data: " + responseModel.Data, 1);
            streamReader.Close();
          }
        }
        //Logger.Instance.WriteMessage("\r\n\r\nResponse model data: " + responseModel.Data, 1);
        return responseModel;
      }
      catch (Exception ex)
      {
        //Logger.Instance.WriteMessage("\r\n\r\nResponse model Exception: " + ex, 1);

        throw ex;

      }
    }
  }
}
