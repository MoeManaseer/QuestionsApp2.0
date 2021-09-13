namespace QuestionsApp
{
    partial class LandingForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LandingForm));
            this.allQuestionsGrid = new System.Windows.Forms.DataGridView();
            this.refreshBtn = new System.Windows.Forms.Button();
            this.removeBtn = new System.Windows.Forms.Button();
            this.editBtn = new System.Windows.Forms.Button();
            this.addBtn = new System.Windows.Forms.Button();
            this.outputLbl = new System.Windows.Forms.Label();
            this.settingsBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.settingsPanel = new System.Windows.Forms.Panel();
            this.refreshButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.allQuestionsGrid)).BeginInit();
            this.settingsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // allQuestionsGrid
            // 
            this.allQuestionsGrid.AllowUserToAddRows = false;
            this.allQuestionsGrid.AllowUserToDeleteRows = false;
            this.allQuestionsGrid.AllowUserToResizeColumns = false;
            this.allQuestionsGrid.AllowUserToResizeRows = false;
            this.allQuestionsGrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.allQuestionsGrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.allQuestionsGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.allQuestionsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.allQuestionsGrid.GridColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.allQuestionsGrid, "allQuestionsGrid");
            this.allQuestionsGrid.MultiSelect = false;
            this.allQuestionsGrid.Name = "allQuestionsGrid";
            this.allQuestionsGrid.ReadOnly = true;
            this.allQuestionsGrid.RowHeadersVisible = false;
            this.allQuestionsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.allQuestionsGrid.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.allQuestionsGrid_ColumnHeaderMouseClick);
            this.allQuestionsGrid.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.allQuestionsGrid_RowEnter);
            // 
            // refreshBtn
            // 
            resources.ApplyResources(this.refreshBtn, "refreshBtn");
            this.refreshBtn.Name = "refreshBtn";
            this.refreshBtn.UseVisualStyleBackColor = true;
            this.refreshBtn.Click += new System.EventHandler(this.exitBtn_Click);
            // 
            // removeBtn
            // 
            resources.ApplyResources(this.removeBtn, "removeBtn");
            this.removeBtn.Name = "removeBtn";
            this.removeBtn.UseVisualStyleBackColor = true;
            this.removeBtn.Click += new System.EventHandler(this.removeBtn_Click);
            // 
            // editBtn
            // 
            resources.ApplyResources(this.editBtn, "editBtn");
            this.editBtn.Name = "editBtn";
            this.editBtn.UseVisualStyleBackColor = true;
            this.editBtn.Click += new System.EventHandler(this.editBtn_Click);
            // 
            // addBtn
            // 
            resources.ApplyResources(this.addBtn, "addBtn");
            this.addBtn.Name = "addBtn";
            this.addBtn.UseVisualStyleBackColor = true;
            this.addBtn.Click += new System.EventHandler(this.addBtn_Click);
            // 
            // outputLbl
            // 
            resources.ApplyResources(this.outputLbl, "outputLbl");
            this.outputLbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.outputLbl.Name = "outputLbl";
            // 
            // settingsBtn
            // 
            resources.ApplyResources(this.settingsBtn, "settingsBtn");
            this.settingsBtn.Name = "settingsBtn";
            this.settingsBtn.UseVisualStyleBackColor = true;
            this.settingsBtn.Click += new System.EventHandler(this.settingsBtn_Click);
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // settingsPanel
            // 
            this.settingsPanel.Controls.Add(this.settingsBtn);
            this.settingsPanel.Controls.Add(this.refreshBtn);
            resources.ApplyResources(this.settingsPanel, "settingsPanel");
            this.settingsPanel.Name = "settingsPanel";
            // 
            // refreshButton
            // 
            resources.ApplyResources(this.refreshButton, "refreshButton");
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // LandingForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.settingsPanel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.outputLbl);
            this.Controls.Add(this.addBtn);
            this.Controls.Add(this.editBtn);
            this.Controls.Add(this.removeBtn);
            this.Controls.Add(this.allQuestionsGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "LandingForm";
            this.Load += new System.EventHandler(this.LandingFrom_Load);
            ((System.ComponentModel.ISupportInitialize)(this.allQuestionsGrid)).EndInit();
            this.settingsPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView allQuestionsGrid;
        private System.Windows.Forms.Button refreshBtn;
        private System.Windows.Forms.Button removeBtn;
        private System.Windows.Forms.Button editBtn;
        private System.Windows.Forms.Button addBtn;
        private System.Windows.Forms.Label outputLbl;
        private System.Windows.Forms.Button settingsBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel settingsPanel;
        private System.Windows.Forms.Button refreshButton;
    }
}

