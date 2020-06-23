namespace T5ShortestTime {
	partial class T5STTableAndTestingForm {
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
			this.textBoxTableName = new System.Windows.Forms.TextBox();
			this.listViewTable = new System.Windows.Forms.ListView();
			this.columnHeaderTableAlias = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeaderTableName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.textBoxTableAlias = new System.Windows.Forms.TextBox();
			this.buttonAddTable = new System.Windows.Forms.Button();
			this.buttonRemoveTable = new System.Windows.Forms.Button();
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
			// textBoxTableName
			// 
			this.textBoxTableName.Location = new System.Drawing.Point(209, 224);
			this.textBoxTableName.Name = "textBoxTableName";
			this.textBoxTableName.Size = new System.Drawing.Size(164, 23);
			this.textBoxTableName.TabIndex = 63;
			// 
			// listViewTable
			// 
			this.listViewTable.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderTableAlias,
            this.columnHeaderTableName});
			this.listViewTable.Location = new System.Drawing.Point(12, 12);
			this.listViewTable.Name = "listViewTable";
			this.listViewTable.Size = new System.Drawing.Size(361, 206);
			this.listViewTable.TabIndex = 66;
			this.listViewTable.TabStop = false;
			this.listViewTable.UseCompatibleStateImageBehavior = false;
			this.listViewTable.View = System.Windows.Forms.View.Details;
			// 
			// columnHeaderTableAlias
			// 
			this.columnHeaderTableAlias.Text = "Alias";
			this.columnHeaderTableAlias.Width = 135;
			// 
			// columnHeaderTableName
			// 
			this.columnHeaderTableName.Text = "Name";
			this.columnHeaderTableName.Width = 198;
			// 
			// textBoxTableAlias
			// 
			this.textBoxTableAlias.Location = new System.Drawing.Point(12, 224);
			this.textBoxTableAlias.Name = "textBoxTableAlias";
			this.textBoxTableAlias.Size = new System.Drawing.Size(164, 23);
			this.textBoxTableAlias.TabIndex = 62;
			// 
			// buttonAddTable
			// 
			this.buttonAddTable.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonAddTable.Location = new System.Drawing.Point(12, 253);
			this.buttonAddTable.Name = "buttonAddTable";
			this.buttonAddTable.Size = new System.Drawing.Size(164, 30);
			this.buttonAddTable.TabIndex = 64;
			this.buttonAddTable.Text = "Add Table";
			this.buttonAddTable.UseVisualStyleBackColor = true;
			this.buttonAddTable.Click += new System.EventHandler(this.buttonAddTable_Click);
			// 
			// buttonRemoveTable
			// 
			this.buttonRemoveTable.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonRemoveTable.Location = new System.Drawing.Point(209, 253);
			this.buttonRemoveTable.Name = "buttonRemoveTable";
			this.buttonRemoveTable.Size = new System.Drawing.Size(164, 30);
			this.buttonRemoveTable.TabIndex = 65;
			this.buttonRemoveTable.Text = "Remove Table";
			this.buttonRemoveTable.UseVisualStyleBackColor = true;
			this.buttonRemoveTable.Click += new System.EventHandler(this.buttonRemoveTable_Click);
			// 
			// T5STTableAndTestingForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(384, 331);
			this.ControlBox = false;
			this.Controls.Add(this.label4);
			this.Controls.Add(this.textBoxTableName);
			this.Controls.Add(this.listViewTable);
			this.Controls.Add(this.textBoxTableAlias);
			this.Controls.Add(this.buttonAddTable);
			this.Controls.Add(this.buttonRemoveTable);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonApply);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "T5STTableAndTestingForm";
			this.Text = "Table And Testing";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonApply;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textBoxTableName;
		private System.Windows.Forms.ListView listViewTable;
		private System.Windows.Forms.ColumnHeader columnHeaderTableAlias;
		private System.Windows.Forms.ColumnHeader columnHeaderTableName;
		private System.Windows.Forms.TextBox textBoxTableAlias;
		private System.Windows.Forms.Button buttonAddTable;
		private System.Windows.Forms.Button buttonRemoveTable;
	}
}