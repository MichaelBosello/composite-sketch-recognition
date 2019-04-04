using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompositeSketchRecognition
{
    [Serializable]
    class Face
    {
        public Image<Bgr, byte> image { get; set; }
        public String name { get; set; }
        public Point leftEye { get; set; }
        public Point rightEye { get; set; }
        public Rectangle mouth { get; set; }

        public Face(Image<Bgr, byte> image, string name, Point leftEye, Point rightEye, Rectangle mouth)
        {
            this.image = image;
            this.name = name;
            this.leftEye = leftEye;
            this.rightEye = rightEye;
            this.mouth = mouth;
        }

        public Face(Image<Bgr, byte> image, string name)
        {
            this.image = image;
            this.name = name;
        }
    }
}
