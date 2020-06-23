using System.Collections.Generic;
using T5ShortestTime.MOAct;

namespace T5ShortestTime.SATS {
  public class Workstation
  {
    private static List<string> wsDoList = new List<string>();
    private static List<string> wsPuList = new List<string>();
    private static Dictionary<string, List<string>> wsJoinDict = new Dictionary<string, List<string>>();

    public static int WsPuCount { get { return wsPuList.Count; } }
    public static int WsDoCount { get { return wsDoList.Count; } }
    public static int JoinDictCount { get { return wsJoinDict.Count; } }

    public static void AddWsPuDo(WorkstationJoin wsJoin) { //this is no longer a recommended way to find all these, except for join
      wsDoList = wsJoin.DropOffList; //it may still worth retaining, however... considering the fact that it cannot validate all places as drop off or pick up
      wsPuList = wsJoin.PickUpList;
      wsJoinDict.Clear();
      for (int i = 0; i < wsJoin.JoinList.Count; ++i) {
        string joinWs = wsJoin.JoinList[i].ToUpper();
        string[] words = joinWs.Split(new char[] { '-' });
        if (words == null || words.Length < 2 || !wsDoList.Contains(words[0]) || !wsPuList.Contains(words[1]))
          continue;
        if (!wsJoinDict.ContainsKey(words[0]))
          wsJoinDict.Add(words[0], new List<string>());
        wsJoinDict[words[0]].Add(words[1]);
      }
    }

    public static bool IsPickUpWorkstation(string puStr) {
      return wsPuList.Contains(puStr);
    }

    public static bool IsDropOffWorkstation(string doStr) {
      return wsPuList.Contains(doStr);
    }

    public static bool IsDoPuJoinable(string doStr, string puStr) {
      if (!wsJoinDict.ContainsKey(doStr))
        return false;
      return wsJoinDict[doStr].Contains(puStr);
    }
  }
}
