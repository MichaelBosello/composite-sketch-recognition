﻿using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CompositeSketchRecognition
{
    class ImageRetreivalSystem
    {
        CascadeClassifier haarEye = new CascadeClassifier(@"..\..\..\haarcascades\haarcascade_eye.xml");
        CascadeClassifier haarFrontalFace = new CascadeClassifier(@"..\..\..\haarcascades\haarcascade_frontalface_default.xml");
        CascadeClassifier haarMouth = new CascadeClassifier(@"..\..\..\haarcascades\haarcascade_mcs_mouth.xml");

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


        public void getFaceAndLandmarks(Image<Bgr, byte> image, out Rectangle realFace, out Rectangle[] realEyes, out Rectangle realMouth, out Rectangle[] faces, out Rectangle[] eyes, out Rectangle[] mouths)
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
                1.01, 15);
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

            Boolean found = false;
            for (int paddingLeft = 10; !found && paddingLeft < face.X; paddingLeft++)
            {
                for (int side = face.Y; side < face.Bottom - 25; side++)
                {

                    if (faceOutline.Data[side, paddingLeft, 0] == 255)
                    {
                        found = true;
                        extendedFace.X = paddingLeft;
                        break;
                    }
                }
            }

            found = false;
            for (int paddingRight = image.Width - 10; !found && paddingRight > face.Right; paddingRight--)
            {
                for (int side = face.Y; side < face.Bottom - 25; side++)
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
            return face.Rotate(degrees, new Bgr(255, 255, 255));
        }

        public Image<Bgr, byte> getStepImage(String imagePath, int index, String sketchPath)
        {
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
            getFaceAndLandmarks(image, out realFace, out realEyes, out realMouth, out faces, out eyes, out mouths);

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

                if(index == 4)
                {
                    drawImage.Draw(extendFace(image, realFace, faceOutline(image)), new Bgr(Color.GreenYellow), 3);
                } else
                {
                    drawImage.Draw(realFace, new Bgr(Color.DarkBlue), 3);
                }

                return drawImage;
            }
            if (index == 3)
            {
                return faceOutline(image).Convert<Bgr, byte>();
            }



            if (sketchPath != null)
            {
                var extendedFace = extendFace(image, realFace, faceOutline(image));
                var cutFace = image.GetSubRect(extendedFace);

                Rectangle sketchRealFace;
                Rectangle[] sketchRealEyes;
                Rectangle sketchRealMouth;
                Image<Bgr, byte> sketch = new Image<Bgr, byte>(sketchPath);
                getFaceAndLandmarks(sketch, out sketchRealFace, out sketchRealEyes, out sketchRealMouth, out faces, out eyes, out mouths);
                var extendedSketchface = extendFace(sketch, sketchRealFace, faceOutline(sketch));
                var sketchCutFace = sketch.GetSubRect(extendedSketchface);

                if (realEyes.Length == 2 && sketchRealEyes.Length == 2)
                {
                    Point sketchLeftEye;
                    Point sketchRightEye;
                    getEyesCenter(sketchRealEyes, out sketchLeftEye, out sketchRightEye);
                    sketchLeftEye.X += sketchRealFace.X - extendedSketchface.X;
                    sketchLeftEye.Y += sketchRealFace.Y - extendedSketchface.Y;
                    sketchRightEye.X += sketchRealFace.X - extendedSketchface.X;
                    sketchRightEye.Y += sketchRealFace.Y - extendedSketchface.Y;

                    Point leftEye;
                    Point rightEye;
                    getEyesCenter(realEyes, out leftEye, out rightEye);
                    leftEye.X += realFace.X - extendedFace.X;
                    leftEye.Y += realFace.Y - extendedFace.Y;
                    rightEye.X += realFace.X - extendedFace.X;
                    rightEye.Y += realFace.Y - extendedFace.Y;
                    Point rotatedLeftEye;
                    Point rotatedRightEye;
                    cutFace = alignEyes(cutFace, leftEye, rightEye, out rotatedLeftEye, out rotatedRightEye);

                    if (index == 5)
                    {
                        CvInvoke.Circle(cutFace, rotatedLeftEye, 2, new MCvScalar(255, 255, 255));
                        CvInvoke.Circle(cutFace, rotatedRightEye, 2, new MCvScalar(255, 255, 255));
                        return cutFace;
                    }
                    
                    double distanceEyeDifference = (double)(sketchRightEye.X - sketchLeftEye.X) / (rightEye.X - leftEye.X);
                    int newWidth = (int) (cutFace.Width * distanceEyeDifference);
                    int newHeight = (int)(cutFace.Height * distanceEyeDifference);
                    cutFace = cutFace.Resize(newWidth, newHeight, Inter.Linear);
                    rotatedLeftEye.X = (int) (rotatedLeftEye.X * distanceEyeDifference);
                    rotatedLeftEye.Y = (int)(rotatedLeftEye.Y * distanceEyeDifference);
                    rotatedRightEye.X = (int)(rotatedRightEye.X * distanceEyeDifference);
                    rotatedRightEye.Y = (int)(rotatedRightEye.Y * distanceEyeDifference);

                    Image<Bgr, byte> cutFaceResized = new Image<Bgr, byte>(sketchCutFace.Width, sketchCutFace.Height);

                    var leftSide = rotatedLeftEye.X - sketchLeftEye.X;
                    var px = leftSide;
                    var py = rotatedLeftEye.Y - sketchLeftEye.Y;

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

                    if (index == 6)
                    {
                        Image<Bgr, byte> step6 = new Image<Bgr, byte>(sketchCutFace.Width, sketchCutFace.Height);
                        CvInvoke.AddWeighted(cutFaceResized, 0.6, sketchCutFace, 0.3, 0, step6);
                        CvInvoke.Circle(step6, rotatedLeftEye, 2, new MCvScalar(255, 255, 255));
                        CvInvoke.Circle(step6, sketchLeftEye, 3, new MCvScalar(0, 0, 0));
                        return step6;
                    }
                }
                else
                {

                }
            }


            

            return null;
        }
    }
}