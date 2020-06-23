using System;
using System.Drawing;
using System.Windows.Forms;

using Extension.Checker;
using Extension.Socket;

namespace T5ShortestTime {
	public partial class T5STAppWatcherForm : Form {
		TCPIPServerSettings settings;
		public T5STAppWatcherForm(TCPIPServerSettings settings) {
			InitializeComponent();
			if (settings == null) {
				MessageBox.Show("T5 Shortest Time Watcher Settings!", "Error");
				Close();
				return;
			}
			this.settings = settings;
			textBoxIpAddress.Text = settings.IPV4Address;
			textBoxPortNo.Text = settings.PortNo.ToString();
			numericUpDownHeartBeatRate.Value = settings.HeartBeatRate;
			numericUpDownMinimumHeartBeatRate.Value = settings.MinimumHeartBeatRate;
			numericUpDownMaxNoOfPendingClients.Value = settings.MaxNoOfPendingClient;
			numericUpDownMaxPortNo.Value = settings.MaxPortNo;
			checkBoxAutoOpen.Checked = settings.AutoOpen;
			checkBoxEnableHeartBeat.Checked = settings.EnableHeartBeat;
			checkBoxFindLocalIP.Checked = settings.FindLocalIP;
		}

		private void textBoxIpAddress_TextChanged(object sender, EventArgs e) {
			TextBox thisTextBox = sender as TextBox;
			thisTextBox.ForeColor = Extension.Checker.Text.CheckTcpIpFormatValidity(thisTextBox.Text) ? 
				TextBox.DefaultForeColor : Color.Red;
		}

		private void textBoxPortNo_TextChanged(object sender, EventArgs e) {
			TextBox thisTextBox = sender as TextBox;
			thisTextBox.ForeColor = Extension.Checker.Text.CheckTextValidity(thisTextBox.Text, TextType.IntegerType, 0, 99999) ?
				TextBox.DefaultForeColor : Color.Red;
		}

		private void buttonApply_Click(object sender, EventArgs e) {
			try {
				if (textBoxIpAddress.ForeColor == Color.Red ||
					textBoxPortNo.ForeColor == Color.Red || 
					numericUpDownHeartBeatRate.Value < numericUpDownMinimumHeartBeatRate.Value) {
					MessageBox.Show("One or more settings are invalid!", "Error");
					return;
				}
				settings.EnableHeartBeat = checkBoxEnableHeartBeat.Checked;
				settings.AutoOpen = checkBoxAutoOpen.Checked;
				settings.FindLocalIP = checkBoxFindLocalIP.Checked;
				settings.IPV4Address = textBoxIpAddress.Text;
				settings.PortNo = Convert.ToInt32(textBoxPortNo.Text);
				settings.HeartBeatRate = (int)numericUpDownHeartBeatRate.Value;
				settings.MinimumHeartBeatRate = (int)numericUpDownMinimumHeartBeatRate.Value;
				settings.MaxNoOfPendingClient = (int)numericUpDownMaxNoOfPendingClients.Value;
				settings.MaxPortNo = (int)numericUpDownMaxPortNo.Value;
				DialogResult = DialogResult.OK;
				Close();
			} catch (Exception ex) {
				MessageBox.Show("Fail to apply the changes! " + ex.ToString(), "Error");
				return;
			}
		}

		private void buttonCancel_Click(object sender, EventArgs e) {
			DialogResult = DialogResult.Cancel;
			Close();
		}
	}
}
