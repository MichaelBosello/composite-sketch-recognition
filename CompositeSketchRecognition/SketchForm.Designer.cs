namespace CompositeSketchRecognition
{
    partial class SketchForm
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
            this.sketchImageBox = new Emgu.CV.UI.ImageBox();
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.buttonStepNext = new System.Windows.Forms.Button();
            this.buttonStepPre = new System.Windows.Forms.Button();
            this.labelStep = new System.Windows.Forms.Label();
            this.imageBoxStep = new Emgu.CV.UI.ImageBox();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.rank200label = new System.Windows.Forms.Label();
            this.rank100label = new System.Windows.Forms.Label();
            this.rank50label = new System.Windows.Forms.Label();
            this.rank10label = new System.Windows.Forms.Label();
            this.rank1label = new System.Windows.Forms.Label();
            this.buttonAccuracyTest = new System.Windows.Forms.Button();
            this.progressBarAccuracy = new System.Windows.Forms.ProgressBar();
            this.backgroundWorkerAccuracy = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.sketchImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox7)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBoxStep)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonQuery
            // 
            this.buttonQuery.Location = new System.Drawing.Point(18, 17);
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
            this.labelFoundIn.Location = new System.Drawing.Point(147, 22);
            this.labelFoundIn.Name = "labelFoundIn";
            this.labelFoundIn.Size = new System.Drawing.Size(51, 13);
            this.labelFoundIn.TabIndex = 1;
            this.labelFoundIn.Text = "Found in:";
            // 
            // labelFound
            // 
            this.labelFound.AutoSize = true;
            this.labelFound.Location = new System.Drawing.Point(205, 22);
            this.labelFound.Name = "labelFound";
            this.labelFound.Size = new System.Drawing.Size(0, 13);
            this.labelFound.TabIndex = 2;
            // 
            // sketch
            // 
            this.sketchImageBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sketchImageBox.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.PanAndZoom;
            this.sketchImageBox.Location = new System.Drawing.Point(6, 330);
            this.sketchImageBox.Name = "sketch";
            this.sketchImageBox.Size = new System.Drawing.Size(357, 447);
            this.sketchImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.sketchImageBox.TabIndex = 6;
            this.sketchImageBox.TabStop = false;
            // 
            // imageBox1
            // 
            this.imageBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.imageBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageBox1.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.PanAndZoom;
            this.imageBox1.Location = new System.Drawing.Point(3, 3);
            this.imageBox1.Name = "imageBox1";
            this.imageBox1.Size = new System.Drawing.Size(271, 317);
            this.imageBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageBox1.TabIndex = 7;
            this.imageBox1.TabStop = false;
            // 
            // imageBox2
            // 
            this.imageBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.imageBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageBox2.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.PanAndZoom;
            this.imageBox2.Location = new System.Drawing.Point(280, 3);
            this.imageBox2.Name = "imageBox2";
            this.imageBox2.Size = new System.Drawing.Size(271, 317);
            this.imageBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageBox2.TabIndex = 8;
            this.imageBox2.TabStop = false;
            // 
            // imageBox3
            // 
            this.imageBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.imageBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageBox3.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.PanAndZoom;
            this.imageBox3.Location = new System.Drawing.Point(557, 3);
            this.imageBox3.Name = "imageBox3";
            this.imageBox3.Size = new System.Drawing.Size(272, 317);
            this.imageBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageBox3.TabIndex = 9;
            this.imageBox3.TabStop = false;
            // 
            // imageBox6
            // 
            this.imageBox6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.imageBox6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageBox6.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.PanAndZoom;
            this.imageBox6.Location = new System.Drawing.Point(557, 326);
            this.imageBox6.Name = "imageBox6";
            this.imageBox6.Size = new System.Drawing.Size(272, 317);
            this.imageBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageBox6.TabIndex = 12;
            this.imageBox6.TabStop = false;
            // 
            // imageBox5
            // 
            this.imageBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.imageBox5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageBox5.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.PanAndZoom;
            this.imageBox5.Location = new System.Drawing.Point(280, 326);
            this.imageBox5.Name = "imageBox5";
            this.imageBox5.Size = new System.Drawing.Size(271, 317);
            this.imageBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageBox5.TabIndex = 11;
            this.imageBox5.TabStop = false;
            // 
            // imageBox4
            // 
            this.imageBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.imageBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageBox4.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.PanAndZoom;
            this.imageBox4.Location = new System.Drawing.Point(3, 326);
            this.imageBox4.Name = "imageBox4";
            this.imageBox4.Size = new System.Drawing.Size(271, 317);
            this.imageBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageBox4.TabIndex = 10;
            this.imageBox4.TabStop = false;
            // 
            // imageBox9
            // 
            this.imageBox9.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.imageBox9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageBox9.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.PanAndZoom;
            this.imageBox9.Location = new System.Drawing.Point(557, 649);
            this.imageBox9.Name = "imageBox9";
            this.imageBox9.Size = new System.Drawing.Size(272, 317);
            this.imageBox9.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageBox9.TabIndex = 15;
            this.imageBox9.TabStop = false;
            // 
            // imageBox8
            // 
            this.imageBox8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.imageBox8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageBox8.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.PanAndZoom;
            this.imageBox8.Location = new System.Drawing.Point(280, 649);
            this.imageBox8.Name = "imageBox8";
            this.imageBox8.Size = new System.Drawing.Size(271, 317);
            this.imageBox8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageBox8.TabIndex = 14;
            this.imageBox8.TabStop = false;
            // 
            // imageBox7
            // 
            this.imageBox7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.imageBox7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageBox7.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.PanAndZoom;
            this.imageBox7.Location = new System.Drawing.Point(3, 649);
            this.imageBox7.Name = "imageBox7";
            this.imageBox7.Size = new System.Drawing.Size(271, 317);
            this.imageBox7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageBox7.TabIndex = 13;
            this.imageBox7.TabStop = false;
            // 
            // buttonNext
            // 
            this.buttonNext.Enabled = false;
            this.buttonNext.Location = new System.Drawing.Point(203, 214);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(113, 23);
            this.buttonNext.TabIndex = 16;
            this.buttonNext.Text = "Next Page";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // buttonPrevious
            // 
            this.buttonPrevious.Enabled = false;
            this.buttonPrevious.Location = new System.Drawing.Point(47, 214);
            this.buttonPrevious.Name = "buttonPrevious";
            this.buttonPrevious.Size = new System.Drawing.Size(113, 23);
            this.buttonPrevious.TabIndex = 17;
            this.buttonPrevious.Text = "Previous Page";
            this.buttonPrevious.UseVisualStyleBackColor = true;
            this.buttonPrevious.Click += new System.EventHandler(this.buttonPrevious_Click);
            // 
            // labelShowing
            // 
            this.labelShowing.AutoSize = true;
            this.labelShowing.Enabled = false;
            this.labelShowing.Location = new System.Drawing.Point(44, 165);
            this.labelShowing.Name = "labelShowing";
            this.labelShowing.Size = new System.Drawing.Size(54, 13);
            this.labelShowing.TabIndex = 18;
            this.labelShowing.Text = "Showing: ";
            // 
            // labelIndex
            // 
            this.labelIndex.AutoSize = true;
            this.labelIndex.Location = new System.Drawing.Point(105, 165);
            this.labelIndex.Name = "labelIndex";
            this.labelIndex.Size = new System.Drawing.Size(0, 13);
            this.labelIndex.TabIndex = 19;
            // 
            // labelSketchID
            // 
            this.labelSketchID.Location = new System.Drawing.Point(41, 652);
            this.labelSketchID.Name = "labelSketchID";
            this.labelSketchID.Size = new System.Drawing.Size(276, 23);
            this.labelSketchID.TabIndex = 20;
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.WorkerSupportsCancellation = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.imageBox2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.imageBox3, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.imageBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.imageBox4, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.imageBox5, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.imageBox6, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.imageBox9, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.imageBox7, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.imageBox8, 1, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(369, 6);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(832, 969);
            this.tableLayoutPanel1.TabIndex = 21;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonNext);
            this.panel1.Controls.Add(this.buttonQuery);
            this.panel1.Controls.Add(this.labelFoundIn);
            this.panel1.Controls.Add(this.labelFound);
            this.panel1.Controls.Add(this.labelIndex);
            this.panel1.Controls.Add(this.buttonPrevious);
            this.panel1.Controls.Add(this.labelShowing);
            this.panel1.Location = new System.Drawing.Point(6, 22);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(357, 283);
            this.panel1.TabIndex = 23;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(-3, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1211, 1016);
            this.tabControl1.TabIndex = 24;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.progressBar);
            this.tabPage1.Controls.Add(this.tableLayoutPanel1);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Controls.Add(this.sketchImageBox);
            this.tabPage1.Controls.Add(this.labelSketchID);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1203, 990);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Query";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 825);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(351, 23);
            this.progressBar.Step = 1;
            this.progressBar.TabIndex = 24;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.buttonStepNext);
            this.tabPage2.Controls.Add(this.buttonStepPre);
            this.tabPage2.Controls.Add(this.labelStep);
            this.tabPage2.Controls.Add(this.imageBoxStep);
            this.tabPage2.Controls.Add(this.listBox2);
            this.tabPage2.Controls.Add(this.listBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1203, 990);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Steps";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // buttonStepNext
            // 
            this.buttonStepNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStepNext.Location = new System.Drawing.Point(760, 938);
            this.buttonStepNext.Name = "buttonStepNext";
            this.buttonStepNext.Size = new System.Drawing.Size(75, 23);
            this.buttonStepNext.TabIndex = 10;
            this.buttonStepNext.Text = "Next";
            this.buttonStepNext.UseVisualStyleBackColor = true;
            this.buttonStepNext.Click += new System.EventHandler(this.buttonStepNext_Click);
            // 
            // buttonStepPre
            // 
            this.buttonStepPre.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStepPre.Location = new System.Drawing.Point(638, 938);
            this.buttonStepPre.Name = "buttonStepPre";
            this.buttonStepPre.Size = new System.Drawing.Size(75, 23);
            this.buttonStepPre.TabIndex = 9;
            this.buttonStepPre.Text = "Prev";
            this.buttonStepPre.UseVisualStyleBackColor = true;
            this.buttonStepPre.Click += new System.EventHandler(this.buttonStepPre_Click);
            // 
            // labelStep
            // 
            this.labelStep.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelStep.AutoSize = true;
            this.labelStep.Location = new System.Drawing.Point(731, 943);
            this.labelStep.Name = "labelStep";
            this.labelStep.Size = new System.Drawing.Size(13, 13);
            this.labelStep.TabIndex = 8;
            this.labelStep.Text = "0";
            // 
            // imageBoxStep
            // 
            this.imageBoxStep.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.imageBoxStep.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageBoxStep.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.PanAndZoom;
            this.imageBoxStep.Location = new System.Drawing.Point(297, 7);
            this.imageBoxStep.Name = "imageBoxStep";
            this.imageBoxStep.Size = new System.Drawing.Size(900, 914);
            this.imageBoxStep.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageBoxStep.TabIndex = 7;
            this.imageBoxStep.TabStop = false;
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(4, 489);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(287, 472);
            this.listBox2.TabIndex = 1;
            this.listBox2.SelectedIndexChanged += new System.EventHandler(this.listBox2_SelectedIndexChanged);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(4, 7);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(287, 472);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.progressBarAccuracy);
            this.tabPage3.Controls.Add(this.buttonAccuracyTest);
            this.tabPage3.Controls.Add(this.rank200label);
            this.tabPage3.Controls.Add(this.rank100label);
            this.tabPage3.Controls.Add(this.rank50label);
            this.tabPage3.Controls.Add(this.rank10label);
            this.tabPage3.Controls.Add(this.rank1label);
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Controls.Add(this.label4);
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Controls.Add(this.label2);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1203, 990);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Accuracy test";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(187, 140);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Rank 1 accuracy";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(187, 179);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Rank 10 accuracy";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(187, 218);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Rank 50 accuracy";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(187, 256);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Rank 100 accuracy";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(187, 290);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Rank 200 accuracy";
            // 
            // rank200label
            // 
            this.rank200label.AutoSize = true;
            this.rank200label.Location = new System.Drawing.Point(311, 290);
            this.rank200label.Name = "rank200label";
            this.rank200label.Size = new System.Drawing.Size(10, 13);
            this.rank200label.TabIndex = 9;
            this.rank200label.Text = "-";
            // 
            // rank100label
            // 
            this.rank100label.AutoSize = true;
            this.rank100label.Location = new System.Drawing.Point(311, 256);
            this.rank100label.Name = "rank100label";
            this.rank100label.Size = new System.Drawing.Size(10, 13);
            this.rank100label.TabIndex = 8;
            this.rank100label.Text = "-";
            // 
            // rank50label
            // 
            this.rank50label.AutoSize = true;
            this.rank50label.Location = new System.Drawing.Point(311, 218);
            this.rank50label.Name = "rank50label";
            this.rank50label.Size = new System.Drawing.Size(10, 13);
            this.rank50label.TabIndex = 7;
            this.rank50label.Text = "-";
            // 
            // rank10label
            // 
            this.rank10label.AutoSize = true;
            this.rank10label.Location = new System.Drawing.Point(311, 179);
            this.rank10label.Name = "rank10label";
            this.rank10label.Size = new System.Drawing.Size(10, 13);
            this.rank10label.TabIndex = 6;
            this.rank10label.Text = "-";
            // 
            // rank1label
            // 
            this.rank1label.AutoSize = true;
            this.rank1label.Location = new System.Drawing.Point(311, 140);
            this.rank1label.Name = "rank1label";
            this.rank1label.Size = new System.Drawing.Size(10, 13);
            this.rank1label.TabIndex = 5;
            this.rank1label.Text = "-";
            // 
            // buttonAccuracyTest
            // 
            this.buttonAccuracyTest.Location = new System.Drawing.Point(635, 142);
            this.buttonAccuracyTest.Name = "buttonAccuracyTest";
            this.buttonAccuracyTest.Size = new System.Drawing.Size(121, 50);
            this.buttonAccuracyTest.TabIndex = 10;
            this.buttonAccuracyTest.Text = "Run Test";
            this.buttonAccuracyTest.UseVisualStyleBackColor = true;
            this.buttonAccuracyTest.Click += new System.EventHandler(this.buttonAccuracyTest_Click);
            // 
            // progressBarAccuracy
            // 
            this.progressBarAccuracy.Location = new System.Drawing.Point(494, 218);
            this.progressBarAccuracy.Name = "progressBarAccuracy";
            this.progressBarAccuracy.Size = new System.Drawing.Size(394, 23);
            this.progressBarAccuracy.Step = 1;
            this.progressBarAccuracy.TabIndex = 11;
            // 
            // backgroundWorkerAccuracy
            // 
            this.backgroundWorkerAccuracy.WorkerReportsProgress = true;
            this.backgroundWorkerAccuracy.WorkerSupportsCancellation = true;
            this.backgroundWorkerAccuracy.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerAccuracy_DoWork);
            this.backgroundWorkerAccuracy.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerAccuracy_ProgressChanged);
            this.backgroundWorkerAccuracy.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerAccuracy_RunWorkerCompleted);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1203, 1002);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.sketchImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox7)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBoxStep)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonQuery;
        private System.Windows.Forms.Label labelFoundIn;
        private System.Windows.Forms.Label labelFound;
        private Emgu.CV.UI.ImageBox sketchImageBox;
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
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button buttonStepPre;
        private System.Windows.Forms.Label labelStep;
        private Emgu.CV.UI.ImageBox imageBoxStep;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button buttonStepNext;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBarAccuracy;
        private System.Windows.Forms.Button buttonAccuracyTest;
        private System.Windows.Forms.Label rank200label;
        private System.Windows.Forms.Label rank100label;
        private System.Windows.Forms.Label rank50label;
        private System.Windows.Forms.Label rank10label;
        private System.Windows.Forms.Label rank1label;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.ComponentModel.BackgroundWorker backgroundWorkerAccuracy;
    }
}

