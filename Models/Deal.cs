
namespace CP.Models;

public partial class Deal
{
    public long Id { get; set; }

    public long? Realtor { get; set; }

    public long? OfferCode { get; set; }

    public long? Buyer { get; set; }

    public string TransactionDate { get; set; } = null!;

    public double? Commission { get; set; }

    public double? RealtorReward { get; set; }

    public string? Dealtype { get; set; }

    public virtual Client? BuyerNavigation { get; set; }

    public virtual Realty? OfferCodeNavigation { get; set; }

    public virtual Realtor? RealtorNavigation { get; set; }
}
