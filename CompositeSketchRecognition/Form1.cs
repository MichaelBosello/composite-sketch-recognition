using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompositeSketchRecognition
{
    public partial class Form1 : Form
    {
        public const string PHOTO_PATH = @"..\..\..\database\UoM-SGFS-v2\Photos\Images\";
        public const string SKETCH_PATH = @"..\..\..\database\UoM-SGFS-v2\Sketches\Set A\Images\";
        public const string PHOTO_EXTENSION = "*.ppm";
        public const string SKETCH_EXTENSION = "*.bmp";
        public const int RANK = 100;

        ImageRetreivalSystem imageRetreivalSystem = new ImageRetreivalSystem();
        int currentIndex = 0;
        int currentStep = 0;
        String currentStepImage = "";
        String otherImage = "";

        public Form1()
        {
            InitializeComponent();
            PopulateListBox(listBox1, PHOTO_PATH, PHOTO_EXTENSION);
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

        private void buttonQuery_Click(object sender, EventArgs e)
        {
            sketch.Image = null;
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


            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    labelSketchID.Text = dlg.FileName;
                    sketch.Image = new Image<Bgr, byte>(labelSketchID.Text);
                }
            }

            if (!backgroundWorker.IsBusy)
            {
                backgroundWorker.RunWorkerAsync();
            }
            else
            {
                backgroundWorker.CancelAsync();
            }
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            imageRetreivalSystem.search((Image<Bgr, byte>)sketch.Image);
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
            labelIndex.Text = currentIndex.ToString();
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

        

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentStepImage = PHOTO_PATH + listBox1.SelectedItem;

            if (listBox2.SelectedItem != null)
            {
                otherImage = SKETCH_PATH + listBox2.SelectedItem;
            }
            else
            {
                otherImage = null;
            }
            imageBoxStep.Image = imageRetreivalSystem.getStepImage(currentStepImage, currentStep, otherImage);
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
            imageBoxStep.Image = imageRetreivalSystem.getStepImage(currentStepImage, currentStep, otherImage);
        }

        private void buttonStepPre_Click(object sender, EventArgs e)
        {
            if (currentStep > 0)
            {
                currentStep--;
                labelStep.Text = currentStep.ToString();
                imageBoxStep.Image = imageRetreivalSystem.getStepImage(currentStepImage, currentStep, otherImage);
            }
        }

        private void buttonStepNext_Click(object sender, EventArgs e)
        {
            if (currentStep < 9)
            {
                currentStep++;
                labelStep.Text = currentStep.ToString();
                imageBoxStep.Image = imageRetreivalSystem.getStepImage(currentStepImage, currentStep, otherImage);
            }
        }
    }
}
