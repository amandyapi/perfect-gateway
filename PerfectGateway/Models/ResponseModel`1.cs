﻿// Decompiled with JetBrains decompiler
// Type: PerfectGateway.Models.ResponseModel`1
// Assembly: PerfectGateway, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E14048F0-83C7-4E08-BAE8-B5E0CA532BD7
// Assembly location: D:\Work\adec\PerfectGateway\PerfectGateway.dll

namespace PerfectGateway.Models
{
  public class ResponseModel<T>
  {
    public StatusModel Status { get; set; }

    public T Data { get; set; }
  }
}
