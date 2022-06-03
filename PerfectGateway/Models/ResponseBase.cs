// Decompiled with JetBrains decompiler
// Type: PerfectGateway.Models.ResponseBase
// Assembly: PerfectGateway, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E14048F0-83C7-4E08-BAE8-B5E0CA532BD7
// Assembly location: D:\Work\adec\PerfectGateway\PerfectGateway.dll

using Newtonsoft.Json;

namespace PerfectGateway.Models
{
  public class ResponseBase
  {
    [JsonProperty("codereponse")]
    public string CodeReponse { get; set; }

    [JsonProperty("detailreponse")]
    public string DetailReponse { get; set; }

    [JsonProperty("methodeoperation")]
    public string MethodeOperation { get; set; }

    [JsonProperty("referenceope")]
    public string ReferenceOpe { get; set; }

    [JsonProperty("numerocompte")]
    public string NumeroCompte { get; set; }

    [JsonProperty("referencereponse")]
    public string ReferenceReponse { get; set; }
  }
}
