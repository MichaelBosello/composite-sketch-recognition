using Emgu.CV;
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




        public Image<Bgr, byte> getStepImage(String imagePath, int index)
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(imagePath);
            Image<Bgr, byte> drawImage = image.Copy();
            Image<Bgr, byte> imageCorrectLandmark = drawImage.Copy();
            var grayImage = drawImage.Convert<Gray, Byte>();
            
            var faces = haarFrontalFace.DetectMultiScale(grayImage,
                1.05, 15, new Size(drawImage.Width/8, drawImage.Height/8));
            if (faces.Length == 0)
            {
                CvInvoke.EqualizeHist(grayImage, grayImage);
                faces = haarFrontalFace.DetectMultiScale(grayImage,
                1.01, 15);
                if (faces.Length == 0)
                {
                    return drawImage;
                }
            }

            var realFace = faces.First();
            foreach (var face in faces)
            {
                drawImage.Draw(face, new Bgr(Color.LightBlue), 3);
                if (face.Y < realFace.Y)
                {
                    realFace = face;
                }
            }
            drawImage.Draw(realFace, new Bgr(Color.DarkBlue), 3);
            imageCorrectLandmark.Draw(realFace, new Bgr(Color.DarkBlue), 3);

            var cutFace = drawImage.GetSubRect(realFace);
            var cutFaceCorrectLandmark = imageCorrectLandmark.GetSubRect(realFace);
            var grayCutFace = grayImage.GetSubRect(realFace);

            

            var eyes = haarEye.DetectMultiScale(cutFace,
                1.01, 15);
            foreach (var eye in eyes)
            {
                cutFace.Draw(eye, new Bgr(Color.Magenta), 3);
            }
            var realEyes = eyes.ToList();
            foreach (var eye in eyes)
            {
                if (eye.Y > cutFace.Height * 0.4)
                {
                    realEyes.Remove(eye);
                }
            }
            while (realEyes.Count > 2)
            {
                var top = realEyes.First();
                foreach (var eye in realEyes)
                {
                    if (eye.Y + eye.Height < top.Y + top.Height)
                    {
                        top = eye;
                    }
                }
                realEyes.Remove(top);
            }

            foreach (var eye in realEyes)
            {
                cutFace.Draw(eye, new Bgr(Color.DarkMagenta), 3);
                cutFaceCorrectLandmark.Draw(eye, new Bgr(Color.DarkMagenta), 3);
            }

            var mouths = haarMouth.DetectMultiScale(grayCutFace,
                1.01, 15, new Size(grayCutFace.Width / 8, grayCutFace.Height / 8));
            if (mouths.Length > 0)
            {
                var realMouth = mouths.First();
                foreach (var mouth in mouths)
                {
                    cutFace.Draw(realMouth, new Bgr(Color.LightGreen), 3);
                    if (mouth.Y + mouth.Height > realMouth.Y + realMouth.Height)
                    {
                        realMouth = mouth;
                    }
                }
                cutFace.Draw(realMouth, new Bgr(Color.DarkGreen), 3);
                cutFaceCorrectLandmark.Draw(realMouth, new Bgr(Color.DarkGreen), 3);
            }

    

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

            Rectangle extendedFace = new Rectangle(realFace.X, realFace.Y, realFace.Width, realFace.Height + 35);


            Boolean found = false;
            for (int paddingLeft = 10; !found && paddingLeft < realFace.X; paddingLeft++)
            {
                for (int side = realFace.Y; side < realFace.Bottom - 25; side++)
                {
                    
                    if (gradientMagnitude.Data[side, paddingLeft, 0] == 255)
                    {
                        found = true;
                        extendedFace.X = paddingLeft;
                        break;
                    }
                }
            }

            found = false;
            for (int paddingRight = image.Width -10; !found && paddingRight > realFace.Right; paddingRight--)
            {
                for (int side = realFace.Y; side < realFace.Bottom - 25; side++)
                {
                    if (gradientMagnitude.Data[side, paddingRight, 0] == 255)
                    {
                        found = true;
                        extendedFace.Width = paddingRight - extendedFace.X;
                        break;
                    }
                }
            }

            found = false;
            for (int paddingTop = 10; !found && paddingTop < realFace.Y; paddingTop++)
            {
                for (int side = realFace.X; side < realFace.Right; side++)
                {
                    if (gradientMagnitude.Data[paddingTop, side, 0] == 255)
                    {
                        found = true;
                        extendedFace.Height += extendedFace.Y - paddingTop;
                        extendedFace.Y = paddingTop;
                        break;
                    }
                }
            }

            imageCorrectLandmark.Draw(extendedFace, new Bgr(Color.GreenYellow), 3);

            /*float[,] srcPoints = { { 221, 156 }, { 4740, 156 }, { 4740, 3347 }, { 221, 3347 } };
            float[,] dstPoints = { { 371, 356 }, { 4478, 191 }, { 4595, 3092 }, { 487, 3257 } };

            var srcMat = new Matrix<float>(srcPoints);
            var dstMat = new Matrix<float>(dstPoints);
            
            var invertHomogMat = new Matrix<float>(3, 3);

            var homogMat = CvInvoke.FindHomography(srcMat, dstMat, HomographyMethod.Default, 3, null);
            CvInvoke.Invert(homogMat, invertHomogMat, DecompMethod.LU);
            CvInvoke.WarpPerspective(srcImage, dstImage, invertHomogMat, (int) Inter.Nearest);*/


            if (index == 0)
            {
                return new Image<Bgr, byte>(imagePath);
            }
            else if (index == 1)
            {
                return drawImage;
            }
            else if (index == 2)
            {
                return imageCorrectLandmark;
            }
            else if (index == 3)
            {
                return gradientMagnitude
                    .Convert<Bgr, Byte>();
            }
            else if (index == 4)
            {
                return imageCorrectLandmark;
            }
            return null;
        }
    }
}
