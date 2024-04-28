using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SoundlightInteractive.SecureSave
{
    public class SecureDataManager<T> where T : new()
    {
        private T stat;
        private string key;

        public SecureDataManager(string fileName)
        {
            key = fileName;
            stat = Load();
        }

        private T Load()
        {
            if (!SecurePlayerPrefs.HasKey(key))
            {
                return new T();
            }

            string data = SecurePlayerPrefs.GetString(key);

            T loadedPlayerObject = DeserializeObject(data);

            return loadedPlayerObject;
        }

        private string SerializeObject(T pObject)
        {
            MemoryStream memoryStream = new ();
            XmlSerializer xs = new(typeof(T));
            XmlTextWriter xmlTextWriter = new(memoryStream, Encoding.UTF8);
            xs.Serialize(xmlTextWriter, pObject);
            memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
            string xmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());

            return xmlizedString;
        }
        
        private T DeserializeObject(string pXmlizedString)
        {
            XmlSerializer xs = new(typeof(T));
            MemoryStream memoryStream = new(StringToUTF8ByteArray(pXmlizedString));
            return (T)xs.Deserialize(memoryStream);
        }

        private static string UTF8ByteArrayToString(byte[] characters)
        {
            UTF8Encoding encoding = new();
            string constructedString = encoding.GetString(characters);
            return (constructedString);
        }

        private static byte[] StringToUTF8ByteArray(string pXmlString)
        {
            UTF8Encoding encoding = new();
            byte[] byteArray = encoding.GetBytes(pXmlString);
            return byteArray;
        }
        
        public T Get()
        {
            return stat;
        }

        public void Save(T statValue)
        {
            string serializedData = SerializeObject(statValue);
            SecurePlayerPrefs.SetString(key, serializedData);
            SecurePlayerPrefs.Save();
        }
    }
}