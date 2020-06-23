using System;
using System.Collections.Generic;
using System.Linq;
using T5ShortestTime.SATS;

namespace T5ShortestTime.MOAct {
  public class Act {
    public int MoNumber; //derived from MO    
    public string EqpId = "";
    public bool IsPickUp; //to determine whether it is a pick up or a drop off
    public bool IsWorkstation; //to determine if the target is workstation or not
    public bool IsAirSide; //to determine if the target is the airside or not (for PU, it is FROM air side, for DO, it is TO airside)
    public int X = 1; //to get the x position, cannot be lower than 1
    public int Z; //to get the z position
    public ULDCategory ULDCat;
    public Act CoupleMOAct; //to assign to this MO what is its coupled Act    
    public double StaticTime = 0; //To gives the static time of doing this (apart from moving from one place to another)
    public double OriginalPriority = 0;
    public double EffectivePriority = 0; //adjusted if it is a blocking MoAct
    public List<Act> WsJoinedActs = new List<Act>(); //this is to list the acts joined by WS
                                                     //public Dictionary<Act, int> StaticMovingTimeNew = new Dictionary<Act, int>();
    public List<KeyValuePair<Act, int>> StaticMovingTimeOriginal = new List<KeyValuePair<Act, int>>();
    public List<List<KeyValuePair<Act, int>>> StaticMovingTimeDuplicates;
    public int BlockingMoNo = -1;
    public Act PresidingAct = null; //to tell if there is a blocking moAct here..
    public string LocString = "";
    public string Stat = "Q";

    public Act() {
    }

    public string GetString() {
      return string.Concat(
        "[",
        MoNumber,
        IsPickUp ? "PU" : "DO",
        IsWorkstation ? "W" : "N",
        EffectivePriority.ToString("f0"),
        EffectivePriority > OriginalPriority ? "U" : EffectivePriority == OriginalPriority ? "N" : "D",
        "]"
      );
    }

    public string GetInitString(List<Act> ipma) {
      string msg = string.Concat(
        "{00} InitMOAct\tX: ", X,
        " Z: ", (char)(Z + 'A'),
        " W: ", IsWorkstation ? "T" : "F",
        ", PU-Unfinished: ");
      return string.Concat(msg, string.Join(" ", ipma.Select(x => x.GetString())));
    }

    public string GetSMTString(int no) {
      List<KeyValuePair<Act, int>> smt = StaticMovingTimeOriginal;
      return string.Concat(
        "{", (no + 1).ToString("d2"), "} MOAct [",
        smt[no].Key.MoNumber.ToString("d2"), "-", smt[no].Key.IsPickUp ? "PU]" : "DO]",
        " ", smt[no].Key.ULDCat.ToString(),
        "\tX: ", smt[no].Key.X,
        " Z: ", (char)(smt[no].Key.Z + 'A'),
        " S: ", smt[no].Key.IsAirSide ? "A" : "L",
        "\tB: ", smt[no].Key.BlockingMoNo,
        "\tW: ", smt[no].Key.IsWorkstation ? "T" : "F",
        "\tP: ", smt[no].Key.EffectivePriority, "-", smt[no].Key.OriginalPriority,
        "\tMoving Time: ", (0.1 * smt[no].Value).ToString("f1"), " second(s)",
        smt[no].Key == CoupleMOAct ? " (couple)" : "");
    }

    public string GetFullInfoString(int no = 0) {
      return string.Concat(
        "{", (no + 1).ToString("d2"), "} MOAct [",
        MoNumber.ToString("d2"), "-", IsPickUp ? "PU]" : "DO]",
        " ", ULDCat.ToString(),
        "\tX: ", X,
        " Z: ", (char)(Z + 'A'),
        " S: ", IsAirSide ? "A" : "L",
        "\tB: ", BlockingMoNo,
        "\tW: ", IsWorkstation ? "T" : "F",
        "\tP: ", EffectivePriority, "-", OriginalPriority,
        "\tCouple MOAct [", CoupleMOAct.MoNumber.ToString("d2"),
        "-", CoupleMOAct.IsPickUp ? "PU]" : "DO]",
        " " + CoupleMOAct.ULDCat.ToString(),
        " X: ", CoupleMOAct.X,
        " Z: ", (char)(CoupleMOAct.Z + 'A'),
        " S: ", CoupleMOAct.IsAirSide ? "A" : "L",
        " W: ", CoupleMOAct.IsWorkstation ? "T" : "F",
        ", Legal Move(s): ", StaticMovingTimeOriginal.Count);
    }

    public string GetFullInfoAndSMTString(int no = 0) {
      int index = 0;
      return string.Concat(
        string.Concat(GetFullInfoString(no), Environment.NewLine),
        "  ",
        string.Join("  ",
          StaticMovingTimeOriginal
            .Select(x => string.Concat(GetSMTString(index++), Environment.NewLine))));
    }

    public override string ToString() {
      return string.Concat(MoNumber, IsPickUp ? "[PU]" : "[DO]", IsAirSide ? "[A]" : "[L]");
    }
  }
}
