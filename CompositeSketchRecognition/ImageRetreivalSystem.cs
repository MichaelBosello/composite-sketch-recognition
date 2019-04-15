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
        public const string SKETCH_EXTENSION = "*.bmp";

        public const string OTHER_PHOTO_PATH = @"..\..\..\database\UoM-SGFS-v2\Photos\Others\";
        public const string OTHER_PHOTO_EXTENSION = "*.jpg";

        public const string DB_NAME = "descriptors.bin";

        public const int HOG_WIDTH = 144;
        public const int HOG_HEIGHT = 176;

        public Size standardSize = new Size(208, 256);

        FaceDetection faceDetection = new FaceDetection();
        HogDescriptor hogDescriptor = new HogDescriptor();

        List<FaceDescriptor> descriptors;
        SortedDictionary<double, string> result =
            new SortedDictionary<double, string>();
        String sketchName = "";

        public void test(BackgroundWorker worker, out double rank1, out double rank10, out double rank50, out double rank100, out double rank200)
        {
            rank1 = 0;
            rank10 = 0;
            rank50 = 0;
            rank100 = 0;
            rank200 = 0;
            DirectoryInfo dinfo = new DirectoryInfo(SKETCH_PATH);
            FileInfo[] sketches = dinfo.GetFiles(SKETCH_EXTENSION);
            for (int i = 0; i < sketches.Length; i++)
            {
                search(sketches[i].FullName);
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
                worker.ReportProgress(i * 100 / sketches.Length);

                if (index > 150)
                {
                    Console.WriteLine(sketches[i].Name);
                }
            }

            rank1 /= sketches.Length;
            rank10 /= sketches.Length;
            rank50 /= sketches.Length;
            rank100 /= sketches.Length;
            rank200 /= sketches.Length;
            worker.ReportProgress(100);
        }

        public void search(String sketchPath)
        {
            search(sketchPath, null, false);
        }

        public void search(String sketchPath, BackgroundWorker worker)
        {
            search(sketchPath, worker, true);
        }

        private void search(String sketchPath, BackgroundWorker worker, bool progress)
        {
            if (sketchPath.Equals("")) { return; }
            sketchName = sketchPath.Substring(sketchPath.LastIndexOf('\\') + 1, 5);
            //for out, not important here
            Rectangle[] faces; Rectangle[] eyes; Rectangle[] mouths;

            Image<Bgr, byte> sketch = new Image<Bgr, byte>(sketchPath);

            if (descriptors == null)
            {
                if (File.Exists(DB_NAME))
                {
                    Stream openFileStream = File.OpenRead(DB_NAME);
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
                        Image<Bgr, byte> image = new Image<Bgr, byte>(files[i].FullName);
                        Rectangle realFace; Rectangle[] realEyes; Rectangle realMouth;
                        faceDetection.faceAndLandmarks(image, out realFace, out realEyes, out realMouth, out faces, out eyes, out mouths);
                        var extendedFace = faceDetection.extendFace(image, realFace, faceDetection.faceOutline(image));
                        image = image.GetSubRect(extendedFace);

                        FaceDescriptor face = new FaceDescriptor(files[i].Name);

                        realMouth.X += realFace.X - extendedFace.X;
                        realMouth.Y += realFace.Y - extendedFace.Y;

                        Point leftEye = new Point(); Point rightEye = new Point();
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
                            leftEye = rotatedLeftEye;
                            rightEye = rotatedRightEye;
                            realMouth = faceDetection.getMouth(
                                image.GetSubRect(
                                new Rectangle(new Point(0, 0), new Size((int)(image.Width * 0.9), (int)(image.Height * 0.9)))
                            ));
                        }

                        //Console.WriteLine(face.Name);

                        Rectangle hair; Rectangle brow; Rectangle roiEyes; Rectangle nose;
                        faceDetection.getFaceROI(image, leftEye, rightEye, realMouth, out hair, out brow, out roiEyes, out nose, out realMouth);
                        Image<Bgr, byte> hairImage; Image<Bgr, byte> browImage; Image<Bgr, byte> eyesImage; Image<Bgr, byte> noseImage; Image<Bgr, byte> mouthImge;
                        roiToFixedImage(image, hair, brow, roiEyes, nose, realMouth,
                            out hairImage, out browImage, out eyesImage, out noseImage, out mouthImge);
                        face.Hair = hogDescriptor.GetHog(hairImage);
                        face.Brow = hogDescriptor.GetHog(browImage);
                        face.Eyes = hogDescriptor.GetHog(eyesImage);
                        face.Nose = hogDescriptor.GetHog(noseImage);
                        face.Mouth = hogDescriptor.GetHog(mouthImge);
                        face.processDescriptor();

                        descriptors.Add(face);

                        if(progress)
                            worker.ReportProgress(i * 100 / files.Count);
                    }

                    Stream SaveFileStream = File.Create(DB_NAME);
                    BinaryFormatter serializer = new BinaryFormatter();
                    serializer.Serialize(SaveFileStream, descriptors);
                    SaveFileStream.Close();
                }
            }

            if (progress)
                worker.ReportProgress(0);
            //Console.WriteLine("Db lenght" + descriptors.Count);

            Rectangle sketchRealFace; Rectangle[] sketchRealEyes; Rectangle sketchRealMouth;
            faceDetection.faceAndLandmarks(sketch, out sketchRealFace, out sketchRealEyes, out sketchRealMouth, out faces, out eyes, out mouths);
            var extendedSketchface = faceDetection.extendFace(sketch, sketchRealFace, faceDetection.faceOutline(sketch));
            sketch = sketch.GetSubRect(extendedSketchface);

            sketchRealMouth.X += sketchRealFace.X - extendedSketchface.X;
            sketchRealMouth.Y += sketchRealFace.Y - extendedSketchface.Y;

            Point sketchLeftEye = new Point();
            Point sketchRightEye = new Point();
            if (sketchRealEyes.Length == 2)
            {
                faceDetection.getEyesCenter(sketchRealEyes, out sketchLeftEye, out sketchRightEye);
                sketchLeftEye.X += sketchRealFace.X - extendedSketchface.X;
                sketchLeftEye.Y += sketchRealFace.Y - extendedSketchface.Y;
                sketchRightEye.X += sketchRealFace.X - extendedSketchface.X;
                sketchRightEye.Y += sketchRealFace.Y - extendedSketchface.Y;
            }

            FaceDescriptor sketchFace = new FaceDescriptor(sketchName);

            Rectangle sketchHair; Rectangle sketchBrow; Rectangle sketchRoiEyes; Rectangle sketchNose;
            faceDetection.getFaceROI(sketch, sketchLeftEye, sketchRightEye, sketchRealMouth, out sketchHair, out sketchBrow, out sketchRoiEyes, out sketchNose, out sketchRealMouth);
            Image<Bgr, byte> sketchHairImage; Image<Bgr, byte> sketchBrowImage; Image<Bgr, byte> sketchEyesImage; Image<Bgr, byte> sketchNoseImage; Image<Bgr, byte> sketchMouthImage;
            roiToFixedImage(sketch, sketchHair, sketchBrow, sketchRoiEyes, sketchNose, sketchRealMouth,
                out sketchHairImage, out sketchBrowImage, out sketchEyesImage, out sketchNoseImage, out sketchMouthImage);
            sketchFace.Hair = hogDescriptor.GetHog(sketchHairImage);
            sketchFace.Brow = hogDescriptor.GetHog(sketchBrowImage);
            sketchFace.Eyes = hogDescriptor.GetHog(sketchEyesImage);
            sketchFace.Nose = hogDescriptor.GetHog(sketchNoseImage);
            sketchFace.Mouth = hogDescriptor.GetHog(sketchMouthImage);
            sketchFace.processDescriptor();

            result = new SortedDictionary<double, string>();

            for (int i = 0; i < descriptors.Count; i++)
            {
                result.Add(euclideanDistance(sketchFace.Descriptor, descriptors[i].Descriptor), descriptors[i].Name);
                if (progress)
                    worker.ReportProgress(i * 100 / descriptors.Count);
            }

            if (progress)
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

        void roiToFixedImage(Image<Bgr, Byte> face, Rectangle hairRoi, Rectangle browRoi, Rectangle eyesRoi, Rectangle noseRoi, Rectangle mouthRoi,
            out Image<Bgr, byte> hair, out Image<Bgr, byte> brow, out Image<Bgr, byte> eyes, out Image<Bgr, byte> nose, out Image<Bgr, byte> mouth)
        {
            hair = face.GetSubRect(hairRoi).Resize(176, 48, Inter.Linear);
            brow = face.GetSubRect(browRoi).Resize(128, 16, Inter.Linear);
            eyes = face.GetSubRect(eyesRoi).Resize(112, 16, Inter.Linear);
            nose = face.GetSubRect(noseRoi).Resize(64, 80, Inter.Linear);
            mouth = face.GetSubRect(mouthRoi).Resize(48, 32, Inter.Linear);
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
                faceDetection.getEyesCenter(realEyes, out leftEye, out rightEye);
                leftEye.X += realFace.X - extendedFace.X;
                leftEye.Y += realFace.Y - extendedFace.Y;
                rightEye.X += realFace.X - extendedFace.X;
                rightEye.Y += realFace.Y - extendedFace.Y;
            }

            if (!inverted)
            {
                if (realEyes.Length == 2)
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
            }
            if (index == 5)
            {
                return cutFace;
            }

            Rectangle hair; Rectangle brow; Rectangle roiEyes; Rectangle nose;
            faceDetection.getFaceROI(cutFace, leftEye, rightEye, realMouth, out hair, out brow, out roiEyes, out nose, out realMouth);

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
                return hogDescriptor.hogVisualization(hairImage, hogDescriptor.GetHog(hairImage));
            }
            if (index == 8)
            {
                return hogDescriptor.hogVisualization(browImage, hogDescriptor.GetHog(browImage));
            }
            if (index == 9)
            {
                return hogDescriptor.hogVisualization(eyesImage, hogDescriptor.GetHog(eyesImage));
            }
            if (index == 10)
            {
                return hogDescriptor.hogVisualization(noseImage, hogDescriptor.GetHog(noseImage));
            }
            if (index == 11)
            {
                return hogDescriptor.hogVisualization(mouthImge, hogDescriptor.GetHog(mouthImge));
            }

            /*if (sketchPath != null)
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
            }*/




            return null;
        }
    }
}
