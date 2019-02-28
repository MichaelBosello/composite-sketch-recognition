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
                image.Draw(face, new Bgr(Color.BlueViolet), 3);
                if (face.Y < realFace.Y)
                {
                    realFace = face;
                }
            }

            var cutFace = image.GetSubRect(realFace);
            var grayCutFace = grayImage.GetSubRect(realFace);

            image.Draw(realFace, new Bgr(Color.BurlyWood), 3);

            var eyes = haarEye.DetectMultiScale(cutFace,
                1.01, 15);
            var mouths = haarMouth.DetectMultiScale(grayCutFace,
                1.01, 15, new Size(grayCutFace.Width / 8, grayCutFace.Height / 8));

            foreach (var eye in eyes)
            {
                cutFace.Draw(eye, new Bgr(Color.DarkOrange), 3);
            }

            if (mouths.Length > 0)
            {
                var realMouth = mouths.First();
                foreach (var mouth in mouths)
                {
                    cutFace.Draw(realMouth, new Bgr(Color.MediumVioletRed), 3);
                    if (mouth.Y > realMouth.Y)
                    {
                        realMouth = mouth;
                    }
                }
                cutFace.Draw(realMouth, new Bgr(Color.DeepSkyBlue), 3);
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
                return cutFace;
            }
            return null;
        }
    }
}
