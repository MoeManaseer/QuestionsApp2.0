namespace QuestionsApp
{
    partial class QuestionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuestionForm));
            this.input_Text = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.containerStar = new System.Windows.Forms.Panel();
            this.input_NumberOfStar = new System.Windows.Forms.NumericUpDown();
            this.textbox4 = new System.Windows.Forms.Label();
            this.containerSmiley = new System.Windows.Forms.Panel();
            this.input_NumberOfSmiley = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.input_Order = new System.Windows.Forms.NumericUpDown();
            this.containerSlider = new System.Windows.Forms.Panel();
            this.input_EndValueCaption = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.input_StartValueCaption = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.input_EndValue = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.input_StartValue = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.questionTypeCombo = new System.Windows.Forms.ComboBox();
            this.exitButton = new System.Windows.Forms.Button();
            this.controlBtn = new System.Windows.Forms.Button();
            this.headerLbl = new System.Windows.Forms.Label();
            this.containerQuestion = new System.Windows.Forms.Panel();
            this.containerStar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.input_NumberOfStar)).BeginInit();
            this.containerSmiley.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.input_NumberOfSmiley)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.input_Order)).BeginInit();
            this.containerSlider.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.input_EndValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.input_StartValue)).BeginInit();
            this.containerQuestion.SuspendLayout();
            this.SuspendLayout();
            // 
            // input_Text
            // 
            resources.ApplyResources(this.input_Text, "input_Text");
            this.input_Text.Name = "input_Text";
            this.input_Text.Tag = "Question text";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // containerStar
            // 
            resources.ApplyResources(this.containerStar, "containerStar");
            this.containerStar.Controls.Add(this.input_NumberOfStar);
            this.containerStar.Controls.Add(this.textbox4);
            this.containerStar.Name = "containerStar";
            // 
            // input_NumberOfStar
            // 
            resources.ApplyResources(this.input_NumberOfStar, "input_NumberOfStar");
            this.input_NumberOfStar.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.input_NumberOfStar.Name = "input_NumberOfStar";
            this.input_NumberOfStar.Tag = "Number of stars";
            // 
            // textbox4
            // 
            resources.ApplyResources(this.textbox4, "textbox4");
            this.textbox4.Name = "textbox4";
            // 
            // containerSmiley
            // 
            resources.ApplyResources(this.containerSmiley, "containerSmiley");
            this.containerSmiley.Controls.Add(this.input_NumberOfSmiley);
            this.containerSmiley.Controls.Add(this.label3);
            this.containerSmiley.Name = "containerSmiley";
            // 
            // input_NumberOfSmiley
            // 
            resources.ApplyResources(this.input_NumberOfSmiley, "input_NumberOfSmiley");
            this.input_NumberOfSmiley.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.input_NumberOfSmiley.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.input_NumberOfSmiley.Name = "input_NumberOfSmiley";
            this.input_NumberOfSmiley.Tag = "Number of smiley";
            this.input_NumberOfSmiley.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // input_Order
            // 
            resources.ApplyResources(this.input_Order, "input_Order");
            this.input_Order.Name = "input_Order";
            this.input_Order.Tag = "Question order";
            // 
            // containerSlider
            // 
            resources.ApplyResources(this.containerSlider, "containerSlider");
            this.containerSlider.Controls.Add(this.input_EndValueCaption);
            this.containerSlider.Controls.Add(this.label7);
            this.containerSlider.Controls.Add(this.input_StartValueCaption);
            this.containerSlider.Controls.Add(this.label6);
            this.containerSlider.Controls.Add(this.input_EndValue);
            this.containerSlider.Controls.Add(this.label5);
            this.containerSlider.Controls.Add(this.input_StartValue);
            this.containerSlider.Controls.Add(this.label4);
            this.containerSlider.Name = "containerSlider";
            // 
            // input_EndValueCaption
            // 
            resources.ApplyResources(this.input_EndValueCaption, "input_EndValueCaption");
            this.input_EndValueCaption.Name = "input_EndValueCaption";
            this.input_EndValueCaption.Tag = "End value caption";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // input_StartValueCaption
            // 
            resources.ApplyResources(this.input_StartValueCaption, "input_StartValueCaption");
            this.input_StartValueCaption.Name = "input_StartValueCaption";
            this.input_StartValueCaption.Tag = "End value caption";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // input_EndValue
            // 
            resources.ApplyResources(this.input_EndValue, "input_EndValue");
            this.input_EndValue.Name = "input_EndValue";
            this.input_EndValue.Tag = "End value";
            this.input_EndValue.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.input_EndValue.ValueChanged += new System.EventHandler(this.input_EndStartValues_ValueChanged);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // input_StartValue
            // 
            resources.ApplyResources(this.input_StartValue, "input_StartValue");
            this.input_StartValue.Name = "input_StartValue";
            this.input_StartValue.Tag = "Start value";
            this.input_StartValue.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.input_StartValue.ValueChanged += new System.EventHandler(this.input_EndStartValues_ValueChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // questionTypeCombo
            // 
            resources.ApplyResources(this.questionTypeCombo, "questionTypeCombo");
            this.questionTypeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.questionTypeCombo.FormattingEnabled = true;
            this.questionTypeCombo.Name = "questionTypeCombo";
            this.questionTypeCombo.SelectedValueChanged += new System.EventHandler(this.questionTypeCombo_SelectedValueChanged);
            // 
            // exitButton
            // 
            resources.ApplyResources(this.exitButton, "exitButton");
            this.exitButton.Name = "exitButton";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // controlBtn
            // 
            resources.ApplyResources(this.controlBtn, "controlBtn");
            this.controlBtn.Name = "controlBtn";
            this.controlBtn.UseVisualStyleBackColor = true;
            this.controlBtn.Click += new System.EventHandler(this.controlBtn_Click);
            // 
            // headerLbl
            // 
            resources.ApplyResources(this.headerLbl, "headerLbl");
            this.headerLbl.Name = "headerLbl";
            // 
            // containerQuestion
            // 
            resources.ApplyResources(this.containerQuestion, "containerQuestion");
            this.containerQuestion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.containerQuestion.Controls.Add(this.label1);
            this.containerQuestion.Controls.Add(this.input_Text);
            this.containerQuestion.Controls.Add(this.label2);
            this.containerQuestion.Controls.Add(this.input_Order);
            this.containerQuestion.Controls.Add(this.containerStar);
            this.containerQuestion.Controls.Add(this.containerSlider);
            this.containerQuestion.Controls.Add(this.containerSmiley);
            this.containerQuestion.Name = "containerQuestion";
            // 
            // QuestionForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.containerQuestion);
            this.Controls.Add(this.headerLbl);
            this.Controls.Add(this.questionTypeCombo);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.controlBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "QuestionForm";
            this.Load += new System.EventHandler(this.QuestionForm_Load);
            this.containerStar.ResumeLayout(false);
            this.containerStar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.input_NumberOfStar)).EndInit();
            this.containerSmiley.ResumeLayout(false);
            this.containerSmiley.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.input_NumberOfSmiley)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.input_Order)).EndInit();
            this.containerSlider.ResumeLayout(false);
            this.containerSlider.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.input_EndValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.input_StartValue)).EndInit();
            this.containerQuestion.ResumeLayout(false);
            this.containerQuestion.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox input_Text;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel containerStar;
        private System.Windows.Forms.Label textbox4;
        private System.Windows.Forms.Panel containerSmiley;
        private System.Windows.Forms.NumericUpDown input_NumberOfSmiley;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown input_NumberOfStar;
        private System.Windows.Forms.NumericUpDown input_Order;
        private System.Windows.Forms.Panel containerSlider;
        private System.Windows.Forms.TextBox input_EndValueCaption;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox input_StartValueCaption;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown input_EndValue;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown input_StartValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox questionTypeCombo;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Button controlBtn;
        private System.Windows.Forms.Label headerLbl;
        private System.Windows.Forms.Panel containerQuestion;
    }
}