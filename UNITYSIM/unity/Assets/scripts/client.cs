using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Net.Sockets;
using System.Net;
using System.Net.NetworkInformation;

//=======================================================================
//PGITIC TCP SERVER LIBRARY CLASS FOR ANDROID XAMARIN MONO # 2
//LAST UPDATE : 1394/1/23
//BY : EDWIN BABAIANS - EMAIL : edwin.babaians@gmail.com
//LICENSED : GNU PUBLIC LICENSE
//V 2.0
//=======================================================================


namespace core
{
    public class ClientEventArgs
    {
        public string message;
    }
    public class my_client
    {
        public int port = 0;
        public string ip = "";
        public bool active = false;
        public string name;

        public TcpClient client;
        System.Timers.Timer time_out_timer;

        public event TCPEventHandler get_event;
        public event TCPEventHandler get_message;
        public delegate void TCPEventHandler(ClientEventArgs args);

        public void Disconnect()
        {
            if (client != null && active)
            {
                active = false;
                client.Close();
                client = null;

                ClientEventArgs mes = new ClientEventArgs();
                mes.message = "Disconnected";
                if (get_event != null)
                    get_event(mes);
            }
        }
        public bool Connect()
        {
            if (active) return true;

            try
            {
                client = new TcpClient();
                IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

                active = false;
              
                client.Connect(serverEndPoint);

                ClientEventArgs mes = new ClientEventArgs();
                mes.message = "Connected";
                if (get_event != null)
                    get_event(mes);

                active = true;

                System.Threading.Thread clientThread = new System.Threading.Thread(new System.Threading.ThreadStart(HandleClientComm));
                clientThread.Start();

                return true;

            }
            catch (System.Exception ex)
            {
                ClientEventArgs mes = new ClientEventArgs();
                mes.message = "Timeout";
                if (get_event != null)
                    get_event(mes);

                client = null;
                name = "";
                active = false;
            }

            return false;
        }

        public my_client(string name)
        {
            this.name = name;
        }

        public void write_send(string msg)
        {
            if (client != null && active)
            {
                NetworkStream clientStream = client.GetStream();
                byte[] buffer = Encoding.UTF8.GetBytes(msg);
                clientStream.Write(buffer, 0, buffer.Length);
                clientStream.Flush();

                
             
            }
        }

        void read_process(string msg)
        {
            //PROTOCOL ANALISYS
            ClientEventArgs mes = new ClientEventArgs();
            mes.message = msg;
            if (get_message != null)
                get_message(mes);

        }
        private void HandleClientComm()
        {
                TcpClient tcpClient = client;
                if (tcpClient == null) return;

                if (active)
                {
                    NetworkStream clientStream = tcpClient.GetStream();

                    byte[] message = new byte[4096];
                    int bytesRead;

                    while (active)
                    {
                        bytesRead = 0;

                        try
                        {
                            bytesRead = clientStream.Read(message, 0, 4096);
                        }
                        catch
                        {

                            break;
                        }

                        if (bytesRead == 0)
                        {
                            break;
                        }

                        string read_str = Encoding.UTF8.GetString(message, 0, bytesRead);
                        read_process(read_str);
                    }

                    active = false;

                    ClientEventArgs mes = new ClientEventArgs();
                    mes.message = "Disconnected";

                    if (get_event != null)
                        get_event(mes);

                    if (tcpClient != null)
                    {
                        tcpClient.Close();
                        tcpClient = null;
                    }
                }

                
        }
    }
}