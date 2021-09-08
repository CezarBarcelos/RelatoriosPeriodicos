using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RelatoriosPeriodicos.Entidades
{
    public class Serie
    {
        public string Nome { get; set; }
        public List<string> Valores { get; set; }
        public string Valor { get; set; }
        public System.Drawing.Color Cor { get; set; }
    }
}
