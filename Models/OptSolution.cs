using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using T5ShortestTime.MOAct;
using T5ShortestTime.SATS;

namespace T5ShortestTime.Models {
  [Serializable]
  public class OptSolution : ISimulatedModel {
		public int MoNo { get; set; }
		public string EqpId { get; set; }
		public string Action { get; set; }
		public int SequenceNo { get; set; }
		public DateTime EstExTime { get; set; }
		public DateTime EstFnTime { get; set; }

    public static List<OptSolution> Create(T5OracleHandler oracleHandler, DateTime initialDateTime, ActNode solutionTreeNode) {
      if (solutionTreeNode == null || solutionTreeNode.Level == 0)
        return null;
      try {
        List<OptSolution> tableRows = new List<OptSolution>(solutionTreeNode.Level);
        List<ActNode> treeNodes = solutionTreeNode.GenerateMoActTreeNodes();
        int accActionTime = 0;
        int totalTime = 0;
        for (int i = 0; i < treeNodes.Count; ++i) { //should be the same number as tableRows
          int actionTime = ActHandler.GetActionTime(treeNodes[i].Act); //should have the act...
          totalTime = treeNodes[i].AccMoveTime + accActionTime; //start from zero
          DateTime exTime = initialDateTime.AddSeconds(0.1 * totalTime);
          DateTime fnTime = initialDateTime.AddSeconds(0.1 * (totalTime + actionTime));
          accActionTime += actionTime; //accActionTime
          Act act = treeNodes[i].Act;
          tableRows.Add(new OptSolution() {
            MoNo = act.MoNumber,
            EqpId = act.EqpId,
            Action = act.IsPickUp ? "PU" : "DO",
            SequenceNo = i + 1,
            EstExTime = exTime,
            EstFnTime = fnTime
          });
        }
        return tableRows;
      } catch {
        return null;
      }
    }

    private static ActNode lastSolutionTableRecorded;
    public static CmdStat Update(T5OracleHandler oracleHandler, DateTime initialDateTime,
      Equipment equipment, CmdStat originalCmdStat, out string failMsg) {
      failMsg = "";

      if (equipment.UpdateOnFinished && !equipment.IsCompleted)
        return originalCmdStat;

      ActNode eqpLastSolution = equipment.LastSolution; //get last solution
      if (!equipment.IsCompleted && // if the solution is completed, then there is no need to get here!
        (eqpLastSolution == null || //if no solution or there is an existing solution identical to the current solution
        (lastSolutionTableRecorded != null && lastSolutionTableRecorded.IsIdenticalSequence(eqpLastSolution))))
        return CmdStat.Processing; //Not completed yet...
      if (equipment.IsCompleted && lastSolutionTableRecorded != null && //if the the search is completed and having
        lastSolutionTableRecorded.IsIdenticalSequence(eqpLastSolution)) //identical solution
        return CmdStat.Completed; //return completed, no need to recreate the new table!
                                  //The table can be updated multiple times depending on how frequent is the solution changed			

      if (eqpLastSolution != null) { //only need to update if there is any solution
        List<OptSolution> tableRows = Create(oracleHandler, initialDateTime, eqpLastSolution);
        if (tableRows != null) {
          bool result = fillTable(oracleHandler, equipment.ID, tableRows, out failMsg);
          if (result) {
            lastSolutionTableRecorded = eqpLastSolution; //only update the table recorded last solution if it is successful
            return equipment.IsCompleted ? CmdStat.Completed : CmdStat.Processing;
          } else if (!string.IsNullOrEmpty(failMsg)) {
            failMsg = "Solution table creation failure! " + failMsg;
            return CmdStat.SolFail;
          } else
            return CmdStat.SolNotFound;
        } else { //error
          failMsg = "Solution table creation failure!";
          return CmdStat.SolNotFound;
        }
      }
      return originalCmdStat; //if there is no update, returns original cmd stat
    }

    private static List<OptSolution> deserializeList(string eqpId) {
      string filepath = PH.OptSolutionBaseSimulationFilepath + string.Format("_{0}.xml", eqpId);
      if (!File.Exists(filepath))
        return new List<OptSolution>();
      FileStream filestream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read);
      XmlSerializer serializer = new XmlSerializer(typeof(OptSolutionList));
      OptSolutionList list = (OptSolutionList)serializer.Deserialize(filestream);
      filestream.Close();
      return list.Items;
    }

    private static void serializeList(string eqpId, List<OptSolution> items) {
      using (TextWriter textWriter = new StreamWriter(PH.OptSolutionBaseSimulationFilepath + 
        string.Format("_{0}.xml", eqpId))) {
        OptSolutionList optSolutionList = new OptSolutionList();
        optSolutionList.Items.AddRange(items.ToArray());
        XmlSerializer serializer = new XmlSerializer(typeof(OptSolutionList));
        serializer.Serialize(textWriter, optSolutionList);
        textWriter.Close();
      }
    }

    private static bool fillTable(T5OracleHandler oracleHandler, string eqpId, List<OptSolution> tableRows, out string failMsg) {
      failMsg = "";
      if (tableRows == null || tableRows.Count <= 0)
        return false;

      if (PH.IsSimulation) {
        serializeList(eqpId, tableRows);
        return true;
      }

      string sqlStatement = "delete from " + oracleHandler.TableAndTest.TableNames["Solution"] + " where EQPID='" + eqpId + "'";
      try {
        using (OracleDataAdapter adapter = new OracleDataAdapter()) {
          adapter.DeleteCommand = new OracleCommand(sqlStatement, oracleHandler.GetConnection()); //String in oracle command must strictly using '' (apostrophe)
          adapter.DeleteCommand.ExecuteNonQuery();
          for (int i = 0; i < tableRows.Count; ++i) { //learn about parameterized query here!
            sqlStatement = "insert into " + oracleHandler.TableAndTest.TableNames["Solution"] +
              " (MONO, EQPID, ACTION, SEQUENCENO, ESTEXTIME, ESTFNTIME) values (" +
              tableRows[i].MoNo.ToString() + ", '" + tableRows[i].EqpId + "', '" +
              tableRows[i].Action + "', " + tableRows[i].SequenceNo.ToString() +
              ", '" + tableRows[i].EstExTime.ToString(T5OracleHandler.DateTimeFormat) +
              "', '" + tableRows[i].EstFnTime.ToString(T5OracleHandler.DateTimeFormat) + "')";
            adapter.InsertCommand = new OracleCommand(sqlStatement, oracleHandler.GetConnection());
            adapter.InsertCommand.ExecuteNonQuery();
          }
          return true;
        }
      } catch (Exception e) {
        failMsg = sqlStatement + " " + e.ToString();
        return false;
      }
    }

    public void ApplyTestData() {
      MoNo = 1278805;
      EqpId = "ETV1";
      Action = "DO";
      SequenceNo = 1;
      EstExTime = new DateTime(2018, 12, 25, 13, 53, 42, 586);
      EstFnTime = new DateTime(2018, 12, 25, 13, 54, 17, 586);
    }

    public Tuple<List<string>, List<List<object>>> GetTable(List<string> eqpNames, bool withIndex, int limit) {
      List<string> headers = new List<string> { "MONO", "EQPID", "ACTION", "SEQUENCENO", "ESTEXTIME", "ESTFNTIME" };
      if (withIndex)
        headers.Insert(0, "ADD_INDEX"); //added index
      List<List<object>> rows = new List<List<object>>();
      bool limitReached = false;
      for (int k = 0; k < eqpNames.Count; ++k) {
        List<OptSolution> list = deserializeList(eqpNames[k]);
        for(int i = 0; i < list.Count; ++i) {
          List<object> row = new List<object>();
          OptSolution item = list[i];
          if (withIndex) //if it is to be added with index
            row.Add(i + 1); //add index number as first element in the row
          row.Add(item.MoNo);
          row.Add(item.EqpId);
          row.Add(item.Action);
          row.Add(item.SequenceNo);
          row.Add(item.EstExTime);
          row.Add(item.EstFnTime);
          rows.Add(row);
          limitReached = rows.Count >= limit;
          if (limitReached)
            break;
        }
        if (limitReached)
          break;
      }
      Tuple<List<string>, List<List<object>>> result = new Tuple<List<string>, List<List<object>>>(headers, rows);
      return result;
    }
  }

  //For simulation purpose only
  [XmlRoot("OptSolutionList")]
  public class OptSolutionList {
    [XmlElement("OptSolution")]
    public List<OptSolution> Items = new List<OptSolution>();
  }

  //TABLE NOTE: By default, taken from OPTSOLUTION table (Alias "Solution")
  //all columns are taken, ESTEXTIME = Estimated execution time, ESTFNTIME = Estimated finished time
  //[0]       [1]     [2]      [3]          [4]                       [5]         
  //MONO    | EQPID | ACTION | SEQUENCENO | ESTEXTIME               | ESTFNTIME
  //1278805	  ETV3    DO       3	          2018-12-25 13:53:42.586	  2018-12-25 13:54:17.586
  //1278807	  ETV3    PU       4	          2018-12-25 13:54:44.586	  2018-12-25 13:55:24.586
  //1278807	  ETV3    DO       5	          2018-12-25 13:55:28.786	  2018-12-25 13:56:33.786
  //1278803	  ETV3    DO       1	          2018-12-25 13:50:24.586	  2018-12-25 13:50:59.586
  //1278805	  ETV3    PU       2	          2018-12-25 13:52:01.086	  2018-12-25 13:52:41.086
  //1288628	  ETV2    DO       3	          2019-01-03 01:12:39.489	  2019-01-03 01:13:44.489
  //1289015	  BV1     PU       1	          2019-01-03 11:59:06	      2019-01-03 12:00:36
  //1288632	  ETV2    DO       4	          2019-01-03 01:13:59.989	  2019-01-03 01:15:04.989
  //1288631	  ETV2    PU       5	          2019-01-03 01:15:20.489	  2019-01-03 01:16:00.489
  //1288631	  ETV2    DO       6	          2019-01-03 01:16:04.489	  2019-01-03 01:17:09.489
  //1288622	  ETV2    PU       7	          2019-01-03 01:18:10.989	  2019-01-03 01:18:50.989
  //1288622	  ETV2    DO       8	          2019-01-03 01:19:52.489	  2019-01-03 01:20:27.489
  //1288645	  ETV2    PU       9	          2019-01-03 01:21:28.989	  2019-01-03 01:22:08.989
  //1288645	  ETV2    DO       10	          2019-01-03 01:22:35.989	  2019-01-03 01:23:10.989
  //1288638	  ETV2    PU       11	          2019-01-03 01:23:26.489	  2019-01-03 01:24:06.489
  //1288638	  ETV2    DO       12	          2019-01-03 01:24:21.989	  2019-01-03 01:24:56.989
  //1288613	  ETV2    DO       1	          2019-01-03 01:10:21.089	  2019-01-03 01:11:26.089
  //1288632	  ETV2    PU       2	          2019-01-03 01:11:53.089	  2019-01-03 01:12:33.089
  //1288647	  ETV2    PU       13	          2019-01-03 01:25:03.389	  2019-01-03 01:25:43.389
  //1288647	  ETV2    DO       14	          2019-01-03 01:25:58.889	  2019-01-03 01:26:33.889
  //1288625	  ETV2    PU       15	          2019-01-03 01:27:12.389	  2019-01-03 01:27:52.389
  //1288625	  ETV2    DO       16	          2019-01-03 01:28:19.389	  2019-01-03 01:28:54.389
  //1288854	  ETV1    PU       1	          2019-01-03 05:35:29.155	  2019-01-03 05:35:59.155
  //1288854	  ETV1    DO       2	          2019-01-03 05:36:25.355	  2019-01-03 05:37:30.355
}
