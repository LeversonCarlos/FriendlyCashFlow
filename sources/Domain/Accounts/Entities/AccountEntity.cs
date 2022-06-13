﻿namespace Lewio.CashFlow.Domain.Accounts;

public partial class AccountEntity
{

   public Guid ID { get; set; }
   public AccountTypeEnum Type { get; set; }

   public string Text { get; set; } = "";

}
