// Decompiled with JetBrains decompiler
// Type: PerfectGateway.Models.ReleveClientRequest
// Assembly: PerfectGateway, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E14048F0-83C7-4E08-BAE8-B5E0CA532BD7
// Assembly location: D:\Work\adec\PerfectGateway\PerfectGateway.dll

using Newtonsoft.Json;

namespace PerfectGateway.Models
{
  public class ReleveClientRequest : RequestBase
  {
    public ReleveClientRequest(
      string referenceOpe,
      string numeroCompte,
      string datedebut,
      string datefin,
      int nbreligne = 5)
      : base("releve", referenceOpe, numeroCompte)
    {
      this.DateDebut = datedebut;
      this.DateFin = datefin;
      this.NbreLigne = nbreligne;
    }

    [JsonProperty("nbreligne")]
    public int NbreLigne { get; private set; }

    [JsonProperty("datedebut")]
    public string DateDebut { get; private set; }

    [JsonProperty("datefin")]
    public string DateFin { get; internal set; }
  }
}
