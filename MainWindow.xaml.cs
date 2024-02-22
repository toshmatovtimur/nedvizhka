using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.EntityFrameworkCore;

namespace CP
{
    public partial class MainWindow : Window
    {
        public static int salesmanhik = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        bool logOne = true;
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //Вход
            //Проверка на пустые поля
            //Проверка входных значений
            //Добавить подсветку красным
            if (string.IsNullOrWhiteSpace(login.Text)
            && string.IsNullOrWhiteSpace(password.Password)
            || string.IsNullOrWhiteSpace(login.Text)
            || string.IsNullOrWhiteSpace(password.Password))
            {
                MessageBox.Show("Не заполнено одно или несколько полей");
                if (string.IsNullOrWhiteSpace(login.Text))
                    Keyboard.ClearFocus();
                await RedOrBlackToTextBox();
                if (string.IsNullOrWhiteSpace(password.Password))
                    Keyboard.ClearFocus();
                await RedOrBlackToPass();
            }
            else
            {
                using (RealContext db = new())
                {
                    var GetUsersNotIsAdmin = await db.Realtors.ToListAsync();
                    int temp = 0;
                    foreach (var item in GetUsersNotIsAdmin)
                    {
                        if (item.Login == login.Text && item.Password == password.Password)
                        {
                            temp++;
                            User user = new($"{item.Firstname} {item.Name} {item.Lastname}", (int)item.Id);
                            user.Show();
                            Close();
                            break;
                        }
                    }
                    if (temp == 0)
                    {
                        MessageBox.Show("Повторите попытку", "Неправильный логин или пароль", MessageBoxButton.OK, MessageBoxImage.Stop);
                    }
                }
            }
        }

        private void LogOne(object sender, MouseEventArgs e)
        {
            if (logOne)
            {
                logOne = false;
                login.Clear();
                password.Clear();
                login.Focus();
            }
        }


        //Моргание красным при неправильном вводе
        private async Task RedOrBlackToTextBox()
        {

            await Task.Run(() =>
            {
                Dispatcher.Invoke(async () =>
                {
                    while (true)
                    {
                        login.BorderBrush = Brushes.Red;
                        await Task.Delay(300);
                        login.BorderBrush = Brushes.Black;
                        await Task.Delay(300);
                        login.BorderBrush = Brushes.Red;
                        await Task.Delay(300);
                        login.BorderBrush = Brushes.Black;
                        await Task.Delay(300);
                        login.BorderBrush = Brushes.Red;
                        await Task.Delay(300);
                        login.BorderBrush = Brushes.Black;
                        break;
                    }
                });

            });
        }

        //Моргание красным при неправильном вводе
        private async Task RedOrBlackToPass()
        {
            await Task.Run(() =>
            {
                Dispatcher.Invoke(async () =>
                {
                    while (true)
                    {
                        password.BorderBrush = Brushes.Red;
                        await Task.Delay(300);
                        password.BorderBrush = Brushes.Black;
                        await Task.Delay(300);
                        password.BorderBrush = Brushes.Red;
                        await Task.Delay(300);
                        password.BorderBrush = Brushes.Black;
                        await Task.Delay(300);
                        password.BorderBrush = Brushes.Red;
                        await Task.Delay(300);
                        password.BorderBrush = Brushes.Black;
                        break;
                    }
                });

            });
        }

        private void GoOpen(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Button_Click(sender, e);
        }

        private void GoOpenLog(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Button_Click(sender, e);
        }
    }
}

