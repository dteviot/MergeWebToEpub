namespace MergeWebToEpub
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
            this.buttonLoad = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.textBoxInituialFile = new System.Windows.Forms.TextBox();
            this.buttonBrowseInitialFile = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.textBoxToAddFileName = new System.Windows.Forms.TextBox();
            this.buttonBrowseAdd = new System.Windows.Forms.Button();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(12, 39);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(75, 23);
            this.buttonLoad.TabIndex = 0;
            this.buttonLoad.Text = "Load";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // textBoxInituialFile
            // 
            this.textBoxInituialFile.Location = new System.Drawing.Point(94, 41);
            this.textBoxInituialFile.Name = "textBoxInituialFile";
            this.textBoxInituialFile.Size = new System.Drawing.Size(574, 20);
            this.textBoxInituialFile.TabIndex = 2;
            // 
            // buttonBrowseInitialFile
            // 
            this.buttonBrowseInitialFile.Location = new System.Drawing.Point(675, 39);
            this.buttonBrowseInitialFile.Name = "buttonBrowseInitialFile";
            this.buttonBrowseInitialFile.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowseInitialFile.TabIndex = 3;
            this.buttonBrowseInitialFile.Text = "Browse...";
            this.buttonBrowseInitialFile.UseVisualStyleBackColor = true;
            this.buttonBrowseInitialFile.Click += new System.EventHandler(this.buttonBrowseInitialFile_Click);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(13, 69);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonAdd.TabIndex = 4;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // textBoxToAddFileName
            // 
            this.textBoxToAddFileName.Location = new System.Drawing.Point(94, 69);
            this.textBoxToAddFileName.Name = "textBoxToAddFileName";
            this.textBoxToAddFileName.Size = new System.Drawing.Size(574, 20);
            this.textBoxToAddFileName.TabIndex = 5;
            // 
            // buttonBrowseAdd
            // 
            this.buttonBrowseAdd.Location = new System.Drawing.Point(675, 69);
            this.buttonBrowseAdd.Name = "buttonBrowseAdd";
            this.buttonBrowseAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowseAdd.TabIndex = 6;
            this.buttonBrowseAdd.Text = "Browse...";
            this.buttonBrowseAdd.UseVisualStyleBackColor = true;
            this.buttonBrowseAdd.Click += new System.EventHandler(this.buttonBrowseAdd_Click);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonBrowseAdd);
            this.Controls.Add(this.textBoxToAddFileName);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.buttonBrowseInitialFile);
            this.Controls.Add(this.textBoxInituialFile);
            this.Controls.Add(this.buttonLoad);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.TextBox textBoxInituialFile;
        private System.Windows.Forms.Button buttonBrowseInitialFile;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.TextBox textBoxToAddFileName;
        private System.Windows.Forms.Button buttonBrowseAdd;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
    }
}

