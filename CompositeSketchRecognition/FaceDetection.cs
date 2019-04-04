using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Linq;
using System.Drawing;

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
    }
}
