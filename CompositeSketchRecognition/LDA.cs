using System;
using System.Collections.Generic;

namespace CompositeSketchRecognition
{
  [Serializable]
  public class LDA
  {
    public List<string> trainingSet { get; set; }
    public double[] projectingVectorHOG { get; set; }
    public double[] projectingVectorSIFT { get; set; }
    public LDA() { 
      trainingSet = new List<string>();
    }
  }
}