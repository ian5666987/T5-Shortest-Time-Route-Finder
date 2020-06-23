namespace T5ShortestTime {
  partial class GenericTableViewForm {
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GenericTableViewForm));
      this.dataGridViewTable = new System.Windows.Forms.DataGridView();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTable)).BeginInit();
      this.SuspendLayout();
      // 
      // dataGridViewTable
      // 
      this.dataGridViewTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
      this.dataGridViewTable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
      this.dataGridViewTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewTable.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dataGridViewTable.Location = new System.Drawing.Point(0, 0);
      this.dataGridViewTable.Name = "dataGridViewTable";
      this.dataGridViewTable.ReadOnly = true;
      this.dataGridViewTable.RowTemplate.Height = 24;
      this.dataGridViewTable.Size = new System.Drawing.Size(800, 450);
      this.dataGridViewTable.TabIndex = 0;
      // 
      // GenericTableViewForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 450);
      this.Controls.Add(this.dataGridViewTable);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "GenericTableViewForm";
      this.Text = "Table View Form";
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTable)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.DataGridView dataGridViewTable;
  }
}