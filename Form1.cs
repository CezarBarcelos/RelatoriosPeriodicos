using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using RelatoriosPeriodicos.Servicos;
using RelatoriosPeriodicos.Entidades;
using System.IO;

namespace RelatoriosPeriodicos
{
    public partial class Form1 : Form
    {
        int qtdeItensNota = 0;
        Banco objBanco;
        Blip objBlip;
        Config objConfig;
        List<Chatbot> listaChatbots;
        List<RegistroTicket> listaRegistroTickets = new List<RegistroTicket>();
        DateTime dtIni = new DateTime();
        DateTime dtFim = new DateTime();
        public Form1()
        {
            InitializeComponent();
            objConfig = new Config();
            objBlip = new Blip(objConfig);
            objBanco = new Banco(objConfig.StringConnection);
        }

        private async void button1_Click(object sender, EventArgs e)
        {            
            //label1.Location = new Point(18, 5);
            label1.TextAlign = ContentAlignment.MiddleCenter;
            label1.Size = new Size(1180, 40);
            //label1.BackColor = Color.Cyan;

            dtIni = monthCalendar1.SelectionStart;
            dtFim = monthCalendar2.SelectionStart;

            label1.Text = dtIni.ToShortDateString() + " à " + dtFim.ToShortDateString(); 

            listaRegistroTickets = new List<RegistroTicket>();
            listaChatbots = new List<Chatbot>();
            MontaListaChatbotsHistorico(objBanco);
            ObtemRegistrosTickets();

            foreach (Chatbot chatbot in listaChatbots)
            {
                RegistroGrid reg = new RegistroGrid();

                string key = string.IsNullOrEmpty(chatbot.RouterKey) ? chatbot.Key : chatbot.RouterKey;

                reg.UsuariosChatbot = await ObtemUsuariosAtivos(key, dtIni, dtFim);                                    
                reg.MensagensEnviadas = await ObtemMensagensEnviadas(key, dtIni, dtFim);
                reg.MensagensRecebidas = await ObtemMensagensRecebidas(key, dtIni, dtFim);
                reg.Nota = await ObtemNotasNPS(key, dtIni, dtFim);
                if (dtIni.Month <= (DateTime.Now.Month - 3))
                {
                    int val = BuscaTickets(chatbot.Key, dtIni, dtFim);
                    int val2 = await ObtemTickets(chatbot.Key, dtIni, dtFim);
                    reg.UsuariosPA = val + val2;                    
                }
                else
                {
                    reg.UsuariosPA = await ObtemTickets(chatbot.Key, dtIni, dtFim);
                }

                if (reg.UsuariosPA == 0) { reg.UsuariosPA = qtdeItensNota; }
                dataGridView1.Rows.Add(chatbot.NomeBase, reg.MensagensEnviadas, reg.MensagensRecebidas, reg.UsuariosChatbot, reg.UsuariosPA, reg.Nota);                
            }
        }

        private int BuscaTickets(string key, DateTime dtIni, DateTime dtFim)
        {
            int soma = 0;
            List<RegistroPeriodo> listaRegistrosPeriodo = new List<RegistroPeriodo>();
            string ano = dtIni.Year.ToString();
            DateTime temp = dtIni;
            while(temp <= dtFim)
            {
                RegistroPeriodo reg = new RegistroPeriodo();
                reg.DataInicio = temp;                
                temp = temp.AddMonths(1);
                reg.DataFim = temp.AddDays(-1);
                listaRegistrosPeriodo.Add(reg);
            }

            foreach (RegistroPeriodo registro in listaRegistrosPeriodo)
            {
                RegistroTicket item = listaRegistroTickets.Find(delegate (RegistroTicket r) { return (r.KeyChatbot == key && r.Ano == registro.DataInicio.Year.ToString() && r.Mes == Convert.ToString(registro.DataFim.Month)); });

                if (item != null)
                {
                    soma += item.Qtde;
                }
            }

            return soma;
        }

        public void MontaListaChatbotsHistorico(Banco objBanco)
        {
            listaChatbots.Clear();

            try
            {
                DataTable tabela;

                if (!string.IsNullOrEmpty(objConfig.Grupo))
                {
                    tabela = objBanco.SELECT("SELECT * FROM Chatbot WHERE AtendimentoHumano = 1 AND Grupo like '" + objConfig.Grupo + "' ORDER BY Nacionalidade");
                }
                else
                {
                    tabela = objBanco.SELECT("SELECT * FROM Chatbot WHERE AtendimentoHumano = 1 ORDER BY Nacionalidade");
                }

                for (int i = 0; i < tabela.Rows.Count; i++)
                {
                    Chatbot chatbot = new Chatbot();

                    chatbot.AutoId = Convert.ToInt32(tabela.Rows[i]["AutoId"]);
                    chatbot.Id = Convert.ToString(tabela.Rows[i]["Id"]);
                    chatbot.Nome = Convert.ToString(tabela.Rows[i]["Nome"]);
                    chatbot.NomeBase = Convert.ToString(tabela.Rows[i]["NomeBase"]);
                    chatbot.Router = Convert.ToBoolean(tabela.Rows[i]["Router"]);
                    chatbot.DateTimeOffset = Convert.ToInt16(tabela.Rows[i]["DateTimeOffset"]);
                    chatbot.Key = Convert.ToString(tabela.Rows[i]["AuthorizationKey"]);
                    chatbot.RouterKey = Convert.ToString(tabela.Rows[i]["RouterAuthorizationKey"]);
                    chatbot.Nacionalidade = Convert.ToString(tabela.Rows[i]["Nacionalidade"]);
                    chatbot.AtendimentoHumano = Convert.ToBoolean(tabela.Rows[i]["AtendimentoHumano"]);
                    chatbot.Broadcast = Convert.ToBoolean(tabela.Rows[i]["Broadcast"]);
                    chatbot.Report = Convert.ToBoolean(tabela.Rows[i]["Report"]);

                    listaChatbots.Add(chatbot);
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private void ObtemRegistrosTickets()
        {
            listaRegistroTickets.Clear();
            string dirApp = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory.ToString());
            string sourcePath = dirApp + "\\Source\\Tickets.dat";

            using (StreamReader sr = new StreamReader(sourcePath))
            {
                while (!sr.EndOfStream)
                {
                    try
                    {
                        string[] linha = sr.ReadLine().Split(',');
                        RegistroTicket rt = new RegistroTicket();
                        rt.Mes = linha[0];
                        rt.Ano = linha[1];
                        rt.Qtde = Convert.ToInt32(linha[2]);
                        rt.NomeChatbot = linha[3];
                        rt.KeyChatbot = linha[4];
                        listaRegistroTickets.Add(rt);
                    }
                    catch { }
                }
            }
        }

        async Task<int> ObtemTickets(string key, DateTime dtIni, DateTime dtFim)
        {
            int qtdeTickets = 0;

            try
            {
                string json = objBlip.RequisicaoObtemTicketsAnalytics(dtIni, dtFim);
                string resposta = await objBlip.ResquestMessage(json, key);

                if (resposta != null)
                {
                    JObject objJson = JObject.Parse(resposta);

                    if (objJson.HasValues)
                    {
                        IList<JToken> results = objJson["resource"]["items"].Children().ToList();

                        foreach (JToken result in results)
                        {
                            Ticket ticket = Newtonsoft.Json.JsonConvert.DeserializeObject<Ticket>(result.ToString());
                            qtdeTickets += ticket.closed;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return qtdeTickets;
        }
        async Task<int> ObtemUsuariosAtivos(string key, DateTime dtIni, DateTime dtFim)
        {
            int qtdeUsuarios = 0;

            try
            {
                string json = objBlip.RequisicaoObtemUsuariosAtivosMetrics(dtIni, dtFim);
                string resposta = await objBlip.ResquestMessage(json, key);

                if (resposta != null)
                {
                    JObject objJson = JObject.Parse(resposta);

                    if (objJson.HasValues)
                    {
                        IList<JToken> results = objJson["resource"]["items"].Children().ToList();

                        for (int j = 0; j < results.Count; j++)
                        {
                            Metricas item = Newtonsoft.Json.JsonConvert.DeserializeObject<Metricas>(results[j].ToString());
                            qtdeUsuarios += item.count;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return qtdeUsuarios;
        }
        async Task<int> ObtemMensagensEnviadas(string key, DateTime dataInicio, DateTime dataFim)
        {
            int mensagensEnviadas = 0;

            try
            {
                string jsonContent = objBlip.RequisicaoObtemMensagensEnviadasMetrics(dataInicio, dataFim);
                string resposta = await objBlip.ResquestMessage(jsonContent, key);

                if (!string.IsNullOrEmpty(resposta))
                {
                    JObject objJson = JObject.Parse(resposta);

                    if (objJson.HasValues)
                    {
                        try
                        {
                            IList<JToken> results = objJson["resource"]["items"].Children().ToList();

                            foreach (JToken result in results)
                            {
                                mensagensEnviadas += Convert.ToInt32(result["count"]);
                            }
                        }
                        catch { }
                    }
                }
            }
            catch
            {
                return 0;
            }

            return mensagensEnviadas;
        }
        async Task<int> ObtemMensagensRecebidas(string key, DateTime dataInicio, DateTime dataFim)
        {
            int mensagensRecebidas = 0;

            try
            {
                string jsonContent = objBlip.RequisicaoObtemMensagensRecebidasMetrics(dataInicio, dataFim);
                string resposta = await objBlip.ResquestMessage(jsonContent, key);

                if (!string.IsNullOrEmpty(resposta))
                {
                    JObject objJson = JObject.Parse(resposta);

                    if (objJson.HasValues)
                    {
                        try
                        {
                            IList<JToken> results = objJson["resource"]["items"].Children().ToList(); //serialize JSON results into .NET objects

                            foreach (JToken result in results)
                            {
                                mensagensRecebidas += Convert.ToInt32(result["count"]);
                            }
                        }
                        catch { }
                    }
                }
            }
            catch
            {
                return 0;
            }

            return mensagensRecebidas;
        }
        async Task<float> ObtemNotasNPS(string key, DateTime dataInicio, DateTime dataFim)
        {
            float media = 0;
            qtdeItensNota = 0;

            try
            {
                string jsonContent = objBlip.RequisicaoObtemCategoria("NotaTicket", dataInicio, dataFim);
                string resposta = await objBlip.ResquestMessage(jsonContent, key);

                if (!string.IsNullOrEmpty(resposta))
                {
                    JObject objJson = JObject.Parse(resposta);

                    if (objJson.HasValues)
                    {
                        IList<JToken> results = objJson["resource"]["items"].Children().ToList();

                        if (results.Count > 0)
                        {
                            qtdeItensNota = results.Count;
                            float mediaNota = 0;
                            float mediaAtendimento = 0;

                            for (int i = 0; i < results.Count; i++)
                            {
                                mediaAtendimento += ObtemValorFloat(results[i]["count"].ToString().Split('|')[0].Trim());
                                mediaNota += ObtemValorFloat(results[i]["action"].ToString().Split('|')[0].Trim().Replace("Nota:", "").Trim()) * ObtemValorFloat(results[i]["count"].ToString().Split('|')[0].Trim());
                            }

                            media = Convert.ToSingle(Convert.ToSingle((mediaNota / mediaAtendimento)).ToString("F1"));
                        }
                    }
                }
            }
            catch { }

            return media;
        }
        async Task<List<Categoria>> ObtemFlow(string key, DateTime dataInicio, DateTime dataFim)
        {
            List<Categoria> listaCategorias = new List<Categoria>();

            try
            {
                string jsonContent = objBlip.RequisicaoObtemCategoria("AssuntoMaisProcurado", dataInicio, dataFim);
                string resposta = await objBlip.ResquestMessage(jsonContent, key);

                if (!string.IsNullOrEmpty(resposta))
                {
                    JObject objJson = JObject.Parse(resposta);

                    if (objJson.HasValues)
                    {
                        IList<JToken> results = objJson["resource"]["items"].Children().ToList();

                        if (results.Count > 0)
                        {
                            foreach (JToken result in results)
                            {
                                Categoria categoria = Newtonsoft.Json.JsonConvert.DeserializeObject<Categoria>(result.ToString());
                                Categoria item = listaCategorias.Find(delegate (Categoria c) { return c.action == categoria.action; });
                                if (item == null)
                                {
                                    listaCategorias.Add(categoria);
                                }
                            }
                        }
                    }
                }
            }
            catch { }

            return listaCategorias;
        }
        private float ObtemValorFloat(string valor)
        {
            for (int i = 0; i < valor.Length; i++)
            {
                if (Char.IsLetter(valor[i]))
                {
                    return 8f;
                }
            }

            try
            {
                return Convert.ToSingle(valor);
            }
            catch
            {
                return 8f;
            }
        }
    }
}
