﻿using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

namespace CompositeSketchRecognition
{
    class ImageRetreivalSystem
    {
        public const string PHOTO_PATH = @"..\..\..\database\UoM-SGFS-v2\Photos\Images\";
        public const string SKETCH_PATH = @"..\..\..\database\UoM-SGFS-v2\Sketches\Set A\Images\";
        public const string PHOTO_EXTENSION = "*.ppm";
        public const string SKETCH_EXTENSION = "*.bmp";

        public const string OTHER_PHOTO_PATH = @"..\..\..\database\UoM-SGFS-v2\Photos\Others\";
        public const string OTHER_PHOTO_EXTENSION = "*.jpg";

        public const string DESCRIPTOR_FILE_NAME = "descriptors.bin";
        public const string LDA_FILE_NAME = "lda.bin";

        public const int TRAINING_SIZE = 150;
        public const int TEST_SIZE = 150;

        FaceDetection faceDetection = new FaceDetection();
        HogDescriptor hogDescriptor = new HogDescriptor();
        SiftDescriptor siftDescriptor = new SiftDescriptor();

        List<FaceDescriptor> descriptors;
        LDA lda;
        List<String> trainingSetSketchesPath;
        List<String> testSetSketchesPath;
        SortedDictionary<double, string> resultHog =
            new SortedDictionary<double, string>();
        SortedDictionary<double, string> resultSift =
            new SortedDictionary<double, string>();
        SortedDictionary<double, string> resultBordaCount =
            new SortedDictionary<double, string>();

        string sketchName = "";

        //test on all images
        public void accuracyTest(BackgroundWorker worker, out double rank1, out double rank10, out double rank50, out double rank100, out double rank200)
        {
            rank1 = 0;
            rank10 = 0;
            rank50 = 0;
            rank100 = 0;
            rank200 = 0;
            if (lda == null)
                lda = LDA.loadTraining(
                    TRAINING_SIZE, TEST_SIZE,
                    SKETCH_PATH, SKETCH_EXTENSION, LDA_FILE_NAME,
                    out trainingSetSketchesPath, out testSetSketchesPath
                );
            for (int i = 0; i < testSetSketchesPath.Count; i++)
            {
                search(testSetSketchesPath[i], worker, true);
                var index = getSubjectIndex();
                if (index == 1)
                {
                    rank1++;
                }
                if (index <= 10)
                {
                    rank10++;
                }
                if (index <= 50)
                {
                    rank50++;
                }
                if (index <= 100)
                {
                    rank100++;
                }
                if (index <= 200)
                {
                    rank200++;
                }
                worker.ReportProgress(i * 100 / testSetSketchesPath.Count);
            }

            rank1 /= testSetSketchesPath.Count;
            rank10 /= testSetSketchesPath.Count;
            rank50 /= testSetSketchesPath.Count;
            rank100 /= testSetSketchesPath.Count;
            rank200 /= testSetSketchesPath.Count;
            worker.ReportProgress(100);
        }

        
        

        // main query method
        public void search(String sketchPath, BackgroundWorker worker = null, bool progressOnlyDescriptor = false)
        {
            bool progress = (worker != null);
            if (sketchPath.Equals("")) { return; }

            if(lda == null)
                lda = LDA.loadTraining(
                    TRAINING_SIZE, TEST_SIZE,
                    SKETCH_PATH, SKETCH_EXTENSION, LDA_FILE_NAME,
                    out trainingSetSketchesPath, out testSetSketchesPath
                );

            if (descriptors == null)
            {
                if (File.Exists(DESCRIPTOR_FILE_NAME))
                {
                    Stream openFileStream = File.OpenRead(DESCRIPTOR_FILE_NAME);
                    BinaryFormatter deserializer = new BinaryFormatter();
                    descriptors = (List<FaceDescriptor>)deserializer.Deserialize(openFileStream);
                    openFileStream.Close();
                }
                else
                {
                    descriptors = new List<FaceDescriptor>();
                    List<FileInfo> files = new List<FileInfo>();
                    DirectoryInfo dinfo = new DirectoryInfo(PHOTO_PATH);
                    files.AddRange(dinfo.GetFiles(PHOTO_EXTENSION));
                    dinfo = new DirectoryInfo(OTHER_PHOTO_PATH);
                    files.AddRange(dinfo.GetFiles(PHOTO_EXTENSION));
                    files.AddRange(dinfo.GetFiles(OTHER_PHOTO_EXTENSION));

                    for (int i = 0; i < files.Count; i++)
                    {
                        FaceDescriptor face = processImage(files[i].FullName, files[i].Name, true);
                        if(face != null)
                            descriptors.Add(face);

                        if(progress)
                            worker.ReportProgress(i * 100 / files.Count);
                    }

                    Stream SaveFileStream = File.Create(DESCRIPTOR_FILE_NAME);
                    BinaryFormatter serializer = new BinaryFormatter();
                    serializer.Serialize(SaveFileStream, descriptors);
                    SaveFileStream.Close();
                }

                LDA.training(lda, descriptors, trainingSetSketchesPath,
                    processImage, TRAINING_SIZE, LDA_FILE_NAME, worker);

                for (int i = 0; i < descriptors.Count; i++)
                {
                    for(int k = 0; k < descriptors[i].DescriptorHog.Count(); k++){
                        descriptors[i].DescriptorHog[k] *= (float)lda.projectingVectorHOG[k];
                    }
                    for(int k = 0; k < descriptors[i].DescriptorSift.Count(); k++){
                        descriptors[i].DescriptorSift[k] *= (float)lda.projectingVectorSIFT[k];
                    }
                }
            }


            if (progress && !progressOnlyDescriptor)
                worker.ReportProgress(0);

            sketchName = sketchPath.Substring(sketchPath.LastIndexOf('\\') + 1, 5);
            FaceDescriptor sketchFace = processImage(sketchPath, sketchName, false);

            resultHog = new SortedDictionary<double, string>();
            resultSift = new SortedDictionary<double, string>();
            resultBordaCount = new SortedDictionary<double, string>();

            for(int i = 0; i < sketchFace.DescriptorHog.Count(); i++){
                sketchFace.DescriptorHog[i] *= (float)lda.projectingVectorHOG[i];
            }
            for(int i = 0; i < sketchFace.DescriptorSift.Count(); i++){
                sketchFace.DescriptorSift[i] *= (float)lda.projectingVectorSIFT[i];
            }

            int hogSize = 540;
            int siftSize = 512;

            for (int i = 0; i < descriptors.Count; i++)
            {
                addDictionaryUnique(
                    euclideanDistance(
                        sketchFace.DescriptorHog, descriptors[i].DescriptorHog, hogSize
                    ), descriptors[i].Name, resultHog);
                addDictionaryUnique(
                    euclideanDistance(
                        sketchFace.DescriptorSift, descriptors[i].DescriptorSift, siftSize
                    ), descriptors[i].Name, resultSift);
                if (progress && !progressOnlyDescriptor)
                    worker.ReportProgress(i * 100 / descriptors.Count);
            }

            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            int count = 0;
            foreach (var r in resultHog)
            {
                dictionary.Add(r.Value, (resultHog.Count - count) * 2);//weight
                count++;
            }
            count = 0;
            foreach (var r in resultSift)
            {
                dictionary[r.Value] += resultSift.Count - count;
                count++;
            }
            foreach (var r in dictionary)
            {
                double value = r.Value;
                //negative int because sorted dictionary work in ascending mode only
                addDictionaryUnique(-r.Value, r.Key, resultBordaCount);
            }

            if (progress && !progressOnlyDescriptor)
                worker.ReportProgress(100);
        }

        void addDictionaryUnique(double key, String value, SortedDictionary<double, string> dictionary)
        {
            while (dictionary.ContainsKey(key))
            {
                key += 0.0001;
            }
            dictionary.Add(key, value);
        }

        double euclideanDistance(float[] q, float[] p, int size)
        {
            double distance = 0;
            for (int i = 0; i < q.Length; i++)
            {
                int weight = 1;
                if (i < size * 5)//mouth
                {
                    weight = 2;
                }
                if (i < size * 4)//nose
                {
                    weight = 1;
                }
                if (i < size * 3)//eyes
                {
                    weight = 1;
                }
                if (i < size * 2)//brow
                {
                    weight = 1;
                }
                if (i < size)//hair
                {
                    weight = 6;
                }
                distance += Math.Pow(q[i] - p[i], 2) * weight;
            }
            return Math.Sqrt(distance);
        }

        FaceDescriptor processImage(string path, string name, bool isPhoto)
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(path);
            Rectangle realFace; Rectangle[] realEyes; Rectangle realMouth;
            Rectangle[] faces; Rectangle[] eyes; Rectangle[] mouths;//placeholder for out, not important for query
            faceDetection.faceAndLandmarks(image, out realFace, out realEyes, out realMouth, out faces, out eyes, out mouths);
            if (realFace.Width == 0) { return null; }
            var extendedFace = faceDetection.extendFace(image, realFace, faceDetection.faceOutline(image));
            image = image.GetSubRect(extendedFace);

            FaceDescriptor face = new FaceDescriptor(name);

            realMouth.X += realFace.X - extendedFace.X;
            realMouth.Y += realFace.Y - extendedFace.Y;

            Point leftEye = new Point(); Point rightEye = new Point();
            if (realEyes.Length == 2)
            {
                faceDetection.eyesCenter(realEyes, out leftEye, out rightEye);
                leftEye.X += realFace.X - extendedFace.X;
                leftEye.Y += realFace.Y - extendedFace.Y;
                rightEye.X += realFace.X - extendedFace.X;
                rightEye.Y += realFace.Y - extendedFace.Y;

                if(isPhoto){
                    Point rotatedLeftEye; Point rotatedRightEye;
                    image = faceDetection.alignEyes(image, leftEye, rightEye, out rotatedLeftEye, out rotatedRightEye);
                    leftEye = rotatedLeftEye;
                    rightEye = rotatedRightEye;

                    realMouth = faceDetection.getMouth(
                        image.GetSubRect(
                        new Rectangle(new Point(0, 0), new Size((int)(image.Width * 0.9), (int)(image.Height * 0.9)))
                    ));
                }
            }

            Rectangle hair; Rectangle brow; Rectangle roiEyes; Rectangle nose;
            faceDetection.faceROI(image, leftEye, rightEye, realMouth,
                out hair, out brow, out roiEyes, out nose, out realMouth);

            Image<Bgr, byte> hairImage; Image<Bgr, byte> browImage;
            Image<Bgr, byte> eyesImage; Image<Bgr, byte> noseImage; Image<Bgr, byte> mouthImge;
            roiToFixedImage(image, hair, brow, roiEyes, nose, realMouth,
                out hairImage, out browImage, out eyesImage, out noseImage, out mouthImge);

            face.HairHog = hogDescriptor.GetHog(hairImage, 32, 16);
            face.BrowHog = hogDescriptor.GetHog(browImage, 32, 16);
            face.EyesHog = hogDescriptor.GetHog(eyesImage, 32, 16);
            face.NoseHog = hogDescriptor.GetHog(noseImage, 16, 8);
            face.MouthHog = hogDescriptor.GetHog(mouthImge, 16, 8);

            face.HairSift = siftDescriptor.ComputeDescriptor(hairImage, 24, 16);
            face.BrowSift = siftDescriptor.ComputeDescriptor(browImage, 24, 16);
            face.EyesSift = siftDescriptor.ComputeDescriptor(eyesImage, 24, 16);
            face.NoseSift = siftDescriptor.ComputeDescriptor(noseImage, 8, 12);
            face.MouthSift = siftDescriptor.ComputeDescriptor(mouthImge, 12, 8);

            face.processDescriptors();
            return face;
        }

        void roiToFixedImage(Image<Bgr, Byte> face, Rectangle hairRoi, Rectangle browRoi, Rectangle eyesRoi, Rectangle noseRoi, Rectangle mouthRoi,
            out Image<Bgr, byte> hair, out Image<Bgr, byte> brow, out Image<Bgr, byte> eyes, out Image<Bgr, byte> nose, out Image<Bgr, byte> mouth)
        {
            hair = face.GetSubRect(hairRoi).Resize(96, 64, Inter.Linear);
            brow = face.GetSubRect(browRoi).Resize(96, 64, Inter.Linear);
            eyes = face.GetSubRect(eyesRoi).Resize(96, 64, Inter.Linear);
            nose = face.GetSubRect(noseRoi).Resize(32, 48, Inter.Linear);
            mouth = face.GetSubRect(mouthRoi).Resize(48, 32, Inter.Linear);
        }






        // retrieve results
        public Image<Bgr, byte> getImage(int index)
        {
            int count = 0;
            foreach (var r in resultBordaCount)
            {
                if (count == index)
                {
                    Console.WriteLine("photo " + index + " distance: " + r.Key);
                    var path = PHOTO_PATH + r.Value;
                    if (!File.Exists(path))
                    {
                        path = OTHER_PHOTO_PATH + r.Value;
                        if (!File.Exists(path))
                        {
                            return null;
                        }
                    }
                    return new Image<Bgr, byte>(path);
                }
                count++;
            }
            return null;
        }

        public int getSubjectIndex()
        {
            int index = -1;
            int count = 1;
            foreach (var r in resultBordaCount)
            {
                if (r.Value.StartsWith(sketchName))
                {
                    index = count;
                    break;
                }
                count++;
            }
            return index;
        }







        // step by step process

        public Image<Bgr, byte> getStepImage(String imagePath, int index, String sketchPath, bool inverted)
        {
            if (imagePath == null || imagePath.Equals(""))
            {
                return null;
            }

            Image<Bgr, byte> image = new Image<Bgr, byte>(imagePath);

            if (index == 0)
            {
                return new Image<Bgr, byte>(imagePath);
            }

            Rectangle realFace; Rectangle[] realEyes; Rectangle realMouth;
            Rectangle[] faces; Rectangle[] eyes; Rectangle[] mouths;
            faceDetection.faceAndLandmarks(image, out realFace, out realEyes, out realMouth, out faces, out eyes, out mouths);

            if (index == 1 || index == 2 || index == 4)
            {
                Image<Bgr, byte> drawImage = image.Copy();
                var cutDrawFace = drawImage.GetSubRect(realFace);

                if (index == 1)
                {
                    foreach (var face in faces)
                    {
                        drawImage.Draw(face, new Bgr(Color.LightBlue), 3);
                    }
                    foreach (var eye in eyes)
                    {
                        cutDrawFace.Draw(eye, new Bgr(Color.Magenta), 3);
                    }
                    foreach (var mouth in mouths)
                    {
                        cutDrawFace.Draw(mouth, new Bgr(Color.LightGreen), 3);
                    }
                }

                foreach (var eye in realEyes)
                {
                    cutDrawFace.Draw(eye, new Bgr(Color.DarkMagenta), 3);
                }
                cutDrawFace.Draw(realMouth, new Bgr(Color.DarkGreen), 3);

                if (index == 4)
                {
                    drawImage.Draw(faceDetection.extendFace(image, realFace, faceDetection.faceOutline(image)), new Bgr(Color.GreenYellow), 3);
                }
                else
                {
                    drawImage.Draw(realFace, new Bgr(Color.DarkBlue), 3);
                }

                return drawImage;
            }
            if (index == 3)
            {
                return faceDetection.faceOutline(image).Convert<Bgr, byte>();
            }

            var extendedFace = faceDetection.extendFace(image, realFace, faceDetection.faceOutline(image));
            var cutFace = image.GetSubRect(extendedFace);

            realMouth.X += realFace.X - extendedFace.X;
            realMouth.Y += realFace.Y - extendedFace.Y;

            Point leftEye = new Point();
            Point rightEye = new Point();

            if (realEyes.Length == 2)
            {
                faceDetection.eyesCenter(realEyes, out leftEye, out rightEye);
                leftEye.X += realFace.X - extendedFace.X;
                leftEye.Y += realFace.Y - extendedFace.Y;
                rightEye.X += realFace.X - extendedFace.X;
                rightEye.Y += realFace.Y - extendedFace.Y;
            }

            if (!inverted && realEyes.Length == 2)
            {
                Point rotatedLeftEye;
                Point rotatedRightEye;
                cutFace = faceDetection.alignEyes(cutFace, leftEye, rightEye, out rotatedLeftEye, out rotatedRightEye);
                leftEye = rotatedLeftEye;
                rightEye = rotatedRightEye;

                if (index == 5)
                {
                    CvInvoke.Circle(cutFace, leftEye, 2, new MCvScalar(255, 255, 255));
                    CvInvoke.Circle(cutFace, rightEye, 2, new MCvScalar(255, 255, 255));
                    return cutFace;
                }

                realMouth = faceDetection.getMouth(
                    cutFace.GetSubRect(
                        new Rectangle(new Point(0, 0), new Size((int)(cutFace.Width * 0.9), (int)(cutFace.Height * 0.9)))
                    ));

            }
            if (index == 5)
            {
                return cutFace;
            }

            Rectangle hair; Rectangle brow; Rectangle roiEyes; Rectangle nose;
            faceDetection.faceROI(cutFace, leftEye, rightEye, realMouth, out hair, out brow, out roiEyes, out nose, out realMouth);

            if (index == 6)
            {
                cutFace.Draw(hair, new Bgr(Color.Yellow), 3);
                cutFace.Draw(brow, new Bgr(Color.Violet), 3);
                cutFace.Draw(roiEyes, new Bgr(Color.DarkMagenta), 3);
                cutFace.Draw(nose, new Bgr(Color.DarkBlue), 3);
                cutFace.Draw(realMouth, new Bgr(Color.DarkGreen), 3);
                return cutFace;
            }

            Image<Bgr, byte> hairImage; Image<Bgr, byte> browImage; Image<Bgr, byte> eyesImage; Image<Bgr, byte> noseImage; Image<Bgr, byte> mouthImge;
            roiToFixedImage(cutFace, hair, brow, roiEyes, nose, realMouth,
                out hairImage, out browImage, out eyesImage, out noseImage, out mouthImge);

            if (index == 7)
            {
                return hogDescriptor.hogVisualization(hairImage, hogDescriptor.GetHog(hairImage, 32, 16), 16);
            }
            if (index == 8)
            {
                return hogDescriptor.hogVisualization(browImage, hogDescriptor.GetHog(browImage, 32, 16), 16);
            }
            if (index == 9)
            {
                return hogDescriptor.hogVisualization(eyesImage, hogDescriptor.GetHog(eyesImage, 32, 16), 16);
            }
            if (index == 10)
            {
                return hogDescriptor.hogVisualization(noseImage, hogDescriptor.GetHog(noseImage, 16, 8), 8);
            }
            if (index == 11)
            {
                return hogDescriptor.hogVisualization(mouthImge, hogDescriptor.GetHog(mouthImge, 16, 8), 8);
            }

            return null;
        }
    }
}
