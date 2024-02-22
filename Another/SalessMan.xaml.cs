using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CP.Another
{
    public partial class SalessMan : Window
    {
        //Переменная для того чтобы не выбрать в качестве покупателя продавца
        int r = 0;

        public SalessMan()
        {
            InitializeComponent();
            GetMyClients();
        }

        public SalessMan(int reald)
        {
            //Здесь просто задаю id покупателя
            InitializeComponent();
            GetMyClients();
            r = reald;
        }

        //Добавить в базу клиента
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Добавить в базу клиента
            //first - фамилия
            //nam - имя
            //last - отчество
            //numberfhone - номер телефона
            //
            //seria - паспорт серия
            //numb - паспорт номер
            //datevidach - дата выдачи
            //kemvidan - кем выдан
            List<TextBox> textBoxes = new() { first, nam, last, numberfhone, seria, numb, datevidach, kemvidan };
            int temp = 0;
            foreach (var item in textBoxes)
            {
                if (item.Text == "" || item.Text == null)
                temp++;
            }

            if (temp > 0)
                MessageBox.Show("Не заполнено одно из полей!", "Пропущено поле", MessageBoxButton.OK, MessageBoxImage.None);
            else if(temp == 0)
            {
                string ste = first.Text + nam.Text + last.Text + kemvidan.Text + datevidach.Text + numb.Text;
                for (int j = 0; j < ste.Length; j++)
                {
                    if (char.IsDigit(ste[j]))
                        temp++;
                        break;
                }
                ste = numberfhone.Text + seria.Text;
                for (int i = 0; i < ste.Length; i++)
                {
                    if (!char.IsDigit(ste[i]))
                        temp++;
                }
                if (temp > 0)
                    MessageBox.Show("Текстовые поля не должны содержать цифры\nЧисловые поля буквы", "Некорректно заполнено одно из полей!", MessageBoxButton.OK, MessageBoxImage.None);
                else if (temp == 0)
                {
                    //Проверки прошли, теперь добавим клиента в базу
                    using(RealContext db = new())
                    {
                        try
                        {
                            //Сначала вставим данные в таблицу паспорт
                            db.Database.ExecuteSqlRaw("INSERT INTO passport(serial, number, dateof, isby) VALUES({0}, {1}, {2}, {3})", seria.Text, numb.Text, datevidach.Text, kemvidan.Text);

                            //После получим id последнего элемента, который мы добавили в таблицу Паспорт
                            int d = db.Passports.Count();

                            //Вставим данные в таблицу Клиент
                            db.Database.ExecuteSqlRaw("INSERT INTO Client(firstname, name, lastname, passwordFK, numberphone) VALUES({0}, {1}, {2}, {3}, {4})", first.Text, nam.Text, last.Text, d, numberfhone.Text);

                            MessageBox.Show("Новый клиент добавлен в базу!", "Это успех!", MessageBoxButton.OK, MessageBoxImage.None);

                            //Обновление таблицы
                            GetMyClients();

                            var getep = db.Clients.OrderBy(u => u.Id).Last();
                          
                             MessageBoxResult dialog = MessageBox.Show($"Добавить покупателя: {getep.Firstname} {getep.Name} {getep.Lastname}? в договор", "Ваш ход!", MessageBoxButton.YesNo, MessageBoxImage.Question);
                             if (dialog == MessageBoxResult.Yes)
                             {
                                MainWindow.salesmanhik = (int)getep.Id;
                                 Close();
                             }
                             else if (dialog == MessageBoxResult.No)
                             {
                                 return;
                             }

                }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Произошла ошибка :(", MessageBoxButton.OK, MessageBoxImage.None);
                        }
                    }

                    
                }
            }
            
           
        }

        //Общая загрузка списка клиентов агенства (Без нареканий)
        private void GetMyClients()
        {
            
            poisk.Focus();
            using (RealContext db = new())
            {
                var getget = from clie in  db.Clients.AsNoTracking().ToList()
                             join pass in  db.Passports.AsNoTracking().ToList() on clie.PasswordFk equals pass.Id
                             where clie.Id != r
                             select new
                             {
                                 clie.Id,
                                 f = clie.Firstname,
                                 n = clie.Name,
                                 l = clie.Lastname,
                                 nf = clie.Numberphone,
                                 s = pass.Serial,
                                 number = pass.Number,
                                 dv = pass.Dateof,
                                 kv = pass.Isby
                             };
                listviewCards.ItemsSource = getget.ToList();
               
            }
        }

        //Динамический поиск (Без нареканий)
        private async void PoissKK(object sender, KeyEventArgs e)
        {
            //Динамический поиск
            string str = poisk.Text.ToLower().Replace(" ", "");
            using (RealContext db = new())
            {
                var getget = from clie in await db.Clients.AsNoTracking().ToListAsync()
                             join pass in await db.Passports.AsNoTracking().ToListAsync() on clie.PasswordFk equals pass.Id
                             where    clie.Firstname.ToLower().Contains(str) 
                                   || clie.Name.ToLower().Contains(str)
                                   || clie.Lastname.ToLower().Contains(str)
                                   && clie.Id != r
                             select new
                             {
                                 clie.Id,
                                 f = clie.Firstname,
                                 n = clie.Name,
                                 l = clie.Lastname,
                                 nf = clie.Numberphone,
                                 s = pass.Serial,
                                 number = pass.Number,
                                 dv = pass.Dateof,
                                 kv = pass.Isby
                             };
                listviewCards.ItemsSource = getget.ToList();
            }
        }

        //Получение id покупателя при двойном клике на любую запись в таблице(без нареканий)
        private void GetSalesDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var str = GetMyId(listviewCards.SelectedItem.ToString());
            MainWindow.salesmanhik = Convert.ToInt32(str);
            using(RealContext db = new())
            {
                var getep = db.Clients.FirstOrDefault(u => u.Id == MainWindow.salesmanhik);
                str = $"{getep.Firstname} {getep.Name} {getep.Lastname}";
            }
            MessageBoxResult dialog = MessageBox.Show($"Добавить покупателя: {str}?", "Вы уверены в своем выборе?!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(dialog == MessageBoxResult.Yes)
            {
                Close();
            }
            else if(dialog == MessageBoxResult.No)
            {
                return;
            }

            static string GetMyId(string dsa)
            {
                string s = "";
                for (int i = 0; i < dsa.Length; i++)
                {
                    if (char.IsDigit(dsa[i]))
                        s += dsa[i];
                    else if (dsa[i] == ',')
                        break;
                }
                return s;
            }
        }
    }
}
