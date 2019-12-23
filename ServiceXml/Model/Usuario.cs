using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ServiceXml.Model
{
    [XmlRoot("usuario", Namespace = ""), Serializable]
    public class Usuario
    {
        [XmlIgnore]
        public decimal Id { get; set; }

        [XmlIgnore]
        public string NomeArquivo { get; set; }

        [XmlElement("usuario")]
        public Nome nome { get; set; }

        [XmlElement("documento")]
        public Documento documento { get; set; }
    }

    public partial class Nome
    {
        [XmlElement("primeiroNome")]
        public string PrimeiroNome { get; set; }

        [XmlElement("ultimoNome")]
        public string UltimoNome { get; set; }        
    }

    public partial class Documento
    {
        [XmlElement("cpf")]
        public string Cpf { get; set; }
    }
}
