// Decompiled with JetBrains decompiler
// Type: PerfectGateway.CodeOperation
// Assembly: PerfectGateway, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E14048F0-83C7-4E08-BAE8-B5E0CA532BD7
// Assembly location: D:\Work\adec\PerfectGateway\PerfectGateway.dll

namespace PerfectGateway
{
  public enum CodeOperation
  {
    ConsultationSolde = 1,
    Depot = 21,
    Verification = 4,
    WalletToBank = 23,
    BankToWallet = 24,
    Retrait = 22, // 0x0000000B
    Annulation = 12, // 0x0000000C
    TransfertEntreCompte = 25, // 0x0000000E
    PayementPartenaire = 26, // 0x0000000F
    PayementMarchand = 27, // 0x00000010
  }
}
