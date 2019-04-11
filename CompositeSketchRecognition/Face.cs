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
    class FaceDescriptor
    {
        public String Name { get; set; }
        public float[] Hair { get; set; }
        public float[] Brow { get; set; }
        public float[] Eyes { get; set; }
        public float[] Nose { get; set; }
        public float[] Mouth { get; set; }

        public float[] Descriptor { get; set; }

        public FaceDescriptor(string name, float[] hair, float[] brow, float[] eyes,float[] nose, float[] mouth)
        {
            this.Name = name;
            this.Hair = hair;
            this.Brow = brow;
            this.Eyes = eyes;
            this.Nose = nose;
            this.Mouth = mouth;
        }

        public FaceDescriptor(string name)
        {
            this.Name = name;
        }

        public void processDescriptor()
        {
            List<float> list = new List<float>();
            list.AddRange(Hair);
            list.AddRange(Brow);
            list.AddRange(Eyes);
            list.AddRange(Nose);
            list.AddRange(Mouth);
            Descriptor = list.ToArray();

            Hair = null;
            Brow = null;
            Eyes = null;
            Nose = null;
            Mouth = null;
        }
    }
}
