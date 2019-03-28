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


        public void getFaceAndLandmarks(Image<Bgr, byte> image, out Rectangle realFace, out Rectangle[] realEyes, out Rectangle realMouth, out Rectangle[] faces, out Rectangle[] eyes, out Rectangle[] mouths)
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

        public float[] GetHog(Image<Bgr, Byte> image)
        {
            HOGDescriptor hog = new HOGDescriptor(image.Size, new Size(16, 16), new Size(8, 8), new Size(8, 8));
            Point[] p = new Point[image.Width * image.Height];
            int k = 0;
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    Point p1 = new Point(i, j);
                    p[k++] = p1;
                }
            }
            float[] result = hog.Compute(image, new Size(0, 0), new Size(0, 0), p);
            return result;
        }

        //Thanks to Carl Vondrick
        //http://www.juergenbrauer.org/old_wiki/doku.php?id=public:hog_descriptor_computation_and_visualization
        public Image<Bgr, Byte> hogVisualization(Image<Bgr, Byte> image, float[] descriptorValues)
        {
            int DIMX = image.Width;
            int DIMY = image.Height;
            float zoomFac = 3;

            Image<Bgr, Byte> result = image.Copy();
            result = result.Resize((int)(result.Cols * zoomFac), (int)(result.Rows * zoomFac), Inter.Linear);

            int cellSize = 8;
            int gradientBinSize = 9;
            float radRangeForOneBin = (float)(Math.PI / (float)gradientBinSize); // dividing 180 into 9 bins, how large (in rad) is one bin?

            // prepare data structure: 9 orientation / gradient strenghts for each cell
            int cells_in_x_dir = DIMX / cellSize;
            int cells_in_y_dir = DIMY / cellSize;

            Console.WriteLine("cells in x " + cells_in_x_dir + " cells in y" + cells_in_y_dir);

            float[][][] gradientStrengths = new float[cells_in_y_dir][][];
            int[][] cellUpdateCounter = new int[cells_in_y_dir][];
            for (int y = 0; y < cells_in_y_dir; y++)
            {
                gradientStrengths[y] = new float[cells_in_x_dir][];
                cellUpdateCounter[y] = new int[cells_in_x_dir];
                for (int x = 0; x < cells_in_x_dir; x++)
                {
                    gradientStrengths[y][x] = new float[gradientBinSize];
                    cellUpdateCounter[y][x] = 0;

                    for (int bin = 0; bin < gradientBinSize; bin++)
                        gradientStrengths[y][x][bin] = 0.0f;
                }
            }

            // nr of blocks = nr of cells - 1
            // since there is a new block on each cell (overlapping blocks!) but the last one
            int blocks_in_x_dir = cells_in_x_dir - 1;
            int blocks_in_y_dir = cells_in_y_dir - 1;

            // compute gradient strengths per cell
            int descriptorDataIdx = 0;
            int cellx = 0;
            int celly = 0;

            for (int blockx = 0; blockx < blocks_in_x_dir; blockx++)
            {
                for (int blocky = 0; blocky < blocks_in_y_dir; blocky++)
                {
                    // 4 cells per block ...
                    for (int cellNr = 0; cellNr < 4; cellNr++)
                    {
                        // compute corresponding cell nr
                        cellx = blockx;
                        celly = blocky;
                        if (cellNr == 1) celly++;
                        if (cellNr == 2) cellx++;
                        if (cellNr == 3)
                        {
                            cellx++;
                            celly++;
                        }

                        for (int bin = 0; bin < gradientBinSize; bin++)
                        {
                            float gradientStrength = descriptorValues[descriptorDataIdx];
                            descriptorDataIdx++;

                            gradientStrengths[celly][cellx][bin] += gradientStrength;

                        } // for (all bins)


                        // note: overlapping blocks lead to multiple updates of this sum!
                        // we therefore keep track how often a cell was updated,
                        // to compute average gradient strengths
                        cellUpdateCounter[celly][cellx]++;

                    } // for (all cells)


                } // for (all block x pos)
            } // for (all block y pos)


            // compute average gradient strengths
            for (celly = 0; celly < cells_in_y_dir; celly++)
            {
                for (cellx = 0; cellx < cells_in_x_dir; cellx++)
                {

                    float NrUpdatesForThisCell = (float)cellUpdateCounter[celly][cellx];

                    // compute average gradient strenghts for each gradient bin direction
                    for (int bin = 0; bin < gradientBinSize; bin++)
                    {
                        gradientStrengths[celly][cellx][bin] /= NrUpdatesForThisCell;
                    }
                }
            }

            // draw cells
            for (celly = 0; celly < cells_in_y_dir; celly++)
            {
                for (cellx = 0; cellx < cells_in_x_dir; cellx++)
                {
                    int drawX = cellx * cellSize;
                    int drawY = celly * cellSize;

                    int mx = drawX + cellSize / 2;
                    int my = drawY + cellSize / 2;

                    result.Draw(
                        new Rectangle(
                            new Point((int)(drawX * zoomFac), (int)(drawY * zoomFac)),
                            new Size((int)(cellSize * zoomFac), (int)(cellSize * zoomFac))),
                        new Bgr(Color.Gray), 3);

                    // draw in each cell all 9 gradient strengths
                    for (int bin = 0; bin < gradientBinSize; bin++)
                    {
                        float currentGradStrength = gradientStrengths[celly][cellx][bin];

                        // no line to draw?
                        if (currentGradStrength == 0)
                            continue;

                        float currRad = bin * radRangeForOneBin + radRangeForOneBin / 2;

                        float dirVecX = (float)Math.Cos(currRad);
                        float dirVecY = (float)Math.Sin(currRad);
                        float maxVecLen = (float)(cellSize / 2.0f);
                        float scale = 2.5f; // just a visualization scale, to see the lines better

                        // compute line coordinates
                        float x1 = mx - dirVecX * currentGradStrength * maxVecLen * scale;
                        float y1 = my - dirVecY * currentGradStrength * maxVecLen * scale;
                        float x2 = mx + dirVecX * currentGradStrength * maxVecLen * scale;
                        float y2 = my + dirVecY * currentGradStrength * maxVecLen * scale;

                        CvInvoke.Line(result, 
                            new Point((int)(x1 * zoomFac), (int)(y1 * zoomFac)), 
                            new Point((int)(x2 * zoomFac), (int)(y2 * zoomFac)),
                            new MCvScalar(0, 255, 0), 1);

                    } // for (all bins)

                } // for (cellx)
            } // for (celly)

            return result;
        }

        public Image<Bgr, byte> getStepImage(String imagePath, int index, String sketchPath)
        {
            if (imagePath == null || imagePath.Equals(""))
            {
                return null;
            }

            Image<Bgr, byte> image = new Image<Bgr, byte>(imagePath);

            if (index == 0)
            {
                return new Image<Bgr, byte>(imagePath);
            }

            Rectangle realFace;
            Rectangle[] realEyes;
            Rectangle realMouth;
            Rectangle[] faces;
            Rectangle[] eyes;
            Rectangle[] mouths;
            getFaceAndLandmarks(image, out realFace, out realEyes, out realMouth, out faces, out eyes, out mouths);

            if (index == 1 || index == 2 || index == 4)
            {
                Image<Bgr, byte> drawImage = image.Copy();
                var cutDrawFace = drawImage.GetSubRect(realFace);

                if (index == 1)
                {
                    foreach (var face in faces)
                    {
                        drawImage.Draw(face, new Bgr(Color.LightBlue), 3);
                    }
                    foreach (var eye in eyes)
                    {
                        cutDrawFace.Draw(eye, new Bgr(Color.Magenta), 3);
                    }
                    foreach (var mouth in mouths)
                    {
                        cutDrawFace.Draw(mouth, new Bgr(Color.LightGreen), 3);
                    }
                }
                
                foreach (var eye in realEyes)
                {
                    cutDrawFace.Draw(eye, new Bgr(Color.DarkMagenta), 3);
                }
                cutDrawFace.Draw(realMouth, new Bgr(Color.DarkGreen), 3);

                if(index == 4)
                {
                    drawImage.Draw(extendFace(image, realFace, faceOutline(image)), new Bgr(Color.GreenYellow), 3);
                } else
                {
                    drawImage.Draw(realFace, new Bgr(Color.DarkBlue), 3);
                }

                return drawImage;
            }
            if (index == 3)
            {
                return faceOutline(image).Convert<Bgr, byte>();
            }

            var extendedFace = extendFace(image, realFace, faceOutline(image));
            var cutFace = image.GetSubRect(extendedFace);

            if (sketchPath != null)
            {
                Rectangle sketchRealFace;
                Rectangle[] sketchRealEyes;
                Rectangle sketchRealMouth;
                Image<Bgr, byte> sketch = new Image<Bgr, byte>(sketchPath);
                getFaceAndLandmarks(sketch, out sketchRealFace, out sketchRealEyes, out sketchRealMouth, out faces, out eyes, out mouths);
                var extendedSketchface = extendFace(sketch, sketchRealFace, faceOutline(sketch));
                var sketchCutFace = sketch.GetSubRect(extendedSketchface);

                if (realEyes.Length == 2 && sketchRealEyes.Length == 2)
                {
                    Point sketchLeftEye;
                    Point sketchRightEye;
                    getEyesCenter(sketchRealEyes, out sketchLeftEye, out sketchRightEye);
                    sketchLeftEye.X += sketchRealFace.X - extendedSketchface.X;
                    sketchLeftEye.Y += sketchRealFace.Y - extendedSketchface.Y;
                    sketchRightEye.X += sketchRealFace.X - extendedSketchface.X;
                    sketchRightEye.Y += sketchRealFace.Y - extendedSketchface.Y;

                    Point leftEye;
                    Point rightEye;
                    getEyesCenter(realEyes, out leftEye, out rightEye);
                    leftEye.X += realFace.X - extendedFace.X;
                    leftEye.Y += realFace.Y - extendedFace.Y;
                    rightEye.X += realFace.X - extendedFace.X;
                    rightEye.Y += realFace.Y - extendedFace.Y;
                    Point rotatedLeftEye;
                    Point rotatedRightEye;
                    cutFace = alignEyes(cutFace, leftEye, rightEye, out rotatedLeftEye, out rotatedRightEye);

                    if (index == 5)
                    {
                        CvInvoke.Circle(cutFace, rotatedLeftEye, 2, new MCvScalar(255, 255, 255));
                        CvInvoke.Circle(cutFace, rotatedRightEye, 2, new MCvScalar(255, 255, 255));
                        return cutFace;
                    }
                    
                    double distanceEyeDifference = (double)(sketchRightEye.X - sketchLeftEye.X) / (rightEye.X - leftEye.X);
                    int newWidth = (int) (cutFace.Width * distanceEyeDifference);
                    int newHeight = (int)(cutFace.Height * distanceEyeDifference);
                    cutFace = cutFace.Resize(newWidth, newHeight, Inter.Linear);
                    rotatedLeftEye.X = (int) (rotatedLeftEye.X * distanceEyeDifference);
                    rotatedLeftEye.Y = (int)(rotatedLeftEye.Y * distanceEyeDifference);
                    rotatedRightEye.X = (int)(rotatedRightEye.X * distanceEyeDifference);
                    rotatedRightEye.Y = (int)(rotatedRightEye.Y * distanceEyeDifference);

                    Image<Bgr, byte> cutFaceResized = new Image<Bgr, byte>(sketchCutFace.Width, sketchCutFace.Height);

                    var leftSide = rotatedLeftEye.X - sketchLeftEye.X;
                    var px = leftSide;
                    var py = rotatedLeftEye.Y - sketchLeftEye.Y;

                    for (int y = 0; y < sketchCutFace.Height; y++)
                    {
                        for (int x = 0; x < sketchCutFace.Width; x++)
                        {
                            if (px >= 0 && py >= 0 && px < cutFace.Width && py < cutFace.Height)
                            {
                                cutFaceResized.Data[y, x, 0] = cutFace.Data[py, px, 0];
                                cutFaceResized.Data[y, x, 1] = cutFace.Data[py, px, 1];
                                cutFaceResized.Data[y, x, 2] = cutFace.Data[py, px, 2];
                            }
                            else
                            {
                                cutFaceResized.Data[y, x, 0] = 255;
                                cutFaceResized.Data[y, x, 1] = 255;
                                cutFaceResized.Data[y, x, 2] = 255;
                            }
                            px++;
                        }
                        px = leftSide;
                        py++;
                    }

                    if (index == 6)
                    {
                        Image<Bgr, byte> step6 = new Image<Bgr, byte>(sketchCutFace.Width, sketchCutFace.Height);
                        CvInvoke.AddWeighted(cutFaceResized, 0.6, sketchCutFace, 0.3, 0, step6);
                        return step6;
                    }

                    if (index == 7)
                    {
                        Image<Bgr, Byte> imageOfInterest = cutFaceResized.Resize(144, 176, Inter.Linear);
                        return hogVisualization(imageOfInterest, GetHog(imageOfInterest));
                    }

                    if (index == 8)
                    {
                        Image<Bgr, Byte> imageOfInterest = sketchCutFace.Resize(144, 176, Inter.Linear);
                        return hogVisualization(imageOfInterest, GetHog(imageOfInterest));
                    }



                }
                else
                {

                }
            }


            

            return null;
        }
    }
}
