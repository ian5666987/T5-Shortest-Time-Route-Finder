using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace T5ShortestTime {
	public partial class T5STTableAndTestingForm : Form {
		T5TableAndTest tableAndTest;
		public T5STTableAndTestingForm(T5TableAndTest tableAndTest) {
			InitializeComponent();
			if (tableAndTest == null) {
				MessageBox.Show("T5 Shortest Time Table And Testing Settings!", "Error");
				Close();
				return;
			}
			this.tableAndTest = tableAndTest;
			foreach(var t in tableAndTest.TableNames) {
				ListViewItem item = new ListViewItem(new string[] { t.Key, t.Value });
				listViewTable.Items.Add(item);
			}
		}

		private void buttonCancel_Click(object sender, EventArgs e) {
			DialogResult = DialogResult.Cancel;
			Close();
		}

		private void buttonApply_Click(object sender, EventArgs e) {
			tableAndTest.TableNames = listViewTable.Items.Cast<ListViewItem>()
				.Select(x => new { Key = x.Text, Value = x.SubItems[1].Text })
				.ToDictionary(x => x.Key, y => y.Value);
			tableAndTest.TableNameAliases = listViewTable.Items.Cast<ListViewItem>()
				.Select(x => new AliasNamePair(x.Text, x.SubItems[1].Text))
				.ToArray();
			DialogResult = DialogResult.OK;
			Close();
		}

		private void buttonAddTable_Click(object sender, EventArgs e) {
			string[] aliasTexts = textBoxTableAlias.Text.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
			string[] tableTexts = textBoxTableName.Text.Split(';'); //empty string is not removed here!
			bool isEmptyConversion = tableTexts.Length == 1 && string.IsNullOrWhiteSpace(tableTexts[0]);

			if (aliasTexts.Length != tableTexts.Length && !isEmptyConversion){ //not empty conversion
				MessageBox.Show("Mismatching number between the alias and the name elements or empty text", "Error");
				return;
			}
			if (aliasTexts.Any(x => string.IsNullOrWhiteSpace(x))
				|| tableTexts.Any(x => string.IsNullOrWhiteSpace(x))) {
				MessageBox.Show("Table alias or name cannot be empty text", "Error");
				return;
			}
			if (aliasTexts.Any(x => x.Contains(" "))
				|| tableTexts.Any(x => x.Contains(" "))) {
				MessageBox.Show("Table alias or name cannot contain white space", "Error");
				return;
			}
			if (aliasTexts.Any(x => !(char.IsLetter(x[0]) || x[0] == '_'))
				|| tableTexts.Any(x => !(char.IsLetter(x[0]) || x[0] == '_'))) {
				MessageBox.Show("Table alias or name cannot start with anything but letter or underscore", "Error");
				return;
			}

			List<string> keys = listViewTable.Items.Cast<ListViewItem>().Select(x => x.Text).ToList();
			for (int i = 0; i < aliasTexts.Length; ++i)
				if (keys.Contains(aliasTexts[i])) { //old key, replace
					ListViewItem item = listViewTable.Items.Cast<ListViewItem>().FirstOrDefault(x => x.Text == aliasTexts[i]);
					if (item == null)
						continue;
					int index = listViewTable.Items.IndexOf(item);
					ListViewItem newItem = new ListViewItem(new string[] { aliasTexts[i], isEmptyConversion ? "" : tableTexts[i] });
					listViewTable.Items[index] = newItem;
				} else { //new key, add
					ListViewItem newItem = new ListViewItem(new string[] { aliasTexts[i], isEmptyConversion ? "" : tableTexts[i] }); //empty conversion is to be replaced with empty too...
					listViewTable.Items.Add(newItem);
				}
			textBoxTableAlias.Text = "";
			textBoxTableName.Text = "";
		}

		private void listViewRemoveSelected(ListView listView) {
			IEnumerable<int> indices = listView.SelectedIndices.Cast<int>();
			if (!indices.Any()) //if there is nothing selected, just return
				return;
			foreach (int i in indices.Reverse())
				listView.Items.RemoveAt(i);
		}

		private void buttonRemoveTable_Click(object sender, EventArgs e) {
			listViewRemoveSelected(listViewTable);
		}
	}
}
