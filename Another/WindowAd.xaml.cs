using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CP.Another
{
    public partial class WindowAd : Window
    {
        private int idAD;
        int mestniyrieltor = 0;
        bool cl = true;
        public WindowAd(int id, int realtor)
        {
            InitializeComponent();
            idAD = id;
            mestniyrieltor = realtor;
            StartSale();
        }


        private async void StartSale()
        {
            await GetMyRealList();
        }

        private async Task GetMyRealList()
        {

            using (RealContext db = new())
            {
                var goquerty = from real in await db.Realties.AsNoTracking().ToListAsync()
                               join objtype in await db.ObjectTypes.AsNoTracking().ToListAsync() on real.TypeObject equals objtype.Id
                               join foto in await db.Photos.AsNoTracking().ToListAsync() on real.IdPhoto equals foto.Id
                               join owner in await db.Clients.AsNoTracking().ToListAsync() on real.Salesman equals owner.Id
                               join are in await db.Districts.AsNoTracking().ToListAsync() on real.Area equals are.Id
                               join sanuzel in await db.BathroomTypes.AsNoTracking().ToListAsync() on real.BathroomId equals sanuzel.Id
                               where real.Id == idAD
                               select new
                               {
                                   Name = $"{real.NameReal}, {real.Square} м², {real.Floor}/{real.NumberOfStoreys} эт.",
                                   AdddressReal = $"{are.Name}, {real.Adress}",
                                   Status = $"Статус: {real.ProOrAre}",
                                   Opisan = real.Description,
                                   ClientName = owner.Name,
                                   sanuz = $"Санузел: {sanuzel.Type}",
                                   yearPostroy = $"Год постройки: {real.YearOfConstruction}",
                                   Price = $"Цена: {real.Price} рублей",
                                   otdel = $"Отделка: {real.Finishing}",
                                   Contact = $"Владелец: {owner.Firstname} {owner.Name} {owner.Lastname}\nТелефон: {owner.Numberphone}",
                                   Image1 = LoadImage(foto.Image1),
                                   Image2 = LoadImage(foto.Image2),
                                   Image3 = LoadImage(foto.Image3),
                                   Image4 = LoadImage(foto.Image4),
                                   Image5 = LoadImage(foto.Image5),
                                   Image6 = LoadImage(foto.Image6),
                                   Image7 = LoadImage(foto.Image7),
                                   Image8 = LoadImage(foto.Image8),
                                   Image9 = LoadImage(foto.Image9),
                                   Image10 = LoadImage(foto.Image10),
                                   balk = real.BalconyGlazing
                               };
                if (goquerty != null)
                {
                    foreach (var item in goquerty)
                    {
                        Name.Text = item.Name;
                        Adddress.Text = item.AdddressReal;
                        Status.Text = item.Status;
                        Image1.Source = item.Image1;
                        Image2.Source = item.Image2;                                               
                        Image3.Source = item.Image3;                                               
                        Image4.Source = item.Image4;                                               
                        Image5.Source = item.Image5;                                               
                        Image6.Source = item.Image6;                                               
                        Image7.Source = item.Image7;                                               
                        Image8.Source = item.Image8;                                               
                        Image9.Source = item.Image9;
                        Image10.Source = item.Image10;
                        Opisan.Text = item.Opisan;
                        Contact.Text = item.Contact;
                        pricec.Text = item.Price;
                        yearPostroy.Text = item.yearPostroy;
                        sanuzell.Text = item.sanuz;
                        otdelka.Text = item.otdel;
                        balkon.Text = item.balk;
                    }
                }
                else
                    MessageBox.Show("Что то пошло не так");  
            }

           

        }

        private static BitmapImage? LoadImage(byte[] imageData)
        {
            var image = new BitmapImage();
            if (imageData != null)
            {
                using (var mem = new MemoryStream(imageData))
                {
                    mem.Position = 0;
                    image.BeginInit();
                    image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = null;
                    image.StreamSource = mem;
                    image.EndInit();
                }
                image.Freeze();
                return image;
            }
            else return null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //ShowDialog открыть окно с данными свидетельства о собственности
            SOPS oPS = new(idAD);
            oPS.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Заключить договор аренда или продажа
            if(Status.Text == "Статус: Продажа")
            {
                Dogovor dogovor = new(idAD, mestniyrieltor);
                dogovor.ShowDialog();
            }
            else if(Status.Text == "Статус: Аренда")
            {
                Arenda arenda = new(idAD, mestniyrieltor);
                arenda.ShowDialog();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            cl = false;
            Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = cl;
        }
    }
}
