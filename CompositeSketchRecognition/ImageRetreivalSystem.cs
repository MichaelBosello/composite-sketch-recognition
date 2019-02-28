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
            Image<Bgr, byte> imageCorrectLandmark = image.Copy();
            var grayImage = image.Convert<Gray, Byte>();

            var faces = haarFrontalFace.DetectMultiScale(grayImage,
                1.05, 15, new Size(image.Width/8, image.Height/8));
            if (faces.Length == 0)
            {
                CvInvoke.EqualizeHist(grayImage, grayImage);
                faces = haarFrontalFace.DetectMultiScale(grayImage,
                1.01, 15);
                if (faces.Length == 0)
                {
                    return image;
                }
            }

            var realFace = faces.First();
            foreach (var face in faces)
            {
                image.Draw(face, new Bgr(Color.LightBlue), 3);
                if (face.Y < realFace.Y)
                {
                    realFace = face;
                }
            }
            image.Draw(realFace, new Bgr(Color.DarkBlue), 3);
            imageCorrectLandmark.Draw(realFace, new Bgr(Color.DarkBlue), 3);

            var cutFace = image.GetSubRect(realFace);
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



            if (index == 0)
            {
                return new Image<Bgr, byte>(imagePath);
            }
            else if (index == 1)
            {
                return image;
            }
            else if (index == 2)
            {
                return imageCorrectLandmark;
            }
            return null;
        }
    }
}
