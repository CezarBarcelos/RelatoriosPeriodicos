using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelatoriosPeriodicos.Entidades
{
    public class StatusBroadcast
    {
        public string NomeChatbot { get; set; }
        public string id { get; set; }
        public string recipients { get; set; }
        public string sent { get; set; }
        public string received { get; set; }
        public string consumed { get; set; }
        public string failed { get; set; }
        public string status { get; set; }
        public string statusDate { get; set; }
    }
}