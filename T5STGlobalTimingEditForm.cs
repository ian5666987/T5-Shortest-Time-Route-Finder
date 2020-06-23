using System;
using System.Windows.Forms;

namespace T5ShortestTime {
	public partial class T5STGlobalTimingEditForm : Form {
		public T5STGlobalTimingEditForm(string alias, int value) {
			InitializeComponent();
			labelTimingAlias.Text = alias;
			numericUpDownActTime.Value = Math.Min(Math.Max(1, value), numericUpDownActTime.Maximum);
		}

		public int NewValue { get; set; }
		private void buttonApply_Click(object sender, EventArgs e) {
			NewValue = (int)numericUpDownActTime.Value;
			DialogResult = DialogResult.OK;
		}

		private void buttonCancel_Click(object sender, EventArgs e) {
			DialogResult = DialogResult.Cancel;
			Close();
		}
	}
}
