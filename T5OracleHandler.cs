using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using Extension.Database.OldOracle;
using T5ShortestTime.Models;
using T5ShortestTime.MOAct;
using T5ShortestTime.SATS;

namespace T5ShortestTime
{
  public class T5OracleHandler : OracleHandler
  {
    public T5TableAndTest TableAndTest = new T5TableAndTest();
    public static string DateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";

    public T5OracleHandler() { }

		public ActInitialState CreateInitialState(string eqpId, ActHandler moActHandler, out string eStr) {
      eStr = "";
      try {
        ActInitialState state = new ActInitialState();
        state.LeftActs = new List<ActLeftBase>();
        EqpInfoReduced eqpRow = EqpInfoReduced.Get(this, eqpId);
        eqpRow.ZPos = getCorrectZPos(eqpRow.ZPos);
        LedInfoReduced ledRow = LedInfoReduced.Get(this, "EQPID", eqpId);
        List<MoInfoView> moInfos = MoInfoView.GetList(this, "MOSTAT='E' AND (MOSTAGE='PKG' OR MOSTAGE='DPF')");
				DateTime now = TableAndTest.UseFixedDateTime ? TableAndTest.FixedDateTime : DateTime.Now; //check if fixed date time is to be used
        if (moInfos == null || moInfos.Count <= 0) { //"free" (to do what the EQP wants) state, not carrying out moving order
          //Get the initial position from the current EQP position
          state.InitialX = eqpRow.XPos;
          state.InitialZ = eqpRow.ZPos;					

          //But a "free" state does not mean the EQP does not have anything at hand right now
					for (int i = 0; i < ledRow.ULDsOn.Count; ++i) {
            //Check if there is any ULD on the EQP to be cleared ("Left Acts")
            moInfos = MoInfoView.GetList(this, "ULDID='" + ledRow.ULDsOn[i] + "'"); //By default, the MO stat must be 'E' and then its MOSTAGE must be PKD            
						if (moInfos == null) //error in reading the table
							return null;
						if (moInfos.Count <= 0) { //a ULD not found, no longer considered an error, it is OK
              //2018-05-07 TODO
							//eStr = "The LED row the ETV is in (LEDID: " + ledRow.LedId + ", LEDEND: " + ledRow.LedEnd +
							//	") shows that it has the following ULD on it (ULDID: " + ledRow.ULDsOn[i] + "). But the " +
							//	TableAndTest.TableNames["Info"].ToString() + " does not contain the same ULDID. " +
							//	"Please provide a valid and executable MO for ULDID: " + ledRow.ULDsOn[i] + " for " + eqpId;
							return state;
						}
            state.LeftActs.Add(createUnfinishedMOAct(moInfos[0], now));            
          }
					//It is possible that the fixed datetime is to be used to say when "now" is
					state.InitialDateTime = now.AddSeconds(1); //the initial date time is now + 1 second, just to be safe (TODO check this in LIVE mode)
        } else { //"moving" state, projection as if the current moving order has been carried out is needed to plan how to route the next ones
          bool isPickingUp = moInfos[0].MoStage == "PKG";

          //takes the initial point not from the current EQP position
          string initPoint = isPickingUp ? moInfos[0].PuPoint : moInfos[0].DoPoint; 
					bool isAirSide = initPoint.EndsWith("12"); //if it ends with 12, then it is air side, otherwise land side
					bool isWorkstation = isPickingUp ? Workstation.IsPickUpWorkstation(initPoint) : Workstation.IsDropOffWorkstation(initPoint);

          //number of ULDsOn not as said, but plus 1 (picking up considered picked up) or minus 1 (dropping off considered dropped of) from it
          int uldsOn = isPickingUp ? ledRow.ULDsOn.Count + 1 : ledRow.ULDsOn.Count - 1; 
          state.InitialX = Convert.ToInt32(initPoint.Substring(0, 2));
          state.InitialZ = initPoint.Substring(2, 1).ToUpper()[0];          
          if (isPickingUp) //adds the ULD on the LED info as if it is already picked up
            ledRow.ULDsOn.Add(moInfos[0].UldId);
          else //if it is dropping, but there is left over... removes the currently being dropped ULD as if it is already dropped
            ledRow.ULDsOn.Remove(moInfos[0].UldId);
          for (int i = 0; i < ledRow.ULDsOn.Count; ++i) { //possibilities: DO[2]->1, PU[0]->1, PU[1]->2            
            moInfos = MoInfoView.GetList(this, "ULDID='" + ledRow.ULDsOn[i] + "'"); //By default, the MO stat must be 'E' and then its MOSTAGE must be PKD            
						if (moInfos == null) //error in reading the table
							return null;
						if (moInfos.Count <= 0) { //a ULD not found, no longer considered an error, it is OK
                //2018-05-07 TODO
        //      eStr = "The LED row the ETV is in (LEDID: " + ledRow.LedId + ", LEDEND: " + ledRow.LedEnd +
								//") shows that it has the following ULD on it (ULDID: " + ledRow.ULDsOn[i] + "). But the " +
								//TableAndTest.TableNames["Info"].ToString() + " does not contain the same ULDID. " +
								//"Please provide a valid and executable MO for ULDID: " + ledRow.ULDsOn[i] + " for " + eqpId;
							return state;
						}
						state.LeftActs.Add(createUnfinishedMOAct(moInfos[0], now)); //TODO bug why would this creates multiple times?
          }
          double actionTime = ActHandler.GetActionTime(isWorkstation, isPickingUp, isAirSide);
          double moveTime = moActHandler.GetDistanceMoveTime(eqpRow.XPos, 
            eqpRow.ZPos.ToString().ToUpper()[0] - 'A',
            state.InitialX, state.InitialZ.ToString().ToUpper()[0] - 'A');
          state.InitialDateTime = now.AddSeconds(actionTime + moveTime + 1); //the initial datetime is also projected to the future...
        }
        return state;
      } catch (Exception e){
        eStr = e.ToString();
        return null; //error for whatever reason
      }
    }

    private char getCorrectZPos(char zPos) {
      if (zPos.ToString().ToUpper()[0] > 'F') //since Z can be wrong, needs to be corrected
        zPos = 'F';
      if (zPos.ToString().ToUpper()[0] < 'A') //since Z can be wrong, needs to be corrected
        zPos = 'A';
      return zPos;
    }

    private ActLeftBase createUnfinishedMOAct(MoInfoView moInfoRow, DateTime now) {
      ActLeftBase basic = new ActLeftBase();
      basic.PUPoint = moInfoRow.PuPoint;
      basic.DOPoint = moInfoRow.DoPoint;
      basic.ULDCat = moInfoRow.Cat;
      basic.PriorLevel = moInfoRow.PriorLevel;
      basic.No = moInfoRow.MoNo;
      return basic;
    }
  }

	[Serializable()]
  public class T5TableAndTest
  {
    [XmlArrayItem("TableName")]
    public AliasNamePair[] TableNameAliases = null;
    [XmlIgnore] //[NonSerialized()] //This doesn't work well to ignore the elements
    public Dictionary<string, string> TableNames = new Dictionary<string, string>();

    public int DisplayLimit = 1000;
		public bool ShowTableView = false;
		public bool AddIndex = false;
    public bool UseFixedDateTime;
    public DateTime FixedDateTime;

    public void AfterDeserialization() {
      if (TableNameAliases == null || TableNameAliases.Length <= 0)
        return;
      TableNames = TableNameAliases.ToDictionary(x => x.Alias, x => x.Name);
    }
  }

  [Serializable]
  [XmlType(TypeName = "TableNameAlias")]
  public struct TableNameAlias<A, N>
  {
    public A Alias { get; set; }
    public N Name { get; set; }

    public TableNameAlias(A alias, N name)
      : this() { //this() is necessary for struct to initialize all variables
      Alias = alias;
      Name = name;
    }
  }

  public class AliasNamePair
  {
    [XmlAttribute]
    public string Alias;
    [XmlAttribute]
    public string Name;
    public AliasNamePair() { }
    public AliasNamePair(string alias, string name) {
      Alias = alias;
      Name = name;
    }
  }
}



