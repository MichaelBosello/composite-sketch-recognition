using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CompositeSketchRecognition
{
    public partial class Form1 : Form
    {
        public const string PHOTO_PATH = @"..\..\..\database\UoM-SGFS-v2\Photos\Images\";
        public const string SKETCH_PATH = @"..\..\..\database\UoM-SGFS-v2\Sketches\Set A\Images\";
        public const string PHOTO_EXTENSION = "*.ppm";
        public const string SKETCH_EXTENSION = "*.bmp";
        public const string OTHER_PHOTO_PATH = @"..\..\..\database\UoM-SGFS-v2\Photos\Others\";
        public const string OTHER_PHOTO_EXTENSION = "*.jpg";
        public const int RANK = 100;

        ImageRetreivalSystem imageRetreivalSystem = new ImageRetreivalSystem();
        int currentIndex = 0;
        int currentStep = 0;
        String currentStepImage = "";
        String otherImage = "";
        bool inverted = false;

        public Form1()
        {
            InitializeComponent();
            PopulateListBox(listBox1, PHOTO_PATH, PHOTO_EXTENSION);
            PopulateListBox(listBox1, OTHER_PHOTO_PATH, PHOTO_EXTENSION);
            PopulateListBox(listBox1, OTHER_PHOTO_PATH, OTHER_PHOTO_EXTENSION);
            PopulateListBox(listBox2, SKETCH_PATH, SKETCH_EXTENSION);
        }

        private void PopulateListBox(ListBox lsb, string Folder, string FileType)
        {
            DirectoryInfo dinfo = new DirectoryInfo(Folder);
            FileInfo[] Files = dinfo.GetFiles(FileType);
            foreach (FileInfo file in Files)
            {
                lsb.Items.Add(file.Name);
            }
            lsb.Sorted = true;
        }

        // tab 1

        private void buttonQuery_Click(object sender, EventArgs e)
        {
            sketchImageBox.Image = null;
            imageBox1.Image = null;
            imageBox2.Image = null;
            imageBox3.Image = null;
            imageBox4.Image = null;
            imageBox5.Image = null;
            imageBox6.Image = null;
            imageBox7.Image = null;
            imageBox8.Image = null;
            imageBox9.Image = null;

            labelFoundIn.Enabled = false;
            labelFound.Text = "";
            labelShowing.Enabled = false;
            labelIndex.Text = "";

            buttonPrevious.Enabled = false;
            buttonNext.Enabled = false;

            buttonQuery.Text = "Stop";

            currentIndex = 0;

            if (!backgroundWorker.IsBusy)
            {
                using (OpenFileDialog dlg = new OpenFileDialog())
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        labelSketchID.Text = dlg.FileName;
                        sketchImageBox.Image = new Image<Bgr, byte>(labelSketchID.Text);
                    }
                }
                backgroundWorker.RunWorkerAsync();
            }
            else
            {
                backgroundWorker.CancelAsync();
                buttonQuery.Text = "Query";
            }
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var backgroundWorker = sender as BackgroundWorker;
            imageRetreivalSystem.search(labelSketchID.Text, backgroundWorker);
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            labelFoundIn.Enabled = true;
            labelShowing.Enabled = true;
            buttonPrevious.Enabled = true;
            buttonNext.Enabled = true;

            buttonQuery.Text = "Query";

            int foundIndex = imageRetreivalSystem.getSubjectIndex();

            labelFound.Text = foundIndex.ToString();
            if (foundIndex < 0)
            {
                labelFound.ForeColor = Color.DarkOrange;
                labelFound.Text = "NOT FOUND";
            }
            else if (foundIndex < RANK)
            {
                labelFound.ForeColor = Color.DarkGreen;
            }
            else
            {
                labelFound.ForeColor = Color.DarkRed;
            }
            showImages();
        }

        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            currentIndex -= 9;
            if (currentIndex < 0)
            {
                currentIndex = 0;
            }
            showImages();
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            currentIndex += 9;
            showImages();
        }

        private void showImages()
        {
            labelIndex.Text = (currentIndex + 1).ToString();
            imageBox1.Image = imageRetreivalSystem.getImage(currentIndex);
            imageBox2.Image = imageRetreivalSystem.getImage(currentIndex + 1);
            imageBox3.Image = imageRetreivalSystem.getImage(currentIndex + 2);
            imageBox4.Image = imageRetreivalSystem.getImage(currentIndex + 3);
            imageBox5.Image = imageRetreivalSystem.getImage(currentIndex + 4);
            imageBox6.Image = imageRetreivalSystem.getImage(currentIndex + 5);
            imageBox7.Image = imageRetreivalSystem.getImage(currentIndex + 6);
            imageBox8.Image = imageRetreivalSystem.getImage(currentIndex + 7);
            imageBox9.Image = imageRetreivalSystem.getImage(currentIndex + 8);
        }

        
        // tab 2

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentStepImage = PHOTO_PATH + listBox1.SelectedItem;
            if (!File.Exists(currentStepImage))
            {
                currentStepImage = OTHER_PHOTO_PATH + listBox1.SelectedItem;
                if (!File.Exists(currentStepImage))
                {
                    currentStepImage = null;
                }
            }

            if (listBox2.SelectedItem != null)
            {
                otherImage = SKETCH_PATH + listBox2.SelectedItem;
            }
            else
            {
                otherImage = null;
            }
            inverted = false;
            imageBoxStep.Image = imageRetreivalSystem.getStepImage(currentStepImage, currentStep, otherImage, inverted);
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentStepImage = SKETCH_PATH + listBox2.SelectedItem;
            if (listBox1.SelectedItem != null)
            {
                otherImage = PHOTO_PATH + listBox1.SelectedItem;
            }
            else
            {
                otherImage = null;
            }
            inverted = true;
            imageBoxStep.Image = imageRetreivalSystem.getStepImage(currentStepImage, currentStep, otherImage, inverted);
        }

        private void buttonStepPre_Click(object sender, EventArgs e)
        {
            if (currentStep > 0)
            {
                currentStep--;
                labelStep.Text = currentStep.ToString();
                imageBoxStep.Image = imageRetreivalSystem.getStepImage(currentStepImage, currentStep, otherImage, inverted);
            }
        }

        private void buttonStepNext_Click(object sender, EventArgs e)
        {
            if (currentStep < 11)
            {
                currentStep++;
                labelStep.Text = currentStep.ToString();
                imageBoxStep.Image = imageRetreivalSystem.getStepImage(currentStepImage, currentStep, otherImage, inverted);
            }
        }



        // tab 3

        private void buttonAccuracyTest_Click(object sender, EventArgs e)
        {
            if (!backgroundWorkerAccuracy.IsBusy)
            {
                backgroundWorkerAccuracy.RunWorkerAsync();
                buttonQuery.Text = "Stop";
            }
            else
            {
                backgroundWorkerAccuracy.CancelAsync();
                buttonQuery.Text = "Query";
            }
        }

        double rank1 = 0, rank10 = 0, rank50 = 0, rank100 = 0, rank200 = 0;
        private void backgroundWorkerAccuracy_DoWork(object sender, DoWorkEventArgs e)
        {
            var backgroundWorkerAccuracy = sender as BackgroundWorker;
            imageRetreivalSystem.accuracyTest(backgroundWorkerAccuracy, out rank1, out rank10, out rank50, out rank100, out rank200);
        }

        private void backgroundWorkerAccuracy_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBarAccuracy.Value = e.ProgressPercentage;
        }

        private void backgroundWorkerAccuracy_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            rank1label.Text = rank1.ToString();
            rank10label.Text = rank10.ToString();
            rank50label.Text = rank50.ToString();
            rank100label.Text = rank100.ToString();
            rank200label.Text = rank200.ToString();
        }
    }
}
