using System;
using System.Collections.Generic;
using System.Linq;

namespace T5ShortestTime.MOAct {
  public class Evaluator
  {
    //Cluster
    private ClusterVariables clusterVar;

    //Current state
    private ActNode currentMaxOfMin = null;
    private List<int> destroyedNodesIndex = new List<int>();
    private List<Act> branchMoActsList = new List<Act>(); //this is to be filled by the giver!    

    //Solution Node
    public int CurrentMinOfMinAccTime = int.MaxValue;
    public List<ActNode> SearchSolution = new List<ActNode>();

    //Boost
    private List<Act> parentMoActs = new List<Act>(); //this is to ensure that parent MOActs are known "instantly" by the Node, without this, the performance drops!

    //Info
    public int EvaluatorNo = 0;

    //Performance measurement...
    private EvaluationCount evCount = new EvaluationCount();

    //Initial values
    private ActNode initialNode; //very important parameter to be evaluated...

    //Thread relating behavior
    public bool IsEvaluating;
    public bool IsCompleted;    

    //by giving iterationBranchLimit to 1, that already becomes brute search...
    public Evaluator(int evaluatorNo, ActNode initialNode, List<Act> branchMoActsList, EvaluationCount evCount, ClusterVariables clusterVar) {
      this.EvaluatorNo = evaluatorNo; //maybe useful later on...
      this.initialNode = initialNode;
      this.branchMoActsList = branchMoActsList == null ? new List<Act>() : new List<Act>(branchMoActsList); //never defile the original!
      this.evCount = evCount;
      this.clusterVar = clusterVar;
      IsEvaluating = false;
      IsCompleted = false;
    }

    //FIXME, may as well included the starting PU MOAct
    public void GenerateStaticMovingTimeDuplicate(ActNode startingNode, List<Act> excludedParentMoActs) {
      startingNode.Act.StaticMovingTimeDuplicates[EvaluatorNo] = startingNode.Act.StaticMovingTimeOriginal.FindAll(x => !excludedParentMoActs.Contains(x.Key));
      for (int i = 0; i < branchMoActsList.Count; ++i) //to create duplicates for all the static move time in the proper place of the list... (remember, moAct is shared!)        
        branchMoActsList[i].StaticMovingTimeDuplicates[EvaluatorNo] =
          branchMoActsList[i].StaticMovingTimeOriginal.FindAll(x => !excludedParentMoActs.Contains(x.Key));
    }

    public void SearchByItself() {
      IsEvaluating = true;
      GenerateStaticMovingTimeDuplicate(initialNode, initialNode.GenerateMoActsListWithoutNode0());
      Search(initialNode);
      localProcessEvaluatorData();
      IsEvaluating = false;
      IsCompleted = true;
    }

    private void localProcessEvaluatorData() {
      int globalSearchCountCopy = clusterVar.SearchCount; //to avoid global search count updated
      double globalMaxOfMinAccTimeCopy = clusterVar.MaxOfMinAccTime;
      if (globalSearchCountCopy >= clusterVar.IterationBranchLimit) { //if the global search count is already at the limit, we either destroy all the weak nodes of destroy some solutions..
        if (CurrentMinOfMinAccTime > globalMaxOfMinAccTimeCopy) //destroy all its search solution...
          DestroySearchSolutions(SearchSolution.Count);
        else
        //if (CurrentMinOfMinAccTime <= globalMaxOfMinAccTimeCopy)
          destroyWeakNodes();
      } 
      else if (CurrentMinOfMinAccTime > globalMaxOfMinAccTimeCopy) //we cannot destroy the weak nodes, but we can first check its currentMinOfMin, and destroy some parts which are exessive, have no hope of joined...
        DestroySearchSolutions(SearchSolution.Count - (clusterVar.IterationBranchLimit - globalSearchCountCopy)); //just destroy as much branch, no more...
    }

    public void DestroySearchSolutions(int no) { //cannot destroy things above its given parent... but then, somebody must do the job...
      if (SearchSolution.Count == 0) //so far, seems to be stable already...
        return;
      lock (locker) //FIXME this operation may probably be the solution which causes multi-threading to be very slow!
        SearchSolution = SearchSolution.OrderBy(x => x.AccMoveTime).ToList();
      int maxDestroyedNodes = SearchSolution.Count > 0 ? SearchSolution[0].Level - clusterVar.InitialLevel : 0;
      while (no > 0 && SearchSolution.Count > 0) {
        int destroyedNodes = SearchSolution[SearchSolution.Count - 1].Destroy(clusterVar.InitialLevel);
        SearchSolution.RemoveAt(SearchSolution.Count - 1);
        evCount.DestroyedNodes += destroyedNodes;
        --no;
      }
    }

    private void destroyWeakNodes() { //The find all could become troublesome!
      int globalMaxOfMinAccTimeCopy = clusterVar.MaxOfMinAccTime;
      if (SearchSolution == null || SearchSolution.Count <= 0)
        return;
      destroyedNodesIndex.Clear();
      for (int i = SearchSolution.Count - 1; i >= 0; --i)
        if (SearchSolution[i].AccMoveTime > globalMaxOfMinAccTimeCopy)
          destroyedNodesIndex.Add(i);
      int maxDestroyedNodes = SearchSolution[0].Level - clusterVar.InitialLevel;
      for (int i = 0; i < destroyedNodesIndex.Count; ++i) {
        int destroyedNodes = SearchSolution[destroyedNodesIndex[i]].Destroy(clusterVar.InitialLevel);
        SearchSolution.RemoveAt(destroyedNodesIndex[i]);
        evCount.DestroyedNodes += destroyedNodes;
      }
    }

    public void Search(ActNode moActNode) {
			//Sometimes moActNode.Level can be null
			if (moActNode == null)
				throw new NullActNodeException("Evaluator: " + EvaluatorNo);
      bool isLeaf = moActNode.Level >= clusterVar.LeafDepth;
      moActNode.IsBeingEvaluated = true;
      if (isLeaf) { //is time to have "static" evaluation
        processLeaf(moActNode);
      } else { //go deeper
        parentMoActs.Add(moActNode.Act); //branch needs to have parent MO acts added before the process (and then later removed afterwards)
        processBranch(moActNode);
        parentMoActs.RemoveAt(parentMoActs.Count - 1);
      }
      moActNode.IsBeingEvaluated = false; //After evaluation, the branch is no longer being evaluated...      
      if (moActNode.GetNodeCount(false) == 0 && !isLeaf) { //if this is not leaf node, destroy
        int destroyedNodes = moActNode.Destroy(clusterVar.InitialLevel);
        evCount.DestroyedNodes += destroyedNodes;
      }
    }

    private bool isSelectable(int accTime, int timePenalty) { //if there is no current max of min, or the solution is less than 100
      int totalTime = accTime + timePenalty;
      return currentMaxOfMin == null ||
        SearchSolution.Count < clusterVar.IterationBranchLimit || //if there is space, then it is selectable...
        totalTime < clusterVar.MaxOfMinAccTime; //if it surpass the global limit //if it surpass the local limit, very slow!
    }

    private void processLeaf(ActNode moActNode) {      
      ++evCount.EvaluatedLeaves;
      if (moActNode.AccMoveTime < CurrentMinOfMinAccTime) { //current min of min is simple
        CurrentMinOfMinAccTime = moActNode.AccMoveTime;
        if (clusterVar.IsMinOfMinToBeUpdated)
          updateClusterMinOfMin(moActNode);        
      }
      if (currentMaxOfMin == null || moActNode.AccMoveTime > currentMaxOfMin.AccMoveTime) {
        currentMaxOfMin = moActNode;
        updateClusterMaxOfMinAccTime(currentMaxOfMin.AccMoveTime);
      }
      SearchSolution.Add(moActNode);
      if (SearchSolution.Count > clusterVar.IterationBranchLimit) { //if the solution count is more than current branch iteration limit                
        int destroyedNodes = currentMaxOfMin.Destroy(clusterVar.InitialLevel);
        evCount.DestroyedNodes += destroyedNodes;
        SearchSolution.Remove(currentMaxOfMin);
        int maxVal = SearchSolution.Max(x => x.AccMoveTime);
        currentMaxOfMin = SearchSolution.First(x => x.AccMoveTime >= maxVal);
      }
    }

    private bool evaluateNode(int accTime, int timePenalty) {
      ++evCount.EvaluatedNodes;
      if (!isSelectable(accTime, timePenalty)) { //If this leaf node is not selectable, no need to evaluate further
        ++evCount.AbortedNodes;
        return false; //select the next node
      } //pass this, and the node is to be evaluated!        
      return true;
    }

    private ActNode createAttachToParentAndReturnChildNode(ActNode parentNode, Act act, int accTime) {
			if (act == null) {
				throw new NullActException("Parent Node: " + parentNode?.Act?.ToString() + "\n" 
					+ "Level: " + parentNode?.Level); //by this we can know if the parentNode or the Act is not null, then return it
			}
			++evCount.CreatedNodes; //TODO this is creation... could be dangerous...
      ActNode childNode = new ActNode(act, accTime); //If we get the legal time, then...?
      parentNode.Nodes.Add(childNode); //assumes it has the child node...
      childNode.GeneratePuMOActUnfinished(); //can only be done after it has parent...
      return childNode;
    }
    
    private void processBranch(ActNode moActNode) {
			//FIXME sometimes moActNode.Level can be null on multi-core
			if (moActNode == null)
				throw new NullActNodeException("Evaluator: " + EvaluatorNo);
			int timePenalty = clusterVar.TimePenaltyPerDepth * Math.Min(clusterVar.SearchDepth, clusterVar.LeafDepth - moActNode.Level);
      if (moActNode.Act.StaticMovingTimeDuplicates[EvaluatorNo].Count > 1) {
        List<Act> smtKeys = moActNode.Act.StaticMovingTimeDuplicates[EvaluatorNo].Select(x => x.Key).ToList();
        List<Act> moActLegalList = moActNode.GenerateOnBoardLegalMoves(clusterVar.JoinDoPuInWs, smtKeys, parentMoActs);
        for (int i = 0; i < moActLegalList.Count; ++i) { //for each legal move...
          KeyValuePair<Act, int> smt = moActNode.Act.StaticMovingTimeOriginal.Find(x => x.Key == moActLegalList[i]);
          int accTime = moActNode.AccMoveTime + smt.Value; //evaluate the time
          if (!evaluateNode(accTime, timePenalty))
            continue;
          if (smt.Key == null) //this may be caused by [on board legal moves] != [static move]
            continue;
					//if (smt.Key == null) { //TODO for checking purpose, remove when not needed
					//	string msg = "ParentAct: " + moActNode.Act.ToString() + 
					//		", Legal act(s) no: " + moActLegalList.Count.ToString() +
					//		", SMT no: " + moActNode.Act.StaticMovingTimeOriginal.Count.ToString(); //what act node the parent act currently is
					//	msg += "\n Legal Act(s):";
					//	for (int j = 0; j < moActLegalList.Count; ++j) { //check its SMT generated, whether it is really empty
					//		Act act = moActLegalList[j];
					//		msg += "\n  [" + j.ToString() + "] " + (act == null ? "NULL Act" : act.ToString());
					//	}
					//	msg += "\n SMTs:";
					//	for (int j = 0; j < moActNode.Act.StaticMovingTimeOriginal.Count; ++j) { //check its SMT generated, whether it is really empty
					//		Act act = moActNode.Act.StaticMovingTimeOriginal[j].Key;
					//		msg += "\n  [" + j.ToString() + "] " + (act == null ? "NULL Act" : act.ToString());
					//	}
					//	msg += "\n";
					//	throw new NullActException(msg);
					//}
          Search(createAttachToParentAndReturnChildNode(moActNode, smt.Key, accTime));
        }
      } else if (moActNode.Act.StaticMovingTimeDuplicates[EvaluatorNo].Count > 0) { //gives some pragmatic solution now...
        KeyValuePair<Act, int> smt = moActNode.Act.StaticMovingTimeDuplicates[EvaluatorNo][0];
        int accTime = moActNode.AccMoveTime + smt.Value; //evaluate the time
				if (evaluateNode(accTime, timePenalty)) {
          if (smt.Key != null) //this may be caused by [on board legal moves] != [static move]
          //if (smt.Key == null) { //TODO for checking purpose, remove when not needed
          //	string msg = "ParentAct: " + moActNode.Act.ToString() +
          //		", SMT no: " + moActNode.Act.StaticMovingTimeOriginal.Count.ToString(); //what act node the parent act currently is
          //	msg += "\n SMTs:";
          //	for (int j = 0; j < moActNode.Act.StaticMovingTimeOriginal.Count; ++j) { //check its SMT generated, whether it is really empty
          //		Act act = moActNode.Act.StaticMovingTimeOriginal[j].Key;
          //		msg += "\n  [" + j.ToString() + "] " + (act == null ? "NULL Act" : act.ToString());
          //	}
          //	msg += "\n";
          //	throw new NullActException(msg);
          //}
            Search(createAttachToParentAndReturnChildNode(moActNode, smt.Key, accTime));
				}
      }
    }

    public void InitLocalVariables() { //to be called whenever the searchSolution of this evaluator is no longer needed
      SearchSolution.Clear();
      currentMaxOfMin = null;
      CurrentMinOfMinAccTime = int.MaxValue;
    }

    public void ResetFlags() {
      IsEvaluating = false;
      IsCompleted = false;
    }

    public void ReassignInitialConditions(ActNode initialNode, List<Act> branchMoActsList) {
      this.initialNode = initialNode;
      this.branchMoActsList = branchMoActsList == null ? new List<Act>() : new List<Act>(branchMoActsList); //never defile the original!      
    }

    public void AssignParentMOActs(List<Act> externalParentMoActs) {
      parentMoActs = externalParentMoActs;
    }

    private readonly object locker = new object();
    private void updateClusterMaxOfMinAccTime(int newValue) {
      lock (locker)
        if (newValue < clusterVar.MaxOfMinAccTime) //only update is proven to be smaller at the time of reading!
          clusterVar.MaxOfMinAccTime = newValue;
    }

    private void updateClusterMinOfMin(ActNode newNode) {
      lock (locker) {
        if (clusterVar.MinOfMin == null && newNode != null) {
          clusterVar.MinOfMin = newNode;
          return;
        }
        if ((clusterVar.MinOfMin.Level == newNode.Level && clusterVar.MinOfMin.AccMoveTime > newNode.AccMoveTime) ||
          newNode.Level > clusterVar.MinOfMin.Level) //higher level can always update this!
          clusterVar.MinOfMin = newNode;
      }
    }

  }

  public class ClusterVariables {
    public int MaxOfMinAccTime = int.MaxValue; //be careful when reading or writing this value, TODO must be initialized outside!
    public ActNode MinOfMin = null;
    public int SearchCount = 0;
    public int IterationBranchLimit = 100;
    public int SearchDepth = 0; //to know if it is leaf node
    public int TimePenaltyPerDepth;
    public int InitialLevel = 0;
    public int LeafDepth = 2;
    public bool JoinDoPuInWs = false;
    public bool IsMinOfMinToBeUpdated = false; //automatically triggered by search used...
  }

  public class EvaluationCount
  {
    public int EvaluatedLeaves = 0;
    public long EvaluatedNodes = 0;
    public long AbortedNodes = 0;
    public long CreatedNodes = 0;
    public long DestroyedNodes = 0;

    public EvaluationCount() {
    }

    public EvaluationCount(EvaluationCount evCount) {
      EvaluatedLeaves = evCount.EvaluatedLeaves;
      EvaluatedNodes = evCount.EvaluatedNodes;
      AbortedNodes = evCount.AbortedNodes;
      CreatedNodes = evCount.CreatedNodes;
      DestroyedNodes = evCount.DestroyedNodes;
    }

    public void Reset() {
      EvaluatedLeaves = 0;
      EvaluatedNodes = 0;
      AbortedNodes = 0;
      CreatedNodes = 0;
      DestroyedNodes = 0;
    }

    public void Add(EvaluationCount evCount) {
      EvaluatedLeaves += evCount.EvaluatedLeaves;
      EvaluatedNodes += evCount.EvaluatedNodes;
      AbortedNodes += evCount.AbortedNodes;
      CreatedNodes += evCount.CreatedNodes;
      DestroyedNodes += evCount.DestroyedNodes;
    }

    public EvaluationCount Clone() {
      return new EvaluationCount(this);
    }
  }
}
