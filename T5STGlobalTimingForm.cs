using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using T5ShortestTime.MOAct;

namespace T5ShortestTime {
	public partial class T5STGlobalTimingForm : Form {
		GlobalActTimeSettings globalActTimeSettings;
		public T5STGlobalTimingForm(GlobalActTimeSettings globalActTimeSettings) {
			InitializeComponent();
			if (globalActTimeSettings == null) {
				MessageBox.Show("T5 Shortest Time Global Timming!", "Error");
				Close();
				return;
			}
			this.globalActTimeSettings = globalActTimeSettings;
      if (globalActTimeSettings.AliasActTimePairs != null)
			  foreach (var t in globalActTimeSettings.AliasActTimePairs) {
				  ListViewItem item = new ListViewItem(new string[] { t.Alias, t.ActTime.ToString() });
				  listViewTiming.Items.Add(item);
			  }
		}

		private void buttonCancel_Click(object sender, EventArgs e) {
			DialogResult = DialogResult.Cancel;
			Close();
		}

		private void buttonApply_Click(object sender, EventArgs e) {
			globalActTimeSettings.AliasActTimePairs = listViewTiming.Items.Cast<ListViewItem>()
				.Select(x => new AliasActTimePair() { Alias = x.Text, ActTime = int.Parse(x.SubItems[1].Text) })
				.ToArray();
			globalActTimeSettings.AfterDeserialization();
			DialogResult = DialogResult.OK;
			Close();
		}

		private void buttonAddTiming_Click(object sender, EventArgs e) {
			string[] aliasTexts = textBoxTimingAlias.Text.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
			string[] actTimeTexts = textBoxTimingActTime.Text.Split(';'); //empty string is not removed here!
			bool isEmptyConversion = actTimeTexts.Length == 1 && string.IsNullOrWhiteSpace(actTimeTexts[0]);

			if (aliasTexts.Length != actTimeTexts.Length && !isEmptyConversion){ //not empty conversion
				MessageBox.Show("Mismatching number between the alias and the action time elements or empty text", "Error");
				return;
			}
			if (aliasTexts.Any(x => string.IsNullOrWhiteSpace(x))
				|| actTimeTexts.Any(x => string.IsNullOrWhiteSpace(x))) {
				MessageBox.Show("Timing alias or action time cannot be empty text", "Error");
				return;
			}
			int result;
			if (actTimeTexts.Any(x => !int.TryParse(x, out result))) {
				MessageBox.Show("One or more elements in the action time columns cannot be converted to a valid action time", "Error");
				return;
			}

			List<string> keys = listViewTiming.Items.Cast<ListViewItem>().Select(x => x.Text).ToList();
			for (int i = 0; i < aliasTexts.Length; ++i)
				if (keys.Contains(aliasTexts[i])) { //old key, replace
					ListViewItem item = listViewTiming.Items.Cast<ListViewItem>().FirstOrDefault(x => x.Text == aliasTexts[i]);
					if (item == null)
						continue;
					int index = listViewTiming.Items.IndexOf(item);
					ListViewItem newItem = new ListViewItem(new string[] { aliasTexts[i], isEmptyConversion ? "" : actTimeTexts[i] });
					listViewTiming.Items[index] = newItem;
				} else { //new key, add
					ListViewItem newItem = new ListViewItem(new string[] { aliasTexts[i], isEmptyConversion ? "" : actTimeTexts[i] }); //empty conversion is to be replaced with empty too...
					listViewTiming.Items.Add(newItem);
				}
			textBoxTimingAlias.Text = "";
			textBoxTimingActTime.Text = "";
		}

		private void listViewRemoveSelected(ListView listView) {
			IEnumerable<int> indices = listView.SelectedIndices.Cast<int>();
			if (!indices.Any()) //if there is nothing selected, just return
				return;
			foreach (int i in indices.Reverse())
				listView.Items.RemoveAt(i);
		}

		private void buttonRemoveTiming_Click(object sender, EventArgs e) {
			listViewRemoveSelected(listViewTiming);
		}

		private void listViewTiming_MouseDoubleClick(object sender, MouseEventArgs e) {
			IEnumerable<int> indices = listViewTiming.SelectedIndices.Cast<int>();
			if (!indices.Any())
				return;			
			T5STGlobalTimingEditForm form = new T5STGlobalTimingEditForm(
				listViewTiming.SelectedItems[0].Text, 
				int.Parse(listViewTiming.SelectedItems[0].SubItems[1].Text));
			if (form.ShowDialog() != DialogResult.OK)
				return;
			listViewTiming.SelectedItems[0].SubItems[1].Text = form.NewValue.ToString();
			form.Close();	
		}
	}
}
