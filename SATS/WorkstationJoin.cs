using System;
using System.Collections.Generic;

namespace T5ShortestTime.SATS {
  [Serializable()]
  public class WorkstationJoin {
    public List<string> PickUpList = new List<string>();
    public List<string> DropOffList = new List<string>();
    public List<string> JoinList = new List<string>();
  }
}
