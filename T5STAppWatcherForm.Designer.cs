namespace T5ShortestTime {
	partial class T5STAppWatcherForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.checkBoxFindLocalIP = new System.Windows.Forms.CheckBox();
			this.textBoxPortNo = new System.Windows.Forms.TextBox();
			this.textBoxIpAddress = new System.Windows.Forms.TextBox();
			this.labelIP = new System.Windows.Forms.Label();
			this.labelPortNo = new System.Windows.Forms.Label();
			this.checkBoxEnableHeartBeat = new System.Windows.Forms.CheckBox();
			this.numericUpDownMinimumHeartBeatRate = new System.Windows.Forms.NumericUpDown();
			this.numericUpDownMaxPortNo = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.checkBoxAutoOpen = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.numericUpDownMaxNoOfPendingClients = new System.Windows.Forms.NumericUpDown();
			this.numericUpDownHeartBeatRate = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonApply = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinimumHeartBeatRate)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxPortNo)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxNoOfPendingClients)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownHeartBeatRate)).BeginInit();
			this.SuspendLayout();
			// 
			// checkBoxFindLocalIP
			// 
			this.checkBoxFindLocalIP.AutoSize = true;
			this.checkBoxFindLocalIP.Location = new System.Drawing.Point(12, 97);
			this.checkBoxFindLocalIP.Name = "checkBoxFindLocalIP";
			this.checkBoxFindLocalIP.Size = new System.Drawing.Size(104, 20);
			this.checkBoxFindLocalIP.TabIndex = 0;
			this.checkBoxFindLocalIP.Text = "Find Local IP";
			this.checkBoxFindLocalIP.UseVisualStyleBackColor = true;
			// 
			// textBoxPortNo
			// 
			this.textBoxPortNo.Location = new System.Drawing.Point(70, 39);
			this.textBoxPortNo.Margin = new System.Windows.Forms.Padding(4);
			this.textBoxPortNo.Name = "textBoxPortNo";
			this.textBoxPortNo.Size = new System.Drawing.Size(128, 22);
			this.textBoxPortNo.TabIndex = 21;
			this.textBoxPortNo.Text = "5123";
			this.textBoxPortNo.TextChanged += new System.EventHandler(this.textBoxPortNo_TextChanged);
			// 
			// textBoxIpAddress
			// 
			this.textBoxIpAddress.Location = new System.Drawing.Point(70, 11);
			this.textBoxIpAddress.Margin = new System.Windows.Forms.Padding(4);
			this.textBoxIpAddress.Name = "textBoxIpAddress";
			this.textBoxIpAddress.Size = new System.Drawing.Size(128, 22);
			this.textBoxIpAddress.TabIndex = 23;
			this.textBoxIpAddress.Text = "127.0.0.1";
			this.textBoxIpAddress.TextChanged += new System.EventHandler(this.textBoxIpAddress_TextChanged);
			// 
			// labelIP
			// 
			this.labelIP.AutoSize = true;
			this.labelIP.Location = new System.Drawing.Point(9, 14);
			this.labelIP.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.labelIP.Name = "labelIP";
			this.labelIP.Size = new System.Drawing.Size(20, 16);
			this.labelIP.TabIndex = 22;
			this.labelIP.Text = "IP";
			// 
			// labelPortNo
			// 
			this.labelPortNo.AutoSize = true;
			this.labelPortNo.Location = new System.Drawing.Point(9, 42);
			this.labelPortNo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.labelPortNo.Name = "labelPortNo";
			this.labelPortNo.Size = new System.Drawing.Size(53, 16);
			this.labelPortNo.TabIndex = 25;
			this.labelPortNo.Text = "Port No";
			// 
			// checkBoxEnableHeartBeat
			// 
			this.checkBoxEnableHeartBeat.AutoSize = true;
			this.checkBoxEnableHeartBeat.Location = new System.Drawing.Point(12, 69);
			this.checkBoxEnableHeartBeat.Name = "checkBoxEnableHeartBeat";
			this.checkBoxEnableHeartBeat.Size = new System.Drawing.Size(137, 20);
			this.checkBoxEnableHeartBeat.TabIndex = 26;
			this.checkBoxEnableHeartBeat.Text = "Enable Heart Beat";
			this.checkBoxEnableHeartBeat.UseVisualStyleBackColor = true;
			// 
			// numericUpDownMinimumHeartBeatRate
			// 
			this.numericUpDownMinimumHeartBeatRate.Location = new System.Drawing.Point(385, 40);
			this.numericUpDownMinimumHeartBeatRate.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
			this.numericUpDownMinimumHeartBeatRate.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.numericUpDownMinimumHeartBeatRate.Name = "numericUpDownMinimumHeartBeatRate";
			this.numericUpDownMinimumHeartBeatRate.Size = new System.Drawing.Size(77, 22);
			this.numericUpDownMinimumHeartBeatRate.TabIndex = 27;
			this.numericUpDownMinimumHeartBeatRate.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			// 
			// numericUpDownMaxPortNo
			// 
			this.numericUpDownMaxPortNo.Location = new System.Drawing.Point(385, 97);
			this.numericUpDownMaxPortNo.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
			this.numericUpDownMaxPortNo.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numericUpDownMaxPortNo.Name = "numericUpDownMaxPortNo";
			this.numericUpDownMaxPortNo.Size = new System.Drawing.Size(77, 22);
			this.numericUpDownMaxPortNo.TabIndex = 28;
			this.numericUpDownMaxPortNo.Value = new decimal(new int[] {
            99999,
            0,
            0,
            0});
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(218, 99);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(81, 16);
			this.label1.TabIndex = 29;
			this.label1.Text = "Max Port No";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(218, 42);
			this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(160, 16);
			this.label2.TabIndex = 30;
			this.label2.Text = "Minimum Heart Beat Rate";
			// 
			// checkBoxAutoOpen
			// 
			this.checkBoxAutoOpen.AutoSize = true;
			this.checkBoxAutoOpen.Location = new System.Drawing.Point(122, 97);
			this.checkBoxAutoOpen.Name = "checkBoxAutoOpen";
			this.checkBoxAutoOpen.Size = new System.Drawing.Size(90, 20);
			this.checkBoxAutoOpen.TabIndex = 31;
			this.checkBoxAutoOpen.Text = "Auto Open";
			this.checkBoxAutoOpen.UseVisualStyleBackColor = true;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(218, 70);
			this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(137, 16);
			this.label3.TabIndex = 32;
			this.label3.Text = "Max Pending Client(s)";
			// 
			// numericUpDownMaxNoOfPendingClients
			// 
			this.numericUpDownMaxNoOfPendingClients.Location = new System.Drawing.Point(385, 68);
			this.numericUpDownMaxNoOfPendingClients.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numericUpDownMaxNoOfPendingClients.Name = "numericUpDownMaxNoOfPendingClients";
			this.numericUpDownMaxNoOfPendingClients.Size = new System.Drawing.Size(77, 22);
			this.numericUpDownMaxNoOfPendingClients.TabIndex = 33;
			this.numericUpDownMaxNoOfPendingClients.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
			// 
			// numericUpDownHeartBeatRate
			// 
			this.numericUpDownHeartBeatRate.Location = new System.Drawing.Point(385, 12);
			this.numericUpDownHeartBeatRate.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
			this.numericUpDownHeartBeatRate.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.numericUpDownHeartBeatRate.Name = "numericUpDownHeartBeatRate";
			this.numericUpDownHeartBeatRate.Size = new System.Drawing.Size(77, 22);
			this.numericUpDownHeartBeatRate.TabIndex = 34;
			this.numericUpDownHeartBeatRate.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(218, 14);
			this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(104, 16);
			this.label4.TabIndex = 35;
			this.label4.Text = "Heart Beat Rate";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(469, 14);
			this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(26, 16);
			this.label5.TabIndex = 36;
			this.label5.Text = "ms";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(469, 42);
			this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(26, 16);
			this.label6.TabIndex = 37;
			this.label6.Text = "ms";
			// 
			// buttonCancel
			// 
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonCancel.Location = new System.Drawing.Point(12, 128);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(237, 30);
			this.buttonCancel.TabIndex = 61;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// buttonApply
			// 
			this.buttonApply.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonApply.Location = new System.Drawing.Point(258, 128);
			this.buttonApply.Name = "buttonApply";
			this.buttonApply.Size = new System.Drawing.Size(237, 30);
			this.buttonApply.TabIndex = 60;
			this.buttonApply.Text = "Apply";
			this.buttonApply.UseVisualStyleBackColor = true;
			this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
			// 
			// T5STAppWatcherForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(506, 171);
			this.ControlBox = false;
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonApply);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.numericUpDownHeartBeatRate);
			this.Controls.Add(this.numericUpDownMaxNoOfPendingClients);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.checkBoxAutoOpen);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.numericUpDownMaxPortNo);
			this.Controls.Add(this.numericUpDownMinimumHeartBeatRate);
			this.Controls.Add(this.checkBoxEnableHeartBeat);
			this.Controls.Add(this.labelPortNo);
			this.Controls.Add(this.textBoxPortNo);
			this.Controls.Add(this.textBoxIpAddress);
			this.Controls.Add(this.labelIP);
			this.Controls.Add(this.checkBoxFindLocalIP);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MaximumSize = new System.Drawing.Size(522, 210);
			this.MinimumSize = new System.Drawing.Size(522, 210);
			this.Name = "T5STAppWatcherForm";
			this.Text = "Settings [Watcher]";
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinimumHeartBeatRate)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxPortNo)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxNoOfPendingClients)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownHeartBeatRate)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox checkBoxFindLocalIP;
		private System.Windows.Forms.TextBox textBoxPortNo;
		private System.Windows.Forms.TextBox textBoxIpAddress;
		private System.Windows.Forms.Label labelIP;
		private System.Windows.Forms.Label labelPortNo;
		private System.Windows.Forms.CheckBox checkBoxEnableHeartBeat;
		private System.Windows.Forms.NumericUpDown numericUpDownMinimumHeartBeatRate;
		private System.Windows.Forms.NumericUpDown numericUpDownMaxPortNo;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox checkBoxAutoOpen;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown numericUpDownMaxNoOfPendingClients;
		private System.Windows.Forms.NumericUpDown numericUpDownHeartBeatRate;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonApply;
	}
}