using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using T5ShortestTime.MOAct;

namespace T5ShortestTime.SATS {
  public class Equipment
  {
    public RouteFinderSettings RouteFinderSettings = new RouteFinderSettings();
    public RecordLogSettings RecordLogSettings = new RecordLogSettings();
    public ActHandler Handler = new ActHandler(); //in the handler, there is a time setting...
    public ActInitialState InitialState = new ActInitialState();
    public RouteFinder MoActRouteFinder = new RouteFinder();

    //Record to be used independently from the RecordLogSettings during the search process
    public bool IsTracked = false;
    public bool UpdateOnFinished = false;
    public bool IsMemoryRecorded = false;
    public int RecordLimit = 50000; 
    
    public bool IsSearching = false;
    public bool IsTerminated = false; //this is to signal the indicator in the GUI, use this as per necessary...
    public bool IsCompleted = false;
    public bool IsPending = false; //controlled from outside 

    private string id = "";
    public string ID { get { return id; } set { id = value; MoActRouteFinder.ParentId = id; } }

    //Recording purpose is now in the equipments to help the separation of concern
    //It needs the file name
    private string recordFile;
    private int recordComputeNo = -1;
    private int subRecordComputeNo = 0;
    private int recordNo = 0;
    private StreamWriter recordWriter;
    private int subMemoryComputeNo = 0;
    private int recordMemoryNo = 0;
    private string recordMemoryFile;
    private StreamWriter recordMemoryWriter;

    //Showing whenever necessary
    public RouteFinderState State = null; //initialized as null and must be nullified on new search

    //Threading purpose
    private Thread searchThread;
    private SearchArg searchArg;
    private System.Windows.Forms.Timer updateTimer = new System.Windows.Forms.Timer();
    public bool SearchObjectHasEnded { get { return searchArg != null && searchArg.HasEnded; } }

    //Shared
    private static string timeDisplayFormat = "yyyy-MM-dd HH:mm:ss.fff"; //shared among all the classes
    public static DateTime StartUpDateTime; //All there shared must be defined by the outsider, TODO how to force it?
    public static DateTime RecordDateTime;
    public static string RecordFolder;
    public static string EquipmentFolderName;
    private static string recordSessionFolder; //only initialized before the start of actual recording, not used otherwise...
    private static int MAX_LOG_QUEUE_SIZE = 1000;
    public static Queue<string> LogQueue = new Queue<string>(MAX_LOG_QUEUE_SIZE);

    //Diagnostics: http://stackoverflow.com/questions/278071/how-to-get-the-cpu-usage-in-c
    public static bool IncludePC = false;
    public static PerformanceCounter CpuCounter = null;
    public static PerformanceCounter RamCounter = null;

    //Solution (for table making)    
    public ActNode LastSolution { get { return MoActRouteFinder.GetFinalNodeSolution(); } }

    public Equipment() {
      //timer for recording initialization
      updateTimer.Interval = 1000;
      updateTimer.Tick += updateTimer_Tick;
    }

    public void Destroy() {
      if (searchThread != null) {
        searchThread.Abort();
        searchArg = null;
      }
      if (MoActRouteFinder != null)
        MoActRouteFinder.Terminate();
    }

    void updateTimer_Tick(object sender, EventArgs e) {
      //If the record is on, then this will naturally be ticking, only to stop this when the search has ended
      if (MoActRouteFinder == null || (searchArg != null && searchArg.HasEnded))
        updateTimer.Stop();
      if (MoActRouteFinder != null && (IsTracked || IsMemoryRecorded || RecordLogSettings.ShowLog)) {
        State = MoActRouteFinder.GetState(); //get the latest state only upon record
        recordTrackOrMemory();
      }
      if (searchArg != null && searchArg.HasEnded) {
				try {
					State = MoActRouteFinder.GetState(); //for the last time...
					writeEndRecord();
				} catch(Exception ex) {
					writeFailEndRecord(ex.ToString());
				}
				closeRecorder();				
        IsSearching = false;
        IsTerminated = false;
        IsCompleted = true;
      }
    }

    public RouteFinderState GetState(bool forceUpdate = false) {
      if (forceUpdate) {
        State = MoActRouteFinder.GetState();
        return State;
      }
      return MoActRouteFinder.GetState();
    }

    private void searchThreadFunction(object sender) {
      SearchArg arg = sender as SearchArg; //will be immediately started!
      SynchronizationContext syncContext = arg.SyncContext;
      RouteFinder routeFinder = arg.RouteFinder;
      routeFinder.Search(arg.InitPar);
      arg.HasEnded = true;      
    }

    public bool InitSearchParameters(List<Act> moActsList, out string eStr, out CmdStat eStat) {
			eStr = "";
			eStat = CmdStat.Uninitialized;
      bool result = Handler.InitializeSearchParameters(ID, InitialState, moActsList, 
				RouteFinderSettings.IsSingleL, RouteFinderSettings.FirstDropHasHighPriority, 
				RouteFinderSettings.FirstDropHasHighPriorityForMultipleUlds,
				out eStr, out eStat);
      if (result) {
        SearchArgInitPar initialParameter = Handler.CreateActSearchArgInitialParameter();
        MoActRouteFinder = new RouteFinder(); //the route finder is renewed everytime the search is re-initialized (because of threading)
        MoActRouteFinder.ParentId = ID;
        MoActRouteFinder.Settings = RouteFinderSettings; //the settings are only applied on the start search
        searchArg = new SearchArg(initialParameter, MoActRouteFinder, SynchronizationContext.Current);
      }
      return result;
    }

    public void StartSearch() { //basically, it only needs the initial condition and the list of MOActs
      State = null;
      IsSearching = true;
      IsTerminated = false;
      IsCompleted = false;
      IsPending = false;

      initRecorder();
      writeInitRecord();

      updateTimer.Start();
      searchThread = new Thread(searchThreadFunction);
      searchThread.Name = "Search Thread " + ID;
      searchThread.Start(searchArg);      
    }

    public void TerminateSearch() {
      if (searchThread != null) {
        searchThread.Abort();
        searchThread = null;
      }
      if (MoActRouteFinder != null)
        MoActRouteFinder.Terminate();      
      closeRecorder();
      IsSearching = false;
      IsCompleted = false;
      IsTerminated = true;
    }

    #region recording and logging
    private string lastRoute = "";
    private void initRecorder() {
      closeRecorder(); //to close previously opened record, if any

      IsTracked = RecordLogSettings.Track; //Initialize such that it won't change during the process
      IsMemoryRecorded = RecordLogSettings.Memory;
      UpdateOnFinished = RecordLogSettings.UpdateOnFinished;
      RecordLimit = RecordLogSettings.MaxRecord;

      if (IsMemoryRecorded || IsTracked) {
        DateTime now = DateTime.Now;
        string dtStr = StartUpDateTime.ToString("yyyyMMdd");
        bool dayChange = false;
        if (dtStr != now.ToString("yyyyMMdd")) { //day has changed!
          dtStr = now.ToString("yyyyMMdd");
          dayChange = true;
        }
        string dtHmsStr = StartUpDateTime.ToString("HHmmss_fff");
        if (dayChange) { //prepare to create new folder whenever necessary...
          if (RecordDateTime == null || RecordDateTime.ToString("yyyyMMdd") != dtStr) //if no new folder for the new day
            RecordDateTime = now;
          dtHmsStr = RecordDateTime.ToString("HHmmss_fff"); //use whatever has been defined
        }

        if (!Directory.Exists(RecordFolder))
          Directory.CreateDirectory(RecordFolder);
        if (!Directory.Exists(RecordFolder + "\\" + dtStr))
          Directory.CreateDirectory(RecordFolder + "\\" + dtStr);
        recordSessionFolder = RecordFolder + "\\" + dtStr + "\\" + dtHmsStr;
        if (!Directory.Exists(recordSessionFolder))
          Directory.CreateDirectory(recordSessionFolder);
        ++recordComputeNo;
        if (IsTracked) {
          subRecordComputeNo = 1;
          recordNo = 0;
          if (!Directory.Exists(recordSessionFolder + "\\log"))
            Directory.CreateDirectory(recordSessionFolder + "\\log");
          recordFile = recordSessionFolder + "\\log\\" + ID + "_" +
            recordComputeNo.ToString("d3") + "_" + RouteFinderSettings.SMethod.ToString().Substring(0, 2);
          recordWriter = File.CreateText(recordFile + "_" + subRecordComputeNo.ToString("d3") + ".txt");
        }
        if (IsMemoryRecorded) {
          subMemoryComputeNo = 1;
          recordMemoryNo = 0;
          if (!Directory.Exists(recordSessionFolder + "\\memory"))
            Directory.CreateDirectory(recordSessionFolder + "\\memory");
          recordMemoryFile = recordSessionFolder + "\\memory\\" + ID + "_" +
            recordComputeNo.ToString("d3") + "_" + RouteFinderSettings.SMethod.ToString().Substring(0, 2);
          recordMemoryWriter = File.CreateText(recordMemoryFile + "_" + subMemoryComputeNo.ToString("d3") + ".txt");
        }
      }

      lastRoute = "";
    }

    private void writeInitRecord() {
      if ((IsTracked && recordWriter != null) ||
        (RecordLogSettings.ShowLog)) {
				StringBuilder strBuilder = new StringBuilder();
				strBuilder.AppendLine(string.Concat("ID: ", ID));
				strBuilder.AppendLine(string.Concat("MO Acts count = ", Handler.MoActCount));
				strBuilder.AppendLine(Handler.GetBasicActListString());
				strBuilder.AppendLine();

				//string msg = "ID: " + ID + Environment.NewLine + 
    //      "MO Acts count = " + Handler.MoActCount.ToString() + Environment.NewLine;
    //    msg += Handler.GetBasicActListString() + Environment.NewLine + Environment.NewLine;
        Act ima = Handler.InitialAct;
        List<Act> ipma = Handler.InitialPuActs;

				strBuilder.AppendLine(ima.GetInitString(ipma));

				//msg += ima.GetInitString(ipma) + Environment.NewLine;
				List <KeyValuePair<Act, int>> smt = ima.StaticMovingTimeOriginal;
				for (int i = 0; i < smt.Count; ++i) {
					strBuilder.AppendLine(string.Concat("  ", ima.GetSMTString(i)));
					//msg += "  " + ima.GetSMTString(i) + Environment.NewLine;
				}
				//msg += Environment.NewLine;
				//msg += Handler.GetMoActListString();

				strBuilder.AppendLine();
				strBuilder.Append(Handler.GetMoActListString());

				int combinedMoActs = Handler.MoActCount + 1;

				//msg += "Combined Legal Moves: " + Handler.CombinedLegalMoves.ToString() +
				//  ", Average Branching Factor: " + Handler.AverageBranchingFactor.ToString("f1") +
				//  Environment.NewLine + Environment.NewLine;

				strBuilder.AppendLine(string.Concat("Combined Legal Moves: ", Handler.CombinedLegalMoves.ToString(),
					", Average Branching Factor: ", Handler.AverageBranchingFactor.ToString("f1")));
				strBuilder.AppendLine();

				if (MoActRouteFinder.Settings.SMethod == SearchMethod.Brute_Force) { //For brute
          double posRoute = Math.Pow(Handler.AverageBranchingFactor, 0.4 * combinedMoActs) / (combinedMoActs / 3); //0.4 is an estimation!
          //msg += "Possible routes: " + posRoute.ToString() + Environment.NewLine;
					strBuilder.AppendLine(string.Concat("Possible routes: ", posRoute));
				}

				strBuilder.AppendLine("Settings: " + MoActRouteFinder.Settings.GetString());
				strBuilder.AppendLine();
				//msg += "Settings: " + MoActRouteFinder.Settings.GetString() + Environment.NewLine + Environment.NewLine;
				string msg = strBuilder.ToString();
        if (IsTracked && recordWriter != null)
          recordWriter.Write(msg);
        if (RecordLogSettings.ShowLog && LogQueue.Count < MAX_LOG_QUEUE_SIZE)
          updateLogQueue(msg);
      }
    }

    private void recordTrackOrMemory() { //to record the track
      if (recordWriter != null || recordMemoryWriter != null || RecordLogSettings.ShowLog) {
        string dateTimeString = State.DateTimeStamp.ToString(timeDisplayFormat);
        string sourceCode = "NA";
        KeyValuePair<List<Act>, int> moActFinalSolution = State.Solution;
        RouteFinderPerformance pf = State.Performance;
        string routeStr = "";
        string routeMsg = routeMsg = "D" + State.BestSolvedDepth + "|" + State.SolvedDepth + "|" + sourceCode;
        string timeText = "";

        if (moActFinalSolution.Key != null) {
          for (int i = 0; i < moActFinalSolution.Key.Count; ++i)
            routeStr += moActFinalSolution.Key[i].GetString() + " ";
          timeText = ((sourceCode != "FS" && !SearchObjectHasEnded) ?
                  (moActFinalSolution.Key.Count < State.TotalDepth ? ">=" : "<=") : "") +
                  (0.1 * moActFinalSolution.Value).ToString("N1");           
          routeMsg += "|" + moActFinalSolution.Key.Count;
        } else {
          routeStr = "NA";
          timeText = "NA";
        }
        string evaluationStr = 
          "Evaluation: {Solution=" + routeMsg + ", Session=" + pf.CurrentSessionDepth.ToString() +
          ", Iteration=" + pf.IterationNo.ToString() + "-" + pf.EvaluatedBranchCode + ", Depth=" +
          pf.CurrentIterationDepth.ToString() + " [+" + pf.IterationNodeExtraDepth.ToString() + "]" +
          ", Evaluated=" + pf.IterationNodesEvaluated.ToString("N0") +
          ", Nodes=" + pf.IterationNodes.ToString("N0") + "}"; //for the evaluation part
        string recordMsg = 
          evaluationStr + Environment.NewLine +
          "Result: {Time=" + timeText + " second(s), Route=" +
          routeStr + "}" + Environment.NewLine; //for the result part
        string performanceString = 
          "Performance: {EL=" + pf.EvCount.EvaluatedLeaves.ToString("N0") +
          ", EN=" + pf.EvCount.EvaluatedNodes.ToString("N0") +
          ", AN=" + pf.EvCount.AbortedNodes.ToString("N0") +
          ", CN=" + pf.EvCount.CreatedNodes.ToString("N0") +
          ", DN=" + pf.EvCount.DestroyedNodes.ToString("N0") +
          ", LN=" + (pf.EvCount.CreatedNodes - pf.EvCount.DestroyedNodes).ToString("N0") +
          ", Time Lapse=" + pf.TotalTimeLapse.ToString("N0") +
          " ms, Speed=" + pf.NodeSpeed.ToString("N0") + " Nodes/s}";
        recordMsg += performanceString + Environment.NewLine;

        if ((IsTracked && lastRoute != routeStr && recordWriter != null) ||
          (RecordLogSettings.ShowLog)) {
          if (IsTracked && lastRoute != routeStr && recordWriter != null) {
            recordMsg += "On: " + dateTimeString + Environment.NewLine + Environment.NewLine;
            recordWriter.Write(recordMsg);
            recordNo++;
            if (recordNo >= RecordLimit) {
              subRecordComputeNo++;
              recordNo = 0;
              recordWriter.Close();
              recordWriter = File.CreateText(recordWriter + "_" + subRecordComputeNo.ToString("d3") + ".txt");
            }
          } else if (RecordLogSettings.ShowLog && LogQueue.Count < MAX_LOG_QUEUE_SIZE) {
            updateLogQueue("ID: " + ID + Environment.NewLine + recordMsg);
          }
          lastRoute = routeStr;
        }

        if (IsMemoryRecorded && recordMemoryWriter != null) {
          string cpuUsed = "";
          string avRam = "";
          if (IncludePC) {
            if (CpuCounter != null)
              cpuUsed = CpuCounter.NextValue().ToString("N1");
            if (RamCounter != null)
              avRam = RamCounter.NextValue().ToString("N1");
          }
          string memoryString = "[" + dateTimeString + "] Memory: {Used=" + State.TotalMemoryMB.ToString("N1") +
            " MB, CPU=" + cpuUsed + " %, AvRAM=" + avRam + " MB} " + evaluationStr + " " +
            performanceString + Environment.NewLine;
          recordMemoryWriter.Write(memoryString);
          recordMemoryNo++;
          if (recordMemoryNo >= RecordLimit) {
            subMemoryComputeNo++;
            recordMemoryNo = 0;
            recordMemoryWriter.Close();
            recordMemoryWriter = File.CreateText(recordMemoryFile + "_" + subMemoryComputeNo.ToString("d3") + ".txt");
          }
        }
      }
    }

    private void writeEndRecord() {
      if ((IsTracked && recordWriter != null) ||
        (RecordLogSettings.ShowLog)) {
        string msg = MoActRouteFinder.GetClosingString();
        if (IsTracked && recordWriter != null)
          recordWriter.Write(msg);
        if (RecordLogSettings.ShowLog && LogQueue.Count < MAX_LOG_QUEUE_SIZE)
          updateLogQueue("ID: " + ID + Environment.NewLine + msg);
      }
    }

		private void writeFailEndRecord(string exceptionMessage) {
			if ((IsTracked && recordWriter != null) ||
				(RecordLogSettings.ShowLog)) {
				string msg = "Failed to write the last state or to write the end record: " + exceptionMessage;
				if (IsTracked && recordWriter != null)
					recordWriter.Write(msg);
				if (RecordLogSettings.ShowLog && LogQueue.Count < MAX_LOG_QUEUE_SIZE)
					updateLogQueue("ID: " + ID + Environment.NewLine + msg);
			}
		}

		private void closeRecorder() {
      if (recordWriter != null) {
        recordWriter.Close();
        recordWriter = null;
      }
      if (recordMemoryWriter != null) {
        recordMemoryWriter.Close();
        recordMemoryWriter = null;
      }
    }

    private static readonly object locker = new object();
    private void updateLogQueue(string msg) {
      //lock (LogQueue)
      //  lock (msg)
      lock (locker)
        LogQueue.Enqueue(msg);
    }

    public static string GetTopLogQueue() {
      //lock (LogQueue)
      lock (locker)
        return LogQueue.Dequeue();
    }
    #endregion
  }

  [Serializable()]
  public class RecordLogSettings{
    public bool Track;
    public bool Memory;
    public int MaxRecord = 50000;
    public bool ShowLog;
    public bool UpdateOnFinished;
  }
}
