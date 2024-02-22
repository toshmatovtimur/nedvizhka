
using System.Collections.Generic;

namespace CP.Models;

public partial class Realty
{
    public long Id { get; set; }

    public long? TypeObject { get; set; }

    public long? Area { get; set; }

    public string? Adress { get; set; }

    public long? NumberOfStoreys { get; set; }

    public long Floor { get; set; }

    public long? NumberOfRooms { get; set; }

    public double Square { get; set; }

    public string? YearOfConstruction { get; set; }

    public string? Description { get; set; }

    public string Price { get; set; } = null!;

    public long? Salesman { get; set; }

    public long? Actual { get; set; }

    public long? IdPhoto { get; set; }

    public string Material { get; set; } = null!;

    public string Finishing { get; set; } = null!;

    public long? BathroomId { get; set; }

    public string? BalconyGlazing { get; set; }

    public string? NameReal { get; set; }

    public long? Certificate { get; set; }

    public string? ProOrAre { get; set; }

    public virtual District? AreaNavigation { get; set; }

    public virtual BathroomType? Bathroom { get; set; }

    public virtual Proof? CertificateNavigation { get; set; }

    public virtual ICollection<Deal> Deals { get; } = new List<Deal>();

    public virtual Photo? IdPhotoNavigation { get; set; }

    public virtual ObjectType? TypeObjectNavigation { get; set; }
}
