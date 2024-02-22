
using System.Collections.Generic;

namespace CP.Models;

public partial class Photo
{
    public long Id { get; set; }

    public byte[]? Image1 { get; set; }

    public byte[]? Image2 { get; set; }

    public byte[]? Image3 { get; set; }

    public byte[]? Image4 { get; set; }

    public byte[]? Image5 { get; set; }

    public byte[]? Image6 { get; set; }

    public byte[]? Image7 { get; set; }

    public byte[]? Image8 { get; set; }

    public byte[]? Image9 { get; set; }

    public byte[]? Image10 { get; set; }

    public byte[]? Image11 { get; set; }

    public byte[]? Image12 { get; set; }

    public virtual ICollection<Realty> Realties { get; } = new List<Realty>();
}
