using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System;
using System.Drawing;

namespace CompositeSketchRecognition
{
    class ImageRetreivalSystem
    {

        FaceDetection faceDetection = new FaceDetection();
        HogDescriptor hogDescriptor = new HogDescriptor();

        public void search(Image<Bgr, byte> sketch)
        {

        }

        public Image<Bgr, byte> getImage(int index)
        {
            return null;
        }

        public int getSubjectIndex()
        {
            return -1;
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
                        cutFace.Draw(realMouth, new Bgr(Color.DarkGreen), 3);
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
                }

                if (index == 6)
                {
                    Image<Bgr, byte> step6 = new Image<Bgr, byte>(sketchCutFace.Width, sketchCutFace.Height);
                    CvInvoke.AddWeighted(cutFace, 0.6, sketchCutFace, 0.3, 0, step6);
                    return step6;
                }

                if (index == 7)
                {
                    Image<Bgr, Byte> imageOfInterest = cutFace.Resize(144, 176, Inter.Linear);
                    return hogDescriptor.hogVisualization(imageOfInterest, hogDescriptor.GetHog(imageOfInterest));
                }

                if (index == 8)
                {
                    Image<Bgr, Byte> imageOfInterest = sketchCutFace.Resize(144, 176, Inter.Linear);
                    return hogDescriptor.hogVisualization(imageOfInterest, hogDescriptor.GetHog(imageOfInterest));
                }
            }




            return null;
        }
    }
}
