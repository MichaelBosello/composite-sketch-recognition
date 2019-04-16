using System;
using System.Collections.Generic;

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
            Name = name;
            Hair = hair;
            Brow = brow;
            Eyes = eyes;
            Nose = nose;
            Mouth = mouth;
        }

        public FaceDescriptor(string name)
        {
            Name = name;
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
