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

        public float[] ComputeDescriptor(Image<Bgr, byte> image, int stepX = 9, int stepY = 9)
        {
            SIFT sift = new SIFT();

            VectorOfKeyPoint keypoints = new VectorOfKeyPoint();
            Mat descriptors = new Mat();

            for (int y = stepY; y < image.Rows - stepY; y += stepY)
            {
                for (int x = stepX; x < image.Cols - stepX; x += stepX)
                {
                    MKeyPoint[] point = { new MKeyPoint() };
                    point[0].Size = stepX;
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
