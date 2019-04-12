using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Linq;
using System.Drawing;
using Emgu.CV.CvEnum;


namespace CompositeSketchRecognition
{
    class FaceDetection
    {

        CascadeClassifier haarEye = new CascadeClassifier(@"..\..\..\haarcascades\haarcascade_eye.xml");
        CascadeClassifier haarFrontalFace = new CascadeClassifier(@"..\..\..\haarcascades\haarcascade_frontalface_default.xml");
        CascadeClassifier haarMouth = new CascadeClassifier(@"..\..\..\haarcascades\haarcascade_mcs_mouth.xml");

        public void faceAndLandmarks(Image<Bgr, byte> image, out Rectangle realFace, out Rectangle[] realEyes, out Rectangle realMouth, out Rectangle[] faces, out Rectangle[] eyes, out Rectangle[] mouths)
        {
            realFace = new Rectangle();
            realMouth = new Rectangle();
            realEyes = null;
            eyes = null;
            mouths = null;

            var grayImage = image.Convert<Gray, Byte>();
            faces = haarFrontalFace.DetectMultiScale(grayImage,
                1.05, 15, new Size(image.Width / 8, image.Height / 8));
            if (faces.Length == 0)
            {
                CvInvoke.EqualizeHist(grayImage, grayImage);
                faces = haarFrontalFace.DetectMultiScale(grayImage,
                1.01, 15);
                if (faces.Length == 0)
                {
                    return;
                }
            }

            realFace = faces.First();
            foreach (var face in faces)
            {
                if (face.Y < realFace.Y)
                {
                    realFace = face;
                }
            }

            var cutFace = image.GetSubRect(realFace);
            var grayCutFace = grayImage.GetSubRect(realFace);

            eyes = haarEye.DetectMultiScale(cutFace,
                1.01, 15, maxSize: new Size(grayCutFace.Width / 4, grayCutFace.Height / 4));
            var realEyesList = eyes.ToList();
            foreach (var eye in eyes)
            {
                if (eye.Y > cutFace.Height * 0.4)
                {
                    realEyesList.Remove(eye);
                }
            }
            while (realEyesList.Count > 2)
            {
                var top = realEyesList.First();
                foreach (var eye in realEyesList)
                {
                    if (eye.Y + eye.Height < top.Y + top.Height)
                    {
                        top = eye;
                    }
                }
                realEyesList.Remove(top);
            }
            realEyes = realEyesList.ToArray();

            mouths = haarMouth.DetectMultiScale(grayCutFace,
                1.01, 15, new Size(grayCutFace.Width / 8, grayCutFace.Height / 8));
            if (mouths.Length > 0)
            {
                realMouth = mouths.First();
                foreach (var mouth in mouths)
                {
                    if (mouth.Y + mouth.Height > realMouth.Y + realMouth.Height)
                    {
                        realMouth = mouth;
                    }
                }

            }
        }

        public Rectangle getMouth(Image<Bgr, byte> image)
        {
            var mouth = new Rectangle();
            var grayImage = image.Convert<Gray, Byte>();

            var mouths = haarMouth.DetectMultiScale(grayImage,
                1.01, 15, new Size(grayImage.Width / 10, grayImage.Height / 12)
                );
            if (mouths.Length > 0)
            {
                mouth = mouths.First();
                foreach (var m in mouths)
                {
                    if (m.Y > mouth.Y)
                    {
                        mouth = m;
                    }
                }
            }

            return mouth;
        }

        public Image<Gray, float> faceOutline(Image<Bgr, byte> image)
        {
            var imageGray = image.Convert<Gray, byte>();
            var gx = imageGray.Sobel(1, 0, 3);
            var gy = imageGray.Sobel(0, 1, 3);

            var gradientMagnitude =
                new Image<Gray, float>(imageGray.Width, imageGray.Height);

            for (int y = 0; y < imageGray.Height; y++)
            {
                for (int x = 0; x < imageGray.Width; x++)
                {
                    var gradient = Math.Sqrt(gx[y, x].Intensity * gx[y, x].Intensity + gy[y, x].Intensity * gy[y, x].Intensity);
                    gradientMagnitude[y, x] = new Gray(gradient);
                }
            }

            gradientMagnitude = gradientMagnitude
                    .ThresholdBinary(new Gray(60), new Gray(255))
                    .Dilate(5).Erode(5)
                    .Erode(2).Dilate(2);

            return gradientMagnitude;
        }

        public Rectangle extendFace(Image<Bgr, Byte> image, Rectangle face, Image<Gray, float> faceOutline)
        {
            Rectangle extendedFace = new Rectangle(face.X, face.Y, face.Width, face.Height + 60);

            int middle = face.Y + face.Height / 2; 

            Boolean found = false;
            for (int paddingLeft = 10; !found && paddingLeft < face.X; paddingLeft++)
            {
                for (int side = face.Y; side < middle; side++)
                {

                    if (faceOutline.Data[side, paddingLeft, 0] == 255)
                    {
                        found = true;
                        extendedFace.X = paddingLeft;
                        break;
                    }
                }
            }

            extendedFace.Width = face.Right - extendedFace.X;
            found = false;
            for (int paddingRight = image.Width - 10; !found && paddingRight > face.Right; paddingRight--)
            {
                for (int side = face.Y; side < middle; side++)
                {
                    if (faceOutline.Data[side, paddingRight, 0] == 255)
                    {
                        found = true;
                        extendedFace.Width = paddingRight - extendedFace.X;
                        break;
                    }
                }
            }

            found = false;
            for (int paddingTop = 10; !found && paddingTop < face.Y; paddingTop++)
            {
                for (int side = face.X; side < face.Right; side++)
                {
                    if (faceOutline.Data[paddingTop, side, 0] == 255)
                    {
                        found = true;
                        extendedFace.Height += extendedFace.Y - paddingTop;
                        extendedFace.Y = paddingTop;
                        break;
                    }
                }
            }

            return extendedFace;
        }

        public void getEyesCenter(Rectangle[] eyes, out Point leftEye, out Point rightEye)
        {
            var lEye = eyes.First();
            var rEye = eyes.Last();
            if (lEye.X > rEye.X)
            {
                lEye = eyes.Last();
                rEye = eyes.First();
            }
            leftEye = new Point(lEye.X + lEye.Width / 2, lEye.Y + lEye.Height / 2);
            rightEye = new Point(rEye.X + rEye.Width / 2, rEye.Y + rEye.Height / 2);
        }

        public Image<Bgr, byte> alignEyes(Image<Bgr, byte> face, Point leftEye, Point rightEye, out Point rotatedLeftEye, out Point rotatedRightEye)
        {
            var deltaY = leftEye.Y - rightEye.Y;
            var deltaX = leftEye.X - rightEye.X;
            double degrees = (Math.Atan2(deltaY, deltaX) * 180) / Math.PI;
            degrees = 180 - degrees;
            leftEye.Y -= deltaY / 2;
            rightEye.Y += deltaY / 2;
            rotatedLeftEye = leftEye;
            rotatedRightEye = rightEye;

            return face.Rotate(degrees, face[0, 0]);
        }

        public Image<Bgr, byte> alignFaces(Image<Bgr, byte> cutFace, Image<Bgr, byte> sketchCutFace,
            Point face1LeftEye, Point face1RightEye,
            Point face2LeftEye, Point face2RightEye)
        {
            double distanceEyeDifference = (double)(face2RightEye.X - face2LeftEye.X) / (face1RightEye.X - face1LeftEye.X);
            int newWidth = (int)(cutFace.Width * distanceEyeDifference);
            int newHeight = (int)(cutFace.Height * distanceEyeDifference);
            cutFace = cutFace.Resize(newWidth, newHeight, Inter.Linear);
            face1LeftEye.X = (int)(face1LeftEye.X * distanceEyeDifference);
            face1LeftEye.Y = (int)(face1LeftEye.Y * distanceEyeDifference);
            face1RightEye.X = (int)(face1RightEye.X * distanceEyeDifference);
            face1RightEye.Y = (int)(face1RightEye.Y * distanceEyeDifference);

            Image<Bgr, byte> cutFaceResized = new Image<Bgr, byte>(sketchCutFace.Width, sketchCutFace.Height);

            var leftSide = face1LeftEye.X - face2LeftEye.X;
            var px = leftSide;
            var py = face1LeftEye.Y - face2LeftEye.Y;

            for (int y = 0; y < sketchCutFace.Height; y++)
            {
                for (int x = 0; x < sketchCutFace.Width; x++)
                {
                    if (px >= 0 && py >= 0 && px < cutFace.Width && py < cutFace.Height)
                    {
                        cutFaceResized.Data[y, x, 0] = cutFace.Data[py, px, 0];
                        cutFaceResized.Data[y, x, 1] = cutFace.Data[py, px, 1];
                        cutFaceResized.Data[y, x, 2] = cutFace.Data[py, px, 2];
                    }
                    else
                    {
                        cutFaceResized.Data[y, x, 0] = 255;
                        cutFaceResized.Data[y, x, 1] = 255;
                        cutFaceResized.Data[y, x, 2] = 255;
                    }
                    px++;
                }
                px = leftSide;
                py++;
            }
            return cutFaceResized;
        }

        public void getFaceROI(Image<Bgr, byte> face, Point leftEye, Point rightEye, Rectangle mouthIn, out Rectangle hair, out Rectangle brow, out Rectangle eyes, out Rectangle nose, out Rectangle mouthOut)
        {
            mouthOut = mouthIn;
            if (leftEye.Equals(new Point()))
            {
                leftEye.Y = (int)(face.Height * 0.5);
                rightEye.Y = (int)(face.Height * 0.5);
                leftEye.X = (int)(face.Width * 0.32);
                rightEye.X = face.Width - leftEye.X;
            }

            eyes = new Rectangle();

            eyes.Width = rightEye.X - leftEye.X;
            double padding = eyes.Width * 0.3;
            eyes.X = (int)(leftEye.X - padding);
            eyes.Width = (int)(eyes.Width + 2 * padding);

            double eyesHeight = eyes.Width * 0.2;
            eyes.Y = (int)(leftEye.Y - eyesHeight / 2);
            eyes.Height = (int)eyesHeight;

            brow = new Rectangle();
            double browPadding = eyes.X / 3;
            brow.X = (int)(eyes.X - browPadding);
            brow.Width = (int)(eyes.Width + (browPadding * 2));
            brow.Height = (int)(eyes.Height * 0.8);
            brow.Y = eyes.Y - brow.Height;

            hair = new Rectangle();
            hair.X = 0;
            hair.Y = 0;
            hair.Width = face.Width;
            hair.Height = brow.Y;

            
            nose = new Rectangle();
            nose.Width = (int)(mouthIn.Width * 0.8);
            nose.X = mouthIn.X + (mouthIn.Width - nose.Width) / 2;
            nose.Y = eyes.Bottom;
            nose.Height = mouthIn.Y - eyes.Bottom;




            if (eyes.Width <= 0)
            {
                eyes.Width = 1;
            }
            if (brow.Width <= 0)
            {
                brow.Width = 1;
            }
            if (nose.Width <= 0)
            {
                nose.Width = 1;
            }
            if (mouthOut.Width <= 0)
            {
                mouthOut.Width = 1;
            }

            if (eyes.Height <= 0)
            {
                eyes.Height = 1;
            }
            if (brow.Height <= 0)
            {
                brow.Height = 1;
            }
            if (nose.Height <= 0)
            {
                nose.Height = 1;
            }
            if (mouthOut.Height <= 0)
            {
                mouthOut.Height = 1;
            }

            if (eyes.X < 0)
            {
                eyes.X = 0;
            }
            if (brow.X < 0)
            {
                brow.X = 0;
            }
            if (nose.X < 0)
            {
                nose.X = 0;
            }
            if (mouthOut.X < 0)
            {
                mouthOut.X = 0;
            }

            if (eyes.Y < 0)
            {
                eyes.Y = 0;
            }
            if (brow.Y < 0)
            {
                brow.Y = 0;
            }
            if (nose.Y < 0)
            {
                nose.Y = 0;
            }
            if (mouthOut.Y < 0)
            {
                mouthOut.Y = 0;
            }


            if (eyes.Right > face.Width)
            {
                eyes.Width = face.Width - eyes.X;
            }
            if (brow.Right > face.Width)
            {
                brow.Width = face.Width - brow.X;
            }
            if (nose.Right > face.Width)
            {
                nose.Width = face.Width - nose.X;
            }
            if (mouthOut.Right > face.Width)
            {
                mouthOut.Width = face.Width - mouthOut.X;
            }

            if (eyes.Bottom > face.Height)
            {
                eyes.Height = face.Height - eyes.Y;
            }
            if (brow.Bottom > face.Height)
            {
                brow.Height = face.Height - brow.Y;
            }
            if (nose.Bottom > face.Height)
            {
                nose.Height = face.Height - nose.Y;
            }
            if (hair.Height > face.Height)
            {
                hair.Height = face.Height;
            }
            if (mouthOut.Height > face.Height)
            {
                mouthOut.Height = face.Height;
            }
        }
    }
}
