namespace T5ShortestTime {
	partial class T5STGlobalTimingEditForm {
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
			this.labelTimingAlias = new System.Windows.Forms.Label();
			this.numericUpDownActTime = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownActTime)).BeginInit();
			this.SuspendLayout();
			// 
			// labelTimingAlias
			// 
			this.labelTimingAlias.Location = new System.Drawing.Point(17, 16);
			this.labelTimingAlias.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.labelTimingAlias.Name = "labelTimingAlias";
			this.labelTimingAlias.Size = new System.Drawing.Size(145, 22);
			this.labelTimingAlias.TabIndex = 0;
			this.labelTimingAlias.Text = "label1";
			this.labelTimingAlias.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// numericUpDownActTime
			// 
			this.numericUpDownActTime.Location = new System.Drawing.Point(172, 17);
			this.numericUpDownActTime.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
			this.numericUpDownActTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numericUpDownActTime.Name = "numericUpDownActTime";
			this.numericUpDownActTime.Size = new System.Drawing.Size(78, 23);
			this.numericUpDownActTime.TabIndex = 1;
			this.numericUpDownActTime.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(262, 19);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(60, 17);
			this.label1.TabIndex = 2;
			this.label1.Text = "x100 ms";
			// 
			// buttonCancel
			// 
			this.buttonCancel.Location = new System.Drawing.Point(172, 49);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(150, 30);
			this.buttonCancel.TabIndex = 3;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(12, 49);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(150, 30);
			this.button1.TabIndex = 4;
			this.button1.Text = "Apply";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.buttonApply_Click);
			// 
			// T5STGlobalTimingEditForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(334, 91);
			this.ControlBox = false;
			this.Controls.Add(this.button1);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.numericUpDownActTime);
			this.Controls.Add(this.labelTimingAlias);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MaximumSize = new System.Drawing.Size(350, 130);
			this.MinimumSize = new System.Drawing.Size(350, 130);
			this.Name = "T5STGlobalTimingEditForm";
			this.Text = "Global Timing (Edit)";
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownActTime)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label labelTimingAlias;
		private System.Windows.Forms.NumericUpDown numericUpDownActTime;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button button1;
	}
}