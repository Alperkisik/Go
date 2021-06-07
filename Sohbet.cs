using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Windows;
using System.Timers;
using System.Windows.Controls;

namespace Go
{
    class Sohbet
    {
        class ClientHandler
        {
            public ClientHandler(NetworkStream networkStream)
            {
                buffer = new byte[32];
                this.networkStream = networkStream;
                callBackRead = new AsyncCallback(this.okumabittiginde);
                callBackWrite = new AsyncCallback(this.yazmabittiginde);
                oku();
            }
            public void oku()
            {
                networkStream.BeginRead(buffer, 0, buffer.Length, callBackRead, null);
            }
            public void yaz(string s)
            {
                System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                byte[] buffer = encoding.GetBytes(s);
                networkStream.BeginWrite(buffer, 0, buffer.Length, callBackWrite, null);
            }
            private void okumabittiginde(IAsyncResult ar)
            {
                int okunanByte = networkStream.EndRead(ar);
                if (okunanByte > 0)
                {
                    string alınanMesaj = System.Text.Encoding.ASCII.GetString(buffer, 0, okunanByte);
                    GelenMesaj(alınanMesaj);
                    oku();
                }
            }
            private void yazmabittiginde(IAsyncResult ar)
            {
                networkStream.EndWrite(ar);
                oku();
            }
            private byte[] buffer;
            private NetworkStream networkStream;
            private AsyncCallback callBackRead;
            private AsyncCallback callBackWrite;

            private void GelenMesaj(string mesaj)
            {
                string[] komut = mesaj.Split('|');
                switch (komut[0])
                {
                    case "MasaBoyu":
                        #region komutlar
                        if (komut[1] == "Kucuk")
                        {
                            Masa.masa.masa_K = "evet";
                            Masa.masa.masa_O = "hayir";
                            Masa.masa.masa_B = "hayir";
                            Masa.masa.ayarlar.ayar_13x13 = false;
                            Masa.masa.ayarlar.ayar_9x9 = true;
                            Masa.masa.ayarlar.ayar_19x19 = false;
                            Masa.masa.büyükmasa = false;
                            Masa.masa.ortamasa = false;
                            Masa.masa.küçükmasa = true;
                        }
                        else if (komut[1] == "Orta")
                        {
                            Masa.masa.masa_K = "hayir";
                            Masa.masa.masa_O = "evet";
                            Masa.masa.masa_B = "hayir";
                            Masa.masa.ayarlar.ayar_13x13 = true;
                            Masa.masa.ayarlar.ayar_9x9 = false;
                            Masa.masa.ayarlar.ayar_19x19 = false;
                            Masa.masa.büyükmasa = false;
                            Masa.masa.ortamasa = true;
                            Masa.masa.küçükmasa = false;
                        }
                        else if (komut[1] == "Buyuk")
                        {
                            Masa.masa.masa_K = "hayir";
                            Masa.masa.masa_O = "hayir";
                            Masa.masa.masa_B = "evet";
                            Masa.masa.büyükmasa = true;
                            Masa.masa.ortamasa = false;
                            Masa.masa.küçükmasa = false;
                            Masa.masa.ayarlar.ayar_13x13 = false;
                            Masa.masa.ayarlar.ayar_9x9 = false;
                            Masa.masa.ayarlar.ayar_19x19 = true;
                        }
                        Oyun.oyun_penceresi.rakipOyuncuAdı(komut[2] + "|" + komut[3]);
                        Oyun.oyun_penceresi.Dispatcher.Invoke((Action)(() =>
                        {
                            Masa.masa.Masayı_Oluştur();
                        }));
                        #endregion
                        break;
                    case "RakipOyuncu":
                        #region komutlar
                        if (komut[1] == "host")
                        {
                            Oyun.oyun_penceresi.rakipOyuncuAdı(komut[1] + "|" + komut[2]);
                        }
                        else
                        {
                            Oyun.oyun_penceresi.rakipOyuncuAdı(komut[1] + "|" + komut[2]);
                        }
                        #endregion
                        break;
                    case "Koordinat":
                        #region komutlar
                        Oyun.oyun_penceresi.Dispatcher.Invoke((Action)(() =>
                        {
                                 Oyun.oyun_penceresi.taş_koymak(mesaj);
                        }));
                        #endregion
                        break;
                    case"SohbetGelenYazi":
                        Oyun.oyun_penceresi.sohbetMesaj(mesaj);
                        break;
                    case"OyunBitti":
                        Oyun.oyun_penceresi.oyunu_kapat();
                        break;
                }
            }
        }

        Socket socket;
        private NetworkStream stream;
        ClientHandler handler;
        TcpListener tcpListener;

        public void SunucuBaşlat(int port)
        {
            tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();
            Timer timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(gelenBağlantıKontrol);
            timer.Start();
        }

        void gelenBağlantıKontrol(object sender, ElapsedEventArgs e)
        {
            socket = tcpListener.AcceptSocket();
            if (socket.Connected)
            {
                (sender as Timer).Stop();
                stream = new NetworkStream(socket);
                handler = new ClientHandler(stream);
                Oyun.oyun_penceresi.Dispatcher.Invoke((Action)(() =>
                {
                    Masa.masa.Masayı_Oluştur();
                }));
            }
        }

        public void SunucuKapat()
        {
            stream.Close();
            socket.Close();
            stream = null;
            socket = null;
        }

        public void İstemciBaşlat(string sunucu, int port)
        {
            TcpClient tcpClient = new TcpClient(sunucu, port);
            stream = tcpClient.GetStream();
            handler = new ClientHandler(stream);
        }

        public void İstemciKapat()
        {
            stream.Close();
            stream = null;
        }

        public void BilgiOku()
        {
            handler.oku();
        }

        public void BilgiGönder(string mesaj)
        {
            handler.yaz(mesaj);
        }
    }
}
