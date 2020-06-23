using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace T5ShortestTime.MOAct {
  public class ActLeftBase {
    [XmlElement]
    public int No = 0;
    [XmlElement]
    public string PUPoint = "";
    [XmlElement]
    public string DOPoint = "";
    [XmlElement]
    public double PriorLevel = 0;
    [XmlElement]
    public string ULDCat = "";
    public override string ToString() {
      return string.Concat(new object[]
      { "[", No, "-P", PriorLevel, "-", ULDCat, "]" });
    }
  }

  [XmlRoot, Serializable]
  public class ActInitialState {
    [XmlElement]
    public int InitialX = 1;
    [XmlElement]
    public char InitialZ = 'A';
    [XmlArray]
    [XmlArrayItem("Act")]
    public List<ActLeftBase> LeftActs = new List<ActLeftBase>();
    public DateTime InitialDateTime = DateTime.Now;
    public override string ToString() {
      IEnumerable<string> lefts = LeftActs == null ? new List<string>() : LeftActs.Select(x => x.ToString());
      return string.Concat(new object[] { "X: ", InitialX, " | Z: ", InitialZ,
      " | Act(s) Left: ", (LeftActs == null ? 0 : LeftActs.Count),
      (lefts.Any() ? string.Concat(" ", string.Join("", lefts)) : ""),
      " | Init Date: ", InitialDateTime.ToString("dd-MM-yyyy HH:mm:ss.fff")});
    }
  }
}
