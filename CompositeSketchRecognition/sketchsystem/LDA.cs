using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using System.ComponentModel;

namespace CompositeSketchRecognition
{
  [Serializable]
  public class LDA
  {
    public List<string> trainingSet { get; set; }
    public double[] projectingVectorHOG { get; set; }
    public double[] projectingVectorSIFT { get; set; }
    public LDA()
    {
      trainingSet = new List<string>();
    }

    public static LDA loadTraining(
            int TRAINING_SIZE, int TEST_SIZE,
            string SKETCH_PATH, string SKETCH_EXTENSION, string LDA_FILE_NAME,
            out List<string> trainingSetSketchesPath, out List<string> testSetSketchesPath
          )
    {

      LDA lda;
      List<FileInfo> files = new List<FileInfo>();
      DirectoryInfo dinfo = new DirectoryInfo(SKETCH_PATH);
      files.AddRange(dinfo.GetFiles(SKETCH_EXTENSION));
      Random rnd = new Random();

      if (File.Exists(LDA_FILE_NAME))
      {
        Stream openFileStream = File.OpenRead(LDA_FILE_NAME);
        BinaryFormatter deserializer = new BinaryFormatter();
        lda = (LDA)deserializer.Deserialize(openFileStream);
        openFileStream.Close();

        trainingSetSketchesPath = new List<string>();
        testSetSketchesPath = new List<string>();

        foreach (FileInfo file in files)
        {
          foreach (string train in lda.trainingSet)
          {
            if (!file.Name.StartsWith(train))
            {
              testSetSketchesPath.Add(file.FullName);
              break;
            }
          }

        }
        testSetSketchesPath = testSetSketchesPath.OrderBy(x => rnd.Next()).ToList();
        testSetSketchesPath = testSetSketchesPath.Take(TEST_SIZE).ToList();
      }
      else
      {
        lda = new LDA();

        files = files.OrderBy(x => rnd.Next()).ToList();
        List<FileInfo> trainingSet = files.Take(TRAINING_SIZE).ToList();
        List<FileInfo> testSet = files.Skip(TRAINING_SIZE).Take(TEST_SIZE).ToList();

        trainingSetSketchesPath = trainingSet.Select(file => file.FullName).ToList();
        testSetSketchesPath = testSet.Select(file => file.FullName).ToList();
        lda.trainingSet = trainingSetSketchesPath.Select(file => file.Substring(file.LastIndexOf('\\') + 1, 5)).ToList();
      }
      return lda;
    }

    public static void training(
        LDA lda, List<FaceDescriptor> descriptors, List<string> trainingSetSketchesPath,
        Func<string, string, bool, FaceDescriptor> processImage,
        int TRAINING_SIZE, string LDA_FILE_NAME, BackgroundWorker worker = null
        )
    {
      bool progress = (worker != null);
      if (lda.projectingVectorHOG == null)
      {
        List<FaceDescriptor> trainingDescriptors = new List<FaceDescriptor>();
        for (int i = 0; i < trainingSetSketchesPath.Count; i++)
        {
          FaceDescriptor face = processImage(trainingSetSketchesPath[i], lda.trainingSet[i], true);
          if (face != null)
            trainingDescriptors.Add(face);

          if (progress)
            worker.ReportProgress(i * 100 / trainingSetSketchesPath.Count);
        }

        if (progress)
          worker.ReportProgress(0);

        Dictionary<string, FaceDescriptor> descriptorsWithName = new Dictionary<string, FaceDescriptor>();

        foreach (var descriptor in descriptors)
        {
          string name = descriptor.Name.Substring(0, 5);
          if (!descriptorsWithName.ContainsKey(name))
          {
            descriptorsWithName.Add(name, descriptor);
          }
        }

        int NPoints = TRAINING_SIZE * 2;
        int NClasses = TRAINING_SIZE;
        int NVarsHOG = trainingDescriptors[0].DescriptorHog.Count();
        int NVarsSIFT = trainingDescriptors[0].DescriptorSift.Count();

        double[,] xyHOG = new double[NPoints, NVarsHOG + 1];
        double[,] xySIFT = new double[NPoints, NVarsSIFT + 1];
        for (int i = 0; i < TRAINING_SIZE; i++)
        {
          FaceDescriptor photoDescriptor = descriptorsWithName[trainingDescriptors[i].Name];

          for (int k = 0; k < NVarsHOG; k++)
          {
            xyHOG[i, k] = trainingDescriptors[i].DescriptorHog[k];
            xyHOG[i + TRAINING_SIZE, k] = photoDescriptor.DescriptorHog[k];
          }
          xyHOG[i, NVarsHOG] = i;
          xyHOG[i + TRAINING_SIZE, NVarsHOG] = i;

          for (int k = 0; k < NVarsSIFT; k++)
          {
            xySIFT[i, k] = trainingDescriptors[i].DescriptorSift[k];
            xySIFT[i + TRAINING_SIZE, k] = photoDescriptor.DescriptorSift[k];
          }
          xySIFT[i, NVarsSIFT] = i;
          xySIFT[i + TRAINING_SIZE, NVarsSIFT] = i;

          if (progress)
            worker.ReportProgress(i * 100 / TRAINING_SIZE);
        }

        int info = 0;
        double[] wHOG = new double[0];
        double[] wSIFT = new double[0];

        alglib.lda.fisherlda(xyHOG, NPoints, NVarsHOG, NClasses, ref info, ref wHOG, null);
        alglib.lda.fisherlda(xySIFT, NPoints, NVarsSIFT, NClasses, ref info, ref wSIFT, null);
        lda.projectingVectorHOG = wHOG;
        lda.projectingVectorSIFT = wSIFT;

        Stream SaveFileStream = File.Create(LDA_FILE_NAME);
        BinaryFormatter serializer = new BinaryFormatter();
        serializer.Serialize(SaveFileStream, lda);
        SaveFileStream.Close();
      }
    }
    
  }
}