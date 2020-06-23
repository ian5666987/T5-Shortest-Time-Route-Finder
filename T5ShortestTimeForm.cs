using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using System.Reflection;
using System.Diagnostics; //for performance measurement...

using T5ShortestTime.MOAct;
using T5ShortestTime.SATS;

using Extension.Debugger;
using Extension.Versioning;
using Extension.Database.OldOracle;
using Extension.Socket;
using Extension.Manipulator;
using T5ShortestTime.Models;

namespace T5ShortestTime
{
	public partial class T5ShortestTimeForm : Form {
		#region variables
		//Debugging
		private LogBoxForm logBox = new LogBoxForm();

		//Back-end logic
		private T5ShortestTimeLogic t5Logic; //separation of UI & Logic, started from v2.8.0.0

		//Monitoring purpose
		private Timer displayTimer = new Timer();
		private Timer tableTimer = new Timer(); //read per 25 milliseconds
		private Timer heartBeatTimer = new Timer(); //definitely tick per second
		private TimeSpan runningTime;
		private DateTime startTime = DateTime.Now;
		private int searchManualCount = 0;
		private int searchAutoCount = 0;
		private DateTime lastManualSearchOn = DateTime.Now;
		private DateTime lastAutoSearchOn = DateTime.Now;
		private string timeFormat = "yyyy-MM-dd HH:mm:ss.fff";

		//Network monitor
		private DateTime lastHeartBeatSent = DateTime.Now;

		//Drawing
		private Bitmap green_indicator, red_indicator, gray_indicator, seablue_indicator;

		//DataGridView
		private OracleTableViewForm oracleTvForm = new OracleTableViewForm();
    private GenericTableViewForm genericTvForm = new GenericTableViewForm();
		#endregion

		public T5ShortestTimeForm() {
			InitializeComponent();

      //Versioning
			Assembly assembly = Assembly.GetExecutingAssembly();
			FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
			string version = fvi.FileVersion;
			Text += " v" + version;

			t5Logic = new T5ShortestTimeLogic(Application.StartupPath);
			oracleTvForm.PreventClosing = true; //shouldn't be closed by X Control button
      genericTvForm.PreventClosing = true;

			//Timing
			logBox.GetTimeLapse(1); //for overall timing
			logBox.GetTimeLapse(); //for basic initialization timing

			//Basic initialization
			logBox.WriteTimedLogLine(Text.ToString() + " (c)2016 - by Ian. Released: " + TimeStamp.RetrieveLinkerTimestamp().ToString() + " (Singapore Time)"); //The first to be printed      
			try { //Initialize drawings and pictures
				gray_indicator = Properties.Resources.gray_indicator;
				red_indicator = Properties.Resources.red_indicator;
				green_indicator = Properties.Resources.green_indicator;
				seablue_indicator = Properties.Resources.seablue_indicator;
				pictureBoxConnection.Image = gray_indicator;
				pictureBoxCS.Image = gray_indicator;
				pictureBoxTcpConnection.Image = gray_indicator;
			} catch (Exception e) {
				logBox.WriteTimedLogLine(e.ToString(), Color.Red);
			}
			logBox.WriteTimedLogLine("Basic initilization completed... (" + logBox.GetTimeLapse() + ")", Color.Green);

			//Initialize the app settings, the most important...      
      //Do not continue if this fails
			logBox.GetTimeLapse();
			t5Logic.InitApp();
			bool result = !t5Logic.HasError;
			logBox.WriteTimedLogLine((result ?
				"Shared settings config file(s) successfully loaded!" : t5Logic.GetError()) +
				" (" + logBox.GetTimeLapse() + ")", result ? Color.Green : Color.Red);
			if (t5Logic.AppSettings.EquipmentIDList.Count <= 0) {
				groupBoxAppSettings.Enabled = false;
				Enabled = false; //disable the whole form!
				MessageBox.Show("Fail to initialize! No equipment is found!", "Fatal error");
				return;
			}

			//Initialize workstation (may fail, that's ok)
			logBox.GetTimeLapse();
			t5Logic.InitWorkstation();
			result = !t5Logic.HasError;
			logBox.WriteTimedLogLine((result ? "Join PU-DO in workstation info is successfully loaded!" : t5Logic.GetError()) +
				" (" + logBox.GetTimeLapse() + ")", result ? Color.Green : Color.Red);

			//Initialize (heart beat) timer
			heartBeatTimer.Interval = t5Logic.ServerHandler.Settings.MinimumHeartBeatRate; //always tick every 100 ms, but...
			heartBeatTimer.Tick += heartBeatTimer_Tick;

			//Initialize TCP-IP settings
			initHeartBeat();
			t5Logic.ServerHandler.ClientAccepted += serverHandler_ClientAccepted;
			t5Logic.ServerHandler.ClientDisposed += serverHandler_ClientDisposed;
			t5Logic.ServerHandler.PackageReceived += serverHandler_PackageReceived;
			t5Logic.ServerHandler.ErrorMessageReceived += serverHandler_ErrorMessageReceived;

			//Initialize equipments
			logBox.GetTimeLapse();
			t5Logic.InitEquipments();
			result = !t5Logic.HasError;
			logBox.WriteTimedLogLine((result ? "Equipment initialization successfully completed!" : t5Logic.GetError()) +
				" (" + logBox.GetTimeLapse() + ")", result ? Color.Green : Color.Red);
			foreach (var eqpId in t5Logic.AppSettings.EquipmentIDList) {
				ToolStripItem tsi = new ToolStripMenuItem() {
					Name = "equipmentToolStripMenuItem" + eqpId,
					Size = new Size(152, 24),
					Text = eqpId,
				};
				tsi.Click += equipmentToolStripMenuItemInstance_Click;
				equipmentToolStripMenuItem.DropDownItems.Add(tsi);
			}

			//Initialize Diagnostics
			logBox.GetTimeLapse();
			initGUISharedSettings(t5Logic.DbHandler.TableAndTest);
			initGUIInitialization(); //after equipment
			t5Logic.InitDiagnostics();
			result = !t5Logic.HasError;
			logBox.WriteTimedLogLine((result ? "Performance counter check ok..." : t5Logic.GetError()) +
				" (" + logBox.GetTimeLapse() + ")", result ? Color.Green : Color.Red);

			//Initialize (display) timer
			logBox.GetTimeLapse();
			displayTimer.Interval = t5Logic.AppSettings.DisplayTimerTickInMs;
			displayTimer.Tick += displayTimer_Tick;
			displayTimer.Start(); //always ticking

			//Initialize (table) timer
			tableTimer.Interval = t5Logic.AppSettings.TableTimerTickInMs; //must be quick enough to response
			tableTimer.Tick += tableTimer_Tick;
			tableTimer.Start();

			logBox.WriteTimedLogLine("GUI and timers initialization completed... (" + logBox.GetTimeLapse() + ")", Color.Blue);

			//DB auto-connect
			logBox.GetTimeLapse();
			t5Logic.InitAutoConnect();
			result = !t5Logic.HasError;
			logBox.WriteTimedLogLine((result ? "DB auto - connect(" +
				t5Logic.AutoConnectResult.ToString() + ")..." : t5Logic.GetError()) +
				" (" + logBox.GetTimeLapse() + ")", result ? Color.Green : Color.Red);
			if (t5Logic.AutoConnectResult)
        updateControls(t5Logic.IsDataConnectionReady); //for initialization, only update the control if it is auto-connect
                                                       //updateControls(t5Logic.DbHandler.IsConnectionUp()); //for initialization, only update the control if it is auto-connect

      //TV form			
      if (!PH.IsSimulation)
        oracleTvForm.SetOracleHandler(t5Logic.DbHandler);

			//Time calculation
			logBox.WriteTimedLogLine("Initialization completed! (" + logBox.GetTimeLapse(1) + ")", Color.Green);
		}

		private void serverHandler_ErrorMessageReceived(object sender, ServerEventArgs e) {
			//throw new NotImplementedException();
			logBox.WriteTimedLogLine("Error message from TCP-IP Client: " + e.ErrorMessage, Color.Red);
		}

		private void serverHandler_PackageReceived(object sender, ServerEventArgs e) {
			string msgContent = Data.GetVisualStringOfBytes(e.Package);
			logBox.WriteTimedLogLine("Message from TCP-IP Client: " + msgContent);
		}

		private void serverHandler_ClientDisposed(object sender, ServerEventArgs e) {
			//textBoxNoOfClient.Text = serverHandler.ClientNo.ToString(); //no need to implement anything
			logBox.WriteTimedLog("Client " + e.DisposedClientNo.ToString() + " is disposed!\n", Color.Red);
		}

		private void serverHandler_ClientAccepted(object sender, ServerEventArgs e) {
			string ipStr = e.ClientIPAddress.ToString();
			logBox.WriteTimedLog("Client " + e.AcceptedClientNo.ToString() + " [" + ipStr + "] " + "is accepted!\n", Color.Blue);			
			//string welcomeMessage = "This is a welcome message!";
			//byte[] msg = Encoding.ASCII.GetBytes(welcomeMessage);
			//t5Logic.ServerHandler.Send(e.AcceptedClientNo, msg);
		}

		#region timers
		private bool processTableTimerTick = false;
		private void tableTimer_Tick(object sender, EventArgs e) {
			if (processTableTimerTick || !t5Logic.CanProcessTable())
				return;
			processTableTimerTick = true; //one process at a time
			string eqpId = comboBoxEquipmentList.SelectedItem.ToString();
			t5Logic.HandlePendingEquipment(); //if there is update, it should return it			
			updateDisplay();
			t5Logic.ProcessTable();
			if (t5Logic.SearchCommandAccepted) {
				t5Logic.SearchCommandAccepted = false;
				++searchAutoCount;
				lastAutoSearchOn = DateTime.Now;
				updateSearchCountAndTime();
			}
			updateDisplay();
			tableTimer.Interval = t5Logic.FailedWithException ? 5000 : t5Logic.AppSettings.TableTimerTickInMs; //TODO currently 5000 is hard-coded
			processTableTimerTick = false;
		}

		private bool processDisplayTimerTick = false;
		private void displayTimer_Tick(object sender, EventArgs e) {
			if (processDisplayTimerTick)
				return;
			processDisplayTimerTick = true; //one process at a time
			runningTime = DateTime.Now - startTime;
			labelSessionTime.Text = "Session Time: " + ((int)runningTime.TotalDays).ToString("D4") + " " +
				runningTime.Hours.ToString("D2") + ":" + runningTime.Minutes.ToString("D2") + ":" +
				runningTime.Seconds.ToString("D2");
			displayCurrentState();
			while (Equipment.LogQueue.Count > 0) {
				string msg = Equipment.GetTopLogQueue();
				if (msg == null || msg == string.Empty)
					break;
				logBox.WriteTimedLogLines(msg
					.Split(new string[] { Environment.NewLine }, StringSplitOptions.None));
			}
			//TCP state
			textBoxNoOfClient.Text = t5Logic.ServerHandler.ClientNo.ToString();
			processDisplayTimerTick = false;
		}

		private void heartBeatTimer_Tick(object sender, EventArgs e) { //this is executed according to the minimum heart beat rate which is 100 ms
      try {
        TimeSpan ts = DateTime.Now - lastHeartBeatSent;
        if (ts.TotalMilliseconds > t5Logic.ServerHandler.Settings.HeartBeatRate) { //though the ping follows the heart beat rate, not so the timer tick execution
          t5Logic.ServerHandler.Ping();
          lastHeartBeatSent = DateTime.Now;
        }
      } catch { //this catch cannot be displayed, otherwise the log can be too large
        //2018-07-05, follows v2.10.2.4 onwards have this catch because it seems like sometimes the "Ping" may cause some issue
        lastHeartBeatSent = DateTime.Now; //update the last heart beat sent
      }
		}
		#endregion

		#region GUI load, and display
		private void updateDisplay() {
			string eqpId = comboBoxEquipmentList.SelectedItem.ToString();
			if (t5Logic.HasIndicatorUpdate) {
				labelSolutionCompleted.Text = "Processing";
				pictureBoxSolutionCompleted.Image = seablue_indicator;
			}
			if (t5Logic.HasCmdStatUpdate) //has update
				updateTableLightDisplay(t5Logic.CmdStatUpdate);
			if (t5Logic.HasInitStateUpdate)
				textBoxInitialState.Text = t5Logic.GetEquipmentInitialState(eqpId)?.ToString();
			if (t5Logic.HasError) //has some error message
				logBox.WriteTimedLogLine(t5Logic.GetError(), Color.Red);
		}

		private void updateTableLightDisplay(CmdStat cmdStat) {
			switch (cmdStat) {
				case CmdStat.Processing:
					pictureBoxCS.Image = seablue_indicator;
					break;
				case CmdStat.Completed:
				case CmdStat.Terminated:
					pictureBoxCS.Image = green_indicator;
					break;
				case CmdStat.Pending:
				case CmdStat.Requesting:
				case CmdStat.Terminating:
				case CmdStat.Uninitialized:
					pictureBoxCS.Image = gray_indicator;
					break;
				default:
					pictureBoxCS.Image = red_indicator;
					break;
			}
			labelCS.Text = cmdStat.ToString();
		}

		private void updateButtonAndSolutionLightDisplay(bool isSearching, bool isTerminated, bool isCompleted, bool isPending) {
			buttonSearch.Text = isSearching ? "Terminate" : "Search";
			if (isSearching && labelSolutionCompleted.Text != "Processing") {
				pictureBoxSolutionCompleted.Image = seablue_indicator;
				labelSolutionCompleted.Text = "Processing";
			}
			if (isTerminated && labelSolutionCompleted.Text != "Terminated") {
				pictureBoxSolutionCompleted.Image = green_indicator;
				labelSolutionCompleted.Text = "Terminated";
			}
			if (isCompleted && labelSolutionCompleted.Text != "Completed") {
				pictureBoxSolutionCompleted.Image = green_indicator;
				labelSolutionCompleted.Text = "Completed";
			}
			if (isPending && labelSolutionCompleted.Text != "Pending") {
				pictureBoxSolutionCompleted.Image = red_indicator;
				labelSolutionCompleted.Text = "Pending";
			}
			if (!isSearching && !isTerminated && !isPending &&
				!isCompleted && labelSolutionCompleted.Text != "Uninitialized") {
				pictureBoxSolutionCompleted.Image = gray_indicator;
				labelSolutionCompleted.Text = "Uninitialized";
			}
		}

		private void initHeartBeat(bool manualInit = false) {
			logBox.GetTimeLapse();
			t5Logic.InitNetwork(manualInit);
			var result = !t5Logic.HasError;
			logBox.WriteTimedLogLine((result ? "Network TCP-IP Heart beat is successfully initialized!" : t5Logic.GetError()) +
				" (" + logBox.GetTimeLapse() + ")", result ? Color.Green : Color.Red);
			logBox.WriteTimedLogLine(t5Logic.ServerHandler.InitMessage,
				t5Logic.ServerHandler.InitResult ? Color.Green : Color.Red);
			if (t5Logic.ServerHandler.Settings.AutoOpen || manualInit) {
				pictureBoxTcpConnection.Image = t5Logic.ServerHandler.AutoOpenResult ? green_indicator : red_indicator;
				if (t5Logic.ServerHandler.AutoOpenResult)
					heartBeatTimer.Start();
				else
					heartBeatTimer.Stop();
			} else if (!manualInit) {
				pictureBoxTcpConnection.Image = gray_indicator;
				heartBeatTimer.Stop();
			}
		}

		private void displayInitialState(ActInitialState state) {
			textBoxInitialState.Text = state?.ToString();
		}

		private void displayCurrentState() {
			T5STLogicState cs = t5Logic.GetCurrentState();
			if (cs == null) {
				logBox.WriteTimedLogLine("Null current state retrieval!", Color.Red);
				displayTimer.Interval = 5000;
				return;
			}
			displayTimer.Interval = t5Logic.AppSettings.DisplayTimerTickInMs;
			updateButtonAndSolutionLightDisplay(
				cs.Flags.IsSearching, cs.Flags.IsTerminated, cs.Flags.IsCompleted, cs.Flags.IsPending);
			if (cs.Pf == null) {
				logBox.WriteTimedLog("Null performance retrieval! " + cs.Flags.ID + "\n", Color.Red);
				return;
			}

			textBoxExecutionTimeTotal.Text = cs.Pf.TotalTimeLapse.ToString("N0");
			textBoxExecutionTimeCurrentIteration.Text = cs.Pf.IterationTimeLapse.ToString("N0");
			textBoxEL.Text = cs.Pf.EvCount.EvaluatedLeaves.ToString("N0");
			textBoxEN.Text = cs.Pf.EvCount.EvaluatedNodes.ToString("N0");
			textBoxAN.Text = cs.Pf.EvCount.AbortedNodes.ToString("N0");
			textBoxLN.Text = (cs.Pf.EvCount.CreatedNodes - cs.Pf.EvCount.DestroyedNodes).ToString("N0");
			textBoxSpeed.Text = cs.Pf.LeafSpeed.ToString("N0") + " | " + cs.Pf.NodeSpeed.ToString("N0");

			string stableStr = cs.Pf.CurrentSessionDepth.ToString();
			if (stableStr != textBoxSessionDepth.Text)
				textBoxSessionDepth.Text = cs.Pf.CurrentSessionDepth.ToString();

			stableStr = cs.Pf.IterationNo.ToString() + "-" + cs.Pf.EvaluatedBranchCode;
			if (stableStr != textBoxIteration.Text)
				textBoxIteration.Text = stableStr;

			stableStr = cs.Pf.CurrentIterationDepth.ToString() + " [+" + cs.Pf.IterationNodeExtraDepth.ToString() + "]";
			if (stableStr != textBoxIterationDepth.Text)
				textBoxIterationDepth.Text = stableStr;

			stableStr = cs.Pf.IterationNodesEvaluated.ToString("N0");
			if (stableStr != textBoxIterationNodesEvaluated.Text)
				textBoxIterationNodesEvaluated.Text = stableStr;

			stableStr = cs.Pf.IterationNodes.ToString("N0");
			if (stableStr != textBoxIterationNodes.Text)
				textBoxIterationNodes.Text = stableStr;

			stableStr = cs.EqpState.TotalMemoryMB.ToString("N1");
			if (stableStr != textBoxUsedRAM.Text)
				textBoxUsedRAM.Text = stableStr;

			stableStr = cs.EqpState.AverageBranchingFactor.ToString("N1"); ;
			if (stableStr != textBoxAvgBranching.Text)
				textBoxAvgBranching.Text = stableStr;

			stableStr = cs.EqpState.TotalDepth.ToString("N0");
			if (stableStr != textBoxMOActsNo.Text)
				textBoxMOActsNo.Text = stableStr;

			if (t5Logic.AppSettings.TrackResourcesUsage) {
				textBoxAvailableRAM.Text = t5Logic.NextRAMValue.ToString("N1");
				textBoxUsedCPU.Text = t5Logic.NextCPUValue.ToString("N1");
			} else {
				textBoxAvailableRAM.Text = "";
				textBoxUsedCPU.Text = "";
			}

			string sourceCode = "NA";
			KeyValuePair<List<Act>, int> moActFinalSolution = cs.EqpState.Solution;
			string routeMsg = "D" + cs.EqpState.BestSolvedDepth + "|" + cs.EqpState.SolvedDepth + "|" + sourceCode;
			if (moActFinalSolution.Key == null) { //Not yet available, but now not possible
				textBoxRouteSolution.Text = routeMsg;
				richTextBoxBestRoute.Text = "NA";
				textBoxSolutionShortestTime.Text = "NA";
				return;
			}

			string timeText = ((sourceCode != "FS" && !cs.Flags.HasEnded) ?
				(moActFinalSolution.Key.Count < cs.EqpState.TotalDepth ? ">=" : "<=") : "") +
				(0.1 * moActFinalSolution.Value).ToString("N1");
			if (textBoxSolutionShortestTime.Text != timeText)
				textBoxSolutionShortestTime.Text = timeText;

			routeMsg += "|" + moActFinalSolution.Key.Count;
			if (textBoxRouteSolution.Text != routeMsg)
				textBoxRouteSolution.Text = routeMsg;
			string routeStr = "";
			for (int i = 0; i < moActFinalSolution.Key.Count; ++i)
				routeStr += moActFinalSolution.Key[i].GetString() + " ";
			if (routeStr != richTextBoxBestRoute.Text)
				richTextBoxBestRoute.Text = routeStr;
		}

		private void initGUIInitialization() {
			pictureBoxSolutionCompleted.Image = gray_indicator;
			if (t5Logic.AppSettings.EquipmentIDList.Count > 0)
				comboBoxEquipmentList.SelectedIndex = 0;//only if SATS equipment is available, we can show the equipment...      
			updateGui();
		}

		private void updateGui() {
			buttonSearch.Enabled = t5Logic.AppSettings.Mode != T5STMode.Auto;
			labelMode.Text = "Mode: " + t5Logic.AppSettings.Mode.ToString();
			logBox.Visible = t5Logic.AppSettings.ShowLogBox;
			if (!t5Logic.AppSettings.ShowLogBox)
				t5Logic.HideAllLogBox();
		}

		private void initGUISharedSettings(T5TableAndTest tableAndTest) {
			if (t5Logic.AppSettings.EquipmentIDList.Any())
				comboBoxEquipmentList.Items.AddRange(t5Logic.AppSettings.EquipmentIDList.ToArray());
			initGUITableAndTest(tableAndTest);
			if (t5Logic.AppSettings.LockTable) {
				comboBoxTableName.Enabled = false;
				buttonReadTable.Enabled = false;
			}
			updateGui();
		}

		private void initGUITableAndTest(T5TableAndTest tableAndTest) {
			comboBoxTableName.Items.Clear();
			comboBoxTableName.Items.AddRange(tableAndTest.TableNames.Select(x => x.Value + " [" + x.Key + "]").ToArray());
			if (comboBoxTableName.Items.Count > 0)
				comboBoxTableName.SelectedIndex = 0;
			checkBoxFixedTime.Checked = tableAndTest.UseFixedDateTime;
			numericUpDownReadTableLimit.Value = tableAndTest.DisplayLimit;
			checkBoxTableViewAddIndex.Checked = tableAndTest.AddIndex;
			checkBoxShowTableView.Checked = tableAndTest.ShowTableView;
			if (tableAndTest.UseFixedDateTime)
				dateTimePickerFixed.Value = tableAndTest.FixedDateTime;
		}

		private T5TableAndTest getTableAndTestFromGUI() { //TODO note that now we handle the events in the tableTest GUI one by one as it changes the TableTest we use. Then the TableTest isn't really the one used to save here. Maybe bad duplicate.
			T5TableAndTest tableAndTest = new T5TableAndTest();
			tableAndTest.TableNameAliases = comboBoxTableName
				.Items.Cast<string>()
				.Select(x => x.Split(new char[] { ' ' }, 2))
				.Select(x => new AliasNamePair(x[1].Substring(1, x[1].Length - 2), x[0]))
				.ToArray();
			tableAndTest.UseFixedDateTime = checkBoxFixedTime.Checked;
			tableAndTest.ShowTableView = checkBoxShowTableView.Checked;
			tableAndTest.AddIndex = checkBoxTableViewAddIndex.Checked;
			tableAndTest.DisplayLimit = (int)numericUpDownReadTableLimit.Value;
			tableAndTest.FixedDateTime = dateTimePickerFixed.Value; //update this no matter what
			return tableAndTest;
		}

		private void loadEquipmentSettingsToGUI(string id) {
			T5STEqpFlags flags = t5Logic.GetEquipmentFlags(id);
			textBoxInitialState.Text = t5Logic.GetEquipmentInitialState(id)?.ToString();
			updateButtonAndSolutionLightDisplay(flags.IsSearching, flags.IsTerminated, flags.IsCompleted, flags.IsPending);
		}
		#endregion

		#region non-timer components' event handlers
		private void buttonSearch_Click(object sender, EventArgs e) {
			bool isInitStateUpdated;
			++searchManualCount;
			lastManualSearchOn = DateTime.Now;
			updateSearchCountAndTime();
			t5Logic.ManualSearch(out isInitStateUpdated);
			updateDisplay();
			loadEquipmentSettingsToGUI(comboBoxEquipmentList.SelectedItem.ToString());
		}

		private void buttonReadTable_Click(object sender, EventArgs e) {
			int limit = (int)numericUpDownReadTableLimit.Value;
      bool addIndex = checkBoxTableViewAddIndex.Checked;
      if (PH.IsSimulation) {
        string tableAlias = comboBoxTableName.SelectedItem.ToString().Split(' ').Last().Trim(); //the last element is the table alias with square bracket
        tableAlias = tableAlias.Substring(1, tableAlias.Length - 2);
        Tuple<List<string>, List<List<object>>> tableData = null;
        switch (tableAlias) {
          case "Info": tableData = new MoInfoView().GetTable(addIndex, limit); break;
          case "LED": case "LED_S": tableData = new LedInfoReduced().GetTable(addIndex, limit); break;
          case "EQP": tableData = new EqpInfoReduced().GetTable(addIndex, limit); break;
          case "OptFlag": tableData = new OptFlag().GetTable(addIndex, limit); break;
          case "Solution":
            List<string> eqpIds = new List<string>();
            foreach(string item in comboBoxEquipmentList.Items)
              eqpIds.Add(item);            
            tableData = new OptSolution().GetTable(eqpIds, addIndex, limit); break;
        }
        if (tableData == null)
          return;
        genericTvForm.DisplayResult(tableData.Item1, tableData.Item2);
      } else {
        string tableName = comboBoxTableName.SelectedItem.ToString().Split(' ').First().Trim(); //the first element is the table name
        oracleTvForm.DisplayTableResult(tableName, "", limit, addIndex);
      }
		}

		private void comboBoxEquipmentList_SelectedIndexChanged(object sender, EventArgs e) {
			string eqpId = comboBoxEquipmentList.SelectedItem.ToString();
			t5Logic.CurrentUIEqpId = eqpId; //very important to monitor as of now...
			loadEquipmentSettingsToGUI(eqpId);
		}

		#region table and test event handlers
		private void checkBoxFixedTime_CheckedChanged(object sender, EventArgs e) {
			dateTimePickerFixed.Enabled = checkBoxFixedTime.Checked;
			if (t5Logic.DbHandler == null || t5Logic.DbHandler.TableAndTest == null)
				return;
			t5Logic.DbHandler.TableAndTest.UseFixedDateTime = checkBoxFixedTime.Checked;
			t5Logic.DbHandler.TableAndTest.FixedDateTime = dateTimePickerFixed.Value;
		}

		private void checkBoxShowTableView_CheckedChanged(object sender, EventArgs e) {
      oracleTvForm.Visible = checkBoxShowTableView.Checked && !PH.IsSimulation;
      genericTvForm.Visible = checkBoxShowTableView.Checked && PH.IsSimulation;
      if (t5Logic.DbHandler == null || t5Logic.DbHandler.TableAndTest == null)
				return;
			t5Logic.DbHandler.TableAndTest.ShowTableView = checkBoxShowTableView.Checked;
		}

		private void checkBoxTableViewAddIndex_CheckedChanged(object sender, EventArgs e) {
			if (t5Logic.DbHandler == null || t5Logic.DbHandler.TableAndTest == null)
				return;
			t5Logic.DbHandler.TableAndTest.AddIndex = checkBoxTableViewAddIndex.Checked;
		}

		private void numericUpDownReadTableLimit_ValueChanged(object sender, EventArgs e) {
			if (t5Logic.DbHandler == null || t5Logic.DbHandler.TableAndTest == null)
				return;
			t5Logic.DbHandler.TableAndTest.DisplayLimit = (int)numericUpDownReadTableLimit.Value;
		}

		private void dateTimePickerFixed_ValueChanged(object sender, EventArgs e) {
			if (t5Logic.DbHandler == null || t5Logic.DbHandler.TableAndTest == null)
				return;
			t5Logic.DbHandler.TableAndTest.FixedDateTime = dateTimePickerFixed.Value;
		}
		#endregion
		#endregion

		#region update controls
		private void updateSearchCountAndTime() {
			textBoxSearchCount.Text = searchManualCount.ToString() + "|" + searchAutoCount.ToString();
			labelLastSearchOn.Text = "Last: " + lastManualSearchOn.ToString(timeFormat) + " SGT (manual) | " + lastAutoSearchOn.ToString(timeFormat) + " SGT (auto)";
		}
		private void updateControls(bool isConnected) {
			pictureBoxConnection.Image = isConnected ? green_indicator : red_indicator;
			connectToolStripMenuItem.Enabled = !isConnected;
			disconnectToolStripMenuItem.Enabled = isConnected;
			labelDBConnStat.Text = isConnected ? "Connected" : "Disconnected";
			buttonSearch.Enabled = isConnected;
			groupBoxSolution.Enabled = isConnected;
			groupBoxPerformance.Enabled = isConnected;
			groupBoxTablesAndTestings.Enabled = isConnected;
			labelCS.Enabled = isConnected;
		}
		#endregion

		#region menu strip
		private void connectToolStripMenuItem_Click(object sender, EventArgs e) {
			ToolStripMenuItem item = sender as ToolStripMenuItem;
			OracleConnectionSettingsForm form = new OracleConnectionSettingsForm(t5Logic.DbHandler, t5Logic.DbConnectionSettings);
			if (form == null || form.IsDisposed)
				return;
			if (form.ShowDialog() != DialogResult.OK)
				return;
      updateControls(t5Logic.IsDataConnectionReady); //the latest status determine...
			//updateControls(t5Logic.DbHandler.IsConnectionUp()); //the latest status determine...
		}

		private void infoToolStripMenuItem_Click(object sender, EventArgs e) {
			ToolStripMenuItem item = sender as ToolStripMenuItem;
			OracleConnectionSettingsForm form = new OracleConnectionSettingsForm(t5Logic.DbHandler, t5Logic.DbConnectionSettings);
			form.AsInfo = true;
			form.ShowDialog();
		}

		private void tableToolStripMenuItem_Click(object sender, EventArgs e) {
			ToolStripMenuItem item = sender as ToolStripMenuItem;
			T5STTableAndTestingForm form = new T5STTableAndTestingForm(t5Logic.DbHandler.TableAndTest);
			if (form != null && !form.IsDisposed) {
				if (form.ShowDialog() != DialogResult.OK)
					return;
				initGUITableAndTest(t5Logic.DbHandler.TableAndTest);
			}
		}

		private void watcherToolStripMenuItem_Click(object sender, EventArgs e) {
			try {
				ToolStripMenuItem item = sender as ToolStripMenuItem;
				T5STAppWatcherForm form = new T5STAppWatcherForm(t5Logic.ServerHandler.Settings);
				if (form != null && !form.IsDisposed) {
					if (form.ShowDialog() != DialogResult.OK)
						return;
					t5Logic.SaveWatcherSettings();
					initHeartBeat();
				}
			} catch (Exception ex) {
				MessageBox.Show("Error while attempting to initialize/change watcher settings!", "Error");
				logBox.WriteTimedLogLine("Error while attempting to initialize/change watcher settings! " + ex.ToString(), Color.Red);
			}
		}

		private void globalTimingToolStripMenuItem_Click(object sender, EventArgs e) {
			ToolStripMenuItem item = sender as ToolStripMenuItem;
			T5STGlobalTimingForm form = new T5STGlobalTimingForm(ActHandler.GlobalActTimeSettings);
			if (form != null && !form.IsDisposed) {
				if (form.ShowDialog() != DialogResult.OK)
					return;
				//TODO add something whenever necessary
			}
		}

		private void disconnectToolStripMenuItem_Click(object sender, EventArgs e) {
      if (!PH.IsSimulation)
			  t5Logic.DbHandler.CloseConnection();
      updateControls(t5Logic.IsDataConnectionReady); //the latest status determine...
      //updateControls(t5Logic.DbHandler.IsConnectionUp());
		}

		private void applicationToolStripMenuItem_Click(object sender, EventArgs e) {
			ToolStripMenuItem item = sender as ToolStripMenuItem;
			T5STAppSettingsForm form = new T5STAppSettingsForm(t5Logic.AppSettings);
			if (form != null && !form.IsDisposed) {
				if (form.ShowDialog() != DialogResult.OK)
					return;
				updateGui();
				Equipment.IncludePC = t5Logic.AppSettings.TrackResourcesUsage; //anytime
			}
		}		

		private void equipmentToolStripMenuItemInstance_Click(object sender, EventArgs e) {
			ToolStripMenuItem item = sender as ToolStripMenuItem;
			Equipment equipment = t5Logic.GetEquipment(item.Text);
			if (equipment == null) {//do nothing
				logBox.WriteTimedLogLine("Equipment not found!", Color.Red);
				MessageBox.Show("Equipment not found!", "Error");
				return;
			}
			if (equipment.IsSearching) { //don't do anything when processing
				logBox.WriteTimedLogLine("Cannot change the equipment settings while processing!", Color.Red);
				MessageBox.Show("Cannot save the equipment settings while processing!", "Error");
				return;
			}
			T5STEqpSettingsForm form = new T5STEqpSettingsForm(equipment);
			if (form != null && !form.IsDisposed) {
				DialogResult result = form.ShowDialog();
				textBoxInitialState.Text = equipment.InitialState?.ToString(); //no matter what happen, the initial state still be updated
				if (result == DialogResult.OK)
					t5Logic.SaveEquipment(item.Text);
			}
		}
		#endregion menu strip
	}
}
