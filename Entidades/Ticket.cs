namespace RelatoriosPeriodicos.Entidades
{
    public class Ticket
    {
        public string date { get; set; }
        public int waiting { get; set; }
        public int open { get; set; }     
        public int closed { get; set; }
        public int closedAttendant { get; set; }
        public int closedClient { get; set; }
        public int transferred { get; set; }
        public int missed { get; set; }
    }
}
