using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using T5ShortestTime.MOAct;
using T5ShortestTime.SATS;

namespace T5ShortestTime {
	public partial class T5STEqpSettingsForm : Form {
		Equipment equipment;
		public T5STEqpSettingsForm(Equipment equipment) {
			InitializeComponent();

			if (equipment == null) {
				MessageBox.Show("Invalid equipment!");
				Close();
			}

			this.equipment = equipment;
			Text += " [Equipment: " + equipment.ID + "]";
			foreach (string method in Enum.GetNames(typeof(SearchMethod)))
				comboBoxMethod.Items.Add(method.Replace("_", " "));
			comboBoxRetainBestRoute.SelectedIndex = 0;
			comboBoxRetainCandidates.SelectedIndex = 0;

			//Record Log
			checkBoxShow.Checked = equipment.RecordLogSettings.ShowLog;
			checkBoxMemory.Checked = equipment.RecordLogSettings.Memory;
			checkBoxTrack.Checked = equipment.RecordLogSettings.Track;
      checkBoxUpdateOnFinished.Checked = equipment.RecordLogSettings.UpdateOnFinished;
			numericUpDownRecordLimit.Value = equipment.RecordLogSettings.MaxRecord;

			//MO action time
			numericUpDownXTimePerLaneIn100ms.Value = equipment.Handler.TimeSettings.XTimePerLaneIn100ms;
			numericUpDownXExtraTimeIn100ms.Value = equipment.Handler.TimeSettings.XExtraTimeIn100ms;
			numericUpDownZTimePerLevelIn100ms.Value = equipment.Handler.TimeSettings.ZTimePerLevelIn100ms;
			numericUpDownZExtraTimeIn100ms.Value = equipment.Handler.TimeSettings.ZExtraTimeIn100ms;

			//MO Route Finder [Execution]
			IEnumerable<string> cbItems = comboBoxMethod.Items.Cast<string>();
			comboBoxMethod.SelectedIndex = cbItems.ToList().IndexOf(
				cbItems.Single(x => x == equipment.RouteFinderSettings.SMethod.ToString().Replace("_", " "))
			);
			numericUpDownStartDepth.Value = equipment.RouteFinderSettings.StartDepth;
			numericUpDownFinalDepth.Value = equipment.RouteFinderSettings.FinalDepth;
			numericUpDownOldIterationBranch.Value = equipment.RouteFinderSettings.OldIterationBranch;
			numericUpDownNewIterationBranch.Value = equipment.RouteFinderSettings.NewIterationBranch;
			checkBoxJoinMO.Checked = equipment.RouteFinderSettings.JoinDoPuInWs;

			//MO Route Finder [Logic]
			checkBoxMultiThread.Checked = equipment.RouteFinderSettings.IsMultithreading;
			numericUpDownNoOfThread.Value = Math.Max(equipment.RouteFinderSettings.NoOfThread, numericUpDownNoOfThread.Minimum); //when searching, it may go to 1...
			numericUpDownMultiThreadMinDepth.Value = Math.Max(equipment.RouteFinderSettings.MultithreadMinimumDepth, numericUpDownMultiThreadMinDepth.Minimum); //when searching, it may go to 1...
			numericUpDownByFactorOf.Value = (decimal)equipment.RouteFinderSettings.ByFactorOf;
			checkBoxAutoByFactor.Checked = equipment.RouteFinderSettings.AutoByFactorOf;
			numericUpDownMDDToByFactor.Value = equipment.RouteFinderSettings.MDDToByFactor;
			numericUpDownMDDExtraToApplyThreadSleep.Value = equipment.RouteFinderSettings.MDDExtraToApplyThreadSleep;
			checkBoxRetainBestRoute.Checked = equipment.RouteFinderSettings.IncludeFinalSolution;
			comboBoxRetainBestRoute.SelectedIndex = equipment.RouteFinderSettings.FinalSolutionIncludedInNewBranch ? 0 : 1;
			checkBoxRetainCandidates.Checked = equipment.RouteFinderSettings.IncludeFinalCandidates;
			comboBoxRetainCandidates.SelectedIndex = equipment.RouteFinderSettings.FinalCandidatesIncludedOnlyOnce ? 0 : 1;
			checkBoxRetainCandidatesOnlyBests.Checked = equipment.RouteFinderSettings.OnlyBestFinalCandidates;
			numericUpDownRetainCandidatesOnlyBests.Value = equipment.RouteFinderSettings.OnlyBestFinalCandidatesNo;
			numericUpDownMaxNodesEvalSpeed.Value = (decimal)equipment.RouteFinderSettings.MaxNodesEvalSpeed;
			numericUpDownAbortionFactor.Value = (decimal)equipment.RouteFinderSettings.AbortionFactor;
			checkBoxSingleL.Checked = equipment.RouteFinderSettings.IsSingleL;
			checkBoxFirstDOHighPriority.Checked = equipment.RouteFinderSettings.FirstDropHasHighPriority;
			checkBoxForMultipleULDs.Checked = equipment.RouteFinderSettings.FirstDropHasHighPriorityForMultipleUlds;
		}

		private void buttonApply_Click(object sender, EventArgs e) {
			//Record Log
			equipment.RecordLogSettings.ShowLog = checkBoxShow.Checked;
			equipment.RecordLogSettings.Memory = checkBoxMemory.Checked;
			equipment.RecordLogSettings.Track = checkBoxTrack.Checked;
      equipment.RecordLogSettings.UpdateOnFinished = checkBoxUpdateOnFinished.Checked;
			equipment.RecordLogSettings.MaxRecord = (int)numericUpDownRecordLimit.Value;

			//MO action time
			equipment.Handler.TimeSettings.XTimePerLaneIn100ms = (int)numericUpDownXTimePerLaneIn100ms.Value;
			equipment.Handler.TimeSettings.XExtraTimeIn100ms = (int)numericUpDownXExtraTimeIn100ms.Value;
			equipment.Handler.TimeSettings.ZTimePerLevelIn100ms = (int)numericUpDownZTimePerLevelIn100ms.Value;
			equipment.Handler.TimeSettings.ZExtraTimeIn100ms = (int)numericUpDownZExtraTimeIn100ms.Value;

			//MO Route Finder [Execution]
			List<string> names = Enum.GetNames(typeof(SearchMethod)).ToList();
			string name = comboBoxMethod.SelectedItem.ToString().Replace(" ", "_");
			equipment.RouteFinderSettings.SMethod = (SearchMethod)names.IndexOf(name);
			equipment.RouteFinderSettings.StartDepth = (int)numericUpDownStartDepth.Value;
			equipment.RouteFinderSettings.FinalDepth = (int)numericUpDownFinalDepth.Value;
			equipment.RouteFinderSettings.OldIterationBranch = (int)numericUpDownOldIterationBranch.Value;
			equipment.RouteFinderSettings.NewIterationBranch = (int)numericUpDownNewIterationBranch.Value;
			equipment.RouteFinderSettings.JoinDoPuInWs = checkBoxJoinMO.Checked;

			//MO Route Finder [Logic]
			equipment.RouteFinderSettings.IsMultithreading = checkBoxMultiThread.Checked;
			equipment.RouteFinderSettings.NoOfThread = (int)numericUpDownNoOfThread.Value;
			equipment.RouteFinderSettings.MultithreadMinimumDepth = (int)numericUpDownMultiThreadMinDepth.Value;
			equipment.RouteFinderSettings.ByFactorOf = (double)numericUpDownByFactorOf.Value;
			equipment.RouteFinderSettings.AutoByFactorOf = checkBoxAutoByFactor.Checked;
			equipment.RouteFinderSettings.MDDToByFactor = (int)numericUpDownMDDToByFactor.Value;
			equipment.RouteFinderSettings.MDDExtraToApplyThreadSleep = (int)numericUpDownMDDExtraToApplyThreadSleep.Value;
			equipment.RouteFinderSettings.IncludeFinalSolution = checkBoxRetainBestRoute.Checked;
			equipment.RouteFinderSettings.FinalSolutionIncludedInNewBranch = comboBoxRetainBestRoute.SelectedIndex == 0;
			equipment.RouteFinderSettings.IncludeFinalCandidates = checkBoxRetainCandidates.Checked;
			equipment.RouteFinderSettings.FinalCandidatesIncludedOnlyOnce = comboBoxRetainCandidates.SelectedIndex == 0;
			equipment.RouteFinderSettings.OnlyBestFinalCandidates = checkBoxRetainCandidatesOnlyBests.Checked;
			equipment.RouteFinderSettings.OnlyBestFinalCandidatesNo = (int)numericUpDownRetainCandidatesOnlyBests.Value;
			equipment.RouteFinderSettings.MaxNodesEvalSpeed = (double)numericUpDownMaxNodesEvalSpeed.Value;
			equipment.RouteFinderSettings.AbortionFactor = (double)numericUpDownAbortionFactor.Value;
			equipment.RouteFinderSettings.IsSingleL = checkBoxSingleL.Checked;
			equipment.RouteFinderSettings.FirstDropHasHighPriority = checkBoxFirstDOHighPriority.Checked;
			equipment.RouteFinderSettings.FirstDropHasHighPriorityForMultipleUlds = checkBoxForMultipleULDs.Checked;

			DialogResult = DialogResult.OK;
			Close();
		}

		private void buttonCancel_Click(object sender, EventArgs e) {
			DialogResult = DialogResult.Cancel;
			Close();
		}

		private void buttonInitialState_Click(object sender, EventArgs e) {
			T5STEqpInitialStateForm form = new T5STEqpInitialStateForm(equipment.InitialState);
			if (form != null && !form.IsDisposed)
				form.ShowDialog(); //nothing needs to be processed here...
		}

		private void checkBoxMultiThread_CheckedChanged(object sender, EventArgs e) {
      CheckBox cb = sender as CheckBox;
      numericUpDownNoOfThread.Enabled = cb.Checked;
      numericUpDownMultiThreadMinDepth.Enabled = cb.Checked;
		}

		private void checkBoxAutoByFactor_CheckedChanged(object sender, EventArgs e) {
			CheckBox cb = sender as CheckBox;
			numericUpDownByFactorOf.Enabled = !cb.Checked;
		}

		private void checkBoxRetainBestRoute_CheckedChanged(object sender, EventArgs e) {
			CheckBox cb = sender as CheckBox;
			comboBoxRetainBestRoute.Enabled = cb.Checked;
		}

		private void checkBoxRetainCandidates_CheckedChanged(object sender, EventArgs e) {
			CheckBox cb = sender as CheckBox;
			comboBoxRetainCandidates.Enabled = cb.Checked;
			checkBoxRetainCandidatesOnlyBests.Enabled = cb.Checked;
			numericUpDownRetainCandidatesOnlyBests.Enabled = cb.Checked && checkBoxRetainCandidatesOnlyBests.Checked;
		}

		private void checkBoxRetainCandidatesOnlyBests_CheckedChanged(object sender, EventArgs e) {
			CheckBox cb = sender as CheckBox;
			numericUpDownRetainCandidatesOnlyBests.Enabled = cb.Checked && checkBoxRetainCandidates.Checked;
		}

	}
}
