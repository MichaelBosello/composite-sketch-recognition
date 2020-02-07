using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.XFeatures2D;
using System.Drawing;

namespace CompositeSketchRecognition
{
    public class SiftDescriptor
    {
        public SiftDescriptor(){}

        public float[] ComputeDescriptor(Image<Bgr, byte> image)
        {
            SIFT sift = new SIFT();

            VectorOfKeyPoint keypoints = new VectorOfKeyPoint();
            Mat descriptors = new Mat();

            int step = 9;

            for (int y = step; y < image.Rows - step; y += step)
            {
                for (int x = step; x < image.Cols - step; x += step)
                {
                    MKeyPoint[] point = { new MKeyPoint() };
                    point[0].Size = step;
                    point[0].Point = new Point(x, y);
                    keypoints.Push(point);
                }
            }

            sift.Compute(image, keypoints, descriptors);

            float[] returnArray = new float[descriptors.Rows * descriptors.Cols];
            descriptors.CopyTo(returnArray);
            return returnArray;
        }
    }
}
