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
    /// Interaction logic for Ayar.xaml
    /// </summary>
    public partial class Ayar : Window
    {
        public Ayar()
        {
            InitializeComponent();
        }

        public bool çoklu_oyna_host = false;
        public static Ayar a;
        private void btn_OyunaBaşla_Click(object sender, RoutedEventArgs e)
        {
            Oyun oyun = new Oyun();
            if (rb_süreli.IsChecked == true)
            {
                oyun.varmı = true;
            }
            else if (rb_süresiz.IsChecked == true)
            {
                oyun.varmı = false;
            }
           


            if (txt_BeyazOyuncu.IsEnabled == false)
            {
                oyun.sp_sohbet.Visibility = Visibility.Visible;
                oyun.btn_sohbetiKapat.Visibility = Visibility.Visible;
                oyun.Width = 1102;
            }
            else
            {
                oyun.sp_sohbet.Visibility = Visibility.Collapsed;
                oyun.btn_sohbetiKapat.Visibility = Visibility.Collapsed;
                oyun.Width = 785;
            }

            oyun.çoklu_oyna_host = çoklu_oyna_host;
            if (çoklu_oyna_host == false)
                oyun.çoklu_oyna_bağlanan = false;
            bool doğru1 = true, doğru2 = true;

            #region Kontroller

            if (txt_SiyahOyuncu.Text == "" || txt_BeyazOyuncu.Text == "")
                doğru1 = false;
            else if (txt_BeyazOyuncu.Text != "" && txt_SiyahOyuncu.Text != "")
                doğru1 = true;
            if (rb_13x13.IsChecked == true || rb_19x19.IsChecked == true || rb_9x9.IsChecked == true)
                doğru2 = true;
            else
                doğru2 = false;

            #region radioButtonlar

            if (rb_9x9.IsChecked == true)
            {
                oyun.ayar_9x9 = true;
                oyun.ayar_13x13 = false;
                oyun.ayar_19x19 = false;
            }
            else if (rb_13x13.IsChecked == true)
            {
                oyun.ayar_9x9 = false;
                oyun.ayar_13x13 = true;
                oyun.ayar_19x19 = false;
            }
            else if(rb_19x19.IsChecked == true)
            {
                oyun.ayar_19x19 = true;
                oyun.ayar_13x13 = false;
                oyun.ayar_9x9 = false;
            }

            #endregion

            if (doğru1 == true && doğru2 == true)
            {
                oyun.lbl_beyazOyuncuAdı.Content = txt_BeyazOyuncu.Text;
                oyun.lbl_SiyahOyuncuAdı.Content = txt_SiyahOyuncu.Text;

                oyun.Owner = this;
                oyun.ShowDialog();
            }
            else
                MessageBox.Show("Lütfen tüm bilgileri doldurun...","Bilgilendirme");

          #endregion

            #region temizle

            if(çoklu_oyna_host != true)
                txt_BeyazOyuncu.Clear();
            txt_SiyahOyuncu.Clear();
            rb_9x9.IsChecked = false;
            rb_13x13.IsChecked = false;
            rb_19x19.IsChecked = false;

            #endregion
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            a = this;
        }
    }
}
