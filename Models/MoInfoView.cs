using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace T5ShortestTime.Models {
  [Serializable]
  public class MoInfoView : ISimulatedModel {
		public int MoNo { get; set; }
		//public string MoType { get; set; } //not needed
		public string UldId { get; set; }
		public string EqpId { get; set; }
		//public string PuBufId { get; set; } //just in case, maybe needed
		public string Cat { get; set; } //may take two Ls, but only one A or AW
		public string PuPoint { get; set; }
		//public string DoBufId { get; set; } //just in case, maybe needed
		public string DoPoint { get; set; }
		//public string UldCurrElbId { get; set; } //not needed
		//public char MoExecReady { get; set; } //this is where, we don't need
		//public Nullable<int> NextMoNo { get; set; } //this is not needed too
		//public int UserPrior { get; set; } //maybe needed
		public string MoStat { get; set; } //v.2.8.3.6 or above, this is included to check the initial state
		//public DateTime MoGenDat { get; set; } //not needed
		public DateTime PuDtTm { get; set; }
		//public DateTime? DoDtTm { get; set; }
		public DateTime Deadline { get; set; }
		public int PriorLevel { get; set; } //higher priority means earlier, already adjusted from actual numbers (like 50001) to (sequential) level (like 5, 4, 3, ...) by Oracle Storage Procedure
		public string BlockingUld { get; set; } //maybe needed
    //PKG and DPF are "moving stage", "PKD" is a "stopping state"
		public string MoStage { get; set; } //needed, "PKG" means picking up, "DPF" means dropping of, "PKD" seems to mean picked up
		//public int IsExec { get; set; } //another where, not needed
		//public int UnexecCauseId { get; set; } //not needed
		public int BlockingMoNo { get; set; } //maybe needed

    //PuDtTm, Deadline, and BlockingUld are actually unused

    public static List<MoInfoView> GetList(T5OracleHandler oracleHandler, string whereClause) {
      try {
        if (PH.IsSimulation) {
          List<MoInfoView> items = deserializeList();
          //There are some possible usage of get list, hence we have to painfully creates different return based on it
          //1. MoInfoView.GetList(t5OracleHandler, equipment.ID == null ? null : "EQPID='" + equipment.ID + "'");
          if (string.IsNullOrWhiteSpace(whereClause)) //means return everything
            return items;
          if (whereClause.StartsWith("EQPID")) {
            //strip the "EQPID='" and the last "'"
            var eqpIdString = whereClause.Substring("EQPID='".Length, whereClause.Length - "EQPID='".Length - 1);
            return items.Where(x => x.EqpId == eqpIdString).ToList();
          }

          //2. MoInfoView.GetList(this, "MOSTAT='E' AND (MOSTAGE='PKG' OR MOSTAGE='DPF')");
          if (whereClause == "MOSTAT='E' AND (MOSTAGE='PKG' OR MOSTAGE='DPF')")
            return items.Where(x => x.MoStat == "E" && (x.MoStage == "PKG" || x.MoStage == "DPF")).ToList();

          //3. MoInfoView.GetList(this, "ULDID='" + ledRow.ULDsOn[i] + "'");          
          if (whereClause.StartsWith("ULDID")) {
            //strip the "ULDID='" and the last "'"
            var uldIdString = whereClause.Substring("ULDID='".Length, whereClause.Length - "ULDID='".Length - 1);
            return items.Where(x => x.UldId == uldIdString).ToList();
          }

          throw new Exception("Unhandled simulation case!");
        }

        IEnumerable<object[]> rowCells = oracleHandler.ReadAllRowCells(oracleHandler.TableAndTest.TableNames["Info"], whereClause);
        if (rowCells == null) //to distinguish between unable to read from the database and empty entry
          return null;
        if (!rowCells.Any()) //empty entry
          return new List<MoInfoView>(); //return empty List
        return rowCells.Select(cells => new MoInfoView() {
          MoNo = Convert.ToInt32(cells[0]),
          UldId = (string)cells[1],
          EqpId = (string)cells[2],
          Cat = (string)cells[3],
          PuPoint = (string)cells[4],
          DoPoint = (string)cells[5],
          MoStat = (string)cells[6],
          PuDtTm = cells[7] is DBNull ? DateTime.MaxValue : (DateTime)cells[7],
          Deadline = cells[8] is DBNull ? DateTime.MaxValue : (DateTime)cells[8],
          PriorLevel = cells[9] is DBNull ? 0 : Convert.ToInt32(cells[9]),
          BlockingUld = cells[10] is DBNull ? null : (string)cells[10],
          MoStage = cells[11] is DBNull ? null : (string)cells[11],
          BlockingMoNo = cells[12] is DBNull ? -1 : Convert.ToInt32(cells[12])
        }).ToList();
      } catch {
        return null;
      }
    }

    private static List<MoInfoView> deserializeList() {
      FileStream filestream = new FileStream(PH.MoInfoViewSimulationFilepath, FileMode.Open, FileAccess.Read, FileShare.Read);
      XmlSerializer serializer = new XmlSerializer(typeof(MoInfoViewList));
      MoInfoViewList list = (MoInfoViewList)serializer.Deserialize(filestream);
      filestream.Close();
      return list.Items;
    }

    public void ApplyTestData() {
      MoNo = 1280922;
      UldId = "PMC58289SQ";
      EqpId = "ETV1";
      Cat = "A";
      PuPoint = "67A15";
      DoPoint = "67A15";
      MoStat = "Q";
      PuDtTm = new DateTime(2019, 3, 1, 2, 30, 0, 0);
      Deadline = new DateTime(2019, 3, 1, 14, 30, 0, 0);
      PriorLevel = 1;
      BlockingUld = "AKE86233SQ";
      MoStage = "PKG";
      BlockingMoNo = 1280923;
    }

    public Tuple<List<string>, List<List<object>>> GetTable(bool withIndex, int limit) {
      List<MoInfoView> list = deserializeList();
      List<string> headers = new List<string> { "MONO", "ULDID", "EQPID", "CAT", "PUPOINT", "DOPOINT", "MOSTAT",
        "PUDTTM", "DEADLINE", "PRIORLEVEL", "BLOCKINGULD", "MOSTAGE", "BLOCKINGMONO" };
      if (withIndex)
        headers.Insert(0, "ADD_INDEX"); //added index
      List<List<object>> rows = new List<List<object>>();
      for (int i = 0; i < Math.Min(limit, list.Count); ++i) {
        List<object> row = new List<object>();
        MoInfoView item = list[i];
        if (withIndex) //if it is to be added with index
          row.Add(i + 1); //add index number as first element in the row
        row.Add(item.MoNo);
        row.Add(item.UldId);
        row.Add(item.EqpId);
        row.Add(item.Cat);
        row.Add(item.PuPoint);
        row.Add(item.DoPoint);
        row.Add(item.MoStat);
        row.Add(item.PuDtTm);
        row.Add(item.Deadline);
        row.Add(item.PriorLevel);
        row.Add(item.BlockingUld);
        row.Add(item.MoStage);
        row.Add(item.BlockingMoNo);
        rows.Add(row);
      }
      Tuple<List<string>, List<List<object>>> result = new Tuple<List<string>, List<List<object>>>(headers, rows);
      return result;
    }
  }

  //For simulation purpose only
  [XmlRoot("MoInfoViewList")]
  public class MoInfoViewList {
    [XmlElement("MoInfoView")]
    public List<MoInfoView> Items = new List<MoInfoView>();
  }

  //TABLE NOTE: By default, taken from VW_MOINFO table (Alias "Info")
  //all columns are taken
  //[0]       [1]          [2]     [3]   [4]       [5]       [6]      [7]                   [8]                   [9]          [10]          [11]      [12]         
  //MONO    | ULDID      | EQPID | CAT | PUPOINT | DOPOINT | MOSTAT | PUDTTM              | DEADLINE            | PRIORLEVEL | BLOCKINGULD | MOSTAGE | BLOCKINGMONO 
  //1280922   PMC58289SQ   ETV2    A     67A15     67A15     Q        1/3/2019 2:30:00 pm   1/3/2019 2:30:00 pm   5            AKE86233SQ    PKG       1280923
  //1280923   AKE86233SQ   ETV2    AW    69A15     69A15     ?        1/3/2019 2:30:00 pm   1/3/2019 2:30:00 pm   3                                    1280921
  //1280924   AKE81213SQ   ETV2    L     85A07     89A15     ?        1/3/2019 2:30:00 pm   1/3/2019 2:30:00 pm   1                                    1280925
}