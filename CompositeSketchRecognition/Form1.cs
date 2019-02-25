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


            labelFoundIn.Enabled = true;
            labelShowing.Enabled = true;
            buttonPrevious.Enabled = true;
            buttonNext.Enabled = true;

            buttonQuery.Text = "Query";

        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }
    }
}
