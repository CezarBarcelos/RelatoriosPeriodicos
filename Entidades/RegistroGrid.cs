using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RelatoriosPeriodicos.Entidades
{
    public class RegistroGrid
    {
        public short chatbot { get; set; }
        public string nacionalidade { get; set; }
        public int MensagensEnviadas { get; set; }
        public int MensagensRecebidas { get; set; }
        public int UsuariosChatbot { get; set; }
        public int UsuariosPA { get; set; }
        public float Nota { get; set; }
    }
}
