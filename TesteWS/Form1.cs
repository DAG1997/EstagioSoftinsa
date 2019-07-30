using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Threading;
using ST = System.Timers;
using System.Configuration;
using System.Collections.Specialized;
using System.Globalization;

namespace TesteWS
{
    public partial class Form1 : Form
    {

        WS.ServiceDBSoapClient client = new WS.ServiceDBSoapClient();
        const int TIMER_SECONDS = 30; 

        private readonly ST.Timer clock;

   
        public Form1()
        {
            //Timer
            InitializeComponent();   
            clock = new ST.Timer();
            clock.Interval = TIMER_SECONDS * 1000;   
            Start();
            
            
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now.AddDays(-7);

            
            try
            {
                //Chamada dos sps
                string resultado = client.getEmpName();
                string resultado1 = client.GetDate(DateTime.Now.AddMonths(-7), DateTime.Now);
                string resultado2 = client.GetLogsMade(DateTime.Now.AddMonths(-7), DateTime.Now);
                MessageBox.Show(Environment.UserName, "Utilizador Máquina");

                                          
                if (!ColaboradorRegistosValidados(DateTime.Now))
                {
                    MessageBox.Show("Nao tem as horas registadas, por favor corrija!");
                                      
                                        
                }
            }


            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
               
            }

        }

        #region TIMER
        public void Start()
        {
            clock.Elapsed += Clock_Elapsed;
            this.clock.Start();
        }

        public void Stop()
        {
            clock.Elapsed -= Clock_Elapsed;
            this.clock.Stop();
        }

        private void Clock_Elapsed(object sender, ElapsedEventArgs e)

        {
            //Chama/define as datekeys do app.config
            var agora = DateTime.Now;
            NameValueCollection sAll;
            sAll = ConfigurationManager.AppSettings;
                     

            foreach (string s in sAll.AllKeys)
                Console.WriteLine("Key: " + s + " Value: " + sAll.Get(s));

            //convert namevaluecollection em strings e inteiros
            DayOfWeek diaDaSemana = (DayOfWeek)int.Parse(sAll["DiaDaSemana"]);
            string horas = sAll["HoraDoDia"];
            string minutos = sAll["MinutosDoDia"];
            
            if (agora.DayOfWeek == diaDaSemana)
            {
                //Data em que o sistema deve chamar a atenção do utilizador
                DateTime dataConfigs = new DateTime(agora.Year, agora.Month,agora.Day,Convert.ToInt32(horas),Convert.ToInt32(minutos),0);
                
 
                if (agora >= dataConfigs)
                {
                    MessageBox.Show("Passou a hora de registos!!");

                }
                else if (agora <= dataConfigs)
                {

                }

                                                                            
                Console.WriteLine("Timer elapsed: " + agora.ToString());
            
            }
            else
            {
               
            }
           
        }
        #endregion
     
        public bool ColaboradorRegistosValidados(DateTime dtValidar)
        {
           
          
            return false;
        }
      
        private void Button1_Click(object sender, EventArgs e)
        {
                           
            DialogResult dialog = MessageBox.Show("Tem a certeza que registou as horas?", "Aviso!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dialog == DialogResult.Yes)
                {
                    Application.Exit();
                }
                else if( dialog == DialogResult.No)
                {
               
                }

                else if( dialog == DialogResult.Cancel)
                 {

                 }
            
        }

        private void Button2_Click(object sender, EventArgs e)
        {

            //Chama metodo dos Logs ao botao
            DateTime dataInicio = dateTimePicker1.Value;
            DateTime dataFim = dateTimePicker2.Value;
            string resultado = client.GetLogsMade(dataInicio, dataFim);
            MessageBox.Show(resultado);
           
        }

    }

}
