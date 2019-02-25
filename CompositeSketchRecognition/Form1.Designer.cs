namespace CompositeSketchRecognition
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.buttonQuery = new System.Windows.Forms.Button();
            this.labelFoundIn = new System.Windows.Forms.Label();
            this.labelFound = new System.Windows.Forms.Label();
            this.sketch = new Emgu.CV.UI.ImageBox();
            this.imageBox1 = new Emgu.CV.UI.ImageBox();
            this.imageBox2 = new Emgu.CV.UI.ImageBox();
            this.imageBox3 = new Emgu.CV.UI.ImageBox();
            this.imageBox6 = new Emgu.CV.UI.ImageBox();
            this.imageBox5 = new Emgu.CV.UI.ImageBox();
            this.imageBox4 = new Emgu.CV.UI.ImageBox();
            this.imageBox9 = new Emgu.CV.UI.ImageBox();
            this.imageBox8 = new Emgu.CV.UI.ImageBox();
            this.imageBox7 = new Emgu.CV.UI.ImageBox();
            this.buttonNext = new System.Windows.Forms.Button();
            this.buttonPrevious = new System.Windows.Forms.Button();
            this.labelShowing = new System.Windows.Forms.Label();
            this.labelIndex = new System.Windows.Forms.Label();
            this.labelSketchID = new System.Windows.Forms.Label();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.sketch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox7)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonQuery
            // 
            this.buttonQuery.Location = new System.Drawing.Point(13, 13);
            this.buttonQuery.Name = "buttonQuery";
            this.buttonQuery.Size = new System.Drawing.Size(75, 23);
            this.buttonQuery.TabIndex = 0;
            this.buttonQuery.Text = "Query";
            this.buttonQuery.UseVisualStyleBackColor = true;
            this.buttonQuery.Click += new System.EventHandler(this.buttonQuery_Click);
            // 
            // labelFoundIn
            // 
            this.labelFoundIn.AutoSize = true;
            this.labelFoundIn.Enabled = false;
            this.labelFoundIn.Location = new System.Drawing.Point(142, 18);
            this.labelFoundIn.Name = "labelFoundIn";
            this.labelFoundIn.Size = new System.Drawing.Size(51, 13);
            this.labelFoundIn.TabIndex = 1;
            this.labelFoundIn.Text = "Found in:";
            // 
            // labelFound
            // 
            this.labelFound.AutoSize = true;
            this.labelFound.Location = new System.Drawing.Point(200, 18);
            this.labelFound.Name = "labelFound";
            this.labelFound.Size = new System.Drawing.Size(0, 13);
            this.labelFound.TabIndex = 2;
            // 
            // sketch
            // 
            this.sketch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sketch.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.PanAndZoom;
            this.sketch.Location = new System.Drawing.Point(42, 335);
            this.sketch.Name = "sketch";
            this.sketch.Size = new System.Drawing.Size(269, 317);
            this.sketch.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.sketch.TabIndex = 6;
            this.sketch.TabStop = false;
            // 
            // imageBox1
            // 
            this.imageBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageBox1.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.PanAndZoom;
            this.imageBox1.Location = new System.Drawing.Point(361, 12);
            this.imageBox1.Name = "imageBox1";
            this.imageBox1.Size = new System.Drawing.Size(269, 317);
            this.imageBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageBox1.TabIndex = 7;
            this.imageBox1.TabStop = false;
            // 
            // imageBox2
            // 
            this.imageBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageBox2.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.PanAndZoom;
            this.imageBox2.Location = new System.Drawing.Point(636, 12);
            this.imageBox2.Name = "imageBox2";
            this.imageBox2.Size = new System.Drawing.Size(269, 317);
            this.imageBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageBox2.TabIndex = 8;
            this.imageBox2.TabStop = false;
            // 
            // imageBox3
            // 
            this.imageBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageBox3.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.PanAndZoom;
            this.imageBox3.Location = new System.Drawing.Point(911, 12);
            this.imageBox3.Name = "imageBox3";
            this.imageBox3.Size = new System.Drawing.Size(269, 317);
            this.imageBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageBox3.TabIndex = 9;
            this.imageBox3.TabStop = false;
            // 
            // imageBox6
            // 
            this.imageBox6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageBox6.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.PanAndZoom;
            this.imageBox6.Location = new System.Drawing.Point(911, 335);
            this.imageBox6.Name = "imageBox6";
            this.imageBox6.Size = new System.Drawing.Size(269, 317);
            this.imageBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageBox6.TabIndex = 12;
            this.imageBox6.TabStop = false;
            // 
            // imageBox5
            // 
            this.imageBox5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageBox5.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.PanAndZoom;
            this.imageBox5.Location = new System.Drawing.Point(636, 335);
            this.imageBox5.Name = "imageBox5";
            this.imageBox5.Size = new System.Drawing.Size(269, 317);
            this.imageBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageBox5.TabIndex = 11;
            this.imageBox5.TabStop = false;
            // 
            // imageBox4
            // 
            this.imageBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageBox4.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.PanAndZoom;
            this.imageBox4.Location = new System.Drawing.Point(361, 335);
            this.imageBox4.Name = "imageBox4";
            this.imageBox4.Size = new System.Drawing.Size(269, 317);
            this.imageBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageBox4.TabIndex = 10;
            this.imageBox4.TabStop = false;
            // 
            // imageBox9
            // 
            this.imageBox9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageBox9.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.PanAndZoom;
            this.imageBox9.Location = new System.Drawing.Point(911, 658);
            this.imageBox9.Name = "imageBox9";
            this.imageBox9.Size = new System.Drawing.Size(269, 317);
            this.imageBox9.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageBox9.TabIndex = 15;
            this.imageBox9.TabStop = false;
            // 
            // imageBox8
            // 
            this.imageBox8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageBox8.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.PanAndZoom;
            this.imageBox8.Location = new System.Drawing.Point(636, 658);
            this.imageBox8.Name = "imageBox8";
            this.imageBox8.Size = new System.Drawing.Size(269, 317);
            this.imageBox8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageBox8.TabIndex = 14;
            this.imageBox8.TabStop = false;
            // 
            // imageBox7
            // 
            this.imageBox7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageBox7.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.PanAndZoom;
            this.imageBox7.Location = new System.Drawing.Point(361, 658);
            this.imageBox7.Name = "imageBox7";
            this.imageBox7.Size = new System.Drawing.Size(269, 317);
            this.imageBox7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageBox7.TabIndex = 13;
            this.imageBox7.TabStop = false;
            // 
            // buttonNext
            // 
            this.buttonNext.Enabled = false;
            this.buttonNext.Location = new System.Drawing.Point(198, 210);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(113, 23);
            this.buttonNext.TabIndex = 16;
            this.buttonNext.Text = "Next Page";
            this.buttonNext.UseVisualStyleBackColor = true;
            // 
            // buttonPrevious
            // 
            this.buttonPrevious.Enabled = false;
            this.buttonPrevious.Location = new System.Drawing.Point(42, 210);
            this.buttonPrevious.Name = "buttonPrevious";
            this.buttonPrevious.Size = new System.Drawing.Size(113, 23);
            this.buttonPrevious.TabIndex = 17;
            this.buttonPrevious.Text = "Previous Page";
            this.buttonPrevious.UseVisualStyleBackColor = true;
            // 
            // labelShowing
            // 
            this.labelShowing.AutoSize = true;
            this.labelShowing.Enabled = false;
            this.labelShowing.Location = new System.Drawing.Point(39, 161);
            this.labelShowing.Name = "labelShowing";
            this.labelShowing.Size = new System.Drawing.Size(54, 13);
            this.labelShowing.TabIndex = 18;
            this.labelShowing.Text = "Showing: ";
            // 
            // labelIndex
            // 
            this.labelIndex.AutoSize = true;
            this.labelIndex.Location = new System.Drawing.Point(100, 161);
            this.labelIndex.Name = "labelIndex";
            this.labelIndex.Size = new System.Drawing.Size(0, 13);
            this.labelIndex.TabIndex = 19;
            // 
            // labelSketchID
            // 
            this.labelSketchID.Location = new System.Drawing.Point(42, 658);
            this.labelSketchID.Name = "labelSketchID";
            this.labelSketchID.Size = new System.Drawing.Size(269, 23);
            this.labelSketchID.TabIndex = 20;
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1203, 985);
            this.Controls.Add(this.labelSketchID);
            this.Controls.Add(this.labelIndex);
            this.Controls.Add(this.labelShowing);
            this.Controls.Add(this.buttonPrevious);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.imageBox9);
            this.Controls.Add(this.imageBox8);
            this.Controls.Add(this.imageBox7);
            this.Controls.Add(this.imageBox6);
            this.Controls.Add(this.imageBox5);
            this.Controls.Add(this.imageBox4);
            this.Controls.Add(this.imageBox3);
            this.Controls.Add(this.imageBox2);
            this.Controls.Add(this.imageBox1);
            this.Controls.Add(this.sketch);
            this.Controls.Add(this.labelFound);
            this.Controls.Add(this.labelFoundIn);
            this.Controls.Add(this.buttonQuery);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.sketch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox7)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonQuery;
        private System.Windows.Forms.Label labelFoundIn;
        private System.Windows.Forms.Label labelFound;
        private Emgu.CV.UI.ImageBox sketch;
        private Emgu.CV.UI.ImageBox imageBox1;
        private Emgu.CV.UI.ImageBox imageBox2;
        private Emgu.CV.UI.ImageBox imageBox3;
        private Emgu.CV.UI.ImageBox imageBox6;
        private Emgu.CV.UI.ImageBox imageBox5;
        private Emgu.CV.UI.ImageBox imageBox4;
        private Emgu.CV.UI.ImageBox imageBox9;
        private Emgu.CV.UI.ImageBox imageBox8;
        private Emgu.CV.UI.ImageBox imageBox7;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Button buttonPrevious;
        private System.Windows.Forms.Label labelShowing;
        private System.Windows.Forms.Label labelIndex;
        private System.Windows.Forms.Label labelSketchID;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
    }
}

