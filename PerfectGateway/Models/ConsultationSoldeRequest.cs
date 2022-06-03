﻿// Decompiled with JetBrains decompiler
// Type: PerfectGateway.Models.ConsultationSoldeRequest
// Assembly: PerfectGateway, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E14048F0-83C7-4E08-BAE8-B5E0CA532BD7
// Assembly location: D:\Work\adec\PerfectGateway\PerfectGateway.dll

using Newtonsoft.Json;

namespace PerfectGateway.Models
{
  public class ConsultationSoldeRequest : RequestBase
  {
    public ConsultationSoldeRequest(string referenceOpe, string numeroCompte, string pinotp)
      : base("solde", referenceOpe, numeroCompte)
    {
      this.Pinotp = pinotp;
    }

    [JsonProperty("pinotp")]
    public string Pinotp { get; private set; }
  }
}
