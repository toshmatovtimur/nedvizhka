using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CP.Another;

namespace CP.Pages
{
    public partial class Prodazha : Page
    {
        int myrieltor = 0;
        List<int> kolko = new();
        List<string> list1 = new() { "Продажа", "Аренда" };
        List<string> list2 = new() { "Квартира", "Дом/дача", "Коммерческая", "Гараж/погреб", "Земля" };
        List<string> list3 = new() { "Кировский", "Советский", "Октябрьский", "Ленинский", "Северск", "Томский", "Асиновский", "Область", "Другое" };
        int start = 0;
        int end = 1000000000;
        public Prodazha(int realtor)
        {
            InitializeComponent();
            AddListFor();
            StartSale();
            myrieltor = realtor;
        }
        private void AddListFor()
        {
            kolko.Clear();
            for (int i = 1; i < 150; i++)
            {
                kolko.Add(i);
            }
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
                                   join dist in await db.Districts.AsNoTracking().ToListAsync() on real.Area equals dist.Id
                                   where real.Actual == 1
                                   && list1.Contains(real.ProOrAre)
                                   && list2.Contains(objtype.Name)
                                   && list3.Contains(dist.Name)
                                   && kolko.Contains((int)real.NumberOfRooms)
                                   && Convert.ToInt32(real.Price) > start
                                   && Convert.ToInt32(real.Price) < end
                                   select new
                                   {
                                       id = real.Id,
                                       Name = real.NameReal,
                                       AdressReal = $"Адрес: {real.Adress}",
                                       Status = $"Статус: {real.ProOrAre}",
                                       Opisan = real.Description,
                                       Contact = $"Контакт: {owner.Numberphone}",
                                       Image1 = LoadImage(foto.Image1),
                                       Image2 = LoadImage(foto.Image2),
                                       Image3 = LoadImage(foto.Image3),
                                       Image4 = LoadImage(foto.Image4),
                                       Image5 = LoadImage(foto.Image5)
                                      
                                   };
               
                if (goquerty.Count() == 0)
                    MessageBox.Show("По вашему запросу ничего не найдено");

                    prodazhaList.ItemsSource = goquerty.ToList();

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
        private void NameClick(object sender, MouseButtonEventArgs e)
        {
            //Обнуляем нашего покупателя
            MainWindow.salesmanhik = 0;
            var r = prodazhaList.SelectedItem.ToString();
            string str = "";
            for (int i = 0; i < r.Length; i++)
            {
                if (char.IsDigit(r[i]))
                    str += r[i];
                else if (r[i] == ',')
                    break;
            }
            WindowAd ad = new(Convert.ToInt32(str), myrieltor);
            ad.ShowDialog();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Кнопка очистка фильтра
            prodazhaa.IsChecked = false;
            arendaa.IsChecked = false;
            kvartiry.IsChecked = false;
            domDacha.IsChecked = false;
            kommerciya.IsChecked = false;
            garazh.IsChecked = false;
            zemlya.IsChecked = false;
            kirovskiy.IsChecked = false;
            sovetskiy.IsChecked = false;
            oktyabr.IsChecked = false;
            lenin.IsChecked = false;
            seversk.IsChecked = false;
            tomskiy.IsChecked = false;
            asino.IsChecked = false;
            oblast.IsChecked = false;
            drugoe.IsChecked = false;
            one.IsChecked = false;
            two.IsChecked = false;
            three.IsChecked = false;
            four.IsChecked = false;
            startPrice.Clear();
            endPrice.Clear();
            AddListFor();
            list1 = new List<string>() { "Продажа", "Аренда" };
            list2 = new List<string>() { "Квартира", "Дом/дача", "Коммерческая", "Гараж/погреб", "Земля" };
            list3 = new List<string>() { "Кировский", "Советский", "Октябрьский", "Ленинский", "Северск", "Томский", "Асиновский", "Область", "Другое" };
            start = 0;
            end = 999999999;
        }
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Фильтр первый
            if (prodazhaa.IsChecked == true)
            {
                list1.Clear();
                list1.Add(prodazhaa.Content.ToString());
            }

            else if (arendaa.IsChecked == true)
            {
                list1.Clear();
                list1.Add(arendaa.Content.ToString());
            }
                

            //Фильтр второй
            if(kvartiry.IsChecked == true)
            {
                list2.Clear();
                list2.Add(kvartiry.Content.ToString());
            }

            else if (domDacha.IsChecked == true)
            {
                list2.Clear();
                list2.Add(domDacha.Content.ToString());
            }

            else if (kommerciya.IsChecked == true)
            {
                list2.Clear();
                list2.Add(kommerciya.Content.ToString());
            }

            else if (garazh.IsChecked == true)
            {
                list2.Clear();
                list2.Add(garazh.Content.ToString());
            }

            else if (zemlya.IsChecked == true)
            {
                list2.Clear();
                list2.Add(zemlya.Content.ToString());
            }

            //Фильтр третий

            if(kirovskiy.IsChecked == true)
            {
                list3.Clear();
                list3.Add(kirovskiy.Content.ToString());
            }

            else if (sovetskiy.IsChecked == true)
            {
                list3.Clear();
                list3.Add(sovetskiy.Content.ToString());
            }
            else if (oktyabr.IsChecked == true)
            {
                list3.Clear();
                list3.Add(oktyabr.Content.ToString());
            }
            else if (lenin.IsChecked == true)
            {
                list3.Clear();
                list3.Add(lenin.Content.ToString());
            }
            else if (seversk.IsChecked == true)
            {
                list3.Clear();
                list3.Add(seversk.Content.ToString());
            }
            else if (tomskiy.IsChecked == true)
            {
                list3.Clear();
                list3.Add(tomskiy.Content.ToString());
            }
            else if (asino.IsChecked == true)
            {
                list3.Clear();
                list3.Add(asino.Content.ToString());
            }
            else if (oblast.IsChecked == true)
            {
                list3.Clear();
                list3.Add(oblast.Content.ToString());
            }
            else if (drugoe.IsChecked == true)
            {
                list3.Clear();
                list3.Add(drugoe.Content.ToString());
            }

            var asd = new List<int>();
            asd.Clear();
            int ter = 0;
            if ((bool)one.IsChecked)
            {
                asd.Add(1);
                ter++;
            }
            if ((bool)two.IsChecked)
            {
                asd.Add(2);
                ter++;
            }
            if ((bool)three.IsChecked)
            {
                asd.Add(3);
                ter++;
            }
            if ((bool)four.IsChecked)
            {
                ter++;
                for (int i = 4; i < 150; i++)
                {
                    asd.Add(i);
                }
            }
            if(ter > 0)
            {
                kolko.Clear();
                kolko.AddRange(asd);
            }

            //Цены по новой
            if (startPrice.Text == "" && endPrice.Text == "")
                await GetMyRealList();
            else if(startPrice.Text != "" && endPrice.Text == "" || startPrice.Text == "" && endPrice.Text != "")
            {
                MessageBox.Show("Не заполнено одно из полей", "Запрос не выполнен");
                if(startPrice.Text == "")
                    startPrice.BorderBrush = Brushes.Red;
                if (endPrice.Text == "")
                    endPrice.BorderBrush = Brushes.Red;
            }
            else if(startPrice.Text != "" && endPrice.Text != "")
            {
                if(!IsDigitInString(startPrice.Text) || !IsDigitInString(endPrice.Text) || !IsDigitInString(startPrice.Text) && !IsDigitInString(endPrice.Text))
                {
                    MessageBox.Show("Поля должны содержать целые положительные числа");
                    if (!IsDigitInString(startPrice.Text))
                        startPrice.BorderBrush = Brushes.Red;
                    if (!IsDigitInString(endPrice.Text))
                        endPrice.BorderBrush = Brushes.Red;
                }
                else
                {
                    int temp = 0;
                    if (Convert.ToInt32(startPrice.Text) > 0 && Convert.ToInt32(endPrice.Text) > 0)
                    {
                        if (Convert.ToInt32(startPrice.Text) < Convert.ToInt32(endPrice.Text) || Convert.ToInt32(startPrice.Text) == Convert.ToInt32(endPrice.Text))
                        {
                            start = Convert.ToInt32(startPrice.Text);
                            end = Convert.ToInt32(endPrice.Text);
                        }
                        else if (Convert.ToInt32(startPrice.Text) > Convert.ToInt32(endPrice.Text))
                        {
                            MessageBox.Show("Стартовая цена должна быть меньше конечной", "Запрос не выполнен", MessageBoxButton.OK, MessageBoxImage.Error);
                            temp++;
                        }
                    }

                    else if (Convert.ToInt32(startPrice.Text) < 0 || Convert.ToInt32(endPrice.Text) < 0)
                    {
                        MessageBox.Show("Поле для цен не должны быть меньше нуля", "Запрос не выполнен", MessageBoxButton.OK, MessageBoxImage.Error);
                        temp++;
                    }

                    if (temp > 0)
                        return;
                    else
                        await GetMyRealList();
                }
            }
        }
        private bool IsDigitInString(string str)
        {
            int te = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (!char.IsDigit(str[i]))
                    te++;
            }
            if(te > 0)
                return false;
            else
                return true;

        }
        private void Black(object sender, KeyEventArgs e)
        {
            endPrice.BorderBrush = Brushes.Black;
        }
        private void Blask(object sender, KeyEventArgs e)
        {
            startPrice.BorderBrush = Brushes.Black;
        }
        #region Чтоб не было нулей в начале и впринципе
        private void Cleq(object sender, KeyEventArgs e)
        {
            //СтартАП
            for (int i = 0; i < startPrice.Text.Length; i++)
            {
                if (startPrice.Text == "0")
                { startPrice.Clear(); }
                else
                    break;
            }
            for (int i = 0; i < startPrice.Text.Length; i++)
            {
                if (startPrice.Text.Length > 9)
                {
                    string str = startPrice.Text;
                    startPrice.Clear();
                    for (int j = 0; j < str.Length - 1; j++)
                    {
                        startPrice.Text += str[j];
                    }
                }
                else
                    break;
            }
            string der;
            der = startPrice.Text.Replace(" ", "");
            startPrice.Text = der;
        }
        private void DFG(object sender, KeyEventArgs e)
        {
            //КонецАП
            for (int i = 0; i < endPrice.Text.Length; i++)
            {
                if (endPrice.Text == "0")
                { endPrice.Clear(); }
                else
                    break;
            }
            for (int i = 0; i < endPrice.Text.Length; i++)
            {
                if (endPrice.Text.Length > 9)
                {
                    string str = endPrice.Text;
                    endPrice.Clear();
                    for (int j = 0; j < str.Length - 1; j++)
                    {
                        endPrice.Text += str[j];
                    }
                }
                else
                    break;
            }
            string der;
            der = endPrice.Text.Replace(" ", "");
            endPrice.Text = der;
        }
        #endregion
        private void ZemelyaGo(object sender, RoutedEventArgs e)
        {
            KolKom.Visibility= Visibility.Collapsed;
        }
        private void ZemelyaUot(object sender, RoutedEventArgs e)
        {
            KolKom.Visibility = Visibility.Visible;
        }
        private void GarGo(object sender, RoutedEventArgs e)
        {
            KolKom.Visibility = Visibility.Collapsed;
        }
        private void GarOut(object sender, RoutedEventArgs e)
        {
            KolKom.Visibility = Visibility.Visible;
        }
    }
}
