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
using System.Data.SqlServerCe;

namespace Go
{
    /// <summary>
    /// Interaction logic for Skorlar.xaml
    /// </summary>
    public partial class Skorlar : Window
    {
        public Skorlar()
        {
            InitializeComponent();
        }

        List<string> oyunadı = new List<string>();
        List<string> Skor = new List<string>();
        List<string> listboxa = new List<string>();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SqlCeConnection sqlCecon = new SqlCeConnection("DataSource=..\\..\\Go.sdf");
            sqlCecon.Open();
            SqlCeCommand sqlCecom = new SqlCeCommand("Select OyunAdı,SkorSiyah,SkorBeyaz from Oyun ", sqlCecon);
            SqlCeDataReader sqlDr = sqlCecom.ExecuteReader();
            while (sqlDr.Read())
            {
                oyunadı.Add(sqlDr["OyunAdı"].ToString());
                int siyah = int.Parse(sqlDr["SkorSiyah"].ToString());
                int beyaz = int.Parse(sqlDr["SkorBeyaz"].ToString());
                if (siyah > beyaz)
                    Skor.Add("Kazanan Beyaz Skor:," + siyah.ToString());
                else if (siyah < beyaz)
                    Skor.Add("Kazanan Siyah Skor:," + beyaz.ToString());
                else if (siyah == beyaz)
                    Skor.Add("Berabere Skor:," +  siyah.ToString());
            }
            int sınır =  oyunadı.Count - 1;
            for (int i = 0; i < oyunadı.Count; i++)
            {
                if (i > 0)
                {
                    if (oyunadı[i] != oyunadı[i - 1])
                    {
                        string[] koy = Skor[i - 1].Split(',');
                        listboxa.Add(oyunadı[i - 1] + "  " + koy[0] + "  " + koy[1]);
                    }
                }
                if (i == sınır )
                {
                    string[] koy = Skor[i].Split(',');
                    listboxa.Add(oyunadı[i] + "  " + koy[0] + "  " + koy[1]);
                    break;
                }
            }

            for (int i = 0; i < listboxa.Count; i++)
            {
                lb_skorlar.Items.Add(listboxa[i]);
            }

            sqlCecon.Close();
        }
    }
}
