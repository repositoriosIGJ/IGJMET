using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Printer
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            GetFile();
        }

        private void GetFile()
        {
            byte[] bytes = System.IO.File.ReadAllBytes("http://localhost:10180/Reporte/TramitePago?operacion=7009423&timestamp=83869665174&token=4BE4EFBAF607BD534EE00E12412F9900F586BF13");
            tbxText.Text = bytes.Length.ToString();
        }
    }
}
