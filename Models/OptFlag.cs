using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using T5ShortestTime.MOAct;

namespace T5ShortestTime.Models {
  [Serializable]
  public class OptFlag : ISimulatedModel {
		public string EqpId { get; set; }
		public CmdStat CmdStat { get; set; }
    private static bool firstTime { get; set; } = true;
    private static List<OptFlag> flags = new List<OptFlag>();

    public static List<OptFlag> GetList(T5OracleHandler oracleHandler) {
      try {
        if (PH.IsSimulation)
          return getList();
        IEnumerable<object[]> rowCells = oracleHandler.ReadAllRowCells(oracleHandler.TableAndTest.TableNames["OptFlag"]);
        return rowCells == null ? null : rowCells.Select(cells =>
          new OptFlag() { EqpId = (string)cells[0], CmdStat = (CmdStat)Convert.ToInt32(cells[1]) }
          ).ToList();
      } catch {
        return null;
      }
    }

    private static List<OptFlag> getList() {
      if (firstTime) {
        flags = deserializeList();
        firstTime = false;
      }
      return flags;
    }

    private static List<OptFlag> deserializeList() {
      FileStream filestream = new FileStream(PH.OptFlagSimulationFilepath, FileMode.Open, FileAccess.Read, FileShare.Read);
      XmlSerializer serializer = new XmlSerializer(typeof(OptFlagList));
      OptFlagList list = (OptFlagList)serializer.Deserialize(filestream);
      filestream.Close();
      return list.Items;
    }

    //To be called before things are closed
    public static void SerializeLatestList() {
      using (TextWriter textWriter = new StreamWriter(PH.OptFlagSimulationFilepath)) {
        OptFlagList optFlagList = new OptFlagList();
        optFlagList.Items.AddRange(flags.ToArray());
        XmlSerializer serializer = new XmlSerializer(typeof(OptFlagList));
        serializer.Serialize(textWriter, optFlagList);
        textWriter.Close();
      }
    }

    //private static object locker = new object();
    public static bool UpdateCmdStat(T5OracleHandler oracleHandler, string eqpid, CmdStat cmd) {
      try {
        if (PH.IsSimulation) {
          if (firstTime)
            flags = getList();
          OptFlag flag = flags.FirstOrDefault(x => x.EqpId == eqpid);
          flag.CmdStat = cmd;
          return true;
        }
        
        using (OracleDataAdapter adapter = new OracleDataAdapter()) { //default is select command, probably unmanaged
          string sqlStatement = "update " + oracleHandler.TableAndTest.TableNames["OptFlag"] +
            " set CMDSTAT=" + ((int)cmd).ToString() + " where EQPID='" + eqpid + "'";
          adapter.UpdateCommand = new OracleCommand(sqlStatement, oracleHandler.GetConnection());
          bool result = adapter.UpdateCommand.ExecuteNonQuery() > 0;
          return result;
        }
      } catch {
        return false;
      }
    }

    public void ApplyTestData() {
      EqpId = "ETV1";
      CmdStat = CmdStat.Requesting;
    }

    public Tuple<List<string>, List<List<object>>> GetTable(bool withIndex, int limit) {
      List<OptFlag> list = deserializeList();
      List<string> headers = new List<string> { "EQPID", "CMDSTAT" };
      if (withIndex)
        headers.Insert(0, "ADD_INDEX"); //added index
      List<List<object>> rows = new List<List<object>>();
      for (int i = 0; i < Math.Min(limit, list.Count); ++i) {
        List<object> row = new List<object>();
        OptFlag item = list[i];
        if (withIndex) //if it is to be added with index
          row.Add(i + 1); //add index number as first element in the row
        row.Add(item.EqpId);
        row.Add(item.CmdStat);
        rows.Add(row);
      }
      Tuple<List<string>, List<List<object>>> result = new Tuple<List<string>, List<List<object>>>(headers, rows);
      return result;
    }
  }

  //For simulation purpose only
  [XmlRoot("OptFlagList")]
  public class OptFlagList {
    [XmlElement("OptFlag")]
    public List<OptFlag> Items = new List<OptFlag>();
  }

  //TABLE NOTE: By default, taken from OPTFLAG table (Alias "OptFlag")
  //all columns are taken
  //[0]        [1]        
  //EQPID    | CMDSTAT      
  //ETV1       1
  //ETV2       1
  //ETV3       1
}
