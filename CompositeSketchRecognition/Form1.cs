using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompositeSketchRecognition
{
    public partial class Form1 : Form
    {
        public const int RANK = 100;

        ImageRetreivalSystem imageRetreivalSystem = new ImageRetreivalSystem();
        int currentIndex = 0;

        public Form1()
        {
            InitializeComponent();
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
    }
}
