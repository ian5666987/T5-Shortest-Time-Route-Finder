using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace T5ShortestTime.MOAct {
  public enum CmdStat
  {
		FailILegalMove = -14,
		FailLegalMove = -13,
		CircularBlkMO = -12,
		MoVwReadError = -11,
    SearchFail = -10,
    StopFail = -9,
    EntryNotFound = -8,
    UldNotFound = -7,
    SolNotFound = -6,
    SolFail = -5,
    Blocked = -4,
    NormPrioExc = -3,
    PrepEqpExc = -2,
    UldNotFoundExc = -1,
    Uninitialized = 0,
    Requesting = 1,
    Processing = 2,
    Completed = 3,
    Terminating = 4,
    Terminated = 5,
		Pending = 6,
	}

  public class RouteFinder
  {
    //Clustering
    private ClusterVariables ClusterVar = new ClusterVariables(); //must only be declared once, here...

		//Threading    
		private Thread[] branchEvaluatorThread;
		private Evaluator[] branchEvaluator;
		private EvaluationCount[] branchEvCount;

    //Current state
    private int solvedDepth = 0; //To specify the current depth of the MO Acts solution
    private int bestSolvedDepth = 0;
    private int currentIterationBranchLimit = 100;
    private int currentSessionDepth = 0;
    private int currentDepthDifference = 0;
    private int currentBranchDepth = 0;
    private int sessionDepth = 0;
    private int completedDepth = 0;
    private int evaluatorNodeCountRecord = 0; //for reporting...
    private List<ActNode> finalCandidates;
    private ActNode finalSolution;
    private int finalCandidateBestSolutionIndex = -1;
    private int threadNodeExtraDepth = 0;
    private string currentlyEvaluatedBranchCode = "M"; //M=main node, N=new node, O=old node
    private int iterationNo = 1;
    private List<Act> globalMinOfMinMoActsListCopy = new List<Act>();
    private int globalMinOfMinAccTimeCopy = int.MaxValue;
    private int currentNodeProcessedIndex;
    private bool applyThreadSleep = true;
    private List<ActNode> evaluatorNodes;    
    private int noOfActiveThread = 0;
    private int usedFinalDepth = 9;
    private int totalDepth = 0; //This is to be found or to be given... best is given!
    private double averageBranchingFactor; //referred outside for recording...

    //Settings
    public RouteFinderSettings Settings { get; set; } = new RouteFinderSettings();

    //Solution Node
    private ActNode mainNode;
    private List<ActNode> oldParentBranches;
    private List<ActNode> newParentBranches;
    private List<ActNode> searchSolution;
    private List<ActNode> oldSearchSolution;
    private List<ActNode> newSearchSolution;    

    //Boost
    private int timePenaltyPerDepth;
    private List<Act> moActsList; //finally, this is used to boost in multi-threading

    //Info
    public string ParentId { get; set; } = "";

    //Performance measurement...
    private EvaluationCount evCount = new EvaluationCount();
    private DateTime initDt = new DateTime();
    private DateTime startDt;
    private DateTime stopDt;
    private DateTime iterationDt;

    public RouteFinder() {
      startDt = initDt; //Since it cannot be initiated elsewhere...
      stopDt = initDt;
      iterationDt = initDt;
    }

    private void initSearch(SearchArgInitPar initPar) {
      //Data initialization
      stopDt = initDt;
      startDt = DateTime.Now;
      mainNode = new ActNode(initPar.InitialAct, 0); //main node is created with initialAct and time of zero
      mainNode.PuMOActUnfinished = initPar.InitialPuActs == null ? new List<Act>() : initPar.InitialPuActs;
      totalDepth = initPar.TotalRemainingActs;
      usedFinalDepth = Math.Min(Settings.FinalDepth, totalDepth); //impossible to be more than total depth per search
      timePenaltyPerDepth = initPar.TimePenaltyPerDepth;
      finalCandidates = new List<ActNode>();
      sessionDepth = Math.Min(Settings.StartDepth, initPar.TotalRemainingActs);
      averageBranchingFactor = initPar.AverageBranchingFactor;
      Settings.AbortionFactor = Math.Min(1, Math.Max(0, Settings.AbortionFactor)); //must be between 0 to 1
      if (Settings.SMethod == SearchMethod.Brute_Force) {
        Settings.NewIterationBranch = 1; //forced to be 1! 
        Settings.IncludeFinalSolution = false; //cannot retain the final solution or candidates for sure!
        Settings.IncludeFinalCandidates = false;
        sessionDepth = initPar.TotalRemainingActs; //cannot be otherwise!
      }

      //Performance measurement
      evCount.Reset();
      finalSolution = null;

      //Multi-threading
      Settings.NoOfThread = Settings.IsMultithreading ? Math.Max(1, Math.Min(Settings.NoOfThread, RouteFinderSettings.MaxNoOfThread)) : 1;
      Settings.ByFactorOf = Settings.IsMultithreading ? Math.Max(1, Math.Min(Settings.ByFactorOf, RouteFinderSettings.MaxByFactorOf)) : 1;
      branchEvaluatorThread = new Thread[Settings.NoOfThread]; //branch evaluator thread is initialized as 1 if not for multi-threading...
      branchEvaluator = new Evaluator[Settings.NoOfThread];
      branchEvCount = new EvaluationCount[Settings.NoOfThread];
      moActsList = new List<Act>(initPar.ActsList); //never defile the original list...

			//On further work, this is probably not a good idea    
      initPar.InitialAct.StaticMovingTimeDuplicates = new List<List<KeyValuePair<Act, int>>>();
      for (int i = 0; i < Settings.NoOfThread; ++i) {
				initPar.InitialAct.StaticMovingTimeDuplicates.Add(new List<KeyValuePair<Act, int>>()); //ensure that it has sufficient amount of duplicate for the list to be played with...
        branchEvCount[i] = new EvaluationCount();
      }
      for (int i = 0; i < initPar.InitialPuActs.Count; ++i) {
				initPar.InitialPuActs[i].StaticMovingTimeDuplicates = new List<List<KeyValuePair<Act, int>>>();
				initPar.InitialPuActs[i].CoupleMOAct.StaticMovingTimeDuplicates = new List<List<KeyValuePair<Act, int>>>();
        for (int j = 0; j < Settings.NoOfThread; ++j) {
					initPar.InitialPuActs[i].StaticMovingTimeDuplicates.Add(new List<KeyValuePair<Act, int>>()); //ensure that it has sufficient amount of duplicate for the list to be played with...
					initPar.InitialPuActs[i].CoupleMOAct.StaticMovingTimeDuplicates.Add(new List<KeyValuePair<Act, int>>());
        }
      }
      for (int i = 0; i < moActsList.Count; ++i) {
				moActsList[i].StaticMovingTimeDuplicates = new List<List<KeyValuePair<Act, int>>>();
        for (int j = 0; j < Settings.NoOfThread; ++j)
					moActsList[i].StaticMovingTimeDuplicates.Add(new List<KeyValuePair<Act, int>>()); //ensure that it has sufficient amount of duplicate for the list to be played with...
      }

      //Other initialization
      ClusterVar.JoinDoPuInWs = Settings.JoinDoPuInWs;
    }

    #region clustering
    private void initClusterVariables(int iterationBranchLimit, int searchDepth, int timePenaltyPerDepth,
      int globalInitialLevel, bool globalMinToBeUpdated = false, bool globalMinOfMinIncluded = false) {
      ClusterVar.MaxOfMinAccTime = int.MaxValue; //very important to reset the shared static value at the very beginning! of the search by multi-threading!
      if (globalMinOfMinIncluded)
        ClusterVar.MinOfMin = null;
      ClusterVar.IsMinOfMinToBeUpdated = globalMinToBeUpdated;
      ClusterVar.SearchCount = 0;
      ClusterVar.InitialLevel = globalInitialLevel;
      ClusterVar.IterationBranchLimit = iterationBranchLimit;
      ClusterVar.SearchDepth = searchDepth;
      ClusterVar.TimePenaltyPerDepth = timePenaltyPerDepth;
      ClusterVar.LeafDepth = globalInitialLevel + searchDepth;
    }

    private void initReducedClusterVariables(int iterationBranchLimit, int searchDepth, int globalInitialLevel) {
      ClusterVar.SearchCount = 0; //no harm at all...
      ClusterVar.InitialLevel = globalInitialLevel;
      ClusterVar.IterationBranchLimit = iterationBranchLimit;
      ClusterVar.SearchDepth = searchDepth;
      ClusterVar.LeafDepth = globalInitialLevel + searchDepth;
      ClusterVar.MaxOfMinAccTime = int.MaxValue;
    }

    private readonly object locker = new object();
    private void updateClusterSearchCount(int val) {
      //object thisLock = ClusterVar.SearchCount;
      lock (locker)
        ClusterVar.SearchCount = val;
    }

    private void updateClusterMaxOfMinAccTime(int newValue) {
      //object thisLock = ClusterVar.MaxOfMinAccTime;
      //lock (thisLock) //lock the current maxOfMinAccTime just before the update!
      lock (locker)
        if (newValue < ClusterVar.MaxOfMinAccTime) //only update is proven to be smaller at the time of reading!
          ClusterVar.MaxOfMinAccTime = newValue;
    }

    private void updateClusterMinOfMin(ActNode newNode) {
      lock (locker) {
        if (ClusterVar.MinOfMin == null && newNode != null) {
          //lock (newNode)
          ClusterVar.MinOfMin = newNode;
          return;
        }
        //object thisLock = ClusterVar.MinOfMin;
        //lock (ClusterVar.MinOfMin)
        if ((ClusterVar.MinOfMin.Level == newNode.Level && ClusterVar.MinOfMin.AccMoveTime > newNode.AccMoveTime) ||
          newNode.Level > ClusterVar.MinOfMin.Level) //higher level can always update this!
          ClusterVar.MinOfMin = newNode;
      }
    }    
    #endregion

    private void branchEvaluatorFunction(object branchEvaluatorObj) {
      Evaluator branchEvaluator = branchEvaluatorObj as Evaluator;
      branchEvaluator.SearchByItself();
    }

    private void createAndAttachChildNodeToParent(ActNode parentNode, Act act, int accTime) {
      ++evCount.EvaluatedNodes;
      ++evCount.CreatedNodes;
      ActNode childNode = new ActNode(act, accTime);
      parentNode.Nodes.Add(childNode);
      childNode.GeneratePuMOActUnfinished();
      evaluatorNodes.Add(childNode);
    }

    private void searchMainNode() {
      currentlyEvaluatedBranchCode = "M";
      currentIterationBranchLimit = Settings.NewIterationBranch;
      iterationDt = DateTime.Now;
      currentBranchDepth = mainNode.Level;
      --currentDepthDifference; //to decrease the current depth difference by 1 for the first time! this is going to be the search depth!            
      evaluatorNodes = new List<ActNode>();
      List<Act> smtKeysLeftOver = mainNode.Act.StaticMovingTimeOriginal.Select(x => x.Key).ToList();
      List<Act> moActLegalList = mainNode.GenerateOnBoardLegalMoves(Settings.JoinDoPuInWs, smtKeysLeftOver, new List<Act>()); //no parents
      for (int i = 0; i < moActLegalList.Count; ++i)
        createAndAttachChildNodeToParent(mainNode, moActLegalList[i], mainNode.Act.StaticMovingTimeOriginal.Find(x => x.Key == moActLegalList[i]).Value);      
      prepareEvaluatorNodesForThreading(1);
      initClusterVariables(currentIterationBranchLimit, currentDepthDifference, timePenaltyPerDepth, evaluatorNodes[0].Level, noOfActiveThread == 1);
      if (noOfActiveThread > 1) { //only corrects for multithreading, and the minimum depth is correct, otherwise go for single thread
        searchRouteInEvaluatorNodesByMultiThreading();
      } else { //single thread, the safest...
        branchEvaluator[0] = new Evaluator(0, mainNode, moActsList, branchEvCount[0], ClusterVar); //always use no 0...
        branchEvaluator[0].GenerateStaticMovingTimeDuplicate(mainNode, new List<Act>()); //nothing in it...
        branchEvaluator[0].InitLocalVariables(); //at this state, the old branch evalator exists, having search solution etc...
        searchRouteInEvaluatorNodesBySingleThread();
      }
    }

    private void searchParentBranches(List<ActNode> parentBranches, int limit) {
      currentIterationBranchLimit = limit;
      searchSolution.Clear(); //clear the search solution before hand, always!
      currentBranchDepth = parentBranches != null && parentBranches.Count > 0 ? parentBranches[0].Level : 0;
      evaluatorNodes = new List<ActNode>(parentBranches); //evaluator is not reset before reuse!
      iterationDt = DateTime.Now;
      prepareEvaluatorNodesForThreading(0);
      initReducedClusterVariables(currentIterationBranchLimit, currentDepthDifference, completedDepth + threadNodeExtraDepth);
      for (int i = 0; i < noOfActiveThread; ++i) //very important after first iteration!
        if (branchEvaluator[i] != null)
          branchEvaluator[i].InitLocalVariables(); //at this state, the old branch evalator exists, having search solution etc...
      if (isMultithreadingActive()) {
        searchRouteInEvaluatorNodesByMultiThreading();
      } else { //single thread, the safest...        
        searchRouteInEvaluatorNodesBySingleThread();
      }
    }

    public void Search(SearchArgInitPar initPar) { //To start the searching!
      initSearch(initPar);
      do { //outer loop is controlled by sessionDepth <= usedFinalDepth, sessionDepth will increase 1 by 1 per outer loop, 
           //outer loop means it searches the solution with search depth 1 (to reach totalDepth - that is, all MO acts) 
        //and then increases it to 2, 3, 4 until usedFinalDepth
        //the reason for this outer loop is such that at any point of time there will always be solution available on request (since search depth 1 could be easily found)
        initOuterIteration(); //completedDepth is reset to 0 here

        //Inner loop, on the other hand, will always go from 0 until it reaches totalDepth - 1, 
        //it will search the solution till the total depth (the whole actions) is reached, using the search depth provided by outer loop each time       
        while (completedDepth < totalDepth) { //Inner Loop
          initInnerIteration();

          if (searchSolution.Count <= 0) { //the first time, the search solution is empty, main node branch = 0
            searchMainNode(); //Search solution is the global var used to retain the search data all the way
            newSearchSolution = new List<ActNode>(searchSolution.OrderBy(x => x.AccMoveTime)); //get the new search solution from search solution
          } else { //the subsequent time, there are already search solution so far... only applicable for tree search, but no need to put the condition here because tree search condition is implicitly known
            newParentBranches = new List<ActNode>();
            oldParentBranches = new List<ActNode>();

            //Solution is a list of ActNode -> List<ActNode>, which is actual a set of solutions, since each ActNode has a List<Act> 
            // each ActNode carrying an accummulated move time (AccMoveTime) with it which tells how long it will take to complete
            // the whole given tasks if the solution presented (or rather, chained) in the ActNode is followed
            //If there is already solution previously, then we have some lead to start searching for the new solution as follow:
            //1. The final solution (which is a set of options), which will likely be the best candidate for improvement in the next inner iteration
            //2. The final candidates found while trying to find the final solution in the previous inner iteration

            if (Settings.IncludeFinalSolution)//put the final solution first (even before final candidates) to speed up the searching
              includeFinalSolution(Settings.FinalSolutionIncludedInNewBranch ? newParentBranches : oldParentBranches, //inclusion in the new branch/old branch
                Settings.FinalSolutionIncludedInNewBranch || oldSearchSolution.Count <= 0 ? newSearchSolution : oldSearchSolution); //old branch inclusion, first time/subsequent time

            if (Settings.IncludeFinalCandidates)
              if (oldSearchSolution.Count <= 0) //if old search does not have anything, this is definitely the first time (no matter if the old parent already has something from the final solution)
                includeFinalCandidates(oldParentBranches, newSearchSolution); //seek the candidates which qualify to be the old parents
              else if (!Settings.FinalCandidatesIncludedOnlyOnce)//old search solution has provided something //either: (1) The solution will be immediately used (cease the non-surviving old branch), or 
                includeFinalCandidates(oldParentBranches, oldSearchSolution);// (2) force the old branch still to be included in the latest search...
              
            if (oldSearchSolution.Count > 0) //at any point, if the old search solution is ready, it deserves to be put to the old parent
              oldParentBranches.AddRange(oldSearchSolution.ToArray());

            newParentBranches.AddRange(newSearchSolution.ToArray());
            currentlyEvaluatedBranchCode = "N";
            searchParentBranches(newParentBranches, Settings.NewIterationBranch); //Get the new search solution...
            newSearchSolution = new List<ActNode>(searchSolution.OrderBy(x => x.AccMoveTime)); //get the new search iteration from the result of the search solution

            if (oldParentBranches.Count > 0) { //if by now there is a valid oldParent solution, then we can do something...
              if (noOfActiveThread > 1){
                abortAllActiveThreads();
                destroyAllEvaluators();
              }
              initInnerIteration(); //this is added because of strange depth it is produced otherwise
              currentlyEvaluatedBranchCode = "O";
              searchParentBranches(oldParentBranches, Settings.OldIterationBranch); //Get the new search solution...
              oldSearchSolution = new List<ActNode>(searchSolution.OrderBy(x => x.AccMoveTime)); //get the new search iteration from the result of the search solution
            }
          }
          updateInnerIteration();
        }
        updateOuterIteration();
      } while (sessionDepth <= usedFinalDepth);
      stopDt = DateTime.Now;
    }

    private void prepareEvaluatorNodesForThreading(int startingThreadNodeExtraDepth) {
      bool isMultithreading = isMultithreadingActive();
      noOfActiveThread = isMultithreading ? Settings.NoOfThread : 1;
      //MDD = Minimum depth difference. to "byFactor"
      threadNodeExtraDepth = startingThreadNodeExtraDepth; //thread sleep may not be useful at all if the evaluation speed per thread is much less than the sleep
      applyThreadSleep = currentDepthDifference > Settings.MDDToByFactor + Settings.MDDExtraToApplyThreadSleep;
      double leftOverDepthFraction = (double)(totalDepth - evaluatorNodes[0].Level) / totalDepth;
      double scaledAverageBranchingFactor = leftOverDepthFraction * averageBranchingFactor; //so the branching factor is scaled down here...
      double leftOverComputationPerThread = ((1 - Settings.AbortionFactor) / noOfActiveThread) * Math.Pow(scaledAverageBranchingFactor, currentDepthDifference); //by this we can know how much computation is left
      double byFactorCandidate = Math.Log(leftOverComputationPerThread / Settings.MaxNodesEvalSpeed);
      double byFactorOf = Settings.AutoByFactorOf ? (applyThreadSleep ?
        Math.Max(1, Math.Min(RouteFinderSettings.MaxByFactorOf, byFactorCandidate)) : 1) //auto but not able to pass the thread sleep will result in 1
        : Settings.ByFactorOf; //do something for getting the right "by factor of"
      while (evaluatorNodes.Count < (noOfActiveThread * byFactorOf) &&
        currentDepthDifference > Settings.MDDToByFactor) { //do something to increase the nodes count, till the task can be split to all threads or no more can be done..
        List<ActNode> branchNodes = new List<ActNode>(evaluatorNodes); //capture the current nodes
        --currentDepthDifference; //go deeper
        ++threadNodeExtraDepth; //more extra depth
        evaluatorNodes.Clear(); //to ensure that the nodes count is cleared before re-used later
        for (int i = 0; i < branchNodes.Count; ++i) { //for each branch node
          List<Act> smtKeys = branchNodes[i].Act.StaticMovingTimeOriginal.Select(x => x.Key).ToList();
          List<Act> moActLegalList = branchNodes[i].GenerateOnBoardLegalMoves(Settings.JoinDoPuInWs, smtKeys, branchNodes[i].GenerateMoActsListWithoutNode0()); //it can contain no legal move but it cannot have null Act!
          for (int j = 0; j < moActLegalList.Count; ++j) { //for each legal move generated by the branchNode
            KeyValuePair<Act, int> smt = branchNodes[i].Act.StaticMovingTimeOriginal.Find(x => x.Key == moActLegalList[j]);
            int accTime = branchNodes[i].AccMoveTime + smt.Value;            
            createAndAttachChildNodeToParent(branchNodes[i], smt.Key, accTime);
          }
        }
      }
      currentNodeProcessedIndex = 0; //initialized as -1
      evaluatorNodeCountRecord = evaluatorNodes.Count;
    }

    private void searchRouteInEvaluatorNodesByMultiThreading() { //given a set of nodes, do the multithreading!
      while (currentNodeProcessedIndex < evaluatorNodeCountRecord) { //incomplete... process the nodes one by one        
				List<int> freeBranchEvaluatorIndexes = getFreeBranchEvaluatorIndexes();
					//branchEvaluator == null || branchEvaluator.Length <= 0 ? null :
					//branchEvaluator.Where(x => x == null || !x.IsEvaluating)
					//	.Select(x => Array.IndexOf(branchEvaluator,x)).ToList();
			  if (freeBranchEvaluatorIndexes != null && freeBranchEvaluatorIndexes.Count > 0 && currentNodeProcessedIndex < evaluatorNodeCountRecord) { //do something will each free evaluator...          
          for (int i = 0; i < freeBranchEvaluatorIndexes.Count; ++i) {
            if (updateNextEvaluableNodeIndex()) {
              processEvaluatorData(freeBranchEvaluatorIndexes[i]);
              startEvaluator(freeBranchEvaluatorIndexes[i], evaluatorNodes[currentNodeProcessedIndex]); //increase on evaluation for next check
              ++currentNodeProcessedIndex;
            }
            if (currentNodeProcessedIndex >= evaluatorNodeCountRecord)
              break; //means it is finished actually...
          }
        }
        if (applyThreadSleep)
          Thread.Sleep(10); //not sure if this is the best idea...
      } //at this point all nodes must have been processed, except the remaining ones...      
      while (!evaluationsAreAllCompleted())
        if (applyThreadSleep)
          Thread.Sleep(10);
      for (int i = 0; i < noOfActiveThread; ++i)
        processEvaluatorData(i);        
      destroyBarrenEvaluatorNodes();
    }

    private void searchRouteInEvaluatorNodesBySingleThread() {
      while (currentNodeProcessedIndex < evaluatorNodeCountRecord) //incomplete... process the nodes one by one        
        if (updateNextEvaluableNodeIndex()) {
          branchEvaluator[0].AssignParentMOActs(evaluatorNodes[currentNodeProcessedIndex].GenerateMoActsListWithoutNode0());
          branchEvaluator[0].Search(evaluatorNodes[currentNodeProcessedIndex]); //must produce all the necessary search solution!
          ++currentNodeProcessedIndex;
        }
      searchSolution = new List<ActNode>(branchEvaluator[0].SearchSolution);
    }

    #region threading calls
    private bool updateNextEvaluableNodeIndex() {
      bool isEvaluable = false;
      do {
        int accTime = evaluatorNodes[currentNodeProcessedIndex].AccMoveTime;
        int timePenalty = timePenaltyPerDepth * currentDepthDifference;
        isEvaluable = isSelectable(accTime, timePenaltyPerDepth); //test
        if (!isEvaluable) {
          ++currentNodeProcessedIndex; //stays when not evaluatable
          ++evCount.AbortedNodes; //counted as aborted node, however... not destroyed yet! TODO
        }
      } while (!isEvaluable && currentNodeProcessedIndex < evaluatorNodeCountRecord);
      return isEvaluable;
    }

    private bool isSelectable(double accTime, double timePenalty) { //if there is no current max of min, or the solution is less than 100
      double totalTime = accTime + timePenalty;
      return searchSolution.Count < currentIterationBranchLimit || //if there is space, then it is selectable...
        totalTime < ClusterVar.MaxOfMinAccTime; //if it surpass the global limit is used:if it surpass the local limit is used, very slow!
    }

    private bool evaluationsAreAllCompleted() { //to check all evaluation
      for (int i = 0; i < noOfActiveThread; ++i) //somehow, this checking is incorrect...
        if (branchEvaluator[i] != null && branchEvaluator[i].IsEvaluating && !branchEvaluator[i].IsCompleted) //can be null if never declared..
          return false;
      return true;
    }

    private void destroyBarrenEvaluatorNodes() {
      int evaluated = 0;
      int originalEvaluatorNodesCount = evaluatorNodes.Count; //maybe duplicate
      int currentEvaluatedIndex = 0;
      while (evaluated < originalEvaluatorNodesCount) {
        evaluated++;
        int count = evaluatorNodes[currentEvaluatedIndex].GetNodeCount(false);
        if (count > 0)
          currentEvaluatedIndex++;
        else {
          int destroyedNodes = evaluatorNodes[currentEvaluatedIndex].Destroy();
          evaluatorNodes.RemoveAt(currentEvaluatedIndex);
          evCount.DestroyedNodes += destroyedNodes;
        }
      }
    }

    private List<int> getFreeBranchEvaluatorIndexes() {
      List<int> freeBranchEvaluatorIndexes = new List<int>();
      for (int i = 0; i < noOfActiveThread; ++i)
        if (branchEvaluator[i] == null || !branchEvaluator[i].IsEvaluating)
          freeBranchEvaluatorIndexes.Add(i);
      return freeBranchEvaluatorIndexes; //assuming there is something...
    }

    private void destroyExcessiveWeakNodes(int levelLimit) {
      while (searchSolution.Count > currentIterationBranchLimit) {
        ActNode node = searchSolution[searchSolution.Count - 1];
        searchSolution.Remove(node);
        int destroyedNodes = node.Destroy(levelLimit);
        evCount.DestroyedNodes += destroyedNodes; //also must be limited to the searchSolution level...
      }
    }

    private bool isMultithreadingActive() {
      return Settings.IsMultithreading && sessionDepth >= Settings.MultithreadMinimumDepth;
    }

    private void processEvaluatorData(int no) {
      if (no >= noOfActiveThread || branchEvaluator[no] == null ||
        !branchEvaluator[no].IsCompleted)
        return; //fail
      Evaluator evaluator = branchEvaluator[no];
      if (branchEvaluator[no].SearchSolution.Count <= 0)
        return;
      evaluator.SearchSolution = evaluator.SearchSolution.OrderBy(x => x.AccMoveTime).ToList();
      if (searchSolution.Count == 0) { //nothing inside
        searchSolution = new List<ActNode>(evaluator.SearchSolution);
      } else if (evaluator.SearchSolution != null && evaluator.SearchSolution.Count > 0) {
        if (searchSolution.Count >= currentIterationBranchLimit && //only if we are at the limit, we check the qualification!
          evaluator.CurrentMinOfMinAccTime > ClusterVar.MaxOfMinAccTime) { //destroy all its search solution... if not qualified at all...
          evaluator.DestroySearchSolutions(evaluator.SearchSolution.Count);
        } else {
          //if(searchSolution.Count < currentIterationBranchLimit ||
          //  evaluator.CurrentMinOfMinAccTime <= ClusterVar.MaxOfMinAccTime) { 
          searchSolution.AddRange(evaluator.SearchSolution);
          searchSolution = searchSolution.OrderBy(x => x.AccMoveTime).ToList();
          destroyExcessiveWeakNodes(ClusterVar.InitialLevel);
          updateClusterMaxOfMinAccTime(searchSolution[searchSolution.Count - 1].AccMoveTime);
        }
      }
      updateClusterSearchCount(searchSolution.Count); //such that each evaluator can estimate how many search results it can give at most to global search...
      updateClusterMinOfMin(searchSolution[0]); //if this becomes incorrect, means bug... best to process global min of min in the solution itself... but things get a bit tricky here, because there is possibility that the global solution minimum is reset when request is made!
      lock (locker) {
        globalMinOfMinMoActsListCopy = ClusterVar.MinOfMin.GenerateMoActsListWithoutNode0();
        globalMinOfMinAccTimeCopy = ClusterVar.MinOfMin.AccMoveTime;
      }
    }

    private void startEvaluator(int no, ActNode node) {
      if (no >= noOfActiveThread)
        return; //fail
      List<Act> parentMoActsList = node.GenerateMoActsListWithoutNode0();
      branchEvaluator[no] = new Evaluator(no, node, moActsList.FindAll(x => !parentMoActsList.Contains(x)).ToList(), branchEvCount[no], ClusterVar); //always makes the IsCompleted becomes false! (first time)
      branchEvaluatorThread[no] = new Thread(branchEvaluatorFunction);
      branchEvaluatorThread[no].Name = "Evaluator " + no + " " + ParentId;
      branchEvaluatorThread[no].Start(branchEvaluator[no]);
    }
    #endregion

    #region iteration, candidatures, solutions
    private void initInnerIteration() {
      currentDepthDifference = Math.Min(sessionDepth, totalDepth - completedDepth); //started by sessionDepth or by distance from the total depth, whichever is minimum
      threadNodeExtraDepth = 0;
    }

    private void initOuterIteration() { //every depth increase      
      globalMinOfMinMoActsListCopy.Clear();
      globalMinOfMinAccTimeCopy = int.MaxValue;
      searchSolution = new List<ActNode>();
      oldSearchSolution = new List<ActNode>(); //new for every new depth
      newSearchSolution = new List<ActNode>();
      completedDepth = 0;
      currentSessionDepth = sessionDepth;
      iterationNo = 1;
      ClusterVar.MinOfMin = null; //only during the outer initialization (to go deeper) that this globalMinOfMin can be nulled, not before!
    }

    private void updateInnerIteration() {
      completedDepth += sessionDepth;
      if (completedDepth >= totalDepth)
        completedDepth = totalDepth;
      else
        ++iterationNo;
      if (noOfActiveThread > 1) { //multi-threading
        abortAllActiveThreads();//we have to abort all threads! to avoid complication of the running threads... and previous data
        destroyAllEvaluators();
      }
    }

    private void updateOuterIteration() { //The final solution now is proven to be definitely in the new and old solution...      
      searchSolution = new List<ActNode>(newSearchSolution); //new solution is always part of the solution
      if (oldSearchSolution.Count > 0 && oldSearchSolution[0].Level == newSearchSolution[0].Level) //to avoid premature termination of the old search be included...
        searchSolution.AddRange(oldSearchSolution.ToArray()); //only then the old solution has chance to be included...
      searchSolution = searchSolution.OrderBy(x => x.AccMoveTime).ToList(); //for the last time!
      updateFinalSolution();
      ++sessionDepth; //increase session depth for the next iteration...
    }

    private void includeFinalCandidates(List<ActNode> parentSolution, List<ActNode> existingSolution) {
      if (finalCandidates == null || finalCandidates.Count <= 1) //If there is only 1 final candidate, it means it is the best solution...
        return;
      int level = existingSolution[0].Level;
      bool isDuplicate = false;
      for (int i = 0; i < finalCandidates.Count; ++i) { //retaining all the candidates at all time may not really be a best solution because they mess up things!
        if (i == finalCandidateBestSolutionIndex) //always skip final candidate which is also best solution
          continue;
        isDuplicate = false;
        ActNode finalCandidateBranch = finalCandidates[i].GetAncestor(level);
        for (int j = 0; j < existingSolution.Count; ++j)
          if (finalCandidateBranch.IsIdenticalSequence(existingSolution[j])) {
            isDuplicate = true;
            break;
          }        
        if (!isDuplicate && finalCandidateBranch != null)
          parentSolution.Add(finalCandidateBranch);        
      }
    }

    private void includeFinalSolution(List<ActNode> parentSolution, List<ActNode> existingSolution) {//The best solution is worth retaining... together with the main branches
      if (finalSolution == null)
        return;
      int level = existingSolution[0].Level;
      bool isDuplicate = false;
      ActNode finalCandidateBranch = finalSolution.GetAncestor(level);
      for (int j = 0; j < existingSolution.Count; ++j)
        if (finalCandidateBranch.IsIdenticalSequence(existingSolution[j])) {
          isDuplicate = true;
          break;
        }      
      if (!isDuplicate)
        parentSolution.Add(finalCandidateBranch);              
    }

    private void markFinalSolution() {
      for (int i = 0; i < finalCandidates.Count; ++i)
        if (finalCandidates[i].IsIdenticalSequence(finalSolution)) {
          finalCandidateBestSolutionIndex = i;
          break;
        }
    }

    private void sortAndLimitFinalCandidates() {
      finalCandidates = finalCandidates.OrderBy(x => x.AccMoveTime).ToList();
      if (!Settings.OnlyBestFinalCandidates || finalCandidates.Count <= Settings.OnlyBestFinalCandidatesNo + 1) //+1 is for the champion...
        return;
      finalCandidates.RemoveRange(Settings.OnlyBestFinalCandidatesNo + 1, finalCandidates.Count - Settings.OnlyBestFinalCandidatesNo - 1);
    }

    private void updateFinalCandidates() {
      int minAccTime = searchSolution[0].AccMoveTime;
      List<ActNode> includedFinalCandidates = searchSolution.TakeWhile(x => x.AccMoveTime <= minAccTime).ToList();
      if (finalCandidates.Count <= 0) {
        finalCandidates.AddRange(includedFinalCandidates.ToArray());
        sortAndLimitFinalCandidates();
        markFinalSolution();
        return;
      }
      for (int i = 0; i < includedFinalCandidates.Count; ++i) {
        bool isDuplicate = false;
        for (int j = 0; j < finalCandidates.Count; ++j)
          if (includedFinalCandidates[i].IsIdenticalSequence(finalCandidates[j])) {
            isDuplicate = true;
            break;
          }
        if (!isDuplicate)
          finalCandidates.Add(includedFinalCandidates[i]);
      }
      sortAndLimitFinalCandidates();
      markFinalSolution();
    }

    private void updateFinalSolution() {
			if (searchSolution == null || searchSolution.Count < 1) //FIXME, temporarily put this check because searchSolution in index 0 can be non-existing
				return;
      ActNode finalSolutionCandidate = searchSolution[0]; //The final solution always come from the search solution
      if (finalSolution == null || finalSolutionCandidate.AccMoveTime < finalSolution.AccMoveTime) { //If the final solution condidate is better the old one, preserve it...
        finalSolution = finalSolutionCandidate;
        bestSolvedDepth = sessionDepth;
      }      
      solvedDepth = sessionDepth; //record the session depth to the solved depth
      if (Settings.IncludeFinalCandidates)
        updateFinalCandidates();
      currentBranchDepth = finalSolutionCandidate.Level;
      currentlyEvaluatedBranchCode = "F";
      threadNodeExtraDepth = 0;
    }
    #endregion

    #region string, settings, performance collections
    private EvaluationCount getAllEvCount() {
      EvaluationCount allEvCount = new EvaluationCount();
      for (int i = 0; i < noOfActiveThread; ++i)
        allEvCount.Add(branchEvCount[i].Clone());
      //what happens when does not use clone? Does not seem to be adding anything to performance
      //allEvCount.Add(branchEvCount[i]); 
      return allEvCount;
    }

    public RouteFinderState GetState() {
      RouteFinderState state = new RouteFinderState();      
      state.Performance = getPerformance();
      state.Solution = getFinalSolution(out state.SourceCode);
      state.BestSolvedDepth = bestSolvedDepth;
      state.SolvedDepth = solvedDepth;
      state.TotalDepth = totalDepth;
      state.AverageBranchingFactor = averageBranchingFactor;
      //What happens if we remove total memory? Does not seem to be adding anything to performance
      //state.TotalMemoryMB = 0; 
      state.TotalMemoryMB = 0.000001 * GC.GetTotalMemory(false);
      state.DateTimeStamp = DateTime.Now;
      return state;
    }

    public string GetClosingString() {
      RouteFinderPerformance pf = getPerformance();
      string sourceCode = "";
      KeyValuePair<List<Act>, int> moActFinalSolution = getFinalSolution(out sourceCode);
      string routeMsg = "D" + bestSolvedDepth + "|" + solvedDepth + "|" + sourceCode + "|" + moActFinalSolution.Key.Count;
      string routeStr = "";
      for (int i = 0; i < moActFinalSolution.Key.Count; ++i)
        routeStr += moActFinalSolution.Key[i].GetString() + " ";
      string msg = "Time lapse: " + pf.TotalTimeLapse.ToString("N0") + " ms, " +
        Settings.GetString() +
        Environment.NewLine;
      msg += "AccTime: " + (0.1 * moActFinalSolution.Value).ToString("N1") + ", " + "Evaluated [L/N]: " + pf.EvCount.EvaluatedLeaves.ToString("N0") +
        "/" + pf.EvCount.EvaluatedNodes.ToString("N0") + ", Aborted: " + pf.EvCount.AbortedNodes.ToString("N0") +
        ", Speed: " + pf.LeafSpeed.ToString("N0") + " Leaves/s | " + pf.NodeSpeed.ToString("N0") + " Nodes/s" + Environment.NewLine;
      msg += "Best Route: [" + routeMsg + "] " + routeStr + Environment.NewLine;
      for (int i = 0; i < moActFinalSolution.Key.Count; ++i) {
        Act moAct = moActFinalSolution.Key[i];
        msg += "[" + moAct.MoNumber.ToString("d2") + "-" + (moAct.IsPickUp ? "PU]" : "DO]") +
          ", X: " + moAct.X.ToString("d2") + ", Z: " + (char)(moAct.Z + 'A') + ", S: " + (moAct.IsAirSide ? "A" : "L") +
          ", Cat: " + moAct.ULDCat.ToString() + Environment.NewLine;
      }
      return msg;
    }

    private RouteFinderPerformance getPerformance() {
      try {
        RouteFinderPerformance performance = new RouteFinderPerformance();
        EvaluationCount mainEvCount = evCount.Clone();
        EvaluationCount allBranchEvCount = getAllEvCount();
        long evNodes = mainEvCount.EvaluatedNodes + allBranchEvCount.EvaluatedNodes;
        long evLeaves = mainEvCount.EvaluatedLeaves + allBranchEvCount.EvaluatedLeaves;
        getAllTimeAndSpeed(evLeaves, evNodes, out performance.LeafSpeed, out performance.NodeSpeed, out performance.TotalTimeLapse, out performance.IterationTimeLapse);
        performance.EvCount.EvaluatedLeaves = mainEvCount.EvaluatedLeaves + allBranchEvCount.EvaluatedLeaves;
        performance.EvCount.EvaluatedNodes = evNodes;
        performance.EvCount.AbortedNodes = mainEvCount.AbortedNodes + allBranchEvCount.AbortedNodes;
        performance.EvCount.CreatedNodes = mainEvCount.CreatedNodes + allBranchEvCount.CreatedNodes;
        performance.EvCount.DestroyedNodes = mainEvCount.DestroyedNodes + allBranchEvCount.DestroyedNodes;
        performance.IterationNodesEvaluated = currentNodeProcessedIndex;
        performance.IterationNodes = evaluatorNodeCountRecord;
        performance.CurrentSessionDepth = currentSessionDepth;
        performance.CurrentIterationDepth = currentBranchDepth;
        performance.IterationNo = iterationNo;
        performance.IterationNodeExtraDepth = threadNodeExtraDepth;
        performance.EvaluatedBranchCode = currentlyEvaluatedBranchCode;
        return performance;
      } catch {
        return null;
      }
    }

    private void getAllTimeAndSpeed(long evLeaves, long evNodes, out double leafSpeed, out double nodeSpeed, out double totalTimeLapse, out double iterationTimeLapse) {
      if (startDt == initDt) { //not started
        leafSpeed = 0;
        nodeSpeed = 0;
        totalTimeLapse = 0;
        iterationTimeLapse = 0;
        return;
      }
      DateTime dt = stopDt == initDt ? DateTime.Now : stopDt;
      totalTimeLapse = (dt - startDt).TotalMilliseconds;
      nodeSpeed = (evNodes * 1000) / totalTimeLapse;
      leafSpeed = (evLeaves * 1000) / totalTimeLapse;
      iterationTimeLapse = iterationDt == initDt ? 0 : (dt - iterationDt).TotalMilliseconds;
    }

    public ActNode GetFinalNodeSolution() { //cannot get non-first time temporary solution
      try {
        if (finalSolution == null && ClusterVar.MinOfMin == null)
          return null;
        if (finalSolution == null)
          return ClusterVar.MinOfMin;
        return ClusterVar.MinOfMin.AccMoveTime < finalSolution.AccMoveTime && 
          ClusterVar.MinOfMin.Level >= finalSolution.Level ?
          ClusterVar.MinOfMin : finalSolution; 
      } catch { //if fail for any reason...
        return null;
      }
    }

		private KeyValuePair<List<Act>, int> getFinalSolution(out string sourceCode) {
      try {
        if (!isMultithreadingActive() &&
          ClusterVar.MinOfMin != null &&
          (globalMinOfMinAccTimeCopy > ClusterVar.MinOfMin.AccMoveTime || //either better node or longer node-chain
          ClusterVar.MinOfMin.Level > globalMinOfMinMoActsListCopy.Count)) {
          lock (locker) {
            globalMinOfMinMoActsListCopy = ClusterVar.MinOfMin.GenerateMoActsListWithoutNode0(); //actually, this needs not to be generated everytime...
            globalMinOfMinAccTimeCopy = ClusterVar.MinOfMin.AccMoveTime;
          }
        }
        if (finalSolution == null && (globalMinOfMinMoActsListCopy == null || globalMinOfMinMoActsListCopy.Count <= 0)) {
          sourceCode = "NA"; //no solution
					return new KeyValuePair<List<Act>, int>(null, 0);
        }
        if (finalSolution == null) {
          sourceCode = "GM";
					return new KeyValuePair<List<Act>, int>(globalMinOfMinMoActsListCopy, globalMinOfMinAccTimeCopy);
        } else if (globalMinOfMinMoActsListCopy != null && globalMinOfMinMoActsListCopy.Count == finalSolution.Level &&
          globalMinOfMinAccTimeCopy < finalSolution.AccMoveTime) {
          sourceCode = "GM";
          return new KeyValuePair<List<Act>, int>(globalMinOfMinMoActsListCopy, globalMinOfMinAccTimeCopy);
        }
        sourceCode = "FS";
        KeyValuePair<List<Act>, int> solution;
        lock (locker)
          solution = new KeyValuePair<List<Act>, int>(finalSolution.GenerateMoActsListWithoutNode0(), finalSolution.AccMoveTime); //only the last node is not returned...
        return solution;
      } catch { //if fail for any reason...
        sourceCode = "EX";
				return new KeyValuePair<List<Act>, int>(null, 0);
      }
    }
    #endregion

    #region termination methods
    public void Terminate() {
      abortAllActiveThreads();
      stopDt = DateTime.Now; //such that it will not give wrong impression that it is still active...
    }

    private void abortAllActiveThreads() {
      if (branchEvaluatorThread != null)
        for (int i = 0; i < branchEvaluatorThread.Length; ++i)
          if (branchEvaluatorThread[i] != null) {
            branchEvaluatorThread[i].Abort();
            branchEvaluatorThread[i] = null;
          }
    }

    private void destroyAllEvaluators() {
      if (branchEvaluator != null)
        for (int i = 0; i < branchEvaluator.Length; ++i)
          branchEvaluator[i] = null;
    }
    #endregion
  }

  public class RouteFinderState {
    public RouteFinderPerformance Performance;
    public KeyValuePair<List<Act>, int> Solution;
    public string SourceCode;
    public int BestSolvedDepth;
    public int SolvedDepth;
    public int TotalDepth;
    public double AverageBranchingFactor;
    public DateTime DateTimeStamp;
    public double TotalMemoryMB;
  }

  public class RouteFinderPerformance
  {
    public double TotalTimeLapse;
    public double IterationTimeLapse;
    public EvaluationCount EvCount = new EvaluationCount();
    public double LeafSpeed;
    public double NodeSpeed;
    public int CurrentSessionDepth;
    public int CurrentIterationDepth;
    public int IterationNo;
    public int IterationNodesEvaluated;
    public int IterationNodes;
    public int IterationNodeExtraDepth;
    public string EvaluatedBranchCode = "";
    public RouteFinderPerformance() {
    }
  }

  [Serializable()]
  public class RouteFinderSettings
  {
    //Limit variables
    public static int MaxNoOfThread = 24; //interestingly, const and static are incompatible!
    public static double MaxByFactorOf = 1000;

    //Settings
    public SearchMethod SMethod = SearchMethod.Tree_Search; //by default now is tree search
    public int StartDepth = 3; //The starting search depth
    public int FinalDepth = 9;
    public int OldIterationBranch = 50;
    public int NewIterationBranch = 100;
    public bool JoinDoPuInWs = false;

    //Boost Settings
    public bool IncludeFinalSolution = true;
    public bool FinalSolutionIncludedInNewBranch = true;
    public bool IncludeFinalCandidates = false;
    public bool FinalCandidatesIncludedOnlyOnce = false;
    public bool OnlyBestFinalCandidates = true;
    public int OnlyBestFinalCandidatesNo = 4;
    public bool IsSingleL = false;
		public bool FirstDropHasHighPriority;
		public bool FirstDropHasHighPriorityForMultipleUlds;
		public bool IsMultithreading = false;
    public int NoOfThread = 8;
    public double ByFactorOf = 5;
    public bool AutoByFactorOf = true;
    public int MultithreadMinimumDepth = 9;
    public int MDDToByFactor = 6; //for now, is not set from outside... but this should be set by simple text file
    public int MDDExtraToApplyThreadSleep = 2;
    public double MaxNodesEvalSpeed = 1000000; //assuming the conjunction is true...
    public double AbortionFactor = 0.75; //not known, but this is pretty decent assumption I believe...    

    public RouteFinderSettings() {
    }

    public string GetString() {
      string settingsString = "Tech: " + SMethod.ToString() + ", Depth: " + StartDepth + "-" + FinalDepth + ", Iteration Branch: " + OldIterationBranch.ToString("N0") + "/" + NewIterationBranch.ToString("N0");
      settingsString += ", Settings: " + (IncludeFinalSolution ? "R" + (FinalSolutionIncludedInNewBranch ? "N" : "O") : "NA");
      settingsString += "-" + (IncludeFinalCandidates ? "R" + (FinalCandidatesIncludedOnlyOnce ? "O" : "A") + (OnlyBestFinalCandidates ? "O" + OnlyBestFinalCandidatesNo.ToString("d2") : "X00") : "NA");
      settingsString += ", Thread: " + (IsMultithreading ? "M" + NoOfThread.ToString("d2") : "S01");
      settingsString += "-" + (IsMultithreading ? MultithreadMinimumDepth.ToString("d2") : "NA");
      settingsString += "-" + (IsMultithreading ? (AutoByFactorOf ? "A00-" + MDDToByFactor.ToString("d2") : "M" + ByFactorOf.ToString("d2") + "-00") + "-" + MDDExtraToApplyThreadSleep.ToString("d2") : "NA");
      return settingsString;
    }
  }

  public enum SearchMethod
  {
    Tree_Search,
    Brute_Force
  }

  public class SearchArgInitPar
  {
    public Act InitialAct;
    public List<Act> InitialPuActs;
    public List<Act> ActsList;
    public int TotalRemainingActs;
    public int TimePenaltyPerDepth;
    public double AverageBranchingFactor;
    public SearchArgInitPar(Act initialAct, List<Act> initialPuMOActUnfinished, List<Act> actsList, int minimumMoveTime, double averageBranchingFactor) {
      InitialPuActs = initialPuMOActUnfinished;
      InitialAct = initialAct;
      ActsList = actsList == null ? new List<Act>() : new List<Act>(actsList); //never "defile" the original list...
      TotalRemainingActs = ActsList.Count;
      TimePenaltyPerDepth = minimumMoveTime;
      AverageBranchingFactor = averageBranchingFactor;
    }
  }

  public class SearchArg
  { //Must be initiated with all the necessary items...
    public SearchArgInitPar InitPar;
    public RouteFinder RouteFinder;
    public SynchronizationContext SyncContext;
    public bool HasEnded = false;

    public SearchArg(SearchArgInitPar initPar, RouteFinder routeFinder, SynchronizationContext syncContext) {
      InitPar = initPar;
      RouteFinder = routeFinder;
      SyncContext = syncContext;
    }
  }
}
