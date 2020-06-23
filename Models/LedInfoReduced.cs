using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace T5ShortestTime.Models {
  [Serializable]
  public class LedInfoReduced : ISimulatedModel {
		public string LedId { get; set; } //column 0
		public string LedEnd { get; set; } //column 1
		public string AreaId { get; set; } //column 2
		public string EqpId { get; set; } //column 3
    [XmlElement("ULD")]
		public List<string> ULDsOn { get; set; } //column 15

    public static LedInfoReduced Get(T5OracleHandler oracleHandler, string columnIdName = null, string columnId = null) {
      try {
        if (PH.IsSimulation) {
          var list = deserializeList();
          return list.FirstOrDefault(x => x.EqpId == columnId); //column ID here is always EQP ID, so that's OK
        }

        string whereClause = string.IsNullOrEmpty(columnId) || string.IsNullOrEmpty(columnIdName) ?
          "" : columnIdName + "='" + columnId + "'";
        object[] cells = oracleHandler.ReadAllRowCells(oracleHandler.TableAndTest.TableNames["LED"], whereClause, handleDBNull: true).FirstOrDefault(); //current table
        if (cells == null && oracleHandler.TableAndTest.TableNames.ContainsKey("LED_S")) //if not found, check the other table!
          oracleHandler.ReadAllRowCells(oracleHandler.TableAndTest.TableNames["LED_S"], whereClause, handleDBNull: true).FirstOrDefault();
        return cells == null ? null : new LedInfoReduced() {
          LedId = (string)cells[0],
          LedEnd = (string)cells[1],
          AreaId = (string)cells[2],
          EqpId = (string)cells[3],
          ULDsOn = string.IsNullOrWhiteSpace((string)cells[15]) ?
            new List<string>() :
            ((string)cells[15]).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList()
        };
      } catch {
        return null;
      }
    }

    private static List<LedInfoReduced> deserializeList() {
      FileStream filestream = new FileStream(PH.LedInfoSimulationFilepath, FileMode.Open, FileAccess.Read, FileShare.Read);
      XmlSerializer serializer = new XmlSerializer(typeof(LedInfoReducedList));
      LedInfoReducedList list = (LedInfoReducedList)serializer.Deserialize(filestream);
      filestream.Close();
      return list.Items;
    }

    public void ApplyTestData() {
      LedId = "ETV1";
      LedEnd = "ETV12";
      AreaId = "Z";
      EqpId = "ETV1";
      ULDsOn = new List<string>() { "AKE88217SQ", "PMC61429SQ" };
    }

    public Tuple<List<string>, List<List<object>>> GetTable(bool withIndex, int limit) {
      List<LedInfoReduced> list = deserializeList();
      List<string> headers = new List<string> { "LEDID", "LEDEND", "AREAID", "EQPID", "ULDSON" };
      if (withIndex)
        headers.Insert(0, "ADD_INDEX"); //added index
      List<List<object>> rows = new List<List<object>>();
      for (int i = 0; i < Math.Min(limit, list.Count); ++i) {
        List<object> row = new List<object>();
        LedInfoReduced item = list[i];
        if (withIndex) //if it is to be added with index
          row.Add(i + 1); //add index number as first element in the row
        row.Add(item.LedId);
        row.Add(item.LedEnd);
        row.Add(item.AreaId);
        row.Add(item.EqpId);
        row.Add(string.Join("  ", item.ULDsOn.ToArray())); //all the ULDs are separated by double space
        rows.Add(row);
      }
      Tuple<List<string>, List<List<object>>> result = new Tuple<List<string>, List<List<object>>>(headers, rows);
      return result;
    }
  }

  //For simulation purpose only
  [XmlRoot("LedInfoReducedList")]
  public class LedInfoReducedList {
    [XmlElement("LedInfoReduced")]
    public List<LedInfoReduced> Items = new List<LedInfoReduced>();
  }

  //Only ULDsOn is actually of any good use

  //TABLE NOTE: By default, taken from LEDINFO table (Alias "LED")
  //Only some columns are taken, not all
  //ULDSON is the format like: AKE88217SQ  PMC61429SQ  when there are ULD, 12 chars each, last 1-3 chars might not be used
  //[0]     [1]      [2]      [3]     [4]    [5]      [6]       [7]         [8]       [9]    [10]   [11]       [12]     [13]      [14]        [15]     [16]       [17]     [18]      [19]
  //LEDID | LEDEND | AREAID | EQPID | ZONE | LEDTYP | STOPSET | YPOSSTART | YPOSEND | XPOS | ZPOS | USEOPCAP | MAXCAP | LEFTCAP | NUMOFULDS | ULDSON | LANEFUNC | RESCAP | CTRLMOD | AVAILSTAT
  //ETV1    ETV12             ETV1           E        0	        13	        14	      10	   A      Y          2.5	    2.5	      0		                 S	        0	       E	       1
  //ETV2    ETV22             ETV2           E        0	        13	        14	      35	   A      Y          2.5	    2.5	      0	                   S	        0	       E	       1
  //ETV3    ETV32             ETV3           E        0	        13	        14	      55	   A      Y          2.5	    2.5	      0		                 S	        0	       E	       1
}