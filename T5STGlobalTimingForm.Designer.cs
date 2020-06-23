namespace T5ShortestTime {
	partial class T5STGlobalTimingForm {
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
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonApply = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.textBoxTimingActTime = new System.Windows.Forms.TextBox();
			this.listViewTiming = new System.Windows.Forms.ListView();
			this.columnHeaderGlobalTimingAlias = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeaderGlobalTimingActTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.textBoxTimingAlias = new System.Windows.Forms.TextBox();
			this.buttonAddTiming = new System.Windows.Forms.Button();
			this.buttonRemoveTiming = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// buttonCancel
			// 
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonCancel.Location = new System.Drawing.Point(12, 289);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(164, 30);
			this.buttonCancel.TabIndex = 61;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// buttonApply
			// 
			this.buttonApply.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonApply.Location = new System.Drawing.Point(209, 289);
			this.buttonApply.Name = "buttonApply";
			this.buttonApply.Size = new System.Drawing.Size(164, 30);
			this.buttonApply.TabIndex = 60;
			this.buttonApply.Text = "Apply";
			this.buttonApply.UseVisualStyleBackColor = true;
			this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(181, 227);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(24, 17);
			this.label4.TabIndex = 67;
			this.label4.Text = ">>";
			// 
			// textBoxTimingActTime
			// 
			this.textBoxTimingActTime.Location = new System.Drawing.Point(209, 224);
			this.textBoxTimingActTime.Name = "textBoxTimingActTime";
			this.textBoxTimingActTime.Size = new System.Drawing.Size(164, 23);
			this.textBoxTimingActTime.TabIndex = 63;
			// 
			// listViewTiming
			// 
			this.listViewTiming.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderGlobalTimingAlias,
            this.columnHeaderGlobalTimingActTime});
			this.listViewTiming.Location = new System.Drawing.Point(12, 12);
			this.listViewTiming.MultiSelect = false;
			this.listViewTiming.Name = "listViewTiming";
			this.listViewTiming.Size = new System.Drawing.Size(361, 206);
			this.listViewTiming.TabIndex = 66;
			this.listViewTiming.TabStop = false;
			this.listViewTiming.UseCompatibleStateImageBehavior = false;
			this.listViewTiming.View = System.Windows.Forms.View.Details;
			this.listViewTiming.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewTiming_MouseDoubleClick);
			// 
			// columnHeaderGlobalTimingAlias
			// 
			this.columnHeaderGlobalTimingAlias.Text = "Alias";
			this.columnHeaderGlobalTimingAlias.Width = 172;
			// 
			// columnHeaderGlobalTimingActTime
			// 
			this.columnHeaderGlobalTimingActTime.Text = "Action Time (x100 ms)";
			this.columnHeaderGlobalTimingActTime.Width = 145;
			// 
			// textBoxTimingAlias
			// 
			this.textBoxTimingAlias.Location = new System.Drawing.Point(12, 224);
			this.textBoxTimingAlias.Name = "textBoxTimingAlias";
			this.textBoxTimingAlias.Size = new System.Drawing.Size(164, 23);
			this.textBoxTimingAlias.TabIndex = 62;
			// 
			// buttonAddTiming
			// 
			this.buttonAddTiming.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonAddTiming.Location = new System.Drawing.Point(12, 253);
			this.buttonAddTiming.Name = "buttonAddTiming";
			this.buttonAddTiming.Size = new System.Drawing.Size(164, 30);
			this.buttonAddTiming.TabIndex = 64;
			this.buttonAddTiming.Text = "Add Timing";
			this.buttonAddTiming.UseVisualStyleBackColor = true;
			this.buttonAddTiming.Click += new System.EventHandler(this.buttonAddTiming_Click);
			// 
			// buttonRemoveTiming
			// 
			this.buttonRemoveTiming.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonRemoveTiming.Location = new System.Drawing.Point(209, 253);
			this.buttonRemoveTiming.Name = "buttonRemoveTiming";
			this.buttonRemoveTiming.Size = new System.Drawing.Size(164, 30);
			this.buttonRemoveTiming.TabIndex = 65;
			this.buttonRemoveTiming.Text = "Remove Timing";
			this.buttonRemoveTiming.UseVisualStyleBackColor = true;
			this.buttonRemoveTiming.Click += new System.EventHandler(this.buttonRemoveTiming_Click);
			// 
			// T5STGlobalTimingForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(384, 331);
			this.ControlBox = false;
			this.Controls.Add(this.label4);
			this.Controls.Add(this.textBoxTimingActTime);
			this.Controls.Add(this.listViewTiming);
			this.Controls.Add(this.textBoxTimingAlias);
			this.Controls.Add(this.buttonAddTiming);
			this.Controls.Add(this.buttonRemoveTiming);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonApply);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "T5STGlobalTimingForm";
			this.Text = "Global Timing";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonApply;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textBoxTimingActTime;
		private System.Windows.Forms.ListView listViewTiming;
		private System.Windows.Forms.ColumnHeader columnHeaderGlobalTimingAlias;
		private System.Windows.Forms.ColumnHeader columnHeaderGlobalTimingActTime;
		private System.Windows.Forms.TextBox textBoxTimingAlias;
		private System.Windows.Forms.Button buttonAddTiming;
		private System.Windows.Forms.Button buttonRemoveTiming;
	}
}