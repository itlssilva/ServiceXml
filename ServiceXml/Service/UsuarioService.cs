using ServiceXml.Model;
using System.Collections.Generic;
using System.Text;

namespace ServiceXml.Service
{
    public class UsuarioService
    {
        public void Executar()
        {
            var usuario = new Usuario();
            var objXml = PreencherXml(usuario);
            var xml = XmlService.ObterXmlSerializado(objXml, ASCIIEncoding.Unicode);            
        }

        private Usuario PreencherXml(Usuario usuario)
        {
            usuario.nome.PrimeiroNome = "Igor";
            usuario.nome.UltimoNome = "Valentim";
            usuario.documento.Cpf = "12345678965";
            return usuario;
        }

    }
}
