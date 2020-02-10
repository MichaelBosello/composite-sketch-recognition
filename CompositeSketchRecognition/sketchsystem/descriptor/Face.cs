using System;
using System.Collections.Generic;

namespace CompositeSketchRecognition
{
    [Serializable]
    public class FaceDescriptor
    {
        public String Name { get; set; }
        public float[] HairHog { get; set; }
        public float[] BrowHog { get; set; }
        public float[] EyesHog { get; set; }
        public float[] NoseHog { get; set; }
        public float[] MouthHog { get; set; }

        public float[] HairSift { get; set; }
        public float[] BrowSift { get; set; }
        public float[] EyesSift { get; set; }
        public float[] NoseSift { get; set; }
        public float[] MouthSift { get; set; }

        public float[] DescriptorHog { get; set; }
        public float[] DescriptorSift { get; set; }

        public FaceDescriptor(string name)
        {
            Name = name;
        }

        public void processDescriptors()
        {
            List<float> list = new List<float>();
            list.AddRange(HairHog);
            list.AddRange(BrowHog);
            list.AddRange(EyesHog);
            list.AddRange(NoseHog);
            list.AddRange(MouthHog);
            DescriptorHog = list.ToArray();

            HairHog = null;
            BrowHog = null;
            EyesHog = null;
            NoseHog = null;
            MouthHog = null;

            list = new List<float>();
            list.AddRange(HairSift);
            list.AddRange(BrowSift);
            list.AddRange(EyesSift);
            list.AddRange(NoseSift);
            list.AddRange(MouthSift);
            DescriptorSift = list.ToArray();

            HairSift = null;
            BrowSift = null;
            EyesSift = null;
            NoseSift = null;
            MouthSift = null;
        }
    }
}
