namespace T5ShortestTime {
	partial class T5STAppSettingsForm {
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
			this.label1 = new System.Windows.Forms.Label();
			this.numericUpDownMaxParallelSearch = new System.Windows.Forms.NumericUpDown();
			this.checkBoxShowLogBox = new System.Windows.Forms.CheckBox();
			this.checkBoxSearchAllEquipments = new System.Windows.Forms.CheckBox();
			this.checkBoxTrackResourcesUsage = new System.Windows.Forms.CheckBox();
			this.buttonApply = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.checkBoxAutoExecutionCheck = new System.Windows.Forms.CheckBox();
			this.checkBoxAutoPriorityAssignment = new System.Windows.Forms.CheckBox();
			this.checkBoxLockTable = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.numericUpDownTableTimerTick = new System.Windows.Forms.NumericUpDown();
			this.numericUpDownDisplayTimerTick = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.textBoxExecutionCheckProcedure = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.labelPriorityAssignment = new System.Windows.Forms.Label();
			this.textBoxPriorityAssignmentProcedure = new System.Windows.Forms.TextBox();
			this.checkBoxIsStrongCheck = new System.Windows.Forms.CheckBox();
			this.comboBoxMode = new System.Windows.Forms.ComboBox();
			this.label7 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxParallelSearch)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownTableTimerTick)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownDisplayTimerTick)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(203, 9);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(133, 17);
			this.label1.TabIndex = 40;
			this.label1.Text = "Max Parallel Search";
			// 
			// numericUpDownMaxParallelSearch
			// 
			this.numericUpDownMaxParallelSearch.Location = new System.Drawing.Point(343, 7);
			this.numericUpDownMaxParallelSearch.Maximum = new decimal(new int[] {
            64,
            0,
            0,
            0});
			this.numericUpDownMaxParallelSearch.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numericUpDownMaxParallelSearch.Name = "numericUpDownMaxParallelSearch";
			this.numericUpDownMaxParallelSearch.Size = new System.Drawing.Size(44, 23);
			this.numericUpDownMaxParallelSearch.TabIndex = 57;
			this.numericUpDownMaxParallelSearch.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
			// 
			// checkBoxShowLogBox
			// 
			this.checkBoxShowLogBox.AutoSize = true;
			this.checkBoxShowLogBox.Location = new System.Drawing.Point(12, 35);
			this.checkBoxShowLogBox.Name = "checkBoxShowLogBox";
			this.checkBoxShowLogBox.Size = new System.Drawing.Size(116, 21);
			this.checkBoxShowLogBox.TabIndex = 56;
			this.checkBoxShowLogBox.Text = "Show Log Box";
			this.checkBoxShowLogBox.UseVisualStyleBackColor = true;
			// 
			// checkBoxSearchAllEquipments
			// 
			this.checkBoxSearchAllEquipments.AutoSize = true;
			this.checkBoxSearchAllEquipments.Checked = true;
			this.checkBoxSearchAllEquipments.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxSearchAllEquipments.Location = new System.Drawing.Point(12, 8);
			this.checkBoxSearchAllEquipments.Name = "checkBoxSearchAllEquipments";
			this.checkBoxSearchAllEquipments.Size = new System.Drawing.Size(169, 21);
			this.checkBoxSearchAllEquipments.TabIndex = 55;
			this.checkBoxSearchAllEquipments.Text = "Search All Equipments";
			this.checkBoxSearchAllEquipments.UseVisualStyleBackColor = true;
			// 
			// checkBoxTrackResourcesUsage
			// 
			this.checkBoxTrackResourcesUsage.AutoSize = true;
			this.checkBoxTrackResourcesUsage.Checked = true;
			this.checkBoxTrackResourcesUsage.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxTrackResourcesUsage.Location = new System.Drawing.Point(12, 62);
			this.checkBoxTrackResourcesUsage.Name = "checkBoxTrackResourcesUsage";
			this.checkBoxTrackResourcesUsage.Size = new System.Drawing.Size(180, 21);
			this.checkBoxTrackResourcesUsage.TabIndex = 49;
			this.checkBoxTrackResourcesUsage.Text = "Track Resources Usage";
			this.checkBoxTrackResourcesUsage.UseVisualStyleBackColor = true;
			// 
			// buttonApply
			// 
			this.buttonApply.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonApply.Location = new System.Drawing.Point(222, 221);
			this.buttonApply.Name = "buttonApply";
			this.buttonApply.Size = new System.Drawing.Size(198, 30);
			this.buttonApply.TabIndex = 58;
			this.buttonApply.Text = "Apply";
			this.buttonApply.UseVisualStyleBackColor = true;
			this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonCancel.Location = new System.Drawing.Point(12, 221);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(198, 30);
			this.buttonCancel.TabIndex = 59;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// checkBoxAutoExecutionCheck
			// 
			this.checkBoxAutoExecutionCheck.AutoSize = true;
			this.checkBoxAutoExecutionCheck.Location = new System.Drawing.Point(364, 140);
			this.checkBoxAutoExecutionCheck.Name = "checkBoxAutoExecutionCheck";
			this.checkBoxAutoExecutionCheck.Size = new System.Drawing.Size(56, 21);
			this.checkBoxAutoExecutionCheck.TabIndex = 60;
			this.checkBoxAutoExecutionCheck.Text = "Auto";
			this.checkBoxAutoExecutionCheck.UseVisualStyleBackColor = true;
			// 
			// checkBoxAutoPriorityAssignment
			// 
			this.checkBoxAutoPriorityAssignment.AutoSize = true;
			this.checkBoxAutoPriorityAssignment.Location = new System.Drawing.Point(364, 188);
			this.checkBoxAutoPriorityAssignment.Name = "checkBoxAutoPriorityAssignment";
			this.checkBoxAutoPriorityAssignment.Size = new System.Drawing.Size(56, 21);
			this.checkBoxAutoPriorityAssignment.TabIndex = 61;
			this.checkBoxAutoPriorityAssignment.Text = "Auto";
			this.checkBoxAutoPriorityAssignment.UseVisualStyleBackColor = true;
			// 
			// checkBoxLockTable
			// 
			this.checkBoxLockTable.AutoSize = true;
			this.checkBoxLockTable.Location = new System.Drawing.Point(12, 89);
			this.checkBoxLockTable.Name = "checkBoxLockTable";
			this.checkBoxLockTable.Size = new System.Drawing.Size(97, 21);
			this.checkBoxLockTable.TabIndex = 62;
			this.checkBoxLockTable.Text = "Lock Table";
			this.checkBoxLockTable.UseVisualStyleBackColor = true;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(203, 37);
			this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(114, 17);
			this.label2.TabIndex = 63;
			this.label2.Text = "Table Timer Tick";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(203, 65);
			this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(124, 17);
			this.label3.TabIndex = 64;
			this.label3.Text = "Display Timer Tick";
			// 
			// numericUpDownTableTimerTick
			// 
			this.numericUpDownTableTimerTick.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.numericUpDownTableTimerTick.Location = new System.Drawing.Point(330, 35);
			this.numericUpDownTableTimerTick.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.numericUpDownTableTimerTick.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.numericUpDownTableTimerTick.Name = "numericUpDownTableTimerTick";
			this.numericUpDownTableTimerTick.Size = new System.Drawing.Size(57, 23);
			this.numericUpDownTableTimerTick.TabIndex = 65;
			this.numericUpDownTableTimerTick.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
			// 
			// numericUpDownDisplayTimerTick
			// 
			this.numericUpDownDisplayTimerTick.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.numericUpDownDisplayTimerTick.Location = new System.Drawing.Point(330, 63);
			this.numericUpDownDisplayTimerTick.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
			this.numericUpDownDisplayTimerTick.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.numericUpDownDisplayTimerTick.Name = "numericUpDownDisplayTimerTick";
			this.numericUpDownDisplayTimerTick.Size = new System.Drawing.Size(57, 23);
			this.numericUpDownDisplayTimerTick.TabIndex = 66;
			this.numericUpDownDisplayTimerTick.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(394, 37);
			this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(26, 17);
			this.label4.TabIndex = 67;
			this.label4.Text = "ms";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(394, 65);
			this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(26, 17);
			this.label5.TabIndex = 68;
			this.label5.Text = "ms";
			// 
			// textBoxExecutionCheckProcedure
			// 
			this.textBoxExecutionCheckProcedure.Location = new System.Drawing.Point(12, 140);
			this.textBoxExecutionCheckProcedure.Name = "textBoxExecutionCheckProcedure";
			this.textBoxExecutionCheckProcedure.Size = new System.Drawing.Size(339, 23);
			this.textBoxExecutionCheckProcedure.TabIndex = 69;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(9, 120);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(182, 17);
			this.label6.TabIndex = 70;
			this.label6.Text = "Execution Check Procedure";
			// 
			// labelPriorityAssignment
			// 
			this.labelPriorityAssignment.AutoSize = true;
			this.labelPriorityAssignment.Location = new System.Drawing.Point(9, 168);
			this.labelPriorityAssignment.Name = "labelPriorityAssignment";
			this.labelPriorityAssignment.Size = new System.Drawing.Size(199, 17);
			this.labelPriorityAssignment.TabIndex = 71;
			this.labelPriorityAssignment.Text = "Priority Assignment Procedure";
			// 
			// textBoxPriorityAssignmentProcedure
			// 
			this.textBoxPriorityAssignmentProcedure.Location = new System.Drawing.Point(12, 188);
			this.textBoxPriorityAssignmentProcedure.Name = "textBoxPriorityAssignmentProcedure";
			this.textBoxPriorityAssignmentProcedure.Size = new System.Drawing.Size(339, 23);
			this.textBoxPriorityAssignmentProcedure.TabIndex = 72;
			// 
			// checkBoxIsStrongCheck
			// 
			this.checkBoxIsStrongCheck.AutoSize = true;
			this.checkBoxIsStrongCheck.Location = new System.Drawing.Point(294, 119);
			this.checkBoxIsStrongCheck.Name = "checkBoxIsStrongCheck";
			this.checkBoxIsStrongCheck.Size = new System.Drawing.Size(126, 21);
			this.checkBoxIsStrongCheck.TabIndex = 73;
			this.checkBoxIsStrongCheck.Text = "Is Strong Check";
			this.checkBoxIsStrongCheck.UseVisualStyleBackColor = true;
			this.checkBoxIsStrongCheck.CheckedChanged += new System.EventHandler(this.checkBoxIsStrongCheck_CheckedChanged);
			// 
			// comboBoxMode
			// 
			this.comboBoxMode.FormattingEnabled = true;
			this.comboBoxMode.Items.AddRange(new object[] {
            "Manual",
            "SemiAuto",
            "Auto"});
			this.comboBoxMode.Location = new System.Drawing.Point(253, 91);
			this.comboBoxMode.Name = "comboBoxMode";
			this.comboBoxMode.Size = new System.Drawing.Size(134, 24);
			this.comboBoxMode.TabIndex = 74;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(203, 92);
			this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(43, 17);
			this.label7.TabIndex = 75;
			this.label7.Text = "Mode";
			// 
			// T5STAppSettingsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(432, 261);
			this.ControlBox = false;
			this.Controls.Add(this.label7);
			this.Controls.Add(this.comboBoxMode);
			this.Controls.Add(this.checkBoxIsStrongCheck);
			this.Controls.Add(this.textBoxPriorityAssignmentProcedure);
			this.Controls.Add(this.labelPriorityAssignment);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.textBoxExecutionCheckProcedure);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.numericUpDownDisplayTimerTick);
			this.Controls.Add(this.numericUpDownTableTimerTick);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.checkBoxLockTable);
			this.Controls.Add(this.checkBoxAutoPriorityAssignment);
			this.Controls.Add(this.checkBoxAutoExecutionCheck);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonApply);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.numericUpDownMaxParallelSearch);
			this.Controls.Add(this.checkBoxShowLogBox);
			this.Controls.Add(this.checkBoxSearchAllEquipments);
			this.Controls.Add(this.checkBoxTrackResourcesUsage);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MaximumSize = new System.Drawing.Size(448, 300);
			this.MinimumSize = new System.Drawing.Size(448, 300);
			this.Name = "T5STAppSettingsForm";
			this.Text = "Settings [Application]";
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxParallelSearch)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownTableTimerTick)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownDisplayTimerTick)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown numericUpDownMaxParallelSearch;
		private System.Windows.Forms.CheckBox checkBoxShowLogBox;
		private System.Windows.Forms.CheckBox checkBoxSearchAllEquipments;
		private System.Windows.Forms.CheckBox checkBoxTrackResourcesUsage;
		private System.Windows.Forms.Button buttonApply;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.CheckBox checkBoxAutoExecutionCheck;
		private System.Windows.Forms.CheckBox checkBoxAutoPriorityAssignment;
		private System.Windows.Forms.CheckBox checkBoxLockTable;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown numericUpDownTableTimerTick;
		private System.Windows.Forms.NumericUpDown numericUpDownDisplayTimerTick;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textBoxExecutionCheckProcedure;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label labelPriorityAssignment;
		private System.Windows.Forms.TextBox textBoxPriorityAssignmentProcedure;
		private System.Windows.Forms.CheckBox checkBoxIsStrongCheck;
		private System.Windows.Forms.ComboBox comboBoxMode;
		private System.Windows.Forms.Label label7;
	}
}