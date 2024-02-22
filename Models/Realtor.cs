
using System.Collections.Generic;

namespace CP.Models;

public partial class Realtor
{
    public long Id { get; set; }

    public string Firstname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string Numberphone { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Deal> Deals { get; } = new List<Deal>();
}
