using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Runtime.Serialization.Formatters.Binary;

namespace CompositeSketchRecognition
{
    class ImageRetreivalSystem
    {
        public const string PHOTO_PATH = @"..\..\..\database\UoM-SGFS-v2\Photos\Images\";
        public const string SKETCH_PATH = @"..\..\..\database\UoM-SGFS-v2\Sketches\Set A\Images\";
        public const string PHOTO_EXTENSION = "*.ppm";

        public const string DB_NAME = "db.bin";

        public const int HOG_WIDTH = 144;
        public const int HOG_HEIGHT = 176;

        public Size standardSize = new Size(208, 256);

        FaceDetection faceDetection = new FaceDetection();
        HogDescriptor hogDescriptor = new HogDescriptor();

        List<Face> db;
        SortedDictionary<double, string> result =
            new SortedDictionary<double, string>();
        String sketchName = "";

        public void search(String sketchPath, BackgroundWorker worker)
        {
            if (sketchPath.Equals("")) { return; }
            this.sketchName = sketchPath.Substring(sketchPath.LastIndexOf('\\') + 1, 5);
            //for out, not important here
            Rectangle[] faces;
            Rectangle[] eyes;
            Rectangle[] mouths;

            Image<Bgr, byte> sketch = new Image<Bgr, byte>(sketchPath);

            if (db == null)
            {
                if (File.Exists(DB_NAME))
                {
                    Stream openFileStream = File.OpenRead(DB_NAME);
                    BinaryFormatter deserializer = new BinaryFormatter();
                    db = (List<Face>)deserializer.Deserialize(openFileStream);
                    openFileStream.Close();
                }
                else
                {
                    db = new List<Face>();
                    DirectoryInfo dinfo = new DirectoryInfo(PHOTO_PATH);
                    FileInfo[] Files = dinfo.GetFiles(PHOTO_EXTENSION);
                    for (int i = 0; i < Files.Length; i++)
                    {
                        Image<Bgr, byte> image = new Image<Bgr, byte>(Files[i].FullName);
                        Rectangle realFace;
                        Rectangle[] realEyes;
                        Rectangle realMouth;
                        faceDetection.faceAndLandmarks(image, out realFace, out realEyes, out realMouth, out faces, out eyes, out mouths);
                        var extendedFace = faceDetection.extendFace(image, realFace, faceDetection.faceOutline(image));
                        image = image.GetSubRect(extendedFace);

                        Face face = new Face(image, Files[i].Name);

                        realMouth.X += realFace.X - extendedFace.X;
                        realMouth.Y += realFace.Y - extendedFace.Y;

                        face.mouth = realMouth;

                        Point leftEye = new Point();
                        Point rightEye = new Point();
                        if (realEyes.Length == 2)
                        {
                            faceDetection.getEyesCenter(realEyes, out leftEye, out rightEye);
                            leftEye.X += realFace.X - extendedFace.X;
                            leftEye.Y += realFace.Y - extendedFace.Y;
                            rightEye.X += realFace.X - extendedFace.X;
                            rightEye.Y += realFace.Y - extendedFace.Y;
                            Point rotatedLeftEye;
                            Point rotatedRightEye;
                            image = faceDetection.alignEyes(image, leftEye, rightEye, out rotatedLeftEye, out rotatedRightEye);
                            face.leftEye = rotatedLeftEye;
                            face.rightEye = rotatedRightEye;
                        }
                        db.Add(face);

                        worker.ReportProgress(i * 100 / Files.Length);
                    }

                    Stream SaveFileStream = File.Create(DB_NAME);
                    BinaryFormatter serializer = new BinaryFormatter();
                    serializer.Serialize(SaveFileStream, db);
                    SaveFileStream.Close();
                }
            }

            worker.ReportProgress(0);

            Rectangle sketchRealFace;
            Rectangle[] sketchRealEyes;
            Rectangle sketchRealMouth;
            faceDetection.faceAndLandmarks(sketch, out sketchRealFace, out sketchRealEyes, out sketchRealMouth, out faces, out eyes, out mouths);
            var extendedSketchface = faceDetection.extendFace(sketch, sketchRealFace, faceDetection.faceOutline(sketch));
            sketch = sketch.GetSubRect(extendedSketchface);

            var sketchHog = hogDescriptor.GetHog(processForHOG(sketch, true).Resize(HOG_WIDTH, HOG_HEIGHT, Inter.Linear));

            Console.WriteLine("Db lenght" + db.Count);

            result = new SortedDictionary<double, string>();

            for (int i = 0; i < db.Count; i++)
            {
                var faceImage = db[i].image;
                if (db[i].leftEye != null && db[i].rightEye != null
                    && !db[i].leftEye.Equals(new Point()) && !db[i].rightEye.Equals(new Point())
                    && sketchRealEyes.Length == 2)
                {
                    Point sketchLeftEye;
                    Point sketchRightEye;
                    faceDetection.getEyesCenter(sketchRealEyes, out sketchLeftEye, out sketchRightEye);
                    sketchLeftEye.X += sketchRealFace.X - extendedSketchface.X;
                    sketchLeftEye.Y += sketchRealFace.Y - extendedSketchface.Y;
                    sketchRightEye.X += sketchRealFace.X - extendedSketchface.X;
                    sketchRightEye.Y += sketchRealFace.Y - extendedSketchface.Y;

                    faceImage = faceDetection.alignFaces(db[i].image, sketch, db[i].leftEye, db[i].rightEye, sketchLeftEye, sketchRightEye);
                }
                
                var faceHog = hogDescriptor.GetHog(processForHOG(faceImage, false).Resize(HOG_WIDTH, HOG_HEIGHT, Inter.Linear));

                result.Add(euclideanDistance(sketchHog, faceHog), db[i].name);

                worker.ReportProgress(i * 100 / db.Count);
            }

            worker.ReportProgress(100);
        }

        public Image<Bgr, byte> getImage(int index)
        {
            int count = 0;
            foreach (var r in result)
            {
                if (count == index)
                {
                    Console.WriteLine("photo " + index + " distance: " + r.Key);
                    return new Image<Bgr, byte>(PHOTO_PATH + r.Value);
                }
                count++;
            }
            return null;
        }

        public int getSubjectIndex()
        {
            int index = -1;
            int count = 0;
            foreach (var r in result)
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


        double euclideanDistance(float[] q, float[] p)
        {
            double distance = 0;
            for (int i = 0; i < q.Length; i++)
            {
                distance += Math.Pow(q[i] - p[i], 2);
            }
            return Math.Sqrt(distance);
        }

        Image<Gray, Byte> processForHOG(Image<Bgr, byte> image, bool isSketch)
        {
            var edge = image.Convert<Gray, byte>();
            var gx = edge.Sobel(1, 0, 3);
            var gy = edge.Sobel(0, 1, 3);

            var gradientMagnitude = new Image<Gray, float>(edge.Width, edge.Height);

            for (int y = 0; y < edge.Height; y++)
            {
                for (int x = 0; x < edge.Width; x++)
                {
                    var gradient = Math.Sqrt(gx[y, x].Intensity * gx[y, x].Intensity + gy[y, x].Intensity * gy[y, x].Intensity);
                    gradientMagnitude[y, x] = new Gray(gradient);
                }
            }
            /*if (isSketch)
            {
                gradientMagnitude = gradientMagnitude.ThresholdBinary(new Gray(30), new Gray(255));
            }
            else
            {
                gradientMagnitude = gradientMagnitude.ThresholdBinary(new Gray(20), new Gray(255));//.Dilate(1).Erode(1);
            }*/
            

            return gradientMagnitude.Convert<Gray, byte>();
        }



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

            Rectangle realFace;
            Rectangle[] realEyes;
            Rectangle realMouth;
            Rectangle[] faces;
            Rectangle[] eyes;
            Rectangle[] mouths;
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






            if (sketchPath != null)
            {
                if (inverted)
                {
                    image = new Image<Bgr, byte>(sketchPath);
                    faceDetection.faceAndLandmarks(image, out realFace, out realEyes, out realMouth, out faces, out eyes, out mouths);
                    sketchPath = imagePath;
                }

                var extendedFace = faceDetection.extendFace(image, realFace, faceDetection.faceOutline(image));
                var cutFace = image.GetSubRect(extendedFace);

                realMouth.X += realFace.X - extendedFace.X;
                realMouth.Y += realFace.Y - extendedFace.Y;

                Point leftEye = new Point();
                Point rightEye = new Point();
                if (realEyes.Length == 2)
                {
                    faceDetection.getEyesCenter(realEyes, out leftEye, out rightEye);
                    leftEye.X += realFace.X - extendedFace.X;
                    leftEye.Y += realFace.Y - extendedFace.Y;
                    rightEye.X += realFace.X - extendedFace.X;
                    rightEye.Y += realFace.Y - extendedFace.Y;
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
                }

                Rectangle sketchRealFace;
                Rectangle[] sketchRealEyes;
                Rectangle sketchRealMouth;
                Image<Bgr, byte> sketch = new Image<Bgr, byte>(sketchPath);
                faceDetection.faceAndLandmarks(sketch, out sketchRealFace, out sketchRealEyes, out sketchRealMouth, out faces, out eyes, out mouths);
                var extendedSketchface = faceDetection.extendFace(sketch, sketchRealFace, faceDetection.faceOutline(sketch));
                var sketchCutFace = sketch.GetSubRect(extendedSketchface);

                if (realEyes.Length == 2 && sketchRealEyes.Length == 2)
                {
                    Point sketchLeftEye;
                    Point sketchRightEye;
                    faceDetection.getEyesCenter(sketchRealEyes, out sketchLeftEye, out sketchRightEye);
                    sketchLeftEye.X += sketchRealFace.X - extendedSketchface.X;
                    sketchLeftEye.Y += sketchRealFace.Y - extendedSketchface.Y;
                    sketchRightEye.X += sketchRealFace.X - extendedSketchface.X;
                    sketchRightEye.Y += sketchRealFace.Y - extendedSketchface.Y;

                    cutFace = faceDetection.alignFaces(cutFace, sketchCutFace, leftEye, rightEye, sketchLeftEye, sketchRightEye);

                    double xPercentLeft = (double)sketchLeftEye.X / (double)sketchCutFace.Width;
                    double xPercentRight = (double)sketchRightEye.X / (double)sketchCutFace.Width;
                    double yPercentLeft = (double)sketchLeftEye.Y / (double)sketchCutFace.Height;
                    double yPercentRight = (double)sketchRightEye.Y / (double)sketchCutFace.Height;
                    double yPercentMouth = (double)sketchRealMouth.Y / (double)sketchCutFace.Height;
                    double xPercentMouth = (double)sketchRealMouth.X / (double)sketchCutFace.Width;
                    sketchCutFace = sketchCutFace.Resize(standardSize.Width, standardSize.Height, Inter.Linear);
                    sketchLeftEye.X = (int)(xPercentLeft * sketchCutFace.Width);
                    sketchRightEye.X = (int)(xPercentRight * sketchCutFace.Width);
                    sketchLeftEye.Y = (int)(yPercentLeft * sketchCutFace.Height);
                    sketchRightEye.Y = (int)(yPercentRight * sketchCutFace.Height);
                    sketchRealMouth.X = (int)(xPercentMouth * sketchCutFace.Width);
                    sketchRealMouth.Y = (int)(yPercentMouth * sketchCutFace.Height);

                    cutFace = cutFace.Resize(standardSize.Width, standardSize.Height, Inter.Linear);
                    realMouth = faceDetection.getMouth(cutFace);
                }
                else
                {
                    sketchCutFace = sketchCutFace.Resize(standardSize.Width, standardSize.Height, Inter.Linear);
                    cutFace = cutFace.Resize(standardSize.Width, standardSize.Height, Inter.Linear);
                }

                if (index == 6)
                {
                    return cutFace;
                }
                if (index == 7)
                {
                    return sketchCutFace;
                }

                if (index == 8)
                {
                    Image<Gray, Byte> imageOfInterest = processForHOG(cutFace, false).Resize(HOG_WIDTH, HOG_HEIGHT, Inter.Linear);
                    return hogDescriptor.hogVisualization(imageOfInterest, hogDescriptor.GetHog(imageOfInterest));
                }

                if (index == 9)
                {
                    Image<Gray, Byte> imageOfInterest = processForHOG(sketchCutFace, true).Resize(HOG_WIDTH, HOG_HEIGHT, Inter.Linear);
                    return hogDescriptor.hogVisualization(imageOfInterest, hogDescriptor.GetHog(imageOfInterest));
                }
            }




            return null;
        }
    }
}
