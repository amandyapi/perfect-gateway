// Decompiled with JetBrains decompiler
// Type: PerfectGateway.Models.WalletToBankResponse
// Assembly: PerfectGateway, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E14048F0-83C7-4E08-BAE8-B5E0CA532BD7
// Assembly location: D:\Work\adec\PerfectGateway\PerfectGateway.dll

using Newtonsoft.Json;

namespace PerfectGateway.Models
{
  public class WalletToBankResponse : ResponseBase
  {
    [JsonProperty("soldeCompte")]
    public string SoldeCompte { get; set; }

    [JsonProperty("msisdn")]
    public string Msisdn { get; set; }
  }
}
