namespace SupervisorioInstrumentacao
{
    partial class FrmPrincipal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Conexao = new System.IO.Ports.SerialPort(this.components);
            this.txtResultadoSerial = new System.Windows.Forms.TextBox();
            this.lblResultadoSerial = new System.Windows.Forms.Label();
            this.btnAnalisar = new System.Windows.Forms.Button();
            this.lblPorta = new System.Windows.Forms.Label();
            this.btnRecarregarPortas = new System.Windows.Forms.Button();
            this.cmbVelocidade = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnConectar = new System.Windows.Forms.Button();
            this.cmbPortas = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // Conexao
            // 
            this.Conexao.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.Conexao_DataReceived);
            // 
            // txtResultadoSerial
            // 
            this.txtResultadoSerial.Location = new System.Drawing.Point(12, 66);
            this.txtResultadoSerial.Multiline = true;
            this.txtResultadoSerial.Name = "txtResultadoSerial";
            this.txtResultadoSerial.ReadOnly = true;
            this.txtResultadoSerial.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResultadoSerial.Size = new System.Drawing.Size(762, 245);
            this.txtResultadoSerial.TabIndex = 0;
            // 
            // lblResultadoSerial
            // 
            this.lblResultadoSerial.AutoSize = true;
            this.lblResultadoSerial.Location = new System.Drawing.Point(12, 50);
            this.lblResultadoSerial.Name = "lblResultadoSerial";
            this.lblResultadoSerial.Size = new System.Drawing.Size(127, 13);
            this.lblResultadoSerial.TabIndex = 1;
            this.lblResultadoSerial.Text = "Resultado da Serial:";
            // 
            // btnAnalisar
            // 
            this.btnAnalisar.Location = new System.Drawing.Point(699, 317);
            this.btnAnalisar.Name = "btnAnalisar";
            this.btnAnalisar.Size = new System.Drawing.Size(75, 23);
            this.btnAnalisar.TabIndex = 2;
            this.btnAnalisar.Text = "Analisar";
            this.btnAnalisar.UseVisualStyleBackColor = true;
            this.btnAnalisar.Click += new System.EventHandler(this.btnAnalisar_Click);
            // 
            // lblPorta
            // 
            this.lblPorta.AutoSize = true;
            this.lblPorta.Location = new System.Drawing.Point(12, 13);
            this.lblPorta.Name = "lblPorta";
            this.lblPorta.Size = new System.Drawing.Size(49, 13);
            this.lblPorta.TabIndex = 4;
            this.lblPorta.Text = "Portas:";
            // 
            // btnRecarregarPortas
            // 
            this.btnRecarregarPortas.Font = new System.Drawing.Font("Consolas", 7.8F);
            this.btnRecarregarPortas.Image = global::SupervisorioInstrumentacao.Properties.Resources.refresh_128;
            this.btnRecarregarPortas.Location = new System.Drawing.Point(112, 27);
            this.btnRecarregarPortas.Margin = new System.Windows.Forms.Padding(2);
            this.btnRecarregarPortas.Name = "btnRecarregarPortas";
            this.btnRecarregarPortas.Size = new System.Drawing.Size(23, 20);
            this.btnRecarregarPortas.TabIndex = 5;
            this.btnRecarregarPortas.UseVisualStyleBackColor = true;
            this.btnRecarregarPortas.Click += new System.EventHandler(this.btnRecarregarPortas_Click);
            // 
            // cmbVelocidade
            // 
            this.cmbVelocidade.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVelocidade.Font = new System.Drawing.Font("Consolas", 7.8F);
            this.cmbVelocidade.FormattingEnabled = true;
            this.cmbVelocidade.Items.AddRange(new object[] {
            "4800",
            "9600",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.cmbVelocidade.Location = new System.Drawing.Point(139, 28);
            this.cmbVelocidade.Margin = new System.Windows.Forms.Padding(2);
            this.cmbVelocidade.Name = "cmbVelocidade";
            this.cmbVelocidade.Size = new System.Drawing.Size(92, 20);
            this.cmbVelocidade.TabIndex = 17;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Consolas", 7.8F);
            this.label2.Location = new System.Drawing.Point(136, 14);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Velocidade:";
            // 
            // btnConectar
            // 
            this.btnConectar.Font = new System.Drawing.Font("Consolas", 7.8F);
            this.btnConectar.Location = new System.Drawing.Point(235, 25);
            this.btnConectar.Margin = new System.Windows.Forms.Padding(2);
            this.btnConectar.Name = "btnConectar";
            this.btnConectar.Size = new System.Drawing.Size(90, 25);
            this.btnConectar.TabIndex = 19;
            this.btnConectar.Text = "Conectar";
            this.btnConectar.UseVisualStyleBackColor = true;
            this.btnConectar.Click += new System.EventHandler(this.btnConectar_Click);
            // 
            // cmbPortas
            // 
            this.cmbPortas.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPortas.Font = new System.Drawing.Font("Consolas", 7.8F);
            this.cmbPortas.FormattingEnabled = true;
            this.cmbPortas.Location = new System.Drawing.Point(11, 28);
            this.cmbPortas.Margin = new System.Windows.Forms.Padding(2);
            this.cmbPortas.Name = "cmbPortas";
            this.cmbPortas.Size = new System.Drawing.Size(97, 20);
            this.cmbPortas.TabIndex = 21;
            // 
            // FrmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(786, 352);
            this.Controls.Add(this.cmbPortas);
            this.Controls.Add(this.btnConectar);
            this.Controls.Add(this.cmbVelocidade);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnRecarregarPortas);
            this.Controls.Add(this.lblPorta);
            this.Controls.Add(this.btnAnalisar);
            this.Controls.Add(this.lblResultadoSerial);
            this.Controls.Add(this.txtResultadoSerial);
            this.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FrmPrincipal";
            this.Text = "Supervisório Instrumentação";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.IO.Ports.SerialPort Conexao;
        private System.Windows.Forms.TextBox txtResultadoSerial;
        private System.Windows.Forms.Label lblResultadoSerial;
        private System.Windows.Forms.Button btnAnalisar;
        private System.Windows.Forms.Label lblPorta;
        private System.Windows.Forms.Button btnRecarregarPortas;
        private System.Windows.Forms.ComboBox cmbVelocidade;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnConectar;
        private System.Windows.Forms.ComboBox cmbPortas;
    }
}