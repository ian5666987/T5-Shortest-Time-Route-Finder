using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using T5ShortestTime.MOAct;
using T5ShortestTime.Models;
using T5ShortestTime.SATS;

using Extension.Database.OldOracle;
using Extension.Socket;

namespace T5ShortestTime {
	public class T5ShortestTimeLogic {
		//Typical initialization
		private string root;

		//Database
		private T5OracleHandler t5OracleHandler = new T5OracleHandler();
		public T5OracleHandler DbHandler { get { return t5OracleHandler; } }

		//TCP/IP Server
		public ServerHandler ServerHandler { get; private set; } = new ServerHandler();

		//Directories and files
		private string recordFoldername = "records";
		private string configFoldername = "configs";
		private string equipmentFoldername = "equipments";
		private string dbConnectionFilename = "dbconnection";
		private string appSettingsFilename = "appsettings";
		private string tableAndTestFilename = "tableandtest";
		private string wsFilename = "workstation";
		private string tcpConnectionFilename = "tcpserverconnection";
		private string globalActTimeFilename = "globalacttime";

    //Directories and files for simulation purpose
    private string simulationFoldername = "simulation";
    private string eqpInfoFilename = "eqpinfo";
    private string ledInfoFilename = "ledinfo";
    private string moInfoViewFilename = "moinfoview";
    private string optFlagFilename = "optflag";
    private string optSolutionFilename = "optsolution";

    //Directories and files (absolute) v2.8.4.3 and above, to prevent wrong directory when run by different application
    private string recordFolderpath;
		private string configFolderpath;

		//Equipment settings filenames
		private string actTimeFilename = "acttime";
		private string defaultInitialStateFilename = "defaultinit";
		private string recordLogFilename = "recordlog";
		private string routeFinderFilename = "routefinder";

		//Writing and Serializing
		FileStream filestream;
		XmlSerializer serializer;
		string filepath;
		TextWriter configWriteFileStream;

		//Diagnostics: http://stackoverflow.com/questions/278071/how-to-get-the-cpu-usage-in-c
		private PerformanceCounter cpuCounter = new PerformanceCounter();
		public float NextCPUValue { get { return cpuCounter.NextValue(); } }
		private PerformanceCounter ramCounter = new PerformanceCounter();
		public float NextRAMValue { get { return ramCounter.NextValue(); } }

		//Connections
		public OracleConnectionSettings DbConnectionSettings { get; private set; } = new OracleConnectionSettings();
		private string password = ""; //always initialize password as empty

		//Multiple equipments
		public T5STAppSettings AppSettings { get; private set; } = new T5STAppSettings(); //the default value should be "safe" enough		
		private List<Equipment> satsEquipments = new List<Equipment>();
		private List<Equipment> pendingList = new List<Equipment>();

		//UI variables
		public string CurrentUIEqpId { get; set; } = string.Empty;
		private bool initStateUpd = false;
		public bool HasInitStateUpdate { get { bool value = initStateUpd; initStateUpd = false; return value; } }

		public bool HasCmdStatUpdate { get; private set; }
		private CmdStat cmdStateUpd = CmdStat.Uninitialized;
		public CmdStat CmdStatUpdate { get { HasCmdStatUpdate = false; return cmdStateUpd; } }

		private bool indctrUpd = false;
		public bool HasIndicatorUpdate { get { bool value = indctrUpd; indctrUpd = false; return value; } }

		//Error Queues
		private Queue<string> queueTrace = new Queue<string>();
		public bool HasError { get { return queueTrace.Count > 0; } }

		public T5ShortestTimeLogic(string startUpPath) {
			//Directories			
			root = startUpPath;
			configFolderpath = Path.Combine(root, configFoldername);
			recordFolderpath = Path.Combine(root, recordFoldername);
			if (!Directory.Exists(recordFolderpath))
				Directory.CreateDirectory(recordFolderpath);
			if (!Directory.Exists(configFolderpath))
				Directory.CreateDirectory(configFolderpath); //configuraton directory...

			//Specific initialization
			Equipment.StartUpDateTime = DateTime.Now;
			Equipment.RecordFolder = recordFolderpath;
			Equipment.EquipmentFolderName = equipmentFoldername;
		}

		private void addError(string eStr) {
			queueTrace.Enqueue(eStr);
		}

		private List<string> getErrors() {
			List<string> errors = queueTrace.ToList();
			queueTrace.Clear();
			return errors;
		}

		public string GetError() {
			return string.Join(Environment.NewLine, getErrors());
		}

    private void initSimulation() {
      PH.IsSimulation = AppSettings.SimulationMode;
      string folderpath = Path.Combine(root, simulationFoldername);
      if (!Directory.Exists(folderpath))
        Directory.CreateDirectory(folderpath); //simulation directory...
      PH.EqpInfoSimulationFilepath = Path.Combine(folderpath, eqpInfoFilename + ".xml");
      PH.LedInfoSimulationFilepath = Path.Combine(folderpath, ledInfoFilename + ".xml");
      PH.MoInfoViewSimulationFilepath = Path.Combine(folderpath, moInfoViewFilename + ".xml");
      PH.OptFlagSimulationFilepath = Path.Combine(folderpath, optFlagFilename + ".xml");
      PH.OptSolutionBaseSimulationFilepath = Path.Combine(folderpath, optSolutionFilename + ".xml");
    }

    public void InitApp() {
			try {
        //getSimulationTestXml(); 
				filestream = new FileStream(Path.Combine(configFolderpath, appSettingsFilename + ".xml"), FileMode.Open, FileAccess.Read, FileShare.Read);
				serializer = new XmlSerializer(typeof(T5STAppSettings));
				AppSettings = (T5STAppSettings)serializer.Deserialize(filestream);
        filestream.Close();

        if (AppSettings.SimulationMode) //simulation mode is special
          initSimulation();
        else { //only creates real database connection when it is not in the simulation mode
          filestream = new FileStream(Path.Combine(configFolderpath, dbConnectionFilename + ".xml"), FileMode.Open, FileAccess.Read, FileShare.Read);
          serializer = new XmlSerializer(typeof(OracleConnectionSettings));
          DbConnectionSettings = (OracleConnectionSettings)serializer.Deserialize(filestream);
          filestream.Close();
          password = DbConnectionSettings.Password; //update with whatever field is in the password
        }

        filestream = new FileStream(Path.Combine(configFolderpath, tableAndTestFilename + ".xml"), FileMode.Open, FileAccess.Read, FileShare.Read);
				serializer = new XmlSerializer(typeof(T5TableAndTest));
				T5TableAndTest tableNamesSettings = (T5TableAndTest)serializer.Deserialize(filestream);
				tableNamesSettings.AfterDeserialization(); //must be called after deserialization for this type        
				filestream.Close();
				t5OracleHandler.TableAndTest = tableNamesSettings; //when loading the first time, TableAndTest is updated.
				filestream = new FileStream(Path.Combine(configFolderpath, globalActTimeFilename + ".xml"), FileMode.Open, FileAccess.Read, FileShare.Read);
				serializer = new XmlSerializer(typeof(GlobalActTimeSettings));
				ActHandler.GlobalActTimeSettings = (GlobalActTimeSettings)serializer.Deserialize(filestream);
        //ActHandler.GlobalActTimeSettings.AliasActTimePairs = new AliasActTimePair[] {
        //  new AliasActTimePair()
        //};
        ActHandler.GlobalActTimeSettings.AfterDeserialization();
				filestream.Close();
			} catch (Exception e) {
				if (filestream != null)
					filestream.Close();
				AppSettings = new T5STAppSettings();
				addError("Unable to load default shared settings config file(s)! " + e.ToString());
			}
		}

		public void InitWorkstation() {
			try { //Initialize workstation (may fail, that's ok)
				filestream = new FileStream(Path.Combine(configFolderpath, wsFilename + ".xml"), FileMode.Open, FileAccess.Read, FileShare.Read);
				serializer = new XmlSerializer(typeof(WorkstationJoin));
				WorkstationJoin wsSettings = (WorkstationJoin)serializer.Deserialize(filestream);
				filestream.Close();
				Workstation.AddWsPuDo(wsSettings);
			} catch (Exception e) {
				if (filestream != null)
					filestream.Close();
				addError("Unable to load join PU-DO in workstation info! " + e.ToString());
			}
		}

		public void InitNetwork(bool manualOpen = false) {
			try {
				ServerHandler.SetSettingsFromXMLFileSecure(configFolderpath, tcpConnectionFilename, manualOpen);
			} catch (Exception e) {
				addError("Error in network initialization! " + e.ToString());
			}
		}

		public void InitEquipments() {
			string folderpath = Path.Combine(configFolderpath, equipmentFoldername);
			if (!Directory.Exists(folderpath))
				Directory.CreateDirectory(folderpath); //equipments directory...

			//Only can initialize others if the app settings are successful (TODO may not be the best criterion)
			for (int i = 0; i < AppSettings.EquipmentIDList.Count; ++i) { //for each equipment ID, load the file..
				string equipmentId = AppSettings.EquipmentIDList[i];
				string equipmentFolderPath = Path.Combine(folderpath, equipmentId);
				satsEquipments.Add(new Equipment());
				satsEquipments[i].ID = equipmentId;
				try {
					filestream = new FileStream(Path.Combine(equipmentFolderPath, routeFinderFilename + ".xml"), FileMode.Open, FileAccess.Read, FileShare.Read);
					serializer = new XmlSerializer(typeof(RouteFinderSettings));
					satsEquipments[i].RouteFinderSettings = (RouteFinderSettings)serializer.Deserialize(filestream);
					filestream.Close();
					filestream = new FileStream(Path.Combine(equipmentFolderPath, recordLogFilename + ".xml"), FileMode.Open, FileAccess.Read, FileShare.Read);
					serializer = new XmlSerializer(typeof(RecordLogSettings));
					satsEquipments[i].RecordLogSettings = (RecordLogSettings)serializer.Deserialize(filestream);
					if (!AppSettings.ShowLogBox) //TODO look not so nice, but leave it for now...
						satsEquipments[i].RecordLogSettings.ShowLog = false; //forced false
					filestream.Close();
					filestream = new FileStream(Path.Combine(equipmentFolderPath, actTimeFilename + ".xml"), FileMode.Open, FileAccess.Read, FileShare.Read);
					serializer = new XmlSerializer(typeof(TimeSettings));
					satsEquipments[i].Handler.TimeSettings = (TimeSettings)serializer.Deserialize(filestream);
					filestream.Close();
					filestream = new FileStream(Path.Combine(equipmentFolderPath, defaultInitialStateFilename + ".xml"), FileMode.Open, FileAccess.Read, FileShare.Read);
					serializer = new XmlSerializer(typeof(ActInitialState));
					ActInitialState initState = (ActInitialState)serializer.Deserialize(filestream);
					if (initState != null) //protect against null initial state, but why?
						satsEquipments[i].InitialState = initState;
					filestream.Close();
				} catch (Exception e) {
					if (filestream != null)
						filestream.Close();
					addError("Unable to load default one or more equipment [" + i.ToString() +
						":" + equipmentId + "] config file(s)! " + e.ToString());
				}
			}			
		}

		public void InitDiagnostics() {
			try {
				cpuCounter.CategoryName = "Processor";
				cpuCounter.CounterName = "% Processor Time";
				cpuCounter.InstanceName = "_Total";
				ramCounter.CategoryName = "Memory";
				ramCounter.CounterName = "Available MBytes";
				cpuCounter.NextValue(); //test to determine if we can include the PC...
				ramCounter.NextValue();
				Equipment.CpuCounter = cpuCounter;
				Equipment.RamCounter = ramCounter;
			} catch (Exception e) {
				addError("Unable to start performance counter! " + e.ToString());
				AppSettings.TrackResourcesUsage = false; //by force
			}
		}

    public bool AutoConnectResult => PH.IsSimulation || DbConnectionSettings.AutoConnect;

		public void InitAutoConnect() {
			try { //DB auto-connect
        if (PH.IsSimulation) //if it is in simulation mode, no need to connect to anything
          return;
				bool autoConnect = DbConnectionSettings != null && DbConnectionSettings.AutoConnect;
				if (autoConnect) //auto-connect request is there!
					t5OracleHandler.OpenConnection(DbConnectionSettings);
			} catch (Exception e) {
				addError("Auto-Connect exception! " + e.ToString());
			}
		}

		public void UpdatePingMessage(IEnumerable<byte> msg) {
			if (!msg.Any())
				return; //don't update if there isn't any
			byte[] bytes = msg.ToArray();
			ServerHandler.SetPingMessage(bytes);			
		}

		private int getActiveSearchNo() {
			return satsEquipments.Count(x => x.IsSearching && !pendingList.Contains(x)); //TODO use LINQ, could be a little dangerous, but by now it should already be okay...
		}

		public void HandlePendingEquipment() { //best is to separate handle pending equipment from the rests...			
			if (pendingList.Count <= 0 || getActiveSearchNo() >= AppSettings.MaxParallelSearch) //handle pending equipment
				return;
			while (pendingList.Count > 0 && getActiveSearchNo() < AppSettings.MaxParallelSearch) {
				Equipment equipment = pendingList[0];
				pendingList.Remove(equipment);
				CmdStat eStat = CmdStat.Uninitialized;
				bool result = prepareEquipment(equipment, out eStat);
				if (!result) { //If the equipment preparation fails, simply do not continue
					OptFlag.UpdateCmdStat(t5OracleHandler, equipment.ID, eStat == CmdStat.Uninitialized ?
						CmdStat.SearchFail : eStat);
					continue;
				}
				equipment.StartSearch();
        OptFlag.UpdateCmdStat(t5OracleHandler, equipment.ID, CmdStat.Processing);
				if (equipment.ID == CurrentUIEqpId) //if it is updated, then must be processing on the UI side
					indctrUpd = true;
			}
		}

		private bool prepareEquipment(Equipment equipment, out CmdStat eStat) {
			eStat = CmdStat.Uninitialized;
			try {
				if (equipment.IsSearching)
					equipment.TerminateSearch();

				string eStr = string.Empty;
				ActInitialState initState = t5OracleHandler.CreateInitialState(equipment.ID, equipment.Handler, out eStr);

				//Prepare initial states
				if (initState == null) {
					addError("Equipment initialization failure - Initial state not created! " + eStr);
					return false;
				}

				if (!string.IsNullOrWhiteSpace(eStr)) {//the initial state is created, but with error
					addError(eStr);
					return false;
				}

				equipment.InitialState = initState; //whatever is there must be deleted
				//IEnumerable<int> onGoingMoNos = initState.LeftActs.Select(x => x.No);

				//Prepare MO Acts
				List<MoInfoView> table = MoInfoView.GetList(t5OracleHandler, equipment.ID == null ? null : "EQPID='" + equipment.ID + "'"); //now, it is always done per equipment
				if (table == null) {
					addError("Fail to read in the " + t5OracleHandler.TableAndTest.TableNames["Info"] + " Table!");
					eStat = CmdStat.MoVwReadError;
					return false;
				}
				if (table.Count <= 0) {
          addError("No entry read in the " + t5OracleHandler.TableAndTest.TableNames["Info"] + " Table!");
					eStat = CmdStat.EntryNotFound;
          return false;
				}

				//E: dropping off, pickup or in the ETV, select that MO to be executed
				//MO -> X -> E -> E -> DO -> F

				List<Act> moActList = table // all the priority have been normalized, furthermore, all ULDs are found and not left blocked, and whatever blocks has been known and assigned. Time to create MOList
					.Select(x => new MO(x.PuPoint, x.DoPoint, x.Cat.ToString()) {
						Number = x.MoNo,
						Priority = x.PriorLevel,
						BlockingNumber = x.BlockingMoNo,
						EquipmentId = x.EqpId,
						Stat = x.MoStat, //Added in v2.8.3.6
					}) //generates MOs, then below generates MO Acts
					.Where(mo => equipment.ID == null || equipment.ID == mo.EquipmentId) //now the moActList created can be controlled by the equipment ID
					//.Where(mo => mo.Stat != "E") //Added in v2.8.3.6, to remove the non-executing stat MO
					.SelectMany(mo => ActHandler.CreateMOActsFromMO(mo)) //get everything						
					//.Where(moact => !(onGoingMoNos.Contains(moact.MoNumber)))
					.ToList(); //fixed in v2.4.1.5, improved in v2.5.0.0 and above
										 //as of v2.6.0.1 an above, moActList can be empty

				//Based on the initial state and MO acts, initialize the search parameter
				if (!equipment.InitSearchParameters(moActList, out eStr, out eStat)) {
					addError("Equipment initialization failure - Init search parameters failed!" +
						Environment.NewLine + eStat.ToString() + ". " + eStr);
					return false;
				}

				if (equipment.ID == CurrentUIEqpId)
					initStateUpd = true; //Just displaying the initial state, nothing else
				return true;
			} catch (Exception e) {
				addError("Equipment initialization failure! " + e.ToString());
				eStat = CmdStat.PrepEqpExc;
				return false;
			}
		}

		private void equipmentCmdStatHandler(Equipment equipment, CmdStat cmd) {
			switch (cmd) {
				case CmdStat.Processing:
					if (equipment.IsCompleted || equipment.IsSearching) { //check if it has been completed or is searching 
						string eStr;
						CmdStat responseCmdStat = OptSolution.Update(t5OracleHandler, //the only place where the table is updated
							equipment.InitialState.InitialDateTime, equipment, cmd, out eStr);
						if (eStr != string.Empty) {
							addError(eStr);
							return;
						}
						if (responseCmdStat != CmdStat.Processing) //processing need not to be updated all the time...
							OptFlag.UpdateCmdStat(t5OracleHandler, equipment.ID, responseCmdStat);
					} else //neither completed nor searching (but asked to process) -> re-searching
						requestSearchHandler(equipment);
					break;
				case CmdStat.Requesting:
					SearchCommandAccepted = true;
					requestSearchHandler(equipment);
					break;
				case CmdStat.Terminating:
					terminationSearchHandler(equipment);
					break;
				default:
					break;
			}
		}

		private void requestSearchHandler(Equipment equipment) {
			string eStrEqp = string.Empty;
			triggerAutoProcedure(); //added from version 2.4.0.0 and above to do auto triggering of the procedure...
																			//Basically searching equipment will get researched... non-searching equipment will be suspended if active search is limited														
			CmdStat eStat = CmdStat.Uninitialized;
			if (getActiveSearchNo() >= AppSettings.MaxParallelSearch && !equipment.IsSearching) { //not currently searching
				equipment.IsPending = true;
				if (!pendingList.Contains(equipment))
					pendingList.Add(equipment);
        OptFlag.UpdateCmdStat(t5OracleHandler, equipment.ID, CmdStat.Pending);
			} else if (prepareEquipment(equipment, out eStat)) {
				equipment.StartSearch();
        OptFlag.UpdateCmdStat(t5OracleHandler, equipment.ID, CmdStat.Processing);
			} else //failed case...
        OptFlag.UpdateCmdStat(t5OracleHandler, equipment.ID, eStat == CmdStat.Uninitialized ?
					CmdStat.SearchFail : eStat);
		}

		private void terminationSearchHandler(Equipment equipment) {
			try {
				equipment.TerminateSearch();
        OptFlag.UpdateCmdStat(t5OracleHandler, equipment.ID, CmdStat.Terminated);
			} catch {
        OptFlag.UpdateCmdStat(t5OracleHandler, equipment.ID, CmdStat.StopFail);
			}
		}

		private void triggerAutoProcedure() {
			try {
        if (PH.IsSimulation) //nothing can be executed when it is in simulation mode
          return;
				string errMsg = "";
				if (AppSettings.AutoExecutionCheck) { //non-fatal
					if (!t5OracleHandler.ExecuteAllInProcedure(AppSettings.ExecutionCheckProcedure, out errMsg))
						addError("Auto execution check procedure: " + AppSettings.ExecutionCheckProcedure +
							" is failed to execute! " + errMsg);
				}
				if (AppSettings.AutoPriorityAssignment) { //non-fatal
					DateTime dt = t5OracleHandler.TableAndTest.UseFixedDateTime ? t5OracleHandler.TableAndTest.FixedDateTime : DateTime.Now;
					KeyValuePair<string, object> par = new KeyValuePair<string, object>("p_refdate", dt);
					List<KeyValuePair<string, object>> pars = new List<KeyValuePair<string, object>>() { par };
					if (!t5OracleHandler.ExecuteAllInProcedure(AppSettings.PriorityAssignmentProcedure, pars, out errMsg))
						addError("Auto priority assignment procedure: " + AppSettings.PriorityAssignmentProcedure +
						" is failed to execute! " + errMsg);
				}
			} catch (Exception exc) { //non-fatal. Just continue...
				addError("Auto procedure exception: " + exc.ToString());
			}
		}

		public bool CanProcessTable() {
			return PH.IsSimulation || //if it is in simulation mode, table can always be processed
        (t5OracleHandler != null && t5OracleHandler.IsConnectionUp() && t5OracleHandler.TableAndTest != null);
		}

		public bool SearchCommandAccepted { get; set; } = false;
		public bool FailedWithException { get; private set; } = false;
		public void ProcessTable() { //the return will determine the update element
			FailedWithException = false;
			try {
				List<OptFlag> optFlags = OptFlag.GetList(t5OracleHandler);
				if (optFlags == null || !optFlags.Any()) //cannot update
					return;
				UpdatePingMessage(optFlags.Select(x => (byte)x.CmdStat)); //set the ping message accordingly
				if (AppSettings.Mode == T5STMode.Manual) //no further process for manual mode
					return;
				for (int i = 0; i < optFlags.Count; ++i) {
					Equipment equipment = satsEquipments.Find(x => x.ID == optFlags[i].EqpId);
					if (equipment == null) //cannot process such equipment...
						continue;
					if (equipment.ID == CurrentUIEqpId) { //If it happens to be the equipment shown in the GUI itself
						cmdStateUpd = optFlags[i].CmdStat; //Give the light update of certain equipment status					
						HasCmdStatUpdate = true;
					}
					equipmentCmdStatHandler(equipment, optFlags[i].CmdStat); //handle the status of this equipment
				}
				//tableTimer.Interval = AppSettings.TableTimerTickInMs;
			} catch (Exception exc) { //since this is pretty fast, the exception is not expected...
				addError("Fail to read the " + t5OracleHandler.TableAndTest.TableNames["OptFlag"] + " Table! " + exc.ToString());
				FailedWithException = true; //failure, great failure, cannot update
																		//tableTimer.Interval = 5000; //every 5 seconds due to failure...
																		//return false; //failure, great failure, cannot update
			}
		}

		public void SaveState(T5TableAndTest tableAndTest) {
			string folderpath = Path.Combine(root, configFoldername);
			if (!Directory.Exists(folderpath))
				Directory.CreateDirectory(folderpath); //configuration directory...

			//Shared settings
			//v2.8.2.3 and above. Perhaps, it is best not to save the connection settings at all
			//OracleConnectionSettings connectionSettings = DbConnectionSettings; 
			//connectionSettings.Password = password; //get whatever is in the beginning, password cannot be saved. 
			//XmlSerializer serializerObj = new XmlSerializer(typeof(OracleConnectionSettings));
			//string filepath = folderpath + "\\" + dbConnectionFilename + ".xml";
			//TextWriter configWriteFileStream = new StreamWriter(filepath);
			//serializerObj.Serialize(configWriteFileStream, connectionSettings);
			//configWriteFileStream.Close();

			serializer = new XmlSerializer(typeof(T5STAppSettings));
			filepath = Path.Combine(folderpath, appSettingsFilename + ".xml");
			configWriteFileStream = new StreamWriter(filepath);
			serializer.Serialize(configWriteFileStream, AppSettings);
			configWriteFileStream.Close();

			if (tableAndTest != null) { //the only one, at this moment, that depends on the GUI TODO. Kills this dependency!
				serializer = new XmlSerializer(typeof(T5TableAndTest));
				filepath = Path.Combine(folderpath, tableAndTestFilename + ".xml");
				configWriteFileStream = new StreamWriter(filepath);
				serializer.Serialize(configWriteFileStream, tableAndTest);
				configWriteFileStream.Close();
			}

			//wkstn/pickup/airland
			serializer = new XmlSerializer(typeof(GlobalActTimeSettings));
			filepath = Path.Combine(folderpath, globalActTimeFilename + ".xml");
			configWriteFileStream = new StreamWriter(filepath);
			serializer.Serialize(configWriteFileStream, ActHandler.GlobalActTimeSettings);
			configWriteFileStream.Close();

			//Non shared settings 
			for (int i = 0; i < satsEquipments.Count; ++i) {
				string equipmentFolderpath = folderpath + "\\" + equipmentFoldername + "\\" + satsEquipments[i].ID;
				if (!Directory.Exists(equipmentFolderpath))
					Directory.CreateDirectory(equipmentFolderpath); //configuration directory...
				serializer = new XmlSerializer(typeof(ActInitialState));
				filepath = Path.Combine(equipmentFolderpath, defaultInitialStateFilename + ".xml");
				configWriteFileStream = new StreamWriter(filepath);
				serializer.Serialize(configWriteFileStream, satsEquipments[i].InitialState);
				configWriteFileStream.Close();
			}

      //Simulation only
      if (PH.IsSimulation)
        OptFlag.SerializeLatestList();
		}

		public void SaveWatcherSettings() {
			//TCP heart beat settings
			serializer = new XmlSerializer(typeof(TCPIPServerSettings));
			filepath = Path.Combine(configFolderpath, tcpConnectionFilename + ".xml");
			configWriteFileStream = new StreamWriter(filepath);
			serializer.Serialize(configWriteFileStream, ServerHandler.Settings);
			configWriteFileStream.Close();
		}

    public bool IsDataConnectionReady => PH.IsSimulation || t5OracleHandler.IsConnectionUp();    

		public void ManualSearch(out bool isInitStateUpdated) {
			Equipment equipment = satsEquipments.Find(x => x.ID == CurrentUIEqpId);
			bool isCommandToSearch = !equipment.IsSearching; //if this is true, then the command is to search, because it is now not...
			isInitStateUpdated = false;
			bool isEqpInitStateUpdated = false;
			if (isCommandToSearch) { //necessarily after the search, MO will be ready!
				if (!IsDataConnectionReady)
					return;
				string eStrEqp = string.Empty;
				triggerAutoProcedure(); //added from version 2.4.0.0 and above to do auto triggering of the procedure...
        //for non simulation, status will be updated by the Ignition application
        CmdStat eStat = CmdStat.Uninitialized; 
				bool result = prepareEquipment(equipment, out eStat);
				isInitStateUpdated = isInitStateUpdated || isEqpInitStateUpdated;
				if (!result)
					return;
        if (PH.IsSimulation) //simulation will update its own status here
          OptFlag.UpdateCmdStat(t5OracleHandler, equipment.ID, eStat == CmdStat.Uninitialized ? CmdStat.Processing : eStat);
				equipment.StartSearch();
				Equipment.IncludePC = AppSettings.TrackResourcesUsage;
				if (AppSettings.SearchAllEquipments) //changed to get from non GUI
					foreach (Equipment otherEquipment in satsEquipments.Where(x => x != equipment)) {
            if (getActiveSearchNo() >= AppSettings.MaxParallelSearch) {
							otherEquipment.IsPending = true;
              if (!pendingList.Contains(otherEquipment)) {
                pendingList.Add(otherEquipment);
                if (PH.IsSimulation)
                  eStat = CmdStat.Pending;
              }
						} else if (prepareEquipment(otherEquipment, out eStat))
							otherEquipment.StartSearch();						
						isInitStateUpdated = isInitStateUpdated || isEqpInitStateUpdated;
            if (PH.IsSimulation) //simulation will update its own status here
              OptFlag.UpdateCmdStat(t5OracleHandler, otherEquipment.ID, eStat == CmdStat.Uninitialized ? CmdStat.Processing : eStat);
          }
      } else { //termination command
				equipment.TerminateSearch();
        if (PH.IsSimulation)
          OptFlag.UpdateCmdStat(t5OracleHandler, equipment.ID, CmdStat.Terminating); 
				if (AppSettings.SearchAllEquipments) {
					List<Equipment> otherEquipments = satsEquipments.FindAll(x => x != equipment);
          foreach (Equipment otherEquipment in otherEquipments)
            if (otherEquipment.IsSearching) {
              otherEquipment.TerminateSearch();
              if (PH.IsSimulation)
                OptFlag.UpdateCmdStat(t5OracleHandler, otherEquipment.ID, CmdStat.Terminating);
            }
        }
			}			
		}

		public void DestroyAllEquipments() {
			if (satsEquipments == null)
				return;
			for (int i = 0; i < satsEquipments.Count; ++i)
				satsEquipments[i].Destroy();
		}

		public T5STLogicState GetCurrentState() {
			Equipment equipment = satsEquipments.SingleOrDefault(x => x.ID == CurrentUIEqpId);
			if (equipment == null)
				return null;
			return new T5STLogicState(equipment);
		}

		public T5STEqpFlags GetEquipmentFlags(string id) {
			Equipment equipment = satsEquipments.SingleOrDefault(x => x.ID == id);
			if (equipment == null)
				return null;
			return new T5STEqpFlags(equipment);
		}

		public ActInitialState GetEquipmentInitialState(string id) {
			Equipment equipment = satsEquipments.SingleOrDefault(x => x.ID == id);
			if (equipment == null)
				return null;
			return equipment.InitialState;
		}

		public Equipment GetEquipment(string id) {
			return satsEquipments.SingleOrDefault(x => x.ID == id);
		}

		public void SaveEquipment(string id) {
			Equipment equipment = GetEquipment(id);
			if (!Directory.Exists(configFolderpath))
				Directory.CreateDirectory(configFolderpath); //configuration directory...
			string folderpath = Path.Combine(configFolderpath, equipmentFoldername);
			if (!Directory.Exists(folderpath))
				Directory.CreateDirectory(folderpath); //equipments directory...
			string equipmentFolderpath = Path.Combine(folderpath, id);
			if (!Directory.Exists(equipmentFolderpath))
				Directory.CreateDirectory(equipmentFolderpath); //configuration directory...
			serializer = new XmlSerializer(typeof(RouteFinderSettings));
			filepath = Path.Combine(equipmentFolderpath, routeFinderFilename + ".xml");
			configWriteFileStream = new StreamWriter(filepath);
			serializer.Serialize(configWriteFileStream, equipment.RouteFinderSettings);
			configWriteFileStream.Close();
			serializer = new XmlSerializer(typeof(RecordLogSettings));
			filepath = Path.Combine(equipmentFolderpath, recordLogFilename + ".xml");
			configWriteFileStream = new StreamWriter(filepath);
			serializer.Serialize(configWriteFileStream, equipment.RecordLogSettings);
			configWriteFileStream.Close();
			serializer = new XmlSerializer(typeof(TimeSettings));
			filepath = Path.Combine(equipmentFolderpath, actTimeFilename + ".xml");
			configWriteFileStream = new StreamWriter(filepath);
			serializer.Serialize(configWriteFileStream, equipment.Handler.TimeSettings);
			configWriteFileStream.Close();
			serializer = new XmlSerializer(typeof(ActInitialState));
			filepath = Path.Combine(equipmentFolderpath, defaultInitialStateFilename + ".xml");
			configWriteFileStream = new StreamWriter(filepath);
			serializer.Serialize(configWriteFileStream, equipment.InitialState);
			configWriteFileStream.Close();
		}

		public void HideAllLogBox() {
			foreach (Equipment equipment in satsEquipments)
				equipment.RecordLogSettings.ShowLog = false;
		}

    private void getSimulationTestXml() {
      //Just to create all the serialization
      string folderpath = Path.Combine(root, simulationFoldername);
      if (!Directory.Exists(folderpath))
        Directory.CreateDirectory(folderpath); //simulation directory...

      EqpInfoReducedList eqpInfoReducedList = new EqpInfoReducedList();
      EqpInfoReduced eqpInfoReduced = new EqpInfoReduced();
      eqpInfoReduced.ApplyTestData();
      eqpInfoReducedList.Items.Add(eqpInfoReduced);
      eqpInfoReduced = new EqpInfoReduced();
      eqpInfoReduced.ApplyTestData();
      eqpInfoReducedList.Items.Add(eqpInfoReduced);
      serializer = new XmlSerializer(typeof(EqpInfoReducedList));
      filepath = Path.Combine(folderpath, eqpInfoFilename + ".xml");
      configWriteFileStream = new StreamWriter(filepath);
      serializer.Serialize(configWriteFileStream, eqpInfoReducedList);
      configWriteFileStream.Close();

      LedInfoReducedList ledInfoReducedList = new LedInfoReducedList();
      LedInfoReduced ledInfoReduced = new LedInfoReduced();
      ledInfoReduced.ApplyTestData();
      ledInfoReducedList.Items.Add(ledInfoReduced);
      ledInfoReduced = new LedInfoReduced();
      ledInfoReduced.ApplyTestData();
      ledInfoReducedList.Items.Add(ledInfoReduced);
      serializer = new XmlSerializer(typeof(LedInfoReducedList));
      filepath = Path.Combine(folderpath, ledInfoFilename + ".xml");
      configWriteFileStream = new StreamWriter(filepath);
      serializer.Serialize(configWriteFileStream, ledInfoReducedList);
      configWriteFileStream.Close();

      MoInfoViewList moInfoViewList = new MoInfoViewList();
      MoInfoView moInfoView = new MoInfoView();
      moInfoView.ApplyTestData();
      moInfoViewList.Items.Add(moInfoView);
      moInfoView = new MoInfoView();
      moInfoView.ApplyTestData();
      moInfoViewList.Items.Add(moInfoView);
      serializer = new XmlSerializer(typeof(MoInfoViewList));
      filepath = Path.Combine(folderpath, moInfoViewFilename + ".xml");
      configWriteFileStream = new StreamWriter(filepath);
      serializer.Serialize(configWriteFileStream, moInfoViewList);
      configWriteFileStream.Close();

      OptFlagList optFlagList = new OptFlagList();
      OptFlag optFlag = new OptFlag();
      optFlag.ApplyTestData();
      optFlagList.Items.Add(optFlag);
      optFlag = new OptFlag();
      optFlag.ApplyTestData();
      optFlagList.Items.Add(optFlag);
      serializer = new XmlSerializer(typeof(OptFlagList));
      filepath = Path.Combine(folderpath, optFlagFilename + ".xml");
      configWriteFileStream = new StreamWriter(filepath);
      serializer.Serialize(configWriteFileStream, optFlagList);
      configWriteFileStream.Close();

      OptSolutionList optSolutionList = new OptSolutionList();
      OptSolution optSolution = new OptSolution();
      optSolution.ApplyTestData();
      optSolutionList.Items.Add(optSolution);
      optSolution = new OptSolution();
      optSolution.ApplyTestData();
      optSolutionList.Items.Add(optSolution);
      serializer = new XmlSerializer(typeof(OptSolutionList));
      filepath = Path.Combine(folderpath, optSolutionFilename + ".xml");
      configWriteFileStream = new StreamWriter(filepath);
      serializer.Serialize(configWriteFileStream, optSolutionList);
      configWriteFileStream.Close();
    }
  }

	public class T5STLogicState {
		public RouteFinderState EqpState;
		public RouteFinderPerformance Pf;
		public T5STEqpFlags Flags;
		public T5STLogicState(Equipment equipment) {			
			EqpState = equipment.GetState();
			Pf = EqpState.Performance;
			Flags = new T5STEqpFlags(equipment);
		}
	}

	public class T5STEqpFlags {
		public string ID;
		public bool HasEnded, IsSearching, IsTerminated, IsPending, IsCompleted;
		public T5STEqpFlags(Equipment equipment) {
			ID = equipment.ID;
			HasEnded = equipment.SearchObjectHasEnded;
			IsSearching = equipment.IsSearching;
			IsTerminated = equipment.IsTerminated;
			IsPending = equipment.IsPending;
			IsCompleted = equipment.IsCompleted;
		}
	}

	public enum T5STMode {
		Manual = 0,
		SemiAuto = 1,
		Auto = 2,
	}

	#region serializable
	[Serializable()]
	public class T5STAppSettings {
		public static int DEFAULT_TABLE_TIMER_TICK_IN_MS = 25;
		public static int DEFAULT_DISPLAY_TIMER_TICK_IN_MS = 150;
		[XmlArrayItem("EquipmentID", typeof(string))]
		public List<string> EquipmentIDList = new List<string>();
		public bool SearchAllEquipments = true;
		public int MaxParallelSearch = 1;
		public bool ShowLogBox = false;
		public bool TrackResourcesUsage = false;
		public bool LockTable = false;
		public bool AutoExecutionCheck = true;
		public bool AutoPriorityAssignment = true;
		public bool IsStrongCheck = true;
		public string ExecutionCheckProcedure = "";
		public string PriorityAssignmentProcedure = "";
		public int TableTimerTickInMs = DEFAULT_TABLE_TIMER_TICK_IN_MS;
		public int DisplayTimerTickInMs = DEFAULT_DISPLAY_TIMER_TICK_IN_MS;
		public T5STMode Mode = T5STMode.SemiAuto;
    public bool SimulationMode = false; //added so that the logic can be simulated without database
	}
	#endregion
}
