using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms; //for TreeNode

namespace T5ShortestTime.MOAct {
	public class NullActException : Exception {
		public NullActException() : base() {}
		public NullActException(string message) : base(message) {}
	}

	public class NullActNodeException : Exception {
		public NullActNodeException() : base() { }
		public NullActNodeException(string message) : base(message) { }
	}

	public class ActNode : TreeNode
  {
    public Act Act; //the MO act affiliated with this node...
    public int AccMoveTime = 0;
    public List<Act> PuMOActUnfinished = new List<Act>(); //TODO not sure if this is the best idea, but for now, leave it be...
    public bool IsBeingEvaluated = true;

    public ActNode(Act moAct, int accTime) {
			if (moAct == null)
				throw new NullActException();
      Act = moAct;
      AccMoveTime = accTime;
    }

    private List<Act> generateOnBoardLegalMoves(bool joinDoPu, List<Act> smtKeysLeftOver) {
      //Whatever is left over here, if there is anything which is still blocked (means parent is not there) cannot continue...
      List<Act> moActLegalList = ActHandler.GetLegalActs(PuMOActUnfinished, smtKeysLeftOver); //get what is legal according to the current condition
      if (moActLegalList.Count <= 1)
        return moActLegalList;
      if (joinDoPu && Act.WsJoinedActs.Count > 0) {
        Act act = moActLegalList.Find(x => Act.WsJoinedActs.Contains(x));
        if (act != null) //this is useless actually because the act can never be null due to the List.Find
          return new List<Act>() { act };
      }
      double maxPriority = moActLegalList.Max(x => x.EffectivePriority);
      moActLegalList.RemoveAll(x => x.EffectivePriority < maxPriority); //limit the legal list according to the priority
      return moActLegalList;
    }

    public List<Act> GenerateOnBoardLegalMoves(bool joinDoPu, List<Act> smtKeys, List<Act> parentMoActs) {
      if (smtKeys.Count <= 1)
        return smtKeys;
      smtKeys.RemoveAll(y => parentMoActs.Contains(y)); //can the left over be illegal? Should not be!
      if (smtKeys.Count <= 1)
        return smtKeys;
			//removes everything that has presiding act whose act is not yet done in the parent
			smtKeys.RemoveAll(x => x.PresidingAct != null && !parentMoActs.Contains(x.PresidingAct));
      if (smtKeys.Count <= 1)
        return smtKeys;
      return generateOnBoardLegalMoves(joinDoPu, smtKeys);
    }

    private readonly object locker = new object();
    public List<Act> GenerateMoActsListWithoutNode0() { //node 0 can never be included
      //lock (locker) {
        if (this.Level == 0)
          return new List<Act>();
        List<Act> moActsList = new List<Act>();
        ActNode node = this;
        while (node != null && node.Level > 0) {
          moActsList.Insert(0, node.Act);
          node = (ActNode)node.Parent;
        } //node level 0 does not need to be evaluated
        return moActsList;
      //}
    }

    public List<ActNode> GenerateMoActTreeNodes() {
      //lock (locker) {
        if (this.Level == 0)
          return new List<ActNode>();
        List<ActNode> moActTreeNodesList = new List<ActNode>();
        ActNode node = this;
        while (node.Level > 0) {
          moActTreeNodesList.Insert(0, node);
          node = (ActNode)node.Parent;
        } //node level 0 does not need to be evaluated
        return moActTreeNodesList;
      //}
    }

    public void FillMoActsListWithoutNode0(List<Act> moActsList) { //node 0 can never be included
      if (this.Level == 0)
        return;
      ActNode node = this;
      while (node.Level > 0) {
        moActsList.Insert(0, node.Act);
        node = (ActNode)node.Parent;
      } //node level 0 does not need to be evaluated
    }

    public void GeneratePuMOActUnfinished() { //WARNING: only for child! purposely the level check is skipped for performance boost!
      PuMOActUnfinished = new List<Act>(((ActNode)this.Parent).PuMOActUnfinished); //now, every childNode searched will have PuMOActUnfinished
			if (Act == null)
				throw new NullActException("ActNode GetNodeCount: " + this.GetNodeCount(false).ToString());
      if (Act.IsPickUp) //FIXME this Act can be surprisingly null! this happens even for single thread!!
        PuMOActUnfinished.Add(Act);
      else if (Act.CoupleMOAct != null) //not the original MOAct, the child MUST have the PU for this...
        PuMOActUnfinished.Remove(Act.CoupleMOAct);
    }

    public string GetString(bool accTimeIncluded = false) {
      return Act.GetString() + "\tLevel: " + Level.ToString() + (accTimeIncluded ? "\tAccTime: " + (0.1 * AccMoveTime).ToString("f1") : "");
    }

    private bool isIdenticalData(ActNode node) {
      return this.Act == node.Act && this.AccMoveTime == node.AccMoveTime && this.PuMOActUnfinished.SequenceEqual(node.PuMOActUnfinished);
    }

    public bool IsIdenticalSequence(ActNode node) {
      if (this == node) //definitely true
        return true;
      if (this.Level != node.Level || !isIdenticalData(node)) //definitely false
        return false;
      ActNode node1 = this;
      ActNode node2 = node;
      while (node1.Level > 0) {
        node1 = (ActNode)node1.Parent;
        node2 = (ActNode)node2.Parent;
        if (node1.Act != node2.Act)
          return false;
      }
      return node1.Act != node2.Act;
    }

    public ActNode GetAncestor(int level) {
      if (this.Level <= level)
        return this; //cannot go deeper than this
      ActNode node = this;
      while (node.Level > level)
        node = (ActNode)node.Parent;
      return node;
    }

    //public string GetChainString() {
    //  string chain = "";
    //  List<Act> moActsList = GenerateMoActsListWithoutNode0();
    //  for (int i = 0; i < moActsList.Count; ++i)
    //    chain += moActsList[i].GetString();
    //  return chain;
    //}

    public int Destroy(int levelLimit = 0) { //no, actually not only level 0 cannot be destroyed, the branch, if we destroy... could there be other branches keeping it...?
      //lock (locker) {
        int totalDestroy = 1;
        if (this.Level <= levelLimit) //cannot destroy node in the level limit, thus, level 0 can never be destroyed, its creation is not recorded too though...
          return 0;
        ActNode node = (ActNode)this.Parent;
        node.Nodes.Remove(this);
        levelLimit = Math.Max(0, levelLimit);
        while (node.Level > levelLimit && !node.IsBeingEvaluated) { //destroy anything below the level limit
          ActNode childNode = node;
          node = (ActNode)node.Parent;
          if (childNode.Nodes.Count == 0) {
            node.Nodes.Remove(childNode); //instead of removing itself, let the parent does it!
            ++totalDestroy;
          } else
            break; //once we cannot destroy, we should break, no need to check the parent for sure...
        }
        return totalDestroy;
      //}
    }
  }
}
