using System.Linq;
using System.Windows;

namespace CP.Another
{
    public partial class SOPS : Window
    {
        public SOPS(int temp)
        {
            InitializeComponent();
            GetMyReestrInfo(temp);
        }


        private void GetMyReestrInfo(int t)
        {
            using (RealContext db = new())
            {
                var Rees = from real in db.Realties.ToList()
                           join pr in db.Proofs.ToList() on real.Certificate equals pr.Id
                           where real.Id == t
                           select new
                           {
                               dat = pr.Dateof,
                               reg = pr.Registr,
                               ser = pr.Serial,
                               num = pr.Number
                           };
                foreach (var re in Rees)
                {
                    date.Text = re.dat;
                    registr.Text = re.reg;
                    serial.Text = re.ser;
                    number.Text = re.num;
                }
            }
        }
    }
}
