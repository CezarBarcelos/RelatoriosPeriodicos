using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using RelatoriosPeriodicos.Entidades;

namespace RelatoriosPeriodicos.Servicos
{
    public class Config
    {
        const string DIR_UPLOAD_ARQUIVOS = "\\UploadArquivos\\";
        const string DIR_CONFIG = "\\Config\\";
        const string CONFIG_FILE = "config.dat";

        public string IP_SMTP;
        public string StringConnection;
        public string CaminhoArquivosUpload;
        public string EmailSendRelUsoNotif;
        public string EmailCCSendRelUsoNotif;
        public string CaminhoConfig;
        public string BaseUrlBlipCommand;
        public string ErroConfig;
        public string Grupo;
        public byte DayOfWeek;

        public Config()
        {
            ObtemCaminhos();
            ObtemConfig(this.CaminhoConfig);
        }

        private void ObtemCaminhos()
        {
            try
            {
                string dirApp = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory.ToString());
                this.CaminhoArquivosUpload = dirApp + DIR_UPLOAD_ARQUIVOS;
                this.CaminhoConfig = dirApp + DIR_CONFIG + CONFIG_FILE;

                CriaDiretorio(this.CaminhoArquivosUpload);
                CriaDiretorio(this.CaminhoConfig);
            }
            catch(Exception ex)
            {
                ErroConfig = ex.Message;
            }
        }

        public void ObtemConfig(string fileConfigPath)
        {
            try
            {
                using (StreamReader sr = new StreamReader(fileConfigPath))
                {
                    try
                    {
                        JObject objJson = JObject.Parse(sr.ReadToEnd());
                        ConfigApp configApp = JsonConvert.DeserializeObject<ConfigApp>(objJson.ToString());

                        this.IP_SMTP = configApp.IP_SMTP == null ? string.Empty : configApp.IP_SMTP;
                        this.StringConnection = configApp.StringConnection == null ? string.Empty : configApp.StringConnection;
                        this.BaseUrlBlipCommand = configApp.BaseUrlBlipCommand == null ? string.Empty : configApp.BaseUrlBlipCommand;
                        this.EmailSendRelUsoNotif = configApp.EmailSendRelUsoNotif == null ? string.Empty : configApp.EmailSendRelUsoNotif;
                        this.EmailCCSendRelUsoNotif = configApp.EmailCCSendRelUsoNotif == null ? string.Empty : configApp.EmailCCSendRelUsoNotif;
                        this.DayOfWeek = configApp.DayOfWeek;
                        this.Grupo = configApp.Grupo == null ? string.Empty : configApp.Grupo.ToUpper();
                    }
                    catch (Exception ex)
                    {
                        ErroConfig = ex.Message;                        
                    }
                }
            }
            catch (Exception ex)
            {
                ErroConfig = ex.Message;                
            }
        }
        private void CriaDiretorio(string caminho)
        {
            try
            {
                if (!Directory.Exists(caminho))
                {
                    Directory.CreateDirectory(caminho);
                }
            }
            catch
            {

            }
        }        
    }
}
