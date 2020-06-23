using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T5ShortestTime.Models {
  /// <summary>
  /// The interface for simulated model
  /// </summary>
  public interface ISimulatedModel {
    //All simulation must have the ApplyTestData Method
    void ApplyTestData();

    //All simulation must provide a way for its data to be obtained in table form
    //Tuple<List<string>, List<List<object>>> GetTable(bool withIndex, int limit);
  }
}
