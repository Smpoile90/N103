namespace TrafficLightServer
{
    partial class FormServer
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
            this.buttonConnect = new System.Windows.Forms.Button();
            this.listBoxOutput = new System.Windows.Forms.ListBox();
            this.labelStatus = new System.Windows.Forms.Label();
            this.comboBoxLightColour = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lsbActiveConnections = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxIPToAdd = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnAddLight = new System.Windows.Forms.Button();
            this.labelAmber = new System.Windows.Forms.Label();
            this.labelGreen = new System.Windows.Forms.Label();
            this.labelRed = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnControl = new System.Windows.Forms.Button();
            this.labelConnection = new System.Windows.Forms.Label();
            this.labelOverride = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(35, 10);
            this.buttonConnect.Margin = new System.Windows.Forms.Padding(2);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(176, 31);
            this.buttonConnect.TabIndex = 0;
            this.buttonConnect.Text = "Connect to sort of proxy";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // listBoxOutput
            // 
            this.listBoxOutput.FormattingEnabled = true;
            this.listBoxOutput.Location = new System.Drawing.Point(35, 148);
            this.listBoxOutput.Margin = new System.Windows.Forms.Padding(2);
            this.listBoxOutput.Name = "listBoxOutput";
            this.listBoxOutput.Size = new System.Drawing.Size(315, 251);
            this.listBoxOutput.TabIndex = 1;
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStatus.Location = new System.Drawing.Point(231, 17);
            this.labelStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(50, 18);
            this.labelStatus.TabIndex = 2;
            this.labelStatus.Text = "Status";
            // 
            // comboBoxLightColour
            // 
            this.comboBoxLightColour.Enabled = false;
            this.comboBoxLightColour.FormattingEnabled = true;
            this.comboBoxLightColour.Items.AddRange(new object[] {
            "Red",
            "Amber",
            "Green"});
            this.comboBoxLightColour.Location = new System.Drawing.Point(901, 76);
            this.comboBoxLightColour.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxLightColour.Name = "comboBoxLightColour";
            this.comboBoxLightColour.Size = new System.Drawing.Size(51, 21);
            this.comboBoxLightColour.TabIndex = 5;
            this.comboBoxLightColour.SelectedIndexChanged += new System.EventHandler(this.comboBoxLightColour_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(843, 79);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Set colour";
            // 
            // lsbActiveConnections
            // 
            this.lsbActiveConnections.FormattingEnabled = true;
            this.lsbActiveConnections.Location = new System.Drawing.Point(519, 148);
            this.lsbActiveConnections.Margin = new System.Windows.Forms.Padding(2);
            this.lsbActiveConnections.Name = "lsbActiveConnections";
            this.lsbActiveConnections.Size = new System.Drawing.Size(238, 251);
            this.lsbActiveConnections.TabIndex = 1;
            this.lsbActiveConnections.SelectedIndexChanged += new System.EventHandler(this.lsbActiveConnections_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(516, 124);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Lights";
            // 
            // textBoxIPToAdd
            // 
            this.textBoxIPToAdd.Location = new System.Drawing.Point(615, 79);
            this.textBoxIPToAdd.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxIPToAdd.Name = "textBoxIPToAdd";
            this.textBoxIPToAdd.Size = new System.Drawing.Size(142, 20);
            this.textBoxIPToAdd.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(516, 82);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "IP of Light to Add";
            // 
            // btnAddLight
            // 
            this.btnAddLight.Location = new System.Drawing.Point(556, 11);
            this.btnAddLight.Margin = new System.Windows.Forms.Padding(2);
            this.btnAddLight.Name = "btnAddLight";
            this.btnAddLight.Size = new System.Drawing.Size(176, 31);
            this.btnAddLight.TabIndex = 0;
            this.btnAddLight.Text = "Add Light";
            this.btnAddLight.UseVisualStyleBackColor = true;
            this.btnAddLight.Click += new System.EventHandler(this.button1_Click);
            // 
            // labelAmber
            // 
            this.labelAmber.AutoSize = true;
            this.labelAmber.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.labelAmber.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAmber.Location = new System.Drawing.Point(990, 62);
            this.labelAmber.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelAmber.Name = "labelAmber";
            this.labelAmber.Size = new System.Drawing.Size(39, 37);
            this.labelAmber.TabIndex = 17;
            this.labelAmber.Text = "A";
            this.labelAmber.Visible = false;
            // 
            // labelGreen
            // 
            this.labelGreen.AutoSize = true;
            this.labelGreen.BackColor = System.Drawing.Color.Lime;
            this.labelGreen.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGreen.Location = new System.Drawing.Point(990, 107);
            this.labelGreen.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelGreen.Name = "labelGreen";
            this.labelGreen.Size = new System.Drawing.Size(42, 37);
            this.labelGreen.TabIndex = 16;
            this.labelGreen.Text = "G";
            this.labelGreen.Visible = false;
            // 
            // labelRed
            // 
            this.labelRed.AutoSize = true;
            this.labelRed.BackColor = System.Drawing.Color.Red;
            this.labelRed.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRed.Location = new System.Drawing.Point(990, 17);
            this.labelRed.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelRed.Name = "labelRed";
            this.labelRed.Size = new System.Drawing.Size(39, 37);
            this.labelRed.TabIndex = 15;
            this.labelRed.Text = "R";
            this.labelRed.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(843, 35);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Light Manager";
            // 
            // btnControl
            // 
            this.btnControl.Location = new System.Drawing.Point(519, 415);
            this.btnControl.Name = "btnControl";
            this.btnControl.Size = new System.Drawing.Size(160, 58);
            this.btnControl.TabIndex = 18;
            this.btnControl.Text = "Manual Control";
            this.btnControl.UseVisualStyleBackColor = true;
            this.btnControl.Click += new System.EventHandler(this.btnControl_Click);
            // 
            // labelConnection
            // 
            this.labelConnection.AutoSize = true;
            this.labelConnection.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelConnection.Location = new System.Drawing.Point(944, 240);
            this.labelConnection.Name = "labelConnection";
            this.labelConnection.Size = new System.Drawing.Size(108, 25);
            this.labelConnection.TabIndex = 19;
            this.labelConnection.Text = "Connected";
            // 
            // labelOverride
            // 
            this.labelOverride.AutoSize = true;
            this.labelOverride.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOverride.Location = new System.Drawing.Point(942, 289);
            this.labelOverride.Name = "labelOverride";
            this.labelOverride.Size = new System.Drawing.Size(87, 25);
            this.labelOverride.TabIndex = 19;
            this.labelOverride.Text = "Override";
            // 
            // FormServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1124, 585);
            this.Controls.Add(this.labelOverride);
            this.Controls.Add(this.labelConnection);
            this.Controls.Add(this.btnControl);
            this.Controls.Add(this.labelAmber);
            this.Controls.Add(this.labelGreen);
            this.Controls.Add(this.labelRed);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxLightColour);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxIPToAdd);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.lsbActiveConnections);
            this.Controls.Add(this.listBoxOutput);
            this.Controls.Add(this.btnAddLight);
            this.Controls.Add(this.buttonConnect);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormServer";
            this.Text = "Server (sort of)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormServer_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.ListBox listBoxOutput;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.ComboBox comboBoxLightColour;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lsbActiveConnections;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxIPToAdd;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnAddLight;
        private System.Windows.Forms.Label labelAmber;
        private System.Windows.Forms.Label labelGreen;
        private System.Windows.Forms.Label labelRed;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnControl;
        private System.Windows.Forms.Label labelConnection;
        private System.Windows.Forms.Label labelOverride;
    }
}

