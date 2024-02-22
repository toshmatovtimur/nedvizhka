
using System.Collections.Generic;

namespace CP.Models;

public partial class Passport
{
    public long Id { get; set; }

    public string Serial { get; set; } = null!;

    public string Number { get; set; } = null!;

    public string Dateof { get; set; } = null!;

    public string Isby { get; set; } = null!;

    public virtual ICollection<Client> Clients { get; } = new List<Client>();
}
