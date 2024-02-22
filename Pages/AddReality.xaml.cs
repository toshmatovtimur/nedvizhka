using CP.Another;
using CP.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace CP.Pages
{
    public partial class AddReality : Page
    {
        private static int act = 0;
        public static bool potok = true;
        public AddReality()
        {
            InitializeComponent();
            new Thread(UpDateData).Start();
        }

        //Заполнить Comboboxes данными из связанных таблиц(решено)
        private void UpDateData()
        {
            using(RealContext db = new())
            {
                //Типы объектов
                List<ObjectType> typeObj = db.ObjectTypes.FromSqlRaw("SELECT * FROM ObjectType").ToList();
                List<string> list = new();
                foreach (var type in typeObj){list.Add(type.Name);}

                //Районы
                List<District> distr = db.Districts.FromSqlRaw("SELECT * FROM Districts").ToList();
                List<string> list1 = new();
                foreach (var item in distr) {list1.Add(item.Name);}
               
                //Санузел
                List<BathroomType> sunuz = db.BathroomTypes.FromSqlRaw("SELECT * FROM BathroomType").ToList();
                List<string> list2 = new();
                foreach (var item in sunuz) { list2.Add(item.Type); }
                

                Dispatcher.Invoke(() =>
                {
                    category.ItemsSource = list;
                    rayon.ItemsSource = list1;
                    sunuzel.ItemsSource = list2;
                });
            }
        }
        #region Добавить фото(решено)
        //Добавить фотки
        Photo photo = new();
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            potok = false;
            OpenFileDialog ofdPicture = new()
            {
                Filter =
                "Images (*.BMP;*.JPG;*.GIF;*.JFIF;*.JPEG;*.PNG)|*.BMP;*.JPG;*.GIF;*.JFIF;*.JPEG;*.PNG|" +
                "All files (*.*)|*.*",
                Multiselect = true
            };
            System.Windows.Controls.Image ImageName;
            BitmapImage mpImg;
            List<string> images = new List<string>() { "", "", "", "", "", "", "", "", "", "" };
            if (ofdPicture.ShowDialog() == true)
            {
                panel.Children.Clear();
                int temp = 0;

                foreach (var item in ofdPicture.FileNames)
                {

                    //Список картинок(пути к ним)
                    images[temp] = item;
                    temp++;
                    ImageName = new System.Windows.Controls.Image();
                    mpImg = new BitmapImage();
                    mpImg.BeginInit();
                    mpImg.UriSource = new Uri(item);
                    ImageName.Width = 120;
                    ImageName.Height = 100;
                    ImageName.Source = mpImg;

                    panel.Children.Add(ImageName);
                    mpImg.EndInit();

                    if (temp == 10)
                    {
                        break;
                    }

                }
            }
            await Task.Run(() =>
            {
                try
                {
                    using (RealContext db = new())
                    {
                        if (images[0].Length > 0)
                            photo.Image1 = imageBit(images[0]);

                        if (images[1].Length > 0)
                            photo.Image2 = imageBit(images[1]);

                        if (images[2].Length > 0)
                            photo.Image3 = imageBit(images[2]);

                        if (images[3].Length > 0)
                            photo.Image4 = imageBit(images[3]);

                        if (images[4].Length > 0)
                            photo.Image5 = imageBit(images[4]);

                        if (images[5].Length > 0)
                            photo.Image6 = imageBit(images[5]);

                        if (images[6].Length > 0)
                            photo.Image7 = imageBit(images[6]);

                        if (images[7].Length > 0)
                            photo.Image8 = imageBit(images[7]);

                        if (images[8].Length > 0)
                            photo.Image9 = imageBit(images[8]);

                        if (images[9].Length > 0)
                            photo.Image10 = imageBit(images[9]);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Что то пошло не так");
                }
            });


            static byte[] imageBit(string str)
            {
                byte[] bytes;
                FileInfo _imgInfo = new FileInfo(str);
                long _numBytes = _imgInfo.Length;
                FileStream _fileStream = new(str, FileMode.Open, FileAccess.Read); // читаем изображение
                BinaryReader _binReader = new(_fileStream);
                bytes = _binReader.ReadBytes((int)_numBytes); // изображение в байтах
                return bytes;

            }

        }
        #endregion
        #region Добавить владельца(решено)
        //Добавить владельца
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            potok = true;
            new Thread(UpDateToClient).Start();
            sobstvennik.Text = "";
            SalessMan salessMan = new();
            salessMan.ShowDialog();
        }

        //Обновление данных о клиенте
        private void UpDateToClient()
        {
            while (potok)
            {
                if (act != MainWindow.salesmanhik && MainWindow.salesmanhik > 0)
                {
                    using (RealContext db = new())
                    {
                        var getmypoc = db.Clients.FromSqlRaw("SELECT * FROM Client").Where(u => u.Id == MainWindow.salesmanhik).ToList();
                        foreach (var item in getmypoc)
                        {
                            Dispatcher.Invoke(() =>
                            {
                                sobstvennik.Text = $"{item.Firstname} {item.Name} {item.Lastname}";
                            });
                            break;
                        }
                        act = MainWindow.salesmanhik;
                    }
                }
            }
        }



        #endregion

        //Добавить объявление
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //Если все ок, то поток закрываю potok
            potok= false;

            //Сначала проверяю на пустоту
            var mass = new List<string>() { adres.Text, countEtazh.Text, Etazh.Text, countRoom.Text, area.Text, yearOfDesc.Text, price.Text, description.Text, sobstvennik.Text, seria.Text, number.Text, datase.Text, registr.Text };
            int temp = 0;
            foreach (var item in mass)
            {
                if (string.IsNullOrWhiteSpace(item) || string.IsNullOrEmpty(item))
                {
                    temp++;
                }
            }

            if(temp> 0)
            {
                MessageBox.Show("Вы пропустили поле", "Вот балбес ;D");
            }
            else if(temp == 0)
            {
                //Далее проверка на числа
                //Числа
                var chislaa = countEtazh.Text + Etazh.Text + area.Text + price.Text + seria.Text + number.Text;
                chislaa = chislaa.Replace(".", "");
                chislaa = chislaa.Replace(",", "");
                if (!Chislo(chislaa))
                    MessageBox.Show("Вы допустили ошибку, числовые поля не должны содержать буквы", "Вот балбес");
                else
                {

                    //Здесь уже происходит добавление записей в БД
                    using (RealContext db = new())
                    {
                        //После добавления в отдельные таблицы, мне нужны будут id последних записей для главной таблицы

                        //Фотки(Есть)
                        db.Photos.Add(photo);
                        db.SaveChanges();

                        var fotoid = db.Photos.OrderBy(u => u.Id).Last();//(fotoid.Id;)

                        //Свидетельство добавить данные в таблицу свидетельства
                        db.Database.ExecuteSqlRaw("INSERT INTO Proof(serial, number, dateof, registr) VALUES({0}, {1}, {2}, {3})", seria.Text, number.Text, datase.Text, registr.Text);
                        //Получить id Того самого свидетельства
                        var svidetel = db.Proofs.OrderBy(u => u.Id).Last();//(svidetel.Id;)

                        //Клиент
                        //MainWindow.salesmanhik;

                        //Объекты
                        var type = db.ObjectTypes.Where(u => u.Name == category.Text).FirstOrDefault();

                        //Районы
                        var ray = db.Districts.Where(u => u.Name == rayon.Text).FirstOrDefault();//(svidetel.Id;)

                        //Санузел id
                        var sun = db.BathroomTypes.Where(u => u.Type == sunuzel.Text).FirstOrDefault();//(svidetel.Id;)
                        try
                        {
                            Realty realty = new()
                            {
                                TypeObject = type.Id,
                                Area = ray.Id,
                                Adress = adres.Text,
                                NumberOfStoreys = Convert.ToInt64(countEtazh.Text),
                                Floor = Convert.ToInt64(Etazh.Text),
                                NumberOfRooms = Convert.ToInt64(countRoom.Text),
                                Square = Convert.ToDouble(area.Text),
                                YearOfConstruction = yearOfDesc.Text,
                                Description = description.Text,
                                Price = price.Text,
                                Salesman = MainWindow.salesmanhik,
                                Actual = 1,
                                IdPhoto = fotoid.Id,
                                Material = material.Text,
                                Finishing = finish.Text,
                                BathroomId = sun.Id,
                                BalconyGlazing = balcon.Text,
                                Certificate = svidetel.Id,
                                ProOrAre = proare.Text,
                                NameReal = NameForReal(Convert.ToInt64(countRoom.Text), category.Text)

                            };

                            db.Realties.Add(realty);
                            db.SaveChanges();
                            MessageBox.Show("Объявление добавлено", "Довольно успешно, красавчик");
                        }
                        catch (Exception rex)
                        {
                            MessageBox.Show(rex.Message, "Что то пошло не так");
                        }

                    }
                }
            }
           


            static string NameForReal(long t, string str)
            {
                if (t == 1 && str == "Квартира")
                    str = "1-ком. квартира";
                else if (t == 2 && str == "Квартира")
                    str = "2-ком. квартира";
                else if (t > 2 && str == "Квартира")
                    str = "3-ком. квартира";
                else if (str == "Гараж")
                    str = "Гараж";
                else if (str == "Дом/дача")
                    str = "Дом/дача";
                else if (str == "Земля")
                    str = "Земля";
                else if (str == "Коммерческая недвижимость")
                    str = "Коммерческая недвижимость";


                return str;
            }


            static bool Chislo(string str)
            {
                bool fer = double.TryParse(str, out double numb);
                return fer;
            }

        }



       





    }
}
