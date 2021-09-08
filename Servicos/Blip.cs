using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;

namespace RelatoriosPeriodicos.Servicos
{
    public class Blip
    {
        Config objConfig;
        
        public Blip(Config config)
        {
            this.objConfig = config;
        }

        public async Task<string> ResquestMessage(string json, string chatbotKey)
        {
            string resposta = string.Empty;

            try
            {
                HttpClient httpClient = new HttpClient();
                string baseUrl = "https://msging.net/commands";//objConfig.BaseUrlBlipCommand;

                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, baseUrl);
                requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");
                requestMessage.Headers.Add("Authorization", chatbotKey);

                HttpResponseMessage responseMessage = await httpClient.SendAsync(requestMessage);
                resposta = await responseMessage.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                
            }

            return resposta;
        }

        public string RequisicaoRules()
        {
            string json = string.Empty;

            json = "{";
            json += "\"id\": \"{{$guid}}\",";
            json += "\"to\": \"postmaster@desk.msging.net\",";
            json += "\"method\": \"get\",";
            json += "\"uri\": \"/rules\"";
            json += "}";

            return json;
        }

        public string RequisicaoObtemFlows()
        {
            string json = string.Empty;

            json = "{";
            json += "\"id\": \"{{$guid}}\",";
            json += "\"method\": \"get\",";
            json += "\"uri\": \"/buckets/blip_portal:builder_published_flow\"";
            json += "}";

            return json;
        }

        public string RequisicaoObtemSchedules(string dataInicio)
        {
            string json = string.Empty;

            json = "{";
            json += "\"id\": \"640aaaca-499a-4b21-8375-1e005b0fa583\",";
            json += "\"method\": \"get\",";
            json += "\"to\": \"postmaster@scheduler.msging.net\",";
            json += "\"uri\": \"/schedules?since=" + dataInicio + "&$skip=0&$take=100\"";
            json += "}";

            return json;
        }
        public string RequisicaoObtemMensagensBroadcast(string idLista, string idMensagem)
        {
            string json = string.Empty;

            json = "{";
            json += "\"id\": \"640aaaca-499a-4b21-8375-1e005b0fa583\",";
            json += "\"method\": \"get\",";
            json += "\"to\": \"postmaster@broadcast.msging.net\",";
            json += "\"uri\": \"/lists/" + idLista + "/messages/" + idMensagem + "\"";
            json += "}";

            return json;
        }

        public string RequisicaoObtemContatos()
        {
            string json = string.Empty;

            json = "{";
            json += "\"id\": \"{{$guid}}\",";
            json += "\"method\": \"get\",";
            json += "\"uri\": \"/contacts\"";
            json += "}";

            return json;
        }

        public string RequisicaoObtemContatoPorNome(string nome)
        {
            string json = string.Empty;
            nome = Uri.EscapeUriString(nome);

            json = "{";
            json += "\"id\": \"{{$guid}}\",";
            json += "\"method\": \"get\",";
            json += "\"uri\": \"/contacts?$skip=0&$take=1&$filter=(name%20eq%20'"+nome+"')\"";
            json += "}";

            return json;
        }

        public string SimulaErro()
        {
            string json = string.Empty;
            json = "{";
            json += "\"method\": \"get\",";
            json += "\"status\": \"failure\",";
            json += "\"reason\": {";
            json += "\"code\": 67,";
            json += "\"description\": \"The requested resource was not found\"";
            json += "},";
            json += "\"id\": \"79587202-dfa5-4e0a-b7f1-73677b72cd52\",";
            json += "\"from\": \"postmaster@crm.msging.net/#iris-hosted-3\",";
            json += "\"to\": \"iveco@msging.net/!iris-hosted-3-sxpj4dku\",";
            json += "\"metadata\": {";
            json += "\"#command.uri\": \"lime://iveco@msging.net/contacts/55318358019\",";
            json += "\"uber-trace-id\": \"88e7a00c607a3a3e%3A5970e7bd5dec7e9c%3A88e7a00c607a3a3e%3A1\"";
            json += "}";
            json += "}";

            return json;
        }

        public string RequisicaoObtemContatosFracionados(int skip)
        {
            string json = string.Empty;

            json = "{";
            json += "\"id\": \"{{$guid}}\",";
            json += "\"method\": \"get\",";
            json += "\"uri\": \"/contacts?$skip=" + skip + "\"";
            json += "}";

            return json;
        }

        public string RequisicaoObtemContatosPorData(DateTime dataInicio, DateTime dataFim)
        {
            string json = string.Empty;

            json = "{";
            json += "\"id\": \"{{$guid}}\",";
            json += "\"method\": \"get\",";
            json += "\"uri\": \"/contacts?$skip=0&$take=100&$filter=(lastmessagedate%20ge%20datetimeoffset'" + FormataData(dataInicio) + "T00:00:00.000Z')%20%20and%20(lastmessagedate%20le%20datetimeoffset'" + FormataData(dataFim) + "T23:59:00.000Z')%20\"";
            json += "}";

            return json;
        }

        public string RequisicaoAtendentes()
        {
            string json = string.Empty;

            json = "{";
            json += "\"id\": \"{{$guid}}\",";
            json += "\"to\": \"postmaster@desk.msging.net\",";
            json += "\"method\": \"get\",";
            json += "\"uri\": \"/attendants\"";
            json += "}";

            return json;
        }

        public string RequisicaoObtemTickets()
        {
            string json = string.Empty;

            json = "{";
            json += "\"id\": \"{{$guid}}\",";
            json += "\"to\": \"postmaster@desk.msging.net\",";
            json += "\"method\": \"get\",";
            json += "\"uri\": \"/tickets?$take=100\"";
            json += "}";

            return json;
        }

        public string RequisicaoObtemTicketsAnalytics(DateTime dataInicio, DateTime dataFim)
        {
            string json = string.Empty;

            json = "{";
            json += "\"id\": \"{{$guid}}\",";
            json += "\"to\": \"postmaster@desk.msging.net\",";
            json += "\"method\": \"get\",";
            json += "\"uri\": \"/analytics/reports/tickets?beginDate=" + FormataData(dataInicio) + "&endDate=" + FormataData(dataFim) + "\"";
            json += "}";

            return json;
        }

        public string RequisicaoObtemAtendentesAnalytics(DateTime dataInicio, DateTime dataFim)
        {
            string json = string.Empty;

            json = "{";
            json += "\"id\": \"{{$guid}}\",";
            json += "\"to\": \"postmaster@desk.msging.net\",";
            json += "\"method\": \"get\",";
            json += "\"uri\": \"/analytics/reports/attendants?beginDate=" + FormataData(dataInicio) + "&endDate=" + FormataData(dataFim) + "\"";
            json += "}";

            return json;
        }

        public string RequisicaoObtemEquipesAnalytics(DateTime dataInicio, DateTime dataFim)
        {
            string json = string.Empty;

            json = "{";
            json += "\"id\": \"{{$guid}}\",";
            json += "\"to\": \"postmaster@desk.msging.net\",";
            json += "\"method\": \"get\",";
            json += "\"uri\": \"/analytics/reports/teams?beginDate=" + FormataData(dataInicio) + "&endDate=" + FormataData(dataFim) + "\"";
            json += "}";

            return json;
        }

        public string RequisicaoObtemTagsAnalytics(DateTime dataInicio, DateTime dataFim)
        {
            string json = string.Empty;

            json = "{";
            json += "\"id\": \"{{$guid}}\",";
            json += "\"to\": \"postmaster@desk.msging.net\",";
            json += "\"method\": \"get\",";
            json += "\"uri\": \"/analytics/reports/tags?beginDate=" + FormataData(dataInicio) + "&endDate=" + FormataData(dataFim) + "\"";
            json += "}";

            return json;
        }

        public string RequisicaoObtemTimingAnalytics(DateTime dataInicio, DateTime dataFim)
        {
            string json = string.Empty;

            json = "{";
            json += "\"id\": \"{{$guid}}\",";
            json += "\"to\": \"postmaster@desk.msging.net\",";
            json += "\"method\": \"get\",";
            json += "\"uri\": \"/analytics/reports/timings?beginDate=" + FormataData(dataInicio) + "&endDate=" + FormataData(dataFim) + "\"";
            json += "}";

            return json;
        }

        public string RequisicaoObtemTempoAtendimentoAnalytics(DateTime dataInicio, DateTime dataFim)
        {
            string json = string.Empty;

            json = "{";
            json += "\"id\": \"{{$guid}}\",";
            json += "\"to\": \"postmaster@desk.msging.net\",";
            json += "\"method\": \"get\",";
            json += "\"uri\": \"/analytics/reports/attendancetime?beginDate=" + FormataData(dataInicio) + "&endDate=" + FormataData(dataFim) + "\"";
            json += "}";

            return json;
        }

        public string RequisicaoObtemMensagensEnviadasMetrics(DateTime dataInicio, DateTime dataFim)
        {
            string json = string.Empty;

            json = "{";
            json += "\"id\": \"{{$guid}}\",";
            json += "\"to\": \"postmaster@analytics.msging.net\",";
            json += "\"method\": \"get\",";
            json += "\"uri\": \"/metrics/sent-messages/D?startDate=" + FormataData(dataInicio) + "T03%3A00%3A00.000Z&endDate=" + FormataData(dataFim) + "T03%3A00%3A00.000Z\"";
            json += "}";

            return json;
        }

        public string RequisicaoObtemMensagensRecebidasMetrics(DateTime dataInicio, DateTime dataFim)
        {
            string json = string.Empty;

            json = "{";
            json += "\"id\": \"{{$guid}}\",";
            json += "\"to\": \"postmaster@analytics.msging.net\",";
            json += "\"method\": \"get\",";
            json += "\"uri\": \"/metrics/received-messages/D?startDate=" + FormataData(dataInicio) + "T03%3A00%3A00.000Z&endDate=" + FormataData(dataFim) + "T03%3A00%3A00.000Z\"";
            json += "}";

            return json;
        }

        public string RequisicaoObtemUsuariosAtivosMetrics(DateTime dataInicio, DateTime dataFim)
        {
            string json = string.Empty;

            json = "{";
            json += "\"id\": \"{{$guid}}\",";
            json += "\"to\": \"postmaster@analytics.msging.net\",";
            json += "\"method\": \"get\",";
            json += "\"uri\": \"/metrics/active-identity/D?startDate=" + FormataData(dataInicio) + "T03%3A00%3A00.000Z&endDate=" + FormataData(dataFim) + "T03%3A00%3A00.000Z\"";
            json += "}";

            return json;
        }

        public string RequisicaoObtemUsuariosEngajadosMetrics(DateTime dataInicio, DateTime dataFim)
        {
            string json = string.Empty;

            json = "{";
            json += "\"id\": \"{{$guid}}\",";
            json += "\"to\": \"postmaster@analytics.msging.net\",";
            json += "\"method\": \"get\",";
            json += "\"uri\": \"/metrics/engaged-identity/D?startDate=" + FormataData(dataInicio) + "T03%3A00%3A00.000Z&endDate=" + FormataData(dataFim) + "T03%3A00%3A00.000Z\"";
            json += "}";

            return json;
        }

        public string RequisicaoObtemCategorias()
        {
            string json = string.Empty;

            json = "{";
            json += "\"id\": \"{{$guid}}\",";
            json += "\"to\": \"postmaster@analytics.msging.net\",";
            json += "\"method\": \"get\",";
            json += "\"uri\": \"/event-track\"";
            json += "}";

            return json;
        }

        public string RequisicaoObtemCategoria(string categoria, DateTime dataInicio, DateTime dataFim)
        {
            string json = string.Empty;

            json = "{";
            json += "\"id\": \"{{$guid}}\",";
            json += "\"to\": \"postmaster@analytics.msging.net\",";
            json += "\"method\": \"get\",";
            json += "\"uri\": \"/event-track/" + categoria + "?startDate=" + FormataData(dataInicio) + "&endDate=" + FormataData(dataFim) + "&$take=100\"";
            json += "}";

            return json;
        }

        public string RequisicaoObtemBucket(string nomeArquivo)
        {
            string json = string.Empty;

            json = "{";
            json += "\"id\": \"{{$guid}}\",";
            json += "\"to\": \"postmaster@msging.net\",";
            json += "\"method\": \"delete\",";
            json += "\"uri\": \"/buckets/" + nomeArquivo + "\"";
            json += "}";

            return json;
        }

        public string RequisicaoObtemBuckets()
        {
            string json = string.Empty;

            json = "{";
            json += "\"id\": \"{{$guid}}\",";
            json += "\"to\": \"postmaster@msging.net\",";
            json += "\"method\": \"delete\",";
            json += "\"uri\": \"/buckets\"";
            json += "}";

            return json;
        }

        public string RequisicaoChangeUserState(string telAgente, string flowId, string newState)
        {
            string json = string.Empty;

            json = "{";
            json += "\"id\": \"{{random.guid}}\",";
            json += "\"to\": \"postmaster@msging.net\",";
            json += "\"method\": \"set\",";
            json += "\"uri\": \"/contexts/" + telAgente + "@wa.gw.msging.net/stateid%40" + flowId + "\",";
            json += "\"type\": \"text/plain\",";
            json += "\"resource\": \"" + newState + "\"";
            json += "}";

            return json;
        }

        public string RequisicaoDeleteBucket(string nomeArquivo)
        {
            string json = string.Empty;

            json = "{";
            json += "\"id\": \"{{$guid}}\",";
            json += "\"to\": \"postmaster@msging.net\",";
            json += "\"method\": \"delete\",";
            json += "\"uri\": \"/buckets/" + nomeArquivo + "\"";
            json += "}";

            return json;
        }

        public string RequisicaoObtemContatoPorTelefone(string telefone)
        {
            string json = string.Empty;

            json = "{";
            json += "\"id\": \"{{$guid}}\",";
            json += "\"method\": \"get\",";
            json += "\"uri\": \"/contacts?$skip=0&$take=1&$filter=(phoneNumber%20eq%20'" + telefone + "')\"";
            json += "}";

            return json;
        }        

        public string RequisicaoObtemMensagens(string idTicket, bool router)
        {
            string json = string.Empty;

            json = "{";
            json += "\"id\": \"{{$guid}}\",";
            json += "\"to\": \"postmaster@desk.msging.net\",";
            json += "\"method\": \"get\",";

            if (router)
            {
                json += "\"uri\": \"/tickets/" + idTicket + "/messages?getFromOwnerIfTunnel=true&$take=100\"";
            }
            else
            {
                json += "\"uri\": \"/tickets/" + idTicket + "/messages?$take=100\"";
            }

            json += "}";

            return json;
        }

        public string RequisicaoObtemContato(string idContato)
        {
            string json = string.Empty;

            json = "{";
            json += "\"id\": \"{{$guid}}\",";
            json += "\"method\": \"get\",";
            json += "\"uri\": \"/contacts/" + idContato + "\"";
            json += "}";

            return json;
        }

        public string RequisicaoInsereRules(string id, bool isActive, string property, string relation, string team, string title, int priority, string ownerIdentity, List<string> values)
        {
            string json = string.Empty;

            json = "{";
            json += "\"id\": \"{{$guid}}\",";
            json += "\"to\": \"postmaster@desk.msging.net\",";
            json += "\"method\": \"set\",";
            json += "\"uri\": \"/rules\",";
            json += "\"type\": \"application/vnd.iris.desk.rule+json\",";
            json += "\"resource\": {";
            json += "\"id\": \"" + id + "\",";
            json += "\"isActive\": " + isActive.ToString().ToLower() + ",";
            json += "\"ownerIdentity\": \"" + ownerIdentity + "\",";
            json += "\"property\": \"" + property + "\",";
            json += "\"relation\": \"" + relation + "\",";
            json += "\"team\": \"" + team + "\",";
            json += "\"title\": \"" + title + "\",";
            json += "\"priority\": \"" + priority + "\",";
            json += "\"values\": [";

            foreach (string item in values)
            {
                json += "\"" + item + "\",";
            }

            json = json.Remove(json.Length - 1, 1);

            json += "]";
            json += "}";
            json += "}";

            return json.Trim();
        }

        public string RequisicaoInsereAtendentes(string identity, string fullname, string email, List<string> teams)
        {
            string json = string.Empty;

            json = "{";
            json += "\"id\": \"{{$guid}}\",";
            json += "\"to\": \"postmaster@desk.msging.net\",";
            json += "\"method\": \"set\",";
            json += "\"uri\": \"/attendants\",";
            json += "\"type\": \"application/vnd.iris.desk.attendant+json\",";
            json += "\"resource\": {";
            json += "\"identity\": \"" + identity + "\",";
            json += "\"fullname\": \"" + fullname + "\",";
            json += "\"email\": \"" + email + "\",";
            json += "\"teams\": [";

            foreach (string item in teams)
            {
                json += "\"" + item + "\",";
            }

            json = json.Remove(json.Length - 1, 1);

            json += "]";
            json += "}";
            json += "}";

            return json.Trim();
        }

        public string RequisicaoObtemMenusMaisAcessados()
        {
            string json = string.Empty;
            DateTime dtbegin = DateTime.Now;
            dtbegin = dtbegin.AddMonths(-3);

            string dtInicio = dtbegin.Year + "-" + dtbegin.Month + "-" + dtbegin.Day;
            string dtFim = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;

            json = "{";
            json += "\"id\": \"{{$guid}}\",";
            json += "\"to\": \"postmaster@analytics.msging.net\",";
            json += "\"method\": \"get\",";
            json += "\"uri\": \"/event-track/flow?startDate=" + dtInicio + "&endDate=" + dtFim + "&$take=100\"";
            json += "}";

            return json;
        }

        public string RequisicaoNotificacaoTexto(List<string> quickButtons, string telefoneContato, string nameSpace, string nomeMensagemTemplate, List<string> parametros)
        {
            string json = string.Empty;
            string param = string.Empty;

            if (parametros != null && parametros.Count > 0)
            {
                for (int i = 0; i < parametros.Count; i++)
                {
                    param += "{\"type\":\"text\",\"text\":\"" + parametros[i] + "\"},";
                }

                param = param.Remove(param.Length - 1, 1);
                json = "{\"id\":\"{{$guid}}\",\"to\":\"" + telefoneContato + "@wa.gw.msging.net\",\"type\":\"application/json\",\"content\":{\"type\":\"template\",\"template\":{\"namespace\":\"" + nameSpace + "\",\"name\":\"" + nomeMensagemTemplate + "\",\"language\":{\"code\":\"pt_BR\",\"policy\":\"deterministic\"},\"components\":[{\"type\":\"body\",\"parameters\":[" + param + "]}]}}}";
            }
            else
            {
                json = "{\"id\":\"{{$guid}}\",\"to\":\"" + telefoneContato + "@wa.gw.msging.net\",\"type\":\"application/json\",\"content\":{\"type\":\"template\",\"template\":{\"namespace\":\"" + nameSpace + "\",\"name\":\"" + nomeMensagemTemplate + "\",\"language\":{\"code\":\"pt_BR\",\"policy\":\"deterministic\"}}}}";
            }

            json = json.Replace("\\/ ", "/").Replace("\\", string.Empty);
            return json.Trim();
        }

        public string RequisicaoNotificacaoImagem(string telefone, string nameSpace, string nomeTemplate, string linkImagem, List<string> replayButtons, List<string> paramentros)
        {
            string pars = string.Empty;
            string qbuttons = string.Empty;

            if (paramentros.Count > 0)
            {
                pars += "},";
                pars += "{";
                pars += "\"type\": \"body\",";
                pars += "\"parameters\": [";

                for (int i = 0; i < paramentros.Count; i++)
                {
                    pars += "{";
                    pars += "\"type\": \"text\",";
                    pars += "\"text\": \"" + paramentros[i] + "\"";
                    pars += "},";
                }

                pars = pars.Remove(pars.Length - 1, 1);
                pars += "]";
                pars += "}";
            }

            if (replayButtons.Count > 0)
            {
                qbuttons += ",";

                for (int i = 0; i < replayButtons.Count; i++)
                {
                    qbuttons += "{";
                    qbuttons += "\"type\": \"button\",";
                    qbuttons += "\"sub_type\": \"quick_reply\",";
                    qbuttons += "\"index\": \"" + i + "\",";
                    qbuttons += "\"parameters\": [";
                    qbuttons += "{";
                    qbuttons += "\"type\": \"payload\",";
                    qbuttons += "\"text\": \"" + replayButtons[i] + "\"";
                    qbuttons += "}";
                    qbuttons += "]";
                    qbuttons += "},";
                }

                qbuttons = qbuttons.Remove(qbuttons.Length - 1, 1);
            }

            string json = "{";
            json += "\"id\": \"{{$guid}}\",";
            json += "\"to\": \"" + telefone + "@wa.gw.msging.net\",";
            json += "\"type\": \"application/json\",";
            json += "\"content\": {";
            json += "\"type\": \"template\",";
            json += "\"template\": {";
            json += "\"namespace\": \"" + nameSpace + "\",";
            json += "\"name\": \"" + nomeTemplate + "\",";
            json += "\"language\": {";
            json += "\"code\": \"pt_BR\",";
            json += "\"policy\": \"deterministic\"";
            json += "},";
            json += "\"components\": [";
            json += "{";
            json += "\"type\": \"header\",";
            json += "\"parameters\": [";
            json += "{";
            json += "\"type\": \"image\",";
            json += "\"image\": {";
            json += "\"link\": \"" + linkImagem + "\"";
            json += "}";
            json += "}";
            json += "]";
            json += pars + qbuttons;
            json += "]";
            json += "}";
            json += "}";
            json += "}";

            return json;
        }

        public string RequisicaoNotificacaoVideo(string telefone, string nameSpace, string nomeTemplate, string linkVideo, List<string> replayButtons, List<string> paramentros)
        {
            string pars = string.Empty;
            string qbuttons = string.Empty;

            if (paramentros.Count > 0)
            {
                pars += "},";
                pars += "{";
                pars += "\"type\": \"body\",";
                pars += "\"parameters\": [";

                for (int i = 0; i < paramentros.Count; i++)
                {
                    pars += "{";
                    pars += "\"type\": \"text\",";
                    pars += "\"text\": \"" + paramentros[i] + "\"";
                    pars += "},";
                }

                pars = pars.Remove(pars.Length - 1, 1);
                pars += "]";
                pars += "}";
            }

            if (replayButtons.Count > 0)
            {
                qbuttons += ",";

                for (int i = 0; i < replayButtons.Count; i++)
                {
                    qbuttons += "{";
                    qbuttons += "\"type\": \"button\",";
                    qbuttons += "\"sub_type\": \"quick_reply\",";
                    qbuttons += "\"index\": \"" + i + "\",";
                    qbuttons += "\"parameters\": [";
                    qbuttons += "{";
                    qbuttons += "\"type\": \"payload\",";
                    qbuttons += "\"text\": \"" + replayButtons[i] + "\"";
                    qbuttons += "}";
                    qbuttons += "]";
                    qbuttons += "},";
                }

                qbuttons = qbuttons.Remove(qbuttons.Length - 1, 1);
            }

            string json = "{";
            json += "\"id\": \"{{$guid}}\",";
            json += "\"to\": \"" + telefone + "@wa.gw.msging.net\",";
            json += "\"type\": \"application/json\",";
            json += "\"content\": {";
            json += "\"type\": \"template\",";
            json += "\"template\": {";
            json += "\"namespace\": \"" + nameSpace + "\",";
            json += "\"name\": \"" + nomeTemplate + "\",";
            json += "\"language\": {";
            json += "\"code\": \"pt_BR\",";
            json += "\"policy\": \"deterministic\"";
            json += "},";
            json += "\"components\": [";
            json += "{";
            json += "\"type\": \"header\",";
            json += "\"parameters\": [";
            json += "{";
            json += "\"type\": \"video\",";
            json += "\"video\": {";
            json += "\"link\": \"" + linkVideo + "\"";
            json += "}";
            json += "}";
            json += "]";
            json += pars + qbuttons;
            json += "]";
            json += "}";
            json += "}";
            json += "}";

            return json;
        }

        public string RequisicaoNotificacaoDocumento(string telefone, string nameSpace, string nomeTemplate, string linkDocumento, string nomeArquivo, List<string> replayButtons, List<string> paramentros)
        {
            string pars = string.Empty;
            string qbuttons = string.Empty;

            if (paramentros.Count > 0)
            {
                pars += "},";
                pars += "{";
                pars += "\"type\": \"body\",";
                pars += "\"parameters\": [";

                for (int i = 0; i < paramentros.Count; i++)
                {
                    pars += "{";
                    pars += "\"type\": \"text\",";
                    pars += "\"text\": \"" + paramentros[i] + "\"";
                    pars += "},";
                }

                pars = pars.Remove(pars.Length - 1, 1);
                pars += "]";
                pars += "}";
            }

            if (replayButtons.Count > 0)
            {
                qbuttons += ",";

                for (int i = 0; i < replayButtons.Count; i++)
                {
                    qbuttons += "{";
                    qbuttons += "\"type\": \"button\",";
                    qbuttons += "\"sub_type\": \"quick_reply\",";
                    qbuttons += "\"index\": \"" + i + "\",";
                    qbuttons += "\"parameters\": [";
                    qbuttons += "{";
                    qbuttons += "\"type\": \"payload\",";
                    qbuttons += "\"text\": \"" + replayButtons[i] + "\"";
                    qbuttons += "}";
                    qbuttons += "]";
                    qbuttons += "},";
                }

                qbuttons = qbuttons.Remove(qbuttons.Length - 1, 1);
            }

            string json = "{";
            json += "\"id\": \"{{$guid}}\",";
            json += "\"to\": \"" + telefone + "@wa.gw.msging.net\",";
            json += "\"type\": \"application/json\",";
            json += "\"content\": {";
            json += "\"type\": \"template\",";
            json += "\"template\": {";
            json += "\"namespace\": \"" + nameSpace + "\",";
            json += "\"name\": \"" + nomeTemplate + "\",";
            json += "\"language\": {";
            json += "\"code\": \"pt_BR\",";
            json += "\"policy\": \"deterministic\"";
            json += "},";
            json += "\"components\": [";
            json += "{";
            json += "\"type\": \"header\",";
            json += "\"parameters\": [";
            json += "{";
            json += "\"type\": \"document\",";
            json += "\"document\": {";
            json += "\"filename\": \"" + nomeArquivo + "\",";
            json += "\"link\": \"" + linkDocumento + "\"";
            json += "}";
            json += "}";
            json += "]";
            json += pars + qbuttons;
            json += "]";
            json += "}";
            json += "}";
            json += "}";

            return json;
        }

        public bool VerificaErroServidorBlip(JToken objJson)
        {
            if (objJson["resource"] == null) { return false; }
            if (objJson["resource"]["items"] == null) { return false; }

            return true;
        }

        public string RequisicaoInsereIntencao(string nome)
        {
            string json = string.Empty;

            json = "{";
            json += "\"id\": \"{{$guid}}\",";
            json += "\"to\": \"postmaster@ai.msging.net\",";
            json += "\"method\": \"set\",";
            json += "\"uri\": \"/intentions\",";
            json += "\"type\": \"application/vnd.iris.ai.intention+json\",";
            json += "\"resource\": {";
            json += "\"name\": \"" + nome + "\"";            
            json += "}";
            json += "}";

            return json;
        }

        public string RequisicaoInsereQuestaoIntencao(string idIntencao, List<string> frases)
        {
            string json = string.Empty;

            json = "{";
            json += "\"id\": \"{{$guid}}\",";
            json += "\"to\": \"postmaster@ai.msging.net\",";
            json += "\"method\": \"set\",";
            json += "\"uri\": \"/intentions/"+idIntencao+"/questions\",";
            json += "\"type\": \"application/vnd.lime.collection+json\",";
            json += "\"resource\": {";
            json += "\"itemType\": \"application/vnd.iris.ai.question+json\",";
            json += "\"items\":[";

            foreach (string frase in frases)
            {
                json += "{";
                json += "\"text\": \""+frase+"\"";
                json += "},";
            }

            json = json.Remove(json.Length - 1, 1);

            json += "]";
            json += "}";
            json += "}";

            return json;
        }

        public string RequisicaoObtemQuestoes(string idIntencao)
        {
            string json = string.Empty;

            json = "{";
            json += "\"id\": \"{{$guid}}\",";
            json += "\"to\": \"postmaster@ai.msging.net\",";
            json += "\"method\": \"get\",";
            json += "\"uri\": \"/intentions/" + idIntencao + "/questions\"";
            json += "}";

            return json;
        }

        public string RequisicaoInsereContent(string nome, string id, string resposta, string intencao, List<string> entidades)
        {
            string json = string.Empty;
            string tempEntidades = string.Empty;

            json = "{";
            json += "\"id\":\"{{$guid}}\",";
            json += "\"to\":\"postmaster@ai.msging.net\",";
            json += "\"method\":\"set\",";
            json += "\"uri\":\"/content\",";
            json += "\"resource\":{";
            json += "\"id\":\"" + id + "\",";
            json += "\"name\":\"" + nome + "\",";
            json += "\"result\":{";
            json += "\"type\":\"text/plain\",";
            json += "\"content\":\"" + resposta + "\"},";
            json += "\"combinations\":[";
            json += "{";
            json += "\"intent\":\"" + intencao + "\",";

            foreach (string entidade in entidades)
            {
                tempEntidades += "\"" + entidade + "\",";
            }

            tempEntidades = tempEntidades.Remove(tempEntidades.Length - 1, 1);

            json += "\"entities\":[" + tempEntidades + "],";
            json += "\"minEntityMatch\":1,";
            json += "\"intentName\":\"intent\"";
            json += "}";
            json += "]";
            json += "},";
            json += "\"type\":\"application/vnd.iris.ai.content-result+json\"";
            json += "}";

            return json;
        }

        public string FormataData(DateTime data)
        {
            return data.Year.ToString().PadLeft(4, '0') + "-" + data.Month.ToString().PadLeft(2, '0') + "-" + data.Day.ToString().PadLeft(2, '0');
        }
    }
}
