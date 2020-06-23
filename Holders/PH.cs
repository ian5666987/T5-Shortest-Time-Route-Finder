using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T5ShortestTime {
  /// <summary>
  /// The parameter holder class
  /// </summary>
  public class PH {
    public static bool IsSimulation { get; set; }
    public static string EqpInfoSimulationFilepath { get; set; }
    public static string LedInfoSimulationFilepath { get; set; }
    public static string MoInfoViewSimulationFilepath { get; set; }
    public static string OptFlagSimulationFilepath { get; set; }
    public static string OptSolutionBaseSimulationFilepath { get; set; }
  }
}
