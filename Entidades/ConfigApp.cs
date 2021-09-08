namespace RelatoriosPeriodicos.Entidades
{
    public class ConfigApp
    {
        public string IP_SMTP { get; set; }
        public string StringConnection { get; set; }
        public string BaseUrlBlipCommand { get; set; }     
        public string EmailSendRelUsoNotif { get; set; }
        public string EmailCCSendRelUsoNotif { get; set; }
        public bool UpdateDataBase { get; set; }
        public bool SendReportContNotif { get; set; }
        public byte DayOfWeek { get; set; }
        public string Grupo { get; set; }
    }
}
