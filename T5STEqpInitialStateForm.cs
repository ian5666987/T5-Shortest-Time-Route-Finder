using System;
using System.Collections.Generic;
using System.Windows.Forms;
using T5ShortestTime.MOAct;
using T5ShortestTime.SATS;

namespace T5ShortestTime {
	public partial class T5STEqpInitialStateForm : Form {
		ActInitialState initialState;
		public T5STEqpInitialStateForm(ActInitialState initialState) {
			InitializeComponent();

			if (initialState == null) {
				MessageBox.Show("Invalid initial state!");
				Close();
			}

			this.initialState = initialState;
			initGUIInitialParameter(this.initialState);
		}

		private void checkBoxInitialMO1_CheckedChanged(object sender, EventArgs e) {
			CheckBox cb = sender as CheckBox;
			mo1Components(cb.Checked);
			checkBoxInitialMO2.Enabled = cb.Checked;
			mo2Components(checkBoxInitialMO2.Enabled && checkBoxInitialMO2.Checked);
		}

		private void checkBoxInitialMO2_CheckedChanged(object sender, EventArgs e) {
			CheckBox cb = sender as CheckBox;
			mo2Components(cb.Enabled && cb.Checked);
		}

		private void setInitialStateFromGUI() { //initial state of the equipment
			initialState.InitialX = (int)numericUpDownInitialX.Value;
			initialState.InitialZ = (char)(comboBoxInitialZ.SelectedIndex + 'A');
			initialState.LeftActs = new List<ActLeftBase>();
			if (checkBoxInitialMO1.Checked) { //The first item          
				ActLeftBase basic = new ActLeftBase();
				basic.PUPoint = ((int)(numericUpDownInitialMO1X1.Value)).ToString("d2") + comboBoxInitialMO1Z1.SelectedItem.ToString() +
					(comboBoxInitialMO1S1.SelectedIndex == (int)LaneSide.AirSide ? "12" : "15");
				basic.DOPoint = ((int)(numericUpDownInitialMO1X2.Value)).ToString("d2") + comboBoxInitialMO1Z2.SelectedItem.ToString() +
					(comboBoxInitialMO1S2.SelectedIndex == (int)LaneSide.AirSide ? "12" : "15");
				if (comboBoxInitialMO1C.SelectedItem == null)
					return;
				basic.ULDCat = comboBoxInitialMO1C.SelectedItem.ToString();
				basic.PriorLevel = (int)numericUpDownInitialMO1P.Value;
				basic.No = (int)numericUpDownInitialMO1N.Value;
				initialState.LeftActs.Add(basic);
				if (checkBoxInitialMO2.Enabled && checkBoxInitialMO2.Checked && comboBoxInitialMO1C.SelectedIndex == 0) { //only if it is checked, enabled, and having combo initial C == L this can be chosen           
					ActLeftBase basic2 = new ActLeftBase();
					basic2.PUPoint = ((int)(numericUpDownInitialMO2X1.Value)).ToString("d2") + comboBoxInitialMO2Z1.SelectedItem.ToString() +
						(comboBoxInitialMO2S1.SelectedIndex == (int)LaneSide.AirSide ? "12" : "15");
					basic2.DOPoint = ((int)(numericUpDownInitialMO2X2.Value)).ToString("d2") + comboBoxInitialMO2Z2.SelectedItem.ToString() +
						(comboBoxInitialMO2S2.SelectedIndex == (int)LaneSide.AirSide ? "12" : "15");
					if (comboBoxInitialMO2C.SelectedItem == null)
						return;
					basic2.ULDCat = comboBoxInitialMO2C.SelectedItem.ToString();
					basic2.PriorLevel = (int)numericUpDownInitialMO2P.Value;
					basic2.No = (int)numericUpDownInitialMO2N.Value;
					initialState.LeftActs.Add(basic2);
				}
			}

		}

		private void initGUIInitialParameter(ActInitialState initialState) {
			numericUpDownInitialX.Value = initialState.InitialX;
			comboBoxInitialZ.SelectedIndex = (int)(initialState.InitialZ - 'A');
			checkBoxInitialMO1.Checked = initialState.LeftActs.Count > 0;
			resetMo1Components();
			resetMo2Components();
			if (initialState.LeftActs.Count > 0) {
				checkBoxInitialMO2.Enabled = true;
				setMo1Components(initialState);
				checkBoxInitialMO2.Checked = initialState.LeftActs.Count > 1 &&
					initialState.LeftActs[0].ULDCat == (ULDCategory.L).ToString() &&
					initialState.LeftActs[1].ULDCat == (ULDCategory.L).ToString();
				if (checkBoxInitialMO2.Checked) {
					checkBoxInitialMO2.Enabled = true;
					setMo2Components(initialState);
					mo2Components(checkBoxInitialMO2.Checked);
				}
			} else {
				checkBoxInitialMO2.Enabled = false;
				checkBoxInitialMO2.Checked = false;
			}
			mo1Components(checkBoxInitialMO1.Checked);
		}

		private void mo1Components(bool isEnabled) {
			comboBoxInitialMO1C.Enabled = isEnabled;
			comboBoxInitialMO1S1.Enabled = isEnabled;
			comboBoxInitialMO1S2.Enabled = isEnabled;
			comboBoxInitialMO1Z1.Enabled = isEnabled;
			comboBoxInitialMO1Z2.Enabled = isEnabled;
			numericUpDownInitialMO1X1.Enabled = isEnabled;
			numericUpDownInitialMO1X2.Enabled = isEnabled;
			numericUpDownInitialMO1P.Enabled = isEnabled;
			numericUpDownInitialMO1N.Enabled = isEnabled;
		}

		private void mo2Components(bool isEnabled) {
			comboBoxInitialMO2C.Enabled = isEnabled;
			comboBoxInitialMO2S1.Enabled = isEnabled;
			comboBoxInitialMO2S2.Enabled = isEnabled;
			comboBoxInitialMO2Z1.Enabled = isEnabled;
			comboBoxInitialMO2Z2.Enabled = isEnabled;
			numericUpDownInitialMO2X1.Enabled = isEnabled;
			numericUpDownInitialMO2X2.Enabled = isEnabled;
			numericUpDownInitialMO2P.Enabled = isEnabled;
			numericUpDownInitialMO2N.Enabled = isEnabled;
		}

		private void resetMo1Components() {
			numericUpDownInitialMO1X1.Value = 1;
			comboBoxInitialMO1Z1.SelectedIndex = 0;
			comboBoxInitialMO1S1.SelectedIndex = 0;
			numericUpDownInitialMO1X2.Value = 1;
			comboBoxInitialMO1Z2.SelectedIndex = 0;
			comboBoxInitialMO1S2.SelectedIndex = 0;
			numericUpDownInitialMO1P.Value = 0;
			numericUpDownInitialMO1N.Value = 0;
		}

		private void resetMo2Components() {
			numericUpDownInitialMO2X1.Value = 1;
			comboBoxInitialMO2Z1.SelectedIndex = 0;
			comboBoxInitialMO2S1.SelectedIndex = 0;
			numericUpDownInitialMO2X2.Value = 1;
			comboBoxInitialMO2Z2.SelectedIndex = 0;
			comboBoxInitialMO2S2.SelectedIndex = 0;
			numericUpDownInitialMO2P.Value = 0;
			numericUpDownInitialMO2N.Value = 0;
		}

		private void setMo1Components(ActInitialState initialState) {
			numericUpDownInitialMO1X1.Value = Convert.ToInt32(initialState.LeftActs[0].PUPoint.Substring(0, 2));
			comboBoxInitialMO1Z1.SelectedIndex = (int)(initialState.LeftActs[0].PUPoint.ToUpper()[2] - 'A');
			comboBoxInitialMO1S1.SelectedIndex = initialState.LeftActs[0].PUPoint.Substring(3, 2) == "12" ? 0 : 1;
			numericUpDownInitialMO1X2.Value = Convert.ToInt32(initialState.LeftActs[0].DOPoint.Substring(0, 2));
			comboBoxInitialMO1Z2.SelectedIndex = (int)(initialState.LeftActs[0].DOPoint.ToUpper()[2] - 'A');
			comboBoxInitialMO1S2.SelectedIndex = initialState.LeftActs[0].DOPoint.Substring(3, 2) == "12" ? 0 : 1;
			numericUpDownInitialMO1P.Value = (decimal)initialState.LeftActs[0].PriorLevel;
			numericUpDownInitialMO1N.Value = initialState.LeftActs[0].No;
			comboBoxInitialMO1C.SelectedIndex = (int)((ULDCategory)Enum.Parse(typeof(ULDCategory), initialState.LeftActs[0].ULDCat));
		}

		private void setMo2Components(ActInitialState initialState) {
			numericUpDownInitialMO2X1.Value = Convert.ToInt32(initialState.LeftActs[1].PUPoint.Substring(0, 2));
			comboBoxInitialMO2Z1.SelectedIndex = (int)(initialState.LeftActs[1].PUPoint.ToUpper()[2] - 'A');
			comboBoxInitialMO2S1.SelectedIndex = initialState.LeftActs[1].PUPoint.Substring(3, 2) == "12" ? 0 : 1;
			numericUpDownInitialMO2X2.Value = Convert.ToInt32(initialState.LeftActs[1].DOPoint.Substring(0, 2));
			comboBoxInitialMO2Z2.SelectedIndex = (int)(initialState.LeftActs[1].DOPoint.ToUpper()[2] - 'A');
			comboBoxInitialMO2S2.SelectedIndex = initialState.LeftActs[1].DOPoint.Substring(3, 2) == "12" ? 0 : 1;
			numericUpDownInitialMO2P.Value = (decimal)initialState.LeftActs[1].PriorLevel;
			numericUpDownInitialMO2N.Value = initialState.LeftActs[1].No;
			comboBoxInitialMO2C.SelectedIndex = (int)((ULDCategory)Enum.Parse(typeof(ULDCategory), initialState.LeftActs[1].ULDCat));
		}

		private void buttonApply_Click(object sender, EventArgs e) {
			setInitialStateFromGUI();
			DialogResult = DialogResult.OK;
			Close();
		}

		private void buttonCancel_Click(object sender, EventArgs e) {
			DialogResult = DialogResult.Cancel;
			Close();
		}
	}
}

