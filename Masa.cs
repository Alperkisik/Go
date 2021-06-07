#region kütüphane
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Data.SqlClient;
using System.Net.Sockets;
using System.IO;
using System.Windows.Threading;
using System.Collections;
using System.Threading;
using System.Data.SqlServerCe;
#endregion

namespace Go
{
    class Ayarlar
    {
        public WrapPanel pnl1;
        public Grid grd;
        public Border bd_1;
        public Label p1;
        public Label p2;
        public Label s;
        public Label be;
        public Label ha;
        public Image sss;
        public Image bbb;
        public bool ayar_9x9;
        public bool ayar_13x13;
        public bool ayar_19x19;
        public bool coklu_oyna_hst;
        public bool coklu_oyna_baglann;
        public string ıp;
        public Label lbl_oyuncu2;
        public Label lbl_oyuncu1;
        public Label süre_byz;
        public Label süre_syh;
        public Button replay;
        public bool saatVarMı;
        public Sohbet sohbett;
        public DispatcherTimer kontrol;

        public Ayarlar(WrapPanel pnl1, Grid grd, Border bd_1, Label p1, Label p2, Label s, Label be, Label ha, Image sss, Image bbb, bool ayar_9x9, bool ayar_13x13, bool ayar_19x19, bool coklu_oyna_hst, bool coklu_oyna_baglann, string ıp, Label lbl_oyuncu2, Label lbl_oyuncu1, Label asd, Label qwe, Button replay, bool varmı,Sohbet sohbet,DispatcherTimer hjk)
        {
            this.pnl1 = pnl1;
            this.grd = grd;
            this.bd_1 = bd_1;
            this.p1 = p1;
            this.p2 = p2;
            this.s = s;
            this.be = be;
            this.ha = ha;
            this.sss = sss;
            this.bbb = bbb;
            this.ayar_9x9 = ayar_9x9;
            this.ayar_13x13 = ayar_13x13;
            this.ayar_19x19 = ayar_19x19;
            this.coklu_oyna_hst = coklu_oyna_hst;
            this.coklu_oyna_baglann = coklu_oyna_baglann;
            this.ıp = ıp;
            this.lbl_oyuncu2 = lbl_oyuncu2;
            this.lbl_oyuncu1 = lbl_oyuncu1;
            this.süre_byz = asd;
            this.süre_syh = qwe;
            this.replay = replay;
            this.saatVarMı = varmı;
            this.sohbett = sohbet;
            this.kontrol = hjk;
        }
    }

    class Masa
    {
        #region değişkenler
        public static Masa masa;
        public string Oyuncu1 = "Oyuncu1", Oyuncu2 = "Oyuncu2";
        public int B_Taş_Sayısı = 180;
        public int S_Taş_Sayısı = 181;
        public int Masanın_Boyutu = 18;
        public int Hamle_Sayısı = 0;
        private string hamle = "";
        public string sıra = "Siyah";
        private int puan1 = 0, puan2 = 0;
        public string[,] koordinat = new string[19, 19];
        private bool taş_yeme = false;
        private const string beyaz = "Beyaz", siyah = "Siyah";
        public int yeni_x = 100, yeni_y = 100;
        private int katsayı = 32;
        private int uzunluuk = 18;
        private int büyüklük = 30;
        private int boşluk_uzunluğu;
        public int stacpanelsırası;
        private int süre1 = 60, süre2 = 60;
        private string[] başlangıç_noktaları = new string[10];
        private string zaman;
        public bool saatVarmı = true;
        public bool oyunBittimi = false;
        #endregion

        #region nesneler
        public List<StackPanel> tahta_üzerindeki_taş = new List<StackPanel>();
        Label puan_siyah, puan_beyaz, lbl_s_tas, lbl_b_tas, lbl_sayı_hamle , süre_beyaz , süre_siyah;
        Image syh_resim, byz_resim;
        public DispatcherTimer s_siyah, s_beyaz;
        public Button btn_replay;
        #endregion

        #region çoklu_oyuncu değişkenleri
        private bool çoklu_oyna_host = false, çoklu_oyna_bağlanan = false;
        private string IP = "";
        private ArrayList Kayıtlı_IP_Adresleri = new ArrayList();
        public Sohbet yeni_sohbet;
        public DispatcherTimer yanıpsöndür;
        public Label LBL_oyuncu2, LBL_oyuncu1;
        public bool büyükmasa, ortamasa, küçükmasa;
        public string masa_B = "evet" , masa_O = "hayir", masa_K = "hayir";
        bool? sunucu = null;
        #endregion

        #region masanın oluşumu
        public Masa()
        {
            masa = this;
            puan1 = 0;
            puan2 = 0;
            Hamle_Sayısı = 0;
            sıra = siyah;
            yeni_x = yeni_y = 100;
            taş_yeme = false;
            hamle = "";
            B_Taş_Sayısı = 180;
            S_Taş_Sayısı = 181;
            for (int x = 0; x < 19; x++)
                for (int y = 0; y < 19; y++)
                    koordinat[x, y] = "";

            for (int z = 0; z < 10; z++)
                başlangıç_noktaları[z] = "";
        }

        public Ayarlar ayarlar;

        public void BağlantıBaşlat(Ayarlar ayarlar)
        {
            #region nesnelerin eşitlenmesi
            puan_siyah = ayarlar.p1;
            puan_beyaz = ayarlar.p2;
            lbl_s_tas = ayarlar.s;
            lbl_b_tas = ayarlar.be;
            lbl_sayı_hamle = ayarlar.ha;
            syh_resim = ayarlar.sss;
            byz_resim = ayarlar.bbb;
            byz_resim.Visibility = Visibility.Hidden;
            syh_resim.Visibility = Visibility.Visible;
            çoklu_oyna_host = ayarlar.coklu_oyna_hst;
            çoklu_oyna_bağlanan = ayarlar.coklu_oyna_baglann;
            IP = ayarlar.ıp;
            büyükmasa = ayarlar.ayar_19x19;
            ortamasa = ayarlar.ayar_13x13;
            küçükmasa = ayarlar.ayar_9x9;
            LBL_oyuncu2 = ayarlar.lbl_oyuncu2;
            LBL_oyuncu1 = ayarlar.lbl_oyuncu1;
            süre_beyaz = ayarlar.süre_byz;
            süre_siyah = ayarlar.süre_syh;
            btn_replay = ayarlar.replay;
            zaman = DateTime.Now.ToString();
            saatVarmı = ayarlar.saatVarMı;
            yeni_sohbet = ayarlar.sohbett;
            #endregion
            btn_replay.Click += new RoutedEventHandler(btn_replay_Click);
            this.ayarlar = ayarlar;
            if (çoklu_oyna_host == true && çoklu_oyna_bağlanan == false)
            {
                sunucu = true;
                int port = 10000;
                //yeni_sohbet = new Sohbet();
                yeni_sohbet.SunucuBaşlat(port);
            }
            else if (çoklu_oyna_host == false && çoklu_oyna_bağlanan == true)
            {
                sunucu = false;
                int port = 10000;
                //yeni_sohbet = new Sohbet();
                yeni_sohbet.İstemciBaşlat(IP, port);
            }
            else
                Masayı_Oluştur();
        }

        void btn_replay_Click(object sender, RoutedEventArgs e)
        {
            süre1 = 60; süre2 = 60;
            s_siyah.Stop();
            s_beyaz.Stop();
            masa = this;
            Replay rp = new Replay();
            rp.ShowDialog();
        }

        public void Masayı_Oluştur()
        {
            WrapPanel pnl1 = ayarlar.pnl1;
            Grid grd = ayarlar.grd;
            Border bd_1 = ayarlar.bd_1;

            if (büyükmasa == true && küçükmasa == false && ortamasa == false)
            {
                masa_B = "evet";
                masa_O = "hayir";
                masa_K = "hayir";
            }
            else if (ortamasa == true && küçükmasa == false && büyükmasa == false)
            {
                masa_O = "evet";
                masa_B = "hayir";
                masa_K = "hayir";
            }
            else if (küçükmasa == true && ortamasa == false && büyükmasa == false)
            {
                masa_K = "evet";
                masa_B = "hayir";
                masa_O = "hayir";
            }

            #region çoklu oyun
            if (sunucu == true)
            {
                if(masa_K ==  "evet" && masa_B == "hayir" && masa_O == "hayir")
                    yeni_sohbet.BilgiGönder("MasaBoyu|Kucuk|host|" + LBL_oyuncu1.Content);
                else if (masa_K == "hayir" && masa_B == "hayir" && masa_O == "evet")
                    yeni_sohbet.BilgiGönder("MasaBoyu|Orta|host|" + LBL_oyuncu1.Content);
                else if (masa_K == "hayir" && masa_B == "evet" && masa_O == "hayir")
                    yeni_sohbet.BilgiGönder("MasaBoyu|Buyuk|host|" + LBL_oyuncu1.Content);
            }
            else if (sunucu == false)
            {
                yeni_sohbet.BilgiGönder("RakipOyuncu|bağlanan|" + LBL_oyuncu2.Content);
            }
            #endregion

            #region Timer
            yanıpsöndür = new DispatcherTimer();
            yanıpsöndür.Tick += new EventHandler(yanıpsöndür_Tick);
            yanıpsöndür.Interval = new TimeSpan(0, 0, 1);

            if (saatVarmı == true)
            {
                s_siyah = new DispatcherTimer();
                s_siyah.Tick += new EventHandler(süre_siyah_Tick);
                s_siyah.Interval = new TimeSpan(0, 0, 1);
                s_siyah.Start();

                s_beyaz = new DispatcherTimer();
                s_beyaz.Tick += new EventHandler(süre_beyaz_Tick);
                s_beyaz.Interval = new TimeSpan(0, 0, 1);


            }
            else
            {
                süre_siyah.Visibility = Visibility.Hidden;
                süre_beyaz.Visibility = Visibility.Hidden;
            }
            #endregion

            #region ifler

            int kutu_sayısı = 18;

            if (kutu_sayısı == 18)
                bd_1.Height = bd_1.Width = 578;

            if (küçükmasa == true)
            {
                kutu_sayısı = 8;
                uzunluuk = 8;
                bd_1.Height = bd_1.Width = 578;
                katsayı = 578 / kutu_sayısı;
                büyüklük = 50;
                S_Taş_Sayısı = 41;
                B_Taş_Sayısı = 40;
                #region başlandıç noktaları
                başlangıç_noktaları[0] = "3,3";
                başlangıç_noktaları[1] = "5,3";
                başlangıç_noktaları[2] = "3,5";
                başlangıç_noktaları[3] = "5,5";
                #endregion
            }
            else if (ortamasa == true)
            {
                kutu_sayısı = 12;
                uzunluuk = 12;
                bd_1.Height = bd_1.Width = 578;
                katsayı = 578 / kutu_sayısı;
                büyüklük = 40;
                S_Taş_Sayısı = 85;
                B_Taş_Sayısı = 84;
                #region başlandıç noktaları
                başlangıç_noktaları[0] = "3,3";
                başlangıç_noktaları[1] = "9,3";
                başlangıç_noktaları[2] = "6,6";
                başlangıç_noktaları[3] = "3,9";
                başlangıç_noktaları[4] = "9,9";
                #endregion
            }
            else if (büyükmasa == true)
            {
                kutu_sayısı = 18;
                uzunluuk = 18;
                bd_1.Height = bd_1.Width = 578;
                katsayı = 32;
                büyüklük = 30;
                #region başlandıç noktaları
                başlangıç_noktaları[0] = "3,3";
                başlangıç_noktaları[1] = "9,3";
                başlangıç_noktaları[2] = "15,3";
                başlangıç_noktaları[3] = "3,9";
                başlangıç_noktaları[4] = "9,9";
                başlangıç_noktaları[5] = "15,9";
                başlangıç_noktaları[6] = "3,15";
                başlangıç_noktaları[7] = "9,15";
                başlangıç_noktaları[8] = "15,15";
                #endregion
            }
            #endregion

            boşluk_uzunluğu = 578 / kutu_sayısı - 4;

            int genislik = 576 / kutu_sayısı;

            pnl1.Width = pnl1.Height = kutu_sayısı * genislik;

            #region ızgara bölümü
            for (int a = 0; a < kutu_sayısı; a++)
            {
                for (int b = 0; b < kutu_sayısı; b++)
                {
                    Grid kutu = new Grid();

                    kutu.Background = Brushes.Tan;
                    kutu.Width = kutu.Height = genislik;

                    Border cerceve = new Border();
                    cerceve.BorderThickness = new Thickness(2, 2, 0, 0);
                    cerceve.BorderBrush = Brushes.Maroon;

                    kutu.Children.Add(cerceve);

                    pnl1.Children.Add(kutu);
                }
            }
            #endregion

            #region ızagara köşelerine stackpanel eklenmesi
            for (int i = 0; i < 620 * 2; i += katsayı)
            {
                for (int y = 0; y < 620 * 2; y += katsayı)
                {
                    StackPanel taş_bölgesi = new StackPanel();
                    taş_bölgesi.Height = taş_bölgesi.Width = büyüklük;
                    taş_bölgesi.Tag = (i / katsayı).ToString() + "," + (y / katsayı).ToString();

                    #region Başlangıç Noktaları

                    Image baş_nokta = new Image();

                    if (küçükmasa == true)
                    {
                        if (taş_bölgesi.Tag.ToString() == "3,3" || taş_bölgesi.Tag.ToString() == "3,5" || taş_bölgesi.Tag.ToString() == "5,3" || taş_bölgesi.Tag.ToString() == "5,5")
                        {
                            baş_nokta.Source = ResimSeç("nokta");
                            baş_nokta.Stretch = Stretch.Fill;
                            baş_nokta.Height = baş_nokta.Width = 15;
                            baş_nokta.Margin = new Thickness(-2, baş_nokta.Width + 3, 0, 0);
                            taş_bölgesi.Children.Add(baş_nokta);
                        }
                    }

                    else if (ortamasa == true)
                    {
                        if (taş_bölgesi.Tag.ToString() == "3,3" || taş_bölgesi.Tag.ToString() == "3,9" || taş_bölgesi.Tag.ToString() == "6,6" || taş_bölgesi.Tag.ToString() == "9,3" || taş_bölgesi.Tag.ToString() == "9,9")
                        {
                            baş_nokta.Source = ResimSeç("nokta");
                            baş_nokta.Stretch = Stretch.Fill;
                            baş_nokta.Height = baş_nokta.Width = 12;
                            baş_nokta.Margin = new Thickness(0, baş_nokta.Width + 2, 0, 0);
                            taş_bölgesi.Children.Add(baş_nokta);
                        }
                    }

                    else if (büyükmasa == true)
                    {
                        if (taş_bölgesi.Tag.ToString() == "3,3" || taş_bölgesi.Tag.ToString() == "3,9" || taş_bölgesi.Tag.ToString() == "3,15" || taş_bölgesi.Tag.ToString() == "9,3" || taş_bölgesi.Tag.ToString() == "9,9" || taş_bölgesi.Tag.ToString() == "9,15" || taş_bölgesi.Tag.ToString() == "15,3" || taş_bölgesi.Tag.ToString() == "15,9" || taş_bölgesi.Tag.ToString() == "15,15")
                        {
                            baş_nokta.Source = ResimSeç("nokta");
                            baş_nokta.Stretch = Stretch.Fill;
                            baş_nokta.Height = baş_nokta.Width = 9;
                            baş_nokta.Margin = new Thickness(-1, baş_nokta.Width + 1, 0, 0);
                            taş_bölgesi.Children.Add(baş_nokta);
                        }
                    }

                    #endregion

                    taş_bölgesi.Background = Brushes.Transparent;
                    taş_bölgesi.Margin = new Thickness(-585 + i * 2, -585 + y * 2, 0, 0);
                    taş_bölgesi.MouseDown += new System.Windows.Input.MouseButtonEventHandler(taş_bölgesi_MouseDown);
                    taş_bölgesi.MouseEnter += new System.Windows.Input.MouseEventHandler(taş_bölgesi_MouseEnter);
                    taş_bölgesi.MouseLeave += new System.Windows.Input.MouseEventHandler(taş_bölgesi_MouseLeave);
                    tahta_üzerindeki_taş.Add(taş_bölgesi);
                    grd.Children.Add(taş_bölgesi);


                }
            }
            #endregion

            ayarlar.kontrol.Start();
        }
        #endregion

        #region fare hareketleri
        void taş_bölgesi_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            StackPanel sp = (sender as StackPanel);
            sp.Background = Brushes.Transparent;
        }

        void taş_bölgesi_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
             StackPanel sp = (sender as StackPanel);
            if (oyunBittimi == false)
            {
               
                #region tıklanan bölgenin koordinatlarını hesaplama
                string x = "", y = "", kelime;
                bool virgülmü = false;
                kelime = sp.Tag.ToString();
                for (int i = 0; i < kelime.Length; i++)
                {
                    if (kelime[i] != ',')
                        if (virgülmü)
                            y += kelime[i];
                        else
                            x += kelime[i];
                    else if (kelime[i] == ',')
                        virgülmü = true;
                }
                int say1, say2;
                say1 = int.Parse(x);
                say2 = int.Parse(y);
                #endregion

                if (tıklanan_yerde_taş_varmı(say1, say2))
                    sp.Background = Brushes.Transparent;
                else if (iki_taş_ortasındamı(say1, say2))
                    sp.Background = Brushes.Transparent;
                else if (üç_taş_ortasındamı(say1, say2))
                    sp.Background = Brushes.Transparent;
                else if (dört_taş_ortasımı(say1, say2))
                    sp.Background = Brushes.Transparent;
                else if (çoklu_oyna_bağlanan != false || çoklu_oyna_host != false)
                {
                    if (çoklu_oyna_bağlanan == true && Oyun.oyun_penceresi.image_beyaz.Visibility == Visibility.Hidden)
                        sp.Background = Brushes.Transparent;
                    else if (çoklu_oyna_host == true && Oyun.oyun_penceresi.image_siyah.Visibility == Visibility.Hidden)
                        sp.Background = Brushes.Transparent;
                    else
                        sp.Background = Brushes.Aqua;
                }
                else
                    sp.Background = Brushes.Aqua;
            }
            else
                sp.Background = Brushes.Transparent;
        }
        #endregion

        #region taş koyulması gereken yerlere tıklama olayı
        void taş_bölgesi_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            StackPanel sp = (sender as StackPanel);
            if (çoklu_oyna_bağlanan != false || çoklu_oyna_host != false)
            {
                if (çoklu_oyna_host == true && Oyun.oyun_penceresi.image_siyah.Visibility == Visibility.Visible)
                    tıklama_olayı(sp);
                else if (çoklu_oyna_bağlanan == true && Oyun.oyun_penceresi.image_beyaz.Visibility == Visibility.Visible)
                {
                    tıklama_olayı(sp);
                }
            }
            else
                tıklama_olayı(sp);  
        }

        public void tıklama_olayı(StackPanel sp)
        {
            #region tıklanan bölgenin koordinatlarını hesaplama
            string x = "", y = "", kelime;
            bool virgülmü = false;
            kelime = sp.Tag.ToString();
            for (int i = 0; i < kelime.Length; i++)
            {
                if (kelime[i] != ',')
                    if (virgülmü)
                        y += kelime[i];
                    else
                        x += kelime[i];
                else if (kelime[i] == ',')
                    virgülmü = true;
            }
            int say1, say2;
            say1 = int.Parse(x);
            say2 = int.Parse(y);
            #endregion
            if (oyunBittimi == false)
            {
                if (tıklanan_yerde_taş_varmı(say1, say2))
                    MessageBox.Show("Tıkladığınız yerde taş var lütfen başka yere tıklayın hiç değilse deneyin...", "Bilgilendirme");
                else if (iki_taş_ortasındamı(say1, say2))
                    MessageBox.Show("2 taş arasına taş koyamazsınız...", "Bilgilendirme");
                else if (üç_taş_ortasındamı(say1, say2))
                    MessageBox.Show("3 tane taşın ortasına taş koyamazsınız...", "Bilgilendirme");
                else if (dört_taş_ortasımı(say1, say2))
                    MessageBox.Show("4 tane taşın ortasına taş koyamazsınız...", "Bilgilendirme");
                else
                {
                    taşı_koy(say1, say2, sp);
                    #region host , bağlanan
                    if (çoklu_oyna_host == true && çoklu_oyna_bağlanan == false)
                    {
                        yeni_sohbet.BilgiGönder("Koordinat|" + say1.ToString() + "|" + say2.ToString());
                    }
                    else if (çoklu_oyna_bağlanan == true && çoklu_oyna_host == false)
                    {
                        yeni_sohbet.BilgiGönder("Koordinat|" + say1.ToString() + "|" + say2.ToString());
                    }
                    #endregion
                }
            }
        }
        #endregion

        #region kod

        #region alanın ortasındamı kontrolü
        private bool tıklanan_yerde_taş_varmı(int x, int y)
        {
            bool değer = false;
            try
            {
                
                if (koordinat[x, y] != "")
                    değer = true;
                else
                    değer = false;
            }
            catch
            {
            }
            return değer;
        }

        private bool iki_taş_ortasındamı(int x, int y)
        {
            bool değer = false;
            string a, sa;
            #region sol köşe
            if (x == 0 && y == 0)
            {
                a = koordinat[0, 1];
                sa = koordinat[1, 0];
                #region ifler
                if (sıra == beyaz)
                {
                    if (a == siyah && sa == siyah)
                        değer = true;
                    else
                        değer = false;
                }
                else if (sıra == siyah)
                {
                    if (a == beyaz && sa == beyaz)
                        değer = true;
                    else
                        değer = false;
                }
                #endregion
            }
            #endregion
            #region sağ alt
            else if (x == uzunluuk && y == uzunluuk)
            {
                a = koordinat[uzunluuk, uzunluuk - 1];
                sa = koordinat[uzunluuk - 1, uzunluuk];
                #region ifler
                if (sıra == beyaz)
                {
                    if (a == siyah && sa == siyah)
                        değer = true;
                    else
                        değer = false;
                }
                else if (sıra == siyah)
                {
                    if (a == beyaz && sa == beyaz)
                        değer = true;
                    else
                        değer = false;
                }
                #endregion
            }
            #endregion
            #region sağ üst
            else if (x == uzunluuk && y == 0)
            {
                a = koordinat[uzunluuk, 1];
                sa = koordinat[uzunluuk - 1, 0];
                #region ifler
                if (sıra == beyaz)
                {
                    if (a == siyah && sa == siyah)
                        değer = true;
                    else
                        değer = false;
                }
                else if (sıra == siyah)
                {
                    if (a == beyaz && sa == beyaz)
                        değer = true;
                    else
                        değer = false;
                }
                #endregion
            }
            #endregion
            #region sol alt
            else if (x == 0 && y == uzunluuk)
            {
                a = koordinat[0, uzunluuk - 1];
                sa = koordinat[1, uzunluuk];
                #region ifler
                if (sıra == beyaz)
                {
                    if (a == siyah && sa == siyah)
                        değer = true;
                    else
                        değer = false;
                }
                else if (sıra == siyah)
                {
                    if (a == beyaz && sa == beyaz)
                        değer = true;
                    else
                        değer = false;
                }
                #endregion
            }
            #endregion
            return değer;
        }

        private bool üç_taş_ortasındamı(int x, int y)
        {
            bool değer = false;
            string n1, n2, n3;
            try
            {
                if (x == 0)
                {
                    #region içerik
                    if (sıra == beyaz)
                    {
                        n1 = koordinat[0, y + 1]; n2 = koordinat[0, y - 1];
                        n3 = koordinat[1, y];
                        if (n1 == siyah && n2 == siyah && n3 == siyah)
                        {
                            değer = true;
                        }
                    }
                    else if (sıra == siyah)
                    {
                        n1 = koordinat[0, y + 1]; n2 = koordinat[0, y - 1];
                        n3 = koordinat[1, y];
                        if (n1 == beyaz && n2 == beyaz && n3 == beyaz)
                        {
                            değer = true;
                        }
                    }
                    #endregion
                }
                else if (y == 0)
                {
                    #region içerik
                    if (sıra == beyaz)
                    {
                        n1 = koordinat[x + 1, 0]; n2 = koordinat[x - 1, 0];
                        n3 = koordinat[x, 1];
                        if (n1 == siyah && n2 == siyah && n3 == siyah)
                        {
                            değer = true;
                        }
                    }
                    else if (sıra == siyah)
                    {
                        n1 = koordinat[x + 1, 0]; n2 = koordinat[x - 1, 0];
                        n3 = koordinat[x, 1];
                        if (n1 == beyaz && n2 == beyaz && n3 == beyaz)
                        {
                            değer = true;
                        }
                    }
                    #endregion
                }
                else if (y == uzunluuk)
                {
                    #region içerik
                    if (sıra == beyaz)
                    {
                        n1 = koordinat[x + 1, uzunluuk]; n2 = koordinat[x - 1, uzunluuk];
                        n3 = koordinat[x, uzunluuk - 1];
                        if (n1 == siyah && n2 == siyah && n3 == siyah)
                        {
                            değer = true;
                        }
                    }
                    else if (sıra == siyah)
                    {
                        n1 = koordinat[x + 1, 18]; n2 = koordinat[x - 1, 18];
                        n3 = koordinat[x, 17];
                        if (n1 == beyaz && n2 == beyaz && n3 == beyaz)
                        {
                            değer = true;
                        }
                    }
                    #endregion
                }
                else if (x == uzunluuk)
                {
                    #region içerik
                    if (sıra == beyaz)
                    {
                        n1 = koordinat[uzunluuk, y + 1]; n2 = koordinat[uzunluuk, y - 1];
                        n3 = koordinat[uzunluuk - 1, y];
                        if (n1 == siyah && n2 == siyah && n3 == siyah)
                        {
                            değer = true;
                        }
                    }
                    else if (sıra == siyah)
                    {
                        n1 = koordinat[uzunluuk, y + 1]; n2 = koordinat[uzunluuk, y - 1];
                        n3 = koordinat[uzunluuk - 1, y];
                        if (n1 == beyaz && n2 == beyaz && n3 == beyaz)
                        {
                            değer = true;
                        }
                    }
                    #endregion
                }
            }
            catch
            {
                değer = false;
            }
            return değer;
        }

        private bool dört_taş_ortasımı(int x, int y)
        {
            bool değer = false;
            string yu, a, sa, so;
            if (x != 0 && x != uzunluuk && y != 0 && y != uzunluuk)
            {
                yu = koordinat[x, y - 1]; a = koordinat[x, y + 1];
                sa = koordinat[x + 1, y]; so = koordinat[x - 1, y];
                if (sıra == siyah)
                {
                    if (yu == beyaz && sa == beyaz && so == beyaz && a == beyaz)
                    {
                        değer = true;
                    }
                    else
                        değer = false;
                }
                else if (sıra == beyaz)
                {
                    if (yu == siyah && sa == siyah && so == siyah && a == siyah)
                    {
                        değer = true;
                    }
                    else
                        değer = false;
                }
            }
            return değer;
        }

        private bool noktaya_git(int x, int y)
        {
            bool değer = false;
            if (koordinat[x, y] == sıra)
                değer = true;
            else
                değer = false;
            return değer;
        }
        #endregion

        #region taşın koyulması
        int döngü1 = 0;
        public void taşı_koy(int x, int y, StackPanel sp)
        {
            if (sp.Children.Count > 0)
                sp.Children.Clear();
            if (Hamle_Sayısı == 0)
            {
                #region içerik
                string nokta = x.ToString() + "," + y.ToString();
                for (döngü1 = 0; döngü1 < 10; döngü1++)
                {
                    if (nokta == başlangıç_noktaları[döngü1])
                    {
                        koordinat[x, y] = sıra;
                        #region resim ekleme
                        Image Resim = new Image();

                        Resim.Height = Resim.Width = büyüklük;
                        if (sıra == siyah)
                            Resim.Source = ResimSeç(siyah);
                        else if (sıra == beyaz)
                            Resim.Source = ResimSeç(beyaz);
                        sp.Children.Add(Resim);
                        #endregion
                        Puan_Hesapla();
                        hamleyi_yaz(x, y);
                        lbl_sayı_hamle.Content = "Hamle Sayısı = " + Hamle_Sayısı.ToString();
                        break;
                    }
                }
                if (döngü1 == 10)
                {
                    MessageBox.Show("Lütfen başlangıç noktasından başlayın yada deneyin...", "Bilgilendirme");
                }
                #endregion
            }
            else
            {
                koordinat[x, y] = sıra;

                #region resim ekleme
                Image Resim = new Image();

                Resim.Height = Resim.Width = büyüklük;
                if (sıra == siyah)
                    Resim.Source = ResimSeç(siyah);
                else if (sıra == beyaz)
                    Resim.Source = ResimSeç(beyaz);
                sp.Children.Add(Resim);
                #endregion

                #region taş durumları
                if (iki_taş_durumumu_var(x, y, sıra))
                    taşı_ye(yeni_x, yeni_y);
                if (üç_taş_durumumu_var(x, y, sıra))
                    taşı_ye(yeni_x, yeni_y);
                alan_varmı(x, y);
                dört_taş_yeme_durumumu_var(x, y, sıra);
                #endregion

                Puan_Hesapla();
                hamleyi_yaz(x, y);
            }
            for (int i = 0; i < tahta_üzerindeki_taş.Count; i++)
            {
                tahta_üzerindeki_taş[i].Background = Brushes.Transparent;
            }  
        }
        #endregion

        #region sırayı belirlemek
        public void sırayı_belirle()
        {
            Oyun oyun = new Oyun();
            #region sıra belirleme
            if (sıra == beyaz)
            {
                byz_resim.Visibility = Visibility.Hidden;
                syh_resim.Visibility = Visibility.Visible;

                if (çoklu_oyna_bağlanan == false && çoklu_oyna_host == false)
                {
                    try
                    {
                        süre_beyaz.Visibility = Visibility.Hidden;
                        süre_siyah.Visibility = Visibility.Visible;


                        süre_siyah.Content = "Kalan Süre: 01:00";
                        s_siyah.Start(); s_beyaz.Stop();
                        süre1 = 60; süre2 = 0;
                    }
                    catch
                    {
                        süre_siyah.Visibility = Visibility.Hidden;
                        süre_beyaz.Visibility = Visibility.Hidden;
                    }
                }
            }
            else
            {
                byz_resim.Visibility = Visibility.Visible;
                syh_resim.Visibility = Visibility.Hidden;

                if (çoklu_oyna_bağlanan == false && çoklu_oyna_host == false)
                {
                    try
                    {
                        süre_beyaz.Visibility = Visibility.Visible;
                        süre_siyah.Visibility = Visibility.Hidden;

                        süre_beyaz.Content = "Kalan Süre: 01:00";
                        s_siyah.Stop(); s_beyaz.Start();
                        süre1 = 0; süre2 = 60;
                    }
                    catch
                    {
                        süre_siyah.Visibility = Visibility.Hidden;
                        süre_beyaz.Visibility = Visibility.Hidden;
                    }
                }
            }
            #endregion

            if (sıra == beyaz)
                sıra = siyah;
            else if (sıra == siyah)
                sıra = beyaz;
        }
        #endregion

        #region resim ekleme kısmı
        private BitmapImage ResimSeç(string resim)
        {
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri("/Go;component/Images/" + resim + ".png", UriKind.RelativeOrAbsolute);
            src.EndInit();
            return src;
        }
        #endregion

        #region taş yeme durumları ve puan
        public bool iki_taş_durumumu_var(int x, int y, string s)
        {
            bool değer = false;
            try
            {
                string n1;
                //tamamlandı
                #region sol üst köşe durumu
                if (x == 0 && y == 1)
                {
                    #region içerik
                    n1 = koordinat[1, 0];
                    if (n1 == s && koordinat[0, 0] != s)
                    {
                        taş_yeme = true; yeni_x = 0; yeni_y = 0;
                        değer = true;
                    }
                    else
                    {
                        taş_yeme = false; değer = false;
                    }
                    #endregion
                }
                else if (x == 1 && y == 0)
                {
                    #region içerik
                    n1 = koordinat[0, 1];
                    if (n1 == s && koordinat[0, 0] != s)
                    {
                        taş_yeme = true; yeni_x = 0; yeni_y = 0;
                        değer = true;
                    }
                    else
                    {
                        taş_yeme = false; değer = false;
                    }
                    #endregion
                }
                #endregion
                #region sağ üst köşe durumu
                else if (x == uzunluuk - 1 && y == 0)
                {
                    #region içerik
                    n1 = koordinat[uzunluuk, 1];
                    if (n1 == s && koordinat[uzunluuk, 0] != s)
                    {
                        taş_yeme = true; yeni_x = uzunluuk; yeni_y = 0; değer = true;
                    }
                    else
                    {
                        taş_yeme = false; değer = false;
                    }
                    #endregion
                }
                else if (x == uzunluuk && y == 1)
                {
                    #region içerik
                    n1 = koordinat[uzunluuk - 1, 0];
                    if (n1 == s && koordinat[uzunluuk, 0] != s)
                    {
                        taş_yeme = true; yeni_x = uzunluuk; yeni_y = 0; değer = true;
                    }
                    else
                    {
                        taş_yeme = false; değer = false;
                    }
                    #endregion
                }
                #endregion
                #region sol alt köşe durumu
                else if (x == 1 && y == uzunluuk)
                {
                    #region içerik
                    n1 = koordinat[0, uzunluuk - 1];
                    if (n1 == s && koordinat[0, uzunluuk] != s)
                    {
                        taş_yeme = true; yeni_x = 0; yeni_y = uzunluuk; değer = true;
                    }
                    else
                    {
                        taş_yeme = false; değer = false;
                    }
                    #endregion
                }
                else if (x == 0 && y == uzunluuk - 1)
                {
                    #region içerik
                    n1 = koordinat[1, uzunluuk];
                    if (n1 == s && koordinat[0, uzunluuk] != s)
                    {
                        taş_yeme = true; yeni_x = 0; yeni_y = uzunluuk; değer = true;
                    }
                    else
                    {
                        taş_yeme = false; değer = false;
                    }
                    #endregion
                }
                #endregion
                #region sağ alt köşe durumu
                else if (x == uzunluuk && y == uzunluuk - 1)
                {
                    #region içerik
                    n1 = koordinat[uzunluuk - 1, uzunluuk];
                    if (n1 == s && koordinat[uzunluuk, uzunluuk] != s)
                    {
                        taş_yeme = true; yeni_x = uzunluuk; yeni_y = uzunluuk; değer = true;
                    }
                    else
                    {
                        taş_yeme = false; değer = false;
                    }
                    #endregion
                }
                else if (x == uzunluuk - 1 && y == uzunluuk)
                {
                    #region içerik
                    n1 = koordinat[uzunluuk, uzunluuk - 1];
                    if (n1 == s && koordinat[uzunluuk, uzunluuk] != s)
                    {
                        taş_yeme = true; yeni_x = uzunluuk; yeni_y = uzunluuk; değer = true;
                    }
                    else
                    {
                        taş_yeme = false; değer = false;
                    }
                    #endregion
                }
                #endregion
            }
            catch
            {
                değer = false; taş_yeme = false;
            }
            return değer;
        }

        public bool üç_taş_durumumu_var(int x, int y, string s)
        {
            bool değer = false;
            try
            {
                string n1, n2;
                string n3, n4;
                //tamamlandı
                #region 3 taş durumları
                if (x == 0)
                {
                    //tamamlandı
                    #region sol üst köşeye tıklandığındaki 3 taş durumu
                    if (x == 0 && y == 0)
                    {
                        n1 = koordinat[1, 1]; n2 = koordinat[2, 0];
                        if (n1 == s && n2 == s && koordinat[1, 0] != s)
                        {
                            taş_yeme = true; yeni_x = 1; yeni_y = 0; değer = true;
                        }
                        else
                        {
                            #region 2.durum
                            n1 = koordinat[1, 1]; n2 = koordinat[0, 2];
                            if (n1 == s && n2 == s && koordinat[0, 1] != s)
                            {
                                taş_yeme = true; yeni_x = 0; yeni_y = 1; değer = true;
                            }
                            #endregion
                            else
                            {
                                taş_yeme = false; değer = false;
                            }
                        }
                    }
                    #endregion
                    //tamamlandı
                    #region sol alt köşeye tıklandığındaki 3 taş durumu
                    else if (x == 0 && y == uzunluuk)
                    {
                        n1 = koordinat[1, uzunluuk - 1]; n2 = koordinat[2, uzunluuk];
                        if (n1 == s && n2 == s && koordinat[1, uzunluuk] != s)
                        {
                            taş_yeme = true; yeni_x = 1; yeni_y = uzunluuk; değer = true;
                        }
                        else
                        {
                            #region 2.durum
                            n1 = koordinat[1, uzunluuk - 1]; n2 = koordinat[0, uzunluuk - 2];
                            if (n1 == s && n2 == s && koordinat[0, uzunluuk - 1] != s)
                            {
                                taş_yeme = true; yeni_x = 0; yeni_y = uzunluuk - 1; değer = true;
                            }
                            #endregion
                            else
                            {
                                taş_yeme = false; değer = false;
                            }
                        }
                    }
                    #endregion
                    //tamamlandı
                    #region sol 0.sütun
                    else
                    {
                        n1 = koordinat[0, y + 2]; n2 = koordinat[x + 1, y + 1];
                        n3 = koordinat[x + 1, y - 1]; n4 = koordinat[0, y - 2];

                        if (n3 == s && n4 == s && koordinat[0, y - 1] != s)
                        {
                            taş_yeme = true; yeni_x = 0; yeni_y = y - 1; değer = true;
                        }
                        else if (n1 == s && n2 == s && koordinat[0, y + 1] != s)
                        {
                            taş_yeme = true; yeni_x = 0; yeni_y = y + 1; değer = true;
                        }
                        else
                        {
                            taş_yeme = false; değer = false;
                        }
                    }
                    #endregion
                }
                else if (x == 1)
                {
                    //tamamlandı
                    #region sol 1.sütun
                    n1 = koordinat[0, y - 1]; n2 = koordinat[0, y + 1];
                    if (n1 == s && n2 == s && koordinat[0, y] != s)
                    {
                        taş_yeme = true; değer = true;
                        yeni_x = 0; yeni_y = y;
                    }
                    else
                    {
                        taş_yeme = false; değer = false;
                    }
                    #endregion
                }
                else if (x == uzunluuk - 1)
                {
                    //tamamlandı
                    #region sağ uzunluuk-1.sütun
                    n1 = koordinat[uzunluuk, y + 1]; n2 = koordinat[uzunluuk, y - 1];
                    if (n1 == s && n2 == s && koordinat[uzunluuk, y] != s)
                    {
                        taş_yeme = true; değer = true;
                        yeni_x = uzunluuk; yeni_y = y;
                    }
                    else
                    {
                        taş_yeme = false; değer = false;
                    }
                    #endregion
                }
                else if (x == uzunluuk)
                {
                    //tamamlandı
                    #region sağ üst köşeye tıklandığındaki 3 taş durumu
                    if (x == uzunluuk && y == 0)
                    {
                        n1 = koordinat[uzunluuk - 1, 1]; n2 = koordinat[uzunluuk - 2, 0];
                        if (n1 == s && n2 == s && koordinat[uzunluuk - 1, 0] != s)
                        {
                            taş_yeme = true; yeni_x = uzunluuk - 1; yeni_y = 0; değer = true;
                        }
                        else
                        {
                            #region 2.durum
                            n1 = koordinat[uzunluuk - 1, 1]; n2 = koordinat[uzunluuk, 2];
                            if (n1 == s && n2 == s && koordinat[uzunluuk, 1] != s)
                            {
                                taş_yeme = true; yeni_x = uzunluuk; yeni_y = 1; değer = true;
                            }
                            #endregion
                            else
                            {
                                taş_yeme = false; değer = false;
                            }
                        }
                    }
                    #endregion
                    //tamamlandı
                    #region sağ alt köşeye tıklandığındaki 3 taş durumu
                    if (x == uzunluuk && y == uzunluuk)
                    {
                        n1 = koordinat[uzunluuk - 1, uzunluuk - 1]; n2 = koordinat[uzunluuk - 2, uzunluuk];
                        if (n1 == s && n2 == s && koordinat[uzunluuk - 1, uzunluuk] != s)
                        {
                            taş_yeme = true; yeni_x = uzunluuk - 1; yeni_y = uzunluuk; değer = true;
                        }
                        else
                        {
                            #region 2.durum
                            n1 = koordinat[uzunluuk - 1, uzunluuk - 1]; n2 = koordinat[uzunluuk, uzunluuk - 2];
                            if (n1 == s && n2 == s && koordinat[uzunluuk, uzunluuk - 1] != s)
                            {
                                taş_yeme = true; yeni_x = uzunluuk; yeni_y = uzunluuk - 1; değer = true;
                            }
                            #endregion
                            else
                            {
                                taş_yeme = false; değer = false;
                            }
                        }
                    }
                    #endregion
                    //tamamlandı
                    #region sağ uzunluuk.sütun
                    else
                    {
                        n1 = koordinat[uzunluuk, y - 2]; n2 = koordinat[x - 1, y - 1];
                        n3 = koordinat[uzunluuk, y + 2]; n4 = koordinat[x - 1, y + 1];

                        if (n3 == s && n4 == s && koordinat[uzunluuk, y + 1] != s)
                        {
                            taş_yeme = true; yeni_x = uzunluuk; yeni_y = y + 1; değer = true;
                        }
                        else if (n1 == s && n2 == s && koordinat[uzunluuk, y - 1] != s)
                        {
                            taş_yeme = true; yeni_x = uzunluuk; yeni_y = y - 1; değer = true;
                        }
                        else
                        {
                            taş_yeme = false; değer = false;
                        }
                    }
                    #endregion
                }
                else if (y == 0)
                {
                    //tamamlandı
                    #region üst 0.satır
                    n1 = koordinat[x - 2, 0]; n2 = koordinat[x - 1, y + 1];
                    n3 = koordinat[x + 2, 0]; n4 = koordinat[x + 1, y + 1];

                    if (n3 == s && n4 == s && koordinat[x + 1, 0] != s)
                    {
                        taş_yeme = true; yeni_x = x + 1; yeni_y = 0; değer = true;
                    }
                    else if (n1 == s && n2 == s && koordinat[x - 1, 0] != s)
                    {
                        taş_yeme = true; yeni_x = x - 1; yeni_y = 0; değer = true;
                    }
                    else
                    {
                        taş_yeme = false; değer = false;
                    }
                    #endregion
                }
                else if (y == 1)
                {
                    //tamamlandı
                    #region üst 1.satır
                    n1 = koordinat[x - 1, 0]; n2 = koordinat[x + 1, 0];
                    if (n1 == s && n2 == s && koordinat[x, 0] != s)
                    {
                        taş_yeme = true; değer = true;
                        yeni_x = x; yeni_y = 0;
                    }
                    else
                    {
                        taş_yeme = false; değer = false;
                    }
                    #endregion
                }
                else if (y == uzunluuk - 1)
                {
                    //tamamlandı
                    #region alt uzunluuk-1.satır
                    n1 = koordinat[x - 1, uzunluuk]; n2 = koordinat[x + 1, uzunluuk];
                    if (n1 == s && n2 == s && koordinat[x, uzunluuk] != s)
                    {
                        taş_yeme = true; değer = true;
                        yeni_x = x; yeni_y = uzunluuk;
                    }
                    else
                    {
                        taş_yeme = false; değer = false;
                    }
                    #endregion
                }
                else if (y == uzunluuk)
                {
                    //tamamlandı
                    #region alt uzunluuk.satır
                    n1 = koordinat[x - 2, uzunluuk]; n2 = koordinat[x - 1, y - 1];
                    n3 = koordinat[x + 2, uzunluuk]; n4 = koordinat[x + 1, y - 1];

                    if (n3 == s && n4 == s && koordinat[x + 1, uzunluuk] != s)
                    {
                        taş_yeme = true; yeni_x = x + 1; yeni_y = uzunluuk; değer = true;
                    }
                    else if (n1 == s && n2 == s && koordinat[x - 1, uzunluuk] != s)
                    {
                        taş_yeme = true; yeni_x = x - 1; yeni_y = uzunluuk; değer = true;
                    }
                    else
                    {
                        taş_yeme = false; değer = false;
                    }
                    #endregion
                }
                #endregion
            }
            catch
            {
                değer = false; taş_yeme = false;
            }
            return değer;
        }

        public void dört_taş_yeme_durumumu_var(int x, int y, string s)
        {
            try
            {
                if (s == beyaz)
                {
                    #region ifler
                    string yukarı = "", sağ = "", alt = "", sol = "";
                    if (x - 1 >= 0)
                        sol = koordinat[x - 1, y];
                    if (x + 1 <= uzunluuk)
                        sağ = koordinat[x + 1, y];
                    if (y - 1 >= 0)
                        yukarı = koordinat[x, y - 1];
                    if (y + 1 <= uzunluuk)
                        alt = koordinat[x, y + 1];

                    if (yukarı == siyah)
                        kontrol_beyaz(x, (y - 1));
                    if (sağ == siyah)
                        kontrol_beyaz((x + 1), y);
                    if (sol == siyah)
                        kontrol_beyaz((x - 1), y);
                    if (alt == siyah)
                        kontrol_beyaz(x, (y + 1));

                    #endregion
                }
                else if (s == siyah)
                {
                    #region ifler
                    string yukarı = "", sağ = "", alt = "", sol = "";
                    if (x - 1 >= 0)
                        sol = koordinat[x - 1, y];
                    if (x + 1 <= uzunluuk)
                        sağ = koordinat[x + 1, y];
                    if (y - 1 >= 0)
                        yukarı = koordinat[x, y - 1];
                    if (y + 1 <= uzunluuk)
                        alt = koordinat[x, y + 1];

                    if (yukarı == beyaz)
                        kontrol_siyah(x, (y - 1));
                    if (sağ == beyaz)
                        kontrol_siyah((x + 1), y);
                    if (sol == beyaz)
                        kontrol_siyah((x - 1), y);
                    if (alt == beyaz)
                        kontrol_siyah(x, (y + 1));
                    #endregion
                }
            }
            catch
            {
                taş_yeme = false;
            }
        }

        #region 4 taş durumu için kontroller
        private void kontrol_beyaz(int x, int y)
        {
            string y1 = koordinat[x, y - 1];
            string a1 = koordinat[x, y + 1];
            string sa1 = koordinat[x + 1, y];
            string so1 = koordinat[x - 1, y];
            if (y1 == beyaz && a1 == beyaz && sa1 == beyaz && so1 == beyaz)
            {
                taş_yeme = true;
                yeni_x = x; yeni_y = y;
                taşı_ye(yeni_x, yeni_y);
            }
            else
                taş_yeme = false;
        }
        private void kontrol_siyah(int x, int y)
        {
            string a1 = koordinat[x, y + 1];
            string sa1 = koordinat[x + 1, y];
            string so1 = koordinat[x - 1, y];
            string y1 = koordinat[x, y - 1];
            if (y1 == siyah && a1 == siyah && sa1 == siyah && so1 == siyah)
            {
                taş_yeme = true;
                yeni_x = x; yeni_y = y;
                taşı_ye(yeni_x, yeni_y);
            }
            else
                taş_yeme = false;
        }
        #endregion

        List<string> alan = new List<string>();
        bool var = true;
        int kuzey, güney, doğu, batı;

        private void alan_varmı(int x, int y)
        {
            alan.Clear();
            kuzey = y;
            güney = y;
            doğu = x;
            batı = x;
            int dikey = y, yatay = x;
            alan.Add(x + "," + y);
            if (taş_varmı(yatay, kuzey - 1) == true)
            {
                do
                {
                    if (taş_varmı(yatay, kuzey - 1) == true)
                    {
                        var = true;
                        alan.Add(yatay + "," + (kuzey - 1));
                        kuzey -= 1;
                    }
                    else if (taş_varmı(yatay - 1, kuzey - 1) == true)
                    {
                        var = true;
                        alan.Add((yatay - 1) + "," + (kuzey - 1));
                        yatay -= 1;
                        kuzey -= 1;
                    }
                    else if (taş_varmı(yatay + 1, kuzey - 1) == true)
                    {
                        var = true;
                        alan.Add((yatay + 1) + "," + (kuzey - 1));
                        yatay += 1;
                        kuzey -= 1;
                    }
                    else
                        var = false;
                } while (var);
            }
        }

        private bool taş_varmı(int x, int y)
        {
            bool değer = false;
            try
            {
                if (koordinat[x, y] == sıra)
                    değer = true;
                else
                    değer = false;
            }
            catch
            {
                değer = false;
            }
            return değer;
        }

        public void taşı_ye(int x, int y)
        {
            koordinat[yeni_x, yeni_y] = "";
            string x1 = yeni_x.ToString();
            string y1 = yeni_y.ToString();
            string k1 = x1 + "," + y1;
            int artış = katsayı / 4;
            for (int i = 0; i < tahta_üzerindeki_taş.Count; i++)
            {
                if (tahta_üzerindeki_taş[i].Tag.ToString() == k1)
                    tahta_üzerindeki_taş[i].Children.Clear();
            }
            yeni_x = 100; yeni_y = 100;
            Puan_Hesapla();
            taş_yeme = false;
        }

        public bool Hamle_Puan_durumu()
        {
            bool değer = false;
            if (S_Taş_Sayısı == 0 || B_Taş_Sayısı == 0 && Hamle_Sayısı > 0)
            {
                if (puan1 > puan2)
                    MessageBox.Show(LBL_oyuncu2.Content +" kazandı...", "bilgilendirme");
                else if (puan1 == puan2)
                    MessageBox.Show("Berabere...", "bilgilendirme");
                else if (puan2 > puan1)
                    MessageBox.Show(LBL_oyuncu1.Content +" Kazandı...", "bilgilendirme");
                değer = true;
            }
            else if (S_Taş_Sayısı > 0 && B_Taş_Sayısı > 0)
                değer = false;

            return değer;
        }

        private void Puan_Hesapla()
        {
            if (sıra == siyah && taş_yeme == false)
            {
                puan2++;
                S_Taş_Sayısı--;
                lbl_s_tas.Content = "Taş Sayısı = " + S_Taş_Sayısı.ToString();
                puan_siyah.Content = "Puan = " + puan2.ToString();
            }
            else if (sıra == beyaz && taş_yeme == false)
            {
                puan1++;
                B_Taş_Sayısı--;
                lbl_b_tas.Content = "Taş Sayısı = " + B_Taş_Sayısı.ToString();
                puan_beyaz.Content = "Puan = " + puan1.ToString();
            }
            else if (sıra == siyah && taş_yeme == true)
            {
                puan2 += 2;
                S_Taş_Sayısı--;
                B_Taş_Sayısı++;
                lbl_s_tas.Content = "Taş Sayısı = " + S_Taş_Sayısı.ToString();
                lbl_b_tas.Content = "Taş Sayısı = " + B_Taş_Sayısı.ToString();
                puan_siyah.Content = "Puan = " + puan2.ToString();
            }
            else if (sıra == beyaz && taş_yeme == true)
            {
                puan1 += 2;
                S_Taş_Sayısı++;
                B_Taş_Sayısı--;
                lbl_s_tas.Content = "Taş Sayısı = " + S_Taş_Sayısı.ToString();
                lbl_b_tas.Content = "Taş Sayısı = " + B_Taş_Sayısı.ToString();
                puan_beyaz.Content = "Puan = " + puan1.ToString();
            }
        }
        #endregion

        #region hamleyi yazma ve kaydetme
        private void hamleyi_yaz(int x, int y)
        {
            Hamle_Sayısı++;
            lbl_sayı_hamle.Content = "Hamle Sayısı = " + Hamle_Sayısı.ToString();
            hamle = x.ToString() + "," + y.ToString();
            hamleyi_kaydet(hamle, sıra);
            sırayı_belirle();
        }
        public string ad;
        private void hamleyi_kaydet(string h, string s)
        {
            ad = LBL_oyuncu1.Content + " vs " + LBL_oyuncu2.Content;
            
            SqlCeConnection cecon = new SqlCeConnection("DataSource=..\\..\\Go.sdf");
            cecon.Open();
            SqlCeCommand cecom = new SqlCeCommand("INSERT INTO Oyun(OyunAdı,OyunTarihi,HamleSiyah,HamleBeyaz,MasaBoyutu,SkorSiyah,SkorBeyaz) VALUES (@OyunAdı,@OyunTarihi,@h1,@h2,@boyut,@s1,@s2)", cecon);

            cecom.Parameters.Add("@OyunAdı",ad);
            cecom.Parameters.Add("@OyunTarihi", zaman);
            if (s == siyah)
            {
                cecom.Parameters.Add("@h1", h);
                cecom.Parameters.Add("@h2", "");
            }
            else if (s == beyaz)
            {
                cecom.Parameters.Add("@h1", "");
                cecom.Parameters.Add("@h2", h);
            }

            if (büyükmasa == true && küçükmasa == false && ortamasa == false)
            {
                cecom.Parameters.Add("@boyut", "Büyük");
            }
            else if (büyükmasa == false && küçükmasa == false && ortamasa == true)
            {
                cecom.Parameters.Add("@boyut", "Orta");
            }
            else if (büyükmasa == false && küçükmasa == true && ortamasa == false)
            {
                cecom.Parameters.Add("@boyut", "Küçük");
            }
            cecom.Parameters.Add("@s1",puan1);
            cecom.Parameters.Add("@s2", puan2);
            cecom.ExecuteNonQuery();
            cecon.Close();
        }
        #endregion

        #region Timerlar

        #region Yanıp Söndür
        int saniye = 0;
        public void yanıpsöndür_Tick(object sender, EventArgs e)
        {
            if (saniye == 0)
            {
                for (int i = 0; i < tahta_üzerindeki_taş.Count; i++)
                {
                    tahta_üzerindeki_taş[i].Background = Brushes.Transparent;
                }  
            }
            StackPanel sp = tahta_üzerindeki_taş[stacpanelsırası];
            if ((saniye % 2) == 0)
                sp.Background = Brushes.Yellow;
            else if ((saniye % 2) == 1)
                sp.Background = Brushes.Transparent;
            else if (saniye == 60)
            {
                saniye = 0;
                yanıpsöndür.Stop();
            }
            saniye++;
        }
        #endregion

        #region Siyah Oyun Süresi
        public void süre_siyah_Tick(object sender, EventArgs e)
        {
            if (süre1 == 0)
            {
                süre1 = 60; süre2 = 60;
                byz_resim.Source = ResimSeç("Beyaz1");
                syh_resim.Source = ResimSeç("siyah1");
                s_siyah.Stop();
                sırayı_belirle();
            }
            else
            {
                süre1--;
                if (süre1 < 10)
                {
                    if (syh_resim.Source == ResimSeç("siyah1"))
                        syh_resim.Source = ResimSeç("Uyarı2");
                    else
                        syh_resim.Source = ResimSeç("siyah1");
                    süre_siyah.Content = "Kalan Süre: 00:0" + süre1.ToString();
                }
                else
                {
                    süre_siyah.Content = "Kalan Süre: 00:" + süre1.ToString();
                }
            }
        }
        #endregion

        #region Beyaz Oyun Süresi
        public void süre_beyaz_Tick(object sender, EventArgs e)
        {
            if (süre2 == 0)
            {
                byz_resim.Source = ResimSeç("Beyaz1");
                syh_resim.Source = ResimSeç("siyah1");
                süre1 = 60; süre2 = 60;
                s_beyaz.Stop();
                sırayı_belirle();
            }
            else
            {
                süre2--;
                if (süre2 < 10)
                {
                    if (byz_resim.Source == ResimSeç("Beyaz1"))
                        byz_resim.Source = ResimSeç("Uyarı");
                    else
                        byz_resim.Source = ResimSeç("Beyaz1");
                    süre_beyaz.Content = "Kalan Süre: 00:0" + süre2.ToString();
                }
                else
                {
                    süre_beyaz.Content = "Kalan Süre: 00:" + süre2.ToString();
                }
            }
        }
        #endregion

        #endregion

        #endregion
    }
}