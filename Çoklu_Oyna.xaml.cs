using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Go
{
    /// <summary>
    /// Interaction logic for Çoklu_Oyna.xaml
    /// </summary>
    public partial class Çoklu_Oyna : Window
    {
        public Çoklu_Oyna()
        {
            InitializeComponent();
        }

        Ayar ayarlar = new Ayar();
        public static Çoklu_Oyna çoklu;
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (radioButton1.IsChecked == true) //host
            {
                ayarlar.çoklu_oyna_host = true;
                ayarlar.ShowDialog();
            }
            else if (radioButton2.IsChecked == true) //bağlan
            {
                if (textBox1.Text != "")
                {
                    string ip = textBox1.Text;
                    Oyun oyn = new Oyun();
                    oyn.ip = ip;
                    oyn.çoklu_oyna_host = false;
                    oyn.çoklu_oyna_bağlanan = true;
                    oyn.lbl_beyazOyuncuAdı.Content = textBox2.Text;
                    oyn.Owner = this;
                    oyn.ShowDialog();
                }
                else
                    MessageBox.Show("Lütfen bağlanıcağınız bilgisayarın IP'sini girin...","Bilgilendirme");
            }
        }

        private void radioButton1_Checked(object sender, RoutedEventArgs e)
        {
            textBox1.IsEnabled = false;
            textBox2.IsEnabled = false;
            ayarlar.txt_BeyazOyuncu.Text = "kullanılmıyor";
            ayarlar.txt_BeyazOyuncu.IsEnabled = false;
        }

        private void radioButton2_Checked(object sender, RoutedEventArgs e)
        {
            textBox1.IsEnabled = true;
            textBox2.IsEnabled = true;
            ayarlar.txt_BeyazOyuncu.Text = "";
            ayarlar.txt_BeyazOyuncu.IsEnabled = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            çoklu = this;
        }

    }
}
