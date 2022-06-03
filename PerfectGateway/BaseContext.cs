// Decompiled with JetBrains decompiler
// Type: PerfectGateway.BaseContext
// Assembly: PerfectGateway, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E14048F0-83C7-4E08-BAE8-B5E0CA532BD7
// Assembly location: D:\Work\adec\PerfectGateway\PerfectGateway.dll

using System;
using System.Text;

namespace PerfectGateway
{
  public static class BaseContext
  {
    public static string CodeApi { get; set; }

    public static string CodeAgent { get; set; }

    public static string NoAgent { get; set; }

    public static string UserName { get; set; }

    public static string Password { get; set; }

    public static string ApiUrl { get; set; }

    public static string PassAgent { get; set; }

    public static string Method { get; set; }

    public static string BasicToken => Convert.ToBase64String(Encoding.UTF8.GetBytes(BaseContext.UserName + ":" + BaseContext.Password)) ?? "";
  }
}
