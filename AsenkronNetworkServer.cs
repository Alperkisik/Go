using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace Go
{
    class AsenkronNetworkServer
    {
        class ClientHandler
        {
            public ClientHandler(NetworkStream networkStream)
            {
                //socketin kopyasını, yeni bir buffer ve network stream oluştur
                //socket = istemciIcinSocket;
                buffer = new byte[32];
                //networkStream = new NetworkStream(istemciIcinSocket);
                callBackRead = new AsyncCallback(this.okumabittiginde);
                callBackWrite = new AsyncCallback(this.yazmabittiginde);
            }
            public void oku()
            {
                //ilk okuma işini gerçekleştirir sonra callBackreadle ilişkilendirilmiş metodu çağırır
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
                //kaç byte okunmuş
                int okunanByte = networkStream.EndRead(ar);
                if (okunanByte > 0)
                {
                    string alınanMesaj = System.Text.Encoding.ASCII.GetString(buffer, 0, okunanByte);
                    GelenMesaj(alınanMesaj);
                    //Console.WriteLine(s);
                    //aldığın texti tekrar clienta gönder. gönderme işi bitince callBackwrite ile ilişkili metodu çağır.
                    //networkStream.BeginWrite(buffer, 0, buffer.Length, callBackWrite, null);
                    oku();
                }
                else
                {
                    //Console.WriteLine("Herhangi bir string okunmadı bağlantı kapatılıyor");
                }
            }
            private void yazmabittiginde(IAsyncResult ar)
            {
                networkStream.EndWrite(ar);
                //Console.WriteLine("Text gönderme işi bitti");
                //tekrar oku...
                networkStream.BeginRead(buffer, 0, buffer.Length, callBackRead, null);
            }
            private byte[] buffer;
            private Socket socket;
            private NetworkStream networkStream;
            private AsyncCallback callBackRead;
            private AsyncCallback callBackWrite;

            private void GelenMesaj(string mesaj)
            {
                //string mesaj = "MasaBoyu|Küçük";
                //string mesaj = "Hamle|1,5";
                string[] komut = mesaj.Split('|');
                switch (komut[0])
                {
                    case "MasaBoyu":
                        if (komut[1] == "küçük")
                            Masa.masa.masa_K = "evet";
                        else if (komut[1] == "orta")
                            Masa.masa.masa_O = "evet";
                        else if (komut[1] == "büyük")
                            Masa.masa.masa_B = "evet";
                        break;
                    case "Hamle":
                        string[] koor = komut[1].Split(',');

                        break;
                }

            }
        }

        Socket socket;
        private NetworkStream stream;
        ClientHandler handler;

        public void SunucuBaşlat(int port)
        {
            //yeni bir TcpListener oluştur ve dinlemeye başla port:65000
            TcpListener tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();
            //birçok bağlantıyı kabul etmek için sonsuz döngü
            //gelen her istek için oluşan soketi ClientHandlera gönder yeni bir soket oluştursun.
                //istek olursa kabul et ve istemciIcinSocket isminde yeni bir soket oluştur
            socket = tcpListener.AcceptSocket();
            if (socket.Connected)
            {
                //Console.WriteLine("Client Bağlandı");
                stream = new NetworkStream(socket);
                handler = new ClientHandler(stream);
                BilgiOku();
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
            //System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(stream);
            //streamWriter.WriteLine(mesaj);
            //streamWriter.Flush();
            handler.yaz(mesaj);
        }
    }
}
