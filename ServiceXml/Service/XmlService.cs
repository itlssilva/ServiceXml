using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace ServiceXml.Service
{
    public static class XmlService
    {
        /// <summary>
        /// Obter Xml Serializado de um Objeto
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public static XDocument ObterXmlSerializado(Object objeto, Encoding encode)
        {
            var ArquivoXml = RemoverBookMark(Serializacao.Serializar(objeto, encode));

            var objXml = new XmlDocument();
            objXml.LoadXml(ArquivoXml);

            RemoverAtributosNamespace((XmlNode)objXml);

            return XDocument.Parse(objXml.OuterXml);
        }

        /// <summary>
        /// Remover caracteres BookMark de um Arquivo XML UTF-8
        /// </summary>
        /// <param name="pTexto"></param>
        /// <returns></returns>
        public static string RemoverBookMark(string texto)
        {
            string BOMMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
            if (texto.StartsWith(BOMMarkUtf8))
                texto = texto.Remove(0, BOMMarkUtf8.Length);

            return texto.Replace("\0", "");
        }

        /// <summary>
        /// Remover todos os atributos de um Arquivo XML
        /// </summary>
        /// <param name="node"></param>
        public static void RemoverAtributosNamespace(XmlNode node)
        {
            if (node.Attributes != null)
            {
                for (int i = node.Attributes.Count - 1; i >= 0; i--)
                {
                    if (node.Attributes[i].Name.Contains(':') || node.Attributes[i].Name == "xmlns")
                        node.Attributes.Remove(node.Attributes[i]);
                }
            }

            foreach (XmlNode n in node.ChildNodes)
            {
                RemoverAtributosNamespace(n);
            }
        }

        /// <summary>
        /// Remover Acentos
        /// </summary>
        public static string RemoveAcentos(string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return "";
            else
            {
                byte[] bytes = System.Text.Encoding.GetEncoding("iso-8859-8").GetBytes(texto);
                return System.Text.Encoding.UTF8.GetString(bytes);
            }
        }
        ///Fim da Remoção de acentos


        /// <summary>
        /// Remover Espaços de um Texto
        /// </summary>
        /// <param name="pTexto"></param>
        /// <returns></returns>
        public static string RemoverEspacos(string texto)
        {
            string ret = string.Empty;

            foreach (var item in texto)
                ret += item.ToString().Trim();

            return ret;
        }
       
        /// <summary>
        /// Obter Arquivo Xml através do path informado
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static XDocument ObterXml(string path)
        {
            return XDocument.Load(path);
        }       
                       
        public static void EnviarFTP(string pstrArquivo, string pstrNomeArquivo, string diretorio, string usuario, string senha)
        {
            try
            {
                FileStream fs = File.OpenRead(pstrArquivo);
                byte[] buffer = new byte[fs.Length];

                fs.Read(buffer, 0, buffer.Length);

                Uri uri = new Uri(diretorio + pstrNomeArquivo);
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(uri);
                request.Credentials = new NetworkCredential(usuario, senha);
                request.KeepAlive = false;
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.UseBinary = true;
                request.ContentLength = buffer.Length;
                Stream strm = request.GetRequestStream();
                strm.Write(buffer, 0, buffer.Length);
                strm.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string MontaXML<T>(T obj)
        {
            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(obj.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                x.Serialize(textWriter, obj);
                return textWriter.ToString();
            }
        }

        public static void MontaXML<T>(T obj, string nomeArquivo, string path)
        {
            string strFilePath = string.Format("{0}{1}", path, nomeArquivo);

            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(obj.GetType());

            if (File.Exists(strFilePath))
                File.Delete(strFilePath);

            XmlTextWriter xtw = new XmlTextWriter(strFilePath, Encoding.GetEncoding("utf-8"));
            xtw.Formatting = Formatting.Indented;
            x.Serialize(xtw, obj);
            xtw.Close();
        }

        public static T MontarObjetoFromXML<T>(string xml)
        {
            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using (StringReader textReader = new StringReader(xml))
            {
                return (T)x.Deserialize(textReader);
            }
        }

        public static string CriarPasta(string pasta, string path)
        {
            string retorno = string.Format("{0}{1}", path, pasta);
            if (!Directory.Exists(retorno))
                Directory.CreateDirectory(retorno);

            return retorno;
        }

        public static string NomePasta(string pasta)
        {
            var ano = DateTime.Now.Year.ToString("0000");
            var mes = DateTime.Now.Month.ToString("00");
            var dia = DateTime.Now.Day.ToString("00");

            string retorno = !string.IsNullOrEmpty(pasta) ? string.Format("{0}{1}{2}_{3}", ano, mes, dia, pasta.Replace(".", "").Replace("/", "").Replace("-", "")) : "Upload";

            return retorno;
        }
    }
}