// Decompiled with JetBrains decompiler
// Type: PerfectGateway.Models.AnnulationOperationRequest
// Assembly: PerfectGateway, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E14048F0-83C7-4E08-BAE8-B5E0CA532BD7
// Assembly location: D:\Work\adec\PerfectGateway\PerfectGateway.dll

using Newtonsoft.Json;

namespace PerfectGateway.Models
{
  public class AnnulationOperationRequest : RequestBase
  {
    public AnnulationOperationRequest(
      string referenceOpe,
      string numeroCompte,
      string montant,
      int codeOperation)
      : base("annulation", referenceOpe, numeroCompte)
    {
      this.Montant = montant;
      this.CodeOperation = codeOperation;
    }

    [JsonProperty("montant")]
    public string Montant { get; private set; }

    [JsonProperty("codeoperation")]
    public int CodeOperation { get; private set; }
  }
}
