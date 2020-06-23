using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using T5ShortestTime.SATS;

namespace T5ShortestTime.MOAct {
  public class ActHandler
  {
    //MO Act collections    
    List<Act> actList = new List<Act>(); //this necessarily has list of MO acts

		//Time settings, Formula, X speed and Y speed
		public static GlobalActTimeSettings GlobalActTimeSettings { get; set; } = new GlobalActTimeSettings();
		public TimeSettings TimeSettings { get; set; } = new TimeSettings();

    //Initial positioning
    public Act InitialAct { get; set; } = new Act();
    public List<Act> InitialPuActs { get; set; } = new List<Act>(); //this must be made really dynamic (that is the "current" situation of the MoAct)

    //Boost
    private int initialLegalMoves;
    private int actListLegalMoves;
    public int CombinedLegalMoves { get { return initialLegalMoves + actListLegalMoves; } }
    public double AverageBranchingFactor { get { return (double) CombinedLegalMoves / (actList.Count + 1); } }

    //To support multiple equipments
    public string EquipmentId { get; set; } = "";

    public ActHandler() {
    }

    public string GetBasicActListString() {
      return string.Concat("MOActs = ", actList.Count,
        ", MOActsPu = ", actList.Where(x => x.IsPickUp).Count(),
        ", MOActsDo = ", actList.Where(x => !x.IsPickUp).Count());
    }

    public string GetMoActListString() {			
      return string.Concat(
				string.Join(
					Environment.NewLine, actList.Select(x => x.GetFullInfoAndSMTString(actList.IndexOf(x)))
					), 
				Environment.NewLine);
    }

		//public int GetTargetMoveTime() {

		//}

    public int GetDistanceMoveTime(Act fromMOAct, Act toMOAct) {			
      return Math.Max(TimeSettings.XExtraTimeIn100ms + Math.Abs(toMOAct.X - fromMOAct.X) * TimeSettings.XTimePerLaneIn100ms,
        TimeSettings.ZExtraTimeIn100ms + Math.Abs(toMOAct.Z - fromMOAct.Z) * TimeSettings.ZTimePerLevelIn100ms);
    }

    public int GetDistanceMoveTime(int x1, int z1, int x2, int z2) {
			return Math.Max(TimeSettings.XExtraTimeIn100ms + Math.Abs(x1 - x2) * TimeSettings.XTimePerLaneIn100ms,
        TimeSettings.ZExtraTimeIn100ms + Math.Abs(z1 - z2) * TimeSettings.ZTimePerLevelIn100ms);
    }

    private bool isJoinable(Act doAct, Act puAct) {
      if (!doAct.IsWorkstation || !puAct.IsWorkstation || doAct.IsPickUp || !puAct.IsPickUp) //can only join DO to PU in the WS
        return false;
      return Workstation.IsDoPuJoinable(doAct.LocString, puAct.LocString);
    }

    public static List<Act> GetLegalActs(List<Act> puActLeftList, List<Act> remaningActList) {
      if (puActLeftList.Count == 0)
        return remaningActList.FindAll(x => x.IsPickUp); //by now all the self must already been removed!
      else if (puActLeftList.Count == 1)
          return puActLeftList[0].ULDCat == ULDCategory.L ? //5 ft ULD
            GetSingleLTypeLegalActs(puActLeftList[0], remaningActList) : // 5 ft ULD is the most problematic one...
            new List<Act>() { puActLeftList[0].CoupleMOAct }; //Other than 5 ft ULD, this is easy, because only the couple is legal
      return GetDoubleLTypeLegalActs(puActLeftList[0], puActLeftList[1]); //The left over implies that there are two of the PU
    }

    private List<Act> getSingleLTypeLegalActs(bool isSingleL) {
      List<Act> list = InitialPuActs.Count == 1 ? GetSingleLTypeLegalActs(InitialPuActs[0], actList.FindAll(x => x != InitialPuActs[0])) : null;
      if (list != null && isSingleL) { //further process, delete all drop
        list.RemoveAll(x => x.IsPickUp); //all pick ups are illegal
        list.RemoveAll(x => !x.IsPickUp && x != InitialPuActs[0].CoupleMOAct); //all drop but dropping the current one is also removed...
      }
      return list;
    }

    public static List<Act> GetSingleLTypeLegalActs(Act puActUnfinished, List<Act> remaningMoActList) { //already assuming that 5 ft ULD is inside. puAct is the leftOver uld puAct
      return remaningMoActList.FindAll(x =>
        ((x.ULDCat == ULDCategory.L && x.IsPickUp && x != puActUnfinished) && //the L-Type ULD cat and to be picked is legal, self is always illegal here, except...
          !(puActUnfinished.CoupleMOAct.IsAirSide == x.IsAirSide && puActUnfinished.CoupleMOAct.IsAirSide != x.CoupleMOAct.IsAirSide)) || //when the ULD cat is in the target side and going to the other side!
        x == puActUnfinished.CoupleMOAct//or, its couple is always allowed         
        );
    }

    private List<Act> getInitDoubleLTypeLegalActs() { //assuming the poMoActUnfinished are two (only applicable when it is so)
      return InitialPuActs.Count >= 2 ? 
				GetDoubleLTypeLegalActs(InitialPuActs[0], InitialPuActs[1], true) : null;
    }

		public static List<Act> GetDoubleLTypeLegalActs(Act puMoActUnfinished1, Act puMoActUnfinished2, bool initCall = false) {
			//superceded by the blocking mo no, if there is any
			if (initCall) {
				if (puMoActUnfinished1.BlockingMoNo == puMoActUnfinished2.MoNumber) //1 is blocked by 2
					return new List<Act> { puMoActUnfinished2.CoupleMOAct }; //drop 2 first
				if (puMoActUnfinished2.BlockingMoNo == puMoActUnfinished1.MoNumber) //2 is blocked by 1
					return new List<Act> { puMoActUnfinished1.CoupleMOAct }; //drop 1 first
			}
			//return new List<Act> { puMoActUnfinished1.CoupleMOAct, puMoActUnfinished2.CoupleMOAct }; //then both MOAct will be legal!
			
			return puMoActUnfinished1.CoupleMOAct.IsAirSide != puMoActUnfinished2.CoupleMOAct.IsAirSide ? //as long as its couple shows different drop off points, it will be enough!								
				puMoActUnfinished1.EffectivePriority > puMoActUnfinished2.EffectivePriority ? //check the priority 
					new List<Act>() { puMoActUnfinished1.CoupleMOAct, puMoActUnfinished2.CoupleMOAct } : //then both MOAct will be legal! 
					new List<Act>() { puMoActUnfinished2.CoupleMOAct, puMoActUnfinished1.CoupleMOAct } : //TODO, also check the time distance between them
				puMoActUnfinished2.IsAirSide == puMoActUnfinished2.CoupleMOAct.IsAirSide ? //check where the pick up are from, the important one is the last one, and its couple-target, if it is the same, then drop it, otherwise drop the first one					
					new List<Act>() { puMoActUnfinished2.CoupleMOAct } :
					new List<Act>() { puMoActUnfinished1.CoupleMOAct };
		}

    public SearchArgInitPar CreateActSearchArgInitialParameter() { //this is a bit tricky...      
      return new SearchArgInitPar(InitialAct, InitialPuActs, actList, minimumMoveTime, AverageBranchingFactor);
    }

    private bool generateInitialLegalMoves(string eqpId, bool isSingleL, out string eStr) { //Must be generated later than the legal moves!
			eStr = "";

			if (actList.Count <= 0 && InitialPuActs.Count <= 0) { //cannot set initial position if there is no MOAct or unfinished MOAct in the first place //TODO may not be correct!
        InitialAct = null;
				eStr = "Cannot set initial position if there is no new MO Action(s) or unfinished MO Action in the first place.";
				return false;
      }      
      initialLegalMoves = 0;
      InitialAct.StaticMovingTimeOriginal.Clear(); //change2.9
			//InitialAct.StaticMovingTimeNew.Clear();

			InitialAct.WsJoinedActs.Clear();
      List<Act> tempMOActs;
			var allPu = actList.Where(x => x.IsPickUp);			

			switch (InitialPuActs.Count) {
        case 0: //ETV is free, it can pick any PU
					addActStaticMoveTimeWithJoinActs(InitialAct, allPu);
          break;
        case 1: //This is to say, the couple to drop MUST exist!
					if (InitialPuActs[0].ULDCat == ULDCategory.L) { //ETV is carrying 1 5ft ULD
						addActStaticMoveTimeWithJoinActs(InitialAct, getSingleLTypeLegalActs(isSingleL));
          } else { //ETV is carrying 1 10ft or 1 15ft ULD, the only possible things to do here is that to drop what is left behind...
						addActStaticMoveTime(InitialAct, InitialPuActs[0].CoupleMOAct);
          }
          break;
        case 2: //ETV is carrying 2 5ft ULDs
          tempMOActs = getInitDoubleLTypeLegalActs();
					addActStaticMoveTime(InitialAct, tempMOActs);
          if (isSingleL && tempMOActs.Count > 0) { //special treatment for the first possible move..
            Act act = tempMOActs[0];												
						act.StaticMovingTimeOriginal.RemoveAll(x => x.Key.IsPickUp); //the first move always cannot pick up, regardless if one or two moves remain..
            if (tempMOActs.Count > 1) {
              InitialAct.StaticMovingTimeOriginal.RemoveAt(1); //remove the second move (if there is any, forcing to drop the first one...)
              tempMOActs[0].StaticMovingTimeOriginal.RemoveAll(x => !x.Key.IsPickUp && x.Key != tempMOActs[1]); //remove all drop off which is not the second one...
              tempMOActs[1].StaticMovingTimeOriginal.RemoveAll(x => !x.Key.IsPickUp); //removes all drop off from the second move
            } else //even it only has one legal move now
              act.StaticMovingTimeOriginal[0].Key.StaticMovingTimeOriginal.RemoveAll(x => !x.Key.IsPickUp); //It has to be ensured that the next move will necessarily drop the other item before picking up any other...
          }
          break;
        default: //Unknown case
					eStr = "No initial Act(s): " + InitialPuActs.Count.ToString() + 
						". There must be between 0-2 Initial Acts in order for the initial legal moves to be generated. Please check your LEDINFO table, see if the number of ULDs on the " + 
						EquipmentId + " is between the specified numbers!";
					return false;
      }
      initialLegalMoves = InitialAct.StaticMovingTimeOriginal.Count;
      for (int i = 0; i < initialLegalMoves; ++i)
        InitialAct.StaticMovingTimeOriginal[i].Key.EqpId = eqpId; //otherwise, the initial legal moves doesn't know the ID... TODO not really neat, but leave it for now...
			if (moveTimes.Any())
				minimumMoveTime = moveTimes.Min();
      return true;
    }

		private void addActStaticMoveTime(Act act1, Act act2) {
			int moveTime = GetDistanceMoveTime(act1, act2);
			act1.StaticMovingTimeOriginal.Add(new KeyValuePair<Act, int>(act2, moveTime));
			moveTimes.Add(moveTime);
		}

		private void addActStaticMoveTime(Act act1, IEnumerable<Act> acts) {
			foreach (Act act2 in acts) {
				int moveTime = GetDistanceMoveTime(act1, act2);
				act1.StaticMovingTimeOriginal.Add(new KeyValuePair<Act, int>(act2, moveTime));
				moveTimes.Add(moveTime);
			}
		}

		private void addActStaticMoveTimeWithJoinActs(Act act1, IEnumerable<Act> acts) {
			addActStaticMoveTime(act1, acts);
			act1.WsJoinedActs.AddRange(acts.Where(puact => isJoinable(act1, puact))); //if any Act is joinable, join it
		}

		private int minimumMoveTime = int.MaxValue;
		List<int> moveTimes = new List<int>(); //collect all generated moveTime for finding global minimumMoveTime
		private bool generateActListLegalMoves(bool isSingleL) {
      if (actList == null || actList.Count <= 0)
        return false;
			var allPu = actList.Where(x => x.IsPickUp);
			var allDo = actList.Where(x => !x.IsPickUp);
			var allPuL = allPu.Where(x => x.ULDCat == ULDCategory.L);
			var allDoL = allDo.Where(x => x.ULDCat == ULDCategory.L);
			actListLegalMoves = 0;
			minimumMoveTime = int.MaxValue;
			moveTimes.Clear();

			//cannot pickup the blocked mono before the blocking one
			foreach (Act act in actList) {
				act.StaticMovingTimeOriginal.Clear();
				act.WsJoinedActs.Clear();
				
				if (act.IsPickUp) //pick up
					addActStaticMoveTime(act, act.CoupleMOAct);
				if (act.IsPickUp && act.ULDCat == ULDCategory.L && !isSingleL) { //pick up and 5 ft
					addActStaticMoveTime(act, allDoL.Where(x => x != act.CoupleMOAct)); //All 5ft ULD DO and not the couple
					addActStaticMoveTime(act, GetSingleLTypeLegalActs(act, allPuL.Where(x => x != act).ToList())); //All 5ft ULD PU and not itself
				}
				if (!act.IsPickUp) //drop off	
					addActStaticMoveTimeWithJoinActs(act, 
						allPu.Where(x => x != act.CoupleMOAct && //every pick up that is not its couple							
							x.LocString != act.LocString //v2.8.4.0 cannot directly pick up whatever is in the same location
						)
					); 
				if (!act.IsPickUp && act.ULDCat == ULDCategory.L &&
					(!isSingleL || !allPuL.Contains(act.CoupleMOAct))) //it is not single L rule, or it is but the couple Act cannot be found (means, this is a left over/unfinished DO, it can drop others)
					addActStaticMoveTime(act, allDoL.Where(x => x != act));
				actListLegalMoves += act.StaticMovingTimeOriginal.Count;
			}
			if (moveTimes.Any())
				minimumMoveTime = moveTimes.Min();
			return true;
    }

		private bool isCircularAct(Act act, out string eStr) {
			eStr = "";
			List<Act> acts = new List<Act>(); //to detect circular blocking MOs
			while (act.PresidingAct != null) {
				if (act.EffectivePriority > act.PresidingAct.EffectivePriority)
					act.PresidingAct.EffectivePriority = act.EffectivePriority;
				acts.Add(act);
				if (acts.Contains(act.PresidingAct)) { //circular MO detected!					
					eStr = "Circular blocking MOs detected! MO act ["; //somehow detecing circular blocking MO
					for (int j = 0; j < acts.Count; ++j) {
						eStr += acts[j].BlockingMoNo.ToString() + "]";
						if (j != acts.Count - 1)
							eStr += " is blocked by [";
					}
					eStr += ". To continue, please resolve the circular blocking MOs!";
					return true;
				}
				act = act.PresidingAct;
			}
			return false;
		}

		private bool generatePresidingMoActs(out string eStr, out CmdStat eStat) { //no need to the the initial PU for this...
			eStr = "";
			eStat = CmdStat.Uninitialized;
			var allPu = actList.Where(x => x.IsPickUp);
			foreach (Act act in allPu) { //assigning the presiding mo Acts
				act.PresidingAct = act.BlockingMoNo <= 0 ? null : allPu.SingleOrDefault(x => x.MoNumber == act.BlockingMoNo);
				if (act.PresidingAct != null && isCircularAct(act, out eStr)) { //if this has presiding Act, check if it is circular act
					eStat = CmdStat.CircularBlkMO;
					return false;
				}
			}
			return true;
		}

		public bool InitializeSearchParameters(string eqpId, ActInitialState initialState, 
			List<Act> moActList, bool isSingleL, bool firstDropHasHighPriority, 
			bool firstDropHasHighPriorityForMultipleUlds, out string eStr, out CmdStat eStat) { //actually, this only matters when we want to search!
			//Prepare error messages
			eStr = "";
			eStat = CmdStat.Uninitialized;

			//Prepare Initial MOActs
			InitialAct = new Act();
			InitialAct.X = initialState.InitialX;
			InitialAct.Z = (int)(initialState.InitialZ - 'A');
			InitialPuActs = moActList
				.Where(act => act.Stat == "E" && act.IsPickUp)
				.ToList();

			actList = new List<Act>(InitialPuActs.Select(x => x.CoupleMOAct));
			var nos = InitialPuActs.Select(x => x.MoNumber);

			if (firstDropHasHighPriority && actList.Any() && //v2.10.2.2 feature and above
				(firstDropHasHighPriorityForMultipleUlds || actList.Count == 1)) { //if high priority is assigned to multiple uld or if it is not but act list is 1. If act list is 2 then no assignment of the high priority when multipleUlds is not true
				double maxPrio = moActList.Where(act => !nos.Contains(act.MoNumber)).Max(x => x.EffectivePriority);
				actList.ForEach(x => x.EffectivePriority = maxPrio); //assign max prio to all the first drop offs
			}

			actList.AddRange(moActList.Where(act => !nos.Contains(act.MoNumber))); //cannot have any left over PU or DO anymore, only others than them

			//actList = new List<Act>(moActList.OrderBy(x => x.Stat == "E" && x.IsPickUp));

			//Generate legal moves for MO Acts
			if (!generateActListLegalMoves(isSingleL)) { //this requires ActList, the correct ones
				eStr = "Fail to generate legal move(s)! There is no legal MO Action found!";
				eStat = CmdStat.FailLegalMove;
				return false;
			}

			//Generate legal moves for Initial MO Acts
			if (!generateInitialLegalMoves(eqpId, isSingleL, out eStr)) { //initial moves cannot be generated before all other legal moves appear!
				eStr = "Fail to generate initial legal move(s)! " + eStr;
				eStat = CmdStat.FailILegalMove;
				return false;
			}

			//Generate presiding MO Acts (for blocking/circular MO checking)
			return generatePresidingMoActs(out eStr, out eStat); //already check for MOs with presiding acts
    }

		public static Act[] CreateMOActsFromMO(MO mo) {
			Act[] moActs = new Act[2] {
				new Act() {
					MoNumber = mo.Number,
					OriginalPriority = mo.Priority,
					EffectivePriority = mo.Priority,
					IsPickUp = true,
					IsWorkstation = mo.PickUpIsWorkstation,
					IsAirSide = mo.PickUpSide == LaneSide.AirSide,
					X = mo.PickUpX,
					Z = mo.PickUpZ,
					ULDCat = mo.ULDCat,
					LocString = mo.PickUpString,
					BlockingMoNo = mo.BlockingNumber,
					EqpId = mo.EquipmentId,
					Stat = mo.Stat,
				},
				new Act() {
					MoNumber = mo.Number,
					OriginalPriority = mo.Priority,
					EffectivePriority = mo.Priority,
					IsPickUp = false,
					IsWorkstation = mo.DropOffIsWorkstation,
					IsAirSide = mo.DropOffSide == LaneSide.AirSide,
					X = mo.DropOffX,
					Z = mo.DropOffZ,
					ULDCat = mo.ULDCat,
					LocString = mo.DropOffString,
					BlockingMoNo = mo.BlockingNumber,
					EqpId = mo.EquipmentId,
					Stat = mo.Stat,
				}
			}; //every MO will generate 2 MO acts      
			moActs[0].CoupleMOAct = moActs[1]; //every generated pair of MO Acts from single MO is coupled
			moActs[1].CoupleMOAct = moActs[0];
			//return mo.Stat == "E" ? new Act[1] { moActs[1] } :  moActs; //in the event it is executing, only the drop off MO is retained
			return moActs; //in the event it is executing, only the drop off MO is retained
		}

		public int MoActCount { get { return actList.Count; } }

    //0 airside, pickup, workstation
    //1 landside, pickup, workstation
    //2 airside, dropoff, workstation
    //3 landside, dropoff, workstation
    //4 airside, pickup, non-workstation
    //5 landside, pickup, non-workstation
    //6 airside, dropoff, non-workstation
    //7 landside, dropoff, non-workstation
    public static int GetActionTime(bool isWorkstation, bool isPickUp, bool isAirSide) {
      return GlobalActTimeSettings.ActTimeSettings
        [(isWorkstation ? 0 : 4) + (isPickUp ? 0 : 2) + (isAirSide ? 0 : 1)];
    }

    public static int GetActionTime(Act act) {
      return GetActionTime(act.IsWorkstation, act.IsPickUp, act.IsAirSide);
    }
  }
}

