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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlServerCe;

namespace Go
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Windows.Forms.NotifyIcon m_notifyIcon;
        private System.Windows.Forms.ContextMenuStrip m_contextMenu;

        public MainWindow()
        {
            InitializeComponent();

            #region NotifyIcon

            m_contextMenu = new System.Windows.Forms.ContextMenuStrip();
            System.Windows.Forms.ToolStripMenuItem mI1 = new System.Windows.Forms.ToolStripMenuItem();
            mI1.Text = "Göster";
            mI1.Click += new EventHandler(mI1_Click);
            m_contextMenu.Items.Add(mI1);
            System.Windows.Forms.ToolStripMenuItem mI2 = new System.Windows.Forms.ToolStripMenuItem();
            mI2.Text = "Çıkış";
            mI2.Click += new EventHandler(mI2_Click);
            m_contextMenu.Items.Add(mI2);

            //Initalize Notify Icon
            m_notifyIcon = new System.Windows.Forms.NotifyIcon();
            m_notifyIcon.Text = "Go";
            m_notifyIcon.Icon = Properties.Resources.simge;
            m_notifyIcon.ContextMenuStrip = m_contextMenu; //Associate the contextmenustrip with notify icon
            m_notifyIcon.Visible = true;

            #endregion
        }


        #region NotifyIcon

        void mI1_Click(object sender, EventArgs e)
        {
            Show();
        }

        void mI2_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        private void btn_cikis_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btn_basla_Click(object sender, RoutedEventArgs e)
        {
            Ayar ayar = new Ayar();
            ayar.çoklu_oyna_host = false;
            ayar.Owner = this;
            ayar.ShowDialog();
            
        }

        private void btn_Hakkında_Click(object sender, RoutedEventArgs e)
        {
            Hakkında hakkında = new Hakkında();
            hakkında.Owner = this;
            hakkında.ShowDialog();
        }

        private void btn_Nasıl_Click(object sender, RoutedEventArgs e)
        {
            NasılOynanır nasıl = new NasılOynanır();
            nasıl.Owner = this;
            nasıl.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Çoklu_Oyna çkoyn = new Çoklu_Oyna();
            çkoyn.Owner = this;
            çkoyn.ShowDialog();
        }

        private void btn_kayıtlıOyunlar_Click(object sender, RoutedEventArgs e)
        {
            KayıtlıOyunlar kayıtlıOyunlar = new KayıtlıOyunlar();
           
            kayıtlıOyunlar.Owner = this;
            kayıtlıOyunlar.ShowDialog();

        }

        private void btn_skorlar_Click(object sender, RoutedEventArgs e)
        {
            Skorlar skorlar = new Skorlar();
            skorlar.Owner = this;
            skorlar.ShowDialog();
        }
    }
}
