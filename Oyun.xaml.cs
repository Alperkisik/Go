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
using System.Windows.Threading;
using System.Data.SqlClient;
using System.Net.Sockets;
using System.IO;

namespace Go
{
    /// <summary>
    /// Interaction logic for Oyun.xaml
    /// </summary>
    public partial class Oyun : Window
    {
        public Oyun()
        {
            InitializeComponent();
        }

        Masa ms = new Masa();
        public static Oyun oyun_penceresi;
        DispatcherTimer puan_kontrol;
        public bool ayar_9x9 = false, ayar_13x13 = false, ayar_19x19 = true , çoklu_oyna_host = false , çoklu_oyna_bağlanan = false , varmı = true;
        public string ip = "";
        Sohbet sohbet;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lbl_byzSüre.Visibility = Visibility.Hidden;
            puan_kontrol = new DispatcherTimer();
            puan_kontrol.Tick += new EventHandler(puan_kontrol_Tick);
            puan_kontrol.Interval = new TimeSpan(0, 0, 1);
            oyun_penceresi = this;
            if (çoklu_oyna_bağlanan == true || çoklu_oyna_host == true)
                sohbet = new Sohbet();
            ms.BağlantıBaşlat(new Ayarlar(GameBoard, grid1, bd_1, lbl_siyahPuan, lbl_beyazPuan, siyah_taşsayısı, beyaz_taşsayısı, lbl_HamleSayısı, image_siyah, image_beyaz, ayar_9x9, ayar_13x13, ayar_19x19, çoklu_oyna_host, çoklu_oyna_bağlanan, ip, lbl_beyazOyuncuAdı, lbl_SiyahOyuncuAdı, lbl_byzSüre, lbl_syhSüre, btn_replay,varmı,sohbet,puan_kontrol));
        }

        #region Yanıp Söndür
        public void puan_kontrol_Tick(object sender, EventArgs e)
        {
            if (ms.Hamle_Puan_durumu() == true)
            {
                ms.oyunBittimi = true;
                MessageBox.Show("Geçmiş Olsun Oyun Bitti... Gene bekleriz...","Bilgilendirme");
                puan_kontrol.Stop();
                oyunu_kapat();
            }
            else
                ms.oyunBittimi = false;
        }
        #endregion

        int sn = 0;
        int dk = 0;

        public void oyunu_kapat()
        {
            
            if (çoklu_oyna_bağlanan == true && çoklu_oyna_host == false)
            {
                sohbet.BilgiGönder("OyunBitti");
                sohbet.İstemciKapat();
            }
            else if (çoklu_oyna_bağlanan == false && çoklu_oyna_host == true)
            {
                sohbet.BilgiGönder("OyunBitti");
                sohbet.SunucuKapat();
            }
            Close();
        }

        private void btn_pesEt_Click(object sender, RoutedEventArgs e)
        {
            //if (MessageBox.Show("Oyun kaydedilsin mi?", "Uyarı!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            //{
            //    SqlConnection con = new SqlConnection("server=.;Integrated Security=SSPI;database=go_Game;");
            //    con.Open();
            //    SqlCommand com = new SqlCommand("TRUNCATE TABLE go_Tablo", con);
            //    com.ExecuteNonQuery();
            //    con.Close();

            //    Close();
            //}
        }

        private void btn_gönder_Click(object sender, RoutedEventArgs e)
        {
            string ad = "";
            if (çoklu_oyna_host == true && çoklu_oyna_bağlanan == false)
            {
                ad = lbl_SiyahOyuncuAdı.Content.ToString();
            }
            else if (çoklu_oyna_host == false && çoklu_oyna_bağlanan == true)
            {
                ad = lbl_beyazOyuncuAdı.Content.ToString();
            }
            richTextBox1.Text += ad + " yazı : " + GetString(richTextBox2);
            ms.yeni_sohbet.BilgiGönder("SohbetGelenYazi|" + ad + "|" + GetString(richTextBox2));
            FlowDocument myFlowDoc = new FlowDocument();
            myFlowDoc.Blocks.Add(new Paragraph(new Run("")));
            richTextBox2.Document = myFlowDoc;
        }

        private void btn_sohbetiKapat_Click(object sender, RoutedEventArgs e)
        {
            if (btn_sohbetiKapat.Content.ToString() == "Sohbeti Kapat")
            {
                OyunAlanı.Height = 684;
                OyunAlanı.Width = 770;
                btn_sohbetiKapat.Content = "Sohbeti Aç";
            }

            else if (btn_sohbetiKapat.Content.ToString() == "Sohbeti Aç")
            {
                OyunAlanı.Width = 1102;
                OyunAlanı.Height = 684;
                btn_sohbetiKapat.Content = "Sohbeti Kapat";
            }
        }

        string GetString(RichTextBox rtb)
        {
            var textRange = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            return textRange.Text;
        }

        public void rakipOyuncuAdı(string mesaj)
        {
            string[] komut = mesaj.Split('|');
            if (komut[0] == "host")
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    Masa.masa.LBL_oyuncu1.Content = komut[1];
                }));
            }
            else
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    Masa.masa.LBL_oyuncu2.Content = komut[1];
                }));
            }
        }

        public void sohbetMesaj(string mesaj)
        {
            string[] komut = mesaj.Split('|');
            this.Dispatcher.Invoke((Action)(() =>
            {
                richTextBox1.Text += komut[1] + " yazı : " + komut[2];
                FlowDocument myFlowDoc = new FlowDocument();
                myFlowDoc.Blocks.Add(new Paragraph(new Run(komut[1])));
                myFlowDoc.Blocks.Add(new Paragraph(new Run("    " + komut[2])));
            }));
        }

        public void taş_koymak(string mesaj)
        {
            string[] komut = mesaj.Split('|');
            int x = int.Parse(komut[1]), y = int.Parse(komut[2]);
            for (int i = 0; i < Masa.masa.tahta_üzerindeki_taş.Count; i++)
            {
                if (Masa.masa.tahta_üzerindeki_taş[i].Tag != null)
                {
                    if ((komut[1] + "," + komut[2]) == Masa.masa.tahta_üzerindeki_taş[i].Tag.ToString())
                    {
                        Oyun.oyun_penceresi.Dispatcher.Invoke((Action)(() =>
                        {
                            Masa.masa.taşı_koy(x, y, Masa.masa.tahta_üzerindeki_taş[i]);
                        }));
                        Masa.masa.stacpanelsırası = i;
                        Oyun.oyun_penceresi.Dispatcher.Invoke((Action)(() =>
                        {
                            Masa.masa.yanıpsöndür.Start();
                        }));
                        break;
                    }
                }
            }
        }

        private void OyunAlanı_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ms.oyunBittimi == false && ms.Hamle_Sayısı > 0)
            {
                if (çoklu_oyna_bağlanan == true && çoklu_oyna_host == false)
                    sohbet.İstemciKapat();
                else if (çoklu_oyna_host == true && çoklu_oyna_bağlanan == false)
                    sohbet.SunucuKapat();
            }
        }
    }
}
