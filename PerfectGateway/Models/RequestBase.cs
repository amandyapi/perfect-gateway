// Decompiled with JetBrains decompiler
// Type: PerfectGateway.Models.RequestBase
// Assembly: PerfectGateway, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E14048F0-83C7-4E08-BAE8-B5E0CA532BD7
// Assembly location: D:\Work\adec\PerfectGateway\PerfectGateway.dll

using Newtonsoft.Json;
using System;
using System.Text;

namespace PerfectGateway.Models
{
  public class RequestBase
  {
    public RequestBase(string methode, string referenceOpe, string numeroCompte)
    {
      this.Methode = methode;
      this.ReferenceOpe = referenceOpe;
      this.NumeroCompte = numeroCompte;
    }

    [JsonProperty("codeapi")]
    public string CodeApi => BaseContext.CodeApi;

    [JsonProperty("codeagent")]
    public string CodeAgent => BaseContext.CodeAgent;

    [JsonProperty("nomagent")]
    public string NoAgent => BaseContext.NoAgent;

    [JsonProperty("apikey")]
    public string Apikey = "U0tBVVRPOlBhc3NlQDIwMjI";//=> Convert.ToBase64String(Encoding.UTF8.GetBytes(BaseContext.CodeAgent + ":" + BaseContext.PassAgent));

    [JsonProperty("methode")]
    public string Methode { get; private set; }

    [JsonProperty("referenceope")]
    public string ReferenceOpe { get; private set; }

    [JsonProperty("numerocompte")]
    public string NumeroCompte { get; private set; }
  }
}
