using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Data;
using System.Windows.Input;

namespace NetworkTCP
{
    class MainWindow_ViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChangedEvent(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        #endregion

        #region Member Variable
        private TcpListener m_Server;
        private TcpClient m_Client;
        private NetworkStream m_Stream;
        private Xml.XmlParser m_XmlParser;

        private Thread ServerListening;
        private Thread ClientReading;

        private bool m_IsServerOpened;
        private bool m_ServerConnection;
        private string m_ServerOutput;
        private string m_ServerSendMsg;
        private List<string> ServerMsgList;
        private int ServerMsgListNum;
        private bool m_IsClientOpened;
        private bool m_ClientConnection;
        private string m_ClientOutput;
        private string m_ClientSendMsg;
        private List<string> ClientMsgList; 
        private int ClientMsgListNum;
        #endregion

        public bool SentReceivedSymbol { get { return m_XmlParser.SavedData.SentReceivedSymbol; } set { if (m_XmlParser.SavedData.SentReceivedSymbol != value) { m_XmlParser.SavedData.SentReceivedSymbol = value; RaisePropertyChangedEvent("SentReceivedSymbol"); } } }
        public bool ShowTimeStamp { get { return m_XmlParser.SavedData.ShowTimeStamp; } set { if (m_XmlParser.SavedData.ShowTimeStamp != value) { m_XmlParser.SavedData.ShowTimeStamp = value; RaisePropertyChangedEvent("ShowTimeStamp"); } } }
        public bool IsReadHex { get { return m_XmlParser.SavedData.IsReadHex; } set { if (m_XmlParser.SavedData.IsReadHex != value) { m_XmlParser.SavedData.IsReadHex = value; RaisePropertyChangedEvent("IsReadHex"); } } }
        public bool IsWriteHex { get { return m_XmlParser.SavedData.IsWriteHex; } set { if (m_XmlParser.SavedData.IsWriteHex != value) { m_XmlParser.SavedData.IsWriteHex = value; RaisePropertyChangedEvent("IsWriteHex"); } } }
        public string Prefix { get { return m_XmlParser.SavedData.Prefix; } set { if (m_XmlParser.SavedData.Prefix != value) { m_XmlParser.SavedData.Prefix = value; RaisePropertyChangedEvent("Prefix"); } } }
        public string Suffix { get { return m_XmlParser.SavedData.Suffix; } set { if (m_XmlParser.SavedData.Suffix != value) { m_XmlParser.SavedData.Suffix = value; RaisePropertyChangedEvent("Suffix"); } } }

        #region Server Properties
        public string[] ServerItems { get; set; }
        public int ServerPort { get { return m_XmlParser.SavedData.ServerPort; } set { if (m_XmlParser.SavedData.ServerPort != value) { m_XmlParser.SavedData.ServerPort = value; RaisePropertyChangedEvent("ServerPort"); } } }
        public bool IsServerOpened { get { return m_IsServerOpened; } set { if (m_IsServerOpened != value) { m_IsServerOpened = value; RaisePropertyChangedEvent("IsServerOpened"); } } }
        public bool ServerConnection { get { return m_ServerConnection; } set { if (m_ServerConnection != value) { m_ServerConnection = value; RaisePropertyChangedEvent("ServerConnection"); } } }
        public string ServerOutput { get { return m_ServerOutput; } set { if (m_ServerOutput != value) { m_ServerOutput = value; RaisePropertyChangedEvent("ServerOutput"); } } }
        public string ServerSendMsg { get { return m_ServerSendMsg; } set { if (m_ServerSendMsg != value) { m_ServerSendMsg = value; RaisePropertyChangedEvent("ServerSendMsg"); } } }
        #endregion

        #region Client Properties
        public string ClientIP { get { return m_XmlParser.SavedData.ClientIP; } set { if (m_XmlParser.SavedData.ClientIP != value) { m_XmlParser.SavedData.ClientIP = value; RaisePropertyChangedEvent("ClientIP"); } } }
        public int ClientPort { get { return m_XmlParser.SavedData.ClientPort; } set { if (m_XmlParser.SavedData.ClientPort != value) { m_XmlParser.SavedData.ClientPort = value; RaisePropertyChangedEvent("ClientPort"); } } }
        public bool IsClientOpened { get { return m_IsClientOpened; } set { if (m_IsClientOpened != value) { m_IsClientOpened = value; RaisePropertyChangedEvent("IsClientOpened"); } } }
        public bool ClientConnection { get { return m_ClientConnection; } set { if (m_ClientConnection != value) { m_ClientConnection = value; RaisePropertyChangedEvent("ClientConnection"); } } }
        public string ClientOutput { get { return m_ClientOutput; } set { if (m_ClientOutput != value) { m_ClientOutput = value; RaisePropertyChangedEvent("ClientOutput"); } } }
        public string ClientSendMsg { get { return m_ClientSendMsg; } set { if (m_ClientSendMsg != value) { m_ClientSendMsg = value; RaisePropertyChangedEvent("ClientSendMsg"); } } }
        #endregion

        #region Color Properties
        public List<string> ColorList { get; set; }
        public string GridBackgroundColor { get { return m_XmlParser.SavedData.GridBackgroundColor; } set { if (m_XmlParser.SavedData.GridBackgroundColor != value) { m_XmlParser.SavedData.GridBackgroundColor = value; RaisePropertyChangedEvent("GridBackgroundColor"); } } }
        public string GridForegroundColor { get { return m_XmlParser.SavedData.GridForegroundColor; } set { if (m_XmlParser.SavedData.GridForegroundColor != value) { m_XmlParser.SavedData.GridForegroundColor = value; RaisePropertyChangedEvent("GridForegroundColor"); } } }
        public string ButtonBackgroundColor { get { return m_XmlParser.SavedData.ButtonBackgroundColor; } set { if (m_XmlParser.SavedData.ButtonBackgroundColor != value) { m_XmlParser.SavedData.ButtonBackgroundColor = value; RaisePropertyChangedEvent("ButtonBackgroundColor"); } } }
        public string ButtonBorderColor { get { return m_XmlParser.SavedData.ButtonBorderColor; } set { if (m_XmlParser.SavedData.ButtonBorderColor != value) { m_XmlParser.SavedData.ButtonBorderColor = value; RaisePropertyChangedEvent("ButtonBorderColor"); } } }
        public string ButtonForegroundColor { get { return m_XmlParser.SavedData.ButtonForegroundColor; } set { if (m_XmlParser.SavedData.ButtonForegroundColor != value) { m_XmlParser.SavedData.ButtonForegroundColor = value; RaisePropertyChangedEvent("ButtonForegroundColor"); } } }
        public string TextBoxBackgroundColor { get { return m_XmlParser.SavedData.TextBoxBackgroundColor; } set { if (m_XmlParser.SavedData.TextBoxBackgroundColor != value) { m_XmlParser.SavedData.TextBoxBackgroundColor = value; RaisePropertyChangedEvent("TextBoxBackgroundColor"); } } }
        public string TextBoxBorderColor { get { return m_XmlParser.SavedData.TextBoxBorderColor; } set { if (m_XmlParser.SavedData.TextBoxBorderColor != value) { m_XmlParser.SavedData.TextBoxBorderColor = value; RaisePropertyChangedEvent("TextBoxBorderColor"); } } }
        public string TextBoxForegroundColor { get { return m_XmlParser.SavedData.TextBoxForegroundColor; } set { if (m_XmlParser.SavedData.TextBoxForegroundColor != value) { m_XmlParser.SavedData.TextBoxForegroundColor = value; RaisePropertyChangedEvent("TextBoxForegroundColor"); } } }
        #endregion

        public MainWindow_ViewModel()
        {
            m_XmlParser = new Xml.XmlParser();

            ServerItems = new string[] { "0.0.0.0", "127.0.0.1" };

            IsServerOpened = false;
            ServerConnection = false;

            ServerMsgList = new List<string>();
            ServerMsgListNum = 0;
            ClientMsgList = new List<string>();
            ClientMsgListNum = 0;

            //칼라리스트 불러오기
            ColorList = new List<string>();
            Type colorType = typeof(Color);
            PropertyInfo[] propInfoList = colorType.GetProperties(BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public);
            foreach (var c in propInfoList)
            {
                ColorList.Add(c.Name);
            }
        }

        internal void Closing()
        {
            ServerClose();
            ClientClose();
            m_XmlParser.SavedDataSave();

            if (ServerListening != null && ServerListening.IsAlive) ServerListening.Abort();
            if (ClientReading != null && ClientReading.IsAlive) ClientReading.Abort();
        }

        #region Server Methods
        internal void ServerOpenClose(string ip)
        {
            IsServerOpened = !IsServerOpened;

            if (IsServerOpened)
            {
                ClientClose();

                ServerListening = new Thread(new ThreadStart(delegate
                {
                    try
                    {
                        m_Server = new TcpListener(IPAddress.Parse(ip), ServerPort);
                        m_Server.Start();

                        byte[] bytes = new byte[256];
                        string data = null;

                        while (IsServerOpened)
                        {
                            try
                            {
                                m_Client = m_Server.AcceptTcpClient();
                                m_Stream = m_Client.GetStream();

                                ServerConnection = true;

                                int len = 0;
                                while ((len = m_Stream.Read(bytes, 0, bytes.Length)) != 0)
                                {
                                    if (IsReadHex) data = ReadByteToHex(bytes, len);
                                    else data = ReadByteToString(bytes, len);

                                    ServerOutput += (SentReceivedSymbol ? "← " : "") + (ShowTimeStamp ? "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] " : "") + data + "\n";
                                }

                                m_Client.Close();
                                ServerConnection = false;
                            }
                            catch (SocketException) { }
                            catch (IOException) { }
                            catch (Exception ex)
                            {
                                ServerOutput += ex.Message + "\n";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ServerOutput += ex.Message + "\n";
                        IsServerOpened = false;
                    }
                }));

                ServerListening.Start();
            }
            else
            {
                ServerClose();
            }
        }

        internal void ServerClose()
        {
            IsServerOpened = false;
            ServerConnection = false;
            if (m_Stream != null) m_Stream.Close();
            if (m_Client != null) m_Client.Close();
            if (m_Server != null) m_Server.Stop();
            if (ServerListening != null && ServerListening.IsAlive) ServerListening.Join();
        }

        internal void ServerSend()
        {
            try
            {
                if (m_Stream == null || !m_Stream.CanWrite) return;

                if (ServerSendMsg == null || ServerSendMsg == "") return;

                //보내고
                byte[] msg;
                if (IsWriteHex) msg = WriteStringAsHex(Prefix + ServerSendMsg + Suffix);
                else msg = WriteStringAsString(Prefix + ServerSendMsg + Suffix);

                m_Stream.Write(msg, 0, msg.Length);

                //표시
                string sendMsg = "";
                if (IsWriteHex)
                {
                    if (IsReadHex) sendMsg = Prefix + ServerSendMsg + Suffix;
                    else sendMsg = ReadByteToString(WriteStringAsHex(Prefix + ServerSendMsg + Suffix), WriteStringAsHex(Prefix + ServerSendMsg + Suffix).Length);
                }
                else
                {
                    if (IsReadHex) sendMsg = ReadByteToHex(WriteStringAsString(Prefix + ServerSendMsg + Suffix), WriteStringAsString(Prefix + ServerSendMsg + Suffix).Length);
                    else sendMsg = Prefix + ServerSendMsg + Suffix;
                }

                ServerOutput += (SentReceivedSymbol ? "→ " : "") + (ShowTimeStamp ? "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] " : "") + sendMsg + "\n";


                bool duplication = false;
                foreach (string val in ServerMsgList)
                {
                    if (val == ServerSendMsg) duplication = true;
                }

                if (ServerMsgList.Count > 100) ServerMsgList.RemoveAt(0);
                if (!duplication) ServerMsgList.Add(ServerSendMsg);
                ServerMsgListNum = ServerMsgList.Count - 1;

                ServerSendMsg = "";
            }
            catch (Exception ex)
            {
                ServerOutput += ex.Message + "\n";
            }
        }

        internal void ServerClear()
        {
            ServerOutput = "";
        }

        internal void ServerSendMsgKeyUp(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    if (ServerConnection) ServerSend();
                    break;
                case Key.Up:
                    if (ServerMsgList.Count == 0) break;
                    ServerSendMsg = ServerMsgList[ServerMsgListNum];
                    if (ServerMsgListNum > 0) ServerMsgListNum--;
                    break;
                case Key.Down:
                    if (ServerMsgList.Count == 0) break;
                    ServerSendMsg = ServerMsgList[ServerMsgListNum];
                    if (ServerMsgListNum < ServerMsgList.Count - 1) ServerMsgListNum++;
                    break;
            }
        }

        internal void ServerPortKeyUp(string ip, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ServerOpenClose(ip);
            }
        }
        #endregion

        #region Client Methods
        internal void ClientOpenClose()
        {
            IsClientOpened = !IsClientOpened;

            if (IsClientOpened)
            {
                ServerClose();
                
                ClientReading = new Thread(new ThreadStart(delegate 
                {
                    try
                    {
                        byte[] buffer = new byte[256];
                        string data = "";

                        while (IsClientOpened)
                        {
                            try
                            {
                                ClientConnection = false;

                                m_Client = new TcpClient(ClientIP, ClientPort);
                                m_Stream = m_Client.GetStream();

                                ClientConnection = true;

                                int len = 0;
                                while ((len = m_Stream.Read(buffer, 0, buffer.Length)) != 0)
                                {
                                    if (IsReadHex) data = ReadByteToHex(buffer, len);
                                    else data = ReadByteToString(buffer, len);

                                    ClientOutput += (SentReceivedSymbol ? "← " : "") + (ShowTimeStamp ? "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] " : "") + data + "\n";
                                }

                                Thread.Sleep(50);
                            }
                            catch (SocketException) { }
                            catch (IOException) { }
                            catch (ThreadAbortException) { }
                            catch (Exception ex)
                            {
                                ClientOutput += ex.Message + "\n";
                            }
                        }
                    }
                    catch (ThreadAbortException) { }
                    catch (Exception ex)
                    {
                        ClientOutput += ex.Message + "\n";
                    }
                }));

                ClientReading.Start();
            }
            else
            {
                ClientClose();
            }
        }

        internal void ClientClose()
        {
            IsClientOpened = false;
            ClientConnection = false;
            if (m_Stream != null) m_Stream.Close();
            if (m_Client != null) m_Client.Close();
            if (ClientReading != null && ClientReading.IsAlive) ClientReading.Abort();
        }

        internal void ClientSend()
        {
            try
            {
                if (m_Stream == null || !m_Stream.CanWrite) return;

                if (ClientSendMsg == null || ClientSendMsg == "") return;

                //보내고
                byte[] msg;
                if (IsWriteHex) msg = WriteStringAsHex(Prefix + ClientSendMsg + Suffix);
                else msg = WriteStringAsString(Prefix + ClientSendMsg + Suffix);

                m_Stream.Write(msg, 0, msg.Length);

                //표시
                string sendMsg = "";
                if (IsWriteHex)
                {
                    if (IsReadHex) sendMsg = Prefix + ClientSendMsg + Suffix;
                    else sendMsg = ReadByteToString(WriteStringAsHex(Prefix + ClientSendMsg + Suffix), WriteStringAsHex(Prefix + ClientSendMsg + Suffix).Length);
                }
                else
                {
                    if (IsReadHex) sendMsg = ReadByteToHex(WriteStringAsString(Prefix + ClientSendMsg + Suffix), WriteStringAsString(Prefix + ClientSendMsg + Suffix).Length);
                    else sendMsg = Prefix + ClientSendMsg + Suffix;
                }

                ClientOutput += (SentReceivedSymbol ? "→ " : "") + (ShowTimeStamp ? "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] " : "") + sendMsg + "\n";

                bool duplication = false;
                foreach (string val in ClientMsgList)
                {
                    if (val == ClientSendMsg) duplication = true;
                }

                if (ClientMsgList.Count > 100) ClientMsgList.RemoveAt(0);
                if (!duplication) ClientMsgList.Add(ClientSendMsg);
                ClientMsgListNum = ClientMsgList.Count - 1;

                ClientSendMsg = "";
            }
            catch (Exception ex)
            {
                ClientOutput += ex.Message + "\n";
                return;
            }
        }

        internal void ClientClear()
        {
            ClientOutput = "";
        }

        internal void ClientSendMsgKeyUp(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    if (ClientConnection) ClientSend();
                    break;
                case Key.Up:
                    if (ClientMsgList.Count == 0) break;
                    ClientSendMsg = ClientMsgList[ClientMsgListNum];
                    if (ClientMsgListNum > 0) ClientMsgListNum--;
                    break;
                case Key.Down:
                    if (ClientMsgList.Count == 0) break;
                    ClientSendMsg = ClientMsgList[ClientMsgListNum];
                    if (ClientMsgListNum < ClientMsgList.Count - 1) ClientMsgListNum++;
                    break;
            }
        }

        internal void ClientIPPortKeyUp(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ClientOpenClose();
            }
        }
        #endregion

        #region HexConverter
        private string ReadByteToString(byte[] data, int len)
        {
            return Encoding.Default.GetString(data, 0, len);
        }

        private string ReadByteToHex(byte[] data, int len)
        {
            byte[] tmpB = new byte[len];

            for (int i = 0; i < len; i++)
            {
                tmpB[i] = data[i];
            }

            string temp = "";

            foreach (byte b in tmpB)
            {
                temp += b.ToString("X2") + " ";
            }

            return temp;
        }

        private byte[] WriteStringAsString(string data)
        {
            return Encoding.Default.GetBytes(data);
        }

        private byte[] WriteStringAsHex(string data)
        {
            try
            {
                string[] temp = data.Split(' ');
                byte[] b = new byte[temp.Length];

                for (int i = 0; i < temp.Length; i++)
                {
                    b[i] = byte.Parse(temp[i], NumberStyles.HexNumber);
                }

                return b;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        internal void SetDefault_button_Click()
        {
            m_XmlParser.SetDefault();
            RefreshAll();
        }

        private void RefreshAll()
        {
            RaisePropertyChangedEvent(nameof(SentReceivedSymbol));
            RaisePropertyChangedEvent(nameof(ShowTimeStamp));
            RaisePropertyChangedEvent(nameof(IsReadHex));
            RaisePropertyChangedEvent(nameof(IsWriteHex));
            RaisePropertyChangedEvent(nameof(Prefix));
            RaisePropertyChangedEvent(nameof(Suffix));
            RaisePropertyChangedEvent(nameof(ServerPort));
            RaisePropertyChangedEvent(nameof(ClientIP));
            RaisePropertyChangedEvent(nameof(ClientPort));
            RaisePropertyChangedEvent(nameof(GridBackgroundColor));
            RaisePropertyChangedEvent(nameof(GridForegroundColor));
            RaisePropertyChangedEvent(nameof(ButtonBackgroundColor));
            RaisePropertyChangedEvent(nameof(ButtonBorderColor));
            RaisePropertyChangedEvent(nameof(ButtonForegroundColor));
            RaisePropertyChangedEvent(nameof(TextBoxBackgroundColor));
            RaisePropertyChangedEvent(nameof(TextBoxBorderColor));
            RaisePropertyChangedEvent(nameof(TextBoxForegroundColor));
        }
    }

    #region Converters
    public class ServerOpenBoolToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool val)
            {
                if (val) return "CLOSE SERVER";
                else return "OPEN SERVER";
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ServerOpenBoolToOpacity : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool val)
            {
                if (val) return 100;
                else return 0;
            }

            return 100;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ServerConnectionBoolToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool val)
            {
                if (val) return "Connected";
                else return "Wating for a connection...";
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ServerConnectionBoolToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool val)
            {
                if (val) return "LimeGreen";
                else return "Red";
            }

            return "Black";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ClientOpenBoolToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool val)
            {
                if (val) return "CLOSE CLIENT";
                else return "CONNECT";
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ClientConnectionBoolToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool val)
            {
                if (val) return "Connected";
                else return "Trying to connect...";
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToReverseBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool val) return !val;
            else return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool val) return !val;
            else return false;
        }
    }
    #endregion
}
