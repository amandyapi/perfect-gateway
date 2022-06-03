// Decompiled with JetBrains decompiler
// Type: PerfectGateway.Models.TransfertEntreCompteRequest
// Assembly: PerfectGateway, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E14048F0-83C7-4E08-BAE8-B5E0CA532BD7
// Assembly location: D:\Work\adec\PerfectGateway\PerfectGateway.dll

using Newtonsoft.Json;

namespace PerfectGateway.Models
{
  public class TransfertEntreCompteRequest : RequestBase
  {
    public TransfertEntreCompteRequest(
      string referenceOpe,
      string numerocomptedebit,
      string numerocomptecredit,
      string montant,
      int codeOperation,
      string fraisOperation,
      string pinotp,
      string descriptionOperation = "")
      : base("transfert", referenceOpe, (string) null)
    {
      this.Montant = montant;
      this.CodeOperation = codeOperation;
      this.DescriptionOperation = descriptionOperation;
      this.FraisOperation = fraisOperation;
      this.Pinotp = pinotp;
      this.NumeroCompteCredit = numerocomptecredit;
      this.NumeroCompteDebit = numerocomptedebit;
    }

    [JsonProperty("montant")]
    public string Montant { get; private set; }

    [JsonProperty("codeoperation")]
    public int CodeOperation { get; private set; }

    [JsonProperty("descriptionoperation")]
    public string DescriptionOperation { get; private set; }

    [JsonProperty("fraisoperation")]
    public string FraisOperation { get; private set; }

    [JsonProperty("pinotp")]
    public string Pinotp { get; private set; }

    [JsonProperty("numerocomptedebit")]
    public string NumeroCompteDebit { get; private set; }

    [JsonProperty("numerocomptecredit")]
    public string NumeroCompteCredit { get; private set; }
  }
}
