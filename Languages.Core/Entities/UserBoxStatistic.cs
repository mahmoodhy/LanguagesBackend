using System;
using System.Collections.Generic;

namespace Core.Entities;

public partial class UserBoxStatistic
{
    public string UserName { get; set; } = null!;

    public int BoxDay { get; set; }

    public int? BoxCount { get; set; }
}
