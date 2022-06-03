// Decompiled with JetBrains decompiler
// Type: PerfectGateway.Models.ReleveClientResponse
// Assembly: PerfectGateway, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E14048F0-83C7-4E08-BAE8-B5E0CA532BD7
// Assembly location: D:\Work\adec\PerfectGateway\PerfectGateway.dll

using Newtonsoft.Json;
using System.Collections.Generic;

namespace PerfectGateway.Models
{
  public class ReleveClientResponse : ResponseBase
  {
    [JsonProperty("detail")]
    public List<Details> Detail { get; set; }
  }
}
