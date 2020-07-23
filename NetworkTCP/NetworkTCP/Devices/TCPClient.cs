using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TCP
{
    class TCPClient 
    {
        private TcpClient m_Client;
        private NetworkStream m_Stream;
        private byte[] buffer = new byte[1024];

        public bool Connection { get; set; }
        public string ReceivedData { get; set; }
        public string SentData { get; set; }

        public TCPClient()
        {

        }

        public void ClientConnect(string ipAddress, int port)
        {
            try
            {
                IPEndPoint serverAddress = new IPEndPoint(IPAddress.Parse(ipAddress), port);

                m_Client = new TcpClient();
                m_Client.Connect(serverAddress);
                m_Stream = m_Client.GetStream();
                Connection = true;
            }
            catch
            {
                Connection = false;
            }

        }

        public void ClientClose()
        {
            m_Stream.Close();
            m_Client.Close();
        }

        public void Read()
        {
            try
            {
                int byteLength = m_Stream.Read(buffer, 0, buffer.Length);
                ReceivedData = Encoding.Default.GetString(buffer, 0, byteLength);
            }
            catch
            {

            }
        }

        public void Write(string text)
        {
            if (m_Stream.CanWrite)
            {
                byte[] msg = Encoding.Default.GetBytes(text);

                m_Stream.Write(msg, 0, msg.Length);
            }
        }

    }
}
