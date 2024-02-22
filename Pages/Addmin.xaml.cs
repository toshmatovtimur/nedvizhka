using CP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CP.Pages
{
    public partial class Addmin : Page
    {
        private int fds = 0;
        public Addmin()
        {
            InitializeComponent();
            pass.Focus();
        }

        private void Go(object sender, KeyEventArgs e)
        {
            if (pass.Password == "123")
            {
                PasswordShowOff();
                MessageBox.Show("Добро пожаловать, ваше администратейшество!)");
                BaseShow();
                using (RealContext db = new())
                {
                    var df = from g in db.Realtors
                             select new
                             {
                                 g.Id,
                                 fir = g.Firstname,
                                 nam = g.Name,
                                 las = g.Lastname,
                                 tel = g.Numberphone,
                                 log = g.Login,
                                 pas = g.Password
                             };
                    listviewCards.ItemsSource = df.ToList();
                }
                   
            }
               
        }

        private void Fer(object sender, MouseButtonEventArgs e)
        {
            //Двойной клик
            string str = "";
            for (int i = 0; i < listviewCards.SelectedItem.ToString().Length; i++)
            {
                if (char.IsDigit(listviewCards.SelectedItem.ToString()[i]))
                    str += listviewCards.SelectedItem.ToString()[i];
                if (listviewCards.SelectedItem.ToString()[i] == ',')
                    break;
            }
            fds = Convert.ToInt32(str);
            using (RealContext db = new())
            {
                var get = db.Realtors.Where(u => u.Id == Convert.ToInt64(str)).ToList().FirstOrDefault();
                fam.Text = get.Firstname;
                nam.Text = get.Name;
                las.Text = get.Lastname;
                log.Text = get.Login;
                pas.Text = get.Password;
                con.Text = get.Numberphone;
            }
            dob.IsEnabled = false;
            izm.IsEnabled = true;

    
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int temp = 0;
            var der = new List<string>() { fam.Text, nam.Text, las.Text, log.Text, pas.Text, con.Text };
            foreach (var item in der)
            {
                if (item == null || item == "")
                {
                    temp++;
                }
                    
            }
            if(temp == 0)
            {
                //Добавить
                using (RealContext db = new())
                {
                    var Realt = new Realtor();
                    Realt.Firstname = fam.Text;
                    Realt.Name = nam.Text;
                    Realt.Lastname = las.Text;
                    Realt.Login = log.Text;
                    Realt.Password = pas.Text;
                    Realt.Numberphone = con.Text;

                    db.Realtors.Add(Realt);

                    db.SaveChanges();
                    MessageBox.Show("Добавлена новая запись");

                    var df = from g in db.Realtors
                             select new
                             {
                                 g.Id,
                                 fir = g.Firstname,
                                 nam = g.Name,
                                 las = g.Lastname,
                                 tel = g.Numberphone,
                                 log = g.Login,
                                 pas = g.Password
                             };
                    listviewCards.ItemsSource = df.ToList();

                }
            }
            else if(temp > 0)
                MessageBox.Show("Вы пропустили поле или несколько полей");

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Изменить
            using (RealContext db = new())
            {
                var ert = db.Realtors.Where(u => u.Id == fds).FirstOrDefault();
                ert.Firstname =  fam.Text;
                ert.Name =  nam.Text;
                ert.Lastname =  las.Text;
                ert.Login =  log.Text;
                ert.Password =  pas.Text;
                ert.Numberphone = con.Text;

                db.SaveChanges();
                MessageBox.Show("Запись обновлена");
                var df = from g in db.Realtors
                         select new
                         {
                             g.Id,
                             fir = g.Firstname,
                             nam = g.Name,
                             las = g.Lastname,
                             tel = g.Numberphone,
                             log = g.Login,
                             pas = g.Password
                         };
                listviewCards.ItemsSource = df.ToList();
            }
        }

        private void izm_Copy_Click(object sender, RoutedEventArgs e)
        {
            //Очистить
            dob.IsEnabled = true;
            izm.IsEnabled = false;
            fam.Text = "";
            nam.Text = "";
            las.Text = "";
            log.Text = "";
            pas.Text = "";
            con.Text = "";
        }

        private void PasswordShowOff()
        {
            qwe.Visibility = Visibility.Hidden;
        }

        private void BaseShow()
        {
            //Индийский код, да
            tec.Visibility = Visibility.Visible;
            listviewCards.Visibility = Visibility.Visible;
            q.Visibility = Visibility.Visible;
            fam.Visibility = Visibility.Visible;
            w.Visibility = Visibility.Visible;
            e.Visibility = Visibility.Visible;
            r.Visibility = Visibility.Visible;
            log.Visibility = Visibility.Visible;
            las.Visibility = Visibility.Visible;
            nam.Visibility = Visibility.Visible;
            y.Visibility = Visibility.Visible;
            u.Visibility = Visibility.Visible;
            con.Visibility = Visibility.Visible;
            pas.Visibility = Visibility.Visible;
            dob.Visibility = Visibility.Visible;
            izm.Visibility = Visibility.Visible;
            izm_Copy.Visibility = Visibility.Visible;
        }



    }
}
