using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ServiceXml.Service
{
    public static class Serializacao
    {
        /// <summary>
        /// Serializar um Objeto para um arquivo XML
        /// </summary>
        /// <param name="pObjeto"></param>
        /// <returns>String com Arquivo XML</returns>
        public static string Serializar(Object pObjeto, Encoding encode)
        {
            MemoryStream objMemStream = null;

            try
            {
                objMemStream = new MemoryStream();

                var objSettings = new XmlWriterSettings();

                objSettings.Encoding = encode; 

                objSettings.Indent = true;

                var objWriter = XmlTextWriter.Create(objMemStream, objSettings);

                var objSerializador = new XmlSerializer(pObjeto.GetType());

                objSerializador.Serialize(objWriter, pObjeto);

                int count = (int)objMemStream.Length;
                byte[] arr = new byte[count];
                objMemStream.Seek(0, SeekOrigin.Begin);
                objMemStream.Read(arr, 0, count);
                //var utf = new UTF8Encoding();
                return encode.GetString(arr).Trim();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                if (objMemStream != null)
                    objMemStream.Close();
            }
        }

        /// <summary>
        /// Deserializar um arquivo XML para o respectivo Objeto
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pArquivoXml"></param>
        /// <returns></returns>
        public static T Deserializar<T>(string pArquivoXml)
        {
            T Objeto;

            StringReader strReader = null;

            try
            {
                strReader = new StringReader(pArquivoXml);
                var XmlSerializado = new XmlSerializer(typeof(T));
                Objeto = (T)XmlSerializado.Deserialize(strReader);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                strReader.Close();
            }

            return Objeto;
        }
    }
}
