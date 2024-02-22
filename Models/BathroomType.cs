using System.Collections.Generic;

namespace CP.Models;

public partial class BathroomType
{
    public long Id { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<Realty> Realties { get; } = new List<Realty>();
}
