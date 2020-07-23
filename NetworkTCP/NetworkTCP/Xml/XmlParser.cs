using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NetworkTCP.Xml
{
    public class SavedData
    {
        public bool SentReceivedSymbol { get; set; }
        public bool ShowTimeStamp { get; set; }
        public int ServerPort { get; set; }
        public string ClientIP { get; set; }
        public int ClientPort { get; set; }
        public bool IsReadHex { get; set; }
        public bool IsWriteHex { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }

        #region Colors
        public string GridBackgroundColor { get; set; }
        public string GridForegroundColor { get; set; }
        public string ButtonBackgroundColor { get; set; }
        public string ButtonBorderColor { get; set; }
        public string ButtonForegroundColor { get; set; }
        public string TextBoxBackgroundColor { get; set; }
        public string TextBoxBorderColor { get; set; }
        public string TextBoxForegroundColor { get; set; }
        #endregion

    }

    public class XmlParser
    {
        public SavedData SavedData { get; set; }
        private string path = (Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\NetworkTCP");
        private XmlSerializer SavedDataXmlSerializer = new XmlSerializer(typeof(SavedData));

        public XmlParser()
        {
            SavedDataLoad();
        }

        public void SavedDataSave()
        {
            try
            {
                Directory.CreateDirectory(path);

                using (TextWriter tw = new StreamWriter(path + @"\SavedData.xml"))
                {
                    SavedDataXmlSerializer.Serialize(tw, SavedData);
                }
            }
            catch
            {
                
            }
        }

        public void SavedDataLoad()
        {
            try
            {
                if (File.Exists(path + @"\SavedData.xml"))
                {
                    using (TextReader tr = new StreamReader(path + @"\SavedData.xml"))
                    {
                        SavedData = SavedDataXmlSerializer.Deserialize(tr) as SavedData;
                    }
                }
                else
                {
                    SavedData = new SavedData()
                    {
                        SentReceivedSymbol = false,
                        ShowTimeStamp = false,
                        ServerPort = 5000,
                        ClientIP = "127.0.0.1",
                        ClientPort = 5000,
                        IsReadHex = false,
                        IsWriteHex = false,
                        Prefix = "",
                        Suffix = "",
                        GridBackgroundColor = "WhiteSmoke",
                        GridForegroundColor = "Black",
                        ButtonBackgroundColor = "LightGray",
                        ButtonBorderColor = "DarkGray",
                        ButtonForegroundColor = "Black",
                        TextBoxBackgroundColor = "While",
                        TextBoxBorderColor = "Gray",
                        TextBoxForegroundColor = "Black"
                    };
                }
            }
            catch
            {

            }
        }

        public void SetDefault()
        {
            SavedData.SentReceivedSymbol = false;
            SavedData.ShowTimeStamp = false;
            SavedData.ServerPort = 5000;
            SavedData.ClientIP = "127.0.0.1";
            SavedData.ClientPort = 5000;
            SavedData.IsReadHex = false;
            SavedData.IsWriteHex = false;
            SavedData.Prefix = "";
            SavedData.Suffix = "";
            SavedData.GridBackgroundColor = "WhiteSmoke";
            SavedData.GridForegroundColor = "Black";
            SavedData.ButtonBackgroundColor = "LightGray";
            SavedData.ButtonBorderColor = "DarkGray";
            SavedData.ButtonForegroundColor = "Black";
            SavedData.TextBoxBackgroundColor = "While";
            SavedData.TextBoxBorderColor = "Gray";
            SavedData.TextBoxForegroundColor = "Black";
        }
    }
}
