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
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlServerCe;

namespace Go
{
    /// <summary>
    /// Interaction logic for KayıtlıOyunlar.xaml
    /// </summary>
    public partial class KayıtlıOyunlar : Window
    {
        public KayıtlıOyunlar()
        {
            InitializeComponent();
        }

        List<string> boyut = new List<string>();
        List<string> oyunadı = new List<string>();
        List<string> oyuntarihi = new List<string>();
        int numara = 0;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            listboxıDoldur();
        }

        private void lb_kayıtlar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void btn_replay_Click(object sender, RoutedEventArgs e)
        {
            ListBoxItem nesne = (ListBoxItem)lb_kayıtlar.SelectedItem;
            int c = int.Parse(nesne.Tag.ToString());
            Replay replay = new Replay();
            replay.Owner = this;
            replay.aa = 1;
            replay.ad = oyunadı[lb_kayıtlar.SelectedIndex];
            if (boyut[c] == "Büyük")
            {
                replay.büyükmasa = true;
                replay.ortamasa = false;
                replay.küçükmasa = false;
            }
            else if (boyut[c] == "Orta")
            {
                replay.büyükmasa = false;
                replay.ortamasa = true;
                replay.küçükmasa = false;
            }
            else if (boyut[c] == "Küçük")
            {
                replay.büyükmasa = false;
                replay.ortamasa = false;
                replay.küçükmasa = true;
            }
            replay.ShowDialog();
        }

        private void btn_Sil_Click(object sender, RoutedEventArgs e)
        {
            ListBoxItem nesne = new ListBoxItem();
            nesne = (ListBoxItem)lb_kayıtlar.SelectedItem;
            string[] alınanveri = nesne.Content.ToString().Split(',');
            string a = alınanveri[0];
            string asılveri = "";
            for (int i = 0; i < a.Length - 2; i++)
            {
                asılveri += a[i];
            }
            SqlCeConnection sqlCon = new SqlCeConnection("DataSource=..\\..\\Go.sdf");
            sqlCon.Open();

            SqlCeCommand sqlCmd = new SqlCeCommand("SELECT * FROM [Oyun] Where [OyunAdı]='" + asılveri + "'", sqlCon);
            //DELETE FROM [Oyun] WHERE [OyunAdı]='" + asılveri + "'", sqlCon
            SqlCeDataReader sqlDr = sqlCmd.ExecuteReader();
            while (sqlDr.Read())
            {
                SqlCeCommand sqlCmd2 = new SqlCeCommand("DELETE FROM [Oyun] WHERE [OyunAdı]=@OyunAdı", sqlCon);
                sqlCmd2.Parameters.Add("@OyunAdı", sqlDr["OyunAdı"]);
                sqlCmd2.ExecuteNonQuery();
            }
            sqlDr.Close();
            sqlCmd.ExecuteNonQuery();
            sqlCmd.Dispose();
            sqlCon.Close();

            listboxıDoldur();
        }

        private void listboxıDoldur()
        {
            lb_kayıtlar.Items.Clear();
            SqlCeConnection sqlCecon = new SqlCeConnection("DataSource=..\\..\\Go.sdf");
            sqlCecon.Open();
            SqlCeCommand sqlCecom = new SqlCeCommand("Select distinct OyunAdı,OyunTarihi,MasaBoyutu from Oyun", sqlCecon);
            SqlCeDataReader sqlDr = sqlCecom.ExecuteReader();
            while (sqlDr.Read())
            {
                oyunadı.Add(sqlDr["OyunAdı"].ToString());
                oyuntarihi.Add(sqlDr["OyunTarihi"].ToString());
                boyut.Add(sqlDr["MasaBoyutu"].ToString());
                ListBoxItem nesne = new ListBoxItem();
                nesne.Content = oyunadı[numara] + "  ,  " + oyuntarihi[numara];
                nesne.Tag = numara;
                numara++;
                lb_kayıtlar.Items.Add(nesne);
            }

            sqlCecon.Close();
        }

    }
}
