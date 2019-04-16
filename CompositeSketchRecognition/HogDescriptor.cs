using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System;
using System.Drawing;

namespace CompositeSketchRecognition
{
    class HogDescriptor
    {
        public float[] GetHog(Image<Gray, Byte> image)
        {
            HOGDescriptor hog = new HOGDescriptor(image.Size, new Size(16, 16), new Size(8, 8), new Size(8, 8));
            float[] result = hog.Compute(image);
            return result;
        }

        public float[] GetHog(Image<Bgr, Byte> image)
        {
            HOGDescriptor hog = new HOGDescriptor(image.Size, new Size(16, 16), new Size(8, 8), new Size(8, 8));
            float[] result = hog.Compute(image);
            return result;
        }

        public Image<Bgr, Byte> hogVisualization(Image<Gray, Byte> image, float[] descriptorValues)
        {
            return hogVisualization(image.Convert<Bgr, byte>(), descriptorValues);
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
    }
}
