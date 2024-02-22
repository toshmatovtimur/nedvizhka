using System.Linq;
using System.Threading;
using System.Windows.Controls;

namespace CP.Pages
{
    public partial class Arhiv : Page
    {
        public Arhiv()
        {
            InitializeComponent();
            new Thread(UpDateDeals).Start();
        }


        private void UpDateDeals()
        {
            using(RealContext db = new())
            {
                var getmysdelka = from sdel in db.Deals
                                  join riel in db.Realtors on sdel.Realtor equals riel.Id
                                  join real in db.Realties on sdel.OfferCode equals real.Id
                                  join clie in db.Clients on sdel.Buyer equals clie.Id
                                  select new
                                  {
                                      realt = $"{riel.Firstname} {riel.Name.Substring(0, 1)}. {riel.Lastname.Substring(0, 1)}.",
                                      nedvizh = real.NameReal,
                                      prodavec = $"{clie.Firstname} {clie.Name.Substring(0, 1)}. {clie.Lastname.Substring(0, 1)}.",
                                      dat = sdel.TransactionDate,
                                      commis = sdel.Commission,
                                      comagent = sdel.RealtorReward,
                                      typeDeals = sdel.Dealtype
                                  };
                double? f = 0;
                foreach (var item in getmysdelka)
                {
                    f += item.commis;
                }
                Dispatcher.Invoke(() =>
                {
                    listviewCards.ItemsSource = getmysdelka.ToList();
                    viruhka.Text += f.ToString() + "рублей";
                });
            }
        }
        private void Search(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //Поиск
            using (RealContext db = new())
            {
                var getmysdelka = from sdel in db.Deals
                                  join riel in db.Realtors on sdel.Realtor equals riel.Id
                                  join real in db.Realties on sdel.OfferCode equals real.Id
                                  join clie in db.Clients on sdel.Buyer equals clie.Id
                                  where riel.Firstname.ToLower().Contains(ert.Text.ToLower())
                                     || riel.Name.ToLower().Contains(ert.Text.ToLower())
                                     || riel.Lastname.ToLower().Contains(ert.Text.ToLower())
                                     || real.NameReal.ToLower().Contains(ert.Text.ToLower())
                                  select new
                                  {
                                      realt = $"{riel.Firstname} {riel.Name.Substring(0, 1)}. {riel.Lastname.Substring(0, 1)}.",
                                      nedvizh = real.NameReal,
                                      prodavec = $"{clie.Firstname} {clie.Name.Substring(0, 1)}. {clie.Lastname.Substring(0, 1)}.",
                                      dat = sdel.TransactionDate,
                                      commis = sdel.Commission,
                                      comagent = sdel.RealtorReward,
                                      typeDeals = sdel.Dealtype
                                  };
                Dispatcher.Invoke(() =>
                {
                    listviewCards.ItemsSource = getmysdelka.ToList();
                });
            }
        }

    }
}
