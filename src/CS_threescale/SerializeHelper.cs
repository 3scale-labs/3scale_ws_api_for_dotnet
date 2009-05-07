using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Serialization;

namespace CS_threescale
{
    public class SerializeHelper<T>
    {
        public static T Ressurect(string xmlString)
        {
            T obj = default(T);
            try
            {
                if (!xmlString.ToLower().Contains("<?xml"))
                {
                    xmlString = xmlString.Insert(0, "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n");
                }
                
                byte[] raw = Encoding.UTF8.GetBytes(xmlString);
                MemoryStream stream = new MemoryStream(raw);
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                obj = (T)serializer.Deserialize(stream);

                stream.Close();
                return obj;
            }
            catch
            {
                throw new IOException("Unknown format " + xmlString );
            }
          
        }
    }
}
