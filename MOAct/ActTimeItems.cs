using System;
using System.Linq;
using System.Xml.Serialization;

namespace T5ShortestTime.MOAct {
  [Serializable]
  public class GlobalActTimeSettings {
    [XmlArrayItem("AliasActTimePair")]
    public AliasActTimePair[] AliasActTimePairs = null;
    [XmlIgnore]
    public int[] ActTimeSettings = null;
    public void AfterDeserialization() {
      if (AliasActTimePairs == null || AliasActTimePairs.Length <= 0)
        return;
      ActTimeSettings = AliasActTimePairs.Select(x => x.ActTime).ToArray();
    }
  }

  public class AliasActTimePair {
    [XmlAttribute]
    public string Alias;
    [XmlAttribute]
    public int ActTime;
    public AliasActTimePair() { }
    public AliasActTimePair(string alias, int actTime) {
      Alias = alias;
      ActTime = actTime;
    }
  }


  [Serializable()]
  public class TimeSettings {
    //int is used for fast computation
    public int XTimePerLaneIn100ms = 22; //default value
    public int ZTimePerLevelIn100ms = 115; //default value //TODO for now, does not really distinguish between "up" and "down"
    public int XExtraTimeIn100ms = 20;
    public int ZExtraTimeIn100ms = 40;
    //TODO consider to add more time indicator, especially for the workstation versus the storage lane

    public TimeSettings() {
    }
  }

}
