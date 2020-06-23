using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace T5ShortestTime {
  public partial class GenericTableViewForm : Form {
    public GenericTableViewForm() {
      InitializeComponent();
    }

    private bool preventClosing = true; //by default, this is true for logBox
    public bool PreventClosing { get { return preventClosing; } set { preventClosing = value; } }

    private const int CP_NOCLOSE_BUTTON = 0x200;
    protected override CreateParams CreateParams { //to make this unable to be closed
      get {
        if (PreventClosing) {
          CreateParams myCp = base.CreateParams;
          myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
          return myCp;
        }
        return base.CreateParams;
      }
    }

    public void DisplayResult(List<string> headers, List<List<object>> rows) {
      dataGridViewTable.Rows.Clear();
      dataGridViewTable.Columns.Clear();
      for (int i = 0; i < headers.Count; ++i) {
        DataGridViewColumn column = new DataGridViewTextBoxColumn();
        column.HeaderText = headers[i];
        dataGridViewTable.Columns.Add(column);
      }
      for (int i = 0; i < rows.Count; ++i) {
        DataGridViewRow row = new DataGridViewRow();
        for (int j = 0; j < rows[i].Count; ++j) {
          DataGridViewCell cell = new DataGridViewTextBoxCell();
          cell.Value = rows[i][j];
          row.Cells.Add(cell);
        }
        dataGridViewTable.Rows.Add(row);
      }
    }
  }
}
