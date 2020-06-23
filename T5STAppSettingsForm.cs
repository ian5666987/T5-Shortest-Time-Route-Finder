using System;
using System.Windows.Forms;

namespace T5ShortestTime {
	public partial class T5STAppSettingsForm : Form {
		T5STAppSettings appSettings;
		public T5STAppSettingsForm(T5STAppSettings appSettings) {
			InitializeComponent();
			if (appSettings == null) {
				MessageBox.Show("T5 Shortest Time Application Settings!", "Error");
				Close();
				return;
			}
			this.appSettings = appSettings;
			checkBoxSearchAllEquipments.Checked = appSettings.SearchAllEquipments;
			numericUpDownMaxParallelSearch.Value = appSettings.MaxParallelSearch;
			checkBoxShowLogBox.Checked = appSettings.ShowLogBox;
			checkBoxTrackResourcesUsage.Checked = appSettings.TrackResourcesUsage;
			checkBoxLockTable.Checked = appSettings.LockTable;
			checkBoxAutoExecutionCheck.Checked = appSettings.AutoExecutionCheck;
			checkBoxAutoPriorityAssignment.Checked = appSettings.AutoPriorityAssignment;
			numericUpDownTableTimerTick.Value = appSettings.TableTimerTickInMs;
			numericUpDownDisplayTimerTick.Value = appSettings.DisplayTimerTickInMs;
			textBoxExecutionCheckProcedure.Text = appSettings.ExecutionCheckProcedure;
			textBoxPriorityAssignmentProcedure.Text = appSettings.PriorityAssignmentProcedure;
			checkBoxIsStrongCheck.Checked = appSettings.IsStrongCheck;
			comboBoxMode.SelectedIndex = (int)appSettings.Mode;
		}

		private void buttonApply_Click(object sender, EventArgs e) {
			appSettings.IsStrongCheck = checkBoxIsStrongCheck.Checked;
			appSettings.SearchAllEquipments = checkBoxSearchAllEquipments.Checked;
			appSettings.MaxParallelSearch = (int)numericUpDownMaxParallelSearch.Value;
			appSettings.ShowLogBox = checkBoxShowLogBox.Checked;
			appSettings.TrackResourcesUsage = checkBoxTrackResourcesUsage.Checked;
			appSettings.LockTable = checkBoxLockTable.Checked;
			appSettings.AutoExecutionCheck = checkBoxAutoExecutionCheck.Checked;
			appSettings.AutoPriorityAssignment = checkBoxAutoPriorityAssignment.Checked;
			appSettings.TableTimerTickInMs = (int)numericUpDownTableTimerTick.Value;
			appSettings.DisplayTimerTickInMs = (int)numericUpDownDisplayTimerTick.Value;
			appSettings.ExecutionCheckProcedure = textBoxExecutionCheckProcedure.Text;
			appSettings.PriorityAssignmentProcedure = textBoxPriorityAssignmentProcedure.Text;
			appSettings.Mode = (T5STMode)comboBoxMode.SelectedIndex;
			DialogResult = DialogResult.OK;
			Close();
		}

		private void buttonCancel_Click(object sender, EventArgs e) {
			DialogResult = DialogResult.Cancel;
			Close();
		}

		private void checkBoxIsStrongCheck_CheckedChanged(object sender, EventArgs e) {
			bool check = (sender as CheckBox).Checked;
			labelPriorityAssignment.Enabled = !check;
			textBoxPriorityAssignmentProcedure.Enabled = !check;
			checkBoxAutoPriorityAssignment.Enabled = !check;
			if (check)
				checkBoxAutoPriorityAssignment.Checked = false; //cannot be checked while it is a strong check
		}
	}
}
