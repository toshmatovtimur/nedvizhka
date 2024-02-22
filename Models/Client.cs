using System.Collections.Generic;

namespace CP.Models;

public partial class Client
{
    public long Id { get; set; }

    public string Firstname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public long? PasswordFk { get; set; }

    public string Numberphone { get; set; } = null!;

    public virtual ICollection<Deal> Deals { get; } = new List<Deal>();

    public virtual Passport? PasswordFkNavigation { get; set; }
}
