using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;

namespace CP.Another
{

    public partial class Dogovor : Window
    {
     
        private int iddd = 0;
        private static int actual = 0;
        private bool potok = true;
        private bool cl = true;
        private int prodavec = 0;
        private int rieltor = 0;
        private double summaComis = 0;


        public Dogovor(int t, int realtoR)
        {
            InitializeComponent();
            iddd = t;
            rieltor = realtoR;
            ZapolnitDogovor();
            new Thread(Update).Start();
        }

        #region Заполнить данные о продавце (без нареканий)
        private void ZapolnitDogovor()
        {
            //Подчеркнуть текст
            //Дата
            //Заполнить договор - данные продавца и недвижимости
            DOG.IsEnabled = false;
            date.Text = DateTime.Now.ToString().Substring(0, 10);
            datadogovora.Text = DateTime.Now.ToString().Substring(0, 10);
            datadogovora.TextDecorations = TextDecorations.Underline;
            date.TextDecorations = TextDecorations.Underline;


            using (RealContext db = new())
            {
                var getmysalesman = from real in db.Realties.AsNoTracking().ToList()
                                    join clie in db.Clients.AsNoTracking().ToList() on real.Salesman equals clie.Id
                                    join pass in db.Passports.AsNoTracking().ToList() on clie.PasswordFk equals pass.Id
                                    join svidetelstvo in db.Proofs.AsNoTracking().ToList() on real.Certificate equals svidetelstvo.Id
                                    join type in db.ObjectTypes.AsNoTracking().ToList() on real.TypeObject equals type.Id
                                    where real.Id == iddd
                                    select new
                                    {
                                        clie.Id,
                                        fio = $"{clie.Firstname} {clie.Name} {clie.Lastname}",
                                        ps = $"{pass.Serial} №  {pass.Number}, выдан {pass.Dateof} {pass.Isby}",
                                        summa = $"{real.Price} рублей",
                                        kadastrnumber = svidetelstvo.Registr,
                                        ploshad = $"{real.Square} м²",
                                        adrecc = real.Adress,
                                        obshee = $"{type.Name} серия: {svidetelstvo.Serial} №{svidetelstvo.Number} от {svidetelstvo.Dateof} \nрегистрационный номер: {svidetelstvo.Registr}",
                                        type = type.Name,
                                        su = real.Price
                                    };
                foreach (var item in getmysalesman)
                {
                    prodavec = (int)item.Id;
                    FIO.Text = item.fio;
                    passPort.Text = item.ps;
                    kadastr.Text = item.kadastrnumber;
                    kvadrat.Text = item.ploshad;
                    adress.Text = item.adrecc;
                    serianomer.Text = item.obshee;
                    den.Text = $"{item.summa}";
                    dom.Text = item.type;
                    summaComis = Convert.ToDouble(item.su);
                }
            }
        }
        #endregion

        #region Заполнение данных о покупателе (решено)
        private async void Update()
        {
            while (potok)
            {
                if (actual != MainWindow.salesmanhik && MainWindow.salesmanhik > 0)
                {
                    using (RealContext db = new())
                    {
                        var getmypoc = from poc in await db.Clients.AsNoTracking().ToListAsync()
                                       join pas in await db.Passports.AsNoTracking().ToListAsync() on poc.PasswordFk equals pas.Id
                                       where poc.Id == Convert.ToInt32(MainWindow.salesmanhik)
                                       select new
                                       {
                                           fiipoc = $"{poc.Firstname} {poc.Name} {poc.Lastname}",
                                           paspoci = $"серии {pas.Serial} № {pas.Number}, выдан {pas.Dateof} {pas.Isby}"
                                       };

                        foreach (var item in getmypoc)
                        {
                            Dispatcher.Invoke(() =>
                            {
                                pokupatel.Text = item.fiipoc;
                                paspoc.Text = item.paspoci;
                            });
                        }
                        actual = MainWindow.salesmanhik;
                    }
                }
            }
        }
        #endregion

        //Добавить или изменить покупателя(без нареканий)
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Добавить или изменить покупателя
            DOG.IsEnabled = true;
            pokupatel.Text = "";
            paspoc.Text = "";

            SalessMan salessMan = new(iddd);
            salessMan.ShowDialog();
        }

        //Закрыть окно(вроде без нареканий)
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Закрыть окно
            //Прервать поток(типо)
            cl = false;
            potok = false;
            Close();
        }

        //Заключить договор(в работе)
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //Заключить договор
            //Сверстать ПДФ + изменения в БД + сохранить пдф
            //Продавец FIO - passPort
            //Покупатель pokupatel - paspoc

            if (pokupatel.Text == "" || paspoc.Text == "" || pokupatel.Text == "" && paspoc.Text == "")
                MessageBox.Show("Не заполнены данные о покупателе", "Внимательней", MessageBoxButton.OK, MessageBoxImage.None);
            else
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены?","Заключение договора", MessageBoxButton.YesNo, MessageBoxImage.None);
                if (result == MessageBoxResult.Yes)
                {
                    //id продавца и покупателя 
                    using(RealContext db = new())
                    {
                        try
                        {
                            //Обновляем данные в таблице Предложения, а именно меняем владельца недвижимости и делаем ее неактуальной, так как сделка состоялась
                            db.Database.ExecuteSqlRaw("UPDATE Realty SET salesman = {0}, actual = {1} WHERE salesman = {2}", MainWindow.salesmanhik, 0, prodavec);

                            //Добавить запись в таблицу Сделки
                            db.Database.ExecuteSqlRaw("INSERT INTO Deals(realtor, offerCode, Buyer, transactionDate, commission, RealtorReward, dealtype)VALUES({0}, {1}, {2}, {3}, {4}, {5}, {6})", rieltor, iddd, MainWindow.salesmanhik, DateTime.Now.ToString().Substring(0, 10), summaComis / 100 * 5, summaComis / 100 * 1.2, "Продажа");
                        }
                        catch (Exception er)
                        {
                            MessageBox.Show(er.Message, "Произошла ошибка");
                        }
                    }
                   
                    Thread thread = new(PDFReaders);
                    thread.IsBackground = true;
                    thread.Start();
                }
                else if (result == MessageBoxResult.No)
                    return;
            }

        }

        //Верстка PDF документа
        private void PDFReaders() 
        {
            try
            {
                //Сохранить документ //Задаем фильтр
                SaveFileDialog dialog = new SaveFileDialog();

                dialog.FileName = "Договор купли-продажи";
                dialog.DefaultExt = "pdf";
                dialog.Filter = "PDF document (*.pdf)|*.pdf";
                bool? result1 = dialog.ShowDialog();
                //Открываем окно виндовс для сохранения документа
                if (result1 == true)
                {
                    PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dialog.FileName));

                    //Создание документа + задаем формат pdf и A4 
                    Document doc = new Document(pdfDoc, PageSize.A4);

                    //Задаем стиль
                    iText.Layout.Style _styleone = new iText.Layout.Style().SetFontColor(ColorConstants.BLUE).SetFontSize(11).SetBorder(new SolidBorder(ColorConstants.BLACK, 0, 5));


                    //Чтоб поддерживал кириллицу в pdf документе
                    PdfFont f2 = PdfFontFactory.CreateFont("arial.ttf", "Identity-H");


                    Dispatcher.Invoke(() =>
                    {
                        Paragraph paragraph = new("ДОГОВОР КУПЛИ-ПРОДАЖИ КВАРТИРЫ");
                        paragraph.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.RIGHT).SetFont(f2).SetFontSize(14).SetMarginLeft(125).SetBold();

                        //Вторая строка Город и дата
                        Cell cell3 = new Cell().Add(new Paragraph($"город Томск\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0{DateTime.Now.ToString().Substring(0, 10)}")).SetFont(f2);
                        Cell cell4 = new Cell().Add(new Paragraph($"Гражданин(ка): {FIO.Text},")).SetFont(f2);
                        Cell cell5 = new Cell().Add(new Paragraph($"паспорт серии {passPort.Text}")).SetFont(f2);
                        Cell cell6 = new Cell().Add(new Paragraph("именуем в дальнейшем продавец, с одной стороны")).SetFont(f2);
                        Cell cell7 = new Cell().Add(new Paragraph($"и гражданин: {pokupatel.Text}")).SetFont(f2);
                        Cell cell8 = new Cell().Add(new Paragraph($"паспорт {paspoc.Text}")).SetFont(f2);
                        Cell cell9 = new Cell().Add(new Paragraph("именуем в дальнейшем покупатель, с другой стороны, заключили Договор о нижеследующем:")).SetFont(f2);

                        Paragraph paragraph1 = new("Предмет договора");
                        paragraph1.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.RIGHT).SetFont(f2).SetFontSize(16).SetMarginLeft(185).SetBold();

                        Cell cell10 = new Cell().Add(new Paragraph($"\u00A0\u00A0\u00A0\u00A0\u00A0Продавец обязуется передать в собственность Покупателя: {dom.Text}")).SetFont(f2);
                        Cell cell11 = new Cell().Add(new Paragraph($"кадастровый номер: {kadastr.Text}\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0общая площадь: {kvadrat.Text}")).SetFont(f2);
                        Cell cell12 = new Cell().Add(new Paragraph($"расположенный по адресу: {adress.Text}")).SetFont(f2);
                        Cell cell13 = new Cell().Add(new Paragraph($"а покупатель обязуется принять ее и уплатить за нее сумму в размере: {den.Text}")).SetFont(f2);
                        Cell cell14 = new Cell().Add(new Paragraph($"\u00A0\u00A0\u00A0\u00A0\u00A0Право собственности Продавца по Договору основывается на следующих документах: ")).SetFont(f2);
                        Cell cell15 = new Cell().Add(new Paragraph($"Свидетельство о праве собственности на {serianomer.Text}\n")).SetFont(f2);
                        Cell cell16 = new Cell().Add(new Paragraph($"Договор купли-продажи от {DateTime.Now.ToString().Substring(0, 10)}")).SetFont(f2);

                        //Добавляем в документ
                        doc.Add(paragraph);
                        doc.Add(cell3);
                        doc.Add(cell4);
                        doc.Add(cell5);
                        doc.Add(cell6);
                        doc.Add(cell7);
                        doc.Add(cell8);
                        doc.Add(cell9);
                        doc.Add(paragraph1);
                        doc.Add(cell10);
                        doc.Add(cell11);
                        doc.Add(cell12);
                        doc.Add(cell13);
                        doc.Add(cell14);
                        doc.Add(cell15);
                        doc.Add(cell16);
                        doc.Close();
                    });

                    MessageBoxResult result = MessageBox.Show("Договор заключен!\nОткрыть документ?", "Документ успешно сохранен!", MessageBoxButton.YesNo, MessageBoxImage.None);

                    if (result == MessageBoxResult.Yes)
                    {

                        var proc = new Process();
                        proc.StartInfo.FileName = dialog.FileName;
                        proc.StartInfo.UseShellExecute = true;
                        proc.Start();
                    }
                    else if (result == MessageBoxResult.No)
                    {
                        return;
                    }
                }
                else if (result1 == false)
                {
                    Random rnd = new();
                    int qwe = rnd.Next(1, 10000);
                    PdfDocument pdfDoc = new PdfDocument(new PdfWriter($"ДКП-{qwe}.pdf" ));

                    //Создание документа + задаем формат pdf и A4 
                    Document doc = new Document(pdfDoc, PageSize.A4);

                    //Задаем стиль
                    iText.Layout.Style _styleone = new iText.Layout.Style().SetFontColor(ColorConstants.BLUE).SetFontSize(11).SetBorder(new SolidBorder(ColorConstants.BLACK, 0, 5));


                    //Чтоб поддерживал кириллицу в pdf документе
                    PdfFont f2 = PdfFontFactory.CreateFont("arial.ttf", "Identity-H");


                    Dispatcher.Invoke(() =>
                    {
                        Paragraph paragraph = new("ДОГОВОР КУПЛИ-ПРОДАЖИ НЕДВИЖИМОСТИ");
                        paragraph.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.RIGHT).SetFont(f2).SetFontSize(14).SetMarginLeft(125).SetBold();

                        //Вторая строка Город и дата
                        Cell cell3 = new Cell().Add(new Paragraph($"город Томск\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0{DateTime.Now.ToString().Substring(0, 10)}")).SetFont(f2);
                        Cell cell4 = new Cell().Add(new Paragraph($"Гражданин(ка): {FIO.Text},")).SetFont(f2);
                        Cell cell5 = new Cell().Add(new Paragraph($"паспорт серии {passPort.Text}")).SetFont(f2);
                        Cell cell6 = new Cell().Add(new Paragraph("именуем в дальнейшем продавец, с одной стороны")).SetFont(f2);
                        Cell cell7 = new Cell().Add(new Paragraph($"и гражданин: {pokupatel.Text}")).SetFont(f2);
                        Cell cell8 = new Cell().Add(new Paragraph($"паспорт {paspoc.Text}")).SetFont(f2);
                        Cell cell9 = new Cell().Add(new Paragraph("именуем в дальнейшем покупатель, с другой стороны, заключили Договор о нижеследующем:")).SetFont(f2);

                        Paragraph paragraph1 = new("Предмет договора");
                        paragraph1.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.RIGHT).SetFont(f2).SetFontSize(16).SetMarginLeft(185).SetBold();

                        Cell cell10 = new Cell().Add(new Paragraph($"\u00A0\u00A0\u00A0\u00A0\u00A0Продавец обязуется передать в собственность Покупателя: {dom.Text}")).SetFont(f2);
                        Cell cell11 = new Cell().Add(new Paragraph($"кадастровый номер: {kadastr.Text}\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0общая площадь: {kvadrat.Text}")).SetFont(f2);
                        Cell cell12 = new Cell().Add(new Paragraph($"расположенный по адресу: {adress.Text}")).SetFont(f2);
                        Cell cell13 = new Cell().Add(new Paragraph($"а покупатель обязуется принять ее и уплатить за нее сумму в размере: {den.Text}")).SetFont(f2);
                        Cell cell14 = new Cell().Add(new Paragraph($"\u00A0\u00A0\u00A0\u00A0\u00A0Право собственности Продавца по Договору основывается на следующих документах: ")).SetFont(f2);
                        Cell cell15 = new Cell().Add(new Paragraph($"Свидетельство о праве собственности на {serianomer.Text}\n")).SetFont(f2);
                        Cell cell16 = new Cell().Add(new Paragraph($"Договор купли-продажи от {DateTime.Now.ToString().Substring(0, 10)}")).SetFont(f2);

                        //Добавляем в документ
                        doc.Add(paragraph);
                        doc.Add(cell3);
                        doc.Add(cell4);
                        doc.Add(cell5);
                        doc.Add(cell6);
                        doc.Add(cell7);
                        doc.Add(cell8);
                        doc.Add(cell9);
                        doc.Add(paragraph1);
                        doc.Add(cell10);
                        doc.Add(cell11);
                        doc.Add(cell12);
                        doc.Add(cell13);
                        doc.Add(cell14);
                        doc.Add(cell15);
                        doc.Add(cell16);
                        doc.Close();
                    });
                    MessageBox.Show("Договор заключен!");
                }
            }
            catch (Exception fer)
            {
                MessageBox.Show(fer.Message, "Что то пошло не так:(");
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = cl;
        }


    }
}
