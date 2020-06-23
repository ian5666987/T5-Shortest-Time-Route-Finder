namespace T5ShortestTime
{
  partial class T5ShortestTimeForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
			//for (int i = 0; i < satsEquipments.Count; ++i)
			//  satsEquipments[i].Destroy();
			if (t5Logic != null) {
				t5Logic.DestroyAllEquipments();
				t5Logic.SaveState(getTableAndTestFromGUI());
        t5Logic.DbHandler.CloseConnection();
			}      
			if (disposing && (components != null))
        components.Dispose();
      if (oracleTvForm != null)
        oracleTvForm.Dispose();
      if (genericTvForm != null)
        genericTvForm.Dispose();
      if (logBox != null)
        logBox.Dispose();
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(T5ShortestTimeForm));
      this.textBoxExecutionTimeTotal = new System.Windows.Forms.TextBox();
      this.textBoxEL = new System.Windows.Forms.TextBox();
      this.label7 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.buttonSearch = new System.Windows.Forms.Button();
      this.groupBoxSolution = new System.Windows.Forms.GroupBox();
      this.textBoxInitialState = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.label28 = new System.Windows.Forms.Label();
      this.textBoxRouteSolution = new System.Windows.Forms.TextBox();
      this.label27 = new System.Windows.Forms.Label();
      this.label31 = new System.Windows.Forms.Label();
      this.textBoxIterationDepth = new System.Windows.Forms.TextBox();
      this.textBoxIterationNodes = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.textBoxIterationNodesEvaluated = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.textBoxIteration = new System.Windows.Forms.TextBox();
      this.label24 = new System.Windows.Forms.Label();
      this.labelSolutionCompleted = new System.Windows.Forms.Label();
      this.textBoxSessionDepth = new System.Windows.Forms.TextBox();
      this.pictureBoxSolutionCompleted = new System.Windows.Forms.PictureBox();
      this.richTextBoxBestRoute = new System.Windows.Forms.RichTextBox();
      this.label8 = new System.Windows.Forms.Label();
      this.textBoxSolutionShortestTime = new System.Windows.Forms.TextBox();
      this.label17 = new System.Windows.Forms.Label();
      this.label14 = new System.Windows.Forms.Label();
      this.labelDBConnStat = new System.Windows.Forms.Label();
      this.groupBoxPerformance = new System.Windows.Forms.GroupBox();
      this.labelLastSearchOn = new System.Windows.Forms.Label();
      this.textBoxSearchCount = new System.Windows.Forms.TextBox();
      this.label9 = new System.Windows.Forms.Label();
      this.label21 = new System.Windows.Forms.Label();
      this.label45 = new System.Windows.Forms.Label();
      this.textBoxAvgBranching = new System.Windows.Forms.TextBox();
      this.label40 = new System.Windows.Forms.Label();
      this.textBoxMOActsNo = new System.Windows.Forms.TextBox();
      this.label20 = new System.Windows.Forms.Label();
      this.textBoxLN = new System.Windows.Forms.TextBox();
      this.textBoxUsedCPU = new System.Windows.Forms.TextBox();
      this.textBoxUsedRAM = new System.Windows.Forms.TextBox();
      this.label30 = new System.Windows.Forms.Label();
      this.label39 = new System.Windows.Forms.Label();
      this.label32 = new System.Windows.Forms.Label();
      this.label38 = new System.Windows.Forms.Label();
      this.label29 = new System.Windows.Forms.Label();
      this.label36 = new System.Windows.Forms.Label();
      this.textBoxAvailableRAM = new System.Windows.Forms.TextBox();
      this.label26 = new System.Windows.Forms.Label();
      this.textBoxExecutionTimeCurrentIteration = new System.Windows.Forms.TextBox();
      this.label23 = new System.Windows.Forms.Label();
      this.textBoxSpeed = new System.Windows.Forms.TextBox();
      this.label22 = new System.Windows.Forms.Label();
      this.textBoxAN = new System.Windows.Forms.TextBox();
      this.label16 = new System.Windows.Forms.Label();
      this.textBoxEN = new System.Windows.Forms.TextBox();
      this.label10 = new System.Windows.Forms.Label();
      this.label12 = new System.Windows.Forms.Label();
      this.label11 = new System.Windows.Forms.Label();
      this.buttonReadTable = new System.Windows.Forms.Button();
      this.label35 = new System.Windows.Forms.Label();
      this.groupBoxAppSettings = new System.Windows.Forms.GroupBox();
      this.label42 = new System.Windows.Forms.Label();
      this.labelCS = new System.Windows.Forms.Label();
      this.comboBoxEquipmentList = new System.Windows.Forms.ComboBox();
      this.pictureBoxCS = new System.Windows.Forms.PictureBox();
      this.groupBoxTablesAndTestings = new System.Windows.Forms.GroupBox();
      this.checkBoxTableViewAddIndex = new System.Windows.Forms.CheckBox();
      this.checkBoxShowTableView = new System.Windows.Forms.CheckBox();
      this.label43 = new System.Windows.Forms.Label();
      this.dateTimePickerFixed = new System.Windows.Forms.DateTimePicker();
      this.checkBoxFixedTime = new System.Windows.Forms.CheckBox();
      this.numericUpDownReadTableLimit = new System.Windows.Forms.NumericUpDown();
      this.comboBoxTableName = new System.Windows.Forms.ComboBox();
      this.textBoxNoOfClient = new System.Windows.Forms.TextBox();
      this.groupBoxConnectionHBeat = new System.Windows.Forms.GroupBox();
      this.label5 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.pictureBoxTcpConnection = new System.Windows.Forms.PictureBox();
      this.pictureBoxConnection = new System.Windows.Forms.PictureBox();
      this.menuStripMain = new System.Windows.Forms.MenuStrip();
      this.connectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.disconnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.infoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.applicationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.equipmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.tableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.watcherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.globalTimingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.labelSessionTime = new System.Windows.Forms.Label();
      this.labelMode = new System.Windows.Forms.Label();
      this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
      this.groupBoxSolution.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSolutionCompleted)).BeginInit();
      this.groupBoxPerformance.SuspendLayout();
      this.groupBoxAppSettings.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCS)).BeginInit();
      this.groupBoxTablesAndTestings.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownReadTableLimit)).BeginInit();
      this.groupBoxConnectionHBeat.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTcpConnection)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxConnection)).BeginInit();
      this.menuStripMain.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
      this.SuspendLayout();
      // 
      // textBoxExecutionTimeTotal
      // 
      this.textBoxExecutionTimeTotal.Location = new System.Drawing.Point(192, 49);
      this.textBoxExecutionTimeTotal.Margin = new System.Windows.Forms.Padding(4);
      this.textBoxExecutionTimeTotal.Name = "textBoxExecutionTimeTotal";
      this.textBoxExecutionTimeTotal.ReadOnly = true;
      this.textBoxExecutionTimeTotal.Size = new System.Drawing.Size(78, 23);
      this.textBoxExecutionTimeTotal.TabIndex = 27;
      // 
      // textBoxEL
      // 
      this.textBoxEL.Location = new System.Drawing.Point(80, 18);
      this.textBoxEL.Margin = new System.Windows.Forms.Padding(4);
      this.textBoxEL.Name = "textBoxEL";
      this.textBoxEL.ReadOnly = true;
      this.textBoxEL.Size = new System.Drawing.Size(89, 23);
      this.textBoxEL.TabIndex = 24;
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(8, 21);
      this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(45, 17);
      this.label7.TabIndex = 18;
      this.label7.Text = "Count";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(8, 52);
      this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(39, 17);
      this.label6.TabIndex = 17;
      this.label6.Text = "Time";
      // 
      // buttonSearch
      // 
      this.buttonSearch.Enabled = false;
      this.buttonSearch.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.buttonSearch.Location = new System.Drawing.Point(117, 20);
      this.buttonSearch.Margin = new System.Windows.Forms.Padding(4);
      this.buttonSearch.Name = "buttonSearch";
      this.buttonSearch.Size = new System.Drawing.Size(82, 28);
      this.buttonSearch.TabIndex = 38;
      this.buttonSearch.Text = "Search";
      this.buttonSearch.UseVisualStyleBackColor = true;
      this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
      // 
      // groupBoxSolution
      // 
      this.groupBoxSolution.Controls.Add(this.textBoxInitialState);
      this.groupBoxSolution.Controls.Add(this.label2);
      this.groupBoxSolution.Controls.Add(this.label28);
      this.groupBoxSolution.Controls.Add(this.textBoxRouteSolution);
      this.groupBoxSolution.Controls.Add(this.label27);
      this.groupBoxSolution.Controls.Add(this.label31);
      this.groupBoxSolution.Controls.Add(this.textBoxIterationDepth);
      this.groupBoxSolution.Controls.Add(this.textBoxIterationNodes);
      this.groupBoxSolution.Controls.Add(this.label3);
      this.groupBoxSolution.Controls.Add(this.textBoxIterationNodesEvaluated);
      this.groupBoxSolution.Controls.Add(this.label4);
      this.groupBoxSolution.Controls.Add(this.textBoxIteration);
      this.groupBoxSolution.Controls.Add(this.label24);
      this.groupBoxSolution.Controls.Add(this.labelSolutionCompleted);
      this.groupBoxSolution.Controls.Add(this.textBoxSessionDepth);
      this.groupBoxSolution.Controls.Add(this.pictureBoxSolutionCompleted);
      this.groupBoxSolution.Controls.Add(this.richTextBoxBestRoute);
      this.groupBoxSolution.Controls.Add(this.label8);
      this.groupBoxSolution.Controls.Add(this.textBoxSolutionShortestTime);
      this.groupBoxSolution.Controls.Add(this.label17);
      this.groupBoxSolution.Controls.Add(this.label14);
      this.groupBoxSolution.Enabled = false;
      this.groupBoxSolution.Location = new System.Drawing.Point(9, 148);
      this.groupBoxSolution.Margin = new System.Windows.Forms.Padding(4);
      this.groupBoxSolution.Name = "groupBoxSolution";
      this.groupBoxSolution.Padding = new System.Windows.Forms.Padding(4);
      this.groupBoxSolution.Size = new System.Drawing.Size(821, 237);
      this.groupBoxSolution.TabIndex = 19;
      this.groupBoxSolution.TabStop = false;
      this.groupBoxSolution.Text = "Solution";
      // 
      // textBoxInitialState
      // 
      this.textBoxInitialState.Location = new System.Drawing.Point(60, 18);
      this.textBoxInitialState.Name = "textBoxInitialState";
      this.textBoxInitialState.ReadOnly = true;
      this.textBoxInitialState.Size = new System.Drawing.Size(747, 23);
      this.textBoxInitialState.TabIndex = 51;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(8, 18);
      this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(40, 17);
      this.label2.TabIndex = 50;
      this.label2.Text = "Initial";
      // 
      // label28
      // 
      this.label28.AutoSize = true;
      this.label28.Location = new System.Drawing.Point(339, 205);
      this.label28.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label28.Name = "label28";
      this.label28.Size = new System.Drawing.Size(13, 17);
      this.label28.TabIndex = 49;
      this.label28.Text = "-";
      // 
      // textBoxRouteSolution
      // 
      this.textBoxRouteSolution.Location = new System.Drawing.Point(247, 203);
      this.textBoxRouteSolution.Margin = new System.Windows.Forms.Padding(4);
      this.textBoxRouteSolution.Name = "textBoxRouteSolution";
      this.textBoxRouteSolution.ReadOnly = true;
      this.textBoxRouteSolution.Size = new System.Drawing.Size(90, 23);
      this.textBoxRouteSolution.TabIndex = 48;
      // 
      // label27
      // 
      this.label27.AutoSize = true;
      this.label27.Location = new System.Drawing.Point(590, 207);
      this.label27.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label27.Name = "label27";
      this.label27.Size = new System.Drawing.Size(12, 17);
      this.label27.TabIndex = 47;
      this.label27.Text = "/";
      // 
      // label31
      // 
      this.label31.AutoSize = true;
      this.label31.Location = new System.Drawing.Point(447, 206);
      this.label31.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label31.Name = "label31";
      this.label31.Size = new System.Drawing.Size(13, 17);
      this.label31.TabIndex = 46;
      this.label31.Text = "-";
      // 
      // textBoxIterationDepth
      // 
      this.textBoxIterationDepth.Location = new System.Drawing.Point(462, 204);
      this.textBoxIterationDepth.Margin = new System.Windows.Forms.Padding(4);
      this.textBoxIterationDepth.Name = "textBoxIterationDepth";
      this.textBoxIterationDepth.ReadOnly = true;
      this.textBoxIterationDepth.Size = new System.Drawing.Size(55, 23);
      this.textBoxIterationDepth.TabIndex = 45;
      // 
      // textBoxIterationNodes
      // 
      this.textBoxIterationNodes.Location = new System.Drawing.Point(604, 204);
      this.textBoxIterationNodes.Margin = new System.Windows.Forms.Padding(4);
      this.textBoxIterationNodes.Name = "textBoxIterationNodes";
      this.textBoxIterationNodes.ReadOnly = true;
      this.textBoxIterationNodes.Size = new System.Drawing.Size(55, 23);
      this.textBoxIterationNodes.TabIndex = 42;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(393, 206);
      this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(13, 17);
      this.label3.TabIndex = 41;
      this.label3.Text = "-";
      // 
      // textBoxIterationNodesEvaluated
      // 
      this.textBoxIterationNodesEvaluated.Location = new System.Drawing.Point(533, 204);
      this.textBoxIterationNodesEvaluated.Margin = new System.Windows.Forms.Padding(4);
      this.textBoxIterationNodesEvaluated.Name = "textBoxIterationNodesEvaluated";
      this.textBoxIterationNodesEvaluated.ReadOnly = true;
      this.textBoxIterationNodesEvaluated.Size = new System.Drawing.Size(55, 23);
      this.textBoxIterationNodesEvaluated.TabIndex = 40;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(519, 206);
      this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(12, 17);
      this.label4.TabIndex = 39;
      this.label4.Text = "/";
      // 
      // textBoxIteration
      // 
      this.textBoxIteration.Location = new System.Drawing.Point(408, 204);
      this.textBoxIteration.Margin = new System.Windows.Forms.Padding(4);
      this.textBoxIteration.Name = "textBoxIteration";
      this.textBoxIteration.ReadOnly = true;
      this.textBoxIteration.Size = new System.Drawing.Size(37, 23);
      this.textBoxIteration.TabIndex = 39;
      // 
      // label24
      // 
      this.label24.AutoSize = true;
      this.label24.Location = new System.Drawing.Point(167, 205);
      this.label24.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label24.Name = "label24";
      this.label24.Size = new System.Drawing.Size(74, 17);
      this.label24.TabIndex = 31;
      this.label24.Text = "Evaluation";
      // 
      // labelSolutionCompleted
      // 
      this.labelSolutionCompleted.AutoSize = true;
      this.labelSolutionCompleted.Location = new System.Drawing.Point(707, 206);
      this.labelSolutionCompleted.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.labelSolutionCompleted.Name = "labelSolutionCompleted";
      this.labelSolutionCompleted.Size = new System.Drawing.Size(84, 17);
      this.labelSolutionCompleted.TabIndex = 37;
      this.labelSolutionCompleted.Text = "Uninitialized";
      // 
      // textBoxSessionDepth
      // 
      this.textBoxSessionDepth.Location = new System.Drawing.Point(354, 204);
      this.textBoxSessionDepth.Margin = new System.Windows.Forms.Padding(4);
      this.textBoxSessionDepth.Name = "textBoxSessionDepth";
      this.textBoxSessionDepth.ReadOnly = true;
      this.textBoxSessionDepth.Size = new System.Drawing.Size(37, 23);
      this.textBoxSessionDepth.TabIndex = 32;
      // 
      // pictureBoxSolutionCompleted
      // 
      this.pictureBoxSolutionCompleted.BackColor = System.Drawing.Color.Transparent;
      this.pictureBoxSolutionCompleted.Location = new System.Drawing.Point(685, 207);
      this.pictureBoxSolutionCompleted.Name = "pictureBoxSolutionCompleted";
      this.pictureBoxSolutionCompleted.Size = new System.Drawing.Size(17, 17);
      this.pictureBoxSolutionCompleted.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
      this.pictureBoxSolutionCompleted.TabIndex = 36;
      this.pictureBoxSolutionCompleted.TabStop = false;
      // 
      // richTextBoxBestRoute
      // 
      this.richTextBoxBestRoute.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.richTextBoxBestRoute.Location = new System.Drawing.Point(60, 49);
      this.richTextBoxBestRoute.Name = "richTextBoxBestRoute";
      this.richTextBoxBestRoute.ReadOnly = true;
      this.richTextBoxBestRoute.Size = new System.Drawing.Size(748, 147);
      this.richTextBoxBestRoute.TabIndex = 29;
      this.richTextBoxBestRoute.Text = "";
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(127, 205);
      this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(30, 17);
      this.label8.TabIndex = 28;
      this.label8.Text = "sec";
      // 
      // textBoxSolutionShortestTime
      // 
      this.textBoxSolutionShortestTime.Location = new System.Drawing.Point(60, 204);
      this.textBoxSolutionShortestTime.Margin = new System.Windows.Forms.Padding(4);
      this.textBoxSolutionShortestTime.Name = "textBoxSolutionShortestTime";
      this.textBoxSolutionShortestTime.ReadOnly = true;
      this.textBoxSolutionShortestTime.Size = new System.Drawing.Size(65, 23);
      this.textBoxSolutionShortestTime.TabIndex = 27;
      // 
      // label17
      // 
      this.label17.AutoSize = true;
      this.label17.Location = new System.Drawing.Point(7, 205);
      this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label17.Name = "label17";
      this.label17.Size = new System.Drawing.Size(39, 17);
      this.label17.TabIndex = 26;
      this.label17.Text = "Time";
      // 
      // label14
      // 
      this.label14.AutoSize = true;
      this.label14.Location = new System.Drawing.Point(7, 48);
      this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label14.Name = "label14";
      this.label14.Size = new System.Drawing.Size(46, 17);
      this.label14.TabIndex = 17;
      this.label14.Text = "Route";
      // 
      // labelDBConnStat
      // 
      this.labelDBConnStat.AutoSize = true;
      this.labelDBConnStat.Location = new System.Drawing.Point(83, 25);
      this.labelDBConnStat.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.labelDBConnStat.Name = "labelDBConnStat";
      this.labelDBConnStat.Size = new System.Drawing.Size(84, 17);
      this.labelDBConnStat.TabIndex = 34;
      this.labelDBConnStat.Text = "Uninitialized";
      // 
      // groupBoxPerformance
      // 
      this.groupBoxPerformance.Controls.Add(this.labelLastSearchOn);
      this.groupBoxPerformance.Controls.Add(this.textBoxSearchCount);
      this.groupBoxPerformance.Controls.Add(this.label9);
      this.groupBoxPerformance.Controls.Add(this.label21);
      this.groupBoxPerformance.Controls.Add(this.label45);
      this.groupBoxPerformance.Controls.Add(this.textBoxAvgBranching);
      this.groupBoxPerformance.Controls.Add(this.label40);
      this.groupBoxPerformance.Controls.Add(this.textBoxMOActsNo);
      this.groupBoxPerformance.Controls.Add(this.label20);
      this.groupBoxPerformance.Controls.Add(this.textBoxLN);
      this.groupBoxPerformance.Controls.Add(this.textBoxUsedCPU);
      this.groupBoxPerformance.Controls.Add(this.textBoxUsedRAM);
      this.groupBoxPerformance.Controls.Add(this.label30);
      this.groupBoxPerformance.Controls.Add(this.label39);
      this.groupBoxPerformance.Controls.Add(this.label32);
      this.groupBoxPerformance.Controls.Add(this.label38);
      this.groupBoxPerformance.Controls.Add(this.label29);
      this.groupBoxPerformance.Controls.Add(this.label36);
      this.groupBoxPerformance.Controls.Add(this.textBoxAvailableRAM);
      this.groupBoxPerformance.Controls.Add(this.label26);
      this.groupBoxPerformance.Controls.Add(this.textBoxExecutionTimeCurrentIteration);
      this.groupBoxPerformance.Controls.Add(this.label23);
      this.groupBoxPerformance.Controls.Add(this.textBoxSpeed);
      this.groupBoxPerformance.Controls.Add(this.label22);
      this.groupBoxPerformance.Controls.Add(this.textBoxAN);
      this.groupBoxPerformance.Controls.Add(this.label16);
      this.groupBoxPerformance.Controls.Add(this.textBoxEN);
      this.groupBoxPerformance.Controls.Add(this.label10);
      this.groupBoxPerformance.Controls.Add(this.label12);
      this.groupBoxPerformance.Controls.Add(this.textBoxExecutionTimeTotal);
      this.groupBoxPerformance.Controls.Add(this.label7);
      this.groupBoxPerformance.Controls.Add(this.label11);
      this.groupBoxPerformance.Controls.Add(this.label6);
      this.groupBoxPerformance.Controls.Add(this.textBoxEL);
      this.groupBoxPerformance.Enabled = false;
      this.groupBoxPerformance.Location = new System.Drawing.Point(9, 387);
      this.groupBoxPerformance.Margin = new System.Windows.Forms.Padding(4);
      this.groupBoxPerformance.Name = "groupBoxPerformance";
      this.groupBoxPerformance.Padding = new System.Windows.Forms.Padding(4);
      this.groupBoxPerformance.Size = new System.Drawing.Size(821, 110);
      this.groupBoxPerformance.TabIndex = 20;
      this.groupBoxPerformance.TabStop = false;
      this.groupBoxPerformance.Text = "Performance";
      // 
      // labelLastSearchOn
      // 
      this.labelLastSearchOn.AutoSize = true;
      this.labelLastSearchOn.Location = new System.Drawing.Point(233, 84);
      this.labelLastSearchOn.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.labelLastSearchOn.Name = "labelLastSearchOn";
      this.labelLastSearchOn.Size = new System.Drawing.Size(0, 17);
      this.labelLastSearchOn.TabIndex = 54;
      // 
      // textBoxSearchCount
      // 
      this.textBoxSearchCount.Location = new System.Drawing.Point(111, 81);
      this.textBoxSearchCount.Margin = new System.Windows.Forms.Padding(4);
      this.textBoxSearchCount.Name = "textBoxSearchCount";
      this.textBoxSearchCount.ReadOnly = true;
      this.textBoxSearchCount.Size = new System.Drawing.Size(113, 23);
      this.textBoxSearchCount.TabIndex = 53;
      this.textBoxSearchCount.Text = "0";
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Location = new System.Drawing.Point(9, 84);
      this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(94, 17);
      this.label9.TabIndex = 52;
      this.label9.Text = "Search Count";
      // 
      // label21
      // 
      this.label21.AutoSize = true;
      this.label21.Location = new System.Drawing.Point(392, 52);
      this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label21.Name = "label21";
      this.label21.Size = new System.Drawing.Size(50, 17);
      this.label21.TabIndex = 40;
      this.label21.Text = "Avg Br";
      // 
      // label45
      // 
      this.label45.AutoSize = true;
      this.label45.Location = new System.Drawing.Point(600, 52);
      this.label45.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label45.Name = "label45";
      this.label45.Size = new System.Drawing.Size(12, 17);
      this.label45.TabIndex = 50;
      this.label45.Text = "/";
      // 
      // textBoxAvgBranching
      // 
      this.textBoxAvgBranching.Location = new System.Drawing.Point(444, 49);
      this.textBoxAvgBranching.Margin = new System.Windows.Forms.Padding(4);
      this.textBoxAvgBranching.Name = "textBoxAvgBranching";
      this.textBoxAvgBranching.ReadOnly = true;
      this.textBoxAvgBranching.Size = new System.Drawing.Size(52, 23);
      this.textBoxAvgBranching.TabIndex = 39;
      // 
      // label40
      // 
      this.label40.AutoSize = true;
      this.label40.Location = new System.Drawing.Point(787, 52);
      this.label40.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label40.Name = "label40";
      this.label40.Size = new System.Drawing.Size(20, 17);
      this.label40.TabIndex = 51;
      this.label40.Text = "%";
      // 
      // textBoxMOActsNo
      // 
      this.textBoxMOActsNo.Location = new System.Drawing.Point(338, 49);
      this.textBoxMOActsNo.Margin = new System.Windows.Forms.Padding(4);
      this.textBoxMOActsNo.Name = "textBoxMOActsNo";
      this.textBoxMOActsNo.ReadOnly = true;
      this.textBoxMOActsNo.Size = new System.Drawing.Size(52, 23);
      this.textBoxMOActsNo.TabIndex = 38;
      // 
      // label20
      // 
      this.label20.AutoSize = true;
      this.label20.Location = new System.Drawing.Point(301, 52);
      this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label20.Name = "label20";
      this.label20.Size = new System.Drawing.Size(35, 17);
      this.label20.TabIndex = 37;
      this.label20.Text = "Acts";
      // 
      // textBoxLN
      // 
      this.textBoxLN.Location = new System.Drawing.Point(463, 18);
      this.textBoxLN.Margin = new System.Windows.Forms.Padding(4);
      this.textBoxLN.Name = "textBoxLN";
      this.textBoxLN.ReadOnly = true;
      this.textBoxLN.Size = new System.Drawing.Size(78, 23);
      this.textBoxLN.TabIndex = 43;
      // 
      // textBoxUsedCPU
      // 
      this.textBoxUsedCPU.Location = new System.Drawing.Point(743, 49);
      this.textBoxUsedCPU.Margin = new System.Windows.Forms.Padding(4);
      this.textBoxUsedCPU.Name = "textBoxUsedCPU";
      this.textBoxUsedCPU.ReadOnly = true;
      this.textBoxUsedCPU.Size = new System.Drawing.Size(42, 23);
      this.textBoxUsedCPU.TabIndex = 50;
      // 
      // textBoxUsedRAM
      // 
      this.textBoxUsedRAM.Location = new System.Drawing.Point(540, 49);
      this.textBoxUsedRAM.Margin = new System.Windows.Forms.Padding(4);
      this.textBoxUsedRAM.Name = "textBoxUsedRAM";
      this.textBoxUsedRAM.ReadOnly = true;
      this.textBoxUsedRAM.Size = new System.Drawing.Size(58, 23);
      this.textBoxUsedRAM.TabIndex = 46;
      // 
      // label30
      // 
      this.label30.AutoSize = true;
      this.label30.Location = new System.Drawing.Point(145, 52);
      this.label30.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label30.Name = "label30";
      this.label30.Size = new System.Drawing.Size(26, 17);
      this.label30.TabIndex = 46;
      this.label30.Text = "ms";
      // 
      // label39
      // 
      this.label39.AutoSize = true;
      this.label39.Location = new System.Drawing.Point(705, 52);
      this.label39.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label39.Name = "label39";
      this.label39.Size = new System.Drawing.Size(36, 17);
      this.label39.TabIndex = 46;
      this.label39.Text = "CPU";
      // 
      // label32
      // 
      this.label32.AutoSize = true;
      this.label32.Location = new System.Drawing.Point(434, 21);
      this.label32.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label32.Name = "label32";
      this.label32.Size = new System.Drawing.Size(26, 17);
      this.label32.TabIndex = 41;
      this.label32.Text = "LN";
      // 
      // label38
      // 
      this.label38.AutoSize = true;
      this.label38.Location = new System.Drawing.Point(674, 52);
      this.label38.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label38.Name = "label38";
      this.label38.Size = new System.Drawing.Size(28, 17);
      this.label38.TabIndex = 48;
      this.label38.Text = "MB";
      // 
      // label29
      // 
      this.label29.AutoSize = true;
      this.label29.Location = new System.Drawing.Point(48, 52);
      this.label29.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label29.Name = "label29";
      this.label29.Size = new System.Drawing.Size(15, 17);
      this.label29.TabIndex = 45;
      this.label29.Text = "It";
      // 
      // label36
      // 
      this.label36.AutoSize = true;
      this.label36.Location = new System.Drawing.Point(500, 52);
      this.label36.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label36.Name = "label36";
      this.label36.Size = new System.Drawing.Size(38, 17);
      this.label36.TabIndex = 48;
      this.label36.Text = "RAM";
      // 
      // textBoxAvailableRAM
      // 
      this.textBoxAvailableRAM.Location = new System.Drawing.Point(614, 49);
      this.textBoxAvailableRAM.Margin = new System.Windows.Forms.Padding(4);
      this.textBoxAvailableRAM.Name = "textBoxAvailableRAM";
      this.textBoxAvailableRAM.ReadOnly = true;
      this.textBoxAvailableRAM.Size = new System.Drawing.Size(58, 23);
      this.textBoxAvailableRAM.TabIndex = 49;
      // 
      // label26
      // 
      this.label26.AutoSize = true;
      this.label26.Location = new System.Drawing.Point(173, 52);
      this.label26.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label26.Name = "label26";
      this.label26.Size = new System.Drawing.Size(17, 17);
      this.label26.TabIndex = 44;
      this.label26.Text = "T";
      // 
      // textBoxExecutionTimeCurrentIteration
      // 
      this.textBoxExecutionTimeCurrentIteration.Location = new System.Drawing.Point(65, 49);
      this.textBoxExecutionTimeCurrentIteration.Margin = new System.Windows.Forms.Padding(4);
      this.textBoxExecutionTimeCurrentIteration.Name = "textBoxExecutionTimeCurrentIteration";
      this.textBoxExecutionTimeCurrentIteration.ReadOnly = true;
      this.textBoxExecutionTimeCurrentIteration.Size = new System.Drawing.Size(78, 23);
      this.textBoxExecutionTimeCurrentIteration.TabIndex = 43;
      // 
      // label23
      // 
      this.label23.AutoSize = true;
      this.label23.Location = new System.Drawing.Point(754, 21);
      this.label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label23.Name = "label23";
      this.label23.Size = new System.Drawing.Size(59, 17);
      this.label23.TabIndex = 42;
      this.label23.Text = "L/s | N/s";
      // 
      // textBoxSpeed
      // 
      this.textBoxSpeed.Location = new System.Drawing.Point(606, 18);
      this.textBoxSpeed.Margin = new System.Windows.Forms.Padding(4);
      this.textBoxSpeed.Name = "textBoxSpeed";
      this.textBoxSpeed.ReadOnly = true;
      this.textBoxSpeed.Size = new System.Drawing.Size(140, 23);
      this.textBoxSpeed.TabIndex = 41;
      // 
      // label22
      // 
      this.label22.AutoSize = true;
      this.label22.Location = new System.Drawing.Point(549, 21);
      this.label22.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label22.Name = "label22";
      this.label22.Size = new System.Drawing.Size(49, 17);
      this.label22.TabIndex = 40;
      this.label22.Text = "Speed";
      // 
      // textBoxAN
      // 
      this.textBoxAN.Location = new System.Drawing.Point(336, 18);
      this.textBoxAN.Margin = new System.Windows.Forms.Padding(4);
      this.textBoxAN.Name = "textBoxAN";
      this.textBoxAN.ReadOnly = true;
      this.textBoxAN.Size = new System.Drawing.Size(89, 23);
      this.textBoxAN.TabIndex = 39;
      // 
      // label16
      // 
      this.label16.AutoSize = true;
      this.label16.Location = new System.Drawing.Point(307, 21);
      this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label16.Name = "label16";
      this.label16.Size = new System.Drawing.Size(27, 17);
      this.label16.TabIndex = 38;
      this.label16.Text = "AN";
      // 
      // textBoxEN
      // 
      this.textBoxEN.Location = new System.Drawing.Point(207, 18);
      this.textBoxEN.Margin = new System.Windows.Forms.Padding(4);
      this.textBoxEN.Name = "textBoxEN";
      this.textBoxEN.ReadOnly = true;
      this.textBoxEN.Size = new System.Drawing.Size(89, 23);
      this.textBoxEN.TabIndex = 38;
      // 
      // label10
      // 
      this.label10.AutoSize = true;
      this.label10.Location = new System.Drawing.Point(272, 52);
      this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(26, 17);
      this.label10.TabIndex = 29;
      this.label10.Text = "ms";
      // 
      // label12
      // 
      this.label12.AutoSize = true;
      this.label12.Location = new System.Drawing.Point(178, 21);
      this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label12.Name = "label12";
      this.label12.Size = new System.Drawing.Size(27, 17);
      this.label12.TabIndex = 37;
      this.label12.Text = "EN";
      // 
      // label11
      // 
      this.label11.AutoSize = true;
      this.label11.Location = new System.Drawing.Point(53, 21);
      this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label11.Name = "label11";
      this.label11.Size = new System.Drawing.Size(25, 17);
      this.label11.TabIndex = 36;
      this.label11.Text = "EL";
      // 
      // buttonReadTable
      // 
      this.buttonReadTable.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.buttonReadTable.Location = new System.Drawing.Point(276, 15);
      this.buttonReadTable.Margin = new System.Windows.Forms.Padding(4);
      this.buttonReadTable.Name = "buttonReadTable";
      this.buttonReadTable.Size = new System.Drawing.Size(69, 28);
      this.buttonReadTable.TabIndex = 35;
      this.buttonReadTable.Text = "Read";
      this.buttonReadTable.UseVisualStyleBackColor = true;
      this.buttonReadTable.Click += new System.EventHandler(this.buttonReadTable_Click);
      // 
      // label35
      // 
      this.label35.AutoSize = true;
      this.label35.Location = new System.Drawing.Point(7, 20);
      this.label35.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label35.Name = "label35";
      this.label35.Size = new System.Drawing.Size(45, 17);
      this.label35.TabIndex = 14;
      this.label35.Text = "Name";
      // 
      // groupBoxAppSettings
      // 
      this.groupBoxAppSettings.Controls.Add(this.label42);
      this.groupBoxAppSettings.Controls.Add(this.labelCS);
      this.groupBoxAppSettings.Controls.Add(this.comboBoxEquipmentList);
      this.groupBoxAppSettings.Controls.Add(this.buttonSearch);
      this.groupBoxAppSettings.Controls.Add(this.pictureBoxCS);
      this.groupBoxAppSettings.Location = new System.Drawing.Point(9, 88);
      this.groupBoxAppSettings.Name = "groupBoxAppSettings";
      this.groupBoxAppSettings.Size = new System.Drawing.Size(349, 59);
      this.groupBoxAppSettings.TabIndex = 52;
      this.groupBoxAppSettings.TabStop = false;
      this.groupBoxAppSettings.Text = "Application: Equipment Status and Trigger";
      // 
      // label42
      // 
      this.label42.AutoSize = true;
      this.label42.Location = new System.Drawing.Point(7, 26);
      this.label42.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label42.Name = "label42";
      this.label42.Size = new System.Drawing.Size(21, 17);
      this.label42.TabIndex = 54;
      this.label42.Text = "ID";
      // 
      // labelCS
      // 
      this.labelCS.AutoSize = true;
      this.labelCS.Enabled = false;
      this.labelCS.Location = new System.Drawing.Point(229, 26);
      this.labelCS.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.labelCS.Name = "labelCS";
      this.labelCS.Size = new System.Drawing.Size(84, 17);
      this.labelCS.TabIndex = 51;
      this.labelCS.Text = "Uninitialized";
      // 
      // comboBoxEquipmentList
      // 
      this.comboBoxEquipmentList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxEquipmentList.FormattingEnabled = true;
      this.comboBoxEquipmentList.Location = new System.Drawing.Point(33, 23);
      this.comboBoxEquipmentList.Margin = new System.Windows.Forms.Padding(4);
      this.comboBoxEquipmentList.Name = "comboBoxEquipmentList";
      this.comboBoxEquipmentList.Size = new System.Drawing.Size(75, 24);
      this.comboBoxEquipmentList.TabIndex = 54;
      this.comboBoxEquipmentList.SelectedIndexChanged += new System.EventHandler(this.comboBoxEquipmentList_SelectedIndexChanged);
      // 
      // pictureBoxCS
      // 
      this.pictureBoxCS.BackColor = System.Drawing.Color.Transparent;
      this.pictureBoxCS.Enabled = false;
      this.pictureBoxCS.Location = new System.Drawing.Point(207, 26);
      this.pictureBoxCS.Name = "pictureBoxCS";
      this.pictureBoxCS.Size = new System.Drawing.Size(17, 17);
      this.pictureBoxCS.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
      this.pictureBoxCS.TabIndex = 50;
      this.pictureBoxCS.TabStop = false;
      // 
      // groupBoxTablesAndTestings
      // 
      this.groupBoxTablesAndTestings.Controls.Add(this.checkBoxTableViewAddIndex);
      this.groupBoxTablesAndTestings.Controls.Add(this.checkBoxShowTableView);
      this.groupBoxTablesAndTestings.Controls.Add(this.label43);
      this.groupBoxTablesAndTestings.Controls.Add(this.dateTimePickerFixed);
      this.groupBoxTablesAndTestings.Controls.Add(this.checkBoxFixedTime);
      this.groupBoxTablesAndTestings.Controls.Add(this.numericUpDownReadTableLimit);
      this.groupBoxTablesAndTestings.Controls.Add(this.comboBoxTableName);
      this.groupBoxTablesAndTestings.Controls.Add(this.buttonReadTable);
      this.groupBoxTablesAndTestings.Controls.Add(this.label35);
      this.groupBoxTablesAndTestings.Enabled = false;
      this.groupBoxTablesAndTestings.Location = new System.Drawing.Point(364, 31);
      this.groupBoxTablesAndTestings.Name = "groupBoxTablesAndTestings";
      this.groupBoxTablesAndTestings.Size = new System.Drawing.Size(353, 116);
      this.groupBoxTablesAndTestings.TabIndex = 54;
      this.groupBoxTablesAndTestings.TabStop = false;
      this.groupBoxTablesAndTestings.Text = "Tables && Testings";
      // 
      // checkBoxTableViewAddIndex
      // 
      this.checkBoxTableViewAddIndex.AutoSize = true;
      this.checkBoxTableViewAddIndex.Location = new System.Drawing.Point(285, 52);
      this.checkBoxTableViewAddIndex.Name = "checkBoxTableViewAddIndex";
      this.checkBoxTableViewAddIndex.Size = new System.Drawing.Size(60, 21);
      this.checkBoxTableViewAddIndex.TabIndex = 72;
      this.checkBoxTableViewAddIndex.Text = "Index";
      this.checkBoxTableViewAddIndex.UseVisualStyleBackColor = true;
      this.checkBoxTableViewAddIndex.CheckedChanged += new System.EventHandler(this.checkBoxTableViewAddIndex_CheckedChanged);
      // 
      // checkBoxShowTableView
      // 
      this.checkBoxShowTableView.AutoSize = true;
      this.checkBoxShowTableView.Location = new System.Drawing.Point(139, 52);
      this.checkBoxShowTableView.Name = "checkBoxShowTableView";
      this.checkBoxShowTableView.Size = new System.Drawing.Size(134, 21);
      this.checkBoxShowTableView.TabIndex = 71;
      this.checkBoxShowTableView.Text = "Show Table View";
      this.checkBoxShowTableView.UseVisualStyleBackColor = true;
      this.checkBoxShowTableView.CheckedChanged += new System.EventHandler(this.checkBoxShowTableView_CheckedChanged);
      // 
      // label43
      // 
      this.label43.AutoSize = true;
      this.label43.Location = new System.Drawing.Point(8, 52);
      this.label43.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label43.Name = "label43";
      this.label43.Size = new System.Drawing.Size(37, 17);
      this.label43.TabIndex = 70;
      this.label43.Text = "Limit";
      // 
      // dateTimePickerFixed
      // 
      this.dateTimePickerFixed.Enabled = false;
      this.dateTimePickerFixed.Location = new System.Drawing.Point(111, 84);
      this.dateTimePickerFixed.Name = "dateTimePickerFixed";
      this.dateTimePickerFixed.Size = new System.Drawing.Size(234, 23);
      this.dateTimePickerFixed.TabIndex = 69;
      this.dateTimePickerFixed.ValueChanged += new System.EventHandler(this.dateTimePickerFixed_ValueChanged);
      // 
      // checkBoxFixedTime
      // 
      this.checkBoxFixedTime.AutoSize = true;
      this.checkBoxFixedTime.Location = new System.Drawing.Point(10, 84);
      this.checkBoxFixedTime.Name = "checkBoxFixedTime";
      this.checkBoxFixedTime.Size = new System.Drawing.Size(95, 21);
      this.checkBoxFixedTime.TabIndex = 68;
      this.checkBoxFixedTime.Text = "Fixed Time";
      this.checkBoxFixedTime.UseVisualStyleBackColor = true;
      this.checkBoxFixedTime.CheckedChanged += new System.EventHandler(this.checkBoxFixedTime_CheckedChanged);
      // 
      // numericUpDownReadTableLimit
      // 
      this.numericUpDownReadTableLimit.Location = new System.Drawing.Point(59, 50);
      this.numericUpDownReadTableLimit.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      this.numericUpDownReadTableLimit.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.numericUpDownReadTableLimit.Name = "numericUpDownReadTableLimit";
      this.numericUpDownReadTableLimit.Size = new System.Drawing.Size(70, 23);
      this.numericUpDownReadTableLimit.TabIndex = 67;
      this.numericUpDownReadTableLimit.ThousandsSeparator = true;
      this.numericUpDownReadTableLimit.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
      this.numericUpDownReadTableLimit.ValueChanged += new System.EventHandler(this.numericUpDownReadTableLimit_ValueChanged);
      // 
      // comboBoxTableName
      // 
      this.comboBoxTableName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxTableName.FormattingEnabled = true;
      this.comboBoxTableName.Location = new System.Drawing.Point(59, 18);
      this.comboBoxTableName.Name = "comboBoxTableName";
      this.comboBoxTableName.Size = new System.Drawing.Size(210, 24);
      this.comboBoxTableName.TabIndex = 65;
      // 
      // textBoxNoOfClient
      // 
      this.textBoxNoOfClient.Location = new System.Drawing.Point(310, 22);
      this.textBoxNoOfClient.Name = "textBoxNoOfClient";
      this.textBoxNoOfClient.ReadOnly = true;
      this.textBoxNoOfClient.Size = new System.Drawing.Size(28, 23);
      this.textBoxNoOfClient.TabIndex = 35;
      this.textBoxNoOfClient.Text = "0";
      // 
      // groupBoxConnectionHBeat
      // 
      this.groupBoxConnectionHBeat.Controls.Add(this.label5);
      this.groupBoxConnectionHBeat.Controls.Add(this.label1);
      this.groupBoxConnectionHBeat.Controls.Add(this.labelDBConnStat);
      this.groupBoxConnectionHBeat.Controls.Add(this.textBoxNoOfClient);
      this.groupBoxConnectionHBeat.Controls.Add(this.pictureBoxTcpConnection);
      this.groupBoxConnectionHBeat.Controls.Add(this.pictureBoxConnection);
      this.groupBoxConnectionHBeat.Location = new System.Drawing.Point(9, 31);
      this.groupBoxConnectionHBeat.Name = "groupBoxConnectionHBeat";
      this.groupBoxConnectionHBeat.Size = new System.Drawing.Size(349, 56);
      this.groupBoxConnectionHBeat.TabIndex = 50;
      this.groupBoxConnectionHBeat.TabStop = false;
      this.groupBoxConnectionHBeat.Text = "Connection && Heart Beat";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(204, 25);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(76, 17);
      this.label5.TabIndex = 59;
      this.label5.Text = "Heart Beat";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(8, 24);
      this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(48, 17);
      this.label1.TabIndex = 58;
      this.label1.Text = "Status";
      // 
      // pictureBoxTcpConnection
      // 
      this.pictureBoxTcpConnection.BackColor = System.Drawing.Color.Transparent;
      this.pictureBoxTcpConnection.Location = new System.Drawing.Point(288, 25);
      this.pictureBoxTcpConnection.Name = "pictureBoxTcpConnection";
      this.pictureBoxTcpConnection.Size = new System.Drawing.Size(17, 17);
      this.pictureBoxTcpConnection.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
      this.pictureBoxTcpConnection.TabIndex = 35;
      this.pictureBoxTcpConnection.TabStop = false;
      // 
      // pictureBoxConnection
      // 
      this.pictureBoxConnection.BackColor = System.Drawing.Color.Transparent;
      this.pictureBoxConnection.Location = new System.Drawing.Point(61, 25);
      this.pictureBoxConnection.Name = "pictureBoxConnection";
      this.pictureBoxConnection.Size = new System.Drawing.Size(17, 17);
      this.pictureBoxConnection.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
      this.pictureBoxConnection.TabIndex = 24;
      this.pictureBoxConnection.TabStop = false;
      // 
      // menuStripMain
      // 
      this.menuStripMain.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
      this.menuStripMain.Font = new System.Drawing.Font("Segoe UI", 10F);
      this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectionToolStripMenuItem,
            this.settingsToolStripMenuItem});
      this.menuStripMain.Location = new System.Drawing.Point(0, 0);
      this.menuStripMain.Name = "menuStripMain";
      this.menuStripMain.Size = new System.Drawing.Size(839, 27);
      this.menuStripMain.TabIndex = 57;
      this.menuStripMain.Text = "menuStripMain";
      // 
      // connectionToolStripMenuItem
      // 
      this.connectionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToolStripMenuItem,
            this.disconnectToolStripMenuItem,
            this.infoToolStripMenuItem});
      this.connectionToolStripMenuItem.Name = "connectionToolStripMenuItem";
      this.connectionToolStripMenuItem.Size = new System.Drawing.Size(91, 23);
      this.connectionToolStripMenuItem.Text = "Connection";
      // 
      // connectToolStripMenuItem
      // 
      this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
      this.connectToolStripMenuItem.Size = new System.Drawing.Size(145, 24);
      this.connectToolStripMenuItem.Text = "Connect";
      this.connectToolStripMenuItem.Click += new System.EventHandler(this.connectToolStripMenuItem_Click);
      // 
      // disconnectToolStripMenuItem
      // 
      this.disconnectToolStripMenuItem.Enabled = false;
      this.disconnectToolStripMenuItem.Name = "disconnectToolStripMenuItem";
      this.disconnectToolStripMenuItem.Size = new System.Drawing.Size(145, 24);
      this.disconnectToolStripMenuItem.Text = "Disconnect";
      this.disconnectToolStripMenuItem.Click += new System.EventHandler(this.disconnectToolStripMenuItem_Click);
      // 
      // infoToolStripMenuItem
      // 
      this.infoToolStripMenuItem.Name = "infoToolStripMenuItem";
      this.infoToolStripMenuItem.Size = new System.Drawing.Size(145, 24);
      this.infoToolStripMenuItem.Text = "Info";
      this.infoToolStripMenuItem.Click += new System.EventHandler(this.infoToolStripMenuItem_Click);
      // 
      // settingsToolStripMenuItem
      // 
      this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.applicationToolStripMenuItem,
            this.equipmentToolStripMenuItem,
            this.tableToolStripMenuItem,
            this.watcherToolStripMenuItem,
            this.globalTimingToolStripMenuItem});
      this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
      this.settingsToolStripMenuItem.Size = new System.Drawing.Size(70, 23);
      this.settingsToolStripMenuItem.Text = "Settings";
      // 
      // applicationToolStripMenuItem
      // 
      this.applicationToolStripMenuItem.Name = "applicationToolStripMenuItem";
      this.applicationToolStripMenuItem.Size = new System.Drawing.Size(162, 24);
      this.applicationToolStripMenuItem.Text = "Application";
      this.applicationToolStripMenuItem.Click += new System.EventHandler(this.applicationToolStripMenuItem_Click);
      // 
      // equipmentToolStripMenuItem
      // 
      this.equipmentToolStripMenuItem.Name = "equipmentToolStripMenuItem";
      this.equipmentToolStripMenuItem.Size = new System.Drawing.Size(162, 24);
      this.equipmentToolStripMenuItem.Text = "Equipment";
      // 
      // tableToolStripMenuItem
      // 
      this.tableToolStripMenuItem.Name = "tableToolStripMenuItem";
      this.tableToolStripMenuItem.Size = new System.Drawing.Size(162, 24);
      this.tableToolStripMenuItem.Text = "Table";
      this.tableToolStripMenuItem.Click += new System.EventHandler(this.tableToolStripMenuItem_Click);
      // 
      // watcherToolStripMenuItem
      // 
      this.watcherToolStripMenuItem.Name = "watcherToolStripMenuItem";
      this.watcherToolStripMenuItem.Size = new System.Drawing.Size(162, 24);
      this.watcherToolStripMenuItem.Text = "Watcher";
      this.watcherToolStripMenuItem.Click += new System.EventHandler(this.watcherToolStripMenuItem_Click);
      // 
      // globalTimingToolStripMenuItem
      // 
      this.globalTimingToolStripMenuItem.Name = "globalTimingToolStripMenuItem";
      this.globalTimingToolStripMenuItem.Size = new System.Drawing.Size(162, 24);
      this.globalTimingToolStripMenuItem.Text = "Global Timing";
      this.globalTimingToolStripMenuItem.Click += new System.EventHandler(this.globalTimingToolStripMenuItem_Click);
      // 
      // labelSessionTime
      // 
      this.labelSessionTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.labelSessionTime.AutoSize = true;
      this.labelSessionTime.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
      this.labelSessionTime.Location = new System.Drawing.Point(522, 6);
      this.labelSessionTime.Name = "labelSessionTime";
      this.labelSessionTime.Size = new System.Drawing.Size(193, 17);
      this.labelSessionTime.TabIndex = 58;
      this.labelSessionTime.Text = "Session Time: 0000 00:00:00";
      this.labelSessionTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // labelMode
      // 
      this.labelMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.labelMode.AutoSize = true;
      this.labelMode.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
      this.labelMode.Location = new System.Drawing.Point(721, 6);
      this.labelMode.Name = "labelMode";
      this.labelMode.Size = new System.Drawing.Size(111, 17);
      this.labelMode.TabIndex = 59;
      this.labelMode.Text = "Mode: SemiAuto";
      this.labelMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // pictureBoxLogo
      // 
      this.pictureBoxLogo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.pictureBoxLogo.Image = global::T5ShortestTime.Properties.Resources.AstrioSymbolPlusWords;
      this.pictureBoxLogo.InitialImage = global::T5ShortestTime.Properties.Resources.AstrioSymbolPlusWords;
      this.pictureBoxLogo.Location = new System.Drawing.Point(724, 39);
      this.pictureBoxLogo.Margin = new System.Windows.Forms.Padding(4);
      this.pictureBoxLogo.Name = "pictureBoxLogo";
      this.pictureBoxLogo.Size = new System.Drawing.Size(106, 108);
      this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxLogo.TabIndex = 20;
      this.pictureBoxLogo.TabStop = false;
      // 
      // T5ShortestTimeForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(839, 501);
      this.Controls.Add(this.labelMode);
      this.Controls.Add(this.labelSessionTime);
      this.Controls.Add(this.groupBoxConnectionHBeat);
      this.Controls.Add(this.groupBoxTablesAndTestings);
      this.Controls.Add(this.groupBoxAppSettings);
      this.Controls.Add(this.pictureBoxLogo);
      this.Controls.Add(this.groupBoxPerformance);
      this.Controls.Add(this.groupBoxSolution);
      this.Controls.Add(this.menuStripMain);
      this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MainMenuStrip = this.menuStripMain;
      this.Margin = new System.Windows.Forms.Padding(4);
      this.MaximumSize = new System.Drawing.Size(855, 540);
      this.MinimumSize = new System.Drawing.Size(855, 540);
      this.Name = "T5ShortestTimeForm";
      this.Text = "SATS T5 Shortest Time Finder";
      this.groupBoxSolution.ResumeLayout(false);
      this.groupBoxSolution.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSolutionCompleted)).EndInit();
      this.groupBoxPerformance.ResumeLayout(false);
      this.groupBoxPerformance.PerformLayout();
      this.groupBoxAppSettings.ResumeLayout(false);
      this.groupBoxAppSettings.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCS)).EndInit();
      this.groupBoxTablesAndTestings.ResumeLayout(false);
      this.groupBoxTablesAndTestings.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownReadTableLimit)).EndInit();
      this.groupBoxConnectionHBeat.ResumeLayout(false);
      this.groupBoxConnectionHBeat.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTcpConnection)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxConnection)).EndInit();
      this.menuStripMain.ResumeLayout(false);
      this.menuStripMain.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

		private System.Windows.Forms.Label label7;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.TextBox textBoxEL;
    private System.Windows.Forms.TextBox textBoxExecutionTimeTotal;
		private System.Windows.Forms.GroupBox groupBoxSolution;
		private System.Windows.Forms.Label label14;
    private System.Windows.Forms.PictureBox pictureBoxLogo;
    private System.Windows.Forms.Label label17;
		private System.Windows.Forms.TextBox textBoxSolutionShortestTime;
		private System.Windows.Forms.GroupBox groupBoxPerformance;
    private System.Windows.Forms.PictureBox pictureBoxConnection;
    private System.Windows.Forms.Label labelDBConnStat;
		private System.Windows.Forms.Button buttonSearch;
		private System.Windows.Forms.Label label35;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.RichTextBox richTextBoxBestRoute;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.Label label12;
    private System.Windows.Forms.Label label16;
    private System.Windows.Forms.TextBox textBoxAN;
		private System.Windows.Forms.TextBox textBoxEN;
    private System.Windows.Forms.Button buttonReadTable;
    private System.Windows.Forms.Label label20;
    private System.Windows.Forms.TextBox textBoxMOActsNo;
    private System.Windows.Forms.Label label21;
		private System.Windows.Forms.TextBox textBoxAvgBranching;
    private System.Windows.Forms.Label label22;
    private System.Windows.Forms.Label label23;
    private System.Windows.Forms.TextBox textBoxSpeed;
    private System.Windows.Forms.Label label24;
    private System.Windows.Forms.TextBox textBoxSessionDepth;
    private System.Windows.Forms.Label labelSolutionCompleted;
    private System.Windows.Forms.PictureBox pictureBoxSolutionCompleted;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox textBoxIteration;
    private System.Windows.Forms.TextBox textBoxIterationNodesEvaluated;
		private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox textBoxExecutionTimeCurrentIteration;
    private System.Windows.Forms.Label label26;
    private System.Windows.Forms.TextBox textBoxIterationNodes;
    private System.Windows.Forms.Label label30;
    private System.Windows.Forms.Label label29;
    private System.Windows.Forms.Label label31;
		private System.Windows.Forms.TextBox textBoxIterationDepth;
		private System.Windows.Forms.Label label27;
    private System.Windows.Forms.TextBox textBoxLN;
    private System.Windows.Forms.Label label32;
    private System.Windows.Forms.Label label36;
    private System.Windows.Forms.TextBox textBoxUsedRAM;
    private System.Windows.Forms.TextBox textBoxAvailableRAM;
    private System.Windows.Forms.Label label38;
    private System.Windows.Forms.Label label39;
    private System.Windows.Forms.Label label40;
		private System.Windows.Forms.TextBox textBoxUsedCPU;
    private System.Windows.Forms.Label label28;
    private System.Windows.Forms.TextBox textBoxRouteSolution;
    private System.Windows.Forms.GroupBox groupBoxAppSettings;
    private System.Windows.Forms.ComboBox comboBoxEquipmentList;
    private System.Windows.Forms.Label label42;
		private System.Windows.Forms.GroupBox groupBoxTablesAndTestings;
    private System.Windows.Forms.Label label45;
    private System.Windows.Forms.Label labelCS;
    private System.Windows.Forms.PictureBox pictureBoxCS;
    private System.Windows.Forms.PictureBox pictureBoxTcpConnection;
    private System.Windows.Forms.TextBox textBoxNoOfClient;
    private System.Windows.Forms.GroupBox groupBoxConnectionHBeat;
    private System.Windows.Forms.ComboBox comboBoxTableName;
    private System.Windows.Forms.NumericUpDown numericUpDownReadTableLimit;
    private System.Windows.Forms.CheckBox checkBoxFixedTime;
    private System.Windows.Forms.DateTimePicker dateTimePickerFixed;
    private System.Windows.Forms.Label label43;
		private System.Windows.Forms.CheckBox checkBoxShowTableView;
		private System.Windows.Forms.CheckBox checkBoxTableViewAddIndex;
		private System.Windows.Forms.MenuStrip menuStripMain;
		private System.Windows.Forms.ToolStripMenuItem connectionToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem applicationToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem equipmentToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem connectToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem disconnectToolStripMenuItem;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textBoxInitialState;
		private System.Windows.Forms.ToolStripMenuItem infoToolStripMenuItem;
		private System.Windows.Forms.Label labelSessionTime;
		private System.Windows.Forms.Label labelMode;
		private System.Windows.Forms.ToolStripMenuItem tableToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem watcherToolStripMenuItem;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ToolStripMenuItem globalTimingToolStripMenuItem;
		private System.Windows.Forms.Label labelLastSearchOn;
		private System.Windows.Forms.TextBox textBoxSearchCount;
		private System.Windows.Forms.Label label9;
	}
}

