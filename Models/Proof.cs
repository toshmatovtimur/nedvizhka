
using System.Collections.Generic;

namespace CP.Models;

public partial class Proof
{
    public long Id { get; set; }

    public string Serial { get; set; } = null!;

    public string Number { get; set; } = null!;

    public string Dateof { get; set; } = null!;

    public string Registr { get; set; } = null!;

    public virtual ICollection<Realty> Realties { get; } = new List<Realty>();
}
