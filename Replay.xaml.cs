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
using System.Data;
using System.Data.SqlServerCe;

namespace Go
{
    /// <summary>
    /// Interaction logic for Replay.xaml
    /// </summary>
    public partial class Replay : Window
    {
        public Replay()
        {
            InitializeComponent();
        }
        public bool büyükmasa, ortamasa, küçükmasa;
        public string masa_B = "evet", masa_O = "hayir", masa_K = "hayir";
        public int Masanın_Boyutu = 18;
        private int katsayı = 32;
        private int büyüklük = 30;
        private int uzunluuk = 18;
        private int boşluk_uzunluğu;
        List<string> koordinatlar = new List<string>();
        private int yeni_x = 100, yeni_y = 100;
        private const string beyaz = "Beyaz", siyah = "Siyah";
        private string[,] koordinat = new string[19, 19];
        public List<StackPanel> tahta_üzerindeki_taş = new List<StackPanel>();
        List<string> sıralar = new List<string>();
        private bool taş_yeme = false;
        string sıra = siyah;
        DispatcherTimer oynat;
        public static Replay replay;
        int sayı = 0;
        public int aa = 0;
        public string ad;
        bool kaydırma = false;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            replay = this;
            #region masa oluştu
            if (aa == 0)
            {
                büyükmasa = Masa.masa.büyükmasa;
                ortamasa = Masa.masa.ortamasa;
                küçükmasa = Masa.masa.küçükmasa;
                ad = Masa.masa.ad;
            }
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
            }
            else if (ortamasa == true)
            {
                kutu_sayısı = 12;
                uzunluuk = 12;
                bd_1.Height = bd_1.Width = 578;
                katsayı = 578 / kutu_sayısı;
                büyüklük = 40;
            }
            else if (büyükmasa == true)
            {
                kutu_sayısı = 18;
                uzunluuk = 18;
                bd_1.Height = bd_1.Width = 578;
                katsayı = 32;
                büyüklük = 30;
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
                    tahta_üzerindeki_taş.Add(taş_bölgesi);
                    grd.Children.Add(taş_bölgesi);
                }
            }
            #endregion

            #endregion
            sld_replay.IsEnabled = false;
            //koordinat = Masa.masa.koordinat;

            SqlCeConnection cecon = new SqlCeConnection("DataSource=..\\..\\Go.sdf");
            cecon.Open();
            SqlCeCommand cecom = new SqlCeCommand("SELECT * FROM Oyun WHERE OyunAdı='" + ad + "'", cecon);
            SqlCeDataReader dr = cecom.ExecuteReader();
            while (dr.Read())
            {
                if (dr["HamleSiyah"].ToString() == "")
                {
                    koordinatlar.Add(dr["HamleBeyaz"].ToString());
                    sıralar.Add(beyaz);
                }
                else
                {
                    koordinatlar.Add(dr["HamleSiyah"].ToString());
                    sıralar.Add(siyah);
                }
            }
            dr.Close();
            cecom.Dispose();
            cecon.Close();
            GC.Collect();

            #region Timer
            oynat = new DispatcherTimer();
            oynat.Tick += new EventHandler(oynat_Tick);
            oynat.Interval = new TimeSpan(0, 0, 1);
            oynat.Start();
            #endregion
            sld_replay.Maximum = koordinatlar.Count;
        }

        public void oynat_Tick(object sender, EventArgs e)
        {
            sayı = (int)sld_replay.Value;
            if (sayı == sld_replay.Maximum)
            {
                oynat.Stop();
                btn_durdurBaşlat.IsEnabled = false;
                kaydırma = true;
                sld_replay.IsEnabled = true;
            }
            else
            {
                sld_replay.Value += 1;
                kaydırma = false;
                sld_replay.IsEnabled = false;
                for (int i = 0; i < tahta_üzerindeki_taş.Count; i++)
                {
                    if (koordinatlar[sayı] == tahta_üzerindeki_taş[i].Tag.ToString())
                    {
                        string[] ko = koordinatlar[sayı].Split(',');
                        sıra = sıralar[sayı];
                        taşı_koy(int.Parse(ko[0]), int.Parse(ko[1]), tahta_üzerindeki_taş[i]);
                        break;
                    }
                }
            }
            
        }

        void taşı_koy(int x, int y, StackPanel sp)
        {
            if (sp.Children.Count > 0)
                sp.Children.Clear();

            koordinat[x, y] = sıra;

            #region resim ekleme
            Image Resim = new Image();

            Resim.Height = Resim.Width = büyüklük;
            if (sıra == siyah)
                Resim.Source = ResimSeç("siyah");
            else if (sıra == beyaz)
                Resim.Source = ResimSeç("beyaz");
            sp.Children.Add(Resim);
            #endregion

            #region taş durumları

            if (iki_taş_durumumu_var(x, y, sıra))
                taşı_ye(Masa.masa.yeni_x, Masa.masa.yeni_y);
            if (üç_taş_durumumu_var(x, y, sıra))
                taşı_ye(Masa.masa.yeni_x, Masa.masa.yeni_y);
            dört_taş_yeme_durumumu_var(x, y, sıra);
            #endregion
        }

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
            taş_yeme = false;
        }

        #endregion

        private void btn_durdurBaşlat_Click(object sender, RoutedEventArgs e)
        {
            if (btn_durdurBaşlat.Content.ToString() == "Durdur")
            {
                btn_durdurBaşlat.Content = "Devam Et";
                oynat.Stop();
                kaydırma = true;
                sld_replay.IsEnabled = true;
            }

            else if (btn_durdurBaşlat.Content.ToString() == "Devam Et")
            {
                btn_durdurBaşlat.Content = "Durdur";
                oynat.Start();
                kaydırma = false;
                sld_replay.IsEnabled = false;
            }
        }

        private void btn_replayÇıkış_Click(object sender, RoutedEventArgs e)
        {
            Close();

        }
        int uzunluk;
        private void sld_replay_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
            if (kaydırma == true)
            {
                uzunluk = (int)sld_replay.Value;
                for (int i = 0; i < tahta_üzerindeki_taş.Count; i++)
                {
                    tahta_üzerindeki_taş[i].Children.Clear();
                }
                for (int z = 0; z < 19; z++)
                {
                    for (int y = 0; y < 19; y++)
                    {
                        koordinat[z, y] = "";
                    }
                }
                for (int y = 0; y < uzunluk; y++)
                {
                    for (int i = 0; i < tahta_üzerindeki_taş.Count; i++)
                    {
                        if (koordinatlar[y] == tahta_üzerindeki_taş[i].Tag.ToString())
                        {
                            string[] ko = koordinatlar[y].Split(',');
                            sıra = sıralar[y];
                            taşı_koy(int.Parse(ko[0]), int.Parse(ko[1]), tahta_üzerindeki_taş[i]);
                            break;
                        }
                    }
                }
            }
            else
            {
            }
        }
    }
}
