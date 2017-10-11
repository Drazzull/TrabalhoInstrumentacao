namespace SupervisorioInstrumentacao
{
    using System;
    using System.Collections.Generic;
    using System.IO.Ports;
    using System.Threading;
    using System.Windows.Forms;

    public partial class FrmPrincipal : Form
    {
        #region Campos
        /// <summary>
        /// Lista que conterá os caracteres recebidos
        /// </summary>
        private List<byte> listaBytes;

        /// <summary>
        /// Lista que conterá os caracteres recebidos
        /// </summary>
        private List<double> listaDouble;

        /// <summary>
        /// Lista que conterá os caracteres recebidos
        /// </summary>
        private List<char> listaChar;
        #endregion

        #region Propriedades
        /// <summary>
        /// Obtém ou define o valor da StringRx
        /// </summary>
        private string StringRx { get; set; }

        /// <summary>
        /// Obtém ou define o valor da lista de bytes
        /// </summary>
        private List<byte> ListaBytes
        {
            get
            {
                if (this.listaBytes == null)
                {
                    this.listaBytes = new List<byte>();
                }

                return this.listaBytes;
            }
        }

        /// <summary>
        /// Obtém ou define o valor da lista de bytes
        /// </summary>
        private List<char> ListaChar
        {
            get
            {
                if (this.listaChar == null)
                {
                    this.listaChar = new List<char>();
                }

                return this.listaChar;
            }
        }

        /// <summary>
        /// Obtém ou define o valor da lista de double
        /// </summary>
        private List<double> ListaDouble
        {
            get
            {
                if (this.listaDouble == null)
                {
                    this.listaDouble = new List<double>();
                }

                return this.listaDouble;
            }
        }

        /// <summary>
        /// Obtém ou define o valor da variável para término da leitura de dados
        /// </summary>
        private DateTime TerminoLeituraDados { get; set; }
        #endregion

        #region Construtor
        /// <summary>
        /// Construtor da tela principal
        /// </summary>
        public FrmPrincipal()
        {
            this.InitializeComponent();

            // Inicializa o combo com as portas disponíveis no sistema
            this.CarregarListaPortas();

            // Inicializa o estado dos componentes
            this.cmbVelocidade.SelectedIndex = 0;
            this.AlterarEstadoComponentes();
        }
        #endregion

        #region Métodos
        #region Componentes do Form
        /// <summary>
        /// Evento Click do botão para Recarregar Portas
        /// </summary>
        /// <param name="sender">Objeto sender</param>
        /// <param name="e">Objeto EventArgs</param>
        private void btnRecarregarPortas_Click(object sender, EventArgs e)
        {
            this.CarregarListaPortas();
        }

        /// <summary>
        /// Evento Click do botão de conexão
        /// </summary>
        /// <param name="sender">Objeto sender</param>
        /// <param name="e">Objeto EventArgs</param>
        private void btnConectar_Click(object sender, EventArgs e)
        {
            try
            {
                if ((this.cmbPortas.SelectedItem == null) ||
                    string.IsNullOrEmpty(this.cmbPortas.SelectedItem.ToString()))
                {
                    throw new Exception("A porta selecionada é inválida.");
                }

                // Inicia a conexão
                this.Conexao.PortName = this.cmbPortas.SelectedItem.ToString();
                this.Conexao.BaudRate = int.Parse(this.cmbVelocidade.SelectedItem.ToString());
                this.Conexao.Open();
                this.Conexao.DiscardInBuffer();
                this.txtResultadoSerial.AppendText("Enviado: #C7" + Environment.NewLine);
                this.Conexao.Write("#C7");
                this.txtResultadoSerial.AppendText(this.Conexao.ReadLine());
                this.txtResultadoSerial.AppendText("Enviado: #SS" + Environment.NewLine);
                this.Conexao.Write("#SS");
                this.txtResultadoSerial.AppendText("Conectado com Sucesso" + Environment.NewLine);

                // Limpa a lista
                this.ListaBytes.Clear();

                // Aguarda 5 segundos
                Thread.Sleep(5000);

                // Verifica o número de bytes a serem lidos
                int byteCount = this.Conexao.BytesToRead;

                // Cria um array para conter o buffer, do tamanho necessário
                char[] buffer = new char[byteCount];

                // Faz a leitura dos bytes
                int readBytes = this.Conexao.Read(buffer, 0, byteCount);

                // Acrescenta os bytes à lista
                this.ListaChar.AddRange(buffer);

                // Para a Conexão
                this.txtResultadoSerial.AppendText("Enviado: #P" + Environment.NewLine);
                this.Conexao.Write("#P");
                this.Conexao.Close();

                this.txtResultadoSerial.AppendText("Serial Parada.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format(
                        "[Erro]{1}{0}{1}{1}[StackTrace]{1}{2}{1} Porta: {3}{1} Velocidade: {4}",
                        ex.Message,
                        Environment.NewLine,
                        ex.StackTrace,
                        this.cmbPortas.SelectedItem == null ? "Nulo" : this.cmbPortas.SelectedItem.ToString(),
                        this.cmbVelocidade.SelectedItem == null ? "Nulo" : this.cmbVelocidade.SelectedItem.ToString()),
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento DataReceived da conexão serial
        /// </summary>
        /// <param name="sender">Objeto Sender</param>
        /// <param name="e">Objeto SerialDataReceivedEventArgs</param>
        private void Conexao_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {

            }
            catch
            {
            }
        }

        /// <summary>
        /// Evento click do botão de análise dos dados
        /// </summary>
        /// <param name="sender">Objeto sender</param>
        /// <param name="e">Objeto EventArgs</param>
        private void btnAnalisar_Click(object sender, EventArgs e)
        {
            // Abre a tela para apresentar o gráfico
            FrmChart frmChart = new FrmChart();
            frmChart.ShowDialog();
        }
        #endregion

        #region Privados
        /// <summary>
        /// Carrega a lista com as portas disponíveis no sistema
        /// </summary>
        private void CarregarListaPortas()
        {
            try
            {
                // Grava a porta selecionada
                string portaConectada = string.Empty;
                if (this.cmbPortas.SelectedValue != null)
                {
                    portaConectada = this.cmbPortas.SelectedValue.ToString();
                }

                // Limpa a lista das portas
                this.cmbPortas.Items.Clear();

                // Obtém a lista de portas do sistema
                string[] ports = SerialPort.GetPortNames();
                foreach (string port in ports)
                {
                    int convert = 0;
                    if (!int.TryParse(port.Substring(3), out convert))
                    {
                        continue;
                    }

                    this.cmbPortas.Items.Add("COM" + convert.ToString());
                }

                // Seleciona a porta que estava selecionada anteriormente
                for (int i = 0; i < this.cmbPortas.Items.Count; i++)
                {
                    if (this.cmbPortas.Items[i].ToString() == portaConectada)
                    {
                        this.cmbPortas.SelectedIndex = i;
                    }
                }

                // Atualiza o estado dos componentes
                this.AlterarEstadoComponentes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format(
                        "Ocorreu o seguinte erro ao atualizar as portas:{0}[Erro]{1}{0}Tente novamente.",
                        Environment.NewLine,
                        ex.Message),
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Altera o estado dos componentes conforme a lista de portas
        /// </summary>
        private void AlterarEstadoComponentes()
        {
            try
            {
                // Habilita o combo das portas e o botão de conexão apenas quando existirem portas no computador
                this.cmbPortas.Enabled = this.cmbPortas.Items.Count > 0;
                this.btnConectar.Enabled = this.cmbPortas.Items.Count > 0;
                if ((this.cmbPortas.Items.Count > 0) && (this.cmbPortas.SelectedValue == null))
                {
                    this.cmbPortas.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format(
                        "Ocorreu o seguinte erro ao atualizar os componentes:{0}[Erro]{1}{0}",
                        Environment.NewLine,
                        ex.Message),
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Altera o textbox com as informações lidas da serial
        /// </summary>
        /// <param name="sender">Objeto Sender</param>
        /// <param name="e">Objeto EventArgs</param>
        private void AlteraTxtResultado(object sender, EventArgs e)
        {
            this.txtResultadoSerial.AppendText(string.Format("Dados: {0}{1}", this.StringRx, Environment.NewLine));
        }
        #endregion
        #endregion
    }
}