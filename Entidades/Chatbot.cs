using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RelatoriosPeriodicos.Entidades
{
    public class Chatbot
    {
        public int AutoId { get; set; }
        public string Id { get; set; }
        public string Nome { get; set; }
        public string NomeBase { get; set; }
        public bool Router { get; set; }
        public short DateTimeOffset { get; set; }
        public string Key { get; set; }
        public string RouterKey { get; set; }
        public string Nacionalidade { get; set; }
        public bool AtendimentoHumano { get; set; }
        public bool Broadcast { get; set; }
        public bool Report { get; set; }
    }
}
