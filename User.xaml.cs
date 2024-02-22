using CP.Another;
using CP.Models;
using CP.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CP
{
    public partial class User : Window
    {

        int qwe = 0;
        public User(string str, int us)
        {
            InitializeComponent();
            qwe = us;
            Title = str;
            MainFrame.Content = new Prodazha(qwe);
        }

        private void LeFt(object sender, MouseButtonEventArgs e)
        {
            MainFrame.Content = new Prodazha(qwe);
            AddReality.potok = false;
        }

        private void AdDRealse(object sender, MouseButtonEventArgs e)
        {
            MainFrame.Content = new AddReality();
            AddReality.potok = true;
        }

        private void SeeArhive(object sender, MouseButtonEventArgs e)
        {
            AddReality.potok = false;
            MainFrame.Content = new Arhiv();
        }

        private  void NuLadno(object sender, MouseButtonEventArgs e)
        {
            AddReality.potok = false;
            MainFrame.Content = new Addmin();
        }
    }
}
