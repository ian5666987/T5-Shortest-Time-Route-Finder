using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T5ShortestTime.SATS;

namespace T5ShortestTime.MOAct {
  public class MO {
    public string EquipmentId { get; set; } = string.Empty;
    public int Number { get; set; } //completely useless, but may be useful for indexing
    public int PickUpX;
    public int PickUpZ;
    public int DropOffX;
    public int DropOffZ;
    public LaneSide PickUpSide = LaneSide.AirSide;
    public LaneSide DropOffSide = LaneSide.LandSide;
    public ULDCategory ULDCat { get; set; } = ULDCategory.U;
    public bool PickUpIsWorkstation { get; set; }
    public bool DropOffIsWorkstation { get; set; }
    public double Priority { get; set; }
    public string OriginalPickUpString { get; set; } = string.Empty;
    public string OriginalDroffOffString { get; set; } = string.Empty;
    public string PickUpString = string.Empty;
    public string DropOffString = string.Empty;
    public int BlockingNumber { get; set; } = -1;
    public string Stat { get; set; } = "Q";

    public MO(string pu_str, string do_str, string cat_str) {
      int sideVal = Convert.ToInt32(pu_str.Substring(3, 2));
      PickUpX = Convert.ToInt32(pu_str.Substring(0, 2));
      PickUpZ = (int)(pu_str[2]) - (int)('A'); //0 to 5
      PickUpSide = sideVal <= 12 ? LaneSide.AirSide : LaneSide.LandSide;
      PickUpString = pu_str.Substring(0, 3) + (sideVal <= 12 ? "12" : "15"); //force it to either be 12 or 15
      PickUpIsWorkstation = Workstation.IsPickUpWorkstation(PickUpString);
      OriginalPickUpString = pu_str;

      sideVal = Convert.ToInt32(do_str.Substring(3, 2));
      DropOffX = Convert.ToInt32(do_str.Substring(0, 2));
      DropOffZ = (int)(do_str[2]) - (int)('A'); //0 to 5
      DropOffSide = sideVal <= 12 ? LaneSide.AirSide : LaneSide.LandSide;
      DropOffString = do_str.Substring(0, 3) + (sideVal <= 12 ? "12" : "15"); //force it to either be 12 or 15			
      DropOffIsWorkstation = Workstation.IsDropOffWorkstation(DropOffString);
      OriginalDroffOffString = do_str;

      List<string> uldCatList = Enum.GetNames(typeof(ULDCategory)).ToList();
      if (!string.IsNullOrWhiteSpace(cat_str) && uldCatList.Contains(cat_str))
        ULDCat = (ULDCategory)Enum.Parse(typeof(ULDCategory), cat_str);
    }
  }

}
