using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace T5ShortestTime.Models {
  [Serializable]
	public class EqpInfoReduced : ISimulatedModel {
		public string EqpId { get; set; } //EQPID, column 0
		public string EqpTyp { get; set; } //EQPTYP, column 1
		public char CtrlMod { get; set; } //CTRLMOD, column 5, only get the first char E = Enabled, D = Disabled
    public char KeySwitch { get; set; } //KEYSWITCH, column 6, only get the first char
    public bool AvailStat { get; set; } //AVAILSTAT, column 7, true if AVAILSTAT > 0
    public int XPos { get; set; } //XPOS, column 8
		public char ZPos { get; set; } //ZPOS, column 10, only get the first char
		public bool ConnStat { get; set; } //CONNSTAT, column 11, true if AVAILSTAT > 0

    public static EqpInfoReduced Get(T5OracleHandler oracleHandler, string eqpId = null) {
      try {
        if (PH.IsSimulation) {
          var list = deserializeList();
          return list.FirstOrDefault(x => x.EqpId == eqpId);
        }

        object[] cells = oracleHandler.ReadAllRowCells(oracleHandler.TableAndTest.TableNames["EQP"], 
          string.IsNullOrEmpty(eqpId) ? "" : "EQPID='" + eqpId + "'").FirstOrDefault();
        return cells == null ? null : new EqpInfoReduced() {
          EqpId = (string)cells[0],
          EqpTyp = (string)cells[1],
          CtrlMod = ((string)cells[5])[0],
          KeySwitch = ((string)cells[6])[0],
          AvailStat = Convert.ToInt32(cells[7]) > 0,
          XPos = Convert.ToInt32(cells[8]),
          ZPos = ((string)cells[10])[0],
          ConnStat = Convert.ToInt32(cells[12]) > 0
        };
      } catch {
        return null;
      }
    }

    private static List<EqpInfoReduced> deserializeList() {
      FileStream filestream = new FileStream(PH.EqpInfoSimulationFilepath, FileMode.Open, FileAccess.Read, FileShare.Read);
      XmlSerializer serializer = new XmlSerializer(typeof(EqpInfoReducedList));
      EqpInfoReducedList list = (EqpInfoReducedList)serializer.Deserialize(filestream);
      filestream.Close();
      return list.Items;
    }

    public void ApplyTestData() {
      AvailStat = false;
      ConnStat = true;
      CtrlMod = 'E';
      EqpId = "ETV1";
      EqpTyp = "ETV";
      KeySwitch = 'A';
      XPos = 60;
      ZPos = 'A';
    }

    public Tuple<List<string>, List<List<object>>> GetTable(bool withIndex, int limit) {
      List<EqpInfoReduced> list = deserializeList();
      List<string> headers = new List<string> { "EQPID", "EQPTYP", "CTRLMOD", "KEYSWITCH", "AVAILSTAT", "XPOS", "ZPOS", "CONNSTAT" };
      if (withIndex)
        headers.Insert(0, "ADD_INDEX"); //added index
      List<List<object>> rows = new List<List<object>>();
      for(int i = 0; i < Math.Min(limit, list.Count); ++i) {
        List<object> row = new List<object>();
        EqpInfoReduced item = list[i];
        if (withIndex) //if it is to be added with index
          row.Add(i + 1); //add index number as first element in the row
        row.Add(item.EqpId);
        row.Add(item.EqpTyp);
        row.Add(item.CtrlMod);
        row.Add(item.KeySwitch);
        row.Add(item.AvailStat);
        row.Add(item.XPos);
        row.Add(item.ZPos);
        row.Add(item.ConnStat);
        rows.Add(row);
      }
      Tuple<List<string>, List<List<object>>> result = new Tuple<List<string>, List<List<object>>>(headers, rows);
      return result;
    }
  }
  
  //For simulation purpose only
  [XmlRoot("EqpInfoReducedList")]
  public class EqpInfoReducedList {
    [XmlElement("EqpInfoReduced")]
    public List<EqpInfoReduced> Items = new List<EqpInfoReduced>();
  }

  //Currently, only XPos and ZPos are actually used!

  //TABLE NOTE: By default, taken from EQPINFO table (Alias "EQP")
  //Only some columns are taken, not all
  //[0]     [1]      [2]       [3]     [4]    [5]       [6]         [7]         [8]    [9]    [10]   [11]         [12]       [13]      [14]
  //EQPID | EQPTYP | EQPDESC | MCPNO | Zone | CTRLMOD | KEYSWITCH | AVAILSTAT | XPOS | YPOS | ZPOS | IEDOLESENT | CONNSTAT | IEDFLAG | SECTORID
  //ETV1    ETV      ETV	     23	     S      E         A	          1	          22	   13	    A		                1	         N
  //ETV2    ETV      ETV       25	     N      E         A	          1	          60	   13	    A		                1	         N
  //ETV3    ETV      ETV       24	     N      E         A	          1	          75	   13	    A		                1          N
}
